namespace Proyecto_Progra_Grupo8.Negocio.Resultado
{
   
    public class ResultadoOperacion
    {

        public bool Exito { get; set; }


        public string Mensaje { get; set; }

 
        public object Datos { get; set; }


        public ResultadoOperacion(
            bool exito,
            string mensaje,
            object datos = null)
        {
            Exito = exito;
            Mensaje = mensaje;
            Datos = datos;
        }
    }
}
