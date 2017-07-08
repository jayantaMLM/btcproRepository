namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WeeklyIncomes3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WeeklyIncomes", "WeekStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.WeeklyIncomes", "WeekEndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WeeklyIncomes", "WeekEndDate");
            DropColumn("dbo.WeeklyIncomes", "WeekStartDate");
        }
    }
}
