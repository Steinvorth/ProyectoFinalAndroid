using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.SimpleAudioPlayer;
using System.IO;


namespace ProyectoFinal
{
    public partial class App : Application
    {
        ISimpleAudioPlayer player;

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
                player = CrossSimpleAudioPlayer.Current;
                var stream = await FileSystem.OpenAppPackageFileAsync(Path.Combine("Audio", "Main Theme.mp3"));
                player.Load(stream);
                player.Play();

                // Mensaje de depuración para verificar que el audio se está reproduciendo
                System.Diagnostics.Debug.WriteLine("Audio reproducido con éxito.");
            }
            catch (Exception ex)
            {
                // Manejar errores
                System.Diagnostics.Debug.WriteLine($"Error al reproducir audio: {ex.Message}");
            }
        }
    }
}

