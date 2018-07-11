namespace Project1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserPermissions", "Allow");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserPermissions", "Allow", c => c.Boolean(nullable: false));
        }
    }
}
