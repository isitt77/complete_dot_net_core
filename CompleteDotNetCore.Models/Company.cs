using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CompleteDotNetCore.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [DisplayName("Zip Code")]
        public string? ZipCode { get; set; }
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}

