namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WithdrawalRequest_BatchNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WithdrawalRequests", "BatchNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WithdrawalRequests", "BatchNo");
        }
    }
}
