namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assorted_Changes2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "RegistrationId", c => c.Long(nullable: false));
            AddColumn("dbo.Registers", "CountryCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registers", "CountryCode");
            DropColumn("dbo.Members", "RegistrationId");
        }
    }
}
