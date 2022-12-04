using System;
using System.ComponentModel.DataAnnotations;

namespace CompleteDotNetCore.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        //public ShoppingCartViewModel()
        //{
        //}

        public Product? Product { get; set; }
        [Range(1, 1000, ErrorMessage =
            "Pleas enter a number between 1 and 1,000.")]
        public int Count { get; set; }
    }
}

