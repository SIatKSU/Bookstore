using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            ListItem newItem = new ListItem("New ($" + StaticData.getMatrixValue(bookRowIndex, 13) + ")");
            ListItem usedItem = new ListItem("Used ($" + StaticData.getMatrixValue(bookRowIndex, 14) + ")");
            ListItem rentItem = new ListItem("Rental ($" + StaticData.getMatrixValue(bookRowIndex, 15) + ")");
            ListItem eBookItem = new ListItem("eBook ($" + StaticData.getMatrixValue(bookRowIndex, 16) + ")");

            DDList1.Items.Add(newItem);
            DDList1.Items.Add(usedItem);
            DDList1.Items.Add(rentItem);
            DDList1.Items.Add(eBookItem);
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
    }
}