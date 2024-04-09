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
        string username, direccion;
        int clienteId, carritoId, carritoDetalleId, productoId;
        SupaBaseDB supabase = new SupaBaseDB();

        List<CarritoItem> carritoItems = new List<CarritoItem>(); //Lista para almacenar los datos juntos y hacer el list item correctamente


        public CarritoUsuario(string username)
        {
            InitializeComponent();
            this.username = username;

            GetCarritoDetails();
            comprar_btn.Clicked += comprar_btn_Clicked;
            btn_eliminarCarrito.Clicked += btn_eliminarCarrito_Clicked;
        }

        private async void btn_eliminarCarrito_Clicked(object sender, EventArgs e)
        {
            // Mostramos un alert de alerta para confirmar si se quiere borrar el carrito
            bool answer = await DisplayAlert("Confirmar", "¿Estás seguro de que quieres eliminar el carrito?", "Sí", "No");

            // Vemos que contesto el usuario
            if (answer)
            {
                await ClearCarrito();
            }
        }

        private async Task ClearCarrito()
        {
            // confirmarmos que se quiere borrar
            await supabase.DeleteDetalleCarritoAll(carritoId);
            carritoItems.Clear();

            listView.ItemsSource = null;
            listView.ItemsSource = carritoItems;

            // Refresh the cart details
            GetCarritoDetails();
        }

        private async void comprar_btn_Clicked(object sender, EventArgs e)
        {
            try
            {  
                //Si el metodo de pago no esta vacio
                if (!metodoPago.Text.Equals(""))
                {
                    //Revisamos que la direccion del cliente no sea null
                    if(direccion.Equals("") || direccion == null)
                    {
                        throw new Exception("No hay una direccion en su cuenta! Agreguela antes de comprar.");
                    }
                    
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

                    

                    //hay que insertar el pago
                    await supabase.InsertPago(new Pago
                    {
                        Id_Orden = ordenId,
                        Metodo_Pago = metodoPago.Text,
                        Monto = carritoItems.Sum(x => x.Subtotal),
                        Fecha_Pago = DateTime.Now,
                        Direccion = direccion
                    });

                    await DisplayAlert("Orden", "Orden puesta con exito.", "OK");

                    //limpiamos el carrito y ademas limpiamos la lista
                    await ClearCarrito();
                    metodoPago.Text = "";
                }
                else
                {
                    await DisplayAlert("Error", "El metodo de pago esta vacio!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al poner la orden: {ex.Message}", "OK");
                Debug.WriteLine("Error: " + ex.Message);
            }
            
        }

        public async void borrar_item(object sender, EventArgs e)
        {
            // Mostramos un alert de alerta para confirmar si se quiere borrar el carrito
            bool answer = await DisplayAlert("Confirmar", "¿Estás seguro de que quieres eliminar el carrito?", "Sí", "No");

            if (answer)
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
        }

        private async void GetCarritoDetails()
        {
            try
            {
                //conseguimos la direccion del cliente desde el inicio para asi poder ingresarla al pago.
                var clientes = await supabase.GetClientesAsync(username);

                if (clientes != null && clientes.Count > 0)
                {
                    var cliente = clientes[0];
                    if (cliente.Direccion == null || cliente.Direccion.Equals("")) { direccion = ""; }

                    else { direccion = cliente.Direccion; }

                }

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
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
            }
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