namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "PaidForTraining", c => c.Boolean(nullable: false));
            AddColumn("dbo.Profiles", "Attendance", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "CertificateCollected", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "CertificatePrinted", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "NirsalPayment", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "BusinessPlanUpload", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "BusinessPlanLink", c => c.String());
            AddColumn("dbo.Profiles", "BusinessPlanDateUploaded", c => c.DateTime(nullable: false));
            AddColumn("dbo.Profiles", "ProgramBy", c => c.String());
            AddColumn("dbo.Profiles", "FormStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "FormStatus");
            DropColumn("dbo.Profiles", "ProgramBy");
            DropColumn("dbo.Profiles", "BusinessPlanDateUploaded");
            DropColumn("dbo.Profiles", "BusinessPlanLink");
            DropColumn("dbo.Profiles", "BusinessPlanUpload");
            DropColumn("dbo.Profiles", "NirsalPayment");
            DropColumn("dbo.Profiles", "CertificatePrinted");
            DropColumn("dbo.Profiles", "CertificateCollected");
            DropColumn("dbo.Profiles", "Attendance");
            DropColumn("dbo.Profiles", "PaidForTraining");
        }
    }
}
