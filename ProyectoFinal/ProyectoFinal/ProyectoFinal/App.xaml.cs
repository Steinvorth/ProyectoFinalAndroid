using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.SimpleAudioPlayer;



namespace ProyectoFinal
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {

            PlayAudio();

        }

        private async void PlayAudio()
        {
            try
            {
                var stream = await FileSystem.OpenAppPackageFileAsync("Main Theme.mp3");
                await MediaPlayer.PlayAsync(stream);
            }
            catch (Exception ex)
            {
                
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

      
    }
}
