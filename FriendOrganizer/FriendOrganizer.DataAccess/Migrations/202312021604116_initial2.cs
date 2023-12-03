namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Friend", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Friend", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Friend", "Email", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Friend", "Email", c => c.String());
            AlterColumn("dbo.Friend", "LastName", c => c.String());
            AlterColumn("dbo.Friend", "FirstName", c => c.String());
        }
    }
}
