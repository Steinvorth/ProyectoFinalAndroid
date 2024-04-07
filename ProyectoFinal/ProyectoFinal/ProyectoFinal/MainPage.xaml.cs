using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Supabase;
using ProyectoFinal.SupaBase;
using System.Diagnostics;
using ProyectoFinal.Navigation; //Folder de SupaBase connection.

namespace ProyectoFinal
{
    public partial class MainPage : ContentPage
    {
        SupaBaseDB supaBase = new SupaBaseDB();

        public MainPage()
        {
            InitializeComponent();
            InitializeSupaBase();

            login_btn.Clicked += login_btn_Clicked;
            createAcc_btn.Clicked += createAcc_btn_Clicked;
        }

        private void createAcc_btn_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new CreateAccount());
        }

        private void InitializeSupaBase()
        {
            supaBase = new SupaBaseDB();
        }

        public async void login_btn_Clicked(object sender, EventArgs e)
        {
            string usuario = user_entry.Text;
            string pass = pass_entry.Text;
            var res = await supaBase.LoginAsync(usuario, pass);

            if(!res)
            {
                await DisplayAlert("Error", "Usuario o Pass Incorrecto", "OK");
            }
            else
            {
                await DisplayAlert("Success", "Login Correcto!", "OK");

                //El nombre de usuario se va a utilizar para diferenciar entre un usuario final y el ADMIN. El admin puede eliminar y demas, el usuario solo ordernar.
                Application.Current.MainPage = new NavigationPage(new MenuPrincipal(usuario)); //Reemplaza el MainPage por el MenuPrincipal. Esto permite dar el efecto que la flecha para atras sea al Menu Principal.
            }
        }

    }
}
