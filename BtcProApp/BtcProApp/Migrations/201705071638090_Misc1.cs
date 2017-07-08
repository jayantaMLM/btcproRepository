namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Misc1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ledgers", "RegistrationId", c => c.Long(nullable: false));
            DropColumn("dbo.Ledgers", "MemberId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ledgers", "MemberId", c => c.Long(nullable: false));
            DropColumn("dbo.Ledgers", "RegistrationId");
        }
    }
}
