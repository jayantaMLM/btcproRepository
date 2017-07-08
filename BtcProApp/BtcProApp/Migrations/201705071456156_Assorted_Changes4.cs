namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assorted_Changes4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "UplineRegistrationid", c => c.Long());
            DropColumn("dbo.Members", "Uplineid");
            DropColumn("dbo.Members", "Activemembers");
            DropColumn("dbo.Members", "Password");
            DropColumn("dbo.Members", "Leftrightposition");
            DropColumn("dbo.Members", "Topcode");
            DropColumn("dbo.Members", "Position");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "Position", c => c.String());
            AddColumn("dbo.Members", "Topcode", c => c.Long());
            AddColumn("dbo.Members", "Leftrightposition", c => c.String());
            AddColumn("dbo.Members", "Password", c => c.String());
            AddColumn("dbo.Members", "Activemembers", c => c.Long());
            AddColumn("dbo.Members", "Uplineid", c => c.Long());
            DropColumn("dbo.Members", "UplineRegistrationid");
        }
    }
}
