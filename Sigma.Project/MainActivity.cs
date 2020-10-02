using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading; 
using Sigma.Networking;
using System.Threading.Tasks;
using Java.Nio.Channels;

namespace Sigma.Project
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        private TextView txtFeedback;
        private Client clientService; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            clientService = new Client(); 
            txtFeedback = FindViewById<TextView>(Resource.Id.connectionStatus); //get the text object from the main page 
            assignEventHandlers(); 
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        //go through each page element and give it an eventhandler if applicable
        private void assignEventHandlers()
        {
            FindViewById<Button>(Resource.Id.btn_connect).Click += OnConnectClicked; 
        }

        #region EventHandlers
        async void OnConnectClicked(object sender, EventArgs args)
        {
            txtFeedback.Text = "connecting to server....";
            //Task<bool> connectionRoutine = clientService.StartClientAsync();
            //bool connected = await connectionRoutine; 

            bool connected = await Task.Run(clientService.StartClientAsync); 
            if (connected)
            {
                txtFeedback.Text = "Connected";
            }
            else
            {
                txtFeedback.Text = "Unable to Find Server"; 
            }
        }
        #endregion
    }
}