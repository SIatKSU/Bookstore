using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;

namespace Bookstore.Pages
{
    public partial class Checkout : System.Web.UI.Page
    {
        //page one:
        //string[] name = { "Jon", "Smith" };
        //HttpContext.Session["ArrayOfName"] = name;

        //page two:
        //string[] strings = (string[])Session["strings"];

        //what gets passed from Shopping cart?
        //List of the items in the cart, which is a list of 
        //    a) Row Number (this identifies the book from the Books.csv), 
        //    b) Format and 
        //    c) Quantity.  
        //From these three things, we can price, title, etc.

        //*******DUMMY VALUES WHICH WILL LATER BE READ FROM SESSION STATE***********
        List<LineItem> cart;
        double subtotal = 100.49;
        //**************************************************************************


        protected void Page_Load(object sender, EventArgs e)
        {

            //*******DUMMY VALUES WHICH WILL LATER BE READ FROM SESSION STATE***********
            cart = new List<LineItem>();
            cart.Add(new LineItem(3, LineItem.RENTAL, 1));
            //**************************************************************************

            // Populates first DropDownList
            if (!IsPostBack)
            {
                //note: States dropdowns are populated in the .aspx file.
                PopulatePaymentMethodDropDown();
                PopulateMonthsDropDown();
                PopulateYearsDropDown();
            }

            //Customer should never get to this screen if the cart is empty.  But in case this does happen, show an error message.
            if (IsCartEmpty())
                ErrorLabel.Visible = true;
            else
                ErrorLabel.Visible = false;
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
            ListItem month01 = new ListItem("01", "1");
            ListItem month02 = new ListItem("02", "2");
            ListItem month03 = new ListItem("03", "3");
            ListItem month04 = new ListItem("04", "4");
            ListItem month05 = new ListItem("05", "5");
            ListItem month06 = new ListItem("06", "6");
            ListItem month07 = new ListItem("07", "7");
            ListItem month08 = new ListItem("08", "8");
            ListItem month09 = new ListItem("09", "9");
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
                PayPalEmailTextBox.Visible = false;
                KSULoginTextBox.Visible = false;
                paymentLabel3.Visible = true;
                ExpMonthDropDown.Visible = true;
                ExpYearDropDown.Visible = true;
                SecurityCodeTextBox.Visible = true;
                PayPalPasswordTextBox.Visible = false;
                KSUPasswordTextBox.Visible = false;
                paymentLabel1.Text = "Card Number:";
                paymentLabel2.Text = "Expiration Date:";
            }
            else
            {
                CardNumberTextBox.Visible = false;                
                paymentLabel3.Visible = false;
                ExpMonthDropDown.Visible = false;
                ExpYearDropDown.Visible = false;
                SecurityCodeTextBox.Visible = false;                
                paymentLabel2.Text = "Password:";            
                if (method == "paypal")
                {
                    paymentLabel1.Text = "PayPal Email:";
                    PayPalEmailTextBox.Visible = true;
                    KSULoginTextBox.Visible = false;
                    PayPalPasswordTextBox.Visible = true;
                    KSUPasswordTextBox.Visible = false;
                }
                else
                {
                    paymentLabel1.Text = "KSU Username:";
                    PayPalEmailTextBox.Visible = false;
                    KSULoginTextBox.Visible = true;
                    PayPalPasswordTextBox.Visible = false;
                    KSUPasswordTextBox.Visible = true;
                }
            }
        }

        protected void PlaceOrderButton_Click(object sender, EventArgs e)
        {
            if (AddressCheckBox.Checked)
            {
                ShippingStreetTextBox.Text = BillingStreetTextBox.Text;
                ShippingCityTextBox.Text = BillingStreetTextBox.Text;
                ShippingStateDropDown.SelectedIndex = BillingStateDropDown.SelectedIndex;
                ShippingZipTextBox.Text = BillingZipTextBox.Text;
            }

            //if the cart is empty, or the customer didn't fill in the form properly, show an error message.
            if (IsCartEmpty() || !ValidateDataEntry()) {
                ErrorLabel.Visible = true;
            }
            else
            {
                
                //ok, everything checks out.  lets try to charge the payment!

                string method = PaymentMethodDropDown.SelectedValue;
                bool paymentSuccess;
                if (method == "creditcard")
                {
                    paymentSuccess = ChargeCreditCard();
                }
                else if (method == "paypal")
                {
                    paymentSuccess = ChargePayPal();
                }
                else // if (method == "financialaid") 
                {
                    paymentSuccess = ChargeFinancialAid();
                }

                ErrorLabel.Visible = !paymentSuccess;
                if (paymentSuccess)
                {
                    Response.Redirect("Receipt.aspx");
                }

            }
        }

        //SIMULATED credit card payment.
        //returns true if simulated payment went through
        //returns false, and sets Error Message, if simulated payment was rejected
        //Requirements:
        //16-digit number - already checked by ValidateDateEntry.
        //expiration date in the future - already checked by ValidateDateEntry.
        //verification number - "777" - we need to check this.
        public bool ChargeCreditCard()
        {
            int secCode;
            bool result = int.TryParse(SecurityCodeTextBox.Text, out secCode);
            if (result && secCode == 777)
            {
                return true;
            }
            else
            {
                ErrorLabel.Text = "The credit card you entered could not be verified.  Please try again or select another payment method.";
                return false;
            }
        }

