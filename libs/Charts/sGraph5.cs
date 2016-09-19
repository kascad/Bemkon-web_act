//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System;
using System.Collections.Generic;

namespace Charts
{
    public class sGraph5
    {

        int width;
        int height;
        double valMax;
        double valMin;
        double valStep = 0;
        List<percentilxy> datalist = null;
        List<string> namefield = new List<string>();
        public sGraph5(int width, int height, List<percentilxy> data, List<string> data2, double valStep)
        {
            this.width = width;
            this.height = height;
            this.datalist = data;
            this.namefield = data2;
            this.valStep = valStep;
        }
        float getLocalX(double r, PointF cPoint, double angle)
        {
            //return (Math.cos(_angle * Math.PI / 180) * _r);
            return (float)Math.Abs(cPoint.X + (Math.Cos(angle * (-1) * Math.PI / 180) * r));
        }
        float getLocalY(double r, PointF cPoint, double angle)
        {
            return (float)Math.Abs(cPoint.Y + (Math.Sin(angle * (-1) * Math.PI / 180) * r));
            //return (Math.sin(_angle * Math.PI / 180) * _r);
        }
        PointF getLinePoint(double r, PointF cPoint, double angle)
        {
            PointF res = new PointF(getLocalX(r, cPoint, angle), getLocalY(r, cPoint, angle));
            return res;
        }
        PointF getRealCoord(double kx, double ky, double valx, double valy, int offsetw, int offseth)
        {

            PointF res = new PointF();
            res.X = (float)((valx) * kx + offsetw);
            res.Y = (float)((valMax - valy) * ky + offseth);
            return res;
        }
        public void saveImage(string path)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics gBmp = System.Drawing.Graphics.FromImage(bmp);
            Brush backBrush = new SolidBrush(Color.Gray);
            Brush textBrush = new SolidBrush(Color.Black);
            int w, h, offseth, offsetw;
            offsetw = 70;
            offseth = 90;
            valMax = 120;
            valMin = 0;
            double reserv = 20;
            w = (int)(width - 10) - offsetw - (int)reserv;
            h = (int)(height - offseth - reserv);
            //valueh = (int)(h * 0.4);
            Font drawFont = new Font("Tahoma", 13);

            /*
            foreach (var item2 in datalist)
            {
                int i = 0;
                if (item2.valuex == 100)
                        {
                    namefield.Add(item2.name);
                        }

                if (item2.valuex == 0)
                        {
                    namefield.Add(item2.name);
                }
                if (item2.valuey == 100)
                        {
                    namefield.Add(item2.name);
                }
                if (item2.valuey == 0)
                        {
                    namefield.Add(item2.name);
                }
            }
            */

