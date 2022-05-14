namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tesf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExwhyzeeModels", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExwhyzeeModels", "UserId");
        }
    }
}
