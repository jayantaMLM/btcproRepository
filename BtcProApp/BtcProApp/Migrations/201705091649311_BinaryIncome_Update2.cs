namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BinaryIncome_Update2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GenerationIncomes", "TransactionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.GenerationIncomes", "PurchaserRegistrationId", c => c.Long(nullable: false));
            AddColumn("dbo.GenerationIncomes", "PackageId", c => c.Int(nullable: false));
            AddColumn("dbo.GenerationIncomes", "IncomeAmount", c => c.Double(nullable: false));
            AddColumn("dbo.GenerationIncomes", "IncomefromDownlineLevel", c => c.Int(nullable: false));
            AddColumn("dbo.SponsorIncomes", "TransactionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SponsorIncomes", "PurchaserRegistrationId", c => c.Long(nullable: false));
            AddColumn("dbo.SponsorIncomes", "PackageId", c => c.Int(nullable: false));
            AddColumn("dbo.SponsorIncomes", "IncomeAmount", c => c.Double(nullable: false));
            DropColumn("dbo.GenerationIncomes", "TotalWeeklyEarning");
            DropColumn("dbo.GenerationIncomes", "MembersWhoPurchased_L1");
            DropColumn("dbo.GenerationIncomes", "MembersWhoPurchased_L2");
            DropColumn("dbo.GenerationIncomes", "MembersWhoPurchased_L3");
            DropColumn("dbo.GenerationIncomes", "MembersWhoPurchased_L4");
            DropColumn("dbo.SponsorIncomes", "TotalWeeklyEarning");
            DropColumn("dbo.SponsorIncomes", "MembersWhoPurchased");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SponsorIncomes", "MembersWhoPurchased", c => c.String());
            AddColumn("dbo.SponsorIncomes", "TotalWeeklyEarning", c => c.Double(nullable: false));
            AddColumn("dbo.GenerationIncomes", "MembersWhoPurchased_L4", c => c.String());
            AddColumn("dbo.GenerationIncomes", "MembersWhoPurchased_L3", c => c.String());
            AddColumn("dbo.GenerationIncomes", "MembersWhoPurchased_L2", c => c.String());
            AddColumn("dbo.GenerationIncomes", "MembersWhoPurchased_L1", c => c.String());
            AddColumn("dbo.GenerationIncomes", "TotalWeeklyEarning", c => c.Double(nullable: false));
            DropColumn("dbo.SponsorIncomes", "IncomeAmount");
            DropColumn("dbo.SponsorIncomes", "PackageId");
            DropColumn("dbo.SponsorIncomes", "PurchaserRegistrationId");
            DropColumn("dbo.SponsorIncomes", "TransactionDate");
            DropColumn("dbo.GenerationIncomes", "IncomefromDownlineLevel");
            DropColumn("dbo.GenerationIncomes", "IncomeAmount");
            DropColumn("dbo.GenerationIncomes", "PackageId");
            DropColumn("dbo.GenerationIncomes", "PurchaserRegistrationId");
            DropColumn("dbo.GenerationIncomes", "TransactionDate");
        }
    }
}
