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
            populateSortList();
            type = Request.QueryString["Type"].ToLower();
            value = Request.QueryString["Value"].ToLower();

            GridView1.PageIndex = 0; //sets default GridView1 page to the first page
            setGridTable(searchData("ISBN"));
        }

        // Populates sortList
        private void populateSortList()
        {
            if (!IsPostBack)
            {
                ListItem isbnItem = new ListItem("ISBN");
                ListItem titleItem = new ListItem("Title");
                ListItem authorItem = new ListItem("Author");
                ListItem courseItem = new ListItem("Course");

                SortList.Items.Add(isbnItem);
                SortList.Items.Add(titleItem);
                SortList.Items.Add(authorItem);
                SortList.Items.Add(courseItem);
            }
        }

        // returns array with row indicies that contained the search value
        private int[] searchData(string sortBy)
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

            // sort list
            return sortList(indicesOfMatches, sortBy);
        }

        private int[] sortList(List<int> list, String selectedSort)
        {
            int selectedCol;

            //set selectedColumn
            switch (selectedSort)
            {
                case "Title":
                    selectedCol = 1;
                    break;
                case "Course":
                    selectedCol = 4;
                    break;
                case "Author":
                    selectedCol = 2;
                    break;
                case "ISBN":
                    selectedCol = 0;
                    break;
                default:
                    selectedCol = 0;
                    break;
            }

            List<string> sortValueList = new List<string>();


            for (int i = 0; i < list.Count; i++)
            {
                sortValueList.Add(StaticData.getMatrixValue(list[i], selectedCol));
            }

            //sort the values list alphabetically
            sortValueList = sortValueList.OrderBy(q => q).ToList();

            List<int> sortedIndexList = new List<int>();

            for (int i = 0; i < sortValueList.Count; i++)
            {
                for (int j = 0; j < StaticData.getMatrixColumn(0).Length; j++)
                {
                    if (sortValueList[i] == StaticData.getMatrixValue(j, selectedCol))
                    {
                        sortedIndexList.Add(j);
                    }
                }
            }

            return sortedIndexList.ToArray();
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
                string description = StaticData.getMatrixValue(rows[i], 17);   //gets whole book description

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


                int newQuantity = StaticData.convertToInt(rows[i], StaticData.QUANTITY_NEW);
                if (newQuantity > 0)
                {
                    string newPriceStr = String.Format("{0:C}", Convert.ToDecimal(StaticData.getMatrixValue(rows[i], StaticData.PRICE_NEW)));
                    dr["New"] = newPriceStr + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + StaticData.getMatrixValue(rows[i], 9) + ")";
                }
                else
                {
                    dr["New"] = "Not In-Stock";
                }

                int usedQuantity = StaticData.convertToInt(rows[i], StaticData.QUANTITY_USED);
                if (usedQuantity > 0)
                {
                    string usedPriceStr = String.Format("{0:C}", Convert.ToDecimal(StaticData.getMatrixValue(rows[i], StaticData.PRICE_USED)));
                    dr["Used"] = usedPriceStr + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + StaticData.getMatrixValue(rows[i], 10) + ")";
                }
                else
                {
                    dr["Used"] = "Not In-Stock";
                }
                
                int rentalQuantity = StaticData.convertToInt(rows[i], StaticData.QUANTITY_RENTAL);
                if (rentalQuantity > 0)
                {
                    string rentalPriceStr = String.Format("{0:C}", Convert.ToDecimal(StaticData.getMatrixValue(rows[i], StaticData.PRICE_RENTAL)));
                    dr["Rental"] = rentalPriceStr + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + StaticData.getMatrixValue(rows[i], 11) + ")";
                }
                else
                {
                    dr["Rental"] = "Not In-Stock";
                }

                string ebookAvailability = StaticData.getMatrixValue(rows[i], 12);
                //check eBook availability
                if (Int32.Parse(ebookAvailability) >= 99999)
                {
                    ebookAvailability = "In-Stock";
                    string eBookPriceStr = String.Format("{0:C}", Convert.ToDecimal(StaticData.getMatrixValue(rows[i], StaticData.PRICE_EBOOK)));
                    dr["eBook"] = eBookPriceStr + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + ebookAvailability + ")"; 
                }
                else
                {
                    dr["eBook"] = "Not In-Stock";
                }
            
                //dr["New"] = "$" + StaticData.getMatrixValue(rows[i], 13) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + StaticData.getMatrixValue(rows[i], 9) + ")";
                //dr["Used"] = "$" + StaticData.getMatrixValue(rows[i], 14) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + StaticData.getMatrixValue(rows[i], 10) + ")";
                //dr["Rental"] = "$" + StaticData.getMatrixValue(rows[i], 15) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + StaticData.getMatrixValue(rows[i], 11) + ")";
                //dr["eBook"] = "$" + StaticData.getMatrixValue(rows[i], 16) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Quantity: " + ebookAvailability + ")"; // StaticData.getMatrixValue(rows[i], 12);

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

        protected void SortList_SelectedIndexChanged(object sender, EventArgs e)
        {
            setGridTable(searchData(SortList.SelectedValue));
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            setGridTable(searchData(SortList.SelectedValue));
        }
    }
}