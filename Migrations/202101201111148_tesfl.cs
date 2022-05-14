namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tesfl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExwhyzeeModels", "BVN", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "StateOfBusinessLocation", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "Gender", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "EDI", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "Sector", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "SubSector", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "EquipmentAmount", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "WorkingCapital", c => c.String());
            AddColumn("dbo.ExwhyzeeModels", "LoanTenor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExwhyzeeModels", "LoanTenor");
            DropColumn("dbo.ExwhyzeeModels", "WorkingCapital");
            DropColumn("dbo.ExwhyzeeModels", "EquipmentAmount");
            DropColumn("dbo.ExwhyzeeModels", "SubSector");
            DropColumn("dbo.ExwhyzeeModels", "Sector");
            DropColumn("dbo.ExwhyzeeModels", "EDI");
            DropColumn("dbo.ExwhyzeeModels", "Gender");
            DropColumn("dbo.ExwhyzeeModels", "StateOfBusinessLocation");
            DropColumn("dbo.ExwhyzeeModels", "BVN");
        }
    }
}
