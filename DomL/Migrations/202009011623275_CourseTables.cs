namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Course", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SchoolId = c.Int(nullable: false),
                        TeacherId = c.Int(),
                        ScoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.SchoolId, cascadeDelete: true)
                .ForeignKey("dbo.Score", t => t.ScoreId)
                .ForeignKey("dbo.Person", t => t.TeacherId)
                .Index(t => t.SchoolId)
                .Index(t => t.TeacherId)
                .Index(t => t.ScoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseActivity", "CourseId", "dbo.Course");
            DropForeignKey("dbo.Course", "TeacherId", "dbo.Person");
            DropForeignKey("dbo.Course", "ScoreId", "dbo.Score");
            DropForeignKey("dbo.Course", "SchoolId", "dbo.Company");
            DropForeignKey("dbo.CourseActivity", "Id", "dbo.Activity");
            DropIndex("dbo.Course", new[] { "ScoreId" });
            DropIndex("dbo.Course", new[] { "TeacherId" });
            DropIndex("dbo.Course", new[] { "SchoolId" });
            DropIndex("dbo.CourseActivity", new[] { "CourseId" });
            DropIndex("dbo.CourseActivity", new[] { "Id" });
            DropTable("dbo.Course");
            DropTable("dbo.CourseActivity");
        }
    }
}
