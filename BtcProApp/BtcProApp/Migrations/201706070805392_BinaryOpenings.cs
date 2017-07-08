namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BinaryOpenings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BinaryOpenings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LeftSideCd = c.Long(nullable: false),
                        RightSideCd = c.Long(nullable: false),
                        ProcessId = c.String(),
                        IsCurrent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BinaryOpenings");
        }
    }
}
