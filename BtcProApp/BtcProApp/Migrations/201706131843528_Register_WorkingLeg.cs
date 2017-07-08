namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Register_WorkingLeg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registers", "WorkingLeg", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registers", "WorkingLeg");
        }
    }
}
