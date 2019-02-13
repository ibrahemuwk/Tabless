namespace TablessV1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "facebook_chk", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "twitter_chk", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "google_chk", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "google_chk");
            DropColumn("dbo.AspNetUsers", "twitter_chk");
            DropColumn("dbo.AspNetUsers", "facebook_chk");
        }
    }
}
