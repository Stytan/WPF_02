/*
 * Created by SharpDevelop.
 * User: sergey.lezhenko
 * Date: 12.10.2017
 * Time: 9:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace DZ_Galery
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Image> Images { set; get; }
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод для вызова из BitmapConverter, меняет ProgressBar1.Value на заданное значение
        /// </summary>
		public void UpdateProgress(int value)
		{
			if (CheckAccess())
                this.ProgressBar1.Value = this.ProgressBar1.Value + value;
            else
            {
                Dispatcher.Invoke(() => { this.ProgressBar1.Value = this.ProgressBar1.Value + value; });
            }
		}
        void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryInfo dir = new DirectoryInfo(fbd.SelectedPath);
                List<FileInfo> Files = new List<FileInfo>(dir.GetFiles("*.jpg"));
                foreach (string ext in new[] { "*.png", "*.tif", "*.gif", "*.bmp", "*.tiff" })
                {
                    FileInfo[] fi = dir.GetFiles(ext);
                    Files.AddRange(fi);
                }
                ProgressBar1.Maximum = Files.Count;
				ProgressBar1.Value = 0;
                Images = new List<Image>();
                try
                {
                    foreach (FileInfo fi in Files)
                    {
                        BitmapMetadata meta;
                        using (FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            meta = (BitmapMetadata)BitmapFrame.Create(fs).Metadata;
                        }
                        Images.Add(
                            new Image
                            {
                                FileInfo = fi,
                                Metadata = meta
                            });
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                ListViewImages.ItemsSource = Images;
            }
        }

        private void ListViewImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if (ListViewImages.SelectedIndex >= 0) {
				BitmapImage bmi = new BitmapImage();
				bmi.BeginInit();
				bmi.UriSource = new Uri(Images[ListViewImages.SelectedIndex].FileInfo.FullName, UriKind.RelativeOrAbsolute);
				bmi.EndInit();
				ImageOpened.Source = bmi;
				Slider_ValueChanged(sender, new RoutedPropertyChangedEventArgs<double>(0,0));
				var img = Images[ListViewImages.SelectedIndex];
				TbTakenData.Text = img.Metadata.DateTaken;
				TbCamMan.Text = img.Metadata.CameraManufacturer;
				TbCamModel.Text = img.Metadata.CameraModel;
				TbFormat.Text = img.Metadata.Format;
				TbHeight.Text = (bmi.Height * bmi.DpiY / 96).ToString();
				TbWidth.Text = (bmi.Width * bmi.DpiX / 96).ToString();
			}
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if(ListViewImages.SelectedIndex<ListViewImages.Items.Count)
            {
                ListViewImages.SelectedIndex++;
            }
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewImages.SelectedIndex > 0)
            {
                ListViewImages.SelectedIndex--;
            }
        }
		void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (ImageOpened != null && ImageOpened.Source != null) {
				ImageOpened.Width = ImageOpened.Source.Width * slider.Value / 100;
			}
		}
    }
}
