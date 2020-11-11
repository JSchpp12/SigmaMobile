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
using Android.Media; 


namespace Sigma.Project
{
    [Activity(Label="Sigma",MainLauncher =true,Theme="@style/MyTheme.Splash",Icon="@drawable/Icon",NoHistory=true)]
    public class SplashScreen : Activity
    {
        private MediaPlayer mediaPlayer = new MediaPlayer(); 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            Task.Run(PlayStartupSoundAsync);
        }

        async void PlayStartupSoundAsync()
        {
            try
            {
                mediaPlayer = MediaPlayer.Create(this, Resource.Raw.Startup);
                mediaPlayer.Start();
                while (mediaPlayer.IsPlaying)
                {
                    await Task.Delay(500);
                }
                mediaPlayer.Release();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}