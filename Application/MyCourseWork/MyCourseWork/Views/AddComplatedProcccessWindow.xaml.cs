namespace MyCourseWork.Views
{
    using AForge.Video;
    using AForge.Video.DirectShow;
    using Catel.Windows;
    using MyCourseWork.Models;
    using System;
    using System.Linq;
    using ViewModels;
    using ZXing;

    public partial class AddComplatedProcccessWindow
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        VideoCaptureDevice FinalVideo;
        FilterInfoCollection devicesCollection;

        public AddComplatedProcccessWindow()
            : this(null) { }

        public AddComplatedProcccessWindow(AddComplatedProcccessWindowViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
            pictureBox1.Image = null;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            InfoAboutWorker.Content = "Піднесіть свій бейджик до камери!";
            CoderDecoder.CoderDecoder.code();
        }

        private void DataWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            devicesCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo VideoCaptureDevice in devicesCollection)
            {
                VideoDevicesComboBox.Items.Add(VideoCaptureDevice.Name);
            }
            VideoDevicesComboBox.SelectedIndex = 0;
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                FinalVideo.Stop();
                dispatcherTimer.Stop();
            }
            catch { }
            FinalVideo = new VideoCaptureDevice(devicesCollection[VideoDevicesComboBox.SelectedIndex].MonikerString);
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            FinalVideo.Start();

            dispatcherTimer.Start();
        }
        private void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            System.Drawing.Bitmap video = (System.Drawing.Bitmap)eventArgs.Frame.Clone();

            pictureBox1.Image = video;
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            BarcodeReader reader = new BarcodeReader();
            Result result = new Result("", null, null, BarcodeFormat.All_1D);
            if (pictureBox1.Image != null)
                result = reader.Decode((System.Drawing.Bitmap)pictureBox1.Image);
            try
            {
                string decode = result.ToString().Trim();
                if (decode != "")
                {
                    int i = CoderDecoder.CoderDecoder.processOrWorker(decode);

                    if (i == 2)
                    {
                        fullInfoFromQRCode.Text = CoderDecoder.CoderDecoder.getFullInfoAboutProccess(CoderDecoder.CoderDecoder.decode(decode));
                        id_procces.Content = decode.Split(' ')[0];
                        ClassForAudio.playScan();
                    }
                    else
                    {
                        string str = CoderDecoder.CoderDecoder.getInfoAboutWorker(decode);
                        InfoAboutWorker.Content = str.Split('_')[0];
                        id_worker.Content = str.Split('_')[1];
                        ClassForAudio.playScan();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "not found process")
                {
                    fullInfoFromQRCode.Text = "Такого Завдання не знайдено!!!";
                    ClassForAudio.playNoScan();
                }
                if (ex.Message == "not found worker")
                {
                    InfoAboutWorker.Content = "Такого працівника не знайдено!!!";
                    ClassForAudio.playNoScan();
                }
            }
        }

        private void DataWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FinalVideo.Stop();
        }
    }
}
