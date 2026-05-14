using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class MarcaNegocio
    {
        //Método para listar Marcas 

        public List<Marca> listar()
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //conexion a base de datos
                //...
                datos.setearConsulta("Select Id, Descripcion From MARCAS");
                datos.ejecutarLectura();

                //mapeo de objetos
                //...
                while (datos.Lector.Read())
                {
                    Marca marca = new Marca();
                    marca.Id = (int)datos.Lector["Id"];
                    marca.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(marca);
                }

                datos.cerrarConexion();
            }
            catch (Exception)
            {

                throw;
            }




            return lista;
        }



    }
}
