using ProyectoFinal.SupaBase.Tablas;
using ProyectoFinal.SupaBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;

namespace ProyectoFinal.Navigation.ObjComprar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComprarProducto : ContentPage
    {
        private object item; //Objeto de la tabla que vamos a editar
        SupaBaseDB supaBase = new SupaBaseDB();

        string nombre, descripcion, cantidad, username;
        float precio, montoTotal;
        int cantidadInicial = 1;

        public ComprarProducto(object item, string username)
        {
            InitializeComponent();
            this.item = item;
            this.username = username;
            GetProducto();

            btn_mas.Clicked += Btn_mas_Clicked;
            btn_menos.Clicked += Btn_menos_Clicked;
            btn_carrito.Clicked += Btn_carrito_Clicked;

            entry_cantidad.Text = cantidadInicial.ToString();
        }

        private async void Btn_carrito_Clicked(object sender, EventArgs e)
        {
            var clientes = await supaBase.GetClientesAsync(username);

            // Assuming there's only one cliente for the given username
            if (clientes != null && clientes.Count > 0)
            {
                var cliente = clientes[0];
                int cliente_id = cliente.Id;

                // verificamos si existe un carrito del cliente
                var carritos = await supaBase.GetCarritosAsync();
                bool carritoExists = carritos.Any(c => c.Id_Cliente == cliente_id);

                if (carritoExists)
                {
                    Debug.WriteLine("Carrito already exists for the cliente.");

                    //agregamos la "orden" al carrito
                    return;
                }

                // Si no existe, entonces le creamos uno.
                try
                {
                    await supaBase.InsertCarrito(new Carrito
                    {
                        Id_Cliente = cliente_id
                    });

                    Debug.WriteLine("New Carrito created successfully!");

                    //le agregamos la orden al carrito
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error creating Carrito: " + ex.Message);
                }
            }
            else
            {
                Debug.WriteLine("Cliente not found or no clientes retrieved.");
            }
        }

        private void Btn_menos_Clicked(object sender, EventArgs e)
        {
            if (cantidadInicial != 1)
            {
                cantidadInicial--;
                entry_cantidad.Text = cantidadInicial.ToString();
                montoTotal = precio * cantidadInicial;
                total_lbl.Text = "Total: " + montoTotal.ToString();
            }
            else
            {
                DisplayAlert("Error", $"El minimo que se pued comprar es {cantidad}.", "OK");
            }
            
        }

        private void Btn_mas_Clicked(object sender, EventArgs e)
        {
            cantidadInicial++;
            entry_cantidad.Text = cantidadInicial.ToString();
            montoTotal = precio * cantidadInicial;
            total_lbl.Text = "Total: " + montoTotal.ToString();
        }

        public void GetProducto()
        {
            var productoFiltrado = GetProductoFiltrado();

            nombre = productoFiltrado["Nombre"].ToString();
            descripcion = productoFiltrado["Descripcion"].ToString();
            precio = float.Parse(productoFiltrado["Precio"].ToString());
            cantidad = productoFiltrado["Cantidad"].ToString();


            nombre_lbl.Text = nombre;
            descripcion_lbl.Text = descripcion;
            precio_lbl.Text = precio.ToString();
            cantidad_lbl.Text = cantidad;
        }

        public Dictionary<string, object> GetProductoFiltrado()
        {
            var filteredAttributes = new Dictionary<string, object>();

            PropertyInfo[] properties = typeof(Producto).GetProperties();
            foreach (var property in properties)
            {
                // Select only the desired columns
                if (property.Name == "Nombre" ||
                    property.Name == "Descripcion" ||
                    property.Name == "Precio" ||
                    property.Name == "Cantidad")
                {
                    filteredAttributes.Add(property.Name, property.GetValue(item));
                }
            }

            return filteredAttributes;
        }
    }
}