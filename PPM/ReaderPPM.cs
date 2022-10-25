using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows;
using Microsoft.Win32;
//[System.Runtime.InteropServices.DllImport("gdi32.dll")]


namespace zad1___paint.PPM
{
    public class ReaderPPM
    {
        /* -- File settings -- */
        int width = 0;
        int height = 0;
        int maxValue = 0;

        /* -- Read image from file and choose right method depends on file format -- */

        public Bitmap LoadPPMFile()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "d:\\";
            openFileDialog.Filter = "Image files (*.p3; *p6; *jpg)|*.jpg;*.p3;*.p6 | All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                string fileExtension = Path.GetExtension(filePath);

                if (fileExtension == ".ppm")
                {
                    return CheckPpmFormat(filePath);
                }
                else
                {
                    throw new Exception("Given image format is not supported!");
                }
            }
            throw new Exception("User clicked cancel button");
        }

        public Bitmap CheckPpmFormat(string filePath)
        {

            BinaryReader file = new BinaryReader(new FileStream(filePath, FileMode.Open));

            if(file.ReadChar() =='P')
            {
                char number = file.ReadChar();  // It stores information about P format
                width = GetNextValue(file);     // So we can read with
                height = GetNextValue(file);    // and height value
 
                switch(number)
                {
                    case '1':
                        // -- P1
                        return ReadASCIIBitmapImage_P1(file);

                    case '2':
                        // -- P2
                        return ReadASCIIBitmapImage_P2(file);

                    case '3':
                        // -- P3
                        return ReadASCIIBitmapImage_P3(file);

                    case '4':
                        // -- P4
                        return ReadBinaryBitmapImage_P4(file);

                    case '5':
                        // -- P5
                        return ReadBinaryBitmapImage_P5(file);

                    case '6':
                        // -- P6
                        return ReadBinaryBitmapImage_P6(file);
                        break;

                    default:
                        break;
                }
            }
            return null;
        }

        private Bitmap ReadASCIIBitmapImage_P1(BinaryReader file)
        {

            Bitmap bitmap = new Bitmap(width, height);

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    int bit = GetNextValue(file) == 0 ? 255 : 0;
                    bitmap.SetPixel(x, y, Color.FromArgb(bit, bit, bit));
                }
            }

            return bitmap;
        }

        private Bitmap ReadASCIIBitmapImage_P2(BinaryReader file)
        {
            maxValue = GetNextValue(file);  // -- read the max pixel value

            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int bit = GetNextValue(file) * 255 / maxValue; // -- change pixel value scale to 0-255 
                    bitmap.SetPixel(x, y, Color.FromArgb(bit, bit, bit));
                }
            }

            return bitmap;
        }

        private Bitmap ReadASCIIBitmapImage_P3(BinaryReader file)
        {
            maxValue = GetNextValue(file);  // -- read the max pixel value

            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int r = GetNextValue(file) * 255 / maxValue; // -- change pixel value scale to 0-255 
                    int g = GetNextValue(file) * 255 / maxValue; // -- change pixel value scale to 0-255 
                    int b = GetNextValue(file) * 255 / maxValue; // -- change pixel value scale to 0-255 

                    bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return bitmap;
        }

        private Bitmap ReadBinaryBitmapImage_P4(BinaryReader file)
        {

            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int bit = file.ReadByte() == 0 ? 255 : 0;
                    bitmap.SetPixel(x, y, Color.FromArgb(bit, bit, bit));
                }
            }

            return bitmap;
        }

        private Bitmap ReadBinaryBitmapImage_P5(BinaryReader file)
        {
            maxValue = GetNextValue(file);  // -- read the max pixel value

            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int bit = file.ReadByte() * 255 / maxValue;
                    bitmap.SetPixel(x, y, Color.FromArgb(bit, bit, bit));
                }
            }

            return bitmap;
        }


        private Bitmap ReadBinaryBitmapImage_P6(BinaryReader file)
        {
            maxValue = GetNextValue(file);  // -- read the max pixel value

            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int r = file.ReadByte() * 255 / maxValue;
                    int g = file.ReadByte() * 255 / maxValue;
                    int b = file.ReadByte() * 255 / maxValue;

                    bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return bitmap;
        }

        private static int GetNextValue(BinaryReader file)
        {
            bool hasValue = true;
            string value = string.Empty;
            char c;
            bool comment = false;

            do
            {
                //c = file.ReadChar();
                c = Convert.ToChar(file.ReadByte());

                if (c == '#')
                    comment = true; // begin of the comment

                if (comment)
                {
                    if (c == '\n')
                        comment = false; // end of the comment
                    continue;
                }

                if (hasValue)
                {
                    if ((c == '\n' || c == ' ' || c == '\t') && value.Length != 0)
                        hasValue = false; // End of the single value

                    else if (c >= '0' && c <= '9')
                        value += c; // Add next char to value. Value is allways a number
                }

            } while (hasValue);

            return int.Parse(value);
        }

        

    }
}
