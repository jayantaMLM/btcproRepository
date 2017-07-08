namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tables_03 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ledgers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MemberId = c.Long(nullable: false),
                        WalletId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Deposit = c.Double(nullable: false),
                        Withdraw = c.Double(nullable: false),
                        TransactionTypeId = c.Int(nullable: false),
                        TransactionId = c.Long(nullable: false),
                        SubLedgerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubLedgers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubLedgerName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransactionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Wallets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WalletName = c.String(),
                        Show = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Wallets");
            DropTable("dbo.TransactionTypes");
            DropTable("dbo.SubLedgers");
            DropTable("dbo.Ledgers");
        }
    }
}
