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
            TitleText.Text = StaticData.getMatrixValue(bookRowIndex, 1);
            ISBNText.Text = StaticData.getMatrixValue(bookRowIndex, 0);
            AuthorText.Text = StaticData.getMatrixValue(bookRowIndex, 2);
            DescriptionText.Text = StaticData.getMatrixValue(bookRowIndex, 17);

            CoverImage.ImageUrl = "/Images/" + isbn + ".jpg";


        }

        private void setCartPanel()
        {
            ListItem newItem = new ListItem("New $" + StaticData.getMatrixValue(bookRowIndex, 13));
            ListItem usedItem = new ListItem("Used $" + StaticData.getMatrixValue(bookRowIndex, 14));
            ListItem rentItem = new ListItem("Rental $" + StaticData.getMatrixValue(bookRowIndex, 15));
            ListItem eBookItem = new ListItem("eBook $" + StaticData.getMatrixValue(bookRowIndex, 16));
            newItem.Value = "0";
            usedItem.Value = "1";
            rentItem.Value = "2";
            eBookItem.Value = "3";

            RBList1.Items.Add(newItem);
            RBList1.Items.Add(usedItem);
            RBList1.Items.Add(rentItem);
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
                //display error
            }
            else
            {
                if (Session["cart"] == null)
                {
                    List<LineItem> cartList = new List<LineItem>();
                    cartList.Add(new LineItem(bookRowIndex, selectedFormat, 1));
                    Session["cart"] = cartList;
                }
                else
                {
                    List<LineItem> cartList = (List<LineItem>)Session["cart"];
                    List<LineItem> newCartList = new List<LineItem>();
                    bool addItem = true;

                    for (int i = 0; i < cartList.Count; i++)
                    {
                        //if current LineItem matches one already in the Cart
                        if ((cartList[i].rowNumber == bookRowIndex) &&
                            (cartList[i].format == selectedFormat))
                        {
                            LineItem newTemp = new LineItem(bookRowIndex, selectedFormat, (cartList[i].quantity + 1));
                            newCartList.Add(newTemp);
                            addItem = false;
                        }
                        else
                        {
                            newCartList.Add(cartList[i]);
                        }
                    }
                    if (addItem && cartList.Count == newCartList.Count)
                    {
                        newCartList.Add(new LineItem(bookRowIndex, selectedFormat, 1));
                    }
                    // set new cart to session["cart"]
                    Session["cart"] = newCartList;
                }
            }
            // redirect to cart page
            Response.Redirect("Cart.aspx");
        }
    }
}