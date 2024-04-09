using ProyectoFinal.SupaBase;
using ProyectoFinal.SupaBase.Tablas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarProducto : ContentPage
    {
        SupaBaseDB supabase = new SupaBaseDB();

        int id_categoria;

        public AgregarProducto()
        {
            InitializeComponent();
            btn_add.Clicked += Btn_add_Clicked;
        }

        private async void Btn_add_Clicked(object sender, EventArgs e)
        {
            try
            {
                string cantidad = cantidadEntry.Text;
                string descripcion = descripcionEntry.Text;
                string nombre = nombreEntry.Text;
                float precio = float.Parse(precioEntry.Text);
                int codigo = int.Parse(codigoEntry.Text);

                await supabase.InsertProducto(new Producto
                {
                    Cantidad = cantidad,
                    Descripcion = descripcion,
                    Nombre = nombre,
                    Precio = precio,
                    Codigo = codigo,
                    Id_Categoria = id_categoria
                });

                await Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "Error al insertar el producto", "Ok");
                Debug.WriteLine(ex.Message);
            }            
        }

        private void CategoriaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Conseguimos la categoria seleccionada
            string selectedCategory = categoriaPicker.SelectedItem as string;

            if (selectedCategory == "Carnes")
            {
                id_categoria = 1;
            }
            else
            {
                id_categoria = 2;
                
            }

            Debug.WriteLine($"Selected category: {selectedCategory}, ID: {id_categoria}");
        }
    }
}