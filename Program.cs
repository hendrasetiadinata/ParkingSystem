using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingSystem.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using ParkingSystem.Core.Interfaces;
using ParkingSystem.Infrastructure.Services;
using ParkingSystem.Core.Utilities;
using System.Reflection;
using ParkingSystem.Core.Entities;

namespace ParkingSystem
{
    class Program
    {
        public const string create_parking_lot = "create_parking_lot";
        public const string park = "park";
        public const string leave = "leave";
        public const string status = "status";
        public const string type_of_vehicles = "type_of_vehicles";
        public const string registration_numbers_for_vehicles_with_odd_plate = "registration_numbers_for_vehicles_with_odd_plate";
        public const string registration_numbers_for_vehicles_with_even_plate = "registration_numbers_for_vehicles_with_even_plate";
        public const string registration_numbers_for_vehicles_with_colour = "registration_numbers_for_vehicles_with_colour";
        public const string slot_numbers_for_vehicles_with_colour = "slot_numbers_for_vehicles_with_colour";
        public const string slot_number_for_registration_number = "slot_number_for_registration_number";
        public const string exit = "exit";

        static IConfigurationRoot GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            return configuration;
        }

        static async Task Main(string[] args)
        {
            var configuration = GetConfiguration();
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddDbContext<ParkingContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("ParkingSystem"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
            })
            .AddSingleton<IParkingOrderRepo, ParkingOrderRepo>()
            .AddSingleton<ISlotItemRepo, SlotItemRepo>()
            .AddSingleton<ISlotRepo, SlotRepo>()
            .AddSingleton<IParkingServices, ParkingServices>()
            .BuildServiceProvider();

            Console.Clear();
            Help();
            Succes:
            if (!args.Any()) Console.WriteLine("");
            else
            {
                var line = Console.ReadLine().TrimWhiteSpace();
                var key = line.Split(' ')[0];
                if (!IsValidCommand(key))
                {
                    Console.WriteLine("Invalid command");
                    Help();
                    goto InvalidCommand;
                }

                var _parkingRepo = serviceProvider.GetService<IParkingOrderRepo>();
                var _parkingServices = serviceProvider.GetService<IParkingServices>();
                var _slotRepo = serviceProvider.GetService<ISlotRepo>();

                if (key == create_parking_lot)
                {
                    if (!int.TryParse(line.Split(' ')[1], out int size))
                    {
                        Console.WriteLine("Invalid command");
                        Help();
                        goto InvalidCommand;
                    }
                    var result = await _parkingServices.Create_parking_lot(size);
                    Console.WriteLine(result);
                    goto Succes;
                }
                else if (key == park)
                {
                    var param = line.Split(' ');
                    if (line.Split(' ').Length != 4)
                    {
                        goto InvalidCommand;
                    }
                    var result = await _parkingServices.Park(param[1], param[2], param[3]);
                    Console.WriteLine(result);
                    goto Succes;
                }
                else if (key == leave)
                {
                    if (!int.TryParse(line.Split(' ')[1], out int slotNumber))
                    {
                        Console.WriteLine("Invalid command");
                        Help();
                        goto InvalidCommand;
                    }
                    var result = await _parkingServices.Leave(slotNumber);
                    Console.WriteLine(result);
                    goto Succes;
                }
                else if (key == status)
                {
                    var parkingOrders = await _parkingServices.Status();
                    var used = await _parkingServices.GetSlotItem();
                    var lsSlot = used.Where(d => d.Used == true).Select(a => a.SlotNo);
                    var result = parkingOrders.Where(x=> lsSlot.Contains(x.SlotNo));
                    if (result.Count() > 0)
                    {
                        //print header
                        var fieldList = new string[] { "SlotNo", "LicensePlate", "Vehicle", "Colour" };
                        PropertyInfo[] props = typeof(ParkingOrder).GetProperties();

                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (PropertyInfo propertyInfo in props)
                        {
                            if (fieldList.Contains(propertyInfo.Name))
                            {
                                if (propertyInfo.Name.Equals("SlotNo"))
                                {
                                    stringBuilder.Append("Slot         ");
                                }
                                else if (propertyInfo.Name.Equals("LicensePlate"))
                                {
                                    stringBuilder.Append("No         ");
                                }
                                else if (propertyInfo.Name.Equals("Vehicle"))
                                {
                                    stringBuilder.Append("Type         ");
                                }
                                else if (propertyInfo.Name.Equals("Colour"))
                                {
                                    stringBuilder.Append("Registration No Colour         ");
                                }

                            }
                           
                        }
                        Console.WriteLine(stringBuilder);
                        StringBuilder row = new StringBuilder();
                        //print row
                        foreach (var item in result)
                        {
                            row.Append(item.SlotNo + "            ");
                            row.Append(item.LicensePlate + "   ");
                            row.Append(item.Vehicle + "        ");
                            row.Append(item.Colour);
                            Console.WriteLine(row);
                            row.Clear();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Slot Filled");
                    }
                    goto Succes;
                }
                else if (key == type_of_vehicles)
                {
                    var param = line.Split(' ');
                    if (line.Split(' ').Length != 2)
                    {
                        goto InvalidCommand;
                    }
                    var result = await _parkingServices.Type_of_vehicles(param[1]);
                    Console.WriteLine(result);
                    goto Succes;
                }
                else if (key == registration_numbers_for_vehicles_with_odd_plate)
                {
                    var result = await _parkingServices.Registration_numbers_for_vehicles_with_ood_plate();
                    Console.WriteLine(result);
                    goto Succes;
                }
                else if (key == registration_numbers_for_vehicles_with_even_plate)
                {
                    var result = await _parkingServices.Registration_numbers_for_vehicles_with_event_plate();
                    Console.WriteLine(result);
                    goto Succes;
                }
                else if (key == registration_numbers_for_vehicles_with_colour)
                {
                    var param = line.Split(' ');
                    if (line.Split(' ').Length != 2)
                    {
                        goto InvalidCommand;
                    }
                    var result = await _parkingServices.Registration_numbers_for_vehicles_with_colour(param[1]);
                    Console.WriteLine(result);
                    goto Succes;
                }
                else if (key == slot_numbers_for_vehicles_with_colour)
                {
                    var param = line.Split(' ');
                    if (line.Split(' ').Length != 2)
                    {
                        goto InvalidCommand;
                    }
                    var result = await _parkingServices.Slot_numbers_for_vehicles_with_colour(param[1]);
                    Console.WriteLine(string.Join(",", result));
                    goto Succes;
                }
                else if (key == slot_number_for_registration_number)
                {
                    var param = line.Split(' ');
                    if (line.Split(' ').Length != 2)
                    {
                        goto InvalidCommand;
                    }
                    var result = await _parkingServices.Slot_number_for_registration_number(param[1]);
                    Console.WriteLine(result);
                    goto Succes;
                }
            }
        InvalidCommand:;
            Console.WriteLine("Invalid command");
        }

