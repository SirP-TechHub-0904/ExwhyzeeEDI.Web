namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserUpgrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "UserStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "DateEnrolledForTraining", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "DateEnrolledForTraining");
            DropColumn("dbo.Profiles", "UserStatus");
        }
    }
}
