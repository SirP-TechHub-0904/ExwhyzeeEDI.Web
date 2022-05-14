namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class certupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "CertificateDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "CertificateDate");
        }
    }
}
