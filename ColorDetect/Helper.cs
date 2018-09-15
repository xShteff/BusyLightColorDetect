using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;

namespace ColorDetect
{
    class Helper
    {
        //Block Memory Leak
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr handle);
        public static BitmapSource bs;
        public static IntPtr ip;
        public static BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {

            ip = source.GetHbitmap();

            bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ip);

            return bs;

        }
        public static Color GetPixelColor(BitmapSource bitmap, int x, int y)
        {
            Color color;
            var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            var bytes = new byte[bytesPerPixel];
            var rect = new Int32Rect(x, y, 1, 1);

            bitmap.CopyPixels(rect, bytes, bytesPerPixel, 0);

            if (bitmap.Format == PixelFormats.Bgra32)
                color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
            else if (bitmap.Format == PixelFormats.Bgr32)
                color = Color.FromRgb(bytes[2], bytes[1], bytes[0]);
            else
                color = Colors.Black;


            return color;
        }

        public static Color GetAreaColorAverage(BitmapSource bitmap, int xCenter, int yCenter, int size)
        {
            List<Color> colours = new List<Color>();
            int offset = (int)((size - 1) / 2);
            int currentX = xCenter - offset, currentY = yCenter - offset, currentYCopy = currentY;
            for (int x = currentX; x < (xCenter + offset); x++)
                for (int y = currentYCopy; y < (yCenter + offset); y++)
                    colours.Add(GetPixelColor(bitmap, x, y));

            var totals = new int[3] { 0, 0, 0 }; //r, g, b
            for (var i = 0; i < (colours.Count - 1); i++)
            {
                totals[0] += colours.ElementAt(i).R;
                totals[1] += colours.ElementAt(i).G;
                totals[2] += colours.ElementAt(i).B;
            }
            var test = colours.Average(x => x.G);
            Console.Write(test);
            Color finalColora = new Color
            {
                R = (byte)(totals[0] / colours.Capacity),
                G = (byte)(totals[1] / colours.Capacity),
                B = (byte)(totals[2] / colours.Capacity)
            };

            Color finalColor = new Color
            {
                R = (byte)colours.Average(x => x.R),
                G = (byte)colours.Average(x => x.G),
                B = (byte)colours.Average(x => x.B)
            };
            return finalColor;
        }
    }
}
