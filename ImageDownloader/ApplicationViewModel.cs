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

        private double progressValue;

        public double ProgressValue
        {
            get { return progressValue; }
            set
            {
                progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }

        private double progressTotal;

        public double ProgressTotal
        {
            get { return progressTotal; }
            set
            {
                progressTotal = value;
                OnPropertyChanged("ProgressTotal");
            }
        }

        private bool startAllEnable;

        public bool StartAllEnable
        {
            get { return startAllEnable; }
            set
            {
                startAllEnable = value;
                OnPropertyChanged("StartAllEnable");
            }
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
                    ProgressValue = 0; ProgressTotal = 1;

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

        private void RightImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProgressValue")
            {
                ProgressValue = LeftImageDownloader.ProgressValue + CenterImageDownloader.ProgressValue + RightImageDownloader.ProgressValue;
            }

            if (e.PropertyName == "ProgressTotal")
            {
                ProgressTotal = LeftImageDownloader.ProgressTotal + CenterImageDownloader.ProgressTotal + RightImageDownloader.ProgressTotal;
            }

            if (LeftImageDownloader.StartEnable == true && RightImageDownloader.StartEnable == true && CenterImageDownloader.StartEnable == true)
            {
                StartAllEnable = true;
            }
            else
            {
                StartAllEnable = false;
            }
        }

        private void CenterImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProgressValue")
            {
                ProgressValue = LeftImageDownloader.ProgressValue + CenterImageDownloader.ProgressValue + RightImageDownloader.ProgressValue;
            }

            if (e.PropertyName == "ProgressTotal")
            {
                ProgressTotal = LeftImageDownloader.ProgressTotal + CenterImageDownloader.ProgressTotal + RightImageDownloader.ProgressTotal;
            }

            if (LeftImageDownloader.StartEnable == true && RightImageDownloader.StartEnable == true && CenterImageDownloader.StartEnable == true)
            {
                StartAllEnable = true;
            }
            else
            {
                StartAllEnable = false;
            }
        }

        private void LeftImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProgressValue")
            {
                ProgressValue = LeftImageDownloader.ProgressValue + CenterImageDownloader.ProgressValue + RightImageDownloader.ProgressValue;
            }

            if (e.PropertyName == "ProgressTotal")
            {
                ProgressTotal = LeftImageDownloader.ProgressTotal + CenterImageDownloader.ProgressTotal + RightImageDownloader.ProgressTotal;
            }

            if (LeftImageDownloader.StartEnable == true && RightImageDownloader.StartEnable == true && CenterImageDownloader.StartEnable == true)
            {
                StartAllEnable = true;
            }
            else
            {
                StartAllEnable = false;
            }
        }

        public ApplicationViewModel()
        {
            leftImageDownloader = new ImageDownloaderViewModel();

            LeftImageDownloader.PropertyChanged += LeftImageDownloader_PropertyChanged;

            rightImageDownloader = new ImageDownloaderViewModel();

            RightImageDownloader.PropertyChanged += RightImageDownloader_PropertyChanged;

            centerImageDownloader = new ImageDownloaderViewModel();

            CenterImageDownloader.PropertyChanged += CenterImageDownloader_PropertyChanged;

            StartAllEnable = true; ProgressTotal = 0; ProgressValue = 0;
        }
    }
}
