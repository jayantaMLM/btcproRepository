namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BinaryOpenings_Update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BinaryOpenings", "RegistrationId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BinaryOpenings", "RegistrationId");
        }
    }
}
