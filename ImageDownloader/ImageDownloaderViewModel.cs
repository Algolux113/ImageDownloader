using System;
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
        private WebClient webClient;

        private readonly string imageDownloadPath;

        private readonly string imagedefaultPath;

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
            Uri uri = new Uri(ImageViewModel.Url);

            if (!Directory.Exists(imageDownloadPath))
            {
                Directory.CreateDirectory(imageDownloadPath);
            }

            var fileName = $"{imageDownloadPath}/{Guid.NewGuid()}.{ Path.GetExtension(uri.AbsolutePath)}";

            ImageViewModel.Path = imagedefaultPath;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            try
            {
                using (webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                    webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                    await webClient.DownloadFileTaskAsync(uri, fileName);
                }

                ImageViewModel.Path = fileName;
            }
            catch (Exception ex)
            {
                ImageViewModel.Path = imagedefaultPath;

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ProgressValue = 0;

            if (!e.Cancelled)
            {
                MessageBox.Show("Загрузка завершена.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public ImageDownloaderViewModel()
        {
            webClient = new WebClient();

            imageDownloadPath = ConfigurationManager.AppSettings.Get("ImageDownloadPath");
            imagedefaultPath = "img/no_image.png";

            imageViewModel = new ImageViewModel(new Image { Path = imagedefaultPath, Url = "azaza"});

            ProgressValue = 0;
        }
    }
}
