namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BinaryIncome_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BinaryIncomes", "TransactionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BinaryIncomes", "PurchaserRegistrationId", c => c.Long(nullable: false));
            DropColumn("dbo.BinaryIncomes", "TotalWeeklyEarning");
            DropColumn("dbo.BinaryIncomes", "MembersWhoPurchased");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BinaryIncomes", "MembersWhoPurchased", c => c.String());
            AddColumn("dbo.BinaryIncomes", "TotalWeeklyEarning", c => c.Double(nullable: false));
            DropColumn("dbo.BinaryIncomes", "PurchaserRegistrationId");
            DropColumn("dbo.BinaryIncomes", "TransactionDate");
        }
    }
}
