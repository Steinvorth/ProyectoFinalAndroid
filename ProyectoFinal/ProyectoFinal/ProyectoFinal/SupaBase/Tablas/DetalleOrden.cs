using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("DetalleOrden")]
    public class DetalleOrden : BaseModel
    {
        [PrimaryKey("id_detalle")]
        public int Id { get; set; } 

        [Column("id_orden")]
        public int Id_Orden { get; set; }

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
