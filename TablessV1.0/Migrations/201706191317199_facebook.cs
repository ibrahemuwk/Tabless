namespace TablessV1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class facebook : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.AspNetUsers DROP CONSTRAINT DF__AspNetUse__profi__571DF1D5");

            AlterColumn("dbo.AspNetUsers", "profile_photo", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "profile_photo", c => c.Byte(nullable: false));
        }
    }
}
