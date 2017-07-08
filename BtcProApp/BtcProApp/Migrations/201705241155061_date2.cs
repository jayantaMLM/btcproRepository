namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class date2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "Status");
        }
    }
}
