using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Coding4Fun.Toolkit.Audio;
using Coding4Fun.Toolkit.Audio.Helpers;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using sdkMapControlWP8CS.ViewModels;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Diagnostics;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;
using System.IO.IsolatedStorage;
using Windows.Devices.Geolocation;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;


namespace sdkMapControlWP8CS
{
    public partial class AudioEdition : PhoneApplicationPage
    {
        private SoundData soundData;

        private IMobileServiceTable<Location> LocationTable = App.MobileService.GetTable<Location>();


        //private IMobileServiceTable<SoundData> SoundData = App.MobileService.GetTable<SoundData>();


        //actual audio grabado
        private MicrophoneRecorder _recorder = new MicrophoneRecorder();

        //revisa el buffer de audio
        private IsolatedStorageFileStream _audioStream;
        private string _tempFileName = "tempWav.wav";

        //sacar audioName!!!
        private Geoposition myGeoposition;
        private Stream stream;

        // Using the CameraCaptureTask to allow the user to capture a todo item image //
        CameraCaptureTask cameraCaptureTask;

        // Using a stream reference to upload the image to blob storage.
        Stream imageStream = null;





        /*//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////*/
        public AudioEdition()
        {
            InitializeComponent();
			cameraCaptureTask = new CameraCaptureTask();

            cameraCaptureTask.Completed += cameraCompleted;
        }
	
			
		 protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
            string _audio = Convert.ToString(NavigationContext.QueryString["AudioUri"]);
			MessageBox.Show(_audio);
		}
		
        //public void beginInsert()
        //{
        //        soundData = new SoundData();
        //        soundData.FilePath = string.Format("/customAudio/{0}.wav", DateTime.Now.ToFileTime());
        //        soundData.Title = txt_title.Text;
        //        soundData.Latitude = myGeoposition.Coordinate.Latitude;
        //        soundData.Longitude = myGeoposition.Coordinate.Longitude;

        //        Location location = new Location();
        //        location.Latitude = myGeoposition.Coordinate.Latitude;
        //        location.Longitude = myGeoposition.Coordinate.Longitude;
        //        location.Title = txt_title.Text;
        //        location.FilePath = soundData.FilePath;
                
            
                
        //        InsertarImagen(location);
        //        InsertarAudio(location);

        //        // Save wav file into directory /customAudio/
        //        using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication()) 
        //        {
        //            if (!isoStore.DirectoryExists("/customAudio/"))
        //                isoStore.CreateDirectory("/customAudio/");

        //            isoStore.MoveFile(_tempFileName, soundData.FilePath);
        //        }

        //        // Add the SoundData to App.ViewModel.CustomSounds
        //        App.ViewModel.CustomSounds.Items.Add(soundData);

        //        // Save the list of CustomSounds to IsolatedStorage.ApplicationSettings
        //        var data = JsonConvert.SerializeObject(App.ViewModel.CustomSounds);

        //        IsolatedStorageSettings.ApplicationSettings[SoundModel.CustomSoundKey] = data;
        //        IsolatedStorageSettings.ApplicationSettings.Save();

        //        // We'll need to modify our SoundModel to retrieve CustomSounds
        //        // from IsolatedStorage.ApplicationSettings

        //        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        //}

         private async void InsertarImagen(Location location)
         {
             Debug.WriteLine("INSERT IMAGEN");
             string errorString = string.Empty;

             if (imageStream != null)
             {
                 // Set blob properties of TodoItem.
                 location.ContainerName = "locationImages";
                 location.ResourceName = Guid.NewGuid().ToString() + ".jpg";
             }

             Debug.WriteLine("SASQUERY: " + location.SasQueryString);
             Debug.WriteLine("promt Promt");
             //PrompToSubmit(location);
             await LocationTable.InsertAsync(location);
             Debug.WriteLine("SASQUERY: " + location.SasQueryString);
             Debug.WriteLine("promt succes");

             Debug.WriteLine("0");
             try
             {
                 Debug.WriteLine("0.1");

                 var temp = location.SasQueryString;

                 // If we have a returned SAS, then upload the blob.
                 if (!string.IsNullOrEmpty(temp))
                 {
                     // Get the URI generated that contains the SAS 
                     // and extract the storage credentials.
                     Debug.WriteLine("1");
                     StorageCredentials cred = new StorageCredentials(location.SasQueryString);
                     var imageUri = new Uri(location.ImageUri);

                     Debug.WriteLine("2");
                     // Instantiate a Blob store container based on the info in the returned item.
                     CloudBlobContainer container = new CloudBlobContainer(
                         new Uri(string.Format("https://{0}/{1}",
                             imageUri.Host, location.ContainerName)), cred);
                     Debug.WriteLine("3");
                     // Upload the new image as a BLOB from the stream.
                     CloudBlockBlob blobFromSASCredential =
                         container.GetBlockBlobReference(location.ResourceName);
                     await blobFromSASCredential.UploadFromStreamAsync(imageStream);
                     Debug.WriteLine("4");
                     // When you request an SAS at the container-level instead of the blob-level,
                     // you are able to upload multiple streams using the same container credentials.

                     imageStream = null;
                     Debug.WriteLine("5");
                 }
                 else
                 {
                     Debug.WriteLine("no se pudo");
                 }
             }
             catch (Exception ex)
             {
                 Debug.WriteLine("error: " + ex.Message);
             }
         }

