using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Parqueo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sistema de Parqueos Grupo 1");

            int tamanioArreglo = ValidarEntradaEntero("Ingrese el número de espacios para parqueo:");
            EspacioDeParqueo[] arregloEspacios = CrearArregloEspacios(tamanioArreglo);
            Factura[] arregloFacturas = new Factura[0]; 
            List<Cliente> clientes = new List<Cliente>(); 

            // Menú principal
            while (true)
            {
                Console.WriteLine("\nMenú:");
                Console.WriteLine("1. Agregar un nuevo parqueo");
                Console.WriteLine("2. Visualizar arreglo de parqueo");
                Console.WriteLine("3. Imprimir estado de un espacio");
                Console.WriteLine("4. Generar factura de parqueo");
                Console.WriteLine("5. Facturas generadas");
                Console.WriteLine("6. Parqueos disponibles");
                Console.WriteLine("7. Historial Clientes");
                Console.WriteLine("8. Salir");

                int opcion = ValidarEntradaEntero("Seleccione una opción:");

                switch (opcion)
                {
                    case 1:
                        AgregarParqueo(arregloEspacios, clientes); 
                        break;
                    case 2:
                        VisualizarEspacios(arregloEspacios);
                        break;
                    case 3:
                        int idEspacio = ValidarEntradaEntero("Ingrese el número de espacio:");
                        ImprimirEspacio(arregloEspacios, idEspacio);
                        break;
                    case 4:
                        arregloFacturas = GenerarFactura(arregloEspacios, arregloFacturas);
                        break;
                    case 5:
                        VerFacturas(arregloFacturas);
                        break;
                    case 6:
                        VerParqueosDisponibles(arregloEspacios);
                        break;
                    case 7:
                        VerClientes(clientes);
                        break;
                    case 8:
                        Console.WriteLine("¡Hasta luego!");
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

      
        public static EspacioDeParqueo[] CrearArregloEspacios(int tamanio)
        {
            EspacioDeParqueo[] arregloEspacios = new EspacioDeParqueo[tamanio];
            for (int i = 0; i < tamanio; i++)
            {
                arregloEspacios[i] = new EspacioDeParqueo(i + 1);
            }
            return arregloEspacios;
        }

        public static void AgregarParqueo(EspacioDeParqueo[] arregloEspacios, List<Cliente> clientes)
        {
            int idEspacio = ValidarEntradaEntero("Ingrese el número de espacio donde estacionará:");
            if (idEspacio > 0 && idEspacio <= arregloEspacios.Length)
            {
                var espacio = arregloEspacios[idEspacio - 1];
                if (!espacio.EstaOcupado)
                {
                    Console.WriteLine("Ingrese su nombre:");
                    string nombreCliente = Console.ReadLine();

                  
                    Cliente cliente = new Cliente(nombreCliente);
                    clientes.Add(cliente);

                    espacio.NombreCliente = nombreCliente;
                    espacio.EstaOcupado = true;
                    espacio.FechaIngreso = DateTime.Now;
                    Console.WriteLine($"Espacio #{espacio.ID} ha sido ocupado por {nombreCliente}.");
                }
                else
                {
                    Console.WriteLine("El espacio ya está ocupado.");
                }
            }
            else
            {
                Console.WriteLine("Número de espacio inválido.");
            }
        }

        public static void VisualizarEspacios(EspacioDeParqueo[] arregloEspacios)
        {
            foreach (var espacio in arregloEspacios)
            {
                Console.WriteLine($"Espacio #{espacio.ID} - Estado: {(espacio.EstaOcupado ? "Ocupado" : "Libre")}");
            }
        }

        public static void ImprimirEspacio(EspacioDeParqueo[] arregloEspacios, int idEspacio)
        {
            if (idEspacio > 0 && idEspacio <= arregloEspacios.Length)
            {
                var espacio = arregloEspacios[idEspacio - 1];
                Console.WriteLine($"Espacio #{espacio.ID} - Estado: {(espacio.EstaOcupado ? "Ocupado" : "Libre")}");
            }
            else
            {
                Console.WriteLine("Espacio no válido.");
            }
        }

        public static Factura[] GenerarFactura(EspacioDeParqueo[] arregloEspacios, Factura[] arregloFacturas)
        {
            int idEspacio = ValidarEntradaEntero("Ingrese el número de espacio para generar factura:");
            if (idEspacio > 0 && idEspacio <= arregloEspacios.Length)
            {
                var espacio = arregloEspacios[idEspacio - 1];
                if (espacio.EstaOcupado)
                {
                    TimeSpan tiempoEstacionado = DateTime.Now - espacio.FechaIngreso;
                    decimal monto = CalcularMonto(tiempoEstacionado);
                    Factura factura = new Factura(espacio.ID, monto);

                  
                    Array.Resize(ref arregloFacturas, arregloFacturas.Length + 1);
                    arregloFacturas[arregloFacturas.Length - 1] = factura;
                    espacio.EstaOcupado = false; 

                    Console.WriteLine($"Factura generada: Ticket #{factura.TicketID} - Monto: ${factura.Monto}");
                }
                else
                {
                    Console.WriteLine("El espacio está libre, no hay factura para generar.");
                }
            }
            else
            {
                Console.WriteLine("Número de espacio inválido.");
            }
            return arregloFacturas;
        }

        public static decimal CalcularMonto(TimeSpan tiempoEstacionado)
        {
            decimal tarifaHora = 20L; 
            decimal monto = (decimal)tiempoEstacionado.TotalHours * tarifaHora;
            return monto;
        }

        // Función para ver las facturas generadas
        public static void VerFacturas(Factura[] arregloFacturas)
        {
            if (arregloFacturas.Length > 0)
            {
                foreach (var factura in arregloFacturas)
                {
                    Console.WriteLine($"Ticket #{factura.TicketID} - Fecha: {factura.FechaFactura} - Monto: ${factura.Monto}");
                }
            }
            else
            {
                Console.WriteLine("No hay facturas generadas.");
            }
        }

        public static void VerParqueosDisponibles(EspacioDeParqueo[] arregloEspacios)
        {
            int disponibles = 0;
            foreach (var espacio in arregloEspacios)
            {
                if (!espacio.EstaOcupado)
                {
                    disponibles++;
                }
            }
            Console.WriteLine($"Parqueos disponibles: {disponibles}");
        }

      
        public static void VerClientes(List<Cliente> clientes)
        {
            if (clientes.Count > 0)
            {
                foreach (var cliente in clientes)
                {
                    Console.WriteLine($"Cliente: {cliente.Nombre}");
                }
            }
            else
            {
                Console.WriteLine("No hay clientes registrados.");
            }
        }

        public static int ValidarEntradaEntero(string mensaje)
        {
            int valor;
            bool esValido = false;
            do
            {
                Console.WriteLine(mensaje);
                esValido = int.TryParse(Console.ReadLine(), out valor);
            } while (!esValido);
            return valor;
        }
    }

    public class Cliente
    {
        public string Nombre { get; set; }

      
        public Cliente(string nombre)
        {
            Nombre = nombre;
        }
    }

   
    public class EspacioDeParqueo
    {
        public int ID { get; set; }
        public bool EstaOcupado { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaIngreso { get; set; }

        // Constructor
        public EspacioDeParqueo(int id)
        {
            ID = id;
            EstaOcupado = false;
            NombreCliente = string.Empty;
            FechaIngreso = DateTime.MinValue;
        }
    }

    // Clase Factura
    public class Factura
    {
        public int TicketID { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaFactura { get; set; }

        // Constructor
        public Factura(int ticketID, decimal monto)
        {
            TicketID = ticketID;
            Monto = monto;
            FechaFactura = DateTime.Now;
        }
    }
}

