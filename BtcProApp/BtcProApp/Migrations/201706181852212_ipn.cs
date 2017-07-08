namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ipn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ipns",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        ipn_version = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ipn_type = c.String(),
                        ipn_mode = c.String(),
                        ipn_id = c.String(),
                        merchant = c.String(),
                        address = c.String(),
                        txn_id = c.String(),
                        status = c.String(),
                        status_text = c.String(),
                        currency = c.String(),
                        confirms = c.Int(nullable: false),
                        amount = c.Double(nullable: false),
                        amounti = c.Double(nullable: false),
                        fee = c.Double(nullable: false),
                        feei = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ipns");
        }
    }
}
