namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BinaryIncome_Update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BinaryIncomes", "PackageId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BinaryIncomes", "PackageId");
        }
    }
}
