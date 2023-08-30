using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace BarcodeScanner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

      
        public partial class UpdateRecord : ContentPage
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
        public UpdateRecord()
        {
            InitializeComponent();
            string serverdbname = "src_db";
            string servername = "10.0.0.136"; // Using wifi the mobile app can get access to SSMS
            string serverusername = "sa";
            string serverpassword = "masterfile";

            string sqlconn = $"Data Source={servername};Initial Catalog={serverdbname};User ID={serverusername};Password={serverpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }

        private async void MyScanner3_Clicked(object sender, EventArgs e)
        {


            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    MyScanner3.Text = result.Text;


                    try
                    {

                        List<MyTableList> myTableLists = new List<MyTableList>();
                        sqlConnection.Open();
                        string queryString = $"Select * from dbo.tbldevice WHERE sn = '{MyScanner3.Text}'";
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

                        MyView2.ItemsSource = myTableLists;
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                        throw;
                    }


                });
            };
        }

        private async void ConnectServer3_Clicked(object sender, EventArgs e)
        {

            sqlConnection.Open();
            await App.Current.MainPage.DisplayAlert("Alert", "Connection Establish", "Ok");
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

        private Entry Userassettag1 = new Entry();
        private Entry Userassettype1 = new Entry();
        private Entry Userdevicename1 = new Entry();
        private Entry Userbrand1 = new Entry();
        private Entry Usermodel1 = new Entry();
        private Entry Usersn1 = new Entry();
        private Entry Userdepartment1 = new Entry();
        private Entry Userlocation1 = new Entry();
        private Entry Userdeviceuser1 = new Entry();
        private DatePicker Userdatepurchased1 = new DatePicker();
        private Entry Userprice1 = new Entry();
        private Entry UserHWdetail1 = new Entry();
        private Entry Userstatus1 = new Entry();



        private async void UpdateRecord1_Clicked(object sender, EventArgs e)
        {

            try
            {

                sqlConnection.Open();

                string assettagTobeUpdated = Userassettag1.Text;


                string assettypeTobeUpdated = Userassettype1.Text;


                string devicenameTobeUpdated = Userdevicename1.Text;


                string brandTobeUpdated = Userbrand1.Text;


                string modelTobeUpdated = Usermodel1.Text;


                string snTobeUpdated = Usersn1.Text;


                string departmentTobeUpdated = Userdepartment1.Text;


                string locationTobeUpdated = Userlocation1.Text;


                string deviceuserTobeUpdated = Userdeviceuser1.Text;


                DateTime datepurchasedTobeUpdated = Userdatepurchased1.Date;


                string priceTobeUpdated = Userprice1.Text;


                string HWdetailTobeUpdated = UserHWdetail1.Text;


                string statusTobeUpdated = Userstatus1.Text;



                string qerystr = $"UPDATE dbo.tbldevice SET assettag='{assettagTobeUpdated}'," +
                                    $" assettype ='{assettypeTobeUpdated}'," +
                                    $" devicename ='{devicenameTobeUpdated}'," +
                                    $" brand ='{brandTobeUpdated}'," +
                                    $" model ='{modelTobeUpdated}'," +
                                    $" sn ='{snTobeUpdated}'," +
                                    $" department ='{departmentTobeUpdated}'," +
                                    $" location ='{locationTobeUpdated}'," +
                                    $" deviceuser ='{deviceuserTobeUpdated}'," +
                                    $" datepurchased ='{datepurchasedTobeUpdated}'," +
                                    $" price ='{priceTobeUpdated}'," +
                                    $" HWdetail ='{HWdetailTobeUpdated}'," +
                                    $" status ='{statusTobeUpdated}' WHERE assettag='{assettagTobeUpdated}'";

                    using (SqlCommand command = new SqlCommand(qerystr, sqlConnection))
                    {

                        command.ExecuteNonQuery();
                    }

                    await App.Current.MainPage.DisplayAlert("Alert", "Congrats you have Successfully Updated the row item", "Ok");
                
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");

            }
            finally
            { 
                sqlConnection.Close();  // Close the connection in the finally block
            }
        
        }



    }
}