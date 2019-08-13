using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        public double ProgressValue
        {
            get
            {
                double progress = 0;

                progress += leftImageDownloader.IsRunning ? leftImageDownloader.ProgressValue : 0;
                progress += centerImageDownloader.IsRunning ? centerImageDownloader.ProgressValue : 0;
                progress += rightImageDownloader.IsRunning ? rightImageDownloader.ProgressValue : 0;

                return progress;
            }
        }

        public double ProgressTotal
        {
            get
            {
                double total = 0;
                total += leftImageDownloader.IsRunning ? 100 : 0;
                total += rightImageDownloader.IsRunning ? 100 : 0;
                total += centerImageDownloader.IsRunning ? 100 : 0;
                return total;
            }
        }

        public bool StartAllEnable
        {
            get { return !leftImageDownloader.IsRunning && !centerImageDownloader.IsRunning && !rightImageDownloader.IsRunning; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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
                        Task leftImageDownloadTask = Task.Run(() => LeftImageDownloader.DowloadFileAsync());
                        Task centerImageDownloadTask = Task.Run(() => CenterImageDownloader.DowloadFileAsync());
                        Task rightImageDownloadTask = Task.Run(() => RightImageDownloader.DowloadFileAsync());

                        await Task.WhenAll(leftImageDownloadTask, centerImageDownloadTask, rightImageDownloadTask);
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
            leftImageDownloader.PropertyChanged += LeftImageDownloader_PropertyChanged;

            rightImageDownloader = new ImageDownloaderViewModel();
            rightImageDownloader.PropertyChanged += RightImageDownloader_PropertyChanged;

            centerImageDownloader = new ImageDownloaderViewModel();
            centerImageDownloader.PropertyChanged += CenterImageDownloader_PropertyChanged;
        }

        private void CenterImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsRunning")
            {
                OnPropertyChanged("StartAllEnable");
                OnPropertyChanged("ProgressTotal");
            }

            if (e.PropertyName == "ProgressValue")
            {
                OnPropertyChanged("ProgressValue");
            }
        }

        private void RightImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsRunning")
            {
                OnPropertyChanged("StartAllEnable");
                OnPropertyChanged("ProgressTotal");
            }

            if (e.PropertyName == "ProgressValue")
            {
                OnPropertyChanged("ProgressValue");
            }
        }

        private void LeftImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsRunning")
            {
                OnPropertyChanged("StartAllEnable");
                OnPropertyChanged("ProgressTotal");
            }

            if (e.PropertyName == "ProgressValue")
            {
                OnPropertyChanged("ProgressValue");
            }
        }
    }
}
