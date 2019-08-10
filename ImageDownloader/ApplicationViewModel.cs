using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
                    try
                    {
                        await LeftImageDownloader.DowloadFileAsync();

                        await CenterImageDownloader.DowloadFileAsync();

                        await RightImageDownloader.DowloadFileAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    StartAllEnable = true;
                }));
            }
        }

        private void RightImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProgressValue")
            {
                StartAllEnable = false;
                ProgressValue = LeftImageDownloader.ProgressValue + CenterImageDownloader.ProgressValue + RightImageDownloader.ProgressValue;
            }

            if (LeftImageDownloader.ProgressValue == 0 && RightImageDownloader.ProgressValue == 0 && CenterImageDownloader.ProgressValue == 0)
            {
                StartAllEnable = true;
            }
        }

        private void CenterImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProgressValue")
            {
                StartAllEnable = false;
                ProgressValue = LeftImageDownloader.ProgressValue + CenterImageDownloader.ProgressValue + RightImageDownloader.ProgressValue;
            }

            if (LeftImageDownloader.ProgressValue == 0 && RightImageDownloader.ProgressValue == 0 && CenterImageDownloader.ProgressValue == 0)
            {
                StartAllEnable = true;
            }
        }

        private void LeftImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProgressValue")
            {
                StartAllEnable = false;
                ProgressValue = LeftImageDownloader.ProgressValue + CenterImageDownloader.ProgressValue + RightImageDownloader.ProgressValue;
            }

            if (LeftImageDownloader.ProgressValue == 0 && RightImageDownloader.ProgressValue == 0 && CenterImageDownloader.ProgressValue == 0)
            {
                StartAllEnable = true;
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

            ProgressTotal = 100;

            StartAllEnable = true;
        }
    }
}
