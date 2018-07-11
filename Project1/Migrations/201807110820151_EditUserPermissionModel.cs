namespace Project1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserPermissionModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserPermissions", "Deny", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserPermissions", "Deny", c => c.Boolean(nullable: false));
        }
    }
}
