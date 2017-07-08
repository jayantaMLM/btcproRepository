namespace BtcProApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KYCDocuments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RegistrationId = c.Long(nullable: false),
                        DocumentType = c.String(),
                        DocumentName = c.String(),
                        Comments = c.String(),
                        LibraryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LibraryDocuments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Module = c.String(),
                        ModuleId = c.Int(nullable: false),
                        SubModule = c.String(),
                        SubModuleId = c.Int(nullable: false),
                        OriginalImageName = c.String(),
                        UniqueImageName = c.String(),
                        ImageType = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                        UploadedBy = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LibraryDocuments");
            DropTable("dbo.KYCDocuments");
        }
    }
}
