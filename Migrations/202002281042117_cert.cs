namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cert : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "CollectedCertificate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "CollectedCertificate");
        }
    }
}
