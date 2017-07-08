namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assorted_Changes5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "ReferrerRegistrationId", c => c.Long(nullable: false));
            DropColumn("dbo.Members", "Referrerid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "Referrerid", c => c.Long(nullable: false));
            DropColumn("dbo.Members", "ReferrerRegistrationId");
        }
    }
}
