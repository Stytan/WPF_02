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
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Interop;

namespace DZ_Galery
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Image> Images { set; get; }
        public static int ProgressValue { set; get; }
        public MainWindow()
        {
            InitializeComponent();
            //ProgressValue = 0;
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
                        //ProgressValue++;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                //ProgressValue = 0;
                //ProgressBar1.Maximum = Images.Count;
                //((Image)ListViewImages.SelectedItem).FileInfo
                ListViewImages.ItemsSource = Images;
                
            }
        }

        private void ListViewImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromFile(Images[ListViewImages.SelectedIndex].FileInfo.FullName, true))
            {
                using (Bitmap bitmap = new Bitmap(img))
                {
                    ImageOpened.Source = Imaging.CreateBitmapSourceFromHBitmap(
                        bitmap.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions()
                    );
                }
            }
            TbTakenData.Text = Images[ListViewImages.SelectedIndex].Metadata.DateTaken;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if(ListViewImages.SelectedIndex<ListViewImages.Items.Count)
            {
                ListViewImages.SelectedIndex++;
            }
            //Images[ListViewImages.SelectedIndex].Metadata.DateTaken
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewImages.SelectedIndex > 0)
            {
                ListViewImages.SelectedIndex--;
            }
        }
    }
}
