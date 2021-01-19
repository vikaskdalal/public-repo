namespace ShoppingSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Product_Price", c => c.Single(nullable: false));
            AlterColumn("dbo.Products", "Discount_Price", c => c.Single());
            AlterColumn("dbo.Products", "Additional_Discount", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Additional_Discount", c => c.Int());
            AlterColumn("dbo.Products", "Discount_Price", c => c.Int());
            AlterColumn("dbo.Products", "Product_Price", c => c.Int(nullable: false));
        }
    }
}
