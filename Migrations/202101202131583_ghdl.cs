namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ghdl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExwhyzeeModels", "UploadBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExwhyzeeModels", "UploadBy");
        }
    }
}
