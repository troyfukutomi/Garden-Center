using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GardenCenter.Models;

namespace GardenCenter.Validation
{
    /// <summary>
    /// Validation methods for orders are stored here
    /// </summary>
    public class OrderValidation
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Brings in the database for us to use in our validation class
        /// </summary>
        /// <param name="context"></param>
        public OrderValidation(DatabaseContext context)
        {
            _context = context;
        }

        GardenCenter.Logging.Logger logger = new GardenCenter.Logging.Logger();

        /// <summary>
        /// gets orders, can take in optional parameters that can be used to query.
        /// </summary>
        /// <param name="customerId">id of the customer who owns this order </param>
        /// <param name="date">date the order was made</param>
        /// <param name="orderTotal">total price of the order</param>
        /// <param name="productId">product beig ordered</param>
        /// <param name="quantity"> quantity of the product being ordered</param>
        /// <param name="orders">list of orders</param>
        /// <returns>list of orders</returns>
        public List<Order> getOrders(int customerId, string? date, decimal orderTotal, int productId, int quantity, List<Order> orders)
        {
            foreach (var o in orders.ToList())
            {
                if (customerId != 0 && customerId > 0 && customerId != o.CustomerId)
                {
                    orders.Remove(o);
                }
                if (date != null && date != o.Date)
                {
                    orders.Remove(o);
                }
                if (orderTotal != 0 && orderTotal > 0 && orderTotal != o.OrderTotal)
                {
                    orders.Remove(o);
                }
                if (productId != 0 && productId > 0 && productId != o.Items!.ProductId)
                {
                    orders.Remove(o);
                }
                if (quantity != 0 && quantity > 0 && quantity != o.Items!.Quantity)
                {
                    orders.Remove(o);
                }
            }
            return orders;
        }

        /// <summary>
        /// checks to make sure id in the query matches the id of the order being updated
        /// </summary>
        /// <param name="id">id in the query</param>
        /// <param name="order">order being updated</param>
        /// <returns>true if ids match</returns>
        public bool matchingIds(long id, Order order)
        {
            if (id == order.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// checks that the date is in correct format using a regex
        /// </summary>
        /// <param name="date">date being checked</param>
        /// <returns>true if date is valid format</returns>
        public bool validDate(string date)
        {
            Regex dateRegex = new Regex(@"^\d{4}-((0[1-9])|(1[012]))-((0[1-9]|[12]\d)|3[01])$");
            if (dateRegex.IsMatch(date))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// checks to make sure the total has exactly 2 places after decimal point
        /// </summary>
        /// <param name="orderTotal"></param>
        /// <returns>true if total is valid format</returns>
        public bool validTotal(decimal orderTotal)
        {
            Regex totalRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");
            if (totalRegex.IsMatch(orderTotal.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// checks to make sure quantity is not negative or 0
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>true if quantity is a positive number</returns>
        public bool validQuantity(int quantity)
        {
            if (quantity! <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// checks to make sure order exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orders">list of all orders</param>
        /// <returns>true if order exists</returns>
        public bool orderExists(int id, List<Order> orders)
        {
            foreach (var o in orders)
            {
                if (id == o.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}