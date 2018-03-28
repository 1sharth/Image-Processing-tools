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
using System.Drawing.Imaging;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public static int num = 0;
        public static List<String> image_paths = new List<String>();
        public static int curr_imagepath = 0;
        public static Bitmap image_for_picturebox2;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
   
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
            String name=openFileDialog1.FileName;
            MessageBox.Show(name);
            String path=name.Substring(0,name.LastIndexOf('\\'));
            //MessageBox.Show(path);
            foreach (String i in System.IO.Directory.GetFiles(path, "*.png"))
                image_paths.Add(i);
            foreach (String i in System.IO.Directory.GetFiles(path, "*.jpg"))
                image_paths.Add(i);
            foreach (String i in System.IO.Directory.GetFiles(path, "*.jpeg"))
                image_paths.Add(i);
            curr_imagepath = image_paths.IndexOf(name);
            //MessageBox.Show(curr_imagepath.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (++curr_imagepath == image_paths.Count)
                curr_imagepath = 0;
            pictureBox1.ImageLocation = image_paths[curr_imagepath];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (--curr_imagepath == -1)
                curr_imagepath = image_paths.Count-1;
            pictureBox1.ImageLocation = image_paths[curr_imagepath];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            image_for_picturebox2 = new Bitmap(image_paths[curr_imagepath], true);
            switch (comboBox1.GetItemText(this.comboBox1.SelectedItem))
            {
                case "Grayscale": pictureBox2.Image = grayscale(image_for_picturebox2);
                break;

                case "Redify": pictureBox2.Image = colorify(image_for_picturebox2,"r");
                break;

                case "Bluify": pictureBox2.Image = colorify(image_for_picturebox2, "b");
                break;

                case "Greenify": pictureBox2.Image = colorify(image_for_picturebox2, "g");
                break;

                case "Equalify Histogram": pictureBox2.Image = equaliseHistogram(image_for_picturebox2);
                break;

                case "Negate": pictureBox2.Image = negate(image_for_picturebox2);
                break;

                case "Log Transform": pictureBox2.Image = logTransform(image_for_picturebox2);
                break;

                case "Edge Enhance":
                    double[,] filter = new double[3, 3]{
                    {0,1,0},{1,-4,1},{0,1,0}
                    };
                    pictureBox2.Image = setFilter(image_for_picturebox2,filter);
                break;

                case "Boost":
                    double k = trackBar1.Value;;
                    filter = new double[3, 3]{
                    {-1/9,-1/9,-1/9},{-1/9,k-(1/9),-1/9},{-1/9,-1/9,-1/9}
                    };
                    pictureBox2.Image = setFilter(image_for_picturebox2, filter);
                break;
                     
                case "Sharpen":
                    double[,] filter3 = new double[3, 3]{
                    {0,-1,0},{-1,5,-1},{0,-1,0}
                    };
                    pictureBox2.Image = setFilter(image_for_picturebox2, filter3);
                break;

                case "Blur":
                double[,] filter4 = new double[3, 3]{
                    {0.11111,0.11111,0.11111},{0.11111,0.11111,0.11111},{0.11111,0.11111,0.11111}
                    };
                    pictureBox2.Image = setFilter(image_for_picturebox2, filter4);
                break;

                case "Boost and Equalization":
                k = trackBar1.Value;;
                    filter = new double[3, 3]{
                    {-1/9,-1/9,-1/9},{-1/9,k-(1/9),-1/9},{-1/9,-1/9,-1/9}
                    };
                pictureBox2.Image = setFilter(image_for_picturebox2, filter);
                pictureBox2.Image = equaliseHistogram(image_for_picturebox2);
                break;
            }
        }
        
        
  
        private void histogramComparisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        public double[,] convolve(double[,] mat1,double[,] mat2)
        {
            double[,] newmat = new double[mat1.GetLength(0) - mat2.GetLength(0) + 1, mat1.GetLength(1) - mat2.GetLength(1) + 1];
            for (int i = 0; i < mat1.GetLength(0) - mat2.GetLength(0) + 1; i++)
            {
                for (int j = 0; j < mat1.GetLength(1) - mat2.GetLength(1) + 1; j++)
                {
                    double sum = 0;
                    for (int k1 = 0; k1 < mat2.GetLength(0); k1++)
                    {
                        for (int k2 = 0; k2 < mat2.GetLength(1); k2++)
                        {
                            sum += mat2[k1, k2] * mat1[i + k1, j + k2];
                        }
                    }
                    if (sum > 255) sum = 255;
                    if (sum < 0) sum = 0;
                    newmat[i, j] = sum;
                }
            }
            return newmat;
        }

        public Bitmap setFilter(Bitmap image,double[,] filter)
        {
            double[,] imagemat=new double[image.Height,image.Width];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int rgb = (pixelColor.R + pixelColor.B + pixelColor.G) / 3;
                    imagemat[y, x] = rgb;

                }
                
            }
            
            double[,] newimagemat = convolve(imagemat, filter);
            //MessageBox.Show("newmat: "+newimagemat.GetLength(0).ToString() + " " + newimagemat.GetLength(1).ToString()+" image: "+image.Height.ToString() + " " + image.Width.ToString());
            for (int y = 0; y < newimagemat.GetLength(0); y++)
            {
                for (int x = 0; x < newimagemat.GetLength(1); x++)
                {
                    int rgb = (int)newimagemat[y, x];
                    image.SetPixel(x, y,Color.FromArgb(rgb,rgb,rgb));
                }
            }
            return image;
        }
        public Bitmap logTransform(Bitmap image)
        {
            double c=trackBar1.Value;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    double rgb = (pixelColor.R + pixelColor.B + pixelColor.G) / 3;
                    int rgb2 = (int)(c * Math.Log(rgb+1));
                    image.SetPixel(x, y, Color.FromArgb(rgb2,rgb2,rgb2));

                }

            }
            return image;
        }
        public Bitmap colorify(Bitmap image,string color)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int rgb;
                    if (color == "r")
                    {
                        rgb = pixelColor.R;
                        image.SetPixel(x, y, Color.FromArgb(rgb, 0, 0));
                    }
                    else if (color == "b")
                    {
                        rgb = pixelColor.B;
                        image.SetPixel(x, y, Color.FromArgb(0, 0, rgb));
                    }
                        
                    else
                    {
                        rgb = pixelColor.G;
                        image.SetPixel(x, y, Color.FromArgb(0, rgb, 0));
                    }
                        
                    
                }
            }
            return image;
        }

        public Bitmap grayscale(Bitmap image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int rgb= (pixelColor.R + pixelColor.B + pixelColor.G) / 3;
                    image_for_picturebox2.SetPixel(x,y,Color.FromArgb(rgb,rgb,rgb));
                }
            }
            return image_for_picturebox2;
        }

        public Bitmap negate(Bitmap image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int rgb = (pixelColor.R + pixelColor.B + pixelColor.G) / 3;
                    rgb = 255 - rgb;
                    image.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            }
            return image;
        }

        public Bitmap equaliseHistogram(Bitmap image)
        {
            double[] grayhash = new double[256];
            double[] cummulative_p = new double[256];
            double[,] levelmap = new double[image.Height, image.Width];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    double rgb = (pixelColor.R + pixelColor.B + pixelColor.G) / 3;
                    grayhash[(int)rgb] += 1.0;
              
                    //image_for_picturebox2.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            }
            double size=image.Width*image.Height;
            grayhash[0] /= size;
            cummulative_p[0] = grayhash[0];
            for (int i = 1; i < 256; i++)
            {
                grayhash[i] /= size;
                cummulative_p[i] = cummulative_p[i - 1] + grayhash[i];
            }
            for (int i = 1; i < 256; i++)
            {
                cummulative_p[i]*=255;
                cummulative_p[i] = Math.Round(cummulative_p[i]);
            }
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int rgb = (pixelColor.R + pixelColor.B + pixelColor.G) / 3;
                    rgb=(int)cummulative_p[rgb];
                    image_for_picturebox2.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            }
            return image_for_picturebox2;
        }

        private void compressToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        
      
    }
}