        static bool IsValidCommand(string command)
        {
            var commands = new StringBuilder($"{create_parking_lot},{park},{leave},{status},{type_of_vehicles},{registration_numbers_for_vehicles_with_odd_plate},");
            commands.Append($"{registration_numbers_for_vehicles_with_even_plate},{registration_numbers_for_vehicles_with_colour},");
            commands.Append($"{slot_numbers_for_vehicles_with_colour},{slot_number_for_registration_number},{exit}");
            if (commands.ToString().Split(',').Any(x => command.ToLower() == x.ToLower())) return true;
            return false;
        }

        static void Help()
        {
            Console.WriteLine("Parking System Commands:");
            Console.WriteLine("1. Create Parking Lot (command: create_parking_lot {size})");
            Console.WriteLine("2. Park (command: park {plat_no} {colour} {vehicle}");
            Console.WriteLine("3. Leave (command: leave {size})");
            Console.WriteLine("4. Status (command: status)");
            Console.WriteLine("5. Count Vehicle By Type (command: type_of_vehicles {vehicle}");
            Console.WriteLine("6. Show Odd Registration Number (command: registration_numbers_for_vehicles_with_odd_plate)");
            Console.WriteLine("7. Show Even Registration Number (command: registration_numbers_for_vehicles_with_even_plate)");
            Console.WriteLine("8. Show Registration Number by Colour (command: registration_numbers_for_vehicles_with_colour {colour})");
            Console.WriteLine("9. Show Slot Number by Colour (command: slot_numbers_for_vehicles_with_colour {colour})");
            Console.WriteLine("10. Show Slot Number by Registration Number (command: slot_number_for_registration_number {registration_number})");
            Console.WriteLine("0. Exit (command: exit)");
        }
    }
}
