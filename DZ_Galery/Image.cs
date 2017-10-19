/*
 * Created by SharpDevelop.
 * User: sergey.lezhenko
 * Date: 10/17/2017
 * Time: 11:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace DZ_Galery
{
    /// <summary>
    /// Хранит информацию о файле картинки и его метаданные
    /// </summary>
    public class Image
    {
        public FileInfo FileInfo { get; set; }
        public BitmapMetadata Metadata { get; set; }
    }
}
