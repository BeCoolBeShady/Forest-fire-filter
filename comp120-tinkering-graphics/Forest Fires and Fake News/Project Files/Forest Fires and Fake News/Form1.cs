using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forest_Fires_and_Fake_News
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap bmp;
        bool hasImagedChanged = false; //This variable stops the user from pressing the Fix button more then once
        public void Form1_Load(object sender, EventArgs e)
        {
        }

        // Takes the RGB value of the pixel and reverses it
        public void Invert()
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color p = bmp.GetPixel(x, y);
                    float R = p.R;
                    float G = p.G;
                    float B = p.B;

                    float temp;

                    //Swapping R to B and B to R
                    temp = R;
                    R = B;
                    B = temp;

                    bmp.SetPixel(x, y, Color.FromArgb(255, (int)R, (int)G, (int)B));
                     
                }
            }
        }

        //Reduces Orange colour from a picture
        public void ReduceOrange()
        {
            if (bmp != null) //If the player has searched for an image 
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        Color p = bmp.GetPixel(x, y);
                        float R = p.R;
                        float G = p.G;
                        float B = p.B;
                        Color red = Color.FromArgb(255, 255, 0, 0);

                        //Changing pixels which colour is close to red with tolerance of 220, just to be sure most pixels will be changed
                        if (Tolerance(p, red, 220))
                        {
                            R -= Luminance(p);
                            G += 10;
                            B += (Luminance(p) / 2);
                        }

                        //The IF's are to prevent the values to go over/under the max/min value of RGB 
                        if (G > 255)
                            G = 255;
                        if (B > 255)
                            B = 255;
                        if (R < 0)
                            R = 0;

                        bmp.SetPixel(x, y, Color.FromArgb(255, abs((int)R), abs((int)G), abs((int)B)));
                    }
                }
                MakeBrighter(1.2f);
                Invert();
            }
            else //if there is no image then display this text in label 2
            {
                label2.Text = "There is no Image! Please Search for an Image";
            }
        }
        
        //Makes the image brighter by the set float value
        private void MakeBrighter(float tmp)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color c1 = bmp.GetPixel(x, y);
                    float R = c1.R;
                    float G = c1.G;
                    float B = c1.B;
                    R *= tmp;
                    G *= tmp;
                    B *= tmp;

                    //IF's prevent the values to go over the max value of RGB
                    if (G > 255)
                        G = 255;
                    if (B > 255)
                        B = 255;
                    if (R > 255)
                        R = 255;

                    bmp.SetPixel(x, y, Color.FromArgb(255, abs((int)R), abs((int)G), abs((int)B)));
                }
            }
        }

        //Returns an absolute value
        private int abs(int x)
        {
            if (x > 0)
                return x;
            else
                return -x;
        }

        //Calculates the distance between two colours
        private float ColourDistance(Color color, Color color2)
        {

            float distance;

            float math = (float)Math.Pow(color.R - color2.R, 2) + (float)Math.Pow(color.G - color2.G, 2) + (float)Math.Pow(color.B - color2.B, 2);

            distance = (float)Math.Sqrt(math);

            return distance;
        }

        //Calculates the Luminance of a pixel
        private int Luminance(Color c1)
        {
            int luminance = (c1.R + c1.G + c1.B) / 3;

            return luminance;
        }

        //Checks if the Distance between two colours is bigger or smaller than the set tolerance value
        private bool Tolerance(Color c1, Color c2, int t) 
        {
            return ColourDistance(c1, c2) < t;
        }

 
        private void button1_Click(object sender, EventArgs e)
        {
            if (hasImagedChanged == false)
            {
                hasImagedChanged = true;
                //Starts the reduce orange function
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                ReduceOrange();
            }
            else
            {
                label2.Text = "DONT PRESS THE FIX IMAGE BUTTON MORE THEN ONCE!";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            String Input = textBox1.Text;
            try
            {
                // open file dialog   
                OpenFileDialog open = new OpenFileDialog();
                // image filters  
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    // display image in picture box  
                    pictureBox3.Image = new Bitmap(open.FileName);
                    // image file path  
                    textBox1.Text = open.FileName;
                    bmp = new Bitmap(open.FileName);
                }
                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                label2.Text = "";
                hasImagedChanged = false;
            }
            catch
            {
                label2.Text = "Failed to Find Image! Remeber to use double slashes e.g \\ and make sure the Image exists!";
            }

        }
    }
}
