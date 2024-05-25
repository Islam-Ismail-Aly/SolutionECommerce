using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketoo.Application.DTOs.Product
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public string Image { get; set; }
        [Range(0, 5)]
        public double Rating { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Category { get; set; }
    }

}
