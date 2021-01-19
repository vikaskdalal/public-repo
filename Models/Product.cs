

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ShoppingSite.Models
{
    public class Product : IValidatableObject
    {
        // int id, name, price, discount, additional_discount, is_visible;

        [Key]
        [Display(Name = "Id")]
        public int ProductId { get; set; }

        [StringLength(450)]
        [Index(IsUnique = true)]
        [Display(Name = " Product Name")]
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }


        [Range(0, float.MaxValue, ErrorMessage = "Value should not be Negative")]
        [Display(Name = "Price(Rs.)")]
        [Required(ErrorMessage = "Product Price is required")]
        public float ProductPrice { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Value should not be Negative")]
        [Display(Name = "Discount(%)")]
        public float DiscountPrice { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Value should not be Negative")]
        [Display(Name = "AdditionalDiscount(%)")]
        public float AdditionalDiscount { get; set; }

        [Display(Name = "Visibility")]
        [Required(ErrorMessage = "Visibility of Product is required")]
        public string IsVisible { get; set; }
     

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            UserDBContext db = new UserDBContext();
            var validateName = db.Products.FirstOrDefault(x => x.ProductName == ProductName && x.ProductId != ProductId);
            if (validateName != null)
            {               
                yield return new ValidationResult("Product name already exists.");
            }
            else
            {
                yield return ValidationResult.Success;
            }
        }
    }
}