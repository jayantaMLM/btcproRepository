namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KYCTable_newFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KYCDocuments", "DateString", c => c.String());
            AddColumn("dbo.KYCDocuments", "Status", c => c.String());
            AddColumn("dbo.KYCDocuments", "isApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.KYCDocuments", "ApprovedWhen", c => c.DateTime());
            AddColumn("dbo.KYCDocuments", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KYCDocuments", "Comment");
            DropColumn("dbo.KYCDocuments", "ApprovedWhen");
            DropColumn("dbo.KYCDocuments", "isApproved");
            DropColumn("dbo.KYCDocuments", "Status");
            DropColumn("dbo.KYCDocuments", "DateString");
        }
    }
}
