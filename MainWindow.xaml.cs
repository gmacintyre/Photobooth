using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Nikon;

namespace Photobooth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NikonManager Manager { get; set; }
        public NikonDevice Device { get; set; }
        public PhotoStrip photoStrip { get; set; }
        public List<Image> ImageHolders { get; set; }

        public List<Image> GUIImages { get; set; }



        public MainWindow()
        {
            
            InitializeComponent();
            photoStrip = new PhotoStrip(photoStrip_PhotoAdded, 5);
            Manager = new NikonManager(@"Type0010.md3");
            GUIImages = new List<Image>{MiniImage1,MiniImage2, MiniImage3, MiniImage4, MainImage};
            button_takePicture.Content = "Connect\n Camera";
            ImageHolders = new List<Image>{};

            Manager.DeviceAdded += new DeviceAddedDelegate(manager_DeviceAdded);
        }

        void manager_DeviceAdded(NikonManager sender, NikonDevice device)
        {
            Device = device;
            // Wire device events
            Device.ImageReady += new ImageReadyDelegate(device_ImageReady);
            // Enable buttons
            button_takePicture.IsEnabled = true;
            button_takePicture.Content = "Start";
            button_takePicture.FontSize = 56;

            

            try
            {
                int batteryLevel = device.GetInteger(eNkMAIDCapability.kNkMAIDCapability_BatteryLevel);
                Console.WriteLine("Battery Level: " + batteryLevel);
            }
            catch (NikonException ex)
            {
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Manager.Shutdown();
        }

        void device_ImageReady(NikonDevice sender, NikonImage image)
        {
            Console.WriteLine("Magic");
            photoStrip.Push(new Image{Source = LoadImage(image.Buffer)});
        }

        private void button_takePictureClick(object sender, RoutedEventArgs e)
        {
            //for (var i = 0; i < 4; i++)
            //{
                try
                {
                    Device.Capture();
                }
                catch (NikonException ex)
                {

                }
            //}
            
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

        public bool photoStrip_PhotoAdded(Image image, int index)
        {
           
            GUIImages[index].Source = image.Source;
            return false;
        }
    }
}
