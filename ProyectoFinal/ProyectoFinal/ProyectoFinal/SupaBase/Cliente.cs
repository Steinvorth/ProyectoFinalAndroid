using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Supabase;

namespace ProyectoFinal.SupaBase
{
    [Table("clientes")]
    public class Cliente : BaseModel
    {
        [PrimaryKey("id")]
        public long Id { get; set; } // Assuming int8 in the database corresponds to long in C#

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("apellido")]
        public string Apellido { get; set; }

        [Column("usuario")]
        public string Usuario { get; set; }

        [Column("num_tel")]
        public string NumTel { get; set; }
    }
}
