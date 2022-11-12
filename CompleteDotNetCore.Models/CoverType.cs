using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CompleteDotNetCore.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
    }
}

