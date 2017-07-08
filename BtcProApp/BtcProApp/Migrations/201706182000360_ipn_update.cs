namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ipn_update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ipns", "amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ipns", "amounti", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ipns", "fee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ipns", "feei", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ipns", "feei", c => c.Double(nullable: false));
            AlterColumn("dbo.ipns", "fee", c => c.Double(nullable: false));
            AlterColumn("dbo.ipns", "amounti", c => c.Double(nullable: false));
            AlterColumn("dbo.ipns", "amount", c => c.Double(nullable: false));
        }
    }
}
