namespace EventsData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class image : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Path", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Path");
        }
    }
}
