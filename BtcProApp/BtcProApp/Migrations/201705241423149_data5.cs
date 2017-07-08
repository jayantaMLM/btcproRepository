namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class data5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "ApprovedWhen", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "ApprovedWhen", c => c.String());
        }
    }
}
