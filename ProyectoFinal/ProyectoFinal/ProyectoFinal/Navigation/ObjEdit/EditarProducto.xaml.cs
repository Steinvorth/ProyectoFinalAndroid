using ProyectoFinal.SupaBase.Tablas;
using System;
using System.Collections.Generic;
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
        private object item; // Object representing the item to be edited
        private StackLayout stackLayout; // Declare stackLayout at the class level

        public EditarProducto(object item)
        {
            InitializeComponent();
            this.item = item;
            CreateUI(); // Create the UI dynamically
        }

        private void CreateUI()
        {
            stackLayout = new StackLayout(); // Initialize stackLayout
            Content = stackLayout; // Set the content of the page to stackLayout

            var properties = item.GetType().GetProperties();

            foreach (var property in properties)
            {
                // Exclude properties you want to hide (e.g., base URL)
                if (property.Name == "BaseUrl" || property.Name == "TableName")
                    continue;

                // Create UI elements based on property types
                if (property.PropertyType == typeof(string) ||
                    property.PropertyType == typeof(int) ||
                    property.PropertyType == typeof(float) ||
                    property.PropertyType == typeof(DateTime))
                {
                    // Create Label for property name
                    Label propertyNameLabel = new Label();
                    propertyNameLabel.Text = property.Name;
                    stackLayout.Children.Add(propertyNameLabel);

                    // Create Entry for string properties
                    if (property.PropertyType == typeof(string))
                    {
                        Entry entry = new Entry();
                        entry.Text = property.GetValue(item)?.ToString();
                        stackLayout.Children.Add(entry);
                    }
                    // Create Entry for int properties
                    else if (property.PropertyType == typeof(int))
                    {
                        Entry entry = new Entry();
                        entry.Keyboard = Keyboard.Numeric;
                        entry.Text = property.GetValue(item)?.ToString();
                        stackLayout.Children.Add(entry);
                    }
                    // Create Entry for float properties
                    else if (property.PropertyType == typeof(float))
                    {
                        Entry entry = new Entry();
                        entry.Keyboard = Keyboard.Numeric;
                        entry.Text = property.GetValue(item)?.ToString();
                        stackLayout.Children.Add(entry);
                    }
                    // Create DatePicker for DateTime properties
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        DatePicker datePicker = new DatePicker();
                        datePicker.Date = (DateTime)property.GetValue(item);
                        stackLayout.Children.Add(datePicker);
                    }
                    // Add more conditions for other property types as needed
                }
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Update the item with the new values from the UI elements
            var properties = item.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    // Update string properties
                    Entry entry = stackLayout.Children.FirstOrDefault(c => c.GetType() == typeof(Entry)) as Entry;
                    if (entry != null)
                        property.SetValue(item, entry.Text);
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    // Update DateTime properties
                    DatePicker datePicker = stackLayout.Children.FirstOrDefault(c => c.GetType() == typeof(DatePicker)) as DatePicker;
                    if (datePicker != null)
                        property.SetValue(item, datePicker.Date);
                }
                // Add more conditions for other property types as needed
            }

            // Perform any other necessary actions (e.g., save changes to the database)
            // Navigation.PopAsync(); // Navigate back to the previous page
        }
    }
}