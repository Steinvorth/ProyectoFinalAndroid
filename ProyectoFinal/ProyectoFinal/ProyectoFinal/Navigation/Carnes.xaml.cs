using ProyectoFinal.Navigation.ObjEdit;
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
            btn_agregar.Clicked += Btn_agregar_Clicked;
        }

        private async void Btn_agregar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgregarProducto());
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
                bool answer = await DisplayAlert("Confirmar", "¿Estás seguro de que quieres eliminar el producto?", "Sí", "No");
                if (answer)
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