using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class CategoriaNegocio
    {
        //Lista para traer los desplegables

        public List<Categoria> listar()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Select Id, Descripcion From CATEGORIAS;");
                datos.ejecutarLectura();

                //mapeo de la lista 
                while (datos.Lector.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.Id = (int)datos.Lector["Id"];
                    categoria.Descripcion = (string)datos.Lector["Descripcion"];



                    lista.Add(categoria);
                }


                datos.cerrarConexion();


            }
            catch (Exception )
            {

                throw;
            }











            return lista;
        }


    }
}
