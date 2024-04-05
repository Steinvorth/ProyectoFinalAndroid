using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("Producto")]
    public class Producto : BaseModel //comentario prueba ,visto
    {
        [PrimaryKey("id_producto")]
        public int Id { get; set; } 

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("precio")]
        public float Precio { get; set; }

        [Column("Imagen")]
        public string Imagen { get; set; }

        [Column("id_categoria")]
        public int Id_Categoria { get; set; }

        [Column("codigo")]
        public long Codigo { get; set; }
    }
}
