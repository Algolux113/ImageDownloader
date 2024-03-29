﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace ImageDownloader
{
    class ImageDownloaderViewModel : INotifyPropertyChanged
    {
        public WebClient webClient;

        private readonly string imageDownloadDirectory;

        private readonly string imagedefaultPath;

        private string imageDownloadPath;

        private ImageViewModel imageViewModel;

        public ImageViewModel ImageViewModel
        {
            get { return imageViewModel; }
            set
            {
                imageViewModel = value;
                OnPropertyChanged("ImageViewModel");
            }
        }

        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                isRunning = value;
                OnPropertyChanged("IsRunning");
                OnPropertyChanged("StartEnable");
                OnPropertyChanged("StopEnable");
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

        public bool StartEnable
        {
            get { return isRunning ? false : true; }
        }

        public bool StopEnable
        {
            get { return isRunning ? true : false; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private RelayCommand startCommand;

        public RelayCommand StartCommand
        {
            get
            {
                return startCommand ?? (startCommand = new RelayCommand(async obj =>
                {
                    try
                    {
                        await DowloadFileAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }

        private RelayCommand stoptCommand;

        public RelayCommand StopCommand
        {
            get
            {
                return stoptCommand ?? (stoptCommand = new RelayCommand(obj =>
                {
                    webClient.CancelAsync();
                }));
            }
        }

        public async Task DowloadFileAsync()
        {
            if (string.IsNullOrEmpty(ImageViewModel.Url))
            {
                return;
            }

            Uri uri = new Uri(ImageViewModel.Url);

            if (!Directory.Exists(imageDownloadDirectory))
            {
                Directory.CreateDirectory(imageDownloadDirectory);
            }

            imageDownloadPath = $"{imageDownloadDirectory}/{Guid.NewGuid()}.{ Path.GetExtension(uri.AbsolutePath)}";

            if (File.Exists(imageDownloadPath))
            {
                File.Delete(imageDownloadPath);
            }

            try
            {
                using (webClient = new WebClient())
                {
                    IsRunning = true; ProgressValue = 0;

                    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                    webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                    await webClient.DownloadFileTaskAsync(uri, imageDownloadPath);
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(imageDownloadPath))
                {
                    File.Delete(imageDownloadPath);
                }

                imageDownloadPath = imagedefaultPath;

                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ImageViewModel.Path = imageDownloadPath;
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IsRunning = false;
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
        }

        public ImageDownloaderViewModel()
        {
            webClient = new WebClient();

            imageDownloadDirectory = ConfigurationManager.AppSettings.Get("ImageDownloadPath");
            imagedefaultPath = "img/no_image.png";
            imageDownloadPath = "";

            imageViewModel = new ImageViewModel(new Image { Path = imagedefaultPath, Url = ""});

            isRunning = false;
        }
    }
}
