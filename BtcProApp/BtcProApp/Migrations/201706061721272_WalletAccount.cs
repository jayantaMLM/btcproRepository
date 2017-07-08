namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WalletAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registers", "MyWalletAccount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registers", "MyWalletAccount");
        }
    }
}
