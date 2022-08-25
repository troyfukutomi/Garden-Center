using System.Text.RegularExpressions;
using GardenCenter.Models;

namespace GardenCenter.Validation
{
    /// <summary>
    /// Validation methods for orders are stored here
    /// </summary>
    public class ProductValidation
    {

        private readonly DatabaseContext _context;

        /// <summary>
        /// Brings in the database for us to use in our validation class
        /// </summary>
        /// <param name="context"></param>
        public ProductValidation(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// gets products, can take in optional parameters that can be used to query.
        /// </summary>
        /// <param name="sku">sku of the product</param>
        /// <param name="type">type of product</param>
        /// <param name="name">name of the product</param>
        /// <param name="manufacturer">manufacturer of the product</param>
        /// <param name="price">price of product</param>
        /// <param name="products">lsit of products</param>
        /// <returns>list of products</returns>
        public List<Product> getProducts(string? sku, string? type, string? name, string? manufacturer, decimal price, List<Product> products)
        {
            foreach (var p in products.ToList())
            {
                if (sku != null && sku != p.Sku)
                {
                    products.Remove(p);
                }
                if (type != null && type != p.Type)
                {
                    products.Remove(p);
                }
                if (name != null && name != p.Name)
                {
                    products.Remove(p);
                }
                if (manufacturer != null && manufacturer != p.Manufacturer)
                {
                    products.Remove(p);
                }
                if (price != 0 && price > 0 && price != p.Price)
                {
                    products.Remove(p);
                }
            }
            return products;
        }

        /// <summary>
        ///  checks to make sure id in the query matches the id of the product being updated
        /// </summary>
        /// <param name="id">id in the query</param>
        /// <param name="product">product being updated</param>
        /// <returns>true if they match</returns>
        public bool matchingIds(long id, Product product)
        {
            if (id == product.Id)
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// checks that the price has exactly 2 decimal places 
        /// </summary>
        /// <param name="price">price beingn checked</param>
        /// <returns>true if price has 2 decimal places</returns>
        public bool priceIsProperFormat(decimal price){

            Regex priceRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");
            if (priceRegex.IsMatch(price.ToString()))
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// checking to make sure the sku is not already taken by another product
        /// </summary>
        /// <param name="product">product being checked</param>
        /// <param name="products">lis tof products in the database to check against</param>
        /// <returns>true if the sku number is one of a kind</returns>
        public bool uniqueSku(Product product, List<Product>products)
        {
            foreach (var p in products)
            {
                if (p.Sku == product.Sku && p.Id != product.Id)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// checks i the database that the product does exist.
        /// </summary>
        /// <param name="id">id used to check if product exists</param>
        /// <returns>true if the product exists</returns>
        public bool productExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}