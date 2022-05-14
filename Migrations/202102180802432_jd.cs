namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExwhyzeeModels", "CertificateNumber", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "CertificateUploaded", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "BusinessPlanUploaded", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "MonthOfTraining", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "YearOfTraining", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "NumberOfTrainingDays", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "Interviewed", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "Disbursed", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "OtherCooment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExwhyzeeModels", "OtherCooment");
            DropColumn("dbo.ExwhyzeeModels", "Disbursed");
            DropColumn("dbo.ExwhyzeeModels", "Interviewed");
            DropColumn("dbo.ExwhyzeeModels", "NumberOfTrainingDays");
            DropColumn("dbo.ExwhyzeeModels", "YearOfTraining");
            DropColumn("dbo.ExwhyzeeModels", "MonthOfTraining");
            DropColumn("dbo.ExwhyzeeModels", "BusinessPlanUploaded");
            DropColumn("dbo.ExwhyzeeModels", "CertificateUploaded");
            DropColumn("dbo.ExwhyzeeModels", "CertificateNumber");
        }
    }
}