        //SIMULATED PayPal payment.
        //returns true if simulated payment went through
        //returns false, and sets Error Message, if simulated payment was rejected
        //Requirements:
        //valid email address - already checked by ValidateDateEntry.
        //password - "12345678" - we need to check this.
        public bool ChargePayPal()
        {
            int password;
            bool result = int.TryParse(PayPalPasswordTextBox.Text, out password);
            if (result && password == 12345678)
            {
                return true;
            }
            else
            {
                ErrorLabel.Text = "The PayPal account you entered could not be verified.  Please try again or select another payment method.";
                return false;
            }
        }

        //SIMULATED Financial Aid payment.
        //returns true if simulated payment went through
        //returns false, and sets Error Message, if simulated payment was rejected
        //Requirements:
        //1) username and password must match an entry in students.txt
        //2) financial aid >= order total
        public bool ChargeFinancialAid()
        {
            bool result = false;
            string username = KSULoginTextBox.Text.Trim();
            string password = KSUPasswordTextBox.Text;

            int finAidBalance;

            string appPath = HttpRuntime.AppDomainAppPath;
            string bursarFile = appPath + "/students.txt";

            string[] lines = File.ReadAllLines(bursarFile);
            string[] bursarEntry = null;

            int i = 0;
            bool foundUserName = false;

            while (i < lines.Length && !foundUserName) {
                bursarEntry = lines[i].Split(null);
                if (bursarEntry[2] == username) {
                    foundUserName = true;
                }
                i++;
            }

            if (foundUserName && bursarEntry[3] == password)
            {
                bool finAidValidFormat = int.TryParse(bursarEntry[4], out finAidBalance);
                if (finAidValidFormat && finAidBalance > subtotal)
                {
                    result = true;
                }
                else
                {
                    ErrorLabel.Text = "Insufficient financial aid.  Current available balance: $" + finAidBalance + ".";
                }
            }
            else
            {
                ErrorLabel.Text = "Incorrect username or password.";
            }
            return result;
        }



        //check if the cart is empty.
        //returns true, and sets the error message, if cart is empty
        //returns false, if cart is not empty. 
        public bool IsCartEmpty()
        {
            if ((cart != null) && (cart.Count != 0))
            {
                return false;
            }
            else
            {
                ErrorLabel.Text = "Shopping Cart is empty.  Please click the Home/Search button to search for books.";
                return true;
            }
        }


        //check whether the user filled in all fields properly.
        //returns false, and sets the error message, if there was a problem.
        //returns true, if it was filled in properly.    
        public bool ValidateDataEntry()
        {
            Boolean isError = false;
            Regex zipCodeRgx = new Regex("\\d{5}(-\\d{4})?$");  //##### or #####-####
            Regex emailRgx = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            Regex cardRgx = new Regex(@"(?<!\d)\d{16}(?!\d)");  //16 digit number only
            Regex secCodeRgx = new Regex(@"(?<!\d)\d{3}(?!\d)");  //3 digit number only

            //7 or 10 digit phone number with extensions: http://stackoverflow.com/questions/123559/a-comprehensive-regex-for-phone-number-validation
            //Regex phoneRgx = new Regex(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$");
            //for now, not using this because customer could enter a note, etc.

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
                ErrorLabel.Text = "Please enter your phone number.";
                isError = true;
            }
            else
            {
                //payment information
                string paymentMethod = PaymentMethodDropDown.SelectedValue;

                if (paymentMethod == "creditcard")
                {
                    System.DateTime currentDate = System.DateTime.Now;
                    int expMonth;
                    bool result = int.TryParse(ExpMonthDropDown.SelectedValue, out expMonth);
                    if (!result || !cardRgx.IsMatch(CardNumberTextBox.Text))
                    {
                        ErrorLabel.Text = "Please enter a valid 16-digit credit card number.";
                        isError = true;
                    }
                    else if ((ExpYearDropDown.SelectedValue == currentDate.Year.ToString()) && (expMonth < currentDate.Month)) 
                    {
                        ErrorLabel.Text = "Please choose a valid credit card expiration date.";
                        isError = true;
                    }
                    else if (!secCodeRgx.IsMatch(SecurityCodeTextBox.Text))
                    {
                        ErrorLabel.Text = "Please enter a valid 3-digit credit card security code.";
                        isError = true;
                    }
                }
                else if (paymentMethod=="paypal")
                {
                    if (!emailRgx.IsMatch(PayPalEmailTextBox.Text))
                    {
                        ErrorLabel.Text = "Please enter a valid PayPal email address.";
                        isError = true;
                    }
                    else if (String.IsNullOrWhiteSpace(PayPalPasswordTextBox.Text))
                    {
                        ErrorLabel.Text = "Please enter your PayPal password.";
                        isError = true;
                    }
                }
                else //if (paymentMethod == "financialaid")
                {
                    if (String.IsNullOrWhiteSpace(KSULoginTextBox.Text))
                    {
                        ErrorLabel.Text = "Please enter your KSU username.";
                        isError = true;
                    }
                    else if (String.IsNullOrWhiteSpace(KSUPasswordTextBox.Text))
                    {
                        ErrorLabel.Text = "Please enter your KSU password.";
                        isError = true;
                    }
                }
            }
            return !isError;
        }

        protected void AddressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShippingAddressPanel.Visible = !AddressCheckBox.Checked;

            ShippingStreetTextBox.Text = BillingStreetTextBox.Text;
            ShippingCityTextBox.Text = BillingStreetTextBox.Text;
            ShippingStateDropDown.SelectedIndex = BillingStateDropDown.SelectedIndex;
            ShippingZipTextBox.Text = BillingZipTextBox.Text;
        }
    }
}