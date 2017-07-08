namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Registers", "ReferrerName", c => c.String(nullable: false));
            AlterColumn("dbo.Registers", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.Registers", "EmailId", c => c.String(nullable: false));
            AlterColumn("dbo.Registers", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.Registers", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.Registers", "BinaryPosition", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Registers", "BinaryPosition", c => c.String());
            AlterColumn("dbo.Registers", "Password", c => c.String());
            AlterColumn("dbo.Registers", "UserName", c => c.String());
            AlterColumn("dbo.Registers", "EmailId", c => c.String());
            AlterColumn("dbo.Registers", "FullName", c => c.String());
            AlterColumn("dbo.Registers", "ReferrerName", c => c.String());
        }
    }
}
