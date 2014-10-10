using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Photobooth
{
    public class PhotoStrip
    {
        public int Max { get; set; }
        public List<Image> Pictures { get; set; }
        public delegate bool PhotoAdded(Image image, int index);

        public PhotoAdded Photo;

        public PhotoStrip(PhotoAdded photo, int max = 4)
        {
            Photo = photo;
            Max = max;
            Pictures = new List<Image>();

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

            Photo(image, Count() - 1);

            return true;
        }

        public int Count()
        {
            return Pictures.Count();
        }
    }
}
