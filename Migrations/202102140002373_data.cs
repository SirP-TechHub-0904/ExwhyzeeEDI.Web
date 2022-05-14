namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class data : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "NeedCertificate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Profiles", "NeedBusinessPlan", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "NeedBusinessPlan");
            DropColumn("dbo.Profiles", "NeedCertificate");
        }
    }
}
