namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Otherincomes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BinaryIncomes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationId = c.Long(nullable: false),
                        WeekNo = c.Int(nullable: false),
                        WeekStartDate = c.DateTime(nullable: false),
                        WeekEndDate = c.DateTime(nullable: false),
                        LeftNewJoining = c.Long(nullable: false),
                        RightNewJoining = c.Long(nullable: false),
                        LeftNewBusinessCount = c.Long(nullable: false),
                        RightNewBusinessCount = c.Long(nullable: false),
                        LeftNewBusinessAmount = c.Double(nullable: false),
                        RightNewBusinessAmount = c.Double(nullable: false),
                        TotalWeeklyEarning = c.Double(nullable: false),
                        MembersWhoPurchased = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GenerationIncomes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationId = c.Long(nullable: false),
                        WeekNo = c.Int(nullable: false),
                        WeekStartDate = c.DateTime(nullable: false),
                        WeekEndDate = c.DateTime(nullable: false),
                        LeftNewJoining = c.Long(nullable: false),
                        RightNewJoining = c.Long(nullable: false),
                        LeftNewBusinessCount = c.Long(nullable: false),
                        RightNewBusinessCount = c.Long(nullable: false),
                        LeftNewBusinessAmount = c.Double(nullable: false),
                        RightNewBusinessAmount = c.Double(nullable: false),
                        TotalWeeklyEarning = c.Double(nullable: false),
                        MembersWhoPurchased_L1 = c.String(),
                        MembersWhoPurchased_L2 = c.String(),
                        MembersWhoPurchased_L3 = c.String(),
                        MembersWhoPurchased_L4 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SponsorIncomes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationId = c.Long(nullable: false),
                        WeekNo = c.Int(nullable: false),
                        WeekStartDate = c.DateTime(nullable: false),
                        WeekEndDate = c.DateTime(nullable: false),
                        LeftNewJoining = c.Long(nullable: false),
                        RightNewJoining = c.Long(nullable: false),
                        LeftNewBusinessCount = c.Long(nullable: false),
                        RightNewBusinessCount = c.Long(nullable: false),
                        LeftNewBusinessAmount = c.Double(nullable: false),
                        RightNewBusinessAmount = c.Double(nullable: false),
                        TotalWeeklyEarning = c.Double(nullable: false),
                        MembersWhoPurchased = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SponsorIncomes");
            DropTable("dbo.GenerationIncomes");
            DropTable("dbo.BinaryIncomes");
        }
    }
}
