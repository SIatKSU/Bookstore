using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore
{
    //note:
    //we don't have to use this - I know we could just use a list of strings instead.
    //also, if there is a better place to put this class, feel free to move it!


    //this is a line item from the shopping cart; a list of LineItems will be passed by Session state from page to page.
    public class LineItem
    {
        public const int NEW = 0;
        public const int USED = 1;
        public const int RENTAL = 2;
        public const int EBOOK = 3;

        public int rowNumber;          //identifies book by row number, from books.csv
        public int format;             //NEW, USED, RENTAL, EBOOK
        public int quantity;

        public LineItem (int rowNumber, int format, int quantity)
        {
            this.rowNumber = rowNumber;
            this.format = format;
            this.quantity = quantity;
        }
    }
}