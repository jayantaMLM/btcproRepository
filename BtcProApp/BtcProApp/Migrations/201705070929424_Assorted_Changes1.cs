namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assorted_Changes1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "BinaryPosition", c => c.String());
            AddColumn("dbo.Registers", "BinaryPosition", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registers", "BinaryPosition");
            DropColumn("dbo.Members", "BinaryPosition");
        }
    }
}
