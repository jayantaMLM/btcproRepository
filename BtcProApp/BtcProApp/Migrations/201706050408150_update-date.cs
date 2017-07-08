namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WithdrawalRequests", "Approved_Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WithdrawalRequests", "Approved_Date", c => c.DateTime(nullable: false));
        }
    }
}
