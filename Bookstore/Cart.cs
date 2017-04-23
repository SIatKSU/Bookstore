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

        public Cart () {    //constructor
            cartList = new List<LineItem>();        
        }

        //add an item to the cart and update subtotal
        //returns -1 if trying to add an eBook when eBook was already purchased
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
                        cartList[i].quantity += quantity;
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
        //returns true if cart updated successfully
        //returns false if item not found.  *this should never happen*
        public bool RemoveFromCart(int rowNumber, int format, int quantityToRemove)
        {
            //search cartList for the item to be removed
            bool foundLineItem = false;
            int i = 0;
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
                    else
                    {
                        cartList[i].quantity -= quantityToRemove;
                    }                  
                }
                i++;
            }
            RecalcSubtotal();

            return foundLineItem;       //return true if we found and removed an item (should always be the case)
        }

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

            total = subTotal + TAXRATE * subTotal;
            if (hasShipping)
            {
                total += SHIPPING;
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