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
        private double RateDispense = 5;
        private double Fuelprice = 1.19;
        private double SessionDispensed = 0;
        public string fuelType { get; set; }
        public bool Availability { get; set; } = true;
        public string vehicleName { get; set; }
        public double currentFuel { get; set; }
        public double maximumFuel { get; set; }
        public string fuelComparison { get; set; }

        public void StartPump(List<Vehicle> queue, DataTable transactionsTable, DataTable fuelTable, ProgressBar progressbar, Label totalDispensed)
        {
            Availability = false;
            Vehicle vehicle = queue.First();
            queue.Remove(queue.Single(r => r.ID == vehicle.ID));
            vehicleName = vehicle.ModelName;
            currentFuel = vehicle.CurrentFuel;
            maximumFuel = vehicle.FuelCapacity;
            fuelType = vehicle.FuelType;

            progressbar.Invoke((MethodInvoker)(() => progressbar.Maximum = (int)maximumFuel));
            progressbar.Invoke((MethodInvoker)(() => progressbar.Minimum = (int)currentFuel));

            while (currentFuel <= maximumFuel)
            {
                totalDispensed.Invoke((MethodInvoker)(() => totalDispensed.Text = $"Total Dispensed: {(double.Parse(Regex.Match(totalDispensed.Text, @"-?\d+(?:\.\d+)?").Value) + RateDispense).ToString()}"));
                progressbar.Invoke((MethodInvoker)(() => progressbar.Value = (int)currentFuel));
                currentFuel += RateDispense;
                SessionDispensed += RateDispense;
                fuelComparison = $"{currentFuel}/{maximumFuel}L";

                //Edit row in fuel Table that matches fuel type with new value plus previous one.
                foreach (DataRow dr in fuelTable.Rows)
                {
                    if (dr["fuelType"].ToString() == fuelType)
                    {
                        var previousAmount = double.Parse(dr["dispensed"].ToString());
                        dr["dispensed"] = $"{previousAmount + RateDispense}L";
                    }
                }
                Thread.Sleep(1000);
            }
            progressbar.Invoke((MethodInvoker)(() => progressbar.Minimum = 0));
            progressbar.Invoke((MethodInvoker)(() => progressbar.Value = 0));
            transactionsTable.Rows.Add(new object[] { $"{vehicle.VehicleType}", $"{vehicle.FuelType}", $"{vehicle.ModelName}", $"{SessionDispensed}L", $"£{SessionDispensed * Fuelprice}" });
            SessionDispensed = 0;
            Availability = true;
        }
    }
}
