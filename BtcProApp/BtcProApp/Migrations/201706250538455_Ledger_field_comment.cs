namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ledger_field_comment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ledgers", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ledgers", "Comment");
        }
    }
}
