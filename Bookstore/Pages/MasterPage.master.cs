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
    List<LineItem> cartList;

    protected void Page_Load(object sender, EventArgs e)
    {

        // SCAN BOOKS DATA FILE AND ADD ELEMENTS INTO a (static) MULTI-DIM. ARRAY

        //string csvText = @"C:\Users\SWE Group 4\Documents\Visual Studio 2017\WebSites\Bookstore\books.csv";


        StaticData.readFile(); //this will be moved to a more appropriate place later


        int numOfItems = 0;
        double subtotal = 0.00;

        if (Session["cart"] != null)
        {
            cartList = (List<LineItem>)Session["cart"];
            numOfItems = getNumOfItems();
            subtotal = getSubtotal();
        }

        CartQuantityText.Text = numOfItems.ToString() + " Items";
        SubtotalText.Text = "Subtotal: $" + subtotal.ToString();
    }

    private int getNumOfItems()
    {
        int numOfItems = 0;

        for (int i = 0; i < cartList.Count; i++)
        {
            numOfItems += cartList[i].quantity;
        }

        return numOfItems;
    }

    private double getSubtotal()
    {
        double subtotal = 0.00;

        for (int i = 0; i < cartList.Count; i++)
        {
            subtotal += (Convert.ToDouble(StaticData.getMatrixValue(cartList[i].rowNumber, (cartList[i].format + 13))) * cartList[i].quantity);
        }

        return subtotal;
    }

}