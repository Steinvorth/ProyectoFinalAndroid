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
        int clienteId, carritoId, carritoDetalleId, productoId;
        SupaBaseDB supabase = new SupaBaseDB();

        List<CarritoItem> carritoItems = new List<CarritoItem>(); //Lista para almacenar los datos juntos y hacer el list item correctamente


        public CarritoUsuario(string username)
        {
            InitializeComponent();
            this.username = username;

            GetCarritoDetails();
        }

        public async void borrar_item(object sender, EventArgs e)
        {
            // Cast sender to Button
            var button = sender as Button;

            // Get the BindingContext, which should be the CarritoItem
            var item = button.BindingContext as CarritoItem;

            // Retrieve the ProductoId from the CommandParameter
            int productoId = (int)button.CommandParameter;

            // Check if the item is not null
            if (item != null)
            {
                // Remove the item from the collection
                carritoItems.Remove(item);

                // Update the ListView's ItemsSource to reflect the changes
                listView.ItemsSource = null;
                listView.ItemsSource = carritoItems;

                // Delete the item from the database using the retrieved ProductoId
                await supabase.DeleteDetalleCarrito(carritoId, productoId);
            }
        }

        private async void GetCarritoDetails()
        {
            clienteId = await supabase.GetClienteID(username);
            carritoId = await supabase.GetCarritoID(clienteId);

            var detalleCarrito = await supabase.GetDetalleCarritoCliente(carritoId);
            
            foreach (var detalle in detalleCarrito)
            {
                productoId = detalle.Id_Producto;

                string nombreProd = await supabase.GetProductosName(productoId);

                carritoItems.Add(new CarritoItem
                {
                    Nombre = nombreProd,
                    Cantidad = detalle.Cantidad,
                    Precio_Unitario = detalle.Precio_Unitario,
                    Subtotal = detalle.Subtotal,
                    ProductoId = detalle.Id_Producto
                    
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
        public int ProductoId { get; set; }
    }

}