namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Amount_edited : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WithdrawalRequests", "Amount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WithdrawalRequests", "Amount", c => c.String());
        }
    }
}
