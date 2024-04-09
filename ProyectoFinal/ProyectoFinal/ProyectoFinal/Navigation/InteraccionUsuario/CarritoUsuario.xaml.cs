using Newtonsoft.Json.Serialization;
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
            comprar_btn.Clicked += comprar_btn_Clicked;
        }

        private async void comprar_btn_Clicked(object sender, EventArgs e)
        {
            try
            {
                clienteId = await supabase.GetClienteID(username);
                carritoId = await supabase.GetCarritoID(clienteId);

                int ordenId = await supabase.InsertOrdenCompra(new OrdenCompra
                {
                    Id_Cliente = clienteId,
                    Estado = "En Proceso",
                    Total = carritoItems.Sum(x => x.Subtotal),
                    Fecha_Creacion = DateTime.Now
                });

                var detalleCarrito = await supabase.GetDetalleCarritoCliente(carritoId);

                foreach (var detalle in detalleCarrito)
                {
                    await supabase.InsertDetalleOrden(new DetalleOrden
                    {
                        Id_Producto = detalle.Id_Producto,
                        Cantidad = detalle.Cantidad,
                        Precio_Unitario = detalle.Precio_Unitario,
                        Subtotal = detalle.Subtotal,
                        Id_Orden = ordenId
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al poner la orden.", "OK");
                Debug.WriteLine("Error: " + ex.Message);
            }
            
        }

        public async void borrar_item(object sender, EventArgs e)
        {
            // conseguimos el boton
            var button = sender as Button;

            // conseguimos el binding context
            var item = button.BindingContext as CarritoItem;

            // conseguimos el produicto id
            int productoId = (int)button.CommandParameter;

            // vemos si existe el item
            if (item != null)
            {
                // quitamos el item deseado de la lista
                carritoItems.Remove(item);

                // Actualizamos para reflejar los cambios
                listView.ItemsSource = null;
                listView.ItemsSource = carritoItems;

                float subtotal = carritoItems.Sum(x => x.Subtotal);
                lbl_totalCompra.Text = $"Total: {subtotal}";

                // borramos de la base de datos del carrito
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

            float subtotal = carritoItems.Sum(x => x.Subtotal);

            lbl_totalCompra.Text = $"Total: {subtotal}";
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