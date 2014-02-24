using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace sdkMapControlWP8CS
{
    public partial class LogOut : PhoneApplicationPage
    {
        public LogOut()
        {
            InitializeComponent();
        }

        private void noSignOut(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	NavigationService.GoBack();
        }
		
		private void yesSignOut(object sender, Facebook.Client.Controls.SessionStateChangedEventArgs e)
		{
            App.session = Facebook.Client.Controls.FacebookSessionState.Closed;
			NavigationService.Navigate(new Uri("/LogIn.xaml",UriKind.RelativeOrAbsolute));
		}
    }
}