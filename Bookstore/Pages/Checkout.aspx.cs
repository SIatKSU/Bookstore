using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Bookstore.Pages
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Populates first DropDownList
            if (!IsPostBack)
            {
                //note: States dropdowns are populated in the .aspx file.
                PopulatePaymentMethodDropDown();
                PopulateMonthsDropDown();
                PopulateYearsDropDown();
            }
        }


        public void PopulatePaymentMethodDropDown() {
            ListItem creditcard = new ListItem("MasterCard/Visa", "creditcard");
            ListItem paypal = new ListItem("PayPal", "paypal");
            ListItem financialaid = new ListItem("Financial Aid", "financialaid");

            PaymentMethodDropDown.Items.Add(creditcard);
            PaymentMethodDropDown.Items.Add(paypal);
            PaymentMethodDropDown.Items.Add(financialaid);
        }

        public void PopulateMonthsDropDown()
        {
            ListItem month01 = new ListItem("01", "01");
            ListItem month02 = new ListItem("02", "02");
            ListItem month03 = new ListItem("03", "03");
            ListItem month04 = new ListItem("04", "04");
            ListItem month05 = new ListItem("05", "05");
            ListItem month06 = new ListItem("06", "06");
            ListItem month07 = new ListItem("07", "07");
            ListItem month08 = new ListItem("08", "08");
            ListItem month09 = new ListItem("09", "09");
            ListItem month10 = new ListItem("10", "10");
            ListItem month11 = new ListItem("11", "11");
            ListItem month12 = new ListItem("12", "12");

            ExpMonthDropDown.Items.Add(month01);
            ExpMonthDropDown.Items.Add(month02);
            ExpMonthDropDown.Items.Add(month03);
            ExpMonthDropDown.Items.Add(month04);
            ExpMonthDropDown.Items.Add(month05);
            ExpMonthDropDown.Items.Add(month06);
            ExpMonthDropDown.Items.Add(month07);
            ExpMonthDropDown.Items.Add(month08);
            ExpMonthDropDown.Items.Add(month09);
            ExpMonthDropDown.Items.Add(month10);
            ExpMonthDropDown.Items.Add(month11);
            ExpMonthDropDown.Items.Add(month12);
        }

        public void PopulateYearsDropDown()
        {
            System.DateTime currentDate = System.DateTime.Now;        
            int yearCounter = currentDate.Year;

            var yearList = new List<ListItem>();
            for (int i = 0; i < 21; i++)
            {
                yearList.Add(new ListItem(yearCounter.ToString(), yearCounter.ToString()));
                yearCounter++;
            }

            yearList.ForEach(c => ExpYearDropDown.Items.Add(c));
        }

        protected void PaymentMethodDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string method = PaymentMethodDropDown.SelectedValue;

            if (method == "creditcard")
            {
                CardNumberTextBox.Visible = true;
                PaymentEmailTextBox.Visible = false;
                paymentLabel3.Visible = true;
                ExpMonthDropDown.Visible = true;
                ExpYearDropDown.Visible = true;
                SecurityCodeTextBox.Visible = true;
                PasswordTextBox.Visible = false;
                paymentLabel1.Text = "Card Number:";
                paymentLabel2.Text = "Expiration Date:";
            }
            else
            {
                CardNumberTextBox.Visible = false;
                PaymentEmailTextBox.Visible = true;
                paymentLabel3.Visible = false;
                ExpMonthDropDown.Visible = false;
                ExpYearDropDown.Visible = false;
                SecurityCodeTextBox.Visible = false;
                PasswordTextBox.Visible = true;
                paymentLabel2.Text = "Password:";            
                if (method == "paypal")
                {
                    paymentLabel1.Text = "PayPal Email:";
                }
                else
                {
                    paymentLabel1.Text = "KSU Email:";
                }
            }
        }

        protected void PlaceOrderButton_Click(object sender, EventArgs e)
        {
            //validate data entry fields
            if (ValidateDataEntry())
            {
            }
            else
            {
              }
        }


        //check whether the user filled in all fields properly.
        //returns false, and shows an error message, if there was a problem.
        //returns true, and hides the error message, if it was filled in properly.
        public bool ValidateDataEntry()
        {
            Boolean isError = false;
            Regex zipCodeRgx = new Regex("\\d{5}(-\\d{4})?$");
            Regex emailRgx = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            //Billing Address
            if (String.IsNullOrWhiteSpace(BillingStreetTextBox.Text))
            {
                ErrorLabel.Text = "Please fill in the Street Address under Billing Address.";
                isError = true;
            }
            else if (String.IsNullOrWhiteSpace(BillingCityTextBox.Text))
            {
                ErrorLabel.Text = "Please fill in the City Address under Billing Address.";
                isError = true;
            }
            else if (BillingStateDropDown.SelectedValue=="NONE")
            {
                ErrorLabel.Text = "Please select a state under Billing Address.";
                isError = true;
            }
            else if (String.IsNullOrWhiteSpace(BillingZipTextBox.Text))
            {
                ErrorLabel.Text = "Please fill in the Zip Code under Billing Address.";
                isError = true;
            }
            else if (!zipCodeRgx.IsMatch(BillingZipTextBox.Text))
            {
                ErrorLabel.Text = "Billing Zipcode must be in the format ##### or #####-####.";
                isError = true;
            }
            //Shipping Address
            else if (String.IsNullOrWhiteSpace(ShippingStreetTextBox.Text))
            {
                ErrorLabel.Text = "Please fill in the Street Address under Shipping Address.";
                isError = true;
            }
            else if (String.IsNullOrWhiteSpace(ShippingCityTextBox.Text))
            {
                ErrorLabel.Text = "Please fill in the City Address under Shipping Address.";
                isError = true;
            }
            else if (ShippingStateDropDown.SelectedValue == "NONE")
            {
                ErrorLabel.Text = "Please select a state under Shipping Address.";
                isError = true;
            }
            else if (String.IsNullOrWhiteSpace(ShippingZipTextBox.Text))
            {
                ErrorLabel.Text = "Please fill in the Zip Code under Shipping Address.";
                isError = true;
            }
            else if (!zipCodeRgx.IsMatch(ShippingZipTextBox.Text))
            {
                ErrorLabel.Text = "Shipping Zipcode must be in the format ##### or #####-####.";
                isError = true;
            }
            //Name, Email, Phone number
            else if (String.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                ErrorLabel.Text = "Please enter your Full Name.";
                isError = true;
            }
            else if (!emailRgx.IsMatch(EmailTextBox.Text))
            {
                ErrorLabel.Text = "Please enter a valid email address.";
                isError = true;
            }
            else if (String.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                ErrorLabel.Text = "Please enter your Phone number.";
                isError = true;
            }


            ErrorLabel.Visible = isError;
            return !isError;
        }

    }


}