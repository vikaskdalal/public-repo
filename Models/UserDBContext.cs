namespace ShoppingSite.Models
{
   using System.Data.Entity;

    public class UserDBContext : DbContext
    {      
        public UserDBContext(): base("name=UserDBContext")
        {
        }
         public  DbSet<Product> Products { get; set; }
         public  DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}