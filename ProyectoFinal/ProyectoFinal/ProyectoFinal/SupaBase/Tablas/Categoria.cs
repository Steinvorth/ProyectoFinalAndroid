using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("Categoria")]
    public class Categoria : BaseModel
    {
        [PrimaryKey("id_categoria")]
        public int Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }
    }
}
