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
    public class sGraph4
    {

        int width;
        int height;
        double valMax;
        double valMin;
        double valStep = 0;
        List<percentil> datalist = null;
        public sGraph4(int width, int height, List<percentil> data, double valStep)
        {
            this.width = width;
            this.height = height;
            this.datalist = data;
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
        public void saveImage(string path)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics gBmp = System.Drawing.Graphics.FromImage(bmp);
            Brush backBrush = new SolidBrush(Color.Gray);
            Brush textBrush = new SolidBrush(Color.Black);
            int w, h, offseth, offsetw;
            offsetw = 50;
            offseth = 90;
            valMax = 0;
            double reserv = 20;
            w = (int)(width - 10) - offsetw;
            h = (int)(height - offseth - reserv);
            //valueh = (int)(h * 0.4);
            Font drawFont = new Font("Tahoma", 13);
            //double someMin = 0;



            //gBmp.DrawLine(new Pen(Color.Black), cPoint, getLinePoint(h / 2, cPoint, 90));
            foreach (var item in datalist)
            {
                if (valMax <= item.value)
                    valMax = item.value;
                if (valMin >= item.value)
                    valMin = item.value;
            }

            //if (valMin < someMin)
            //    someMin = valMin;
            Font digFont = new Font("Tahoma", 7, FontStyle.Regular);
            PointF cPoint = new PointF(offsetw + w / 2, offseth * 1.5f + h / 2);
            PointF lastPoint = new PointF();
            gBmp.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, width, height));
            gBmp.FillRectangle(backBrush, new Rectangle(offsetw, offseth, w, h));
            gBmp.DrawString("Индексы", drawFont, textBrush, w / 3, offseth / 3);
            gBmp.DrawString("Базовые шкалы: " + datalist[0].name + " - " + datalist[datalist.Count - 1].name, drawFont, textBrush, w / 3, offseth / 3 + 25);
            PointF newpoint = new PointF();
            gBmp.DrawString("Базовые шкалы: " + datalist[0].name + " - " + datalist[datalist.Count - 1].name, drawFont, textBrush, w / 3, offseth / 3 + 20);
            for (int i = 0; i < 3; i++)
            {
                gBmp.DrawLine(new Pen(Color.Black), cPoint, getLinePoint(h / 2, cPoint, 90 + i * 120));
                newpoint = getLinePoint(h / 2 + 20, cPoint, 90 + i * 120);
                if (i == 0)
                    gBmp.DrawString(datalist[i].name, drawFont, textBrush, new PointF(newpoint.X - gBmp.MeasureString(datalist[i].name, drawFont).Width / 2, newpoint.Y - gBmp.MeasureString(datalist[i].name, drawFont).Height / 2));
                else if (i == 1)
                    gBmp.DrawString(datalist[i].name, drawFont, textBrush, new PointF(offsetw, newpoint.Y - gBmp.MeasureString(datalist[i].name, drawFont).Height / 2));
                else if (i == 2)
                    gBmp.DrawString(datalist[i].name, drawFont, textBrush, new PointF(newpoint.X - gBmp.MeasureString(datalist[i].name, drawFont).Width / 2 - 10, newpoint.Y - gBmp.MeasureString(datalist[i].name, drawFont).Height / 2));
            }
            double k = (h / 2) / (valMax - valMin);
            if (valStep == 0)
            {
                valStep = (valMax - valMin) / ((h) / (10));
            }
            valStep = (valStep >= 1 ? valStep : 1);
            double valX = 0;
            for (double j = valMin; j <= valMax; j += valStep)
            {
                //lastPoint = new PointF(getLocalX(((valMax ) - j) * k, cPoint, 90), getLocalY(((valMax ) - j) * k, cPoint, 90));//getLinePoint(j * k, cPoint, 90 + 0 * 120);
                if (j + valStep > valMax)
                    valX = valMax;
                else
                    valX = j;
                lastPoint = getLinePoint(Math.Abs(valMin - valX) * k, cPoint, 90);
                SizeF textf = (gBmp.MeasureString((valX).ToString("N1") + "", digFont));
                gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, lastPoint.X - textf.Width, lastPoint.Y - textf.Height / 2);
                for (int i = 1; i < 4; i++)
                {
                    gBmp.DrawLine(new Pen(Color.Black), lastPoint, getLinePoint(Math.Abs(valMin - valX) * k, cPoint, 90 + i * 120));
                    lastPoint = getLinePoint(Math.Abs(valMin - valX) * k, cPoint, 90 + i * 120);

                }

            }
            int ik = 0;
            //return;
            //float realValue = 0;
            newpoint = new PointF(0, 0);
            foreach (var item in datalist)
            {

                if (newpoint.X == 0)
                    newpoint = getLinePoint(Math.Abs(valMin - item.value) * k, cPoint, 90 + ik * 120);
                else
                {
                    gBmp.DrawLine(new Pen(Color.Red), newpoint, getLinePoint(Math.Abs(valMin - item.value) * k, cPoint, 90 + ik * 120));
                    newpoint = getLinePoint(Math.Abs(valMin - item.value) * k, cPoint, 90 + ik * 120);
                }
                //gBmp.DrawEllipse(new Pen(Color.Red), new RectangleF(new PointF(newpoint.X-5,newpoint.Y-5), new SizeF(5, 5)));

                ik++;
            }
            gBmp.DrawLine(new Pen(Color.Red), newpoint, getLinePoint(Math.Abs(valMin - datalist[0].value) * k, cPoint, 90 + ik * 120));

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

}
