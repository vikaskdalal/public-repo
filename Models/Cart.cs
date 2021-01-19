using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ShoppingSite.Models
{
    public class Cart
    {  
        public int CartId { get; set; }

        [ForeignKey("ProductFK")]
        public int ProductId { get; set; }


        [ForeignKey("UserFK")]
        public int UserId { get; set; }

       
        public virtual User UserFK { get; set; }
        public virtual Product ProductFK { get; set; }
    }
}