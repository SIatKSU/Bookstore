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
        Cart cart;
        CustomerInfo customerInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            //just in case cart is null, create it.
            if (Session["cart"] == null)
            {
                Session["cart"] = new Cart();
            }
            cart = (Cart)Session["cart"];

            //Customer should never get to this screen if the cart is empty.  But in case this does happen, show an error message.
            ErrorLabel.Visible = IsCartEmpty();


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
            ListItem MasterCard = new ListItem("MasterCard", "MasterCard");
            ListItem Visa = new ListItem("Visa", "Visa");
            ListItem paypal = new ListItem("PayPal", "paypal");
            ListItem financialaid = new ListItem("Financial Aid", "financialaid");

            PaymentMethodDropDown.Items.Add(MasterCard);
            PaymentMethodDropDown.Items.Add(Visa);
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

            if (method == "MasterCard" || method == "Visa")
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
                ShippingCityTextBox.Text = BillingCityTextBox.Text;
                ShippingStateDropDown.SelectedIndex = BillingStateDropDown.SelectedIndex;
                ShippingZipTextBox.Text = BillingZipTextBox.Text;
            }

            if (IsCartEmpty() || !ValidateDataEntry() || !CheckStillInStock()) {
                ErrorLabel.Visible = true;
            }
            else
            {              
                //ok, everything checks out.  lets try to charge the payment!

                string method = PaymentMethodDropDown.SelectedValue;
                bool paymentSuccess;
                if (method == "MasterCard" || method == "Visa")
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
                    //update inventory file.
                    updateInventoryFile();

                    //Copy the customer information for the Receipts page, into Session state.
                    LoadSessionData();

                    //clear the cart.
                    cart = new Cart();
                    Session["cart"] = cart;

                    Response.Redirect("Receipt.aspx");
                }

            }
        }


        public void LoadSessionData()
        {
            customerInfo = new CustomerInfo();

            customerInfo.OrderCart = cart;

            customerInfo.FullName = FullNameTextBox.Text;
            customerInfo.Email = EmailTextBox.Text;
            customerInfo.PhoneNumber = PhoneTextBox.Text;

            customerInfo.BillingStreet = BillingStreetTextBox.Text;
            customerInfo.BillingCity = BillingCityTextBox.Text;
            customerInfo.BillingState = BillingStateDropDown.Text;
            customerInfo.BillingZip = BillingZipTextBox.Text;

            customerInfo.ShippingStreet = ShippingStreetTextBox.Text;
            customerInfo.ShippingCity = ShippingCityTextBox.Text;
            customerInfo.ShippingState = ShippingStateDropDown.Text;
            customerInfo.ShippingZip = ShippingZipTextBox.Text;

            customerInfo.PaymentMethod = PaymentMethodDropDown.Text;

            string paymentMethod = PaymentMethodDropDown.SelectedValue;
            if ((paymentMethod == "MasterCard") || (paymentMethod == "Visa"))
            {
                customerInfo.CardNumber = CardNumberTextBox.Text;
            }
            else if (paymentMethod == "paypal")
            {
                customerInfo.PayPalEmail = PayPalEmailTextBox.Text;
            }
            else
            {
                customerInfo.KSUlogin = KSULoginTextBox.Text;
            }

            Session["CustomerInfo"] = customerInfo;
        }



        //note implemented yet - but in future, if there's a problem with updating the inventoryFile, we could flag this order for the Admin Assistant  
        public bool updateInventoryFile()
        {
            bool result = true;

            for (int i = 0; i < cart.cartList.Count; i++)
            {
                if (cart.cartList[i].format != LineItem.EBOOK)       //ebook quantity doesn't change
                {
                    //row 9 = NEW.
                    int quantityAvailable;
                    string quantityString = StaticData.getMatrixValue(cart.cartList[i].rowNumber, StaticData.QUANTITY_NEW + cart.cartList[i].format);
                    bool quantityResult = int.TryParse(quantityString, out quantityAvailable);
                    quantityAvailable -= cart.cartList[i].quantity;

                    //insert the updated quantity back into the matrix
                    StaticData.setMatrixValue(cart.cartList[i].rowNumber, StaticData.QUANTITY_NEW + cart.cartList[i].format, quantityAvailable.ToString());
                }
            }

            //finally - write the result back into books.csv
            StaticData.writeFile();
         

            return result;
        }



        //check that the items are still in stock.
        //did anyone place order in the meantime?
        public Boolean CheckStillInStock()
        {
            StaticData.readFile();
            bool result = true;

            for (int i = 0; i < cart.cartList.Count; i++)
            {

                if (cart.cartList[i].format != LineItem.EBOOK)       //ebooks never go out of stock.
                {
                    //row 9 = NEW.
                    int quantityAvailable;
                    string quantityString = StaticData.getMatrixValue(cart.cartList[i].rowNumber, StaticData.QUANTITY_NEW + cart.cartList[i].format);
                    bool quantityResult = int.TryParse(quantityString, out quantityAvailable);

                    if (quantityAvailable < cart.cartList[i].quantity)
                    {
                        ErrorLabel.Text = "Not enough items in stock.  Please return to the shopping cart page.";
                        result = false;
                    }
                }
            }
            return result;
        }


        //returns true if cart is empty, and sets error message.
        //returns false if cart is not empty.
        public Boolean IsCartEmpty()
        {
            if (cart.cartList.Count == 0)
            {
                ErrorLabel.Text = "Shopping Cart is empty.  Please click the Home/Search button to search for books.";
                return true;
            }
            else
            {
                return false;
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

            decimal finAidBalance;

            string appPath = HttpRuntime.AppDomainAppPath;
            string bursarFile = appPath + "/students.txt";

            if (File.Exists(bursarFile))
            {
                string[] lines = File.ReadAllLines(bursarFile);
                string[] bursarEntry = null;

                int i = 0;
                bool foundUserName = false;

                while (!foundUserName && i < lines.Length)
                {
                    bursarEntry = lines[i].Split(null);
                    if (bursarEntry[2] == username)
                    {
                        foundUserName = true;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (foundUserName && bursarEntry[3] == password)
                {
                    bool finAidValidFormat = decimal.TryParse(bursarEntry[4], out finAidBalance);
                    if (finAidValidFormat && finAidBalance > cart.total)
                    {
                        //i is the line number in students.txt to edit
                        //bursarEntry 4 is the financial aid balance

                        finAidBalance -= cart.total;
                        bursarEntry[4] = finAidBalance.ToString();
                        lines[i] = string.Join("\t", bursarEntry);

                        StreamWriter sw = new StreamWriter(bursarFile);
                        for (int j = 0; j < lines.Length; j++)
                        {
                            sw.WriteLine(lines[j]);
                        }
                        sw.Close();

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
            }
            else
            {
                ErrorLabel.Text = "Could not connect to Bursar. <br> Please select another payment method or try again later.";
            }

            return result;
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

                if (paymentMethod == "Visa" || paymentMethod == "MasterCard") 
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