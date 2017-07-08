namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferrerIdRegister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registers", "ReferrerId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registers", "ReferrerId");
        }
    }
}
