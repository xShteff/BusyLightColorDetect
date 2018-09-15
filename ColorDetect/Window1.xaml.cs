using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Busylight;

namespace ColorDetect
{
    public partial class Window1 : Window
    {
        private SDK _light;
        public Window1()
        {
            InitializeComponent();
            _light = new SDK();
        }

        WebCam webcam;
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            webcam = new WebCam();
            webcam.InitializeWebCam(ref imgVideo);
            var prop = DependencyPropertyDescriptor.FromProperty(Image.SourceProperty, typeof(Image));
            prop.AddValueChanged(imgVideo, ImgVideo_SourceUpdated);
        }

        private void ImgVideo_SourceUpdated(object sender, EventArgs e)
        {
            BitmapSource input = (BitmapSource)imgVideo.Source;
            var color = Helper.GetAreaColorAverage(input, (int)(input.Width / 2), (int)(input.Height / 2), 30);
            var busyColor = new BusylightColor
            {
                BlueRgbValue = color.B,
                GreenRgbValue = color.G,
                RedRgbValue = color.R
            };
            _light.Light(busyColor);
        }

 

        private void bntStart_Click(object sender, RoutedEventArgs e)
        {
            webcam.Start();
        }

        private void bntStop_Click(object sender, RoutedEventArgs e)
        {
            webcam.Stop();
        }

        private void bntSaveImage_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource input = (BitmapSource)imgVideo.Source;
            var lel = Helper.GetAreaColorAverage(input, (int)(input.Height / 2), (int)(input.Width / 2), 25);
        }

        private void bntResolution_Click(object sender, RoutedEventArgs e)
        {
            webcam.ResolutionSetting();
        }

        private void bntSetting_Click(object sender, RoutedEventArgs e)
        {
            webcam.AdvanceSetting();
        }
    }
}
