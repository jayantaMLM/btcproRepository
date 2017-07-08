namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class data4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "isApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tickets", "ApprovedWhen", c => c.String());
            AddColumn("dbo.Tickets", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "Comment");
            DropColumn("dbo.Tickets", "ApprovedWhen");
            DropColumn("dbo.Tickets", "isApproved");
        }
    }
}
