namespace ShoppingSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductPrice", c => c.Single(nullable: false));
            AddColumn("dbo.Products", "DiscountPrice", c => c.Single());
            AddColumn("dbo.Products", "AdditionalDiscount", c => c.Single());
            AddColumn("dbo.Products", "IsVisible", c => c.String(nullable: false));
            AddColumn("dbo.Users", "UserType", c => c.String(nullable: false));
            DropColumn("dbo.Products", "Product_Price");
            DropColumn("dbo.Products", "Discount_Price");
            DropColumn("dbo.Products", "Additional_Discount");
            DropColumn("dbo.Products", "Is_Visible");
            DropColumn("dbo.Users", "User_type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "User_type", c => c.String(nullable: false));
            AddColumn("dbo.Products", "Is_Visible", c => c.String(nullable: false));
            AddColumn("dbo.Products", "Additional_Discount", c => c.Single());
            AddColumn("dbo.Products", "Discount_Price", c => c.Single());
            AddColumn("dbo.Products", "Product_Price", c => c.Single(nullable: false));
            DropColumn("dbo.Users", "UserType");
            DropColumn("dbo.Products", "IsVisible");
            DropColumn("dbo.Products", "AdditionalDiscount");
            DropColumn("dbo.Products", "DiscountPrice");
            DropColumn("dbo.Products", "ProductPrice");
        }
    }
}
