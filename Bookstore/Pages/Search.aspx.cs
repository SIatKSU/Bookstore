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

        // 
        //
        // TODO: add multiple word (string) search
        // TODO: make everything lowercase
        //
        //
        private int[] searchData(string type, string value)
        {
            var indicesOfMatches = new List<int>();

            if (type == "Keyword")
            {
                indicesOfMatches = getKeywordMatcheIndices(value);
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

        private List<int> getKeywordMatcheIndices(string value)
        {
            var indicesOfMatches = new List<int>();


            // search for keyword
            for (int i = 0; i < StaticData.getRowCount(); i++)
            {
                string ISBN = StaticData.getMatrixValue(i, 0);
                string title = StaticData.getMatrixValue(i, 1);
                string authors = StaticData.getMatrixValue(i, 2);
                string description = StaticData.getMatrixValue(i, 17);

                string[] delimiters = new string[] { " " };
                string[] descriptionWords = description.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                string[] titleWords = description.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                string[] authorWords = description.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);


                if (!indicesOfMatches.Contains(i))
                {
                    // description search
                    for (int j = 0; j < descriptionWords.Length; j++)
                    {
                        if (descriptionWords[j] == value)
                        {
                            indicesOfMatches.Add(i);
                            break;
                        }
                    }

                    // title search
                    if (!indicesOfMatches.Contains(i))
                    {
                        for (int j = 0; j < titleWords.Length; j++)
                        {
                            if (titleWords[j] == value)
                            {
                                indicesOfMatches.Add(i);
                                break;

                            }
                        }
                    }

                    // author search
                    if (!indicesOfMatches.Contains(i))
                    {
                        for (int j = 0; j < authorWords.Length; j++)
                        {
                            if (authorWords[j] == value)
                            {
                                indicesOfMatches.Add(i);
                                break;

                            }
                        }
                    }

                    // ISBN search
                    if (!indicesOfMatches.Contains(i))
                    {
                        if (ISBN == value)
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
                    ebookAvailability = "Yes";
                }
                else
                {
                    ebookAvailability = "No";
                }

                //cuts description to < 160 chars
                if (description.Length > 150)
                {
                    description = description.Substring(0, 160);
                    int endIndex = description.LastIndexOf(' ');
                    description = description.Substring(0, endIndex) + "...";
                }

                //fills row data
                dr = dt.NewRow();
                dr["image"] = "/Images/cart.png";
                dr["Title"] = StaticData.getMatrixValue(rows[i], 1);
                dr["Author"] = StaticData.getMatrixValue(rows[i], 2);
                dr["ISBN"] = StaticData.getMatrixValue(rows[i], 0);
                dr["Description"] = description;
                dr["Format"] = "  Price:" + "<br>" + "<br>" + "  Quantity:";
                dr["New"] = "$" + StaticData.getMatrixValue(rows[i], 13) + "<br>" + "<br>" + StaticData.getMatrixValue(rows[i], 9);
                dr["Used"] = "$" + StaticData.getMatrixValue(rows[i], 14) + "<br>" + "<br>" + StaticData.getMatrixValue(rows[i], 10);
                dr["Rental"] = "$" + StaticData.getMatrixValue(rows[i], 15) + "<br>" + "<br>" + StaticData.getMatrixValue(rows[i], 11);
                dr["eBook"] = "$" + StaticData.getMatrixValue(rows[i], 16) + "<br>" + "<br>" + ebookAvailability; // StaticData.getMatrixValue(rows[i], 12);

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