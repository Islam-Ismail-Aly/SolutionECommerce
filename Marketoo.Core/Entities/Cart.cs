using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketoo.Core.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Product> CartItems { get; set; } = new List<Product>();
    }
}
