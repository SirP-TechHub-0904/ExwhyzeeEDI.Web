namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExwhyzeeModels", "Parish", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "ParishState", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "PassportPhoto", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "IDUpload", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExwhyzeeModels", "IDUpload");
            DropColumn("dbo.ExwhyzeeModels", "PassportPhoto");
            DropColumn("dbo.ExwhyzeeModels", "ParishState");
            DropColumn("dbo.ExwhyzeeModels", "Parish");
        }
    }
}
