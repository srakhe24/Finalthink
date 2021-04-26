using System;
using System.ComponentModel.DataAnnotations;

namespace CURDOperationWithImageUploadCore5_Demo.ViewModels
{
    public class InventoryViewModel : EditImageViewModel
    {
        [Required]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        

        [Required]
        public string Desc { get; set; }
    }
}
