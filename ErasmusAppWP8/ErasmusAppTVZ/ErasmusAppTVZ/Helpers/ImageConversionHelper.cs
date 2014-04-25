using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ErasmusAppTVZ.Helpers
{
    class ImageConversionHelper
    {
        public static BitmapImage ToImage(string imageData)
        {
            byte[] buffer = Convert.FromBase64String(imageData);

            using (MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length))
            {
                ms.Write(buffer, 0, buffer.Length);
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(ms);

                return bitmap;
            }
        }
    }
}
