using System;
using System.Collections.Generic;
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
        public MenuPrincipal()
        {
            InitializeComponent();

            comida_cong.Clicked += comida_cong_Clicked;
            carnes.Clicked += carnes_ClickedAsync;
            log_out.Clicked += log_out_Clicked;
            help.Clicked += help_ClickedAsync;
        }

        private async void help_ClickedAsync(object sender, EventArgs e)
        {
            string url = $"https://www.youtube.com/watch?v=eWULmSLfu3E&ab_channel=GOALPROJECT";
            await Launcher.OpenAsync(url);
        }

        //Navigation a Carnes Page
        private async void carnes_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Carnes());
        }

        //Navigation a Comidas Page
        private async void comida_cong_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AlimentosCongelados());
        }

        //Boton de LogOut, Este boton devuelve al LOGIN.
        private void log_out_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage()); //devuelve a la pagina principal.
        }
    }
}