using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace sdkMapControlWP8CS
{
    class Pines
    {

        private Canvas myCanvas;
        public Location _lugar;
        private MainPage _mainPage;
                                                                                                                                                                                                                                                                                 
        const int MIN_ZOOM_LEVEL = 1;
        const double LEFT = -75;
        const double TOP = -275;
        const int MAX_ZOOM_LEVEL = 20;
        const int MIN_ZOOMLEVEL_FOR_LANDMARKS = 14;

        public Pines(Canvas canvas, Location lugar, MainPage mainPage)
        {
            myCanvas = canvas;
            _lugar = lugar;
            _mainPage = mainPage;


            Image image = new Image();
            image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("Assets/globoFondo.png", UriKind.RelativeOrAbsolute));
            image.Opacity = 0.8;

            image.Stretch = System.Windows.Media.Stretch.None;
            Canvas.SetLeft(image, LEFT);
            Canvas.SetTop(image, TOP);
            myCanvas.Children.Add(image);

            TextBlock titulo = new TextBlock();
            titulo.FontSize = 16;
            titulo.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            if (lugar.Title != null) titulo.Text = lugar.Title;
            else titulo.Text = "Hear Me";
            //titulo.IsEnabled = false;
            Canvas.SetTop(titulo, TOP + 20);
            Canvas.SetLeft(titulo, LEFT + 30);

            myCanvas.Children.Add(titulo);

            Image image2 = new Image();
            image2.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("Assets/pin.png", UriKind.RelativeOrAbsolute));
            //lugarGlobal = lugar;
            //image2.Tap += goToReproducir;
            image2.Opacity = 0.8;
            image2.Stretch = System.Windows.Media.Stretch.None;
            Canvas.SetTop(image2, TOP + 80);
            Canvas.SetLeft(image2, LEFT + 40);

            myCanvas.Children.Add(image2);
            image2.Tap += image2_Tap;
        }

        private void image2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPage.lugarGlobal = _lugar;
            string navTo = string.Format("/AudioPlayerWindow.xaml?Name={0}&Description={1}&ImageUri={2}&Audio={3}", _lugar.Title, _lugar.description, _lugar.ImageUri,_lugar.FilePath);
            _mainPage.NavigationService.Navigate(new Uri(navTo, UriKind.RelativeOrAbsolute));
            
        }

    }
}
