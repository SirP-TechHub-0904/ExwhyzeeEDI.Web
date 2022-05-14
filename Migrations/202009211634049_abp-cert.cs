namespace ExwhyzeeEDI.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abpcert : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ABPs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        FullNameStyle = c.String(),
                        RegNumber = c.String(),
                        RegNumberStyle = c.String(),
                        CodeStyle = c.String(),
                        FooterStyle = c.String(),
                        Note = c.String(),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ABPs");
        }
    }
}
