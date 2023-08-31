using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace BarcodeScanner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRecord : ContentPage
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





        public ViewRecord()
            {
                InitializeComponent();
                string serverdbname = "src_db";
                string servername = "192.168.100.106"; // Using wifi the mobile app can get access to SSMS
                string serverusername = "sa";
                string serverpassword = "masterfile";

                string sqlconn = $"Data Source={servername};Initial Catalog={serverdbname};User ID={serverusername};Password={serverpassword}";
                sqlConnection = new SqlConnection(sqlconn);
            }




        private async void Find_Clicked(object sender, EventArgs e)
        {
            try
            {

                List<MyTableList> myTableLists = new List<MyTableList>();
                sqlConnection.Open();
                string queryString = $"Select * from dbo.tbldevice WHERE sn = '{UserSearch.Text}'";
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    myTableLists.Add(new MyTableList

                    {

                        assettag = reader["assettag"].ToString(),
                        assettype = reader["assettype"].ToString(),
                        devicename = reader["devicename"].ToString(),
                        brand = reader["brand"].ToString(),
                        model = reader["model"].ToString(),
                        sn = reader["sn"].ToString(),
                        department = reader["department"].ToString(),
                        location = reader["location"].ToString(),
                        deviceuser = reader["deviceuser"].ToString(),
                        datepurchased = DateTime.Parse(reader["datepurchased"].ToString()),
                        price = float.Parse(reader["price"].ToString()),
                        HWdetail = reader["HWdetail"].ToString(),
                        status = reader["status"].ToString(),

                    }
                    );
                }
                reader.Close();
                sqlConnection.Close();

                MyView.ItemsSource = myTableLists;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }

        }

        private async void ConnectServer2_Clicked(object sender, EventArgs e)
        {
            string serverdbname = "src_db";
            string serverusername = "sa";
            string serverpassword = "masterfile";
            string sqlconn = $"Data Source={Useripaddress2.Text};Initial Catalog={serverdbname};User ID={serverusername};Password={serverpassword}";
            sqlConnection = new SqlConnection(sqlconn);
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

        private async void MyScanner2_Clicked(object sender, EventArgs e)
        {


            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    /* MyScanner.Text = result.Text;*/
                    UserSearch.Text = result.Text;
                });
            };
        }

    }
 }