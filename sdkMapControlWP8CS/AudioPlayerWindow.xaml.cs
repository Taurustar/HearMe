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

        private string _name; // Nombre del sonido en la base de datos
        private string _description; // Descripcion porporcionada por el usuario al momento de crearlo
        private string _imageUri; // Ruta de donde se encuentra la imagen en Azure para ser traida y mostrada
        private string _audio; // Ruta de donde se encuentra el audio en Azire para ser traido y reproducido


        public AudioPlayerWindow()
        {
            InitializeComponent();
            BotonGrabar.Unchecked += AudioStop;
        }

        /// <summary>
        /// Lo que hace este metodo es al minuto en que entra a esta ventana captura los datos que nosotros pasamos desde el pin para usarlos,
        /// estos son los datos desde azure y el geolocalizador
        /// </summary>
        /// <param name="e"></param>
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