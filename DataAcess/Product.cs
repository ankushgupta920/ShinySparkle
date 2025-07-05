using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string? Name { get; set; }
        
        public string? ImageUrl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? OldPrice { get; set; }

        [NotMapped] // This property is calculated, so it should not be stored in DB
        public int? Discount => OldPrice > 0 ? (int)((OldPrice - Price) / OldPrice * 100) : 0;

        public string? MainImageUrl { get; set; } = "";

        public string? Description { get; set; } = "";

        [MaxLength(50)]
        public string? SkuCode { get; set; } = "";

        [Required, MaxLength(100)]
        public string? Category { get; set; }

        // Store additional images as a JSON string
        public string? AdditionalImagesJson { get; set; }

        [NotMapped]
        public List<string> AdditionalImages
        {
            get => string.IsNullOrEmpty(AdditionalImagesJson) ? new List<string>() : System.Text.Json.JsonSerializer.Deserialize<List<string>>(AdditionalImagesJson);
            set => AdditionalImagesJson = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }

}

