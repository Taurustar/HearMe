using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;

namespace sdkMapControlWP8CS
{
    public partial class AudioPlayerWindow : PhoneApplicationPage
    {

        private string _name;
        private string _description;
        private string _imageUri;
        private string _audio;


        public AudioPlayerWindow()
        {
            InitializeComponent();
            BotonGrabar.Unchecked += AudioStop;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            
            _name = Convert.ToString(NavigationContext.QueryString["Name"]);
            _description = Convert.ToString(NavigationContext.QueryString["Description"]);
            _imageUri = Convert.ToString(NavigationContext.QueryString["ImageUri"]);
			_audio = Convert.ToString(NavigationContext.QueryString["Audio"]);

            Name.Text = _name;
			AudioPlayer.Source = new Uri(_audio,UriKind.RelativeOrAbsolute);
            descBox.Text = _description;
            if(MainPage.lugarGlobal.ImageUri != null)
            Imagen.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(MainPage.lugarGlobal.ImageUri));
            else Imagen.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("Assets/Logo336x336.png",UriKind.RelativeOrAbsolute));
            
        }



        private void AudioStop(object sender, RoutedEventArgs e)
        {
        	AudioPlayer.Stop();
        }


        private void AudioPlay(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Play();
        }
    }
}