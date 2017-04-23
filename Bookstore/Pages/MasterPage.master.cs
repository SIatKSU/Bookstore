using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bookstore;

public partial class MasterPage : System.Web.UI.MasterPage
{
    Cart cart;

    protected void Page_Load(object sender, EventArgs e)
    {

        // SCAN BOOKS DATA FILE AND ADD ELEMENTS INTO a (static) MULTI-DIM. ARRAY

        //string csvText = @"C:\Users\SWE Group 4\Documents\Visual Studio 2017\WebSites\Bookstore\books.csv";


        StaticData.readFile(); //this will be moved to a more appropriate place later
       
        int numOfItems = 0;

        //just in case cart is null, create it.
        if (Session["cart"] == null)
        {
            Session["cart"] = new Cart();
        }
        cart = (Cart)Session["cart"];

        numOfItems = getNumOfItems();
            
        CartQuantityText.Text = numOfItems.ToString() + " Items";

        string subTotalString = String.Format("Subtotal: {0:C}", cart.subTotal);
        SubtotalText.Text = subTotalString;
    }

    private int getNumOfItems()
    {
        int numOfItems = 0;

        for (int i = 0; i < cart.cartList.Count; i++)
        {
            numOfItems += cart.cartList[i].quantity;
        }

        return numOfItems;
    }
}