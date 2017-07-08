namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewsReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsReports",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NewsItemTitle = c.String(),
                        NewsItemBody = c.String(),
                        ImageFileName = c.String(),
                        NewsAuthor = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.String(),
                        UpdatedByUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NewsReports");
        }
    }
}
