using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bookstore.Pages
{
    public partial class Search : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["Type"];
            string value = Request.QueryString["Value"];

            setGridTable(searchData(type, value));


            TextBox1.Text = type;
            TextBox2.Text = value;
        }


        private int[] searchData(string type, string value)
        {
            var indicesOfMatches = new List<int>();

            if (type == "Keyword")
            {
                // search for keyword
            }
            else if (type == "Professor")
            {
                string[] profsArray = StaticData.getMatrixColumn(6);

                // checks if the professors name matches the search value and
                // adds the row number(index) to the indexOfMatches array if there is a match
                for (int i = 0; i < profsArray.Length; i++)
                {
                    if (profsArray[i] == value)
                    {
                        indicesOfMatches.Add(i);
                    }
                }
            }
            else if (type == "Course")
            {
                string[] coursesArray = StaticData.getMatrixColumn(4);

                // checks if the professors name matches the search value and
                // adds the row number(index) to the indexOfMatches array if there is a match
                for (int i = 0; i < coursesArray.Length; i++)
                {
                    if (coursesArray[i] == value)
                    {
                        indicesOfMatches.Add(i);
                    }
                }
            }

            return indicesOfMatches.ToArray();
        }



        private void setGridTable(int[] rows)
        {
            //create DataTable & DataRow
            DataTable dt = new DataTable();
            DataRow dr = null;




            dt.Columns.Add("image");
            dt.Columns.Add(new DataColumn("Titlee", typeof(string)));
            dt.Columns.Add(new DataColumn("Author", typeof(string)));
            dt.Columns.Add(new DataColumn("ISBN", typeof(string)));
            /**..
            

            foreach(DataColumn col in dt.Columns)
            {
                TemplateField tfield = new TemplateField();
                if(col.ColumnName == "Image")
                {
                    ImageField ifield = new ImageField();
                    ifield.HeaderText = "aye";// col.ColumnName;
                    ifield.
                    GridView1.Columns.Add(ifield);
                }
            }
     */


            //add elements to DataRow
            for (int i = 0; i < rows.Length; i++)
            {

                dr = dt.NewRow();


                dr["image"] = "/Images/cart.png";
                dr["Titlee"] = StaticData.getMatrixValue(rows[i], 1);
                dr["Author"] = StaticData.getMatrixValue(rows[i], 2);
                dr["ISBN"] = StaticData.getMatrixValue(rows[i], 0);


                dt.Rows.Add(dr);

                Debug.WriteLine("..." + dr["ISBN"].ToString());
            }

            ViewState["CurrentTable"] = dt;



            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}