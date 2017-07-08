namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KYCTable_addField_Date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KYCDocuments", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KYCDocuments", "Date");
        }
    }
}
