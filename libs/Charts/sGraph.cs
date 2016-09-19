//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System;

namespace Charts
{
    public class sGraph
    {

        int width;
        int height;
        double valMax;
        double valMin;
        double valAvr;
        double valReal;
        double valStep;
        string valText;
        public sGraph(int width, int height, double valMax, double valMin, 
            double valAvr, double valReal, double valStep, string valText)
        {
            this.valAvr = valAvr;
            this.valMax = valMax;
            this.valMin = valMin;
            this.valReal = valReal;
            this.valStep = valStep;
            this.valText = valText;
            this.width = width;
            this.height = height;
        }
        public void saveImage(string path)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics gBmp = System.Drawing.Graphics.FromImage(bmp);
            //gBmp.CompositingMode = CompositingMode.SourceCopy;
            Brush backBrush = new SolidBrush(Color.Gray);
            Brush textBrush = new SolidBrush(Color.Black);
            int w, h, offseth, offsetw, valueh;
            offsetw = 90;
            offseth = 10;
            w = (int)(width - 10) - offsetw;
            h = (int)(height * 0.8) - offseth;
            valueh = (int)(h * 0.4);
            Font drawFont = new Font("Microsoft Sans Serif", 13);
            double someMin = 0;


            if (valMin < someMin)
                someMin = valMin;
            if (valReal < someMin)
                someMin = valReal;
            if (valAvr < someMin)
                someMin = valAvr;
            if (valMax < valReal)
                valMax = valReal;
            //if (valStep <= 0)
            //    valStep = 1;
            double k = w / (valMax - someMin);
            if (valStep == 0)
            {
                SizeF sf = gBmp.MeasureString(someMin.ToString("N0"), drawFont);

                valStep = (valMax - someMin) / (w / (sf.Width + 10));
            }
            valStep = (valStep >= 1 ? valStep : 1);
            gBmp.FillRectangle(backBrush, new Rectangle(offsetw, offseth, w, h - offseth));

            float x = 0;
            float y = 0;

            gBmp.SmoothingMode = SmoothingMode.AntiAlias;
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;
            //double lol = (double)(valMin * (valMax - valMin) / w);
            double lol = (double)(valMin * (valMax - someMin) / w);
            //double j = 0;

            //double xstep = (w * valStep) / (valMax - valMin);
            double xstep = (w * valStep) / (valMax - someMin);
            for (double i = someMin; i <= valMax - someMin; i += (valStep))
            //for (double i = someMin; i <= valMax - valMin; i += (valStep))
            {
                //gBmp.DrawLine(new Pen(Color.Black),
                //    new Point((int)(i * (w / (valMax - valMin)) + (valMin < 0 ? valMin * (-1) : valMin)) + offset, offset),
                //    new Point((int)(i * (w / (valMax - valMin)) + (valMin < 0 ? valMin * (-1) : valMin)) + offset, h + offset));
                x = (float)((i - someMin) * k + offsetw);
                gBmp.DrawLine(new Pen(Color.Black),
                    new Point((int)x, offseth),
                    new Point((int)x, h + offseth));

                //gBmp.tra
                y = h + offseth;
                try
                {

                    gBmp.DrawString(i.ToString("N0"), new Font("Microsoft Sans Serif", 10), textBrush, x - (x > offsetw ? gBmp.MeasureString(i.ToString("N0"), new Font("Microsoft Sans Serif", 10)).Width / 2 : 0), y);
                }
                catch (Exception)
                {

                    //throw;
                }
                //j += xstep;
                //gBmp.DrawString(i.ToString(), drawFont, textBrush, new Point(Convert.ToInt32(i * (w / (valMax - valMin))), h + offset));
            }
            double valReal1 = 0;
            if (valReal > valReal1)
            {
                valReal1 = valReal;
                valReal = 0;
            }
            gBmp.DrawLine(new Pen(Color.Black), offsetw, h, w - offsetw, h);
            //
            //new Rectangle(offset,Convert.ToInt32(h / 2 - valueh / 2),
            x = (float)((valReal1 - someMin) * k + offsetw);
            gBmp.FillRectangle(new SolidBrush(Color.PowderBlue),
                new Rectangle(
                    (int)((valReal - someMin) * k + offsetw),
                    offseth + Convert.ToInt32((h / 2) - (valueh / 2)),
                    Math.Abs(Convert.ToInt32(x) - (int)((valReal - someMin) * k + offsetw)),
                    offseth + Convert.ToInt32(valueh))
                );
            gBmp.DrawRectangle(new Pen(Color.Black),
                new Rectangle(
                    (int)((valReal - someMin) * k + offsetw),
                    offseth + Convert.ToInt32((h / 2) - (valueh / 2)),
                    Math.Abs(Convert.ToInt32(x) - (int)((valReal - someMin) * k + offsetw)),
                    offseth + Convert.ToInt32(valueh))
                );
            try
            {
                gBmp.DrawString(valText,
               drawFont,
               new SolidBrush(Color.Red),
                new Rectangle(
                    (int)((valReal - someMin) * k + offsetw),
                    offseth + Convert.ToInt32((h / 2) - (valueh / 2)),
                    Math.Abs(Convert.ToInt32(x) - (int)((valReal - someMin) * k + offsetw)),
                    offseth + Convert.ToInt32(valueh))
                , strFormat
           );

            }
            catch (Exception)
            {

                //throw;
            }
            SizeF avgsize = gBmp.MeasureString("AVG:" + valAvr.ToString(), drawFont);
            //avg
            x = (float)((valAvr - someMin) * k + offsetw);
            gBmp.DrawLine(new Pen(Color.Red),
                    new Point(Convert.ToInt32(x), offseth),
                    new Point(Convert.ToInt32(x), h + offseth));

            gBmp.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(
                    Convert.ToInt32(x),
                   (int)((h / 2) * 0.4),//h / 2 - 20,
                    (int)avgsize.Width,
                    (int)avgsize.Height)
                );
            gBmp.DrawRectangle(new Pen(Color.Black), new Rectangle(
                    Convert.ToInt32(x),
                   (int)((h / 2) * 0.4),//h / 2 - 20,
                    (int)avgsize.Width,
                    (int)avgsize.Height));
            try
            {
                gBmp.DrawString("AVG:" + valAvr.ToString(),
                                drawFont,
                                new SolidBrush(Color.Red),
                                new Point(Convert.ToInt32(x),
                                    (int)((h / 2) * 0.4)//h / 2 - 20,
                                    )
                            );
            }
            catch (Exception)
            {

                //throw;
            }

            gBmp.DrawRectangle(new Pen(Color.Black),
                new Rectangle(
                    0,
                    0,
                    width - 5,
                   height - 5)
                );
            try
            {
                gBmp.DrawString(valText,
               drawFont,
               new SolidBrush(Color.Black),
                 new Rectangle(
                   1,
                    offseth + Convert.ToInt32((h / 2) - (valueh / 2)),
                    offsetw,
                    offseth + Convert.ToInt32(valueh))
                , strFormat
           );

            }
            catch (Exception)
            {

                //throw;
            }
            //
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
    }
}
