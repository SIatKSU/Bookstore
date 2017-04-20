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
        private string type, value;

        protected void Page_Load(object sender, EventArgs e)
        {
            type = Request.QueryString["Type"].ToLower();
            value = Request.QueryString["Value"].ToLower();


            GridView1.PageIndex = 0; //sets default GridView1 page to the first page
            setGridTable(searchData());


            TextBox1.Text = type;
            TextBox2.Text = value;
        }
        //
        //
        //
        // Todo: Projessor "Tsai-Tien Tseng" doesn't show up..
        //
        //
        // returns array with row indicies that contained the search value
        private int[] searchData()
        {
            var indicesOfMatches = new List<int>();

            if (type == "keyword")
            {
                indicesOfMatches = getKeywordMatcheIndices(value);
            }
            else if (type == "professor")
            {
                string[] profsArray = StaticData.getMatrixColumn(6);


                // checks if the professors name matches the search value and
                // adds the row number(index) to the indexOfMatches array if there is a match
                for (int i = 0; i < profsArray.Length; i++)
                {
                    if (profsArray[i].ToLower().Contains(value))
                    {
                        indicesOfMatches.Add(i);
                    }
                }
            }
            else if (type == "course")
            {
                string[] coursesArray = StaticData.getMatrixColumn(4);

                // checks if the professors name matches the search value and
                // adds the row number(index) to the indexOfMatches array if there is a match
                for (int i = 0; i < coursesArray.Length; i++)
                {
                    if (coursesArray[i].ToLower().Contains(value))
                    {
                        indicesOfMatches.Add(i);
                    }
                }
            }
            return indicesOfMatches.ToArray();
        }

        private List<int> getKeywordMatcheIndices(string value)
        {
            var indicesOfMatches = new List<int>();


            // search for keyword
            for (int i = 0; i < StaticData.getRowCount(); i++)
            {
                string ISBN = StaticData.getMatrixValue(i, 0);
                string CRN = StaticData.getMatrixValue(i, 7);
                string title = StaticData.getMatrixValue(i, 1).ToLower();
                string author = StaticData.getMatrixValue(i, 2).ToLower();
                string description = StaticData.getMatrixValue(i, 17).ToLower();


                if (!indicesOfMatches.Contains(i))
                {
                    // description search

                    if (description.Contains(value))
                    {
                        indicesOfMatches.Add(i);
                    }

                    // title search
                    if (!indicesOfMatches.Contains(i))
                    {
                        if (title.Contains(value))
                        {
                            indicesOfMatches.Add(i);
                        }
                    }

                    // author search
                    if (!indicesOfMatches.Contains(i))
                    {
                        if (author.Contains(value))
                        {
                            indicesOfMatches.Add(i);
                        }
                    }

                    // ISBN search
                    if (!indicesOfMatches.Contains(i))
                    {
                        if (ISBN.Contains(value))
                        {
                            indicesOfMatches.Add(i);
                        }
                    }

                    // CRN search
                    if (!indicesOfMatches.Contains(i))
                    {
                        if (CRN.Contains(value))
                        {
                            indicesOfMatches.Add(i);
                        }
                    }
                }
            }
            return indicesOfMatches;
        }


        /**
         * 
         * 
        */
        private void setGridTable(int[] rows)
        {
            //create DataTable & DataRow
            DataTable dt = new DataTable();
            DataRow dr = null;

            //Adds columns to DataTable
            dt.Columns.Add("image");
            dt.Columns.Add(new DataColumn("Title", typeof(string)));
            dt.Columns.Add(new DataColumn("Author", typeof(string)));
            dt.Columns.Add(new DataColumn("ISBN", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Format", typeof(string)));
            dt.Columns.Add(new DataColumn("New", typeof(string)));
            dt.Columns.Add(new DataColumn("Used", typeof(string)));
            dt.Columns.Add(new DataColumn("Rental", typeof(string)));
            dt.Columns.Add(new DataColumn("eBook", typeof(string)));

            //add elements to DataRow
            for (int i = 0; i < rows.Length; i++)
            {
                string ebookAvailability = StaticData.getMatrixValue(rows[i], 12);
                string description = StaticData.getMatrixValue(rows[i], 17);   //gets whole book description


                //changes eBook availability string to "Yes"
                if (Int32.Parse(ebookAvailability) >= 99999)
                {
                    ebookAvailability = "In-Stock";
                }
                else
                {
                    ebookAvailability = "Not In-Stock";
                }

                //cuts description to < 260 chars
                if (description.Length > 260)
                {
                    description = description.Substring(0, 260);
                    int endIndex = description.LastIndexOf(' ');
                    description = description.Substring(0, endIndex) + "...";
                }

                //fills row data
                dr = dt.NewRow();
                dr["image"] = "/Images/cart.png";
                dr["Title"] = StaticData.getMatrixValue(rows[i], 1) + "<br>";
                dr["Author"] = StaticData.getMatrixValue(rows[i], 2);
                dr["ISBN"] = StaticData.getMatrixValue(rows[i], 0);
                dr["Description"] = description;
                dr["Format"] = "  Price:" + "<br>" + "<br>" + "  Quantity:";
                dr["New"] = "$" + StaticData.getMatrixValue(rows[i], 13) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + StaticData.getMatrixValue(rows[i], 9) + ")";
                dr["Used"] = "$" + StaticData.getMatrixValue(rows[i], 14) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + StaticData.getMatrixValue(rows[i], 10) + ")";
                dr["Rental"] = "$" + StaticData.getMatrixValue(rows[i], 15) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + StaticData.getMatrixValue(rows[i], 11) + ")";
                dr["eBook"] = "$" + StaticData.getMatrixValue(rows[i], 16) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + ebookAvailability + ")"; // StaticData.getMatrixValue(rows[i], 12);

                dt.Rows.Add(dr);
            }

            ViewState["CurrentTable"] = dt;

            setHeaderText(rows.Length);

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private void setHeaderText(int rows)
        {
            if (rows <= 10)
            {
                SearchHeaderLabel.Text = "Showing 1-" + rows + " of " + rows + " results for \"" + Request.QueryString["Value"] + "\"";
            }
            else
            {
                SearchHeaderLabel.Text = "Showing 1-10 of " + rows + " results for \"" + Request.QueryString["Value"] + "\"";
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            // this.GridView1.PageIndex = (this.GridView1.PageCount - 3);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            setGridTable(searchData());
        }
    }
}