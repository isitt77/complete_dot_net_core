using System;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CompleteDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        //public ProductViewModel()
        //{
        //}

        public Product? product { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public IEnumerable<SelectListItem>? CoverTypeList { get; set; }
    }
}

