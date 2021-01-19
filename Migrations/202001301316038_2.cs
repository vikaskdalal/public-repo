namespace ShoppingSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Discount_Price", c => c.Int());
            AlterColumn("dbo.Products", "Additional_Discount", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Additional_Discount", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "Discount_Price", c => c.Int(nullable: false));
        }
    }
}
