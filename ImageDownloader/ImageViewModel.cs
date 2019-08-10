using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ImageDownloader
{
    public class ImageViewModel : INotifyPropertyChanged
    {
        private readonly Image image;

        public string Url
        {
            get { return image.Url; }
            set
            {
                image.Url = value;
                OnPropertyChanged("Url");
            }
        }

        public string Path
        {
            get { return image.Path; }
            set
            {
                image.Path = value;
                OnPropertyChanged("Path");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public ImageViewModel(Image img)
        {
            image = img;
        }
    }
}
