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
        if (Session["cart"] != null)
        {
            List<LineItem> cartList = (List<LineItem>)Session["cart"];
            for (int i = 0; i < cartList.Count; i++)
            {

                labText = labText + "<br>" + "row: " + cartList[i].rowNumber + "  format: " + cartList[i].format + "  quantity: " + cartList[i].quantity;
            }
            Debug.WriteLine(cartList.Count);
            Label1.Text = labText;
        }
    }
}