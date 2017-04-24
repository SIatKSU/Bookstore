using Bookstore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

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

        if (cart.GetNumOfItems()==0)
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

        //calculate total
        cart.calcTotal();

        SetGridTable();
        //SetTotalsTable();  //better to do this as labels on a panel.
        SetTotals();


    }

    private void SetTotals()
    {
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
        dt.Columns.Add(new DataColumn("Format"));
        dt.Columns.Add(new DataColumn("Quantity"));

        //add elements to DataRow
        for (int i = 0; i < cart.cartList.Count; i++)
        {
            dr = dt.NewRow();

            string isbn = StaticData.getMatrixValue(cart.cartList[i].rowNumber, StaticData.ISBN);

            //dr["CoverURL"] = @"/Images/" + isbn + ".jpg";
            dr["ISBN"] = isbn;
            dr["Title"] = StaticData.getMatrixValue(cart.cartList[i].rowNumber, StaticData.TITLE);
            dr["TitleURL"] = @"/Pages/Details.aspx?isbn=" + isbn;

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

            dr["Quantity"] = cart.cartList[i].quantity;

            dt.Rows.Add(dr);
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    private void SetTotalsTable()
    {
        //create DataTable & DataRow
        DataTable dt = new DataTable();
        DataRow dr = null;

        //Adds columns to DataTable
        //dt.Columns.Add(new DataColumn("Blank1"));
        dt.Columns.Add(new DataColumn("BlankSpace"));
        dt.Columns.Add(new DataColumn("Label"));
        dt.Columns.Add(new DataColumn("Amount"));

        //add elements to DataRow
        dr = dt.NewRow();
        dr["Label"] = "Subtotal:";
        dr["Amount"] = cart.subTotal;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["Label"] = "7% tax:";
        dr["Amount"] = cart.tax;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["Label"] = "Shipping:";
        dr["Amount"] = cart.shipping;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["Label"] = "Total:";
        dr["Amount"] = cart.total;
        dt.Rows.Add(dr);

        GridView2.DataSource = dt;
        GridView2.DataBind();
    }



    protected void CheckoutButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("Checkout.aspx");
    }

    protected void ContinueShoppingBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("Index.aspx");
    }
}