         private async void InsertarAudio(Location location)
         {
             Debug.WriteLine("INSERT AUDIO");
             string errorString = string.Empty;

             if (stream != null)
             {
                 Debug.WriteLine(" audiostream != null");
                 // Set blob properties of TodoItem.
                 location.ContainerName = "audio";
                 location.ResourceName = Guid.NewGuid().ToString() + ".wav";
             }



             Debug.WriteLine("SASQUERY: " + location.SasQueryString);
             Debug.WriteLine("promt Promt");
             await LocationTable.InsertAsync(location);
             Debug.WriteLine("SASQUERY: " + location.SasQueryString);
             Debug.WriteLine("promt succes");

             Debug.WriteLine("0");
             try
             {
                 Debug.WriteLine("0.1");

                 var temp = location.SasQueryString;

                 // If we have a returned SAS, then upload the blob.
                 if (!string.IsNullOrEmpty(temp))
                 {
                     // Get the URI generated that contains the SAS 
                     // and extract the storage credentials.
                     Debug.WriteLine("1");
                     StorageCredentials cred = new StorageCredentials(location.SasQueryString);
                     var imageUri = new Uri(location.ImageUri);

                     Debug.WriteLine("2");
                     // Instantiate a Blob store container based on the info in the returned item.
                     CloudBlobContainer container = new CloudBlobContainer(
                         new Uri(string.Format("https://{0}/{1}",
                             imageUri.Host, location.ContainerName)), cred);
                     Debug.WriteLine("3");
                     // Upload the new image as a BLOB from the stream.



                     CloudBlockBlob blobFromSASCredential =
                         container.GetBlockBlobReference(location.ResourceName);
                     await blobFromSASCredential.UploadFromStreamAsync(stream);

                     Debug.WriteLine("4");
                     // When you request an SAS at the container-level instead of the blob-level,
                     // you are able to upload multiple streams using the same container credentials.

                     imageStream = null;
                     Debug.WriteLine("5");
                 }
                 else
                 {
                     Debug.WriteLine("no se pudo");
                 }
             }
             catch (Exception ex)
             {
                 Debug.WriteLine("error: " + ex.Message);
             }
         }

         private async void InserData(CloudBlobContainer container)
         {
             Debug.WriteLine("INSERT DATA");
             try
             {
                 // Retrieve reference to a blob 
                 CloudBlockBlob blockBlob = container.GetBlockBlobReference("blob");

                 /*
                 using (var fileStream = System.IO.File.OpenRead(@"localFile"))
                 {
                     //blockBlob.UploadFromStream(fileStream);
                     blockBlob.UploadFromStreamAsync(_audioStream);
                 }*/

                 /*using (var fileStream = _audioStream)
                 {
                     //blockBlob.UploadFromStream(fileStream);
                     blockBlob.UploadFromStreamAsync(fileStream);
                 }*/

                 await blockBlob.UploadFromStreamAsync(imageStream);

             }
             catch (Exception e)
             {
                 Debug.WriteLine("Error: " + e.Message);
             }
         } 







		private void initCamera(object sender, System.Windows.Input.GestureEventArgs e)
		{
			cameraCaptureTask.Show();
		}
		

        //Captura la imagen y la muestra

		private void cameraCompleted(object sender, PhotoResult e)
		{
            Image tempImage = new Image();
			 imageStream = e.ChosenPhoto;
             string uri = e.ChosenPhoto.Length.ToString() + ".jpg";
             System.Windows.Media.Imaging.BitmapImage img = new System.Windows.Media.Imaging.BitmapImage();
             img.SetSource(e.ChosenPhoto);
             p_image.Source = img;
		}
		
        
    }
}