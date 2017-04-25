using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore
{
    public class Cart
    {
        public const decimal TAXRATE = 7;
        public const decimal SHIPPING = 14.99M;

        public List<LineItem> cartList;                               

        public decimal subTotal;   //calculated each time book is added or removed from cart

        public decimal tax;        //calculated on loading the cart page
        public decimal total;      //calculated on loading the cart page
        public decimal shipping;   //calculated on loading the cart page

        public Cart () {    //constructor
            cartList = new List<LineItem>();        
        }

        //add an item to the cart and update subtotal
        //returns -1 if trying to add an eBook when eBook was already purchased
        //returns -2 if trying to add more of a book to cart than exists in stock
        //returns 0 otherwise
        public int AddToCart(int rowNumber, int format, int quantity)
        {
            int result = 0;
            decimal price;

            //search cartList for the item to be added
            bool foundLineItem = false;
            int i = 0;
            while ((i < cartList.Count) && (!foundLineItem))
            {
                //if current LineItem matches one already in the Cart, just increment the quantity.
                if ((cartList[i].rowNumber == rowNumber) && 
                    (cartList[i].format == format))
                {
                    foundLineItem = true;
                    if (cartList[i].format == LineItem.EBOOK)  //but don't increment if its an ebook
                    {
                        result = -1;                //in case we need to warn the user that they've already added this book to the cart.
                    }
                    else
                    {
                        int quantityInStock = StaticData.convertToInt(cartList[i].rowNumber, cartList[i].format + StaticData.QUANTITY_NEW);
                        if (cartList[i].quantity > quantityInStock)
                        {
                            result = -2;    //Error!  Trying to add more items to the cart than exist in stock.
                        }
                        else
                        {
                            cartList[i].quantity += quantity;
                        }
                        
                    }                   
                }
                i++;
            }

            //if we didn't find a match, add the line item to the cart.
            if (!foundLineItem)
            {
                price = Convert.ToDecimal(StaticData.getMatrixValue(rowNumber, format + StaticData.PRICE_NEW));
                cartList.Add(new LineItem(rowNumber, format, quantity, price));
            }

            RecalcSubtotal();

            return result;
        }


        //remove an item from the cart and recalculate subtotal
        //returns 0 if cart updated successfully
        //returns -1 if item not found.  *this should never happen*
        //returns -2 if trying to remove more of an item from the cart than is currently in the cart.
        public int RemoveFromCart(int rowNumber, int format, int quantityToRemove)
        {
            bool removingTooMuch = false;
            bool foundLineItem = false;
            int i = 0;

            //search cartList for the item to be removed
            while ((i < cartList.Count) && (!foundLineItem))
            {
                if ((cartList[i].rowNumber == rowNumber) &&
                    (cartList[i].format == format))
                {
                    foundLineItem = true;
                    if (cartList[i].quantity == quantityToRemove)
                    {
                        cartList.Remove(cartList[i]);
                    }
                    else if (cartList[i].quantity < quantityToRemove)
                    {
                        removingTooMuch = true;
                    }
                    else
                    {
                        cartList[i].quantity -= quantityToRemove;
                    }                  
                }
                i++;
            }
            RecalcSubtotal();

            if (removingTooMuch)
                return -2;              //trying to remove too much
            else if (!foundLineItem)
                return -1;              //item not found
            else
                return 0;               //success
        }

        //calc tax, shipping and total
        public void calcTotal()
        {
            bool hasShipping = false;
            int i = 0;

            while (i < cartList.Count && !hasShipping)
            {
                if (cartList[i].format != LineItem.EBOOK)
                {
                    hasShipping = true;
                }
                i++;
            }

            tax = TAXRATE * subTotal / 100;
            total = subTotal + tax;
            if (hasShipping)
            {
                shipping = SHIPPING;
                total += SHIPPING;
            }
            else
            {
                shipping = 0;
            }
        }

        public void RecalcSubtotal ()
        {          
            subTotal = 0;
            for (int i = 0; i < cartList.Count; i++)
            {
                subTotal += cartList[i].price * cartList[i].quantity;
            }
        }       

        public int GetNumOfItems()
        {
            int numOfItems = 0;
            for (int i = 0; i < cartList.Count; i++)
            {
                numOfItems += cartList[i].quantity;
            }
            return numOfItems;
        }
    }
}