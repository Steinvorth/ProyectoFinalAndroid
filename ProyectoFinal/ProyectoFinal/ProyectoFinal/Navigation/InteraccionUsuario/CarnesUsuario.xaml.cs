using ProyectoFinal.Navigation.ObjEdit;
using ProyectoFinal.SupaBase.Tablas;
using ProyectoFinal.SupaBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Navigation.InteraccionUsuario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarnesUsuario : ContentPage
    {
        SupaBaseDB supabase = new SupaBaseDB();

        public CarnesUsuario()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCarnes();
        }

        private async Task LoadCarnes()
        {
            var carnes = await supabase.GetProductosAsync(1);
            if (carnes != null)
            {
                lstCarnes.ItemsSource = carnes;
            }
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            Button editButton = (Button)sender;
            Producto item = (Producto)editButton.BindingContext;

            // Navigate to the edit page and pass the selected product as a parameter
            await Navigation.PushAsync(new EditarProducto(item));
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
                await LoadCarnes();

                //mesaje de Success
                await DisplayAlert("Success", "Se ha eliminado el producto Exitosamente!", "OK");
            }
            catch (Exception ex)
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