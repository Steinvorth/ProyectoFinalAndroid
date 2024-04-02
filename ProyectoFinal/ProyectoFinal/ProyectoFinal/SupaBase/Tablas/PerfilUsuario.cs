using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("PerfilUsuario")]
    public class PerfilUsuario : BaseModel
    {
        [PrimaryKey("id_perfil")]
        public long Id { get; set; } // Assuming int8 in the database corresponds to long in C#

        [Column("id_cliente")]
        public int Id_Cliente { get; set; }

        [Column("preferencias")]
        public string Preferencias { get; set; }

        [Column("direccion_envio_predeterminada")]
        public string Direccion_Envio_Predeterminada { get; set; }

    }
}
