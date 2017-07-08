namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BinaryOpenings_Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BinaryOpenings", "LeftSideCd", c => c.Double(nullable: false));
            AlterColumn("dbo.BinaryOpenings", "RightSideCd", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BinaryOpenings", "RightSideCd", c => c.Long(nullable: false));
            AlterColumn("dbo.BinaryOpenings", "LeftSideCd", c => c.Long(nullable: false));
        }
    }
}
