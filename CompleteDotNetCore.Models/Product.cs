using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CompleteDotNetCore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public string ISBN { get; set; } = "";

        [Required]
        public string Author { get; set; } = "";

        [Required]
        [DisplayName("List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Required]
        [DisplayName("Price (50+)")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Required]
        [DisplayName("Price (100+)")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        [DisplayName("Iamge Url")]
        public string IamgeUrl { get; set; } = "";

        // Foreign Keys (No annotation needed because "Id" is in name.)
        [Required]
        [DisplayName("Category Id")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        [DisplayName("Cover Type Id")]
        public int CoverTypeId { get; set; }
        public CoverType? CoverType { get; set; }
    }
}

