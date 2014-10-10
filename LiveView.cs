using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Nikon;

namespace Photobooth
{
    public class LiveView : INotifyPropertyChanged
    {

        public Image CurrentImage { get; set; }

        public LiveView()
        {
            CurrentImage = new Image();
        }

        public void Start(NikonDevice device)
        {
            for (var i = 0; i < 5; i++)
            {

                device.LiveViewEnabled = true;
            
                CurrentImage.Source = LoadImage(device.GetLiveViewImage().JpegBuffer);
            
                device.LiveViewEnabled = false;
            }
        }


        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            Console.WriteLine("Current Image changed");
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
