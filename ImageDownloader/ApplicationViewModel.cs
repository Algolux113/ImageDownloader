using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ImageDownloader
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private ImageDownloaderViewModel leftImageDownloader;

        public ImageDownloaderViewModel LeftImageDownloader
        {
            get { return leftImageDownloader; }
            set
            {
                leftImageDownloader = value;
                OnPropertyChanged("LeftImageDownloader");
            }
        }

        private ImageDownloaderViewModel centerImageDownloader;

        public ImageDownloaderViewModel CenterImageDownloader
        {
            get { return centerImageDownloader; }
            set
            {
                centerImageDownloader = value;
                OnPropertyChanged("CenterImageDownloader");
            }
        }

        private ImageDownloaderViewModel rightImageDownloader;

        public ImageDownloaderViewModel RightImageDownloader
        {
            get { return rightImageDownloader; }
            set
            {
                rightImageDownloader = value;
                OnPropertyChanged("RightImageDownloader");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private RelayCommand stoptAllCommand;

        public RelayCommand StopAllCommand
        {
            get
            {
                return stoptAllCommand ?? (stoptAllCommand = new RelayCommand(obj =>
                {
                    MessageBox.Show("Stop");
                }));
            }
        }

        private RelayCommand starttAllCommand;

        public RelayCommand StartAllCommand
        {
            get
            {
                return starttAllCommand ?? (starttAllCommand = new RelayCommand(async obj =>
                {
                    try
                    {
                        await leftImageDownloader.DowloadFileAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }

        public ApplicationViewModel()
        {
            leftImageDownloader = new ImageDownloaderViewModel();
            rightImageDownloader = new ImageDownloaderViewModel();
            centerImageDownloader = new ImageDownloaderViewModel();
        }
    }
}
