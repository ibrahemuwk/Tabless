namespace TablessV1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class google : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "GoogleUserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "GoogleUserId");
        }
    }
}
