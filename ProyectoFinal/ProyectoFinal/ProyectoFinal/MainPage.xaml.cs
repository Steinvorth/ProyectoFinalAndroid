using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Supabase;
using ProyectoFinal.SupaBase;
using System.Diagnostics;
using ProyectoFinal.Navigation; //Folder de SupaBase connection.

namespace ProyectoFinal
{
    public partial class MainPage : ContentPage
    {
        SupaBaseDB supaBase = new SupaBaseDB();

        public MainPage()
        {
            InitializeComponent();
            InitializeSupaBase();

            login_btn.Clicked += login_btn_Clicked;
            createAcc_btn.Clicked += createAcc_btn_Clicked;
        }

        private void createAcc_btn_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new CreateAccount());
        }

        private async void InitializeSupaBase()
        {
            supaBase = new SupaBaseDB();
            await GetBD();
        }

        public async Task GetBD()
        {
            var clientes = await supaBase.GetClientesAsync();
            foreach (var cliente in clientes)
            {
                Debug.WriteLine($"Id: {cliente.Id}, Nombre: {cliente.Nombre}, Apellido: {cliente.Apellido}, Usuario: {cliente.Usuario}, NumTel: {cliente.NumTel}, Correo {cliente.Correo}, Direccion: {cliente.Direccion}");
            }

            var carritos = await supaBase.GetCarritosAsync();
            foreach (var carrito in carritos)
            {
                Debug.WriteLine($"Id: {carrito.Id}, Fecha Creacion: {carrito.Fecha_Creacion}, Estado: {carrito.Estado}, Cliente: {carrito.Cliente}");
            }

            var categorias = await supaBase.GetCategoriasAsync();
            foreach (var categoria in categorias)
            {
                Debug.WriteLine($"Id: {categoria.Id}, Nombre: {categoria.Nombre}");
            }

            var detalleCarrito = await supaBase.GetDetalleCarritosAsync();
            foreach (var detCarrito in detalleCarrito)
            {
                Debug.WriteLine($"Id: {detCarrito.Id}, Id_Carrito: {detCarrito.Id_Carrito}, Id_Producto: {detCarrito.Id_Producto}, Cantidad: {detCarrito.Cantidad}, PrecioUnitario: {detCarrito.Precio_Unitario}, Subtotal {detCarrito.Subtotal}");
            }

            var deleteCliente = supaBase.DeleteClienteAsync(5);
            Debug.WriteLine($"Cliete Eliminado: {deleteCliente}");

            var updateCliente = supaBase.UpdateClienteAsync(4, "Juanito"); //Actualizo el Nombre "Pedrito, ID=4" a "Juanito ID=4"
            Debug.WriteLine($"Cliente Actualizado: {updateCliente}");
        }

        public async void login_btn_Clicked(object sender, EventArgs e)
        {
            string usuario = user_entry.Text;
            string pass = pass_entry.Text;
            var res = await supaBase.LoginAsync(usuario, pass);

            if(!res)
            {
                await DisplayAlert("Error", "Usuario o Pass Incorrecto", "OK");
            }
            else
            {
                await DisplayAlert("Success", "Login Correcto!", "OK");
            }
        }

    }
}
