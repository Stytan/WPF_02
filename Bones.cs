/*
 * Created by SharpDevelop.
 * User: sergey.lezhenko
 * Date: 10/19/2017
 * Time: 15:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Puzzle15
{
	/// <summary>
	/// Description of Bones.
	/// </summary>
	public class Bones
	{
		public class Bone
		{
			public BitmapSource pic { set; get; }
			public Uri uri { set; get; }
			public int number { set; get; }
		}
		public List<Bone> Items {set;get;}
		public Bones()
		{
			//BitmapImage bitmap = new BitmapImage(new Uri("/images/oldUSSR.jpg", UriKind.RelativeOrAbsolute));
			Items = new List<Bone>();
			BitmapSource sourcePic;
			using (Image img = Image.FromFile("/images/oldUSSR.jpg", true))
            {
            	//Задаём размер уменьшенной картинки
//                System.Drawing.Size size = new System.Drawing.Size
//                {
//                    Height = (int)(((double)img.Size.Height / img.Size.Width) * 160),
//                    Width = 160
//                };
                //Создаём из Image Bitmap уменьшенного размера
                using (Bitmap bitmap = new Bitmap(img,300,300))
                {
					sourcePic = Imaging.CreateBitmapSourceFromHBitmap(
						bitmap.GetHbitmap(),
						IntPtr.Zero,
						Int32Rect.Empty,
						BitmapSizeOptions.FromEmptyOptions());
                }
            }
			//https://forums.asp.net/t/1004794.aspx
			using (Bitmap bitmap = new Bitmap(img, 300, 300)) {
				Items.Add(
					new Bone {
						pic = Imaging.CreateBitmapSourceFromHBitmap(
							bitmap.GetHbitmap(),
							IntPtr.Zero,
							Int32Rect.Empty,
							BitmapSizeOptions.FromEmptyOptions()),
						number = 0
					}
				);
			}
		}
	}
}
