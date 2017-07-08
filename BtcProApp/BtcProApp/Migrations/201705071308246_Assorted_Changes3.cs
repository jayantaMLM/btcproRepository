namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assorted_Changes3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Position", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "Position");
        }
    }
}
