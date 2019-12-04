﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gas_station
{
    public partial class Form1 : Form
    {
        List<Vehicle> queue = new List<Vehicle>();
        DataTable transactions = new DataTable();
        DataTable fuelDispensed = new DataTable();
        Station[] pump = new Station[9];
        int vehicleID = 0;
        public Form1()
        {
            InitializeComponent();
            //Transactions DT
            transactions.Columns.Add("vehicleType", typeof(string));
            transactions.Columns.Add("fuelTypes", typeof(string));
            transactions.Columns.Add("vehicleModel", typeof(string));
            transactions.Columns.Add("numberLiters", typeof(string));
            transactions.Columns.Add("paymentAmount", typeof(string));

            //Fuel DT
            fuelDispensed.Columns.Add("fuelType", typeof(string));
            fuelDispensed.Columns.Add("dispensed", typeof(string));
            fuelDispensed.Rows.Add(new Object[] { "Unleaded", "0" });
            fuelDispensed.Rows.Add(new Object[] { "Diesel", "0" });
            fuelDispensed.Rows.Add(new Object[] { "LPG", "0" });

            //Initialize pump objects
            for (int i = 0; i < pump.Length; i++)
                pump[i] = new Station();
        }

        private void UpdateInterface_Tick(object sender, EventArgs e)
        {

            //Update queue list
            queueList.Text = "";
            foreach (Vehicle v in queue)
                queueList.Text += $"{ v.ModelName}{Environment.NewLine}";

            //Update top counters
            lQueue.Text = $"Queue - {queue.Count}";
            lPoundsGenerated.Text = $"Pounds Generated: £{(double.Parse(Regex.Match(lTotalDispensed.Text, @"-?\d+(?:\.\d+)?").Value) * 1.19).ToString()}";
            string pdsGenerated = Regex.Match(lPoundsGenerated.Text, @"-?\d+(?:\.\d+)?").Value;
            lCommission.Text = $"Commission: £{(double.Parse(pdsGenerated) * 0.01).ToString()}";
            lVehiclesServed.Text = $"Vehicles Serviced: {vehicleID}";

            //Update transactions datagridView
            dgTransactions.DataSource = transactions;
            dgTransactions.Update();
            dgTransactions.RowTemplate.Height = 22;
            dgTransactions.ScrollBars = ScrollBars.Vertical;

            //Update fuel datagridview
            dgFuel.DataSource = fuelDispensed;
            dgFuel.Update();

            //Update pump interfaces
            if (!pump[0].Availability)
                UpdatePanel(false, pump[0], Pump1, NumberPanel1, PumpNumber1, vehicleName1, CurrentFuel1);
            else
                UpdatePanel(true, pump[0], Pump1, NumberPanel1, PumpNumber1, vehicleName1, CurrentFuel1);
            if (!pump[1].Availability)
                UpdatePanel(false, pump[1], Pump2, NumberPanel2, PumpNumber2, vehicleName2, CurrentFuel2);
            else
                UpdatePanel(true, pump[1], Pump2, NumberPanel2, PumpNumber2, vehicleName2, CurrentFuel2);
            if (!pump[2].Availability)
                UpdatePanel(false, pump[2], Pump3, NumberPanel3, PumpNumber3, vehicleName3, CurrentFuel3);
            else
                UpdatePanel(true, pump[2], Pump3, NumberPanel3, PumpNumber3, vehicleName3, CurrentFuel3);
            if (!pump[3].Availability)
                UpdatePanel(false, pump[3], Pump4, NumberPanel4, PumpNumber4, vehicleName4, CurrentFuel4);
            else
                UpdatePanel(true, pump[3], Pump4, NumberPanel4, PumpNumber4, vehicleName4, CurrentFuel4);
            if (!pump[4].Availability)
                UpdatePanel(false, pump[4], Pump5, NumberPanel5, PumpNumber5, vehicleName5, CurrentFuel5);
            else
                UpdatePanel(true, pump[4], Pump5, NumberPanel5, PumpNumber5, vehicleName5, CurrentFuel5);
            if (!pump[5].Availability)
                UpdatePanel(false, pump[5], Pump6, NumberPanel6, PumpNumber6, vehicleName6, CurrentFuel6);
            else
                UpdatePanel(true, pump[5], Pump6, NumberPanel6, PumpNumber6, vehicleName6, CurrentFuel6);
            if (!pump[6].Availability)
                UpdatePanel(false, pump[6], Pump7, NumberPanel7, PumpNumber7, vehicleName7, CurrentFuel7);
            else
                UpdatePanel(true, pump[6], Pump7, NumberPanel7, PumpNumber7, vehicleName7, CurrentFuel7);
            if (!pump[7].Availability)
                UpdatePanel(false, pump[7], Pump8, NumberPanel8, PumpNumber8, vehicleName8, CurrentFuel8);
            else
                UpdatePanel(true, pump[7], Pump8, NumberPanel8, PumpNumber8, vehicleName8, CurrentFuel8);
            if (!pump[8].Availability)
                UpdatePanel(false, pump[8], Pump9, NumberPanel9, PumpNumber9, vehicleName9, CurrentFuel9);
            else
                UpdatePanel(true, pump[8], Pump9, NumberPanel9, PumpNumber9, vehicleName9, CurrentFuel9);
        }

        void UpdatePanel(bool availability, Station objectPump, Panel pump, Panel NumberPanel, Label PumpNumber, Label VehicleName, Label CurrentFuel)
        {
            if (!availability)
            {
                pump.BackColor = Color.FromArgb(226, 182, 179);
                NumberPanel.BackColor = Color.FromArgb(232, 157, 153);
                PumpNumber.ForeColor = Color.FromArgb(199, 68, 63);
                PumpNumber.BackColor = Color.FromArgb(232, 157, 153);
                VehicleName.Text = objectPump.vehicleName;
                CurrentFuel.Show();
                CurrentFuel.Text = objectPump.fuelComparison;
            }
            else
            {
                pump.BackColor = Color.FromArgb(163, 207, 236);
                NumberPanel.BackColor = Color.FromArgb(100, 177, 228);
                PumpNumber.ForeColor = Color.FromArgb(0, 105, 165);
                PumpNumber.BackColor = Color.FromArgb(100, 177, 228);
                VehicleName.Text = "Available";
                CurrentFuel.Hide();
            }

        }

        private void SpawnVehicle_Tick(object sender, EventArgs e)
        {
            if (queue.Count < 5)
            {
                switch (new Random().Next(0, 3))
                {
                    case 0:
                        queue.Add(new Vehicle("CAR", vehicleID));
                        vehicleID++;
                        break;
                    case 1:
                        queue.Add(new Vehicle("VAN", vehicleID));
                        vehicleID++;
                        break;
                    case 2:
                        queue.Add(new Vehicle("HGV", vehicleID));
                        vehicleID++;
                        break;
                }
            }
        }

        private void AssignPump_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < pump.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (pump[i].Availability == true && queue.Count > 0)
                        {
                            var thread = new Thread(() => pump[0].StartPump(queue, transactions, fuelDispensed, progressBar1, lTotalDispensed));
                            thread.Start();
                            Thread.Sleep(50);
                        }
                        break;
                    case 1:
                        if (pump[i].Availability == true && queue.Count > 0)
                        {
                            var thread = new Thread(() => pump[1].StartPump(queue, transactions, fuelDispensed, progressBar2, lTotalDispensed));
                            thread.Start();
                            Thread.Sleep(50);
                        }
                        break;
                    case 2:
                        if (pump[i].Availability == true && queue.Count > 0)
                        {
                            var thread = new Thread(() => pump[2].StartPump(queue, transactions, fuelDispensed, progressBar3, lTotalDispensed));
                            thread.Start();
                            Thread.Sleep(50);
                        }
                        break;
                    case 3:
                        if (pump[i].Availability == true && queue.Count > 0)
                        {
                            var thread = new Thread(() => pump[3].StartPump(queue, transactions, fuelDispensed, progressBar4, lTotalDispensed));
                            thread.Start();
                            Thread.Sleep(50);
                        }
                        break;
                    case 4:
                        if (pump[i].Availability == true && queue.Count > 0)
                        {
                            var thread = new Thread(() => pump[4].StartPump(queue, transactions, fuelDispensed, progressBar5, lTotalDispensed));
                            thread.Start();
                            Thread.Sleep(50);
                        }
                        break;
                    case 5:
                        if (pump[i].Availability == true && queue.Count > 0)
                        {
                            var thread = new Thread(() => pump[5].StartPump(queue, transactions, fuelDispensed, progressBar6, lTotalDispensed));
                            thread.Start();
                            Thread.Sleep(50);
                        }
                        break;
                    case 6:
                        if (pump[i].Availability == true && queue.Count > 0)
                        {
                            var thread = new Thread(() => pump[6].StartPump(queue, transactions, fuelDispensed, progressBar7, lTotalDispensed));
                            thread.Start();
                            Thread.Sleep(50);

                        }
                        break;
                    case 7:
                        if (pump[i].Availability == true && queue.Count > 0)
                        {
                            var thread = new Thread(() => pump[7].StartPump(queue, transactions, fuelDispensed, progressBar8, lTotalDispensed));
                            thread.Start();
                            Thread.Sleep(50);

                        }
                        break;
                    case 8:
                        if (pump[i].Availability == true && queue.Count > 0)
                        {
                            var thread = new Thread(() => pump[8].StartPump(queue, transactions, fuelDispensed, progressBar9, lTotalDispensed));
                            thread.Start();
                            Thread.Sleep(50);

                        }
                        break;
                }
            }
        }

        private void btnTransactions_Click(object sender, EventArgs e)
        {
            if (!transactionsPanel.Visible)
            {
                btnTransactions.Text = "Return";
                btnUpdateTransactions.Show();
                transactionsPanel.Visible = true;
            }
            else
            {
                btnTransactions.Text = "Transations to date";
                btnUpdateTransactions.Hide();
                transactionsPanel.Visible = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdateTransactions_Click(object sender, EventArgs e)
        {
            dgTransactions.Refresh();
            dgTransactions.Update();
            dgTransactions.ScrollBars = ScrollBars.Vertical;
        }

        private void dgTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.quitBtn;

        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.quitButton;
        }
    }
}
