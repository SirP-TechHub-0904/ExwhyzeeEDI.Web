namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class apply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "RegisteredBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "RegisteredBy");
        }
    }
}
