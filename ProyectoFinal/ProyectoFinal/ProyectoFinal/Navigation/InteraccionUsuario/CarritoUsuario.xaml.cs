using ProyectoFinal.SupaBase;
using ProyectoFinal.SupaBase.Tablas;
using Supabase.Storage.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Navigation.InteraccionUsuario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarritoUsuario : ContentPage
    {
        string username;
        int clienteId;
        SupaBaseDB supabase = new SupaBaseDB();

        List<CarritoItem> carritoItems = new List<CarritoItem>(); //Lista para almacenar los datos juntos y hacer el list item correctamente


        public CarritoUsuario(string username)
        {
            InitializeComponent();
            this.username = username;

            GetCarritoDetails();
        }

        public void borrar_item(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var item = menuItem.CommandParameter as CarritoItem;

            carritoItems.Remove(item);
            listView.ItemsSource = null;
            listView.ItemsSource = carritoItems;
        }

        private async void GetCarritoDetails()
        {
            clienteId = await supabase.GetClienteID(username);
            int carritoId = await supabase.GetCarritoID(clienteId);

            var detalleCarrito = await supabase.GetDetalleCarritoCliente(carritoId);
            
            foreach (var detalle in detalleCarrito)
            {
                int idProducto = detalle.Id_Producto;

                string nombreProd = await supabase.GetProductosName(idProducto);

                carritoItems.Add(new CarritoItem
                {
                    Nombre = nombreProd,
                    Cantidad = detalle.Cantidad,
                    Precio_Unitario = detalle.Precio_Unitario,
                    Subtotal = detalle.Subtotal
                });
            }

            listView.ItemsSource = carritoItems;

        }
    }

    //clase Carrito Item, solamente sirve para formar la lista. Por Esta razon no tiene un archivo dedicado.
    public class CarritoItem
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public float Precio_Unitario { get; set; }
        public float Subtotal { get; set; }

    }

}