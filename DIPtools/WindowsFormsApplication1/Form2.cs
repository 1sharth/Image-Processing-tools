using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            Bitmap image = new Bitmap(Form1.image_paths[Form1.curr_imagepath], true);
            float[] hashRed = new float[256];                                //TODO: assumption is by default they get 0f value
            float[] hashGreen = new float[256];
            float[] hashBlue = new float[256];
            float maxPixels = -1.0f;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    hashRed[pixelColor.R] += 1;
                    maxPixels = Math.Max(maxPixels, hashRed[pixelColor.R]);
                    hashBlue[pixelColor.B] += 1;
                    maxPixels = Math.Max(maxPixels, hashBlue[pixelColor.B]);
                    hashGreen[pixelColor.G] += 1;
                    maxPixels = Math.Max(maxPixels, hashGreen[pixelColor.G]);
                }
            }
            float size = image.Width * image.Height;
            float valforNorm = size;
            for (int i = 0; i < 256; i++)
            {
                hashRed[i] /= valforNorm;
                hashBlue[i] /= valforNorm;
                hashGreen[i] /= valforNorm;
            }

            Series series1 = new Series();
            Series series2 = new Series();
            Series series3 = new Series();

            for (int i = 0; i < 256; i++)
            {
                series1.Points.Add(new DataPoint(i, hashRed[i]));
                series2.Points.Add(new DataPoint(i, hashGreen[i]));
                series3.Points.Add(new DataPoint(i, hashBlue[i]));
            }
            /*series1.ChartType = SeriesChartType.Line;
            series2.ChartType = SeriesChartType.Line;
            series3.ChartType = SeriesChartType.Line;*/
            series1.Color = Color.Red;
            series2.Color = Color.Green;
            series3.Color = Color.Blue;
            chart1.Series.Add(series1);
            chart1.Series.Add(series2);
            chart1.Series.Add(series3);

            chart2.Series.Clear();
            chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart2.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            Bitmap image2 = Form1.image_for_picturebox2;
            Array.Clear(hashRed, 0, hashRed.Length);                               //TODO: assumption is by default they get 0f value
            Array.Clear(hashBlue, 0, hashBlue.Length);
            Array.Clear(hashGreen, 0, hashGreen.Length);
            maxPixels = -1.0f;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image2.GetPixel(x, y);
                    hashRed[pixelColor.R] += 1;
                    maxPixels = Math.Max(maxPixels, hashRed[pixelColor.R]);
                    hashBlue[pixelColor.B] += 1;
                    maxPixels = Math.Max(maxPixels, hashBlue[pixelColor.B]);
                    hashGreen[pixelColor.G] += 1;
                    maxPixels = Math.Max(maxPixels, hashGreen[pixelColor.G]);
                }
            }
            size = image.Width * image.Height;
            valforNorm = size;
            for (int i = 0; i < 256; i++)
            {
                hashRed[i] /= valforNorm;
                hashBlue[i] /= valforNorm;
                hashGreen[i] /= valforNorm;
            }

            Series series_1 = new Series();
            Series series_2 = new Series();
            Series series_3 = new Series();

            for (int i = 0; i < 256; i++)
            {
                series_1.Points.Add(new DataPoint(i, hashRed[i]));
                series_2.Points.Add(new DataPoint(i, hashGreen[i]));
                series_3.Points.Add(new DataPoint(i, hashBlue[i]));
            }
            /*series_1.ChartType = SeriesChartType.Line;
            series_2.ChartType = SeriesChartType.Line;
            series_3.ChartType = SeriesChartType.Line;*/
            series_1.Color = Color.Red;
            series_2.Color = Color.Green;
            series_3.Color = Color.Blue;
            chart2.Series.Add(series_1);
            chart2.Series.Add(series_2);
            chart2.Series.Add(series_3);
        }

        private void chart1_wheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Delta < 0)
                {
                    chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                    chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                }

                if (e.Delta > 0)
                {
                    double xmin = chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                    double xmax = chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
                    double ymin = chart1.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
                    double ymax = chart1.ChartAreas[0].AxisY.ScaleView.ViewMaximum;

                    double posx1 = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - (xmax - xmin) / 1.2;
                    double posx2 = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + (xmax - xmin) / 1.2;
                    double posy1 = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) - (ymax - ymin) / 1.2;
                    double posy2 = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) + (ymax - ymin) / 1.2;

                    chart1.ChartAreas[0].AxisX.ScaleView.Zoom(posx1, posx2);
                    chart1.ChartAreas[0].AxisY.ScaleView.Zoom(posy1, posy2);
                }
            }
            catch { } 
        }
        private void chart2_wheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Delta < 0)
                {
                    chart2.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                    chart2.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                }

                if (e.Delta > 0)
                {
                    double xmin = chart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                    double xmax = chart2.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
                    double ymin = chart2.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
                    double ymax = chart2.ChartAreas[0].AxisY.ScaleView.ViewMaximum;

                    double posx1 = chart2.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - (xmax - xmin) / 1.2;
                    double posx2 = chart2.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + (xmax - xmin) / 1.2;
                    double posy1 = chart2.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) - (ymax - ymin) / 1.2;
                    double posy2 = chart2.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) + (ymax - ymin) / 1.2;

                    chart2.ChartAreas[0].AxisX.ScaleView.Zoom(posx1, posx2);
                    chart2.ChartAreas[0].AxisY.ScaleView.Zoom(posy1, posy2);
                }
            }
            catch { }
        }
    }
}
