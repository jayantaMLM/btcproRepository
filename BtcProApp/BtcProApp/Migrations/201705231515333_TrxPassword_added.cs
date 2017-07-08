namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrxPassword_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registers", "TrxPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registers", "TrxPassword");
        }
    }
}
