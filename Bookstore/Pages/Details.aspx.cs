using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace Bookstore.Pages
{
    public partial class Details : System.Web.UI.Page
    {
        private string isbn;
        private int bookRowIndex;
        private int[] CRNIndicies;

        protected void Page_Load(object sender, EventArgs e)
        {
            eBookAlreadyInCartError.Visible = false;

            isbn = Request.QueryString["isbn"].ToLower();

            setBookRowIndex();
            CRNIndicies = getCRNIndicies();
            setValues();
            setGridTable();
            if (!IsPostBack)
            {
                setCartPanel();
            }
        }

        private void setValues()
        {
            string description = StaticData.getMatrixValue(bookRowIndex, 17);
            string replaced = description.Replace("\n", "<br/>");
            Debug.WriteLine(replaced);

            TitleText.Text = StaticData.getMatrixValue(bookRowIndex, 1);
            ISBNText.Text = StaticData.getMatrixValue(bookRowIndex, 0);
            AuthorText.Text = StaticData.getMatrixValue(bookRowIndex, 2);
            DescriptionText.Text = replaced;

            CoverImage.ImageUrl = "/Images/" + isbn + ".jpg";


        }

        private void setCartPanel()
        {
            //to format as $
            string newPriceStr = String.Format("Subtotal: {0:C}", Convert.ToDecimal(StaticData.getMatrixValue(bookRowIndex, StaticData.PRICE_NEW)));
            string usedPriceStr = String.Format("Subtotal: {0:C}", Convert.ToDecimal(StaticData.getMatrixValue(bookRowIndex, StaticData.PRICE_USED)));
            string rentalPriceStr = String.Format("Subtotal: {0:C}", Convert.ToDecimal(StaticData.getMatrixValue(bookRowIndex, StaticData.PRICE_RENTAL)));
            string eBookPriceStr = String.Format("Subtotal: {0:C}", Convert.ToDecimal(StaticData.getMatrixValue(bookRowIndex, StaticData.PRICE_EBOOK)));

            ListItem newItem = new ListItem("New " + newPriceStr);
            ListItem usedItem = new ListItem("Used " + usedPriceStr);
            ListItem rentItem = new ListItem("Rental " + rentalPriceStr);
            ListItem eBookItem = new ListItem("eBook " + eBookPriceStr);

            //ListItem newItem = new ListItem("New $" + StaticData.getMatrixValue(bookRowIndex, 13));
            //ListItem usedItem = new ListItem("Used $" + StaticData.getMatrixValue(bookRowIndex, 14));
            //ListItem rentItem = new ListItem("Rental $" + StaticData.getMatrixValue(bookRowIndex, 15));
            //ListItem eBookItem = new ListItem("eBook $" + StaticData.getMatrixValue(bookRowIndex, 16));
            newItem.Value = "0";
            usedItem.Value = "1";
            rentItem.Value = "2";
            eBookItem.Value = "3";


            //if format out of stock, don't add to the dropdown
            int newQuantity, usedQuantity, rentalQuantity, eBookQuantity;
            bool valid;
            string quantityStr;

            quantityStr = StaticData.getMatrixValue(bookRowIndex, StaticData.QUANTITY_NEW);
            valid = int.TryParse(quantityStr, out newQuantity);
            if (valid && newQuantity > 0)
                RBList1.Items.Add(newItem);

            quantityStr = StaticData.getMatrixValue(bookRowIndex, StaticData.QUANTITY_USED);
            valid = int.TryParse(quantityStr, out usedQuantity);
            if (valid && usedQuantity > 0)
                RBList1.Items.Add(usedItem);

            quantityStr = StaticData.getMatrixValue(bookRowIndex, StaticData.QUANTITY_RENTAL);
            valid = int.TryParse(quantityStr, out rentalQuantity);
            if (valid && rentalQuantity > 0)
                RBList1.Items.Add(rentItem);

            quantityStr = StaticData.getMatrixValue(bookRowIndex, StaticData.EBOOK_AVAIL);
            valid = int.TryParse(quantityStr, out eBookQuantity);
            if (valid && eBookQuantity > 0)
                RBList1.Items.Add(eBookItem);

        }

        //
        //
        //
        //
        private void setBookRowIndex()
        {
            string[] ISBN = StaticData.getMatrixColumn(0);
            for (int i = 0; i < ISBN.Length; i++)
            {
                string temp = ISBN[i];
                if (temp.Contains(this.isbn))
                {
                    bookRowIndex = i;
                    break;
                }
            }
        }

        private int[] getCRNIndicies()
        {
            string[] CRN = StaticData.getMatrixColumn(7);
            var list = new List<int>();
            string originalCRN = CRN[bookRowIndex];

            for (int i = 0; i < CRN.Length; i++)
            {
                if (CRN[i].Contains(originalCRN))
                {
                    list.Add(i);
                }
            }
            return list.ToArray();
        }

        private void setGridTable()
        {
            //create DataTable & DataRow
            DataTable dt = new DataTable();
            DataRow dr = null;

            //Adds columns to DataTable
            dt.Columns.Add(new DataColumn("Course", typeof(string)));
            dt.Columns.Add(new DataColumn("CRN", typeof(string)));
            dt.Columns.Add(new DataColumn("Professor", typeof(string)));
            dt.Columns.Add(new DataColumn("Requirement", typeof(string)));

            //add elements to DataRow
            for (int i = 0; i < CRNIndicies.Length; i++)
            {

                //fills row data
                dr = dt.NewRow();
                dr["Course"] = StaticData.getMatrixValue(CRNIndicies[i], 4) + " Section " + StaticData.getMatrixValue(CRNIndicies[i], 5);
                dr["CRN"] = StaticData.getMatrixValue(CRNIndicies[i], 7);
                dr["Professor"] = StaticData.getMatrixValue(CRNIndicies[i], 6);
                dr["Requirement"] = StaticData.getMatrixValue(CRNIndicies[i], 8);


                dt.Rows.Add(dr);
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private int getRBList1SelectedFormat()
        {
            switch (RBList1.SelectedValue)
            {
                case "0": return 0;
                case "1": return 1;
                case "2": return 2;
                case "3": return 3;
                default: return -1;
            }
        }

        protected void AddToCartButton_Click(object sender, EventArgs e)
        {
            int selectedFormat = getRBList1SelectedFormat();

            if (selectedFormat == -1)
            {
                AddToCartError.Visible = true;
            }
            else
            {
                //just in case cart is null, create it.
                if (Session["cart"] == null)
                {
                    Session["cart"] = new Cart();
                }
                Cart cart = (Cart) Session["cart"];

                int addResult = cart.AddToCart(bookRowIndex, selectedFormat, 1);
                if (addResult == -1)
                {
                    eBookAlreadyInCartError.Visible = true;
                }
                else
                {
                    // redirect to cart page
                    Response.Redirect("Cart.aspx");
                }
            }
        }
    }
}