using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        
       

        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                //codigo de conexion a base de datos
                //...
                datos.setearConsulta("Select A.Id, A.Codigo, A.Nombre, A.Descripcion, A.Precio, A.ImagenUrl, A.IdMarca, A.IdCategoria, C.Descripcion as Categoria, M.Descripcion as Marca From ARTICULOS A, CATEGORIAS C, MARCAS M Where A.IdMarca = M.Id AND A.IdCategoria = C.Id; ");
                datos.ejecutarLectura();



                //código de mapeo de objetos y armado de list
                //...
                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo();

                    //mapeo de objeto articulo
                    articulo.Id = (int)datos.Lector["Id"];
                    if (!(datos.Lector["Codigo"] is DBNull))
                    {
                        articulo.Codigo=(string)datos.Lector["Codigo"];
                    }
                    if (!(datos.Lector["Nombre"] is DBNull))
                    {
                        articulo.Nombre = (string)datos.Lector["Nombre"];
                    }
                    articulo.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["Precio"] is DBNull))
                    {
                        articulo.Precio = (decimal)datos.Lector["Precio"];
                    }

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        articulo.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }

                    if (!(datos.Lector["IdCategoria"] is DBNull))
                    {
                        articulo.Categoria = new Categoria
                        {
                            Id = (int)datos.Lector["IdCategoria"],
                            Descripcion = datos.Lector["Categoria"] as string
                        };
                    }
                    if (!(datos.Lector["IdMarca"] is DBNull))
                    {
                        articulo.Marca = new Marca
                        {
                            Id = (int)datos.Lector["IdMarca"],
                            Descripcion = (string)datos.Lector["Marca"]
                        };
                    }
                    lista.Add(articulo);
                }

            }
            finally
            {
                datos.cerrarConexion();

            }

            return lista;
        }


        public void Agregar(Articulo nuevo) 
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio) values (@codigo,@nombre,@desc,@IdMarca,@IdCategoria,@ImagenUrl,@precio)\r\n");
                //seteamos los parametros con el objeto que recibimos
                datos.setearParametros("@codigo", nuevo.Codigo);
                datos.setearParametros("@nombre", nuevo.Nombre);
                datos.setearParametros("@desc", nuevo.Descripcion);
                datos.setearParametros("@IdMarca", nuevo.Marca.Id);
                datos.setearParametros("@IdCategoria", nuevo.Categoria.Id);
                datos.setearParametros("@ImagenUrl", nuevo.ImagenUrl);
                datos.setearParametros("@precio", nuevo.Precio);

                //enviamos la consulta modificada
                datos.ejecutarAccion();


            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public void Modificar(Articulo modificado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @ImagenUrl, Precio = @precio where Id = @Id;\r\n");
                datos.setearParametros("@Id", modificado.Id);
                datos.setearParametros("@codigo", modificado.Codigo);
                datos.setearParametros("@nombre", modificado.Nombre);
                datos.setearParametros("@descripcion", modificado.Descripcion);
                datos.setearParametros("@IdMarca", modificado.Marca.Id);
                datos.setearParametros("@IdCategoria", modificado.Categoria.Id);
                datos.setearParametros("@ImagenUrl", modificado.ImagenUrl);
                datos.setearParametros("@precio", modificado.Precio);

                //ejecutamos el update
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("delete from ARTICULOS where Id = @id;");
                datos.setearParametros("@id", id);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Articulo> filtroAvanzado(string campo, string criterio, decimal? precio = null) 
        {
            List<Articulo> lista = new List<Articulo>();
            //hay que armar la lista y retornarla
            //campo = Categoria o Marca -- Criterio = lo que haya en marca o lo que haya en categoria 
            AccesoDatos datos = new AccesoDatos();
            string consulta = "Select A.Id, A.Codigo, A.Nombre, A.Descripcion, A.Precio, A.ImagenUrl, A.IdMarca, A.IdCategoria, C.Descripcion as Categoria, M.Descripcion as Marca From ARTICULOS A, CATEGORIAS C, MARCAS M Where A.IdMarca = M.Id AND A.IdCategoria = C.Id ";
            try
            {
                if (campo == "Marcas")
                {
                    consulta += " AND M.Descripcion = @marca";
                    datos.setearParametros("@marca", criterio);

                }
                else if (campo == "Categorías")
                {
                    consulta += "AND C.Descripcion = @categoria";
                    datos.setearParametros("@categoria", criterio);

                }else if (campo == "Precio" && precio.HasValue)
                {
                    if (criterio == "Mayor a")
                    {
                        consulta += "AND A.Precio > @precio";
                        datos.setearParametros("@precio", precio.Value);

                    }else if(criterio == "Menor a")
                    {
                        consulta += "AND A.Precio < @precio";
                        datos.setearParametros("@precio", precio.Value);

                    }
                    else if(criterio == "Igual a")
                    {
                        consulta += "AND A.Precio = @precio";
                        datos.setearParametros("@precio", precio.Value);
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                //aca hay que mapear lo que me traiga y ponerlo en la lista 
                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo();

                    //mapeo de objeto articulo
                    articulo.Id = (int)datos.Lector["Id"];
                    if (!(datos.Lector["Codigo"] is DBNull))
                    {
                        articulo.Codigo = (string)datos.Lector["Codigo"];
                    }
                    if (!(datos.Lector["Nombre"] is DBNull))
                    {
                        articulo.Nombre = (string)datos.Lector["Nombre"];
                    }
                    articulo.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["Precio"] is DBNull))
                    {
                        articulo.Precio = (decimal)datos.Lector["Precio"];
                    }

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        articulo.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }


                    if (!(datos.Lector["IdCategoria"] is DBNull))
                    {
                        articulo.Categoria = new Categoria
                        {
                            Id = (int)datos.Lector["IdCategoria"],
                            Descripcion = datos.Lector["Categoria"] as string
                        };
                    }
                    if (!(datos.Lector["IdMarca"] is DBNull))
                    {
                        articulo.Marca = new Marca
                        {
                            Id = (int)datos.Lector["IdMarca"],
                            Descripcion = (string)datos.Lector["Marca"]
                        };
                    }

                    lista.Add(articulo);
                }
            }
            finally 
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        

    }
}
