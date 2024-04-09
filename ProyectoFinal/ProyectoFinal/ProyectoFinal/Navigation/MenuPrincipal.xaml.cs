using ProyectoFinal.Navigation.InteraccionUsuario;
using ProyectoFinal.Navigation.ObjEdit;
using ProyectoFinal.SupaBase;
using ProyectoFinal.SupaBase.Tablas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPrincipal : ContentPage
    {
        string username;
        SupaBaseDB supabase = new SupaBaseDB();
        public MenuPrincipal(string usuario)
        {
            InitializeComponent();

            this.username = usuario;

            comida_cong.Clicked += comida_cong_Clicked;
            carnes.Clicked += carnes_ClickedAsync;
            log_out.Clicked += log_out_Clicked;
            help.Clicked += help_ClickedAsync;

            carrito.Clicked += carrito_Clicked;
            editar_cuenta.Clicked += editar_cuenta_Clicked;
        }

        private async void editar_cuenta_Clicked(object sender, EventArgs e)
        {
            // Retrieve the list of Cliente objects
            List<Cliente> clientes = await supabase.GetClientesAsync(username);

            // Check if any Cliente objects were retrieved
            if (clientes != null && clientes.Count > 0)
            {
                // Get the first Cliente object from the list
                Cliente cliente = clientes[0];

                // Navigate to the EditarProducto page and pass the Cliente object
                await Navigation.PushAsync(new EditarProducto(cliente));
            }
            else
            {
                // Handle the case where no Cliente objects were found
                // Display an alert or perform any other action as needed
                await DisplayAlert("Error", "No se encontró ningún cliente para editar.", "OK");
            }
        }


        private async void carrito_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CarritoUsuario(username));
        }

        private async void help_ClickedAsync(object sender, EventArgs e)
        {
            string url = $"https://www.youtube.com/watch?v=eWULmSLfu3E&ab_channel=GOALPROJECT";
            await Launcher.OpenAsync(url);
        }

        //Navigation a Carnes Page
        private async void carnes_ClickedAsync(object sender, EventArgs e)
        {
            if (username.Equals("admin")) 
            {
                await Navigation.PushAsync(new Carnes());
            }
            else
            {
                await Navigation.PushAsync(new CarnesUsuario(username));
            }
            
        }

        //Navigation a Comidas Page
        private async void comida_cong_Clicked(object sender, EventArgs e)
        {
            if (username.Equals("admin"))
            {
                await Navigation.PushAsync(new AlimentosCongelados());
            }
            else
            {
                await Navigation.PushAsync(new AlimentosCongeladosUsuario(username));
            }
            
        }

        //Boton de LogOut, Este boton devuelve al LOGIN.
        private void log_out_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage()); //devuelve a la pagina principal.
        }
    }
}