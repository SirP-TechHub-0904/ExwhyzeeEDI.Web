namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hjh : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.YoungMinds", "SpecifyVocation");
            DropColumn("dbo.YoungMinds", "DoYouHaveICTSkillsForYourBusiness");
        }
        
        public override void Down()
        {
            AddColumn("dbo.YoungMinds", "DoYouHaveICTSkillsForYourBusiness", c => c.String());
            AddColumn("dbo.YoungMinds", "SpecifyVocation", c => c.String());
        }
    }
}
