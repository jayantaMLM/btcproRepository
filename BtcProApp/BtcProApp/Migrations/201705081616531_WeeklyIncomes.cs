namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WeeklyIncomes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WeeklyIncomes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationId = c.Long(nullable: false),
                        WeekNo = c.Int(nullable: false),
                        Days = c.Int(nullable: false),
                        Pc = c.Double(nullable: false),
                        Income = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WeeklyIncomes");
        }
    }
}
