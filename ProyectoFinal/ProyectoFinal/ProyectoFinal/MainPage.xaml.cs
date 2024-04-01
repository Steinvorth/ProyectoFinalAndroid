using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Supabase;
using ProyectoFinal.SupaBase;
using System.Diagnostics; //Folder de SupaBase connection.

namespace ProyectoFinal
{
    public partial class MainPage : ContentPage
    {
        SupaBaseDB supaBase = new SupaBaseDB();

        public MainPage()
        {
            InitializeComponent();
            InitializeSupaBase();
        }

        private async void InitializeSupaBase()
        {
            supaBase = new SupaBaseDB();
            await GetClientes();
        }

        public async Task GetClientes()
        {
            var clientes = await supaBase.GetClientesAsync();
            foreach (var cliente in clientes)
            {
                Debug.WriteLine($"Id: {cliente.Id}, Nombre: {cliente.Nombre}, Apellido: {cliente.Apellido}, Usuario: {cliente.Usuario}, NumTel: {cliente.NumTel}");
            }
        }

    }
}
