using ProyectoFinal.SupaBase;
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

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Handle item selection here if needed
        }
    }
}