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
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.Design.Animation;

namespace Sigma.Project
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Design.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        private TextView txtFeedback;
        private Client clientService;
        private bool connectedTransitionStatus = false;
        private buttonContainer[] buttons = new buttonContainer[4]; 

        private struct buttonContainer
        {
            public Button button;
            public bool clicked; 
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            clientService = new Client();

            buttons[0].button = FindViewById<Button>(Resource.Id.connection_button1);
            buttons[0].clicked = false; 
            buttons[1].button = FindViewById<Button>(Resource.Id.connection_button2);
            buttons[1].clicked = false; 
            buttons[2].button = FindViewById<Button>(Resource.Id.connection_button3);
            buttons[2].clicked = false;
            buttons[3].button = FindViewById<Button>(Resource.Id.connection_button4);
            buttons[3].clicked = false; 
            assignEventHandlers();
            beginSearchForHostAsync(); 
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async Task beginSearchForHostAsync()
        {
            await Task.Run(scanNetworkAsync); 
        }
        
        private async Task scanNetworkAsync()
        {
            var connectionBar = FindViewById<TextView>(Resource.Id.connectionStatus);
            connectionBar.Text = "Searching for host...";

            var connected = await Task.Run(clientService.StartClientAsync);
            updateConnectionStatus(connected); 
            if (connected)
            {
                connectionBar.Text = "Connected";
            }
            else
            {
                connectionBar.Text = "Not Connected"; 
            }
        }

        //update the connection status bar on the UI with new text and new color
        async void updateConnectionStatusBar(object sender, EventArgs args)
        {
            TextView connectionView = (TextView)FindViewById(Resource.Id.connectionStatus);
            Drawable drawable = connectionView.Background; 
            if (drawable.GetType() == typeof(TransitionDrawable))
            {
                if (!connectedTransitionStatus)
                {
                    ((TransitionDrawable)drawable).StartTransition(500); 
                    connectedTransitionStatus = true;
                    connectionView.Text = "Connected";
                }
                else
                {
                    ((TransitionDrawable)drawable).ReverseTransition(500);
                    connectedTransitionStatus = false;
                    connectionView.Text = "Disconnected";
                }     
            }
        }

        private void updateConnectionStatus(bool connected)
        {
            TextView connectionView = (TextView)FindViewById(Resource.Id.connectionStatus);
            Drawable drawable = connectionView.Background;
            if (drawable.GetType() == typeof(TransitionDrawable))
            {
                if (connected)
                {
                    ((TransitionDrawable)drawable).StartTransition(500);
                    connectedTransitionStatus = true;
                    connectionView.Text = "Connected";
                }
                else
                {
                    ((TransitionDrawable)drawable).ReverseTransition(500);
                    connectedTransitionStatus = false;
                    connectionView.Text = "Disconnected";
                }
            }
            
        }

        //go through each page element and give it an eventhandler if applicable
        private void assignEventHandlers()
        {
            //FindViewById<Button>(Resource.Id.btn_connect).Click += OnConnectClicked; 
            //FindViewById<TextView>(Resource.Id.connectionStatus).Click += updateConnectionStatusBar;
            FindViewById<Button>(Resource.Id.connection_button1).Click += OnChennelSelect;
            FindViewById<Button>(Resource.Id.connection_button2).Click += OnChennelSelect;
            FindViewById<Button>(Resource.Id.connection_button3).Click += OnChennelSelect;
            FindViewById<Button>(Resource.Id.connection_button4).Click += OnChennelSelect;
        }

        #region EventHandlers
        async void OnConnectClicked(object sender, EventArgs args)
        {
            if (!clientService.IsConnected())
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
        }


        async void OnChennelSelect(object sender, EventArgs args)
        {
            var clickedButton = (Button)sender;
            for (int i = 0; i < buttons.Length; i++)
            {
                if ((buttons[i].button.Id == clickedButton.Id) || ((buttons[i].button.Id != clickedButton.Id) && buttons[i].clicked))
                {
                    TransitionButton(i);
                }
            }
        }
            
        async void RequestServerTime(object sender, EventArgs args)
        {
            txtFeedback.Text = "Requesting time from server...."; 
            String serverTime = await Task.Run(clientService.GetServerTimeAsync);
            txtFeedback.Text = serverTime; 
        }
        #endregion

        private void TransitionButton(int target)
        {
            Button buttonView = buttons[target].button; 
            Drawable drawable = buttonView.Background;
            if (drawable.GetType() == typeof(TransitionDrawable))
            {
                if (buttons[target].clicked)
                {
                    ((TransitionDrawable)drawable).ReverseTransition(500);
                    buttons[target].clicked = false;
                }
                else
                {
                    ((TransitionDrawable)drawable).StartTransition(500);
                    buttons[target].clicked = true;
                }
            }
        }
    }
}