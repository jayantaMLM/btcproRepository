namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Misc2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "RegistrationId", c => c.Long(nullable: false));
            DropColumn("dbo.Purchases", "Memberid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Purchases", "Memberid", c => c.Long(nullable: false));
            DropColumn("dbo.Purchases", "RegistrationId");
        }
    }
}
