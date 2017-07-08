namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WeeklyIncomes2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WeeklyIncomes", "DueDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WeeklyIncomes", "DueDate");
        }
    }
}
