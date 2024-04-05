using ProyectoFinal.SupaBase;
using ProyectoFinal.SupaBase.Tablas;
using Supabase.Storage.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Carnes : ContentPage
    {
        SupaBaseDB supabase = new SupaBaseDB();

        public Carnes()
        {
            InitializeComponent();
            LoadCarnes();
        }

        private async void LoadCarnes()
        {
            var carnes = await supabase.GetProductosAsync();
            if (carnes != null)
            {
                lstCarnes.ItemsSource = carnes;
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Conseguimos el boton que fue presionado
                Button deleteButton = (Button)sender;

                // Conseguimos el objeto de Producto
                Producto item = (Producto)deleteButton.BindingContext;

                // Extraemos el ID del objeto
                int itemId = item.Id;

                // llamamos el Task Delete
                await DeleteProducto(itemId);

                //refresh page                
                LoadCarnes();

                //mesaje de Success
                await DisplayAlert("Success", "Se ha eliminado el producto Exitosamente!", "OK");
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", $"Error en base de datos: {ex}", "OK");
            }
        }

        private async Task DeleteProducto(int itemId)
        {
            await supabase.DeleteProducto(itemId);
        }
    }
}