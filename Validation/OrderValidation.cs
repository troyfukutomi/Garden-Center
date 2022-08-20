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
    public class OrderValidation
    {
        private readonly DatabaseContext _context;

        public OrderValidation(DatabaseContext context)
        {
            _context = context;
        }

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


        public bool validDate(string date)
        {
            Regex dateRegex = new Regex(@"^\d{4}-((0[1-9])|(1[012]))-((0[1-9]|[12]\d)|3[01])$");
            if (dateRegex.IsMatch(date))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool validTotal(decimal orderTotal)
        {
            Regex totalRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");
            if (totalRegex.IsMatch(orderTotal.ToString()))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool validQuantity(int quantity)
        {
            if (quantity !<= 0)
            {
                return false;
            } else
            {
                return true;
            }
        }
          
        public bool orderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}