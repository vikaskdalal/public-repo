namespace ShoppingSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "DiscountPrice", c => c.Single(nullable: false));
            AlterColumn("dbo.Products", "AdditionalDiscount", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "AdditionalDiscount", c => c.Single());
            AlterColumn("dbo.Products", "DiscountPrice", c => c.Single());
        }
    }
}
