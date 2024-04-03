﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ProyectoFinal.SupaBase.Tablas;
using Supabase;
using Supabase.Interfaces;

namespace ProyectoFinal.SupaBase
{
    public class SupaBaseDB
    {
        private readonly Supabase.Client _supabase;

        public SupaBaseDB()
        {
            
            var url = "https://qzanjpvwgsiirxipiqxx.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InF6YW5qcHZ3Z3NpaXJ4aXBpcXh4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MTE5OTY0NTYsImV4cCI6MjAyNzU3MjQ1Nn0.9IP1L0mbRSnh0-YkOWL7d_PfRPSFA27IjEta9zTepbQ";

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("SUPABASE_URL or SUPABASE_KEY environment variables are not set.");
            }

            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            _supabase = new Supabase.Client(url, key, options);
            
        }

        //CRUD Cliente
        public async Task<List<Cliente>> GetClientesAsync()
        {
            try
            {
                var result = await _supabase.From<Cliente>().Get();
                return result.Models;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error getting Clientes: " + ex.Message);
                return null;
            }            
        }

        public async Task InsertClienteAsync(Cliente cliente)
        {
            try
            {
                await _supabase.From<Cliente>().Insert(cliente);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error inserting Cliente: " + ex.Message);
            }
            
        }

        public async Task UpdateClienteAsync(int clienteId, string newName)
        {
            try //intenta actualizar el cliente del ID ingresado, actualiza el nombre.
            {
                await _supabase
                .From<Cliente>()
                .Where(x => x.Id == clienteId)
                .Set(x => x.Nombre, newName)
                .Update();
            }
            catch(Exception ex) //Si no fue posible, tira una excepcion con el error.
            {
                Debug.WriteLine($"Error updating Cliente with ID {clienteId}: {ex.Message}");
            }
            
        }

        public async Task DeleteClienteAsync(int clienteId)
        {
            try
            {
                await _supabase
                .From<Cliente>()
                .Where(x => x.Id == clienteId)
                .Delete();

                Debug.WriteLine($"Cliente with ID {clienteId} has been deleted.");
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error deleting Cliente with ID {clienteId}: {ex.Message}");
            }
            
        }


        public async Task<bool> LoginAsync(string usuario, string password)
        {
            try
            {
                // Query the Clientes table for the provided usuario and password
                var result = await _supabase
                    .From<Cliente>()
                    .Where(x => x.Usuario == usuario && x.Pass == password)
                    .Get();

                // If a matching usuario and password are found, return true; otherwise, return false
                return result.Models.Count > 0;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error logging in: " + ex.Message);
                return false;
            }
            
        }

        //CRUD Carrito
        public async Task<List<Carrito>> GetCarritosAsync()
        {
            var result = await _supabase.From<Carrito>().Get();
            return result.Models;
        }

        //CRUD Categoria
        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            var result = await _supabase.From<Categoria>().Get();
            return result.Models;
        }

        //CRUD DetalleCarrito
        public async Task<List<DetalleCarrito>> GetDetalleCarritosAsync()
        {
            var result = await _supabase.From<DetalleCarrito>().Get();
            return result.Models;
        }

    }
}
