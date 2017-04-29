using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Net.Mail;


namespace Bookstore.Pages
{
    public partial class Receipt : System.Web.UI.Page
    {
        CustomerInfo customerinfo;
                    string rentalNotice = "Rental Due Date: December 14th 2017.  Late returns will be charged a fee at the end of the semester.";

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
            decimal subtotal = customerinfo.OrderCart.subTotal;
            decimal tax = customerinfo.OrderCart.tax;
            decimal shipping = customerinfo.OrderCart.shipping;
            decimal total = customerinfo.OrderCart.total;
            ActualSubtotalLabel.Text = String.Format("{0:C}", subtotal);
            ActualTaxLabel.Text = String.Format("{0:C}", tax);
            ActualShippingLabel.Text = String.Format("{0:C}", shipping);
            ActualTotalLabel.Text = String.Format("{0:C}", total);
            //filing out items
            setGridValues();

            List<String> eBookURLS = new List<String>();
            generateEbookUrls(eBookURLS);

            if (!customerinfo.ReceiptSaved)
            {

                //generate filename.
                string path = StaticData.appPath + "/Receipts/";
                string uuid = Guid.NewGuid().ToString();
                string filename = path + uuid + ".pdf";

                var doc1 = new Document();

                //use a variable to let my code fit across the page...

                //string path = Server.MapPath("PDFs");
                PdfWriter.GetInstance(doc1, new FileStream(filename, FileMode.Create));
                doc1.Open();

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(StaticData.appPath + @"/images/KSU_Logo.png");

                doc1.Add(jpg);
                doc1.Add(new Paragraph(" "));
                doc1.Add(new Paragraph(" "));
                doc1.Add(new Paragraph("Thank you for your order from the Kennesaw Online Bookstore."));
                doc1.Add(new Paragraph(" "));
                doc1.Add(new Paragraph("Name: " + customerinfo.FullName));
                doc1.Add(new Paragraph("Phone: " + customerinfo.PhoneNumber));
                doc1.Add(new Paragraph("Email: " + customerinfo.Email));

                doc1.Add(new Paragraph("Billing Address: "));
                doc1.Add(new Paragraph(customerinfo.BillingStreet));
                doc1.Add(new Paragraph(citystateBLabel.Text));
                doc1.Add(new Paragraph("Shipping Address: "));
                doc1.Add(new Paragraph(customerinfo.ShippingStreet));
                doc1.Add(new Paragraph(citystateLabel4.Text));

                doc1.Add(new Paragraph("Payment Method: " + PaymentMethodLabel1.Text));

                doc1.Add(new Paragraph(" "));

                /*
                PdfPTable table = new PdfPTable(6);
                float[] widths = new float[] { 1.5f, 1f, 1f, .75f, .75f, .5f};
                table.SetWidths(widths);
                PdfPCell titleCell = new PdfPCell(new Phrase("Title"));
                titleCell.Border = 0;
                titleCell.HorizontalAlignment = 0;
                titleCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                titleCell.BorderWidthBottom = 1f;
                table.AddCell(titleCell);

                PdfPCell authorCell = new PdfPCell(new Phrase("Author"));
                authorCell.Border = 0;
                authorCell.HorizontalAlignment = 0;
                authorCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                authorCell.BorderWidthBottom = 1f;
                table.AddCell(authorCell);

                PdfPCell isbnCell = new PdfPCell(new Phrase("ISBN"));
                isbnCell.Border = 0;
                isbnCell.HorizontalAlignment = 0;
                isbnCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                isbnCell.BorderWidthBottom = 1f;
                table.AddCell(isbnCell);

                PdfPCell typeCell = new PdfPCell(new Phrase("Type"));
                titleCell.Border = 0;
                titleCell.HorizontalAlignment = 0;
                titleCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                titleCell.BorderWidthBottom = 1f;
                table.AddCell(typeCell);

                PdfPCell priceCell = new PdfPCell(new Phrase("Price"));
                authorCell.Border = 0;
                authorCell.HorizontalAlignment = 0;
                authorCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                authorCell.BorderWidthBottom = 1f;
                table.AddCell(priceCell);

                PdfPCell quantityCell = new PdfPCell(new Phrase("Quantity"));
                isbnCell.Border = 0;
                isbnCell.HorizontalAlignment = 0;
                isbnCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                isbnCell.BorderWidthBottom = 1f;
                table.AddCell(quantityCell);
                */
               
                doc1.Add(new Paragraph("Subtotal: " + String.Format("{0:C}", customerinfo.OrderCart.subTotal)));
                doc1.Add(new Paragraph("Tax: " + String.Format("{0:C}", customerinfo.OrderCart.tax)));
                doc1.Add(new Paragraph("Shipping: " + String.Format("{0:C}", customerinfo.OrderCart.shipping)));
                doc1.Add(new Paragraph("Total: " + String.Format("{0:C}", customerinfo.OrderCart.total)));
                doc1.Add(new Paragraph(" "));

                int eBookURLcounter = 0;
                for (int i = 0; i < customerinfo.OrderCart.cartList.Count; i++)
                {
                    String isbn = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[i].rowNumber, StaticData.ISBN);                
                    String bookName = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[i].rowNumber, StaticData.TITLE);
                    String author = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[i].rowNumber, StaticData.AUTHOR);
                    String format = LineItem.FormatIntToString(customerinfo.OrderCart.cartList[i].format);
                    String price = String.Format("{0:C}", customerinfo.OrderCart.cartList[i].price);
                    String quantity = customerinfo.OrderCart.cartList[i].quantity.ToString();

