namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ledger_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ledgers", "ToFromUser", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ledgers", "ToFromUser");
        }
    }
}
