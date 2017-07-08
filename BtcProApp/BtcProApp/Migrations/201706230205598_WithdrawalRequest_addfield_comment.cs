namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WithdrawalRequest_addfield_comment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WithdrawalRequests", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WithdrawalRequests", "Comment");
        }
    }
}
