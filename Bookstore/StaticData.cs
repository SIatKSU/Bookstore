using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Web;

public static class StaticData
{

    public const int NUM_COLUMNS = 18;


    public const int ISBN = 0;
    public const int TITLE = 1;
    public const int AUTHOR = 2;
    public const int SEMESTER = 3;
    public const int COURSE = 4;
    public const int NUM_SECTIONS = 5;
    public const int PROFESSOR = 6;
    public const int CRN = 7;
    public const int REQUIRED = 8;
    public const int QUANTITY_NEW = 9;
    public const int QUANTITY_USED = 10;
    public const int QUANTITY_RENTAL = 11;
    public const int EBOOK_AVAIL = 12;
    public const int PRICE_NEW = 13;
    public const int PRICE_USED = 14;
    public const int PRICE_RENTAL = 15;
    public const int PRICE_EBOOK = 16;
    public const int DESCRIPTION = 17;


    public static string appPath = HttpRuntime.AppDomainAppPath;
    private static string fileName = appPath + "/books.csv";

    //private static string testFileName = appPath + "/testBooks.csv";
    //private static string fileName = testFileName;

    // matrix directory (column # - column name)
    // 0-ISBN  1-Title  2-Author 3-Semester 4-Course 5-#ofSections 6-Professor 7-CRN 8-Required/Not
    // 9-New Quantity 10-Used Quantity 11-Rental Quantity 12-Ebook Availability 
    // 13-New Price 14-Used Price 15-Rental Price 16-Ebook Price 17-Description
    private static string[][] matrix;

