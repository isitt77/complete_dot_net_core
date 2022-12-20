using System;
namespace CompleteDotNetCore.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        //public ShoppingCartViewModel()
        //{
        //}

        public IEnumerable<ShoppingCart>? CartList { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}

