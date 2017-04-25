using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bookstore.Pages
{
    public partial class Receipt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //here is how you access the customer info.
            CustomerInfo customerInfo = (CustomerInfo)Session["CustomerInfo"];

            //for example:
            //labelXYZ = customerInfo.FullName;

        }
    }
}