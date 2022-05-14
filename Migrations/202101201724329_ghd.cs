namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ghd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExwhyzeeModels", "LoanAmount", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "SortOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExwhyzeeModels", "SortOrder");
            DropColumn("dbo.ExwhyzeeModels", "LoanAmount");
        }
    }
}
