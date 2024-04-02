using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("DetalleCarrito")]
    public class DetalleCarrito : BaseModel
    {
        [PrimaryKey("id_detalle_carrito")]
        public long Id { get; set; } // Assuming int8 in the database corresponds to long in C#

        [Column("id_carrito")]
        public int Id_Carrito { get; set; }

        [Column("id_producto")]
        public int Id_Producto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio_unitario")]
        public float Precio_Unitario { get; set; }

        [Column("subtotal")]
        public float Subtotal { get; set; }
    }
}
