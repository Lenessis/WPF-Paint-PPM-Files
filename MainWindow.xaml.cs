using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using zad1___paint.PPM;

namespace zad1___paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string path = "../../../Pictures";
        Point startPosition, endPosition;
        public int chosenTool = 0;
        // 0 => draw
        // 1 => rectangle
        // 2 => elipse
        // 3 => triangle
        // 4 => line
        // 5 => text
        // 6 => save

        public MainWindow()
        {
            InitializeComponent();
        }

        private void draw_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = 0;
        }

        private void rectangle_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = 1;
        }

        private void elipse_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = 2;
        }

        private void triangle_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = 3;
        }

        private void line_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = 4;
        }

        private void text_Click(object sender, RoutedEventArgs e)
        {
            chosenTool = 5;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = new DateTime();
            date = DateTime.Now;
            FileStream file = new FileStream(path + "/Picture_"+date.ToShortDateString()+"_"+date.Hour+date.Minute+date.Second+".jpg", FileMode.Create);
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)canvasBoard.ActualWidth, (int)canvasBoard.ActualHeight, 1 / 96, 1 / 96, PixelFormats.Pbgra32);
            bmp.Render(canvasBoard);
            BitmapEncoder encoder = new TiffBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(file);
            file.Close();
        }

        private object? figure;

        private void InitEventOnCanvas(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(canvasBoard);
            startPosition = e.GetPosition(canvasBoard);

            switch(chosenTool)
            {
                case 0:
                    //startPosition = e.GetPosition(canvasBoard);
                    Line tempPaint = CreateDrawing(startPosition, startPosition);
                    canvasBoard.Children.Add(tempPaint);
                    figure = tempPaint;
                    break;

                case 1:
                    Rectangle temp = new Rectangle
                    {
                        Width = 0,
                        Height = 0,
                        Fill = Brushes.Pink
                    };
                    temp.SetCurrentValue(Canvas.TopProperty, startPosition.Y);
                    temp.SetCurrentValue(Canvas.LeftProperty, startPosition.X);
                    canvasBoard.Children.Add(temp);
                    figure = temp;
                    break;

                case 2:
                    Ellipse tempEllipse = new Ellipse
                    {
                        Width = 0,
                        Height = 0,
                        Fill = Brushes.LightPink
                    };
                    tempEllipse.SetCurrentValue(Canvas.TopProperty, startPosition.Y);
                    tempEllipse.SetCurrentValue(Canvas.LeftProperty, startPosition.X);
                    canvasBoard.Children.Add(tempEllipse);
                    figure = tempEllipse;
                    break;

                case 3:
                    Polygon tempTriangle = new Polygon();
                    tempTriangle.Fill = Brushes.DeepPink;
                    PointCollection points = new PointCollection();
                    points.Add(new Point(0,0));
                    points.Add(new Point(0, 0));
                    points.Add(new Point(0, 0));
                    tempTriangle.Points = points;

                    tempTriangle.SetCurrentValue(Canvas.TopProperty, startPosition.Y);
                    tempTriangle.SetCurrentValue(Canvas.LeftProperty, startPosition.X);

                    canvasBoard.Children.Add(tempTriangle);
                    figure = tempTriangle;
                    break;

                case 4:
                    Line tempLine = new Line
                    {
                        Stroke = Brushes.HotPink,
                        StrokeThickness = 3
                    };
                    tempLine.X1 = startPosition.X;
                    tempLine.Y1 = startPosition.Y;
                    tempLine.X2 = startPosition.X;
                    tempLine.Y2 = startPosition.Y;
                    canvasBoard.Children.Add(tempLine);
                    figure = tempLine;
                    break;

                case 5:
                    TextBox tempText = new TextBox
                    {
                        Width = 0,
                        Height = 0,
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.Transparent
                    };

                    tempText.SetCurrentValue(Canvas.TopProperty, startPosition.Y);
                    tempText.SetCurrentValue(Canvas.LeftProperty, startPosition.X);
                    canvasBoard.Children.Add(tempText);
                    figure = tempText;
                    break;

                default: break;
            }
        }

        private void ProcessingEventOnCanvas(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (figure is null)
                    return;
                endPosition = e.GetPosition(canvasBoard);
                switch(chosenTool)
                {
                    case 0:
                        canvasBoard.Children.Add(CreateDrawing(startPosition, endPosition));
                        startPosition = e.GetPosition(canvasBoard);
                        break;

                    case 1:
                        CreatingRectangle((Rectangle)figure, startPosition, endPosition);
                        break;

                    case 2:
                        CreatingEllipse((Ellipse)figure, startPosition,endPosition);
                        break;

                    case 3:
                        CreatingTriangle((Polygon)figure, startPosition, endPosition);
                        break;

                    case 4:
                        CreatingLine((Line)figure, endPosition);
                        break;
                    case 5:
                        CreatingText((TextBox)figure, startPosition, endPosition);
                        break;
                    default:
                        break;
                }
            }
        }

        private void EndEventOnCanvas(object sender, MouseButtonEventArgs e)
        {
            endPosition = e.GetPosition(canvasBoard);
            if (figure is null)
                return;
            switch (chosenTool)
            {
                case 0:
                    canvasBoard.Children.Add(CreateDrawing(startPosition, endPosition));
                    startPosition = e.GetPosition(canvasBoard);
                    break;

                case 1:
                    figure = CreatingRectangle((Rectangle)figure, startPosition, endPosition);
                    break;

                case 2:
                    figure = CreatingEllipse((Ellipse)figure, startPosition, endPosition);
                    break;

                case 3:
                    figure = CreatingTriangle((Polygon)figure, startPosition, endPosition);
                    break;

                case 4:
                    figure = CreatingLine((Line)figure, endPosition);
                    break;

                case 5:
                    figure = CreatingText((TextBox)figure, startPosition, endPosition);
                    break;
                default:
                    break;
            }
            figure = null;
            Mouse.Capture(null);
        }

        static private Line CreateDrawing(Point start, Point end)
        {
            Line line = new Line
            {
                Stroke = Brushes.DeepPink,
                StrokeThickness = 2
            };

            line.X1 = start.X;
            line.Y1 = start.Y;
            line.X2 = end.X;
            line.Y2 = end.Y;

            return line;
        }

        static private Rectangle CreatingRectangle(Rectangle item, Point start, Point end)
        {
            item.Width = Math.Abs(end.X - start.X);
            item.Height = Math.Abs(end.Y - start.Y);
            item.SetValue(Canvas.LeftProperty, Math.Min(start.X, end.X));
            item.SetValue(Canvas.TopProperty, Math.Min(start.Y, end.Y));
            return item;
        }

        static private Ellipse CreatingEllipse (Ellipse item, Point start, Point end)
        {
            item.Width = Math.Abs(end.X - start.X);
            item.Height = Math.Abs(end.Y - start.Y);
            item.SetValue(Canvas.LeftProperty, Math.Min(start.X, end.X));
            item.SetValue(Canvas.TopProperty, Math.Min(start.Y, end.Y));
            return item;
        }

        static private Polygon CreatingTriangle (Polygon item, Point start, Point end)
        {

            item.Points[0] = new Point(Math.Min(0, end.X-start.X), end.Y-start.Y);
            item.Points[1] = new Point((end.X-start.X)/2, 0);
            item.Points[2] = new Point(Math.Max(0, end.X-start.X), end.Y-start.Y);

            item.SetValue(Canvas.LeftProperty, Math.Min(start.X, end.X));
            item.SetValue(Canvas.TopProperty, Math.Min(start.Y, end.Y));
            return item;
        }

        static private Line CreatingLine(Line item, Point end)
        {
            item.X2 = end.X;
            item.Y2 = end.Y;
            return item;
        }

        private void read_ppm_Click(object sender, RoutedEventArgs e)
        {
            ImagePPM diag = new ImagePPM();
            diag.Title = "Twój PPM";
            ReaderPPM bmp = new ReaderPPM();
            BitmapSource image = Bitmap2BitmapImage(bmp.LoadPPMFile());
            diag.ppmPicture.Source = image;
            ;
            if (diag.ShowDialog() == true)
            { 
            }
   
        }

        private void write_ppm_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = new DateTime();
            date = DateTime.Now;
            string filePath;// = path + "/Picture_" + date.ToShortDateString() + "_" + date.Hour + date.Minute + date.Second + ".ppm";
            System.Drawing.Bitmap bitmap = CanvasToBitmap(canvasBoard);
            WritePPM w = new WritePPM();
            for (int i = 1; i < 7; i++)
            {
                filePath = path + "/Picture_"+i+"_" + date.ToShortDateString() + "_" + date.Hour + date.Minute + date.Second + ".ppm";
                w.SavePPMFile(i, filePath, bitmap);
            }
            
        }

        static private TextBox CreatingText (TextBox item, Point start, Point end)
        {
            item.Width = Math.Abs(end.X - start.X);
            item.Height = Math.Abs(end.Y - start.Y);
            item.FontSize = (int)item.Height*0.75+1;
            item.Focus();
            //item.IsReadOnly = item.IsReadOnlyCaretVisible = true;
            return item;
        }

        private BitmapSource Bitmap2BitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapSource i = Imaging.CreateBitmapSourceFromHBitmap(
                           bitmap.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            return i;
        }

        private System.Drawing.Bitmap CanvasToBitmap(Canvas canvas)
        {
            int width = Convert.ToInt32(canvas.ActualWidth);
            int height = Convert.ToInt32(canvas.ActualHeight);
            canvas.Background = Brushes.White;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);

            var renderTargetBitmap = new RenderTargetBitmap(width, height, 1 / 96, 1 / 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(canvas);
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmap = new System.Drawing.Bitmap(stream);
                return new System.Drawing.Bitmap(bitmap);
            } 
        }
    }
}
