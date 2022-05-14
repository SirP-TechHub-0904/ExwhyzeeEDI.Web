namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "RecommendationNote", c => c.String());
            AlterColumn("dbo.Profiles", "DateEnrolledForTraining", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Profiles", "DateEnrolledForTraining", c => c.DateTime(nullable: false));
            DropColumn("dbo.Profiles", "RecommendationNote");
        }
    }
}
