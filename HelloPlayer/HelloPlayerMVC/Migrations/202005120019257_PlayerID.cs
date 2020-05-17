namespace HelloPlayerMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PlayerID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PlayerID");
        }
    }
}
