namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newupdateyoungminds : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YoungMinds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationNumber = c.String(),
                        Title = c.String(),
                        Surname = c.String(),
                        FirstName = c.String(),
                        OtherName = c.String(),
                        Gender = c.String(),
                        Dateofbirth = c.DateTime(nullable: false),
                        DateRegistered = c.DateTime(nullable: false),
                        MaritalStatus = c.String(),
                        Religion = c.String(),
                        PermanentHomeAddress = c.String(),
                        ModeOfIdentification = c.String(),
                        IdentificationNumber = c.String(),
                        ContactAddress = c.String(),
                        StateofOrigin = c.String(),
                        LocalGovernmentArea = c.String(),
                        AreYouInAbyVocationNow = c.String(),
                        SpecifyVocation = c.String(),
                        DoYouHaveICTSkillsForYourBusiness = c.String(),
                        UserId = c.String(),
                        WhatAreaAreYouInterestedIn = c.String(),
                        WhatTrackAreYouInterestedIn = c.String(),
                        CanYouDoMoreWithLessResources = c.String(),
                        TellUsHowYouWouldGoAboutThis = c.String(),
                        WhatIsYourCurrentExperienceLevelInInformationTechnology = c.String(),
                        AreYouReadyToMakeChangesToYourEnvironment = c.String(),
                        IfYes_HowCanYouMakeChanges = c.String(),
                        Referee = c.String(),
                        KnowAFriendThatYouThinkWillMakeAGreatChangeInHisEnvironment = c.String(),
                        IdeaAndSuggestion = c.String(),
                        PassportUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.YoungMinds");
        }
    }
}
