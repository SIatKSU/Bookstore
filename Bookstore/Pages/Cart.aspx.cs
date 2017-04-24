using Bookstore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Cart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string labText = "";

        //just in case cart is null, create it.
        if (Session["cart"] == null)
        {
            Session["cart"] = new Cart();
        }
        Cart cart = (Cart)Session["cart"];

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

        for (int i = 0; i < cart.cartList.Count; i++)
        {

            labText = labText + "<br>" + "row: " + cart.cartList[i].rowNumber + "  format: " + cart.cartList[i].format + "  quantity: " + cart.cartList[i].quantity;
        }
        Debug.WriteLine(cart.cartList.Count);
        //Label1.Text = labText;

        //calculate total
        cart.calcTotal();
        

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