                    doc1.Add(new Paragraph("Title: " + bookName));
                    doc1.Add(new Paragraph("Author: " + author));
                    doc1.Add(new Paragraph("ISBN: " + isbn));
                    doc1.Add(new Paragraph("format: " + format));

                    if (format == "Rental")
                    {
                        doc1.Add(new Paragraph(rentalNotice));
                    }
                    else if (format == "eBook")
                    {
                        Font link = FontFactory.GetFont("Arial", 12, Font.UNDERLINE, BaseColor.BLUE);
                        Anchor anchor = new Anchor(eBookURLS[eBookURLcounter], link);
                        anchor.Reference = eBookURLS[eBookURLcounter];
                        doc1.Add(anchor);
                        eBookURLcounter++;                
                    }

                    doc1.Add(new Paragraph("Price: " + price));
                    doc1.Add(new Paragraph("Quantity: " + quantity));
                    doc1.Add(new Paragraph(" "));
                    /*
                    PdfPCell cell = new PdfPCell(new Phrase(bookName));
                    cell.Border = 0;
                    cell.HorizontalAlignment = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(author));
                    cell.Border = 0;
                    cell.HorizontalAlignment = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(isbn));
                    cell.Border = 0;
                    cell.HorizontalAlignment = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(format));
                    cell.Border = 0;
                    cell.HorizontalAlignment = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(price));
                    cell.Border = 0;
                    cell.HorizontalAlignment = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(quantity));
                    cell.Border = 0;
                    cell.HorizontalAlignment = 0;
                    table.AddCell(cell);
                    */
                }

                //doc1.Add(table);


                doc1.Close();
                
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;

                // setup Smtp authentication
                System.Net.NetworkCredential credentials =
                    new System.Net.NetworkCredential("ksuOnlineBookstore2017@gmail.com", "ksuksuksuksu");
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("ksuOnlineBookstore2017@gmail.com");
                msg.To.Add(new MailAddress(customerinfo.Email));

                msg.Subject = "Your order from the Kennesaw Online Bookstore";
//                msg.IsBodyHtml = true;
                msg.Body = string.Format("Thank you for your order from the Kennesaw Online Bookstore.\n\nYour invoice is attached.");
                msg.Attachments.Add(new Attachment(filename));

                try
                {
                    client.Send(msg);
                    //lblMsg.Text = "Your message has been successfully sent.";
                }
                catch (Exception ex)
                {
                    //lblMsg.ForeColor = Color.Red;
                    //lblMsg.Text = "Error occured while sending your message." + ex.Message;
                }


                customerinfo.ReceiptSaved = true;
            }
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

            //getting additional lines for amount of ebooks/rentals in cart
            for (int i = 0; i < customerinfo.OrderCart.cartList.Count; i++)
            {
                dr = dt.NewRow();

                String isbn = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[i].rowNumber, StaticData.ISBN);

                dr["ItemISBN"] = isbn;
                dr["ItemName"] = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[i].rowNumber, StaticData.TITLE);
                dr["ItemAuthor"] = StaticData.getMatrixValue(customerinfo.OrderCart.cartList[i].rowNumber, StaticData.AUTHOR);
                dr["ItemType"] = LineItem.FormatIntToString(customerinfo.OrderCart.cartList[i].format);
                dr["ItemUnitPrice"] = String.Format("{0:C}", customerinfo.OrderCart.cartList[i].price);
                dr["ItemQuantity"] = customerinfo.OrderCart.cartList[i].quantity;
                dt.Rows.Add(dr);


                if (customerinfo.OrderCart.cartList[i].format == LineItem.EBOOK)
                {
                    //add a dummy row to display eBook link
                    dr = dt.NewRow();
                    dr["ItemISBN"] = "eBook";
                    dt.Rows.Add(dr);
                }
                else if (customerinfo.OrderCart.cartList[i].format == LineItem.RENTAL)
                {
                    //add a dummy row to display rental warning
                    dr = dt.NewRow();
                    dr["ItemISBN"] = "Rental";
                    dt.Rows.Add(dr);
                }
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }
        
        private void generateEbookUrls(List<String> eBookURLS)
        {
            //rental and eBook messages

            //<a href=>https://www.w3schools.com/html/</a>
            string urlstart = @"Ebook Link: <a href=>www.bookstore.com/";

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {

                //check for ISBN = "eBook"       
                if (GridView1.Rows[i].Cells[2].Text.ToString().Equals("eBook"))
                {
                    //merge cells and replace with ebook link
                    for (int j = 1; j < 6; j++)
                    {
                        GridView1.Rows[i].Cells[j].Visible = false;
                    }
                    GridView1.Rows[i].Cells[0].Attributes.Add("colspan", "6");
                    GridView1.Rows[i].Cells[0].Style["font-style"] = "italic";
                    //generate ebook links
                    string uuid = Guid.NewGuid().ToString();
                    string ebookurl = urlstart + uuid + @"</a>";
                    eBookURLS.Add("www.bookstore.com/" + uuid);
                    GridView1.Rows[i].Cells[0].Text = ebookurl;
                }

                else if (GridView1.Rows[i].Cells[2].Text.ToString().Equals("Rental"))
                {
                    //merge cells and replace with rental due date
                    for (int j = 1; j < 6; j++)
                    {
                        GridView1.Rows[i].Cells[j].Visible = false;
                    }
                    //filling merged column with rental due notice
                    GridView1.Rows[i].Cells[0].Attributes.Add("colspan", "6");
                    GridView1.Rows[i].Cells[0].Style["font-style"] = "italic";
                    GridView1.Rows[i].Cells[0].Text = rentalNotice;                   
                }
            }
        }
    }
}