            Font digFont = new Font("Tahoma", 7, FontStyle.Regular);
            PointF cPoint = new PointF();
            PointF lastPoint = new PointF();
            gBmp.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, width, height));
            gBmp.FillRectangle(backBrush, new Rectangle(offsetw, offseth, w, h));
            gBmp.DrawString("Расположение понятий в пространстве " + namefield[0] + "-" + namefield[1] + " vs " + namefield[2] + "-" + namefield[3], drawFont, textBrush, 10, offseth / 3);
            //gBmp.DrawString("Расположение понятий в пространстве " + datalist[0].name + "-" + datalist[1].name + " vs " + datalist[2].name + "-" + datalist[3].name, drawFont, textBrush, 10, offseth / 3);
            gBmp.DrawString("Измерение: процентили", drawFont, textBrush, w / 3, offseth / 3 + 25);
            PointF newpoint = new PointF();

            double kx = (w) / (valMax - valMin);
            double ky = (h) / (valMax - valMin);
            //double k= (h / 2) / (valMax - valMin);
            if (valStep == 0)
            {
                valStep = (valMax - valMin) / ((h) / (10));
            }
            valStep = (valStep >= 1 ? valStep : 1);
            //double valX=0;
            //y
            float y = 0;
            for (double i = 0; i <= valMax; i += (valStep))
            {
                double texth = (gBmp.MeasureString((valMax - i).ToString("N0") + "%", new Font("Microsoft Sans Serif", 10)).Height);
                y = (float)((i) * ky + offseth);
                gBmp.DrawLine(new Pen(Color.Black),
                    new PointF(offsetw, y),
                    new PointF(w + offsetw, y));
                try
                {
                    gBmp.DrawString((valMax - i).ToString("N0") + "%", new Font("Microsoft Sans Serif", 10), textBrush, 20, (float)(y - texth / 2));
                }
                catch (Exception)
                {

                    //throw;
                }
            }
            float x = 0;
            for (double i = 0; i <= valMax; i += (valStep))
            {
                double textw = (gBmp.MeasureString((valMax - i).ToString("N0") + "%", new Font("Microsoft Sans Serif", 10)).Width);
                y = (float)((valMax) * ky + offseth);
                x = (float)((i) * kx + offsetw);
                gBmp.DrawLine(new Pen(Color.Black),
                    new PointF(x, y - 5),
                    new PointF(x, y + 5));
                try
                {
                    gBmp.DrawString((i).ToString("N0") + "%", new Font("Microsoft Sans Serif", 10), textBrush, (float)(x - textw / 2), (float)(y));
                }
                catch (Exception)
                {

                    //throw;
                }
            }
            //System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            ////drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            //string text1 = "asd dsa asd dsa asd dsa";

            //Bitmap bmp1 = new Bitmap((int)gBmp.MeasureString(text1, drawFont).Width, (int)gBmp.MeasureString(text1, drawFont).Width);
            //Graphics formGraphics = System.Drawing.Graphics.FromImage(bmp1);
            //formCharts.DrawString(text1, drawFont, textBrush, 0, 0, drawFormat);
            //formCharts.RotateTransform(45);

            ////bmp1.Save(path);
            //formCharts.DrawImage(bmp1, 5, h - 20, bmp1.Width, bmp1.Height);
            //bmp1.Save(path);
            foreach (var item in datalist)
            {
                PointF pf = getRealCoord(kx, ky, item.valuex, item.valuey, offsetw, offseth);
                gBmp.DrawEllipse(new Pen(Color.Black), pf.X - 4, pf.Y - 4, 8, 8);
                gBmp.FillEllipse(new SolidBrush(Color.Yellow), pf.X - 4, pf.Y - 4, 8, 8);
                double texth1 = (gBmp.MeasureString(item.name, drawFont).Height);
                double textw1 = (gBmp.MeasureString(item.name, drawFont).Width);
                pf.Y = pf.Y - (float)texth1 / 2;
                if (textw1 + pf.X > w + offsetw)
                    pf.X -= (float)textw1 + 8;
                else
                    pf.X += 8;
                gBmp.DrawString(item.name, drawFont, textBrush, pf);
            }




            gBmp.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
            try
            {
                //FileStream fs = File.OpenWrite(path)
                bmp.Save(path);
            }
            catch (Exception e)
            {

                //throw;
            }


            gBmp.Dispose();
            bmp.Dispose();
        }
        void drawZeroLine(System.Drawing.Graphics gBmp, float w, float k, float offsetw, float offseth)
        {
            float y = (float)(((valMax - 0)) * k) + offseth;
            gBmp.DrawLine(new Pen(Color.Green, 2),
                    new PointF(offsetw, y),
                    new PointF(w + offsetw, y));
        }
    }
    public class percentilxy
    {
        public percentilxy(string name, double valuex, double valuey)
        {
            this.name = name;
            this.valuex = valuex;
            this.valuey = valuey;
        }
        public string name;
        public double valuex;
        public double valuey;

        public int CompareTo(percentilxy psort)
        {
            return this.valuex.CompareTo(psort.valuex);
        }

    }

}
