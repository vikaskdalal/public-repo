namespace ShoppingSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductName", c => c.String(nullable: false, maxLength: 450));
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 450));
            CreateIndex("dbo.Carts", "ProductId");
            CreateIndex("dbo.Carts", "UserId");
            CreateIndex("dbo.Products", "ProductName", unique: true);
            CreateIndex("dbo.Users", "UserName", unique: true);
            AddForeignKey("dbo.Carts", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.Carts", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Carts", "ProductId", "dbo.Products");
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Products", new[] { "ProductName" });
            DropIndex("dbo.Carts", new[] { "UserId" });
            DropIndex("dbo.Carts", new[] { "ProductId" });
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "ProductName", c => c.String(nullable: false));
        }
    }
}
