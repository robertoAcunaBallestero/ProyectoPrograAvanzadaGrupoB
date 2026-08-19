namespace Proyecto_Progra_Grupo8.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarResenas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resenas",
                c => new
                    {
                        ResenaId = c.Int(nullable: false, identity: true),
                        EventoId = c.Int(nullable: false),
                        UsuarioId = c.String(nullable: false, maxLength: 128),
                        Comentario = c.String(nullable: false, maxLength: 1000),
                        Estado = c.String(nullable: false, maxLength: 20),
                        FechaResena = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ResenaId)
                .ForeignKey("dbo.Eventos", t => t.EventoId)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioId)
                .Index(t => t.EventoId)
                .Index(t => t.UsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resenas", "UsuarioId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Resenas", "EventoId", "dbo.Eventos");
            DropIndex("dbo.Resenas", new[] { "UsuarioId" });
            DropIndex("dbo.Resenas", new[] { "EventoId" });
            DropTable("dbo.Resenas");
        }
    }
}
