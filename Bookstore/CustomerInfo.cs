using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore
{
    public class CustomerInfo
    {
        public string FullName;
        public string Email;
        public string PhoneNumber;

        public string BillingStreet;
        public string BillingCity;
        public string BillingState;
        public string BillingZip;

        public string ShippingStreet;
        public string ShippingCity;
        public string ShippingState;
        public string ShippingZip;

        public string PaymentMethod;

        //credit card
        public string CardNumber;
        public string ExpirationMonth;
        public string ExpirationYear;

        //paypal
        public string PayPalEmail;

        //KSU login
        public string KSUlogin;
    }
}