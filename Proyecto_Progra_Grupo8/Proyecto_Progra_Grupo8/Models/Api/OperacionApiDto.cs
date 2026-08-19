namespace Proyecto_Progra_Grupo8.Models.Api
{
    public class OperacionApiDto
    {
        public bool Exito { get; set; }

        public string Mensaje { get; set; }

        public OperacionApiDto(bool exito, string mensaje)
        {
            Exito = exito;
            Mensaje = mensaje;
        }
    }
}
