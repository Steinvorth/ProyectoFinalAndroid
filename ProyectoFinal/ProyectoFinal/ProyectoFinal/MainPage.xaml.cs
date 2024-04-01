﻿using System;
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

        private async void InitializeSupaBase()
        {
            supaBase = new SupaBaseDB();
            await GetClientes();
        }

        public async Task GetClientes()
        {
            var clientes = await supaBase.GetClientesAsync();
            foreach (var cliente in clientes)
            {
                Debug.WriteLine($"Id: {cliente.Id}, Nombre: {cliente.Nombre}, Apellido: {cliente.Apellido}, Usuario: {cliente.Usuario}, NumTel: {cliente.NumTel}");
            }
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
            }
        }

    }
}
