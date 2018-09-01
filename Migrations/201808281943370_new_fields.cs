namespace EpamAspMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "Name", c => c.String());
            AddColumn("dbo.Pictures", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pictures", "Type");
            DropColumn("dbo.Pictures", "Name");
        }
    }
}
