using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zad1___paint.PPM
{
    class WritePPM
    {
        string filePath = "";
        Bitmap bitmap;
        public void SavePPMFile(int format, string path, Bitmap bpm)
        {
            filePath = path;
            bitmap = bpm;

            switch(format)
            {
                case 1:
                    WriteASCIIFile_P1();
                    break;
                case 2:
                    WriteBinaryFile_P2();
                    break;
                case 3:
                    WriteBinaryFile_P3();
                    break;
                case 4:
                    WriteBinaryFile_P4();
                    break;
                case 5:
                    WriteBinaryFile_P5();
                    break;
                case 6:
                    WriteBinaryFile_P6();
                    break;
                default:
                    break;
            }
        }

        private void WriteASCIIFile_P1()
        {
            var writer = new StreamWriter(filePath);
            writer.WriteLine("P1");
            writer.WriteLine($"{bitmap.Width} {bitmap.Height}");
            writer.WriteLine("1");

            for (int x = 0; x < bitmap.Height; x++)
                for (int y = 0; y < bitmap.Width; y++)
                {
                    Color color = bitmap.GetPixel(y, x);
                    int bit = Convert.ToInt32(Math.Round(color.GetBrightness()));
                    writer.WriteLine($"{bit} ");

                }
            writer.Close();
        }

        private void WriteBinaryFile_P2()
        {
            var writer = new StreamWriter(filePath);
            writer.WriteLine("P2");
            writer.WriteLine($"{bitmap.Width} {bitmap.Height}");
            writer.WriteLine("255");

            for (int x = 0; x < bitmap.Height; x++)
                for (int y = 0; y < bitmap.Width; y++)
                {
                    Color color = bitmap.GetPixel(y, x);
                    int gray = Convert.ToInt32(color.GetBrightness() * 255);
                    writer.WriteLine($"{gray} ");
                }
            writer.Close();
        }

        private void WriteBinaryFile_P3()
        {
            var writer = new StreamWriter(filePath);
            writer.WriteLine("P3");
            writer.WriteLine($"{bitmap.Width} {bitmap.Height}");
            writer.WriteLine("255");

            for (int x = 0; x < bitmap.Height; x++)
                for (int y = 0; y < bitmap.Width; y++)
                {
                    Color color = bitmap.GetPixel(y, x);
                    writer.WriteLine($"{color.R} {color.G} {color.B} ");
                }
            writer.Close();
        }

        private void WriteBinaryFile_P4()
        {
            var writer = new StreamWriter(filePath);
            writer.WriteLine("P4");
            writer.WriteLine($"{bitmap.Width} {bitmap.Height}");
            writer.WriteLine("1");
            writer.Close();

            var writerB = new BinaryWriter(new FileStream(filePath, FileMode.Append));
            for (int x = 0; x < bitmap.Height; x++)
                for (int y = 0; y < bitmap.Width; y++)
                {
                    Color color = bitmap.GetPixel(y, x);
                    byte bit = Convert.ToByte(Math.Round(color.GetBrightness()));
                    writerB.Write(bit);
                }
            writerB.Close();
        }

        private void WriteBinaryFile_P5()
        {
            var writer = new StreamWriter(filePath);
            writer.WriteLine("P5");
            writer.WriteLine($"{bitmap.Width} {bitmap.Height}");
            writer.WriteLine("255");
            writer.Close();

            var writerB = new BinaryWriter(new FileStream(filePath, FileMode.Append));
            for (int x = 0; x < bitmap.Height; x++)
                for (int y = 0; y < bitmap.Width; y++)
                {
                    Color color = bitmap.GetPixel(y, x);
                    byte gray = Convert.ToByte(color.GetBrightness() * 255);
                    writerB.Write(gray);
                }
            writerB.Close();
        }

        private void WriteBinaryFile_P6()
        {
            var writer = new StreamWriter(filePath);
            writer.WriteLine("P6");
            writer.WriteLine($"{bitmap.Width} {bitmap.Height}");
            writer.WriteLine("255");
            writer.Close();

            var writerB = new BinaryWriter(new FileStream(filePath, FileMode.Append));
            for (int x = 0; x < bitmap.Height; x++)
                for (int y = 0; y < bitmap.Width; y++)
                {
                    Color color = bitmap.GetPixel(y, x);
                    writerB.Write(color.R);
                    writerB.Write(color.G);
                    writerB.Write(color.B);
                }
            writerB.Close();
        }
    }
}
