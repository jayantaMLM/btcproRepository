namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CryptoCurrency_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CryptoCurrrencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Transactiondate = c.DateTime(nullable: false),
                        PackageId = c.Int(nullable: false),
                        PackageAmt = c.Double(nullable: false),
                        UplineId = c.Long(nullable: false),
                        Paying_Currency = c.String(),
                        Target_Currency = c.String(),
                        TargetWalletAccount = c.String(),
                        ConvertedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Transactionid = c.String(),
                        Status_url = c.String(),
                        Qrcode_url = c.String(),
                        Paymentstatus = c.Int(nullable: false),
                        StatusRemarks = c.String(),
                        FullyPaid = c.Boolean(nullable: false),
                        PaymentDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CryptoCurrrencies");
        }
    }
}
