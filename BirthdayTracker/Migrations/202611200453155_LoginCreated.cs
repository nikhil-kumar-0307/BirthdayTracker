namespace BirthdayTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeNumber = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false),
                        Role = c.String(nullable: false, maxLength: 20),
                        LastLoginAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
