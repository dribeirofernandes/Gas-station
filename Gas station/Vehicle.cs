using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gas_station
{
    class Vehicle
    {
        string[,] modelNames = new string[,] { {"Opel Corsa", "Mitsubishi Lancer Evo", "Nissan GT-R", "Volvo S60", "Honda Accord"},
                                               {"Ford Transit", "Volkswagen Caddy", "Fiat Doblò", "Nissan NV200", "Vauxhall Combo"},
                                               {"Volvo FH500", "DAF LF45.220", "Volvo F10", "Iveco Lorry 75E16", "Scania R480" } };

        string[] FuelTypes = new string[] { "Unleaded", "LPG", "Diesel" };

        public int ID { get; set; }
        public string ModelName { get; set; }
        public string VehicleType { get; set; }
        public string FuelType { get; set; }
        public int FuelCapacity { get; set; }
        public double CurrentFuel { get; set; }


        public Vehicle(string vehicleType, int id)
        {
            ID = id;
            VehicleType = vehicleType;
            var random = new Random();
            switch (vehicleType.ToUpper())
            {
                case "CAR":
                    ModelName = modelNames[0, random.Next(0, 5)];
                    FuelType = FuelTypes[random.Next(0, 3)];
                    FuelCapacity = 40;
                    CurrentFuel = random.Next(0, FuelCapacity / 4);
                    break;

                case "VAN":
                    ModelName = modelNames[1, random.Next(0, 5)];
                    FuelType = FuelTypes[random.Next(1, 3)];
                    FuelCapacity = 80;
                    CurrentFuel = random.Next(0, FuelCapacity / 4);
                    break;

                case "HGV":
                    ModelName = modelNames[2, random.Next(0, 5)];
                    FuelType = FuelTypes[2];
                    FuelCapacity = 150;
                    CurrentFuel = random.Next(0, FuelCapacity / 4);
                    break;

            }
        }
    }
}
