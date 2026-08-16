namespace Proyecto_Progra_Grupo8.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Compras : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoriasEvento",
                c => new
                    {
                        CategoriaEventoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 80),
                        Descripcion = c.String(maxLength: 250),
                        Activa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoriaEventoId);
            
            CreateTable(
                "dbo.DetallesOrden",
                c => new
                    {
                        DetalleOrdenId = c.Int(nullable: false, identity: true),
                        OrdenId = c.Int(nullable: false),
                        EventoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.DetalleOrdenId)
                .ForeignKey("dbo.Eventos", t => t.EventoId)
                .ForeignKey("dbo.Ordenes", t => t.OrdenId, cascadeDelete: true)
                .Index(t => t.OrdenId)
                .Index(t => t.EventoId);
            
            CreateTable(
                "dbo.Ordenes",
                c => new
                    {
                        OrdenId = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(nullable: false, maxLength: 128),
                        FechaCompra = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrdenId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        DetalleOrdenId = c.Int(nullable: false),
                        CodigoUnico = c.String(nullable: false, maxLength: 50),
                        FechaGeneracion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TicketId)
                .ForeignKey("dbo.DetallesOrden", t => t.DetalleOrdenId, cascadeDelete: true)
                .Index(t => t.DetalleOrdenId)
                .Index(t => t.CodigoUnico, unique: true, name: "IX_Ticket_CodigoUnico");
            
            AddColumn("dbo.Eventos", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterColumn("dbo.ImagenEventoes", "TipoContenido", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AspNetUsers", "NumeroAsociado", c => c.String(nullable: false, maxLength: 7));
            CreateIndex("dbo.Eventos", "CategoriaEventoId");
            AddForeignKey("dbo.Eventos", "CategoriaEventoId", "dbo.CategoriasEvento", "CategoriaEventoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "DetalleOrdenId", "dbo.DetallesOrden");
            DropForeignKey("dbo.DetallesOrden", "OrdenId", "dbo.Ordenes");
            DropForeignKey("dbo.DetallesOrden", "EventoId", "dbo.Eventos");
            DropForeignKey("dbo.Eventos", "CategoriaEventoId", "dbo.CategoriasEvento");
            DropIndex("dbo.Tickets", "IX_Ticket_CodigoUnico");
            DropIndex("dbo.Tickets", new[] { "DetalleOrdenId" });
            DropIndex("dbo.DetallesOrden", new[] { "EventoId" });
            DropIndex("dbo.DetallesOrden", new[] { "OrdenId" });
            DropIndex("dbo.Eventos", new[] { "CategoriaEventoId" });
            AlterColumn("dbo.AspNetUsers", "NumeroAsociado", c => c.String(nullable: false));
            AlterColumn("dbo.ImagenEventoes", "TipoContenido", c => c.String(maxLength: 50));
            DropColumn("dbo.Eventos", "RowVersion");
            DropTable("dbo.Tickets");
            DropTable("dbo.Ordenes");
            DropTable("dbo.DetallesOrden");
            DropTable("dbo.CategoriasEvento");
        }
    }
}
