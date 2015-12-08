using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using Nikon;
using System.Drawing;


namespace Photobooth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NikonManager Manager { get; set; }
        public NikonDevice Device { get; set; }
        public PhotoStrip photostrip { get; set; }
        private DispatcherTimer liveViewTimer;
        public LiveView _LiveView { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Initialize live view timer
            liveViewTimer = new DispatcherTimer();
            liveViewTimer.Tick += new EventHandler(liveViewTimer_Tick);
            liveViewTimer.Interval = new TimeSpan(0,0,1);

            _LiveView = new LiveView();
            photostrip = new PhotoStrip();

            Manager = new NikonManager(@"Type0004.md3");
            button_takePicture.Content = "Connect\n Camera";
            button_takePicture.IsEnabled = false;
            
            Manager.DeviceAdded += new DeviceAddedDelegate(manager_DeviceAdded);
        }

        public  void liveViewTimer_Tick(object sender, EventArgs e)
        {
            // Get live view image
            NikonLiveViewImage image = null;

            try
            {
                image = Device.GetLiveViewImage();
            }
            catch (NikonException)
            {
                liveViewTimer.Stop();
            }

            // Set live view image on picture box
            if (image != null)
            {
                LiveImage.Source = photostrip.LoadWindowsControlImage(image.JpegBuffer);
            }
        }

        private void manager_DeviceAdded(NikonManager sender, NikonDevice device)
        {
            Device = device;
            // Wire device events
            Device.ImageReady += new ImageReadyDelegate(device_ImageReady);
            // Enable buttons
            button_takePicture.IsEnabled = true;
            button_takePicture.Content = "Start";
            ToggleLiveView();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Manager.Shutdown();
        }

        private void device_ImageReady(NikonDevice sender, NikonImage image)
        {
            Console.WriteLine("Image Ready");
            photostrip.Push(image);
             //fill dat strip
            LiveImage.Source = photostrip.LoadWindowsControlImage(image.Buffer);
            if(photostrip.Pictures.Count() != photostrip.Max) TakePicture();
            else photostrip.DrawStrip();
        }

        private void button_takePictureClick(object sender, RoutedEventArgs e)
        {
            photostrip = new PhotoStrip();
            TakePicture();
        }

        public void TakePicture()
        {
            //Thread.Sleep(2000);
            try
            {
                //Device
                Device.Capture();
                ImageList.ItemsSource = photostrip.Pictures;
            }
            catch (NikonException ex)
            {
                Console.Write(ex.Message);

            }
        }

        private void button_startLiveView(object sender, RoutedEventArgs e)
        {
            ToggleLiveView();   
        }

        public void ToggleLiveView()
        {
            if (Device == null)
            {
                return;
            }
            if (Device.LiveViewEnabled)
            {
                Device.LiveViewEnabled = false;
                liveViewTimer.Stop();
                LiveImage.Source = null;
            }
            else
            {
                Device.LiveViewEnabled = true;
                liveViewTimer.Start();
            }
        }

        

        
    }

   
}
