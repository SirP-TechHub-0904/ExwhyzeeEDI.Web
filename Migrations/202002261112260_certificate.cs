namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class certificate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "CertificateId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "CertificateId");
        }
    }
}