    public static void readFile()
    {
        string[] lines = File.ReadAllLines(fileName);

        List<string[]> matrixList = new List<string[]>();

        string[] singleRow = new string[NUM_COLUMNS]; //a single row to be added to the matrixList
        int colIndex = 0;           //0-17.  track which column we are filling in
        bool openQuote = false;     //if there is an open quotation mark, keep appending to current cell in current row.
        int numQuotes = 0;          //temp variable to count the evil quotation marks

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length == 0)
            {
                singleRow[colIndex] = singleRow[colIndex] + Environment.NewLine;     //if the line is empty, just append 2 new lines to the current field and move on
            }
            else
            {
                string[] commaSplitRow = lines[i].Split(',');

                for (int j = 0; j < commaSplitRow.Length; j++)
                {

                    if (colIndex == 0)
                    {
                        singleRow = new string[NUM_COLUMNS];    //create a new row
                        matrixList.Add(singleRow);              //and add it to the matrix list

                        singleRow[0] = commaSplitRow[0];
                        colIndex++;
                    }
                    else
                    {
                        if (openQuote)
                        {
                            if (j != 0) //don't do this if this is the first element in the line.
                            {
                                singleRow[colIndex] += ","; //if the field is a continuation of the previous field, first append a ',' (unless it's col[0].)
                            }
                            else
                            {
                                //preserve newlines in the description, but not for the other fields.
                                if (colIndex != DESCRIPTION)
                                    singleRow[colIndex] += " ";
                                else
                                    singleRow[colIndex] += Environment.NewLine;
                            }
                        }

                        if (commaSplitRow[j][0] == '"')
                        {
                            if (commaSplitRow[j][commaSplitRow[j].Length - 1] == '"')     //first and last elements are "          example: "Spring 2015"
                            {
                                singleRow[colIndex] += commaSplitRow[j].Substring(1, commaSplitRow[j].Length - 2);
                            }
                            else
                            {
                                singleRow[colIndex] += commaSplitRow[j].Substring(1, commaSplitRow[j].Length - 1);    //only first element is "
                            }
                        }
                        else
                        {
                            if (openQuote && commaSplitRow[j][commaSplitRow[j].Length - 1] == '"')              //only last element is "
                            {
                                singleRow[colIndex] += commaSplitRow[j].Substring(0, commaSplitRow[j].Length - 1);     //remove last ''
                            }
                            else
                            {
                                singleRow[colIndex] += commaSplitRow[j];
                            }
                        }

                        //count the evil quotation marks
                        numQuotes = GetHowManyTimeOccurenceCharInString(commaSplitRow[j], '"');

                        if (numQuotes % 2 == 1)
                        {
                            openQuote = !openQuote;  //if there are an odd number of quotation marks, flip the openQuote flag
                        }

                        if (!openQuote) //if no open quotation marks, move to the next column.  Otherwise, we'll keep adding to this column
                        {
                            if (colIndex != NUM_COLUMNS - 1)
                            {
                                colIndex++;
                            }
                            else
                            {
                                colIndex = 0;
                            }
                        }
                    }

                }
            }

        }
        matrix = new string[matrixList.Count][];

        for (int i = 0; i < matrixList.Count; i++)
        {
            matrix[i] = new string[NUM_COLUMNS];

            for (int j = 0; j < NUM_COLUMNS; j++)
            {
                //Console.WriteLine(matrixList[i][j]);
                matrix[i][j] = matrixList[i][j];
            }
        }

    }

    public static void outputMatrix()
    {
        Console.WriteLine("Number of books in matrix: " + matrix.Length);
        Console.WriteLine();


        for (int i = 0; i < matrix.Length; i++)
        {
            Console.WriteLine("Row: " + i);
            Console.WriteLine("Row: " + i);
            Console.WriteLine("Row: " + i);
            for (int j = 0; j < NUM_COLUMNS; j++)
            {
                Console.WriteLine(matrix[i][j]);
            }
            Console.ReadKey();
        }
    }

    //debugging method. 
    //list all the lines that don't start with ISBN - what do they look like?
    public static void listInconsistentLines()
    {
        string[] lines = File.ReadAllLines(fileName);

        List<string[]> badList = new List<string[]>();
        string str;
        string outputStr;

        string badLinesFile = appPath + "/badLines.txt";

        using (FileStream fs = new FileStream(badLinesFile, FileMode.OpenOrCreate))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {

                for (int i = 0; i < lines.Length; i++)
                {

                    if (lines[i].Length == 0)
                    {
                        outputStr = "Line " + i + ": Empty line found.";
                        Console.WriteLine(outputStr);
                        sw.WriteLine(outputStr);

                    }
                    else if (lines[i].Length >= 3)
                    {
                        str = lines[i].Substring(0, 3);
                        if (str != "978" || str == "979")
                        {
                            outputStr = "Line " + i + ": \n" + lines[i] + "\n";
                            Console.Write(outputStr);
                            sw.Write(outputStr);
                        }
                    }
                }
            }
        }
        Console.ReadKey();
    }

    //http://stackoverflow.com/questions/5340564/counting-how-many-times-a-certain-char-appears-in-a-string-before-any-other-char
    //by Mehdi Bugnard
    public static int GetHowManyTimeOccurenceCharInString(string text, char c)
    {
        int count = 0;
        foreach (char ch in text)
        {
            if (ch.Equals(c))
            {
                count++;
            }
        }
        return count;
    }



    //write the matrix back to the books.csv file.
    //currently, this is done when customer successfully makes a purchase.
    public static void writeFile()
    {
        //18 fields.
        string[] line = new string[18];

        //if it contains space, comma, quotation mark, put a " " around it
        char[] checkChars = { ' ', ',' }; //, '\"' };

        //string testFileName = appPath + "/booksWriteTest.csv";
        //System.IO.StreamWriter sw = new System.IO.StreamWriter(testFileName);

        using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {

                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[j].Length; j++)
                    {
                        //if it contains space, comma, quotation mark, put a " on it!
                        if (matrix[i][j].IndexOfAny(checkChars) != -1)
                        {
                            line[j] = "\"" + matrix[i][j] + "\"";
                        }
                        else
                        {
                            line[j] = matrix[i][j];
                        }
                    }

                    sw.WriteLine(string.Join(",", line));
                }

            }
        }
    }

    public static string[][] getMatrix()
    {
        return matrix;
    }

    public static string[] getMatrixRow(int row)
    {
        return matrix[row];
    }

    public static string[] getMatrixColumn(int col)
    {
        string[] colArray = new string[matrix.Length];
        for (int i = 0; i < matrix.Length; i++)
        {
            colArray[i] = matrix[i][col];
        }
        return colArray;
    }

    public static string getMatrixValue(int row, int col)
    {
        return matrix[row][col];
    }

    public static void setMatrixValue(int row, int col, string newValue)
    {
        matrix[row][col] = newValue;
    }

    public static int getRowCount()
    {
        return matrix.Length;
    }

    //convert a cell's string value to int.
    //if the value cannot be converted, i.e. "inf", return 0.
    public static int convertToInt(int row, int col)
    {
        int result;
        bool valid = int.TryParse(matrix[row][col], out result);

        return valid ? result : 0;
    }
}
