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
        CustomerInfo customerinfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            //here is how you access the customer info.
            customerinfo = (CustomerInfo)Session["CustomerInfo"];
            //filling out billing, street address, and payment method info for receipt           
            FullNameLabelB.Text = customerinfo.FullName;
            FullNameLabel2.Text = customerinfo.FullName;

            PhoneNumberLabelB.Text = customerinfo.PhoneNumber;
            PhoneNumberLabel5.Text = customerinfo.PhoneNumber;

            EmailAdressLabelB.Text = customerinfo.Email;
            EmailLabel6.Text = customerinfo.Email;

            addressLabelB.Text = customerinfo.BillingStreet;
            AddressLabel3.Text = customerinfo.ShippingStreet;


            String BState = customerinfo.BillingState;
            String BZip = customerinfo.BillingZip;
            BState = " " + BState.Trim();
            BZip = ", " + BZip.Trim();


            citystateBLabel.Text = string.Concat(string.Concat(customerinfo.BillingCity, BState), BZip);
            citystateLabel4.Text = string.Concat(string.Concat(customerinfo.ShippingCity, customerinfo.ShippingState), customerinfo.ShippingZip);
            String lastfourdigits;
            if (customerinfo.PaymentMethod == "MasterCard"||customerinfo.PaymentMethod == "Visa") { 
                lastfourdigits = customerinfo.CardNumber.Substring(Math.Max(0, customerinfo.CardNumber.Length - 4));
                lastfourdigits = " " + lastfourdigits.Trim();
                PaymentMethodLabel1.Text = string.Concat(customerinfo.PaymentMethod, lastfourdigits);  
                            
                
            }
            else if(customerinfo.PaymentMethod == "paypal") {
                PaymentMethodLabel1.Text = customerinfo.PayPalEmail;
            }
            else
            {
                PaymentMethodLabel1.Text = customerinfo.KSUlogin;
            }
            


        }
       
    }
}