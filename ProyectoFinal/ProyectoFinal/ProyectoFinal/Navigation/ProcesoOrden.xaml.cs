using ProyectoFinal.SupaBase;
using ProyectoFinal.SupaBase.Tablas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProcesoOrden : ContentPage
    {
        string fecha, estado;
        int ordenId;
        float total;

        List<OrdenCompra> ordenes = new List<OrdenCompra>();
        SupaBaseDB supabase = new SupaBaseDB();

        public ProcesoOrden()
        {
            InitializeComponent();
            GetOrdenes();

        }


        async void GetOrdenes()
        {
            var ordenes = await supabase.SelectOrdenCompra();
            listView.ItemsSource = ordenes;
        }

        public async void borrar_item(object sender, EventArgs e)
        {
            try
            {
                var button = (ImageButton)sender;
                var ordenId = (int)button.CommandParameter;

                //if we select yes, then delete
                if (await DisplayAlert("Alerta", $"¿Estás seguro de que quieres eliminar la orden?", "Sí", "No"))
                {
                    await supabase.DeleteOrdenCompra(ordenId);
                    GetOrdenes();

                    await DisplayAlert("Exito", $"Se ha eliminado la orden con exito!", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al borrar la oreden de compra!", "Ok");
                Debug.WriteLine("Error al borrar la orden de compra: " + ex);
            }
        }

        public async void check_item(object sender, EventArgs e)
        {
            try
            {
                var button = (ImageButton)sender;
                var ordenId = (int)button.CommandParameter;

                if (await DisplayAlert("Alerta", $"¿Estás seguro de que quieres marcar la orden como entregada?", "Sí", "No"))
                {
                    await supabase.CompletarOrdenCompra(ordenId, "Completado");
                    GetOrdenes();

                    await DisplayAlert("Exito", $"Se ha completado la orden con exito!", "Ok");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "Error al marcar la orden de compra como completada!", "Ok");
                Debug.WriteLine("Error al marcar la orden de compra como completada: " + ex);
            }
            
        }

    }
}