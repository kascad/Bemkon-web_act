//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System;

namespace sGraph
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
        public sGraph(int width, int height, double valMax, double valMin, double valAvr, double valReal, double valStep, string valText)
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
            int w, h, offset, valueh;
            offset = 5;
            w = width - 20;
            h = height - 20;
            valueh = (int)(h * 0.8);
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
            double k = w/(valMax - someMin);
            if (valStep == 0)
            {
                SizeF sf = gBmp.MeasureString(someMin.ToString("N0"), drawFont);

                valStep = (valMax - someMin)/(w / (sf.Width + 10));
            }
            valStep = (valStep >= 1 ? valStep : 1);
            gBmp.FillRectangle(backBrush, new Rectangle(offset, offset, w, h - offset));
            
            float x = 0;
            float y = 0;
             
            gBmp.SmoothingMode = SmoothingMode.AntiAlias;
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;
            double lol = (double)(valMin * (valMax - valMin) / w);
            //double j = 0;
            
            double xstep = (w * valStep) / (valMax - valMin);
            for (double i = someMin; i <= valMax - valMin; i += (valStep))
            {
                //gBmp.DrawLine(new Pen(Color.Black),
                //    new Point((int)(i * (w / (valMax - valMin)) + (valMin < 0 ? valMin * (-1) : valMin)) + offset, offset),
                //    new Point((int)(i * (w / (valMax - valMin)) + (valMin < 0 ? valMin * (-1) : valMin)) + offset, h + offset));
                x = (float) ((i-someMin)*k + offset);
                gBmp.DrawLine(new Pen(Color.Black),
                    new Point((int)x, offset),
                    new Point((int)x, h + offset));
                
                //gBmp.tra
                y = h + offset;
                try
                {

                    gBmp.DrawString(i.ToString("N0"), new Font("Microsoft Sans Serif", 10), textBrush, x - (x > offset ? gBmp.MeasureString(i.ToString("N0"), new Font("Microsoft Sans Serif", 10)).Width / 2 : 0), y);
                }
                catch (Exception)
                {

                    //throw;
                }
                //j += xstep;
                //gBmp.DrawString(i.ToString(), drawFont, textBrush, new Point(Convert.ToInt32(i * (w / (valMax - valMin))), h + offset));
            }

            gBmp.DrawLine(new Pen(Color.Black), offset, h, w - offset, h);
            //
            //new Rectangle(offset,Convert.ToInt32(h / 2 - valueh / 2),
            x = (float)((valReal - someMin) * k + offset);
            gBmp.FillRectangle(new SolidBrush(Color.PowderBlue),
                new Rectangle(
                    offset,
                    offset + Convert.ToInt32((h / 2) - (valueh / 2)),
                    Convert.ToInt32(x),
                    offset + Convert.ToInt32(valueh))
                );
            gBmp.DrawRectangle(new Pen(Color.Black),
                new Rectangle(
                    offset,
                    offset + Convert.ToInt32((h / 2) - (valueh / 2)),
                    Convert.ToInt32(x),
                    offset + Convert.ToInt32(valueh))
                );
            try
            {
                gBmp.DrawString(valText,
               drawFont,
               new SolidBrush(Color.Red),
                new Rectangle(
                    offset,
                    offset + Convert.ToInt32((h / 2) - (valueh / 2)),
                    Convert.ToInt32(x),
                    offset + Convert.ToInt32(valueh)), strFormat
           );
            }
            catch (Exception)
            {

                //throw;
            }

            //avg
            x = (float)((valAvr - someMin) * k + offset);
            gBmp.DrawLine(new Pen(Color.Red),
                    new Point(Convert.ToInt32(x) , offset),
                    new Point(Convert.ToInt32(x) , h + offset));

            gBmp.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(
                    Convert.ToInt32(x) + offset + 5,
                   h / 2 - 20,
                    100,
                    50)
                );
            try
            {
                gBmp.DrawString("AVG:" + valAvr.ToString(),
                                drawFont,
                                new SolidBrush(Color.Red),
                                new Point(Convert.ToInt32(x) + offset + 5,
                                    h / 2)
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
