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
        }
        else
        {
            CheckoutBtn.Visible = true;
            ErrorLabel.Text = "&nbsp;";
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

            dr["Format"] = LineItem.FormatIntToString(cart.cartList[i].format);

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
        int result;
        bool isError = false;

        //Get the current row in grid
        GridViewRow currentRow = (GridViewRow)(sender as TextBox).Parent.Parent;

        Label rowNumberLabel = (Label)currentRow.Cells[1].FindControl("RowNumber");
        int rowNumber = Convert.ToInt32(rowNumberLabel.Text);

        int format = LineItem.FormatStringToInt(currentRow.Cells[2].Text);

        int newQuantity;
        string quantityStr = (sender as TextBox).Text;
        bool hasQuantity = int.TryParse(quantityStr, out newQuantity);
        if (!hasQuantity)
        {
            newQuantity = 0;
        }
    
        LineItem currLine = cart.GetLineItem(rowNumber, format);

        if (newQuantity != currLine.quantity)
        {
            if (newQuantity == 0)
            {
                cart.DeleteLine(currLine);
            }
            else if (newQuantity > currLine.quantity)
            {
                result = cart.AddToCart(rowNumber, format, newQuantity - currLine.quantity);
                if (result == -1) //ebook 
                {
                    //ErrorLabel.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Error: you can only order 1 copy of an eBook.";
                    //ErrorLabel.Visible = true;
                }
                else if (result == -2) //trying to add more items than exist in stock
                {
                    //CheckoutBtn.Visible = false;
                    ErrorLabel.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Not enough items in stock.";

                    isError = true;
                }
            }
            else
            {
                cart.RemoveFromCart(rowNumber, format, currLine.quantity - newQuantity);
            }


            SetTotals();
            SetGridTable();

            //refresh cart icon
            ((MasterPage)this.Master).RefreshCartIcon();

            if (!isError)
                CheckIsCartEmpty();
        }

    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // If page not Refreshed
        if (Session["update"].ToString() == ViewState["update"].ToString())
        {
            if ((e.CommandName == "DeleteThisRow") || (e.CommandName == "Decrement") || (e.CommandName == "Increment"))
            {
                int result;
                Boolean isError = false;

                // Retrieve the row index stored in the 
                // CommandArgument property.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                TableRow gridRow = GridView1.Rows[index];

                Label rowNumberLabel = (Label)gridRow.Cells[1].FindControl("RowNumber");
                int rowNumber = Convert.ToInt32(rowNumberLabel.Text);

                int format = LineItem.FormatStringToInt(gridRow.Cells[2].Text);

                LineItem currLine = cart.GetLineItem(rowNumber, format);

                if (e.CommandName == "DeleteThisRow")
                {
                    cart.DeleteLine(currLine);
                }
                else if (e.CommandName == "Decrement")
                {
                    if (currLine.quantity == 1)
                    {
                        cart.DeleteLine(currLine);
                    }
                    else
                    {
                        cart.RemoveFromCart(rowNumber, format, 1);
                    }
                }
                else //if (e.CommandName == "Increment")
                {
                    result = cart.AddToCart(rowNumber, format, 1);
                    if (result == -1) //ebook 
                    {
                        //ErrorLabel.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Error: you can only order 1 copy of an eBook.";
                        //ErrorLabel.Visible = true;
                    }
                    else if (result == -2) //trying to add more items than exist in stock
                    {
                        //CheckoutBtn.Visible = false;
                        ErrorLabel.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Not enough items in stock.";

                        isError = true;
                    }

                }


                SetTotals();
                SetGridTable();

                //refresh cart icon
                ((MasterPage)this.Master).RefreshCartIcon();

                if (!isError)
                    CheckIsCartEmpty();

                // After the event/ method, again update the session 
                Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());
            }
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

    protected void gridDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if (((Label)e.Row.FindControl("lblScenario")).Text == "Actual")

            if (e.Row.Cells[2].Text == "eBook")
            {
                //Button minusButton = (Button)e.Row.Cells[4].Controls[0];
                //minusButton.Visible = false;
                ((Button)e.Row.Cells[4].FindControl("DecrementBtn")).Visible = false;
                ((Button)e.Row.Cells[4].FindControl("IncrementBtn")).Visible = false;
                ((TextBox)e.Row.Cells[4].FindControl("QuantityTextBox")).Visible = false;
                ((Label)e.Row.Cells[4].FindControl("QuantityInStock")).Visible = false;
                ((Label)e.Row.Cells[4].FindControl("eBookLabel")).Visible = true;
            }
        }
    }

}