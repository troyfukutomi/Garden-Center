using System.Text.RegularExpressions;
using GardenCenter.Models;

namespace GardenCenter.Validation
{
    public class ProductValidation
    {

        private readonly DatabaseContext _context;

        public ProductValidation(DatabaseContext context)
        {
            _context = context;
        }

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

        public bool productExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}