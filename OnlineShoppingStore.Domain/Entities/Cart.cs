using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> linecollection = new List<CartLine>();
        
        public void AddItem(Product product, int Quantity)
        {
            CartLine line = linecollection.Where(p => p.Product.ProductID == product.ProductID).FirstOrDefault();
            if( line == null)
            {
                linecollection.Add(new CartLine { Product = product, Quantity = Quantity });
            }
            else
            {
                line.Quantity += Quantity;
            }

        }

        public void RemoveLine(Product product)
        {
            linecollection.RemoveAll(p => p.Product.ProductID == product.ProductID);
        }

        public decimal CalculateTotalPrice()
        {
            return linecollection.Sum(p => p.Product.Price * p.Quantity);
        }

        public IEnumerable<CartLine> Lines
        {
           get { return linecollection; }
        }

        public void clear()
        {
            linecollection.Clear();
        }
    }
}
