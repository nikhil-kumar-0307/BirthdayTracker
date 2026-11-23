namespace BirthdayTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonMaster : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonMasters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Salutation = c.String(maxLength: 20),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Designation = c.String(nullable: false, maxLength: 100),
                        Category = c.String(nullable: false, maxLength: 50),
                        Organization = c.String(maxLength: 150),
                        Area = c.String(maxLength: 100),
                        Address = c.String(maxLength: 250),
                        Email = c.String(maxLength: 100),
                        MobileNo = c.String(maxLength: 15),
                        OfficePhone = c.String(maxLength: 15),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(maxLength: 10),
                        PaName = c.String(maxLength: 100),
                        PaMobile = c.String(maxLength: 15),
                        Remarks = c.String(maxLength: 300),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PersonMasters");
        }
    }
}
