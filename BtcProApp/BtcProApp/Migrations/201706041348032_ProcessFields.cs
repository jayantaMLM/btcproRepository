namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcessFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BinaryIncomes", "ProcessId", c => c.String());
            AddColumn("dbo.GenerationIncomes", "ProcessId", c => c.String());
            AddColumn("dbo.Ledgers", "ProcessId", c => c.String());
            AddColumn("dbo.Ledgers", "Leftside_cd", c => c.Double());
            AddColumn("dbo.Ledgers", "Rightside_cd", c => c.Double());
            AddColumn("dbo.SponsorIncomes", "ProcessId", c => c.String());
            AddColumn("dbo.WeeklyIncomes", "ProcessId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WeeklyIncomes", "ProcessId");
            DropColumn("dbo.SponsorIncomes", "ProcessId");
            DropColumn("dbo.Ledgers", "Rightside_cd");
            DropColumn("dbo.Ledgers", "Leftside_cd");
            DropColumn("dbo.Ledgers", "ProcessId");
            DropColumn("dbo.GenerationIncomes", "ProcessId");
            DropColumn("dbo.BinaryIncomes", "ProcessId");
        }
    }
}
