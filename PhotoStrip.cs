using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Nikon;
using Image = System.Windows.Controls.Image;

namespace Photobooth
{
    public class PhotoStrip : INotifyPropertyChanged
    {
        public int Max { get; set; }

        private ObservableCollection<Image> _items;

        public struct PicturesWithData
        {
            public string Name;
            public System.Drawing.Bitmap Picture;
        }
        public List<PicturesWithData> PicturesWithDataList { get; set; }
        public ObservableCollection<Image> Pictures
        {
            get { return _items; }
            set
            {
                _items = value;   //Not the best way to populate your "items", but this is just for demonstration purposes.
                this.RaisePropertyChanged("ListItems");
            }
        }
        public delegate bool PhotoAdded(Image image, int index);

        public PhotoAdded Photo;

        public PhotoStrip(int max = 4)
        {
            Max = max;
            Pictures = new ObservableCollection<Image>();
            PicturesWithDataList = new List<PicturesWithData>();
        }

        //Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        //public void Assign(PhotoStrip Photos, List<Image> ImageHolders)
        //{
        //    foreach (var photo in Photos.Pictures)
        //    {
        //        var holder = ImageHolders.FirstOrDefault(x => x.Source == null);
        //        if (holder != null) holder.Source = photo.Source;
        //    }
        //}
        public  BitmapImage LoadWindowsControlImage(byte[] imageData)
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

        public Boolean Push(NikonImage image)
        {
            if (Pictures.Count >= Max) return false;
            Pictures.Add(new System.Windows.Controls.Image { Source = LoadWindowsControlImage(image.Buffer) });
            PicturesWithDataList.Add(new PicturesWithData() { Name = image.Number.ToString(), Picture = byteArrayToBitmap(image.Buffer)});
            return true;
        }

        public Bitmap byteArrayToBitmap(byte[] imageData)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(imageData))
            {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }

        public void DrawStrip()
        {
            var img = CombineBitmap();
            foreach (var singleimage in PicturesWithDataList)
            {
                SaveImage(singleimage);
            }
            
        }

        public static Bitmap ResizeImage(Bitmap imgToResize)
        {
            var size = new System.Drawing.Size(1232, 816);
            try
            {
                Bitmap b = new Bitmap(size.Width, size.Height);
                using (Graphics g = Graphics.FromImage((System.Drawing.Image)b))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }
                return b;
            }
            catch
            {
                Console.WriteLine("Bitmap could not be resized");
                return imgToResize;
            }
        }

        public System.Drawing.Bitmap CombineBitmap()
        {
            var frameFilePath = @"C:\Projects\Photobooth\Images\FrameTemplate\frame.jpg";
            using (var bmpTemp = new Bitmap(frameFilePath))
            {
                var frame = new Bitmap(bmpTemp);
                Graphics g = Graphics.FromImage(frame);
                g.CompositingMode = CompositingMode.SourceCopy;

                int x = 57;
                int y = 63;
                foreach (var obj in PicturesWithDataList)
                {
                    var bmp = ResizeImage(obj.Picture);
                    g.DrawImage(bmp, new System.Drawing.Point(x, y));
                    y += 870;
                }
                var filePath = string.Format(@"C:\Projects\Photobooth\Images\Strips\{0}.jpeg", DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss"));
                SaveImage(frame, filePath);
                return frame;
            }
        }
        private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            var unlockedBitmap = new Bitmap(bitmap);
            using (MemoryStream memory = new MemoryStream())
            {
                
                unlockedBitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;     
            }
            return null;
        }


        public string SaveImage(PicturesWithData image)
        {
            var path = string.Format(@"C:\Projects\Photobooth\Images\Individual\{0}.jpeg", image.Name);
            return SaveImage(image.Picture, path);
        }

        public string SaveImage(Bitmap image, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new JpegBitmapEncoder();
                var bmi = Bitmap2BitmapImage(image);
                if (bmi == null)
                {
                    return "error";
                }
                encoder.Frames.Add(BitmapFrame.Create(bmi));
                encoder.Save(fileStream);
            }
            return filePath;
        }

        public BitmapImage ImageToBitmapImage(Image img)
        {
            MemoryStream ms = null;
            JpegBitmapEncoder jpegBitmapEncoder = null;
            BitmapEncoder bencoder = new JpegBitmapEncoder();

            System.Drawing.Bitmap bmp = null;
            BitmapImage bitmapImage = new BitmapImage();

            if ((int)img.Source.Width > 0)
            {
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)img.Source.Width,
                                                                               (int)img.Source.Height,
                                                                               100, 100, PixelFormats.Default);
                renderTargetBitmap.Render(img);
                bencoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                using (ms = new MemoryStream())
                {
                    bencoder.Save(ms);
                    {
                        bmp = new Bitmap(ms);

                        //bmp.Save("C:/bmp_thing.jpg");

                        if (bmp != null)
                        {
                            bmp.Save(ms, ImageFormat.Jpeg);
                            ms.Position = 0;
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = ms;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();

                            return bitmapImage;
                        }
                        bmp.Dispose();
                    }
                }
            }
            return null;
        }

        public byte[] imageToByteArray(BitmapImage imageIn)
        {
            Stream stream = imageIn.StreamSource;
            Byte[] buffer = null;
            if (stream != null && stream.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    buffer = br.ReadBytes((Int32)stream.Length);
                }
            }

            return buffer;
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        public Image ResizeImage(Image img)
        {
            double resizeWidth = 1024;
            double resizeHeight = 960;

            double aspect = resizeWidth / resizeHeight;

            resizeHeight = resizeWidth / aspect;

            aspect = resizeWidth / resizeHeight;

            resizeWidth = resizeHeight * aspect;


            img.Width = resizeWidth;
            img.Height = resizeHeight;
            return img;
        }

        public int Count()
        {
            return Pictures.Count();
        }
    }
}
