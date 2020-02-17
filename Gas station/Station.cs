using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Gas_station
{
    class Station
    {
        //Rates
        private double RateDispense = 5;
        private double Fuelprice = 1.19;

        //Variable declaration
        private Vehicle vehicle;
        private double FuelDispensed = 0;
        public bool Availability { get; set; } = true;
        public string FuelComparison { get; private set; }

        public void Start(List<Vehicle> queue, DataTable transactionsTable, DataTable fuelTable, ProgressBar progressbar, Label totalDispensed)
        {
            vehicle = queue.First();
            Availability = false;
            queue.Remove(queue.Single(r => r.ID == vehicle.ID));

            //Sets the maximum and minimum of the progressBar according to the vehicle
            //current fuel and maximum fuel
            progressbar.Invoke((MethodInvoker)(() => progressbar.Maximum = (int)vehicle.FuelCapacity));
            progressbar.Invoke((MethodInvoker)(() => progressbar.Minimum = (int)vehicle.CurrentFuel));

            do
            {
                //Update stat trackers and dispense fuel
                totalDispensed.Invoke((MethodInvoker)(() => totalDispensed.Text = $"Total Dispensed: {(double.Parse(Regex.Match(totalDispensed.Text, @"-?\d+(?:\.\d+)?").Value) + RateDispense).ToString()}"));
                progressbar.Invoke((MethodInvoker)(() => progressbar.Value = (int)vehicle.CurrentFuel));
                DispenseFuel(vehicle);
                FuelComparison = $"{vehicle.CurrentFuel}/{vehicle.FuelCapacity}L";

                //Updates the values on the fuel table
                UpdateFuelTable(vehicle, fuelTable);

            } while (vehicle.CurrentFuel < vehicle.FuelCapacity);

            //Resets the values of the progressBar for the next vehicle
            progressbar.Invoke((MethodInvoker)(() => progressbar.Minimum = 0));
            progressbar.Invoke((MethodInvoker)(() => progressbar.Value = 0));

            //Saves the transaction record in the transactions table with all the information regarding
            //the pump usage.
            transactionsTable.Rows.Add(
                new object[] { 
                    $"{vehicle.VehicleType}", 
                    $"{vehicle.FuelType}", 
                    $"{vehicle.ModelName}", 
                    $"{FuelDispensed}L", 
                    $"£{FuelDispensed * Fuelprice}" 
                });

            FuelDispensed = 0;
            vehicle = null;           
            Availability = true;
        }

        private void DispenseFuel(Vehicle vehicle)
        {
            //Dispense fuel
            vehicle.CurrentFuel += RateDispense;
            FuelDispensed += RateDispense;

            //If vehicle would end up being overfilled, then subtract the amount over the
            //maximum fuel capacity
            if (vehicle.CurrentFuel > vehicle.FuelCapacity)
            {
                var difference = vehicle.CurrentFuel - vehicle.FuelCapacity;
                vehicle.CurrentFuel -= difference;
                FuelDispensed -= difference;
            }
        }
        private void UpdateFuelTable(Vehicle vehicle, DataTable FuelTable)
        {
            //Edit row in fuel Table that matches fuel type with new value plus previous one.
            foreach (DataRow dr in FuelTable.Rows)
            {
                lock (FuelTable)
                {
                    if (dr["fuelType"].ToString() == vehicle.FuelType)
                    {
                        var previousAmount = double.Parse(dr["dispensed"].ToString());
                        dr["dispensed"] = $"{previousAmount + RateDispense}";
                    }
                }
            }
            Thread.Sleep(1000);
        }
        public string VehicleModel()
        {
            return vehicle == null ? null : vehicle.ModelName;
        }
    }
}
