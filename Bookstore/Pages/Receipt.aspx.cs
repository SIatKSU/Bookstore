using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

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
            String SState = customerinfo.ShippingState;
            String SZip = customerinfo.ShippingZip;
            SState = " " + SState.Trim();
            SZip = ", " + SZip.Trim();


            citystateBLabel.Text = string.Concat(string.Concat(customerinfo.BillingCity, BState), BZip);
            citystateLabel4.Text = string.Concat(string.Concat(customerinfo.ShippingCity, SState), SZip);
            String lastfourdigits;
            if (customerinfo.PaymentMethod == "MasterCard"||customerinfo.PaymentMethod == "Visa") { 
                lastfourdigits = customerinfo.CardNumber.Substring(Math.Max(0, customerinfo.CardNumber.Length - 4));
                lastfourdigits = " " + lastfourdigits.Trim();
                PaymentMethodLabel1.Text = string.Concat(customerinfo.PaymentMethod, lastfourdigits);  
                            
                
            }
            else if(customerinfo.PaymentMethod == "paypal") {
                String paypal = "PayPal Email: ";
                String paypalemail = customerinfo.PayPalEmail;
                PaymentMethodLabel1.Text = string.Concat(paypal, paypalemail);
            }
            else
            {
                String ksu = "KSU ID: ";
                PaymentMethodLabel1.Text = string.Concat(ksu, customerinfo.KSUlogin);
            }
            //filling out subtotal, tax, shipping tax, and total
            decimal subtotal = Math.Round(customerinfo.OrderCart.subTotal,2);
            decimal tax = Math.Round(customerinfo.OrderCart.tax, 2);
            decimal shipping = Math.Round(customerinfo.OrderCart.shipping, 2);
            decimal total = Math.Round(customerinfo.OrderCart.total, 2);
            ActualSubtotalLabel.Text = subtotal.ToString();
            ActualTaxLabel.Text = tax.ToString();
            ActualShippingLabel.Text = shipping.ToString();
            ActualTotalLabel.Text = total.ToString();   

            


        }
      
       

            //add elements to DataRow
           

    }
}