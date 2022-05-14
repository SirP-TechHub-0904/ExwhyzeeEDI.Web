namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class certverify : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CertificateInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        Signature = c.String(),
                        SignatureName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CertificateInfoes");
        }
    }
}
