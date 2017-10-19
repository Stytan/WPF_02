/*
 * Created by SharpDevelop.
 * User: sergey.lezhenko
 * Date: 10/17/2017
 * Time: 13:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;


namespace DZ_Galery
{
    /// <summary>
    /// Конвертирует FileInfo в BitmapSource
    /// </summary>
    public class BitmapConverter : IValueConverter
    {
        public BitmapConverter()
        {
        }

        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
        	//Читаем Image из файла
            using (System.Drawing.Image img = System.Drawing.Image.FromFile(((FileInfo)value).FullName, true))
            {
            	//Задаём размер уменьшенной картинки
                System.Drawing.Size size = new System.Drawing.Size
                {
                    Height = (int)(((double)img.Size.Height / img.Size.Width) * 160),
                    Width = 160
                };
                //Создаём из Image Bitmap уменьшенного размера
                using (Bitmap bitmap = new Bitmap(img, size))
                {
                	//Пытаемся изменить значение ProgressBar - почемуто не работает
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
					mainWindow.UpdateProgress(1);
					mainWindow.ProgressBar1.InvalidateVisual();
					//Возвращаем BitmapSourse для привязки
                    return Imaging.CreateBitmapSourceFromHBitmap(
                        bitmap.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions()
                    );
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
