namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hope : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.YoungMinds", "Skills", c => c.String());
            AddColumn("dbo.YoungMinds", "WriteAboutYourself", c => c.String());
            AddColumn("dbo.YoungMinds", "YourBiggestAchievement", c => c.String());
            AddColumn("dbo.YoungMinds", "WhatIsYourPassion", c => c.String());
            AddColumn("dbo.YoungMinds", "DefineYourPersonality", c => c.String());
            AddColumn("dbo.YoungMinds", "HowDoYouSeeYourSelfIn10Years", c => c.String());
            AddColumn("dbo.YoungMinds", "WhatAreYourLimitationsTowardsYourGoal", c => c.String());
            AddColumn("dbo.YoungMinds", "HowDoYouThinkYouCanOvercomeYourLimitations", c => c.String());
            AddColumn("dbo.YoungMinds", "WhatDoYouWantToChangeInTheSociety", c => c.String());
            AddColumn("dbo.YoungMinds", "PreviouseProjectAttemptedResearchPublicityInvention", c => c.String());
            AddColumn("dbo.YoungMinds", "WhatIsYourGreatestGoal", c => c.String());
            DropColumn("dbo.YoungMinds", "WhatAreaAreYouInterestedIn");
            DropColumn("dbo.YoungMinds", "WhatTrackAreYouInterestedIn");
            DropColumn("dbo.YoungMinds", "CanYouDoMoreWithLessResources");
            DropColumn("dbo.YoungMinds", "TellUsHowYouWouldGoAboutThis");
            DropColumn("dbo.YoungMinds", "WhatIsYourCurrentExperienceLevelInInformationTechnology");
            DropColumn("dbo.YoungMinds", "Email");
            DropColumn("dbo.YoungMinds", "Phone");
            DropColumn("dbo.YoungMinds", "Password");
            DropColumn("dbo.YoungMinds", "ConfirmPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.YoungMinds", "ConfirmPassword", c => c.String());
            AddColumn("dbo.YoungMinds", "Password", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.YoungMinds", "Phone", c => c.String());
            AddColumn("dbo.YoungMinds", "Email", c => c.String());
            AddColumn("dbo.YoungMinds", "WhatIsYourCurrentExperienceLevelInInformationTechnology", c => c.String());
            AddColumn("dbo.YoungMinds", "TellUsHowYouWouldGoAboutThis", c => c.String());
            AddColumn("dbo.YoungMinds", "CanYouDoMoreWithLessResources", c => c.String());
            AddColumn("dbo.YoungMinds", "WhatTrackAreYouInterestedIn", c => c.String());
            AddColumn("dbo.YoungMinds", "WhatAreaAreYouInterestedIn", c => c.String());
            DropColumn("dbo.YoungMinds", "WhatIsYourGreatestGoal");
            DropColumn("dbo.YoungMinds", "PreviouseProjectAttemptedResearchPublicityInvention");
            DropColumn("dbo.YoungMinds", "WhatDoYouWantToChangeInTheSociety");
            DropColumn("dbo.YoungMinds", "HowDoYouThinkYouCanOvercomeYourLimitations");
            DropColumn("dbo.YoungMinds", "WhatAreYourLimitationsTowardsYourGoal");
            DropColumn("dbo.YoungMinds", "HowDoYouSeeYourSelfIn10Years");
            DropColumn("dbo.YoungMinds", "DefineYourPersonality");
            DropColumn("dbo.YoungMinds", "WhatIsYourPassion");
            DropColumn("dbo.YoungMinds", "YourBiggestAchievement");
            DropColumn("dbo.YoungMinds", "WriteAboutYourself");
            DropColumn("dbo.YoungMinds", "Skills");
        }
    }
}
