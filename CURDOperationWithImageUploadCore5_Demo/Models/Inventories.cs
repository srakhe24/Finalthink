using System;
using System.ComponentModel.DataAnnotations;

namespace CURDOperationWithImageUploadCore5_Demo.Models
{
    public class Inventories
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(100)]
        public String Price { get; set; }

        [Required]
        [StringLength(100)]
        public int Category { get; set; }

        [Required]
        [StringLength(100)]
        public int Quantity { get; set; }

        [Required]
        [StringLength(255)]
        public string Desc { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string ProfilePicture { get; set; }
    }
}
