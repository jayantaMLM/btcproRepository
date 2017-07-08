namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Register_addfield_isblocked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registers", "isBlocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registers", "isBlocked");
        }
    }
}
