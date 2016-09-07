namespace Bonsaii.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "NickName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "NickName");
        }
    }
}
