using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // Populates first DropDownList
        if (!IsPostBack)
        {
            ListItem keywordItem = new ListItem("Keyword", "Keyword");
            ListItem professorItem = new ListItem("Professor", "Professor");
            ListItem courseItem = new ListItem("Course", "Course");

            DDList1.Items.Add(keywordItem);
            DDList1.Items.Add(professorItem);
            DDList1.Items.Add(courseItem);
        }
    }


    //
    protected void searchClicked(object sender, EventArgs e)
    {
        if (getSearchString() == "Select Professor")
        {
            // display prompt
            ProfsError.Visible = true;
        }
        else if (getSearchString() == "Select Course")
        {
            // display prompt
            CoursesError.Visible = true;
        }
        else if (getSearchString() == "Empty")
        {
            SearchBoxError.Visible = true;
        }
        else
        {
            //this redirects to a URL with parameters
            // ex: pageName.aspx?PARAMETER=value&PARAMETER2=value
            Response.Redirect("Search.aspx" + getSearchString());
        }
    }

    // 
    private string getSearchString()
    {
        string str = "";

        if (DDList1.SelectedValue == "Professor" && DDList2.SelectedValue == "Select Professor")
        {
            str = "Select Professor";
        }
        else if (DDList1.SelectedValue == "Course" && DDList2.SelectedValue == "Select Course")
        {
            str = "Select Course";
        }
        else if (DDList1.SelectedValue == "Keyword" && (String.IsNullOrEmpty(SearchBox.Text) ||
            String.IsNullOrWhiteSpace(SearchBox.Text)))
        {
            str = "Empty";
        }
        else if (DDList1.SelectedValue == "Keyword")
        {
            str = str = "?Type=" + "Keyword" + "&Value=" + SearchBox.Text;
        }
        else if (DDList1.SelectedValue == "Professor")
        {
            str = "?Type=" + "Professor" + "&Value=" + DDList2.SelectedValue;
        }
        else if (DDList1.SelectedValue == "Course")
        {
            str = "?Type=" + "Course" + "&Value=" + DDList2.SelectedValue;

        }
        return str;
    }

    // select index changed for DDList1
    protected void DDList1SelectedIndexChanged(object sender, EventArgs e)
    {
        string value = DDList1.SelectedValue;

        CoursesError.Visible = false;
        ProfsError.Visible = false;
        SearchBoxError.Visible = false;

        if (value == "Keyword")
        {
            SearchBox.Visible = true;
            SearchButton.Visible = true;
            DDList2.Visible = false;
            SearchButton.Visible = true;
        }
        if (value == "Professor")
        {
            DDList2.Visible = true;
            SearchBox.Visible = false;
            DDList2.Items.Clear();
            DDList2.Items.AddRange(getProfessorListItems());   //load professor names
        }
        if (value == "Course")
        {
            DDList2.Visible = true;
            SearchBox.Visible = false;
            DDList2.Items.Clear();
            DDList2.Items.AddRange(getCourseListItems());   //load course names
        }
    }

    // select index changed for DDList2
    protected void DDList2SelectedIndexChanged(object sender, EventArgs e)
    {
        SearchButton.Visible = true;
        CoursesError.Visible = false;
        ProfsError.Visible = false;
    }

    // Load professor names from main matrix
    private ListItem[] getProfessorListItems()
    {
        var profsList = new List<ListItem>();
        string[] profsArray = StaticData.getMatrixColumn(6); // returns professors(6) column
        profsList.Add(new ListItem("Select Professor"));

        for (int i = 0; i < profsArray.Length; i++)
        {
            profsList.Add(new ListItem(profsArray[i]));
        }

        return profsList.Distinct().ToArray(); // removes duplicates
    }

    // Load course names from main matrix
    private ListItem[] getCourseListItems()
    {
        var coursesList = new List<ListItem>();

        string[] coursesArray = StaticData.getMatrixColumn(4); // returns courses(4) column
        coursesList.Add(new ListItem("Select Course"));

        for (int i = 0; i < coursesArray.Length; i++)
        {
            coursesList.Add(new ListItem(coursesArray[i]));
        }

        return coursesList.Distinct().ToArray();
    }
}