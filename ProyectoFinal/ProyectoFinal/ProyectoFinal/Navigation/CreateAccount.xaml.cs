using ProyectoFinal.SupaBase;
using ProyectoFinal.SupaBase.Tablas;
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
    public partial class CreateAccount : ContentPage
    {
        SupaBaseDB supaBase = new SupaBaseDB();
        public CreateAccount()
        {
            InitializeComponent();
            createAcc_btn.Clicked += CreateAccButton_ClickedAsync;
        }

        private async void CreateAccButton_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                string nombre = NombreEntry.Text;
                string apellido = ApellidoEntry.Text;
                string usuario = UsuarioEntry.Text;
                string num_tel = NumTelEntry.Text;
                string pass = PassEntry.Text;

                await supaBase.InsertClienteAsync(new Cliente
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Usuario = usuario,
                    NumTel = num_tel,
                    Pass = pass
                });

                // Optionally, show a success message or navigate to another page
                await DisplayAlert("Success", "Account created successfully", "OK");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the database operation
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}