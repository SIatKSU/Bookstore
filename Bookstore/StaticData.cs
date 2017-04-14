using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

/// <summary>
/// Summary description for StaticData
/// </summary>
public static class StaticData
{
    private static List<string> newLines;
    private static string fileName = Constants.booksFile;

    // matrix directory (column # - column name)
    // 0-ISBN  1-Title  2-Author 3-Semester 4-Course 5-#ofSections 6-Professor 7-CRN 8-Required/Not
    // 9-New Quantity 10-Used Quantity 11-Rental Quantity 12-Ebook Availability 
    // 13-New Price 14-Used Price 15-Rental Price 16-Ebook Price 17-Description
    private static string[,] matrix;

    public static void readFile()
    {
        readLines(); // populates newLines list
        matrix = new string[newLines.Count, 18];
        int rows = newLines.Count;

        //populates matrix
        for (int i = 0; i < rows; i++)
        {
            populateArray(newLines[i], i);
        }

        /* PRINT MATRIX
        Debug.WriteLine("\n\nPRINTING...");
        for (int i = 0; i < rows; i++)
        {
            Debug.WriteLine(matrix[i, 0]);
        } */
    }

    private static void readLines()
    {
        //populates line array with original (unformatted) lines from input CSV file.
        string[] lines = File.ReadAllLines(fileName);

        // stores formatted lines from input CSV file
        newLines = new List<string>();
        int newIndex = -1;
        string temp = "";

        /**
         * Checks if a line doesn't start with an ISBN. 
         * If not, the current line is added to the end of the previous line
         * and the lines array  is adjusted
         */
        for (int i = 0; i < lines.Length; i++)
        {
            string str = "";
            if (lines[i].Length >= 3)
            {
                str = lines[i].Substring(0, 3);
            }

            if (str == "978" || str == "979")
            {
                newIndex++;
                temp = lines[i];
                newLines.Add(lines[i]);
            }
            else
            {
                // adds new line character whenever there is one in the file. 
                // This is so the description could be the exact same in the file and on the site.
                // temp = temp + Environment.NewLine + lines[i];
                temp = temp + " " + lines[i];
                newLines[newIndex] = temp;
            }
        }
    }

    private static void populateArray(string str, int row)
    {
        int col = 0;
        string temp = "";
        char curr = '`', prev = '`', next = '`';
        bool quot = false;

        for (int i = 0; i < str.Length; i++)
        {
            curr = str[i];
            if (i >= 1) { prev = str[i - 1]; } // sets prev char
            if (i < str.Length - 1) { next = str[i + 1]; } // sets next char   

            if (curr == '"' && prev == ',') // start of quote
            {
                quot = true;
            }
            else if ((quot && curr == '"' && next == ',') ||
                (quot && curr == ',' && prev == '"')) // end of quote
            {
                quot = false;
                matrix[row, col] = temp;
                col++;
                temp = "";
            }
            else if (quot && curr == '"' && next == '"' && prev == ' ')
            {
                //do nothing
            }
            else if (quot && curr == '"' && prev == '"' && next != ' ')
            {
                temp = temp + curr;
            }
            else if (quot && curr == '"' && next == '"' && prev != ' ')
            {
                temp = temp + curr;
            }
            else if (quot && curr == '"' && prev == '"' && next == ' ')
            {
                //do nothing
            }
            else if (quot && curr != '"')
            {
                temp = temp + curr;
            }
            else if (!quot && curr != '"' && curr != ',') // always last
            {
                string s1 = prev.ToString();
                string s2 = curr.ToString();
                string s3 = next.ToString();
                string s4 = s1 + s2 + s3;
                temp = temp + curr;
            }
            else if (curr == ',' && prev == '"' && next == '"')
            {
                quot = true;
            }
            else if (curr == ',' && prev == '"' && next != '"')
            {
                quot = false;
            }
            else if (!quot && curr == ',') // end of non-quote
            {
                matrix[row, col] = temp;
                col++;
                temp = "";
            }
            else
            {
                Debug.WriteLine("SOMETHING WENT WRONT WHILE PARSING FILE: " + prev + curr + next + quot);
            }

            if (i == (str.Length - 1))
            {
                matrix[row, col] = temp;
                col++;
                temp = "";
            }
        }
    }

    public static string[,] getMatrix()
    {
        return matrix;
    }

    public static string[] getMatrixRow(int row)
    {
        string[] rowArray = new string[newLines.Count];
        for (int i = 0; i < newLines.Count; i++)
        {
            rowArray[i] = matrix[row, i];
        }
        return rowArray;
    }

    public static string[] getMatrixColumn(int col)
    {
        string[] colArray = new string[newLines.Count];
        for (int i = 0; i < newLines.Count; i++)
        {
            colArray[i] = matrix[i, col];
        }
        return colArray;
    }

    public static string getMatrixValue(int row, int col)
    {
        return matrix[row, col];
    }

    public static int getRowCount()
    {
        return newLines.Count;
    }
}