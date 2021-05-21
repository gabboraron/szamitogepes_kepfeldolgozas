using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Drawing.Imaging;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace PS
{
    public partial class Form1 : Form
    {
        Boolean imageLoaded = false;

        long[] allHistogram = new long[256];
        long[] rHistogram = new long[256];
        long[] gHistogram = new long[256];
        long[] bHistogram = new long[256];

        long maxValueFullHist = 0;
        long maxValueRHist = 0;
        long maxValueGHist = 0;
        long maxValueBHist = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void fToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void calcHistogram()
        {
            ProgressBar pBar = new ProgressBar();
            pBar.Minimum = 0;
            int max = pictureBox.Image.Height * pictureBox.Image.Width;
            pBar.Maximum = max;
            pBar.Value = 0;
            logBox.AppendText("0 -> " + max + "\n");

            ////////////////////////////////////////

            logBox.AppendText("Hisztogram számítása...\n");

            // Stopwatch stopwatch = new Stopwatch();

            maxValueFullHist = 0;
            maxValueRHist = 0;
            maxValueGHist = 0;
            maxValueBHist = 0;

            // stopwatch.Start();
            System.Drawing.Bitmap picture = new Bitmap(pictureBox.Image);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            /////////////////
            Bitmap image = new Bitmap(pictureBox.Image);
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat); //lefoglalunk egy kép méretével megegyező részt a képen bitmap formátumra konvertálva írásra és olvasására, és bekérjük a csatornakiosztás módját 
            int height = bitmapData.Height;
            int bPP = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8; //pixelenkénti bájtok száma, az iteráláshoz
            int width = bitmapData.Width * bPP;

            unsafe //mert a c# nem támogatja a pointereket bitmapen
            {
                byte* firstPixel = (byte*)bitmapData.Scan0; //a lefoglalt terület első pixele
                Parallel.For(0, height, j =>
                {
                    byte* line = firstPixel + (j * bitmapData.Stride); //hogy soronként menjünk végig az leső pixeltől
                        for (int i = 0; i < width; i = i + bPP)//egy pixel hátom elem a három csatorna a sorban, BGR sorrendben a csatornák
                        {

                        long Temp = 0;
                        Temp += line[i + 2]; //R
                            Temp += line[i + 1]; //G
                            Temp += line[i];     //B

                            Temp = (int)Temp / 3;
                        allHistogram[Temp]++;
                        rHistogram[line[i + 2]]++;
                        gHistogram[line[i + 1]]++;
                        bHistogram[line[i]]++;

                        if (maxValueFullHist < allHistogram[Temp]) { maxValueFullHist = allHistogram[Temp]; }
                        if (maxValueRHist < rHistogram[line[i + 2]]) { maxValueRHist = rHistogram[line[i + 2]]; }
                        if (maxValueGHist < gHistogram[line[i + 1]]) { maxValueGHist = gHistogram[line[i + 1]]; }
                        if (maxValueBHist < bHistogram[line[i]]) { maxValueBHist = bHistogram[line[i]]; }
                    }
                });
                image.UnlockBits(bitmapData);
            }

            pictureBox.Image = image;

            //for (int i = 0; i < picture.Height; i++)
            //    for (int j = 0; j < picture.Width; j++)
            //    {
            //        System.Drawing.Color c = picture.GetPixel(j,i);
            //        long Temp = 0;
            //        Temp += c.R;
            //        Temp += c.G;
            //        Temp += c.B;

            //        Temp = (int)Temp / 3;
            //        allHistogram[Temp]++;
            //        rHistogram[c.R]++;
            //        gHistogram[c.G]++;
            //        bHistogram[c.B]++;

            //        if (maxValueFullHist < allHistogram[Temp]) {maxValueFullHist = allHistogram[Temp]; }
            //        if (maxValueRHist < rHistogram[c.R]) { maxValueRHist = rHistogram[c.R]; }
            //        if (maxValueGHist < gHistogram[c.G]) { maxValueGHist = gHistogram[c.G]; }
            //        if (maxValueBHist < bHistogram[c.B]) { maxValueBHist = bHistogram[c.B]; }

            //        pBar.Value = pBar.Value+1;
            //        //logBox.AppendText("\n" + Temp + " -> " + allHistogram[Temp] + "\n");
            //    }

            stopwatch.Stop();
            logBox.AppendText("\n Maximum érték: " + maxValueFullHist + "\n");
            logBox.AppendText("\n Maximum vörös érték: " + maxValueRHist + "\n");
            logBox.AppendText("\n Maximum zöld érték: " + maxValueGHist + "\n");
            logBox.AppendText("\n Maximum kék érték: " + maxValueBHist + "\n");
            logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed + "\n"); // 00:00:00.0689554 párhuzamosítással; 00:00:00.9498320 párhuzamosítás nélkül
            logBox.AppendText("\n \n \n");
            histogram_full.ChartAreas[0].AxisY.Maximum = maxValueFullHist;
            histogram_full.Series[0].Points.DataBindY(allHistogram);
            histogram_R.ChartAreas[0].AxisY.Maximum = maxValueRHist;
            histogram_R.Series[0].Points.DataBindY(rHistogram);
            histogram_G.ChartAreas[0].AxisY.Maximum = maxValueGHist;
            histogram_G.Series[0].Points.DataBindY(gHistogram);
            histogram_B.ChartAreas[0].AxisY.Maximum = maxValueBHist;
            histogram_B.Series[0].Points.DataBindY(bHistogram);
        }

        private void képBetöltéseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png;)|*.jpg; *.jpeg; *.png;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                filenameTextbox.Text = open.FileName;
                pictureBox.Image = new Bitmap(open.FileName);

                calcHistogram();
                imageLoaded = true;
            }
        }

        private void képMentéseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bitmap = new Bitmap(pictureBox.Image);
                    bitmap.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
                }
            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string text = "Nincs kép betöltve!\nTöltsön be egy képet előbb: 'Fájl műveletek' > 'Kép betöltése' menüpontot használva!";
                string caption = "HIBA";
                MessageBox.Show(text, caption);
            }
        }

        private void negálásToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                logBox.AppendText("\n Kép negálása... \n");

                //Bitmap image = new Bitmap(pictureBox.Image);
                ////párhuzamosítás nélkül
                //for (int i = 0; i < image.Height; i++)
                //    for (int j = 0; j < image.Width; j++)
                //    {
                //        System.Drawing.Color c = image.GetPixel(j, i);
                //        c = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
                //        image.SetPixel(j, i, c);
                //    }

                //párhuzmaosítással
                Bitmap image = new Bitmap(pictureBox.Image);
                BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat); //lefoglalunk egy kép méretével megegyező részt a képen bitmap formátumra konvertálva írásra és olvasására, és bekérjük a csatornakiosztás módját 
                int height = bitmapData.Height;
                int bPP = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8; //pixelenkénti bájtok száma, az iteráláshoz
                int width = bitmapData.Width * bPP;

                unsafe //mert a c# nem támogatja a pointereket bitmapen
                {
                    byte* firstPixel = (byte*)bitmapData.Scan0; //a lefoglalt terület első pixele
                    Parallel.For(0, height, j =>
                    {
                        byte* line = firstPixel + (j * bitmapData.Stride); //hogy soronként menjünk végig az leső pixeltől
                        for (int i = 0; i < width; i = i + bPP)//egy pixel hátom elem a három csatorna a sorban, BGR sorrendben a csatornák
                        {
                            int oldB = 255 - line[i];
                            int oldG = 255 - line[i + 1];
                            int oldR = 255 - line[i + 2];

                            line[i] = (byte)oldB;
                            line[i + 1] = (byte)oldG;
                            line[i + 2] = (byte)oldR;
                        }
                    });
                    image.UnlockBits(bitmapData);
                }

                pictureBox.Image = image;
                stopwatch.Stop();
                logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed + "\n"); // 00:00:02.0035990 párhuzamosítás nélkül, párhuzmaosítással: 00:00:00.0361349
                logBox.AppendText("\n \n \n");
                calcHistogram();
            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string message = "Nincs kép betöltve!\nTöltsön be egy képet előbb egy képet: 'Fájlt műveletek' > 'Kép betöltése' menüpontot használva!";
                string title = "HIBA";
                MessageBox.Show(message, title);
            }
        }

        private void egyGammaBemenettelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                double gamma = 1.5;
                string getGamma = Interaction.InputBox("Adja meg a gamma értéket\n\n1    - eredeti\n0-1 - sötétebb\n1+   - világosabb", "Gamma beállítás", "1.5", 100, 100);
                Boolean gotValue = false;
                try
                {
                    gamma = Convert.ToDouble(getGamma);
                    gotValue = true;
                }
                catch (Exception)
                {
                    logBox.AppendText("\n Kilépés gamma transzformáció menüből.\n");
                }
                if (gotValue)
                {
                    logBox.AppendText("\n Kép gamma transzformálása " + gamma + " értékkel... \n");

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    /////////////////
                    Bitmap image = new Bitmap(pictureBox.Image);
                    ////    //párhuzamosítás nélkül
                    //int c = 1;
                    //int width = image.Width;
                    //int height = image.Height;
                    //BitmapData srcData = image.LockBits(new Rectangle(0, 0, width, height),
                    //    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    //int bytes = srcData.Stride * srcData.Height;
                    //byte[] buffer = new byte[bytes];
                    //byte[] result = new byte[bytes];
                    //Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
                    //image.UnlockBits(srcData);
                    //int current = 0;
                    //int cChannels = 3;
                    //for (int y = 0; y < height; y++)
                    //{
                    //    for (int x = 0; x < width; x++)
                    //    {
                    //        current = y * srcData.Stride + x * 4;
                    //        for (int i = 0; i < cChannels; i++)
                    //        {
                    //            double range = (double)buffer[current + i] / 255;
                    //            double correction = c * Math.Pow(range, gamma);
                    //            result[current + i] = (byte)(correction * 255);
                    //        }
                    //        result[current + 3] = 255;
                    //    }
                    //}
                    //Bitmap resImg = new Bitmap(width, height);
                    //BitmapData resData = resImg.LockBits(new Rectangle(0, 0, width, height),
                    //    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    //Marshal.Copy(result, 0, resData.Scan0, bytes);
                    //resImg.UnlockBits(resData);
                    //pictureBox.Image = resImg;

                    ////párhuzamosítással 
                    BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat); //lefoglalunk egy kép méretével megegyező részt a képen bitmap formátumra konvertálva írásra és olvasására, és bekérjük a csatornakiosztás módját 
                    int height = bitmapData.Height;
                    int bPP = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8; //pixelenkénti bájtok száma, az iteráláshoz
                    int width = bitmapData.Width * bPP;
                    int c = 1;

                    unsafe //mert a c# nem támogatja a pointereket bitmapen
                    {
                        byte* firstPixel = (byte*)bitmapData.Scan0; //a lefoglalt terület első pixele
                        Parallel.For(0, height, j =>
                        {
                            byte* line = firstPixel + (j * bitmapData.Stride); //hogy soronként menjünk végig az leső pixeltől
                            for (int i = 0; i < width; i = i + bPP)//egy pixel három elem a három csatorna a sorban, BGR sorrendben a csatornák
                            {
                                int oldB = Convert.ToInt32(255 * c * Math.Pow(line[i] / 255.0, gamma));
                                int oldG = Convert.ToInt32(255 * c * Math.Pow(line[i + 1] / 255.0, gamma));
                                int oldR = Convert.ToInt32(255 * c * Math.Pow(line[i + 2] / 255.0, gamma));

                                line[i] = (byte)oldB;
                                line[i + 1] = (byte)oldG;
                                line[i + 2] = (byte)oldR;
                            }
                        });
                        image.UnlockBits(bitmapData);
                    }

                    pictureBox.Image = image;

                    stopwatch.Stop();
                    logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed + "\n");//párhuzamosítás nélkül alap program 1.5 értékkel, előzetes módosítás nélkül: 00:00:00.2392818; párhuzamosítással ugyanolyan értékekkel:  00:00:00.1023629

                    logBox.AppendText("\n \n \n");
                    calcHistogram();
                }
            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string message = "Nincs kép betöltve!\nTöltsön be egy képet előbb egy képet: 'Fájlt műveletek' > 'Kép betöltése' menüpontot használva!";
                string title = "HIBA";
                MessageBox.Show(message, title);
            }
        }

        private void logaritmikusTranszformációToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                logBox.AppendText("\n Kép logaritmikus transzformálása... \n");
                double maxR = 255.0 / Math.Log10(1 + maxValueRHist);
                double maxG = 255.0 / Math.Log10(1 + maxValueGHist);
                double maxB = 255.0 / Math.Log10(1 + maxValueBHist);
                logBox.AppendText("\n Kép logaritmikus transzformálása: R - " + maxR + " értékkel... \n");
                logBox.AppendText("\n Kép logaritmikus transzformálása: G - " + maxG + " értékkel... \n");
                logBox.AppendText("\n Kép logaritmikus transzformálása: B - " + maxB + " értékkel... \n");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Bitmap bitmap = new Bitmap(pictureBox.Image);

                //párhuzmaosítás néllkül
                //int width = bitmap.Width;
                //int height = bitmap.Height;

                //for (int i = 0; i < height; i++)
                //{
                //    for (int j = 0; j < width; j++)
                //    {
                //        double R, G, B;
                //        lock (bitmap)
                //        {
                //            R = maxR * Math.Log10(1 + bitmap.GetPixel(j, i).R);
                //            G = maxG * Math.Log10(1 + bitmap.GetPixel(j, i).G);
                //            B = maxB * Math.Log10(1 + bitmap.GetPixel(j, i).B);
                //        }
                //        bitmap.SetPixel(j, i, Color.FromArgb((int) R, (int) G, (int) B));
                //    }
                //}

                //párhuzamosítással
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                int toExclusive = bitmapData.Height;
                int bPP = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                int width = bitmapData.Width * bPP;
                int num = 1;

                unsafe
                {
                    byte* firstPixel = (byte*)(void*)bitmapData.Scan0;
                    Parallel.For(0, toExclusive, delegate (int j)
                    {
                        byte* ptr = firstPixel + j * bitmapData.Stride;
                        for (int i = 0; i < width; i += bPP)
                        {
                            double num2 = maxB * Math.Log10(1 + ptr[i]);
                            double num3 = maxG * Math.Log10(1 + ptr[i + 1]);
                            double num4 = maxR * Math.Log10(1 + ptr[i + 2]);
                            ptr[i] = (byte)num2;
                            ptr[i + 1] = (byte)num3;
                            ptr[i + 2] = (byte)num4;
                        }
                    });
                }
                bitmap.UnlockBits(bitmapData);
                pictureBox.Image = bitmap;

                stopwatch.Stop();
                logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed + "\n");//párhuzamosítás nélkül: 00:00:02.5235125;  párhuzamosítással:  00:00:00.0433308

                logBox.AppendText("\n \n \n");
                calcHistogram();
            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string message = "Nincs kép betöltve!\nTöltsön be egy képet előbb egy képet: 'Fájlt műveletek' > 'Kép betöltése' menüpontot használva!";
                string title = "HIBA";
                MessageBox.Show(message, title);
            }
        }

        private void szürkítésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                logBox.AppendText("\n Kép szürkítése... \n");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                /////////////////
                Bitmap image = new Bitmap(pictureBox.Image);
                //párhuzamosítás nélkül
                //for (int i = 0; i < image.Height; i++)
                //    for (int j = 0; j < image.Width; j++)
                //    {
                //        System.Drawing.Color c = image.GetPixel(j, i);
                //        int grey = (c.R + c.G + c.B) / 3;
                //        c = Color.FromArgb(grey, grey, grey);
                //        image.SetPixel(j, i, c);
                //    }

                //párhuzmaosítással
                BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat); //lefoglalunk egy kép méretével megegyező részt a képen bitmap formátumra konvertálva írásra és olvasására, és bekérjük a csatornakiosztás módját 
                int height = bitmapData.Height;
                int bPP = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8; //pixelenkénti bájtok száma, az iteráláshoz
                int width = bitmapData.Width * bPP;

                unsafe //mert a c# nem támogatja a pointereket bitmapen
                {
                    byte* firstPixel = (byte*)bitmapData.Scan0; //a lefoglalt terület első pixele
                    Parallel.For(0, height, j =>
                    {
                        byte* line = firstPixel + (j * bitmapData.Stride); //hogy soronként menjünk végig az leső pixeltől
                        for (int i = 0; i < width; i = i + bPP)//egy pixel három elem a három csatorna a sorban, BGR sorrendben a csatornák
                        {
                            int grey = Convert.ToInt32(line[i + 1] * 0.587 + line[i] * 0.114 + line[i + 2] * 0.299); //a másik szűrkítéshez használt képletet használom, csak mert miért ne
                            int oldB = grey;
                            int oldG = grey;
                            int oldR = grey;

                            line[i] = (byte)oldB;
                            line[i + 1] = (byte)oldG;
                            line[i + 2] = (byte)oldR;
                        }
                    });
                    image.UnlockBits(bitmapData);
                }

                pictureBox.Image = image;

                stopwatch.Stop();
                logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed + "\n");//párhuzamosítás nélkül 00:00:02.0212269, párhuzamosítással:  00:00:00.0247860

                logBox.AppendText("\n \n \n");
                calcHistogram();

            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string message = "Nincs kép betöltve!\nTöltsön be egy képet előbb egy képet: 'Fájlt műveletek' > 'Kép betöltése' menüpontot használva!";
                string title = "HIBA";
                MessageBox.Show(message, title);
            }
        }

        private void hisztogramKiegyenlítésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                logBox.AppendText("\n Hisztogram kiegyenlítése... \n");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                /////////////////
                Bitmap image = new Bitmap(pictureBox.Image);
                ////párhuzamosítás nélkül
                //Bitmap img = new Bitmap(pictureBox.Image);
                //int width = img.Width;
                //int height = img.Height;
                //BitmapData bitmapData = img.LockBits(new Rectangle(0, 0, width, height),
                //    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                //int bytes = bitmapData.Stride * bitmapData.Height;
                //byte[] buffer = new byte[bytes];
                //byte[] result = new byte[bytes];
                //Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);
                //img.UnlockBits(bitmapData);
                //int current = 0;
                //double[] pn = new double[256];
                //for (int p = 0; p < bytes; p += 4)
                //{
                //    pn[buffer[p]]++;
                //}
                //for (int prob = 0; prob < pn.Length; prob++)
                //{
                //    pn[prob] /= (width * height);
                //}
                //for (int y = 0; y < height; y++)
                //{
                //    for (int x = 0; x < width; x++)
                //    {
                //        current = y * bitmapData.Stride + x * 4;
                //        double sum = 0;
                //        for (int i = 0; i < buffer[current]; i++)
                //        {
                //            sum += pn[i];
                //        }
                //        for (int c = 0; c < 3; c++)
                //        {
                //            result[current + c] = (byte)Math.Floor(255 * sum);
                //        }
                //        result[current + 3] = 255;
                //    }
                //}
                //Bitmap res = new Bitmap(width, height);
                //BitmapData rd = res.LockBits(new Rectangle(0, 0, width, height),
                //    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                //Marshal.Copy(result, 0, rd.Scan0, bytes);
                //res.UnlockBits(rd);
                //pictureBox.Image = res;

                ////parhuzamosítással
                BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat); //lefoglalunk egy kép méretével megegyező részt a képen bitmap formátumra konvertálva írásra és olvasására, és bekérjük a csatornakiosztás módját 
                int height = bitmapData.Height;
                int bPP = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8; //pixelenkénti bájtok száma, az iteráláshoz
                int width = bitmapData.Width * bPP;

                unsafe //mert a c# nem támogatja a pointereket bitmapen
                {
                    byte* firstPixel = (byte*)bitmapData.Scan0; //a lefoglalt terület első pixele
                    allHistogram = new long[256];
                    Parallel.For(0, height, j =>
                    {
                        byte* line = firstPixel + (j * bitmapData.Stride); //hogy soronként menjünk végig az leső pixeltől
                        for (int i = 0; i < width; i = i + bPP)//egy pixel három elem a három csatorna a sorban, BGR sorrendben a csatornák
                        {
                            //greying
                            int grey = Convert.ToInt32(line[i + 1] * 0.587 + line[i] * 0.114 + line[i + 2] * 0.299); //a másik szűrkítéshez használt képletet használom, csak mert miért ne
                            int oldB = grey;
                            int oldG = grey;
                            int oldR = grey;

                            line[i] = (byte)oldB;
                            line[i + 1] = (byte)oldG;
                            line[i + 2] = (byte)oldR;

                            //histogram calc
                            long Temp = 0;
                            Temp += line[i + 2]; //R
                            Temp += line[i + 1]; //G
                            Temp += line[i];     //B
                            Temp = (int)Temp / 3;
                            allHistogram[Temp]++;
                        }
                    });

                    //int L = 256; L = 256 - 1;
                    //long count = 0;
                    int number_of_pixels = width * height;
                    //double cdf;

                    long[] aryTransform = new long[256];
                    for (int i = 0; i < 255; i++)
                    {
                        long sum = 0;
                        for (int j = 0; j < i; j++)
                        {
                            sum += allHistogram[j];
                        }
                        aryTransform[i] = 256 * sum / number_of_pixels;
                    }

                    Parallel.For(0, height, j =>
                    {
                        byte* line = firstPixel + (j * bitmapData.Stride); //hogy soronként menjünk végig az első pixeltől
                        for (int i = 0; i < width; i = i + bPP)//egy pixel három elem a három csatorna a sorban, BGR sorrendben a csatornák
                        {
                            line[i] = (byte)aryTransform[line[i]];
                            line[i + 1] = (byte)aryTransform[line[i + 1]];
                            line[i + 2] = (byte)aryTransform[line[i + 2]];
                        }
                    });
                    image.UnlockBits(bitmapData);
                }
                pictureBox.Image = image;

                stopwatch.Stop();
                logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed + "\n");//párhuzamosítás nélkül 00:00:02.0212269, párhuzamosítással: 00:00:00.0384588

                logBox.AppendText("\n \n \n");
                calcHistogram();

            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string message = "Nincs kép betöltve!\nTöltsön be egy képet előbb egy képet: 'Fájlt műveletek' > 'Kép betöltése' menüpontot használva!";
                string title = "HIBA";
                MessageBox.Show(message, title);
            }
        }

        private void gaussSzűrőToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                logBox.AppendText("\n Gauss szűrő alkalmazása... \n");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ////párhuzamosítás nélkül
                //Bitmap bitmap = new Bitmap(pictureBox.Image);
                //int width = bitmap.Width;
                //int height = bitmap.Height;
                //Bitmap bitmap2 = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
                //for (int i = 0; i < width; i++)
                //{
                //    for (int j = 0; j < height; j++)
                //    {
                //        if (i > 1 && j > 1 && i + 1 != width && j + 1 != height)
                //        {
                //            int newRValue = bitmap.GetPixel(i, j + 1).R * 2+
                //                            bitmap.GetPixel(i + 1, j + 1).R +
                //                            bitmap.GetPixel(i - 1, j).R * 2 +
                //                            bitmap.GetPixel(i, j).R * 4 +
                //                            bitmap.GetPixel(i + 1, j).R * 2 +
                //                            bitmap.GetPixel(i - 1, j - 1).R +
                //                            bitmap.GetPixel(i, j - 1).R *2+
                //                            bitmap.GetPixel(i + 1, j - 1).R;
                //            newRValue = Convert.ToInt32(newRValue / 16);

                //            int newGValue = bitmap.GetPixel(i, j + 1).G * 2 +
                //                            bitmap.GetPixel(i + 1, j + 1).G +
                //                            bitmap.GetPixel(i - 1, j).G * 2 +
                //                            bitmap.GetPixel(i, j).G * 4 +
                //                            bitmap.GetPixel(i + 1, j).G * 2 +
                //                            bitmap.GetPixel(i - 1, j - 1).G +
                //                            bitmap.GetPixel(i, j - 1).G * 2 +
                //                            bitmap.GetPixel(i + 1, j - 1).G;
                //            newGValue = Convert.ToInt32(newGValue / 16);

                //            int newBValue = bitmap.GetPixel(i, j + 1).B * 2 +
                //                            bitmap.GetPixel(i + 1, j + 1).B +
                //                            bitmap.GetPixel(i - 1, j).B * 2 +
                //                            bitmap.GetPixel(i, j).B * 4 +
                //                            bitmap.GetPixel(i + 1, j).B * 2 +
                //                            bitmap.GetPixel(i - 1, j - 1).B +
                //                            bitmap.GetPixel(i, j - 1).B * 2 +
                //                            bitmap.GetPixel(i + 1, j - 1).B;
                //            newBValue = Convert.ToInt32(newBValue / 16);

                //            bitmap2.SetPixel(i, j, Color.FromArgb(newRValue, newGValue, newBValue));
                //        }
                //    }
                //}
                //pictureBox.Image = bitmap2;

                //párhuzamosítással
                //Bitmap image = new Bitmap(pictureBox.Image);
                Bitmap image = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height, PixelFormat.Format24bppRgb);
                using (Graphics gr = Graphics.FromImage(image)) gr.DrawImage(pictureBox.Image, new Rectangle(0, 0, image.Width, image.Height));
                BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat); //lefoglalunk egy kép méretével megegyező részt a képen bitmap formátumra konvertálva írásra és olvasására, és bekérjük a csatornakiosztás módját 
                int height = bitmapData.Height;
                int bPP = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8; //pixelenkénti bájtok száma, az iteráláshoz
                int width = bitmapData.Width * bPP;

                Bitmap bitmap2 = image.Clone(new Rectangle(0, 0, image.Width, image.Height), image.PixelFormat);
                unsafe //mert a c# nem támogatja a pointereket bitmapen
                {
                    byte* firstPixel = (byte*)bitmapData.Scan0; //a lefoglalt terület első pixele
                    Parallel.For(1, height - 1, j =>
                    {
                        //hogy soronként menjünk végig az első pixeltől
                        byte* line1 = firstPixel + ((j - 1) * bitmapData.Stride); //első sor
                        byte* line2 = firstPixel + (j * bitmapData.Stride); //második sor
                        byte* line3 = firstPixel + ((j + 1) * bitmapData.Stride); //hamradik sor
                        int idx = 0;

                        for (int i = 1; i < width - 1; i = i + bPP)//egy pixel hátom elem a három csatorna a sorban, BGR sorrendben a csatornák
                        {
                            int oldB = line1[i - 1] + line1[i + 2] * 2 + line1[i + 5] +
                                       line2[i - 1] * 2 + line2[i + 2] * 4 + line2[i + 5] * 2 +
                                       line3[i - 1] + line3[i + 2] * 2 + line3[i + 5];

                            int oldG = line1[i] + line1[i + 3] * 2 + line1[i + 6] +
                                       line2[i] * 2 + line2[i + 3] * 4 + line2[i + 6] * 2 +
                                       line3[i] + line3[i + 3] * 2 + line3[i + 6];

                            int oldR = line1[i + 1] + line1[i + 4] * 2 + line1[i + 7] +
                                       line2[i + 1] * 2 + line2[i + 4] * 4 + line2[i + 7] * 2 +
                                       line3[i + 1] + line3[i + 4] * 2 + line3[i + 7];

                            oldB = oldB / 16;
                            oldG = oldG / 16;
                            oldR = oldR / 16;

                            lock (bitmap2)
                            {
                                bitmap2.SetPixel(idx, j, Color.FromArgb(oldR, oldG, oldB));
                            }
                            idx++;

                            //line[i] = (byte)oldB;
                            //line[i + 1] = (byte)oldG;
                            //line[i + 2] = (byte)oldR;
                        }
                    });
                    image.UnlockBits(bitmapData);
                }

                pictureBox.Image = bitmap2;

                stopwatch.Stop();
                logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed.ToString() + "\n"); //párhuzamosítás nélkül: 00:00:26.6388920; párhuzamosítással:  00:00:02.0942794
                logBox.AppendText("\n \n \n");
                calcHistogram();
            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string text = "Nincs kép betöltve!\nTöltsön be egy képet előbb: 'Fájl műveletek' > 'Kép betöltése' menüpontot használva!";
                string caption = "HIBA";
                MessageBox.Show(text, caption);
            }
        }

        private void convolve(float[,] image, float[,] temp, float[] kernel)
        {
            int width = image.GetLength(1);
            int height = image.GetLength(0);
            int radius = kernel.Length / 2;

            unsafe
            {
                fixed (float* ptrImage = image, ptrTemp = temp)
                {
                    float* src = ptrImage + radius;
                    float* tmp = ptrTemp + radius;

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = radius; x < width - radius; x++, src++, tmp++)
                        {
                            float v = 0;
                            for (int k = 0; k < kernel.Length; k++)
                                v += src[k - radius] * kernel[k];
                            *tmp = v;
                        }
                        src += 2 * radius;
                        tmp += 2 * radius;
                    }


                    for (int x = 0; x < width; x++)
                    {
                        for (int y = radius; y < height - radius; y++)
                        {
                            src = ptrImage + y * width + x;
                            tmp = ptrTemp + y * width + x;

                            float v = 0;
                            for (int k = 0; k < kernel.Length; k++)
                                v += tmp[width * (k - radius)] * kernel[k];
                            *src = v;
                        }
                    }
                }
            }
        }


    private double gaussDensity (double sigma, double x, double mu){ return  1 / (sigma* Math.Sqrt(2 * Math.PI)) * Math.Exp(-0.5 * (x - mu) * (x - mu) / (sigma* sigma));}

    private unsafe void jellemzőpontokDetektálásaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                logBox.AppendText("\n Sarokpontok detektálása... \n");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                /////////////////
                Bitmap image = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height, PixelFormat.Format24bppRgb);
                using (Graphics gr = Graphics.FromImage(image)) gr.DrawImage(pictureBox.Image, new Rectangle(0, 0, image.Width, image.Height));
                BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat); //lefoglalunk egy kép méretével megegyező részt a képen bitmap formátumra konvertálva írásra és olvasására, és bekérjük a csatornakiosztás módját 
                int height = bitmapData.Height;
                int bPP = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8; //pixelenkénti bájtok száma, az iteráláshoz
                int width = bitmapData.Width * bPP;
                int srcStride = bitmapData.Stride;
                int srcOffset = srcStride - width;

                //Előtte kell egy sobel!
                //double[] sobeVektor1 = { -1, 0, 1 };
                //double[] sobeVektor2 = { 1, 2, 1 };

                int Threshold = 1000;
                int sigma = 4;



                pictureBox.Image = image;

                stopwatch.Stop();
                logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed + "\n");

                logBox.AppendText("\n \n \n");
                calcHistogram();

            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string message = "Nincs kép betöltve!\nTöltsön be egy képet előbb egy képet: 'Fájlt műveletek' > 'Kép betöltése' menüpontot használva!";
                string title = "HIBA";
                MessageBox.Show(message, title);
            }
        }

        private void átlagolóSzűrőBoxSzűrőToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                logBox.AppendText("\n Átlagoló (box) szűrő alkalmazása... \n");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ////párhuzamosítás nélkül
                //Bitmap bitmap = new Bitmap(pictureBox.Image);
                //int width = bitmap.Width;
                //int height = bitmap.Height;
                //Bitmap bitmap2 = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
                //for (int i = 0; i < width; i++)
                //{
                //    for (int j = 0; j < height; j++)
                //    {
                //        if (i > 1 && j > 1 && i + 1 != width && j + 1 != height)
                //        {
                //            int newRValue = bitmap.GetPixel(i, j + 1).R +
                //                            bitmap.GetPixel(i + 1, j + 1).R +
                //                            bitmap.GetPixel(i - 1, j).R +
                //                            bitmap.GetPixel(i, j).R +
                //                            bitmap.GetPixel(i + 1, j).R +
                //                            bitmap.GetPixel(i - 1, j - 1).R +
                //                            bitmap.GetPixel(i, j - 1).R +
                //                            bitmap.GetPixel(i + 1, j - 1).R;
                //            newRValue = Convert.ToInt32(newRValue / 9);

                //            int newGValue = bitmap.GetPixel(i, j + 1).G +
                //                            bitmap.GetPixel(i + 1, j + 1).G +
                //                            bitmap.GetPixel(i - 1, j).G +
                //                            bitmap.GetPixel(i, j).G +
                //                            bitmap.GetPixel(i + 1, j).G +
                //                            bitmap.GetPixel(i - 1, j - 1).G +
                //                            bitmap.GetPixel(i, j - 1).G +
                //                            bitmap.GetPixel(i + 1, j - 1).G;
                //            newGValue = Convert.ToInt32(newGValue / 9);

                //            int newBValue = bitmap.GetPixel(i, j + 1).B +
                //                            bitmap.GetPixel(i + 1, j + 1).B +
                //                            bitmap.GetPixel(i - 1, j).B +
                //                            bitmap.GetPixel(i, j).B +
                //                            bitmap.GetPixel(i + 1, j).B +
                //                            bitmap.GetPixel(i - 1, j - 1).B +
                //                            bitmap.GetPixel(i, j - 1).B +
                //                            bitmap.GetPixel(i + 1, j - 1).B;
                //            newBValue = Convert.ToInt32(newBValue / 9);

                //            bitmap2.SetPixel(i, j, Color.FromArgb(newRValue, newGValue, newBValue));
                //        }
                //    }
                //}
                //pictureBox.Image = bitmap2;

                //párhuzamosítással
                //Bitmap image = new Bitmap(pictureBox.Image);
                Bitmap image = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height, PixelFormat.Format24bppRgb); 
                using (Graphics gr = Graphics.FromImage(image)) gr.DrawImage(pictureBox.Image, new Rectangle(0, 0, image.Width, image.Height));
                BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat); //lefoglalunk egy kép méretével megegyező részt a képen bitmap formátumra konvertálva írásra és olvasására, és bekérjük a csatornakiosztás módját 
                int height = bitmapData.Height;
                int bPP = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8; //pixelenkénti bájtok száma, az iteráláshoz
                int width = bitmapData.Width * bPP;
                Bitmap bitmap2 = image.Clone(new Rectangle(0, 0, image.Width, image.Height), image.PixelFormat);

                //int bytes = bitmapData.Stride * bitmapData.Height;
                //byte[] result = new byte[bytes];

                unsafe //mert a c# nem támogatja a pointereket bitmapen
                {
                    byte* firstPixel = (byte*)bitmapData.Scan0; //a lefoglalt terület első pixele
                    Parallel.For(1, height - 1, j =>
                      {
                          //hogy soronként menjünk végig az első pixeltől
                          byte* line1 = firstPixel + ((j - 1) * bitmapData.Stride); //első sor
                          byte* line2 = firstPixel + (j * bitmapData.Stride); //második sor
                          byte* line3 = firstPixel + ((j + 1) * bitmapData.Stride); //hamradik sor
                          int idx = 0;

                          for (int i = 1; i < width - 1; i = i + bPP)//egy pixel hátom elem a három csatorna a sorban, BGR sorrendben a csatornák
                          {
                              int oldB = line1[i - 1] + line1[i + 2] + line1[i + 5] +
                                         line2[i - 1] + line2[i + 2] + line2[i + 5] +
                                         line3[i - 1] + line3[i + 2] + line3[i + 5];

                              int oldG = line1[i] + line1[i + 3] + line1[i + 6] +
                                         line2[i] + line2[i + 3] + line2[i + 6] +
                                         line3[i] + line3[i + 3] + line3[i + 6];

                              int oldR = line1[i + 1] + line1[i + 4] + line1[i + 7] +
                                         line2[i + 1] + line2[i + 4] + line2[i + 7] +
                                         line3[i + 1] + line3[i + 4] + line3[i + 7];

                              oldB /= 9;
                              oldG /= 9;
                              oldR /= 9;

                              lock (bitmap2)
                              {
                                  bitmap2.SetPixel(idx, j, Color.FromArgb(oldR, oldG, oldB));
                              }
                              idx++;

                              //result[i ]    = (byte)oldB;
                              //result[i + 1] = (byte)oldG;
                              //result[i + 2] = (byte)oldR;

                              //line[i] = (byte)oldB;
                              //line[i + 1] = (byte)oldG;
                              //line[i + 2] = (byte)oldR;
                          }
                      });
                    image.UnlockBits(bitmapData);
                }

                //Bitmap resImg = new Bitmap(width, height);
                //BitmapData resData = resImg.LockBits(new Rectangle(0, 0, width, height),
                //                     ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                //Marshal.Copy(result, 0, resData.Scan0, bytes);
                //resImg.UnlockBits(resData);
                //pictureBox.Image = resImg;

                pictureBox.Image = bitmap2;

                stopwatch.Stop();
                logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed.ToString() + "\n"); //párhuzamosítás nélkül: 00:00:13.9035825; párhuzmaosítással: 00:00:01.9231439
                logBox.AppendText("\n \n \n");
                calcHistogram();
            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string text = "Nincs kép betöltve!\nTöltsön be egy képet előbb: 'Fájl műveletek' > 'Kép betöltése' menüpontot használva!";
                string caption = "HIBA";
                MessageBox.Show(text, caption);
            }
        }

        private void sobelÉldetektorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                logBox.AppendText("\n Robert éldetektor alkalmazása... \n");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                ///párhuzamosítás nélkül
                //Bitmap result;
                //var grayImg = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height, PixelFormat.Format24bppRgb);
                //using (Graphics gr = Graphics.FromImage(grayImg)) gr.DrawImage(pictureBox.Image, new Rectangle(0, 0, grayImg.Width, grayImg.Height));

                //lock (grayImg)
                //{
                //    result = new Bitmap(grayImg);
                //    var data = result.LockBits(new Rectangle(0, 0, grayImg.Width, grayImg.Height),
                //               ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                //    unsafe
                //    {
                //        var p = (byte*)data.Scan0.ToPointer();
                //        var offset = data.Stride - result.Width * 3;

                //        for (var i = 0; i < result.Height - 1; i++)
                //        {
                //            for (var j = 0; j < result.Width - 1; j++)
                //            {
                //                var Gx = (int)Math.Pow(p[0] - (p + 3 + data.Stride)[0], 2);
                //                var Gy = (int)Math.Pow((p + 3)[0] - (p + data.Stride)[0], 2);

                //                var f = Gx + Gy;
                //                p[0] = p[1] = p[2] = (byte)f;

                //                p += 3;
                //            }

                //            p += offset;
                //        }
                //    }

                //    result.UnlockBits(data);
                //}
                //pictureBox.Image = result;

                /// párhuzamosítással
                Bitmap result;
                var grayImg = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height, PixelFormat.Format24bppRgb);
                using (Graphics gr = Graphics.FromImage(grayImg)) gr.DrawImage(pictureBox.Image, new Rectangle(0, 0, grayImg.Width, grayImg.Height));

                lock (grayImg)
                {
                    result = new Bitmap(grayImg);
                    var data = result.LockBits(new Rectangle(0, 0, grayImg.Width, grayImg.Height),
                               ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    int width = result.Width;
                    unsafe
                    {
                        var p = (byte*)data.Scan0.ToPointer();
                        var offset = data.Stride - result.Width * 3;


                        Parallel.For(0, result.Height - 1, i =>
                        {
                            //for (var i = 0; i < result.Height - 1; i++)
                            //{
                                for (var j = 0; j < width - 1; j++)
                                {

                                    int Gx;
                                    int Gy;
                                    lock (data)
                                    {
                                        Gx  = (int)Math.Pow(p[0] - (p + 3 + data.Stride)[0], 2);
                                        Gy  = (int)Math.Pow((p + 3)[0] - (p + data.Stride)[0], 2);
                                    } 

                                    var f = Gx + Gy;
                                    p[0] = p[1] = p[2] = (byte)f;

                                    p += 3;
                                }

                                p += offset;
                            //}
                        });
                    }

                    result.UnlockBits(data);
                }
                pictureBox.Image = result;

                stopwatch.Stop();
                logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed + "\n"); // párhuzamosítás nélkül:  00:00:00.3160637; párhuzmaosítva: 00:00:00.2585962

                logBox.AppendText("\n \n \n");
                calcHistogram();
            }
        }

        unsafe public byte[] ConvertTo8bpp(Bitmap image)
        {
            logBox.AppendText("\n ConvertTo8bpp... \n");
            int width = image.Width;
            int height = image.Height;

            byte[] grayData = new byte[width * height];

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, image.PixelFormat);

            uint* ptr = (uint*)imageData.Scan0.ToPointer();
            int inputStride = imageData.Stride / 4;

            byte r, g, b;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Obtain current pixel data
                    uint pixel = *(ptr + y * inputStride + x); // Standard formula from the video, px = y * stride + x

                    // Split channels - this uses bit shifting and bitwise AND
                    r = (byte)((0x00FF0000 & pixel) >> 16);
                    g = (byte)((0x0000FF00 & pixel) >> 8);
                    b = (byte)((0x000000FF & pixel));

                    byte gray = (byte)(0.2126 * r + 0.7152 * g + 0.0722 * b);
                    grayData[y * width + x] = gray;
                }
            }

            image.UnlockBits(imageData);
            logBox.AppendText("\n ConvertTo8bpp kész \n");

            return grayData;
        }

        private void HSVtoRGB(double h, double s, double v, out byte r, out byte g, out byte b)
        {
            // h_ 0-6
            double h_ = h / (2 * Math.PI) * 6;

            double c = s * v;
            double x = c * (1 - Math.Abs((h_ % 2) - 1));
            double r_, g_, b_;
            if (h_ < 1)
            {
                r_ = c;
                g_ = x;
                b_ = 0;
            }
            else if (h_ < 2)
            {
                r_ = x;
                g_ = c;
                b_ = 0;
            }
            else if (h_ < 3)
            {
                r_ = 0;
                g_ = c;
                b_ = x;
            }
            else if (h_ < 4)
            {
                r_ = 0;
                g_ = x;
                b_ = c;
            }
            else if (h_ < 5)
            {
                r_ = x;
                g_ = 0;
                b_ = c;
            }
            else
            {
                r_ = c;
                g_ = 0;
                b_ = x;
            }

            double m = v - c;

            r_ += m;
            g_ += m;
            b_ += m;

            r = (byte)(r_ * 255);
            g = (byte)(g_ * 255);
            b = (byte)(b_ * 255);
        }
        private void sobelÉldetektorToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (imageLoaded)
            {
                logBox.AppendText("\n Sobel alkalmazása... \n");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ////párhuzamosítás nélkül
                Bitmap image = new Bitmap(pictureBox.Image);
                
                // This simple code is meant for 32bpp images, it could be adapted to handle other image types.
                // 32ARGB and 32RGB are the same, except in the latter the alpha byte is simply padding. We ignore alpha in this code anyway.
                if (image.PixelFormat != PixelFormat.Format32bppArgb
                    && image.PixelFormat != PixelFormat.Format32bppRgb)
                {
                    logBox.AppendText("\n Nem megfelelő képformátum! \n");
                    return;
                }

                // Obtain grayscale conversion of the image
                byte[] grayData = ConvertTo8bpp(image);

                int width = image.Width;
                int height = image.Height;

                // Buffers
                byte[] buffer = new byte[9];
                double[] magnitude = new double[width * height]; // Stores the magnitude of the edge response
                double[] orientation = new double[width * height]; // Stores the angle of the edge at that location

                // First pass - convolve sobel operator and calculate orientation. We're using the byte array now, since it's easier.
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        // Unlike the other Kernel operations, where the radius etc. might change this one is simple so we can hard code
                        // the kernel operations in. Pointer arithmetic would make this slightly faster, but we won't worry about it.
                        int index = y * width + x;

                        // 3x3 window around (x,y)
                        buffer[0] = grayData[index - width - 1];
                        buffer[1] = grayData[index - width];
                        buffer[2] = grayData[index - width + 1];
                        buffer[3] = grayData[index - 1];
                        buffer[4] = grayData[index];
                        buffer[5] = grayData[index + 1];
                        buffer[6] = grayData[index + width - 1];
                        buffer[7] = grayData[index + width];
                        buffer[8] = grayData[index + width + 1];

                        // Sobel horizontal and vertical response
                        double dx = buffer[2] + 2 * buffer[5] + buffer[8] - buffer[0] - 2 * buffer[3] - buffer[6];
                        double dy = buffer[6] + 2 * buffer[7] + buffer[8] - buffer[0] - 2 * buffer[1] - buffer[2];

                        magnitude[index] = Math.Sqrt(dx * dx + dy * dy) / 1141; // 1141 is approximately the max sobel response, we will normalise later anyway

                        // Directional orientation
                        orientation[index] = Math.Atan2(dy, dx) + Math.PI; // Angle is in radians, now from 0 - 2PI. 
                    }
                }

                /* Now that we have the magnitude and orientation, we want to combine these into a coloured image for output.
                 * The HSV colour model would work well here, hue is the angle of the colour, saturation we keep constant at 1,
                 * and value is the magnitude. This should produce an image that is coloured based on edge angle, and whose brightness
                 * reflects edge intensity. */

                // System.Bitmap includes SetPixel and GetPixel methods. These are very slow, so it's better to lock
                // the image and access the data directly. Locking prevents C# memory management moving it around.
                BitmapData imageData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, image.PixelFormat);

                unsafe 
                {
                    uint* ptr = (uint*)imageData.Scan0.ToPointer(); // An unsigned int pointer. This points to the image data in memory, each uint is one pixel ARGB
                    int stride = imageData.Stride / 4; // Stride is the width of one pixel row, including any padding. In bytes, /4 converts to 4 byte pixels 

                    byte r, g, b;

                    // We want to scale magnitude from 0 - 1, because it's unlikely any magnitude would reach the theoretical maximum value
                    // C#, like other high level languages, contains various list extension methods for ease of use.
                    double magnitudeMax = magnitude.Max();

                    // Combine 
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int index = y * width + x;

                            // C# functions can make use of an out parameter. This works much like C/C++ address pointer (&), except it is compiler enforced
                            // Here we can use it to essentially return three values
                            double hue = orientation[index];
                            double val = magnitude[index] / magnitudeMax; // This will still highlight mostly very bright edges, to highlight more try Math.Sqrt(magnitude[index] / magnitudeMax)

                            HSVtoRGB(hue, 1, val, out r, out g, out b); // Using a saturation of 0 will make the image grayscale i.e. regular sobel.

                            // Combine rgb back into the output image
                            *(ptr + y * stride + x) = (0xFF000000 | (uint)(r << 16) | (uint)(g << 8) | b);
                        }
                    }
                }
                image.UnlockBits(imageData);
                pictureBox.Image = image;

                stopwatch.Stop();
                logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed.ToString() + "\n"); //párhuzamosítás nélkül: 00:00:00.2155732; párhuzmaosítással: 
                logBox.AppendText("\n \n \n");
                calcHistogram();
            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string text = "Nincs kép betöltve!\nTöltsön be egy képet előbb: 'Fájl műveletek' > 'Kép betöltése' menüpontot használva!";
                string caption = "HIBA";
                MessageBox.Show(text, caption);
            }
        }

        private void laplaceÉldetektorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (imageLoaded)
            {
                logBox.AppendText("\n Laplace éldetektor alkalmazása... \n");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                double[,] MASK = new double[12, 12] {
                                {-0.000699762,  -0.000817119,   -0.000899703,   -0.000929447,   -0.000917118,   -0.000896245,   -0.000896245,   -0.000917118,   -0.000929447,   -0.000899703,   -0.000817119,   -0.000699762},
                                {-0.000817119,  -0.000914231,   -0.000917118,   -0.000813449,   -0.000655442,   -0.000538547,   -0.000538547,   -0.000655442,   -0.000813449,   -0.000917118,   -0.000914231,   -0.000817119},
                                {-0.000899703,  -0.000917118,   -0.000745635,   -0.000389918,   0.0000268,      0.000309618,    0.000309618,    0.0000268,  -0.000389918,   -0.000745635,   -0.000917118,   -0.000899703},
                                {-0.000929447,  -0.000813449,   -0.000389918,   0.000309618,    0.001069552,    0.00156934,     0.00156934,     0.001069552,    0.000309618,    -0.000389918,   -0.000813449,   -0.000929447},
                                {-0.000917118,  -0.000655442,   0.0000268,      0.001069552,    0.002167033,    0.002878738,    0.002878738,    0.002167033,    0.001069552,    0.0000268,  -0.000655442,   -0.000917118},
                                {-0.000896245,  -0.000538547,   0.000309618,    0.00156934,     0.002878738,    0.003722998,    0.003722998,    0.002878738,    0.00156934, 0.000309618,    -0.000538547,   -0.000896245},
                                {-0.000896245,  -0.000538547,   0.000309618,    0.00156934,     0.002878738,    0.003722998,    0.003722998,    0.002878738,    0.00156934, 0.000309618,    -0.000538547,   -0.000896245},
                                {-0.000917118,  -0.000655442,   0.0000268,      0.001069552,    0.002167033,    0.002878738,    0.002878738,    0.002167033,    0.001069552,    0.0000268,  -0.000655442,   -0.000917118},
                                {-0.000929447,  -0.000813449,   -0.000389918,   0.000309618,    0.001069552,    0.00156934,     0.00156934,     0.001069552,    0.000309618,    -0.000389918,   -0.000813449,   -0.000929447},
                                {-0.000899703,  -0.000917118,   -0.000745635,   -0.000389918,   0.0000268,      0.000309618,    0.000309618,    0.0000268,  -0.000389918,   -0.000745635,   -0.000917118,   -0.000899703},
                                {-0.000817119,  -0.000914231,   -0.000917118,   -0.000813449,   -0.000655442,   -0.000538547,   -0.000538547,   -0.000655442,   -0.000813449,   -0.000917118,   -0.000914231,   -0.000817119},
                                {-0.000699762,  -0.000817119,   -0.000899703,   -0.000929447,   -0.000917118,   -0.000896245,   -0.000896245,   -0.000917118,   -0.000929447,   -0.000899703,   -0.000817119,   -0.000699762}
                            };

            double nTemp = 0.0;
            double c = 0;

            int mdl, size;
            size = 12;
            mdl = size / 2;

            double min, max;
            min = max = 0.0;

            double sum = 0.0;
            double mean;
            double d = 0.0;
            double s = 0.0;
            int n = 0;

            Bitmap SrcImage = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height, PixelFormat.Format24bppRgb);
            using (Graphics gr = Graphics.FromImage(SrcImage)) gr.DrawImage(pictureBox.Image, new Rectangle(0, 0, SrcImage.Width, SrcImage.Height));

            Bitmap bitmap = new Bitmap(SrcImage.Width + mdl, SrcImage.Height + mdl);
            int l, k;

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData srcData = SrcImage.LockBits(new Rectangle(0, 0, SrcImage.Width, SrcImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                //párhuzamosítás nélkül
                //unsafe
                //{
                //    int offset = 3;

                //    for (int colm = 0; colm < srcData.Height - size; colm++) //iterate througt the image
                //    {
                //        byte* ptr = (byte*)srcData.Scan0 + (colm * srcData.Stride);
                //        byte* bitmapPtr = (byte*)bitmapData.Scan0 + (colm * bitmapData.Stride);

                //        for (int row = 0; row < srcData.Width - size; row++)
                //        {
                //            nTemp = 0.0;

                //            min = double.MaxValue;
                //            max = double.MinValue;

                //            for (k = 0; k < size; k++) //iterate througt the convolution matrix
                //                {
                //                for (l = 0; l < size; l++)
                //                {
                //                    byte* tempPtr = (byte*)srcData.Scan0 + ((colm + l) * srcData.Stride); //get that part of the image which is currently procesed, with that size which convolutional matrix has
                //                    c = (tempPtr[((row + k) * offset)] + tempPtr[((row + k) * offset) + 1] + tempPtr[((row + k) * offset) + 2]) / 3; //pixelek átlaga

                //                    nTemp += (double)c * MASK[k, l]; //pixelek átlaga * konvolúciós mátrix adott értéke

                //                }
                //            }

                //            sum += nTemp;
                //            n++;
                //        }
                //    }
                //    mean = ((double)sum / n);
                //    d = 0.0;

                //    for (int i = 0; i < srcData.Height - size; i++)
                //    {
                //        byte* ptr = (byte*)srcData.Scan0 + (i * srcData.Stride);
                //        byte* tptr = (byte*)bitmapData.Scan0 + (i * bitmapData.Stride);

                //        for (int j = 0; j < srcData.Width - size; j++)
                //        {
                //            nTemp = 0.0;

                //            min = double.MaxValue;
                //            max = double.MinValue;

                //            for (k = 0; k < size; k++)
                //            {
                //                for (l = 0; l < size; l++)
                //                {
                //                    byte* tempPtr = (byte*)srcData.Scan0 + ((i + l) * srcData.Stride);
                //                    c = (tempPtr[((j + k) * offset)] + tempPtr[((j + k) * offset) + 1] + tempPtr[((j + k) * offset) + 2]) / 3;

                //                    nTemp += (double)c * MASK[k, l];

                //                }
                //            }

                //            s = (mean - nTemp); //másik irány
                //            d += (s * s);
                //        }
                //    }


                //        d = d / (n - 1);
                //        d = Math.Sqrt(d);
                //        d = d * 2;

                //        for (int colm = mdl; colm < srcData.Height - mdl; colm++)
                //        {
                //        byte* ptr = (byte*)srcData.Scan0 + (colm * srcData.Stride);
                //        byte* bitmapPtr = (byte*)bitmapData.Scan0 + (colm * bitmapData.Stride);

                //        for (int row = mdl; row < srcData.Width - mdl; row++)
                //        {
                //            nTemp = 0.0;

                //            min = double.MaxValue;
                //            max = double.MinValue;

                //            for (k = (mdl * -1); k < mdl; k++)
                //            {
                //                for (l = (mdl * -1); l < mdl; l++)
                //                {
                //                    byte* tempPtr = (byte*)srcData.Scan0 + ((colm + l) * srcData.Stride);
                //                    c = (tempPtr[((row + k) * offset)] + tempPtr[((row + k) * offset) + 1] + tempPtr[((row + k) * offset) + 2]) / 3;

                //                    nTemp += (double)c * MASK[mdl + k, mdl + l];

                //                }
                //            }

                //            if (nTemp > d)
                //            {
                //                bitmapPtr[row * offset] = bitmapPtr[row * offset + 1] = bitmapPtr[row * offset + 2] = 255;
                //            }
                //            else
                //                bitmapPtr[row * offset] = bitmapPtr[row * offset + 1] = bitmapPtr[row * offset + 2] = 0;

                //        }
                //    }
                //}

                //bitmap.UnlockBits(bitmapData);
                //SrcImage.UnlockBits(srcData);

                //párhuzamosítással
                unsafe
                {
                    int offset = 3;

                    for (int colm = 0; colm < srcData.Height - size; colm++) //iterate througt the image
                    //Parallel.For(0, srcData.Height - size, colm => //iterate througt the image
                    {
                        //byte* ptr = (byte*)srcData.Scan0 + (colm * srcData.Stride);
                        //byte* bitmapPtr = (byte*)bitmapData.Scan0 + (colm * bitmapData.Stride);

                       for (int row = 0; row < srcData.Width - size; row++)
                       {
                           nTemp = 0.0;

                           min = double.MaxValue;
                           max = double.MinValue;

                           for (k = 0; k < size; k++) //iterate througt the convolution matrix
                            {
                               for (l = 0; l < size; l++)
                               {
                                   byte* tempPtr = (byte*)srcData.Scan0 + ((colm + l) * srcData.Stride); //get that part of the image which is currently procesed, with that size which convolutional matrix has
                                    c = (tempPtr[((row + k) * offset)] + tempPtr[((row + k) * offset) + 1] + tempPtr[((row + k) * offset) + 2]) / 3; //pixelek átlaga

                                    nTemp += (double)c * MASK[k, l]; //pixelek átlaga * konvolúciós mátrix adott értéke

                                }
                           }

                           sum += nTemp;
                           n++;
                       }
                        //});
                    }

                    mean = ((double)sum / n);
                    d = 0.0;

                    for (int i = 0; i < srcData.Height - size; i++)
                    {
                        //byte* ptr = (byte*)srcData.Scan0 + (i * srcData.Stride);
                        //byte* tptr = (byte*)bitmapData.Scan0 + (i * bitmapData.Stride);

                        for (int j = 0; j < srcData.Width - size; j++)
                        {
                            nTemp = 0.0;

                            min = double.MaxValue;
                            max = double.MinValue;

                            for (k = 0; k < size; k++)
                            {
                                for (l = 0; l < size; l++)
                                {
                                    byte* tempPtr = (byte*)srcData.Scan0 + ((i + l) * srcData.Stride);
                                    c = (tempPtr[((j + k) * offset)] + tempPtr[((j + k) * offset) + 1] + tempPtr[((j + k) * offset) + 2]) / 3;

                                    nTemp += (double)c * MASK[k, l];

                                }
                            }

                            s = (mean - nTemp); //másik irány
                            d += (s * s);
                        }
                    }


                    d = d / (n - 1);
                    d = Math.Sqrt(d);
                    d = d * 2;

                    for (int colm = mdl; colm < srcData.Height - mdl; colm++)
                    {
                        //byte* ptr = (byte*)srcData.Scan0 + (colm * srcData.Stride);
                        byte* bitmapPtr = (byte*)bitmapData.Scan0 + (colm * bitmapData.Stride);

                        for (int row = mdl; row < srcData.Width - mdl; row++)
                        {
                            nTemp = 0.0;

                            min = double.MaxValue;
                            max = double.MinValue;

                            for (k = (mdl * -1); k < mdl; k++)
                            {
                                for (l = (mdl * -1); l < mdl; l++)
                                {
                                    byte* tempPtr = (byte*)srcData.Scan0 + ((colm + l) * srcData.Stride);
                                    c = (tempPtr[((row + k) * offset)] + tempPtr[((row + k) * offset) + 1] + tempPtr[((row + k) * offset) + 2]) / 3;

                                    nTemp += (double)c * MASK[mdl + k, mdl + l];

                                }
                            }

                            if (nTemp > d)
                            {
                                bitmapPtr[row * offset] = bitmapPtr[row * offset + 1] = bitmapPtr[row * offset + 2] = 255;
                            }
                            else
                                bitmapPtr[row * offset] = bitmapPtr[row * offset + 1] = bitmapPtr[row * offset + 2] = 0;

                        }
                    }
                }

                bitmap.UnlockBits(bitmapData);
                SrcImage.UnlockBits(srcData);

                pictureBox.Image = bitmap;
            stopwatch.Stop();
            logBox.AppendText("\n Telejsítve ennyi idő alatt: " + stopwatch.Elapsed.ToString() + "\n"); //párhuzamosítás nélkül: 00:00:09.7146431; párhuzmaosítással: 
            logBox.AppendText("\n \n \n");
            calcHistogram();
            }
            else
            {
                logBox.AppendText("\nHIBA: nincs kép betöltve!\n");
                string text = "Nincs kép betöltve!\nTöltsön be egy képet előbb: 'Fájl műveletek' > 'Kép betöltése' menüpontot használva!";
                string caption = "HIBA";
                MessageBox.Show(text, caption);
            }
        }
    }
}
