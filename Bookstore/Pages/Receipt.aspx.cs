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
        int additionallines;
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
            if (customerinfo.PaymentMethod == "MasterCard" || customerinfo.PaymentMethod == "Visa") {
                lastfourdigits = customerinfo.CardNumber.Substring(Math.Max(0, customerinfo.CardNumber.Length - 4));
                lastfourdigits = " " + lastfourdigits.Trim();
                PaymentMethodLabel1.Text = string.Concat(customerinfo.PaymentMethod, lastfourdigits);


            }
            else if (customerinfo.PaymentMethod == "paypal") {
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
            decimal subtotal = Math.Round(customerinfo.OrderCart.subTotal, 2);
            decimal tax = Math.Round(customerinfo.OrderCart.tax, 2);
            decimal shipping = Math.Round(customerinfo.OrderCart.shipping, 2);
            decimal total = Math.Round(customerinfo.OrderCart.total, 2);
            ActualSubtotalLabel.Text = subtotal.ToString();
            ActualTaxLabel.Text = tax.ToString();
            ActualShippingLabel.Text = shipping.ToString();
            ActualTotalLabel.Text = total.ToString();
            //filing out items
            setGridValues();
            generateEbookUrls();


        }
        
        private void setGridValues()
        {
            //actual datatable
            DataTable dt = new DataTable();
            
            DataRow dr = null;


            //Adds columns to DataTable
            dt.Columns.Add(new DataColumn("ItemName"));
            dt.Columns.Add(new DataColumn("ItemAuthor"));
            dt.Columns.Add(new DataColumn("ItemISBN"));
            dt.Columns.Add(new DataColumn("ItemType"));
            dt.Columns.Add(new DataColumn("ItemQuantity"));
            dt.Columns.Add(new DataColumn("ItemUnitPrice"));
            int countEbook = 0;
            int countRentals = 0;


            //getting additional lines for amount of ebooks/rentals in cart
            for (int i = 0; i < customerinfo.OrderCart.cartList.Count; i++)
            {

                switch (customerinfo.OrderCart.cartList[i].format)
                {

                    case LineItem.RENTAL:
                        countRentals++;
                        break;
                    case LineItem.EBOOK:
                        countEbook++;
                        break;

                }

            }
            additionallines = countEbook + countRentals;
            //adding lines to cart
            int n = 0;
            int counter = 0;
            while(n < customerinfo.OrderCart.cartList.Count + additionallines) { 
                //omg it works
               dr = dt.NewRow();
                //breaks if n  = 1 so put this
                if (n > 0)
                {
                    //checks if row before it is a "junk" row and fills row with 
                    //might not need this since else statement does the same but leaving it in anyways as not to break it
                    if (dt.Rows[n-1][3].Equals("junk"))
                    {
                        String isbn = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[n- counter].rowNumber, StaticData.ISBN);

                        dr["ItemISBN"] = isbn;
                        dr["ItemName"] = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[n- counter].rowNumber, StaticData.TITLE);

                        dr["ItemAuthor"] = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[n- counter].rowNumber, StaticData.AUTHOR);

                        switch (customerinfo.OrderCart.cartList[n- counter].format)
                        {
                            case LineItem.NEW:
                                dr["ItemType"] = "New";
                                break;
                            case LineItem.USED:
                                dr["ItemType"] = "Used";
                                break;
                            case LineItem.RENTAL:
                                dr["ItemType"] = "Rental";

                                break;
                            case LineItem.EBOOK:
                                dr["ItemType"] = "eBook";

                                break;
                            default:
                                dr["ItemType"] = "invalid";
                                break;
                        }

                        dr["ItemUnitPrice"] = String.Format("{0:C}", customerinfo.OrderCart.cartList[n- counter].price);
                        dr["ItemQuantity"] = customerinfo.OrderCart.cartList[n- counter].quantity;
                    }

                    //checks if row before is ebook for rental and makes new row a "junk" row
                    //junk rows will be merged later to printout ebook links/rental due notices
                    else if (dt.Rows[n - 1][3].Equals("eBook") || dt.Rows[n - 1][3].Equals("Rental"))
                    {
                        string junk = "junk";
                        dr["ItemISBN"] = junk;
                        dr["ItemName"] = junk;
                        dr["ItemAuthor"] = junk;
                        dr["ItemType"] = junk;
                        dr["ItemUnitPrice"] = junk;
                        dr["ItemQuantity"] = junk;
                        counter++;
                    }
                    //fills cart minus any junk rows that are placed
                    else
                    {
                        String isbn = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[n-counter].rowNumber, StaticData.ISBN);

                        dr["ItemISBN"] = isbn;
                        dr["ItemName"] = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[n-counter].rowNumber, StaticData.TITLE);

                        dr["ItemAuthor"] = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[n-counter].rowNumber, StaticData.AUTHOR);

                        switch (customerinfo.OrderCart.cartList[n-counter].format)
                        {
                            case LineItem.NEW:
                                dr["ItemType"] = "New";
                                break;
                            case LineItem.USED:
                                dr["ItemType"] = "Used";
                                break;
                            case LineItem.RENTAL:
                                dr["ItemType"] = "Rental";

                                break;
                            case LineItem.EBOOK:
                                dr["ItemType"] = "eBook";

                                break;
                            default:
                                dr["ItemType"] = "invalid";
                                break;
                        }

                        dr["ItemUnitPrice"] = String.Format("{0:C}", customerinfo.OrderCart.cartList[n-counter].price);
                        dr["ItemQuantity"] = customerinfo.OrderCart.cartList[n-counter].quantity;
                    }
                
                   
                }
                //fill first row in datatable with whatever is in cart
                else if(n == 0)
                {
                    String isbn = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[n].rowNumber, StaticData.ISBN);

                    dr["ItemISBN"] = isbn;
                    dr["ItemName"] = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[n].rowNumber, StaticData.TITLE);

                    dr["ItemAuthor"] = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[n].rowNumber, StaticData.AUTHOR);

                    switch (customerinfo.OrderCart.cartList[n].format)
                    {
                        case LineItem.NEW:
                            dr["ItemType"] = "New";
                            break;
                        case LineItem.USED:
                            dr["ItemType"] = "Used";
                            break;
                        case LineItem.RENTAL:
                            dr["ItemType"] = "Rental";

                            break;
                        case LineItem.EBOOK:
                            dr["ItemType"] = "eBook";

                            break;
                        default:
                            dr["ItemType"] = "invalid";
                            break;
                    }

                    dr["ItemUnitPrice"] = String.Format("{0:C}", customerinfo.OrderCart.cartList[n].price);
                    dr["ItemQuantity"] = customerinfo.OrderCart.cartList[n].quantity;
                }
               
                
               

            dt.Rows.Add(dr);
                n++;
          }
    

        GridView1.DataSource = dt;
        GridView1.DataBind();
                         
        }

        private void generateEbookUrls()
        {

            string urlstart = "Ebook Link: www.bookstore.com/";
            string rental = "Rental Due Date: December 14th 2017";
            for (int i = 0; i < customerinfo.OrderCart.cartList.Count + additionallines; i++)
            {
                //generates ebook links
                Guid id = Guid.NewGuid();
                string uuid = id.ToString();
                string ebookurl = string.Concat(urlstart, uuid);
                //check for ebooks           
                if (GridView1.Rows[i].Cells[3].Text.ToString().Equals("eBook"))
                {
                    //if only item in cart or last items then just replace line below it with ebook link

                    if (GridView1.Rows[i + 1].Cells[3].Text.ToString().Equals("junk"))
                    {
                        for (int j = 1; j < 6; j++)
                        {
                            //merging columns of row below ebook row
                            GridView1.Rows[i + 1].Cells[j].Visible = false;

                        }
                        //making merged column ebook url
                        GridView1.Rows[i + 1].Cells[0].Attributes.Add("colspan", "6");
                        GridView1.Rows[i + 1].Cells[0].Text = ebookurl;
                    }


                }

                else if (GridView1.Rows[i].Cells[3].Text.ToString().Equals("Rental"))
                {
                    //if only item or last one then just replace line below with rental due date
                    if (GridView1.Rows[i + 1].Cells[3].Text.ToString().Equals("junk"))
                    {
                        for (int j = 1; j < 6; j++)
                        {
                            //merging columns of row below rental
                            GridView1.Rows[i + 1].Cells[j].Visible = false;

                        }
                        //filling merged column with rental due notice
                        GridView1.Rows[i + 1].Cells[0].Attributes.Add("colspan", "6");
                        GridView1.Rows[i + 1].Cells[0].Text = rental;
                    }

                }





            }
        }
       

            //add elements to DataRow
           

    }
}