using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter a Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an Address")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        [Required(ErrorMessage = "Please enter a City")]
        public string City { get; set;  }

        public string State { get; set; }

        [Required(ErrorMessage = "Please enter a ZIP/Postcode")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Please enter a Country")]
        public string Country { get; set; }

        public bool GiftWrapped { get; set; }
    }
}
