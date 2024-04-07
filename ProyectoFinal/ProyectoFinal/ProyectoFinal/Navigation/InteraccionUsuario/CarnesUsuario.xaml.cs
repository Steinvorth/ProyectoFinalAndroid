using ProyectoFinal.Navigation.ObjEdit;
using ProyectoFinal.Navigation.ObjComprar;
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
        string usuario;

        public CarnesUsuario(string username)
        {
            InitializeComponent();
            this.usuario = username;
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

        private async void Comprar_Clicked(object sender, EventArgs e)
        {
            Button editButton = (Button)sender;
            Producto item = (Producto)editButton.BindingContext;

            // Navigate to the edit page and pass the selected product as a parameter
            await Navigation.PushAsync(new ComprarProducto(item));
        }
    }
}