using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Supabase;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("Clientes")]
    public class Cliente : BaseModel
    {
        [PrimaryKey("id_cliente")]
        public int Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("apellido")]
        public string Apellido { get; set; }

        [Column("usuario")]
        public string Usuario { get; set; }

        [Column("num_tel")]
        public string NumTel { get; set; }

        [Column("pass")]
        public string Pass { get; set; }

        [Column("direccion")]
        public string Direccion { get; set; }

        [Column("correo")]
        public string Correo { get; set; }
    }
}
