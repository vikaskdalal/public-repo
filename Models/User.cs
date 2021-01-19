
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ShoppingSite.Validations;

namespace ShoppingSite.Models
{
    public class User : IValidatableObject
    {
        // id, name, email, password(in hashed form), user_type(admin or user)

        [Key]
        public int UserId { get; set; }

        [Display(Name = "User Name")]
        [StringLength(450)]
        [Index(IsUnique = true)]
        [RegularExpression(@"^[a-zA-Z0-9.]+$", ErrorMessage = "Special character should not be entered")]
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

      
        [Required(ErrorMessage = "Email is required")]
        [CustomEmailValidator]
        public string Email { get; set; }

       
        //[MinLength(6, ErrorMessage = "Password too Short")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]    
        public string Password { get; set; }


        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "User Type")]       
        public string UserType { get; set; }

       

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            UserDBContext db = new UserDBContext();
            var validateName = db.Users.FirstOrDefault(x => x.UserName == UserName && x.UserId != UserId);
            if (validateName != null)
            {
                yield return new ValidationResult("Username already exists.");
            }
            else
            {
                yield return ValidationResult.Success;
            }

            var validateEmail = db.Users.FirstOrDefault(x => x.Email == Email && x.UserId != UserId);
            if (validateEmail != null)
            {
                yield return new ValidationResult("Email already exists.");
            }
            else
            {
                yield return ValidationResult.Success;
            }
        }

    }
}