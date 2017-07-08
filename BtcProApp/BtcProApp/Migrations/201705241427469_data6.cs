namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class data6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "ApprovedWhen", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "ApprovedWhen", c => c.DateTime(nullable: false));
        }
    }
}
