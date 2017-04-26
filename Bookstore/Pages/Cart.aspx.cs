using Bookstore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Windows;

public partial class Pages_Cart : System.Web.UI.Page
{
    private Cart cart;

    protected void Page_Load(object sender, EventArgs e)
    {                
        //just in case cart is null, create it.
        if (Session["cart"] == null)
        {
            Session["cart"] = new Cart();
        }
        cart = (Cart)Session["cart"];

        CheckIsCartEmpty();

        if (!IsPostBack) // If page loads for first time
        {
            // Assign the Session["update"] with unique value ------need to handle user pressing F5
            Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());

            SetGridTable();

        }

        SetTotals();
    }

    //need to handle user pressing F5
    protected override void OnPreRender(EventArgs e)
    {
        ViewState["update"] = Session["update"];
    }

    private void CheckIsCartEmpty()
    {
        if (cart.GetNumOfItems() == 0)
        {
            CheckoutBtn.Visible = false;
            ErrorLabel.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Cart is empty.";
            ErrorLabel.Visible = true;
        }
        else
        {
            CheckoutBtn.Visible = true;
            ErrorLabel.Visible = false;
        }
    }

    private void SetTotals()
    {
        //calculate Tax, shipping and total
        cart.calcTaxShippingAndTotal();

        SubtotalLabel.Text = String.Format("{0:C}", cart.subTotal);
        TaxLabel.Text = String.Format("{0:C}", cart.tax);
        ShippingLabel.Text = String.Format("{0:C}", cart.shipping);
        TotalLabel.Text = String.Format("{0:C}", cart.total);
    }


    private void SetGridTable()
    {
        //create DataTable & DataRow
        DataTable dt = new DataTable();
        DataRow dr = null;

        //Adds columns to DataTable
        dt.Columns.Add(new DataColumn("ISBN"));
        dt.Columns.Add(new DataColumn("Title"));
        dt.Columns.Add(new DataColumn("TitleURL"));
        dt.Columns.Add(new DataColumn("Author"));
        dt.Columns.Add(new DataColumn("Format"));
        dt.Columns.Add(new DataColumn("Price"));
        dt.Columns.Add(new DataColumn("Quantity"));
        dt.Columns.Add(new DataColumn("Total"));
        dt.Columns.Add(new DataColumn("RowNumber"));
        dt.Columns.Add(new DataColumn("InStock"));

        //add elements to DataRow
        for (int i = 0; i < cart.cartList.Count; i++)
        {
            dr = dt.NewRow();

            string isbn = StaticData.getMatrixValue(cart.cartList[i].rowNumber, StaticData.ISBN);

            //dr["CoverURL"] = @"/Images/" + isbn + ".jpg";
            dr["ISBN"] = isbn;
            dr["Title"] = StaticData.getMatrixValue(cart.cartList[i].rowNumber, StaticData.TITLE);
            dr["TitleURL"] = @"/Pages/Details.aspx?isbn=" + isbn;

            dr["Author"] = StaticData.getMatrixValue(cart.cartList[i].rowNumber, StaticData.AUTHOR);
            
            switch (cart.cartList[i].format)
            {
                case LineItem.NEW:
                    dr["Format"] = "New";
                    break;
                case LineItem.USED:
                    dr["Format"] = "Used";
                    break;
                case LineItem.RENTAL:
                    dr["Format"] = "Rental";
                    break;
                case LineItem.EBOOK:
                    dr["Format"] = "eBook";
                    break;
                default:
                    dr["Format"] = "invalid";
                    break;
            }

            dr["Price"] = String.Format("{0:C}", cart.cartList[i].price);
            dr["Quantity"] = cart.cartList[i].quantity;

            decimal lineTotal = cart.cartList[i].price * cart.cartList[i].quantity;
            dr["Total"] = String.Format("{0:C}", lineTotal);

            dr["RowNumber"] = cart.cartList[i].rowNumber;

            if (cart.cartList[i].format != LineItem.EBOOK) {
                dr["InStock"] = StaticData.getMatrixValue(cart.cartList[i].rowNumber, StaticData.QUANTITY_NEW + cart.cartList[i].format) + " in stock";
            }
            else
            {
                dr["InStock"] = "";
            }

            dt.Rows.Add(dr);
        }


        GridView1.DataSource = dt;
        GridView1.DataBind();
        //Session["CartSource"] = dt;

    }
    


    protected void CheckoutButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("Checkout.aspx");
    }

    protected void ContinueShoppingBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("Index.aspx");
    }


    protected void TextChangedEvent(object sender, EventArgs e)
    {

        /*
        //Get the current row in grid
        GridViewRow currentRow = (GridViewRow)(sender as TextBox).Parent.Parent;

        Label rowNumberLabel = (Label)currentRow.Cells[1].FindControl("RowNumber");
        int rowNumber = Convert.ToInt32(rowNumberLabel.Text);

        int format = LineItem.FormatStringToInt(currentRow.Cells[2].Text);

        int newQuantity;
        string quantityStr = (sender as TextBox).Text;
        bool result = int.TryParse(quantityStr, out newQuantity);
        if (!result)
        {
            newQuantity = 0;
        }
    
        LineItem currLine = cart.GetLineItem(rowNumber, format);

        if (newQuantity != currLine.quantity)
        {
            if (newQuantity == 0)
            {
                cart.DeleteLine(currLine);
                GridView1.DeleteRow(currentRow.RowIndex);
            }
            else
            {
                if (newQuantity > currLine.quantity)
                {
                    cart.AddToCart(rowNumber, format, newQuantity - currLine.quantity);
                }
                else
                {
                    cart.RemoveFromCart(rowNumber, format, currLine.quantity - newQuantity);
                }

            }

            SetTotals();
            CheckIsCartEmpty();

            //refresh cart icon
            ((MasterPage)this.Master).RefreshCartIcon();
        }

        */
    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // If page not Refreshed
        if (Session["update"].ToString() == ViewState["update"].ToString())
        {
            if ((e.CommandName == "DeleteThisRow") || (e.CommandName == "Decrement") || (e.CommandName == "Increment"))
            {
                // Retrieve the row index stored in the 
                // CommandArgument property.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                TableRow gridRow = GridView1.Rows[index];

                Label rowNumberLabel = (Label)gridRow.Cells[1].FindControl("RowNumber");
                int rowNumber = Convert.ToInt32(rowNumberLabel.Text);

                int format = LineItem.FormatStringToInt(gridRow.Cells[2].Text);

                //DataTable dt = (DataTable)GridView1.DataSource ?? (DataTable)Session["CartSource"];
                //Debug.WriteLine(dt.Rows[index]["RowNumber"]);
                //Debug.WriteLine(dt.Rows[index]["Format"]);

                //int rowNumber = Convert.ToInt32(dt.Rows[index]["RowNumber"]);
                //string formatStr = dt.Rows[index]["Format"].ToString();
                //int formatNum = LineItem.FormatStringToInt(formatStr);

                LineItem currLine = cart.GetLineItem(rowNumber, format);

                bool deletingRow = false;

                if (e.CommandName == "DeleteThisRow")
                {
                    deletingRow = true;
                }
                else if (e.CommandName == "Decrement")
                {
                    if (currLine.quantity == 1)
                    {
                        deletingRow = true;
                    }
                    else
                    {
                        cart.RemoveFromCart(rowNumber, format, 1);
                    }
                }
                else //if (e.CommandName == "Increment")
                {
                    cart.AddToCart(rowNumber, format, 1);
                }

                if (deletingRow)
                {
                    cart.DeleteLine(currLine);
                    GridView1.DeleteRow(index);
                    //dt.Rows[index].Delete();
                }
                else
                {
                    //dt.Rows[index]["Quantity"] = currLine.quantity;
                    TextBox quantityTextBox = (TextBox)gridRow.Cells[4].FindControl("QuantityTextBox");
                    quantityTextBox.Text = currLine.quantity.ToString();

                    decimal lineTotal = currLine.price * currLine.quantity;
                    gridRow.Cells[5].Text = String.Format("{0:C}", lineTotal);
                    //dt.Rows[index]["Total"] = String.Format("{0:C}", lineTotal);


                    /*
                    if (currLine.format != LineItem.EBOOK)
                    {
                        dt.Rows[index]["InStock"] = StaticData.getMatrixValue(currLine.rowNumber, StaticData.QUANTITY_NEW + currLine.format) + " in stock";
                    }
                    else
                    {
                        dt.Rows[index]["InStock"] = "";
                    }
                    */
                }

                SetTotals();
                CheckIsCartEmpty();

                //refresh cart icon
                ((MasterPage)this.Master).RefreshCartIcon();
                
                //GridView1.DataSource = dt;
                //GridView1.DataBind(); //refresh the gridview. 
            }

            // After the event/ method, again update the session 
            Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());
        }
        else
        {   //handle page being refreshed; without this line, if the user presses F5, it could show the previous condition of the datagrid (for example before a deletion)
            SetGridTable();
        }
    }


    //necessary to do this because we are using the GridView.DeleteRow command.
    //http://stackoverflow.com/questions/14301815/the-gridview-pendingrecordsgridview-fired-event-rowdeleting-which-wasnt-handl
    public void PendingRecordsGridview_RowDeleting(Object sender, GridViewDeleteEventArgs e)
    {

    }
}