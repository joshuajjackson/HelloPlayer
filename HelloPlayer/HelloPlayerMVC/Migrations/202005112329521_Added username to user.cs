namespace HelloPlayerMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedusernametouser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "HelloPlayerUserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "HelloPlayerUserName");
        }
    }
}
