namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WithdrawalRequest_update_BitCoinAcfieldadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WithdrawalRequests", "PaidBitCoinAccount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WithdrawalRequests", "PaidBitCoinAccount");
        }
    }
}
