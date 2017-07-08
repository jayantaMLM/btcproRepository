namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WithdrawalRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WithdrawalRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RegistrationId = c.Long(nullable: false),
                        WalletId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        sDate = c.String(),
                        Amount = c.String(),
                        Approved_Date = c.DateTime(nullable: false),
                        sApproved_Date = c.String(),
                        Status = c.String(),
                        PaidOutAmount = c.Double(nullable: false),
                        ServiceCharge = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WithdrawalRequests");
        }
    }
}
