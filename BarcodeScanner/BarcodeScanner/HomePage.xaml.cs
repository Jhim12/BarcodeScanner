﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace BarcodeScanner
{

    public partial class MainPage : ContentPage
    {

        public class MyTableList
        {
            public string assettag { get; set; }
            public string assettype { get; set; }
            public string devicename { get; set; }

            public string brand { get; set; }
            public string model { get; set; }
            public string sn { get; set; }
            public string department { get; set; }
            public string location { get; set; }
            public string deviceuser { get; set; }
            public DateTime datepurchased { get; set; }
            public float price { get; set; }
            public string HWdetail { get; set; }
            public string status { get; set; }
        }

        SqlConnection sqlConnection;

        public MainPage()
        {
            InitializeComponent();
            string serverdbname = "src_db";
            string servername = "10.0.0.136"; // Using wifi the mobile app can get access to SSMS
            string serverusername = "sa";
            string serverpassword = "masterfile";

            string sqlconn = $"Data Source={servername};Initial Catalog={serverdbname};User ID={serverusername};Password={serverpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }

        private async void ConnectServer_Clicked(object sender, EventArgs e)
        {

            sqlConnection.Open();
            await App.Current.MainPage.DisplayAlert("Alert", "Connection Establish", "Ok");
            sqlConnection.Close();
            try
            {

                    
                    // Perform database operations here
                }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;

            }
        }

        private async void MyScanner_Clicked(object sender, EventArgs e)
        {


            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                   /* MyScanner.Text = result.Text;*/
                    Usersn.Text = result.Text;
                });
            };
        }

        private async void ViewRecord_Clicked(object sender, EventArgs e)
        { 
            try
            {
                List<MyTableList> myTableLists = new List<MyTableList>();
                sqlConnection.Open();
                string queryString = "Select * from dbo.tbldevice";
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    myTableLists.Add(new MyTableList

                    {

                        /*                        Id = Convert.ToInt32(reader["Id"]),*/
                        assettag = reader["assettag"].ToString(),
                        assettype = reader["assettype"].ToString(),
                        devicename = reader["devicename"].ToString(),

                    }
                    );
                }
                reader.Close();
                sqlConnection.Close();

                MyListView.ItemsSource = myTableLists;
            }   
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert",ex.Message,"Ok");   
                throw;
            }
                     
        }



        private async void AddRecord_Clicked(object sender, EventArgs e)
        {
            DateTime selectedDate = Userdatepurchased.Date;
            try
            {
                
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO dbo.tbldevice (assettag, assettype, devicename, brand, model, sn, department, location, deviceuser, datepurchased, price, HWdetail, status)" +
                    " VALUES (@assettag, @assettype, @devicename, @brand, @model, @sn, @department, @location, @deviceuser, @datepurchased, @price, @HWdetail, @status)", sqlConnection))
                {
                    command.Parameters.AddWithValue("@assettag", Userassettag.Text);
                    command.Parameters.AddWithValue("@assettype", Userassettype.Text);
                    command.Parameters.AddWithValue("@devicename", Userdevicename.Text);
                    command.Parameters.AddWithValue("@brand", Userbrand.Text);
                    command.Parameters.AddWithValue("@model", Usermodel.Text);
                    command.Parameters.AddWithValue("@sn", Usersn.Text);
                    command.Parameters.AddWithValue("@department", Userdepartment.Text);
                    command.Parameters.AddWithValue("@location", Userlocation.Text);
                    command.Parameters.AddWithValue("@deviceuser", Userdeviceuser.Text);

                    // Convert and set the DateTime value with SqlDbType.Date
                    SqlParameter datePurchasedParameter = new SqlParameter("@datepurchased", System.Data.SqlDbType.Date); 
                    datePurchasedParameter.Value = selectedDate;
                    command.Parameters.Add(datePurchasedParameter); // Add the parameter to the command's Parameters collection

                    command.Parameters.AddWithValue("@price", Userprice.Text);
                    command.Parameters.AddWithValue("@HWdetail", UserHWdetail.Text);
                    command.Parameters.AddWithValue("@status", Userstatus.Text);
                    command.ExecuteNonQuery();

                }
                sqlConnection.Close();
                await App.Current.MainPage.DisplayAlert("Alert", "Congrats you just posted data", "Ok");


            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }

        }

        private async void UpdateRecord_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UpdateRecord());
        }

        private void ViewLogs_Clicked(object sender, EventArgs e)
        {

        }

        private void Exit_Clicked(object sender, EventArgs e)
        {

        }

        private async void View_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewRecord());
        }


    }
}
