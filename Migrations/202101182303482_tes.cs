namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExwhyzeeModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Surname = c.String(),
                        Firstname = c.String(),
                        Othername = c.String(),
                        RegistrationNumber = c.String(),
                        PhoneNumber = c.String(),
                        Date = c.DateTime(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        Uploaded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExwhyzeeModels");
        }
    }
}
