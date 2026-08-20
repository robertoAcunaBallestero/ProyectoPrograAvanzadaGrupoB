namespace Proyecto_Progra_Grupo8.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AgregarEstadoUltimaConexionUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.AspNetUsers",
                "Activo",
                c => c.Boolean(
                    nullable: false,
                    defaultValue: true));

            AddColumn(
                "dbo.AspNetUsers",
                "UltimaConexion",
                c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn(
                "dbo.AspNetUsers",
                "UltimaConexion");

            DropColumn(
                "dbo.AspNetUsers",
                "Activo");
        }
    }
}