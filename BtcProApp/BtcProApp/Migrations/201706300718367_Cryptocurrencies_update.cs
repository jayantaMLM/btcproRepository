namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cryptocurrencies_update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CryptoCurrrencies", "ConvertedAmount", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CryptoCurrrencies", "ConvertedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
