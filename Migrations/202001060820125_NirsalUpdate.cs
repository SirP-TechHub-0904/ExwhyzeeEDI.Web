namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NirsalUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "NirsalRegistration", c => c.Boolean(nullable: false));
            AddColumn("dbo.Profiles", "AccountOpen", c => c.Boolean(nullable: false));
            AddColumn("dbo.Profiles", "BusinessPlan", c => c.Boolean(nullable: false));
            AddColumn("dbo.Profiles", "Department", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "Department");
            DropColumn("dbo.Profiles", "BusinessPlan");
            DropColumn("dbo.Profiles", "AccountOpen");
            DropColumn("dbo.Profiles", "NirsalRegistration");
        }
    }
}
