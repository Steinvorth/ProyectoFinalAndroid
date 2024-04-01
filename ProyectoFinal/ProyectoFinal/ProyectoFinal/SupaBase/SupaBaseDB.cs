using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supabase;
using Supabase.Interfaces;

namespace ProyectoFinal.SupaBase
{
    public class SupaBaseDB
    {
        private readonly Supabase.Client _supabase;

        public SupaBaseDB()
        {
            var url = Environment.GetEnvironmentVariable("https://qzanjpvwgsiirxipiqxx.supabase.co");
            var key = Environment.GetEnvironmentVariable("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InF6YW5qcHZ3Z3NpaXJ4aXBpcXh4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MTE5OTY0NTYsImV4cCI6MjAyNzU3MjQ1Nn0.9IP1L0mbRSnh0-YkOWL7d_PfRPSFA27IjEta9zTepbQ");

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

        public async Task<List<Cliente>> GetClientesAsync()
        {
            var result = await _supabase.From<Cliente>().Get();
            return result.Models;
        }

        public async Task InsertClienteAsync(Cliente cliente)
        {
            await _supabase.From<Cliente>().Insert(cliente);
        }

        public async Task UpdateClienteAsync(int clienteId, string newName)
        {
            await _supabase
                .From<Cliente>()
                .Where(x => x.Id == clienteId)
                .Set(x => x.Nombre, newName)
                .Update();
        }

        public async Task DeleteClienteAsync(int clienteId)
        {
            await _supabase
                .From<Cliente>()
                .Where(x => x.Id == clienteId)
                .Delete();
        }
    }
}
