using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace Sigma.Project
{
    [Activity(Label="Sigma",MainLauncher =true,Theme="@style/MyTheme.Splash",Icon="@drawable/Icon",NoHistory=true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Task.Delay(8000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            //Task startupWork = new Task(() => { SimulateStartup(); });
        }

        async void SimulateStartup()
        {
            await Task.Delay(8000); // Simulate a bit of startup work.
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}