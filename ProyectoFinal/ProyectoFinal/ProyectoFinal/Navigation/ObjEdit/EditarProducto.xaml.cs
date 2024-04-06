using ProyectoFinal.SupaBase;
using ProyectoFinal.SupaBase.Tablas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Navigation.ObjEdit
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditarProducto : ContentPage
	{
        private object item; //Objeto de la tabla que vamos a editar
        private StackLayout stackLayout; // Stacklayout que se utilizara para formar el UI dinamicamente a base del objeto que se utilize
        SupaBaseDB supaBase = new SupaBaseDB();


        private Dictionary<string, Entry> entryMap = new Dictionary<string, Entry>();


        public EditarProducto(object item)
        {
            InitializeComponent();
            this.item = item;
            CreateUI();
        }

        private void CreateUI()
        {
            stackLayout = new StackLayout();
            Content = stackLayout;

            var properties = item.GetType().GetProperties();

            foreach (var property in properties)
            {
                // Exclude certain properties by name
                if (property.Name == "BaseUrl" || property.Name == "TableName" || property.Name == "RequestClientOptions" || property.Name == "PrimaryKey")
                    continue;

                Label propertyNameLabel = new Label();
                propertyNameLabel.Text = property.Name;
                stackLayout.Children.Add(propertyNameLabel);

                if (property.PropertyType == typeof(string) ||
                    property.PropertyType == typeof(int) ||
                    property.PropertyType == typeof(float))
                {
                    Entry entry = new Entry();
                    entry.Text = property.GetValue(item)?.ToString();
                    stackLayout.Children.Add(entry);
                    entryMap[property.Name] = entry; // Store the entry in the dictionary
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    DatePicker datePicker = new DatePicker();
                    datePicker.Date = (DateTime)property.GetValue(item);
                    stackLayout.Children.Add(datePicker);
                }
            }

            Button saveButton = new Button();
            saveButton.Text = "Save";
            saveButton.Clicked += SaveButton_Clicked;
            stackLayout.Children.Add(saveButton);
        }


        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                foreach (var property in item.GetType().GetProperties())
                {
                    if (property.Name == "BaseUrl" || property.Name == "TableName")
                        continue;

                    if (entryMap.ContainsKey(property.Name))
                    {
                        var entry = entryMap[property.Name];
                        string value = entry.Text;
                        // Update the value of the corresponding property
                        property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
                    }
                }

                // Update the Producto
                await supaBase.UpdateProducto((Producto)item);

                Debug.WriteLine("Producto updated successfully.");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating Producto: " + ex.Message);
                // Handle the error accordingly
            }
        }
    }
}