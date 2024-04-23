using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public async Task<List<Cliente>> GetClientesAsync(string username)
        {
            try
            {
                var result = await _supabase.From<Cliente>()
                    .Where(x => x.Usuario == username)
                    .Get();
                Debug.WriteLine($"Cliente encontrado: {result.Models}");
                return result.Models;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error getting Clientes: " + ex.Message);
                return null;
            }            
        }

        public async Task<int> GetClienteID(string username)
        {
            try
            {
                var result = await _supabase.From<Cliente>()
                    .Where(x => x.Usuario == username)
                    .Get();
                Debug.WriteLine($"Cliente encontrado: {result.Models}");
                return result.Models[0].Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error getting Cliente: " + ex.Message);
                throw new Exception($"Error al encontrar el cliente {username}");                
            }
        }

        public async Task InsertClienteAsync(Cliente cliente)
        {
            try
            {
                await _supabase.From<Cliente>().Insert(cliente);
                Debug.WriteLine($"Cliente: {cliente} inserted successfully.");
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error inserting Cliente: " + ex.Message);
            }            
        }

        public async Task UpdateCliente(Cliente cliente)
        {
            try
            {
                var update = _supabase.From<Cliente>()
                    .Where(x => x.Id == cliente.Id);

                if (cliente.Nombre != null)
                    update = update.Set(x => x.Nombre, cliente.Nombre);

                if (cliente.Usuario != null)
                    update = update.Set(x => x.Usuario, cliente.Usuario);

                if (cliente.NumTel != null)
                    update = update.Set(x => x.NumTel, cliente.NumTel);

                if (cliente.Pass != null)
                    update = update.Set(x => x.Pass, cliente.Pass);

                if (cliente.Apellido != null)
                    update = update.Set(x => x.Apellido, cliente.Apellido);

                if (cliente.Direccion != null)
                    update = update.Set(x => x.Direccion, cliente.Direccion);
                if(cliente.Correo != null)
                    update = update.Set(x => x.Correo, cliente.Correo);

                await update.Update();

                Debug.WriteLine($"Updated Producto: {cliente}");
                Debug.WriteLine($"Producto with Id {cliente.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating Producto: " + ex.Message);
                throw new Exception($"Error Updating Producto: {ex.Message}");
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
                Debug.WriteLine("Login result Successful");
                return result.Models.Count > 0;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error logging in: " + ex.Message);
                return false;
            }
            
        }

        //Crud Productos
        public async Task<List<Producto>> GetProductosAsync(int idCategoria)
        {
            try
            {
                var result = await _supabase.From<Producto>()
                    .Where(x => x.Id_Categoria == idCategoria)
                    .Get();
                return result.Models;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error getting Productos: " + ex.Message);
                return null;
            }            
        }

        public async Task<string> GetProductosName(int IdProducto)
        {
            try
            {
                var result = await _supabase.From<Producto>()
                    .Where(x => x.Id == IdProducto)
                    .Get();
                return result.Models[0].Nombre;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error getting Productos: " + ex.Message);
                return null;
            }
        }

        public async Task DeleteProducto(int productoId)
        {
            try
            {
                await _supabase
                .From<Producto>()
                .Where(x => x.Id == productoId)
                .Delete();

                Debug.WriteLine($"Producto with ID {productoId} has been deleted.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("referenced"))
                {
                    throw new Exception($"No se puede eliminar el producto. El item con id:{productoId} esta en el carrito de algun cliente.");
                }
                else
                {
                    Debug.WriteLine($"Error deleting Producto with ID {productoId}: {ex.Message}");
                    throw new Exception($"Por favor, inténtelo de nuevo más tarde.");
                }                              
            }
        }

        public async Task InsertProducto(Producto producto)
        {
            try
            {
                await _supabase.From<Producto>().Insert(producto);
                Debug.WriteLine($"Producto: {producto} inserted successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error inserting Producto: " + ex.Message);
                throw new Exception("Error al insertar el producto");
            }
        }

        

        public async Task UpdateProducto(Producto producto)
        {
            try
            {
                var update = _supabase.From<Producto>()
                    .Where(x => x.Id == producto.Id);

                if (producto.Nombre != null)
                    update = update.Set(x => x.Nombre, producto.Nombre);

                if (producto.Descripcion != null)
                    update = update.Set(x => x.Descripcion, producto.Descripcion);

                if (producto.Precio != null)
                    update = update.Set(x => x.Precio, producto.Precio);

                if (producto.Id_Categoria != null)
                    update = update.Set(x => x.Id_Categoria, producto.Id_Categoria);

                if (producto.Codigo != null)
                    update = update.Set(x => x.Codigo, producto.Codigo);

                if(producto.Cantidad != null)
                    update = update.Set(x => x.Cantidad, producto.Cantidad);

                await update.Update();

                Debug.WriteLine($"Updated Producto: {producto}");
                Debug.WriteLine($"Producto with Id {producto.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating Producto: " + ex.Message);
                throw new Exception($"Error Updating Producto: {ex.Message}");
            }
        }


        //CRUD Carrito
        public async Task<List<Carrito>> GetCarritosAsync()
        {
            var result = await _supabase.From<Carrito>().Get();
            return result.Models;
        }

        public async Task <int> GetCarritoID(int idCliente)
        {
            try
            {
                var result = await _supabase.From<Carrito>()
                    .Where(x => x.Id_Cliente == idCliente)
                    .Get();
                return result.Models[0].Id;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Hubo un error al encontrar el carrito del cliente {idCliente}");
                throw new Exception("Error al encontrar el carrito! Error:", ex);
            }
        }

        public async Task<int> InsertCarrito(Carrito carrito)
        {
            try
            {
                var res = await _supabase.From<Carrito>().Insert(carrito);
                Debug.WriteLine("Carrito Creado exitosamente!");
                
                return res.Models[0].Id;
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("exists"))
                {
                    throw new Exception($"Carrito ya Existe!");
                }
                Debug.WriteLine("Error al crear el carrito..." + ex);
                throw new Exception("Error al crear el carrito..." + ex);
            }            
        }

        //CRUD Categoria
        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            var result = await _supabase.From<Categoria>().Get();
            return result.Models;
        }

        //CRUD DetalleCarrito
        public async Task<List<DetalleCarrito>> GetDetalleCarritoCliente(int idCarrito)
        {
            try
            {
                var result = await _supabase.From<DetalleCarrito>()
                    .Where(x => x.Id_Carrito == idCarrito)
                    .Get();
                return result.Models;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error al encontrar el detalle de carrito: {ex}");
                throw new Exception("Error al encontrar los detalles del carrito!");
            }
            
        }

        public async Task InsertDetalleCarrito(DetalleCarrito detalleCarrito)
        {
            try
            {
                await _supabase.From<DetalleCarrito>().Insert(detalleCarrito);
                Debug.WriteLine("Carrito Creado exitosamente!");
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error al agregar el detallecarrito..." + ex);
                throw new Exception("Error al crear el detallecarrito..." + ex);
            }
        }

        public async Task DeleteDetalleCarrito(int idCarrito, int idProducto)
        {
            try
            {
                await _supabase.From<DetalleCarrito>()
                    .Where(x => x.Id_Carrito == idCarrito)
                    .Where(x => x.Id_Producto == idProducto)
                    .Delete();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al borrar el detalle de carrito: {ex}");
                throw new Exception("Error al borrar el detalle del carrito!");
            }
        }

        public async Task DeleteDetalleCarritoAll(int idCarrito)
        {
            try
            {
                await _supabase.From<DetalleCarrito>()
                    .Where(x => x.Id_Carrito == idCarrito)
                    .Delete();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al borrar el detalle de carrito: {ex}");
                throw new Exception("Error al borrar el detalle del carrito!");
            }
        }

        //CRUD DetalleOrden - Historial de Carrito
        public async Task<List<DetalleOrden>> GetDetalleOrdenAsync()
        {
            var result = await _supabase.From<DetalleOrden>().Get();
            return result.Models;
        }

        public async Task<int> GetIdOrden()
        {
            try
            {
                // Retrieve all DetalleOrden records
                var result = await _supabase.From<DetalleOrden>().Get();

                // Check if there are any records
                if (result.Models == null)
                {
                    // No records found, return an appropriate value (e.g., -1)
                    Debug.WriteLine("Returning Cero");
                    return 0;
                }

                // Find the record with the highest id_orden
                int maxIdOrden = result.Models.Max(detalleOrden => detalleOrden.Id_Orden);

                Debug.WriteLine("Returning maxIdOrden", maxIdOrden);

                return maxIdOrden;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error getting max IdOrden: " + ex.Message);
                return 0;
            }
            
        }

        public async Task InsertDetalleOrden(DetalleOrden detalleOrden)
        {
            try
            {
                await _supabase.From<DetalleOrden>()
                .Insert(detalleOrden);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error al insertar detalleOrden:{ex}");
            }
        }

        //Crud Oden Compra
        public async Task <int> InsertOrdenCompra(OrdenCompra ordenCompra)
        {
            try
            {
                var res = await _supabase.From<OrdenCompra>()
                .Insert(ordenCompra);

                return res.Models[0].Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al insertar detalleOrden:{ex}");
                throw new Exception("Error al crear la orden");
            }
        }

        public async Task<List<OrdenCompra>> SelectOrdenCompra()
        {
            try
            {
                var res = await _supabase.From<OrdenCompra>()
                .Get();

                return res.Models;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al insertar detalleOrden:{ex}");
                throw new Exception("Error al crear la orden");
            }
        }

        public async Task DeleteOrdenCompra(int ordenId)
        {
            try
            {
                await _supabase.From<OrdenCompra>()
                    .Where(x => x.Id == ordenId)
                    .Delete();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al borrar la orden de compra: {ex}");
                throw new Exception("Error al borrar la orden de compra!");
            }
        }
        
        public async Task CompletarOrdenCompra(int ordenId, string estado)
        {
            try
            {
                await _supabase.From<OrdenCompra>()
                    .Where(x => x.Id == ordenId)
                    .Set(x => x.Estado, estado)
                    .Update();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al completar la orden de compra: {ex}");
                throw new Exception("Error al completar la orden de compra!");
            }
        }

        //Crud Pago
        public async Task InsertPago(Pago pago)
        {
            try
            {
                await _supabase.From<Pago>().Insert(pago);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al pagar:{ex}");
                throw new Exception("Error al crear el pago");
            }
        }


    }
}
