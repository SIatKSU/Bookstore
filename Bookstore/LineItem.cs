using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore
{
    //this is a line item from the shopping cart.
    public class LineItem
    {
        public const int NEW = 0;
        public const int USED = 1;
        public const int RENTAL = 2;
        public const int EBOOK = 3;

        public int rowNumber;          //identifies book by row number, from books.csv
        public int format;             //NEW, USED, RENTAL, EBOOK
        public int quantity;
        public decimal price;

        public LineItem(int rowNumber, int format, int quantity, decimal price)
        {
            this.rowNumber = rowNumber;
            this.format = format;
            this.quantity = quantity;
            this.price = price;
        }
    }
}