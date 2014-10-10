using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Photobooth
{
    public class PhotoStrip : INotifyPropertyChanged
    {
        public int Max { get; set; }

        private ObservableCollection<Image> _items;

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
        }

        //Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Assign(PhotoStrip Photos, List<Image> ImageHolders)
        {
            foreach (var photo in Photos.Pictures)
            {
                var holder = ImageHolders.FirstOrDefault(x => x.Source == null);
                if (holder != null) holder.Source = photo.Source;
            }
        }

        public Boolean Push(Image image)
        {
            if (Pictures.Count >= Max) return false;
            Pictures.Add(image);

            return true;
        }

        public int Count()
        {
            return Pictures.Count();
        }
    }
}
