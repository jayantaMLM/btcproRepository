namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WithdrawalRequest_newfield_ReferenceNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WithdrawalRequests", "ReferenceNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WithdrawalRequests", "ReferenceNo");
        }
    }
}
