namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicantCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateCreated = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ExwhyzeeModels", "ApplicantCategoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExwhyzeeModels", "ApplicantCategoryId");
            DropTable("dbo.ApplicantCategories");
        }
    }
}
