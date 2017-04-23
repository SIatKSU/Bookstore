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

        for (int i = 0; i < cart.cartList.Count; i++)
        {

            labText = labText + "<br>" + "row: " + cart.cartList[i].rowNumber + "  format: " + cart.cartList[i].format + "  quantity: " + cart.cartList[i].quantity;
        }
        Debug.WriteLine(cart.cartList.Count);
        Label1.Text = labText;

        //calculate total
        cart.calcTotal();
        

    }
}