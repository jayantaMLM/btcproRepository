namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegisterJoined : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registers", "Joined", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registers", "Joined");
        }
    }
}
