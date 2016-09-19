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
    public class sGraph6
    {

        int width;
        int height;
        double valMaxX;
        double valMaxY;
        double valMinX;
        double valMinY;
        double valStepX = 0;
        double valStepY = 0;
        List<percentilxy> datalist = null;
        List<string> namefield = new List<string>();
        public sGraph6(int width, int height, List<percentilxy> data, List<string> data2, double valStepX, double valStepY)
        {
            this.width = width;
            this.height = height;
            this.datalist = data;
            this.namefield = data2;
            this.valStepX = valStepX;
            this.valStepY = valStepY;
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
            res.X = (float)getX(valx, valMinX, kx, offsetw);
            res.Y = (float)getY(valy, valMaxY, ky, offseth);
            return res;
        }
        double getX(double val, double valMin, double k, double offset)
        {
            return ((val - valMin) * k + offset);
        }
        double getY(double val, double valMax, double k, double offset)
        {
            return ((valMax - val) * k + offset);
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
            valMaxX = 0;
            valMaxY = 0;
            valMinX = 0;
            valMinY = 0;
            foreach (var item in datalist)
            {
                if (valMaxX <= item.valuex)
                    valMaxX = item.valuex;
                if (valMaxY <= item.valuey)
                    valMaxY = item.valuey;
                if (valMinX > item.valuex)
                    valMinX = item.valuex;
                if (valMinY > item.valuey)
                    valMinY = item.valuey;
            }
            valMinY -= valStepY;
            valMinX -= valStepX;
            valMaxX += valStepX;
            valMaxY += valStepY;
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

            if (namefield.Count == 2)
            {
                namefield.Add(null);
                namefield.Add(null);
            }
            */

            Font digFont = new Font("Tahoma", 7, FontStyle.Regular);
            gBmp.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, width, height));
            gBmp.FillRectangle(backBrush, new Rectangle(offsetw, offseth, w, h));
            gBmp.DrawString("Расположение понятий в пространстве " + namefield[0] + "-" + namefield[1] + " vs " + namefield[2] + "-" + namefield[3], drawFont, textBrush, 10, offseth / 3);
            //gBmp.DrawString("Расположение понятий в пространстве " + datalist[0].name + "-" + datalist[1].name + " vs " + datalist[2].name + "-" + datalist[3].name, drawFont, textBrush, 10, offseth / 3);
            gBmp.DrawString("Измерение: индексы", drawFont, textBrush, w / 3, offseth / 3 + 25);

            double kx = (w) / (valMaxX - valMinX);
            double ky = (h) / (valMaxY - valMinY);
            if (valStepX == 0)
            {
                valStepX = (valMaxX - valMinX) / ((w) / (10));
            }
            if (valStepY == 0)
            {
                valStepY = (valMaxY - valMinY) / ((h) / (10));
            }
            valStepX = (valStepX >= 1 ? valStepX : 1);
            valStepY = (valStepY >= 1 ? valStepY : 1);

            float y = 0;
            double valX = 0;
            drawZeroLineX(gBmp, (float)w, (float)ky, (float)offsetw, (float)offseth, (float)valMaxY);
            float x = 0;
            for (double i = valMinX; i <= valMaxX; i += (valStepX))
            {
                double textw = (gBmp.MeasureString((valMaxX - i).ToString("N0"), new Font("Microsoft Sans Serif", 10)).Width);
                if (i + valStepX > valMaxX)
                    valX = valMaxX;
                else
                    valX = i;
                y = (float)((valMaxY) * ky + offseth);
                //x = (float)((valX-valMinX) * kx + offsetw);
                x = (float)getX(valX, valMinX, kx, offsetw);
                gBmp.DrawLine(new Pen(Color.Black),
                    new PointF(x, y - 5),
                    new PointF(x, y + 5));
                try
                {
                    gBmp.DrawString((i).ToString("N0"), new Font("Microsoft Sans Serif", 10), textBrush, (float)(x - textw / 2), (float)(y));
                }
                catch (Exception)
                {

                    //throw;
                }
            }
            double offsetx = getX(0, valMinX, kx, offsetw);
            for (double i = valMinY; i <= valMaxY; i += (valStepY))
            {
                if (i + valStepY > valMaxY)
                    valX = valMaxY;
                else
                    valX = i;
                double texth = (gBmp.MeasureString((i).ToString("N0"), new Font("Microsoft Sans Serif", 10)).Height);
                y = (float)(((valMaxY - valX)) * ky) + offseth;
                gBmp.DrawLine(new Pen(Color.Black),
                    new PointF((float)offsetw, y),
                    new PointF(w + offsetw, y));
                try
                {
                    gBmp.DrawString((valX).ToString("N0"), new Font("Microsoft Sans Serif", 10), textBrush, (float)offsetx - 20, (float)(y - (texth / 2) - 5));
                }
                catch (Exception)
                {

                    //throw;
                }
            }
            gBmp.DrawLine(new Pen(Color.Black),
                    new PointF((float)offsetx, (float)offseth),
                    new PointF((float)offsetx, (float)h + offseth));

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
        void drawZeroLineX(System.Drawing.Graphics gBmp, float w, float k, float offsetw, float offseth, double valmax)
        {
            float y = (float)(((valmax - 0)) * k) + offseth;
            //float y = (float)(((valmax - 0)) * k) + offseth;
            gBmp.DrawLine(new Pen(Color.Green, 2),
                    new PointF(offsetw, y),
                    new PointF(w + offsetw, y));
        }
    }

}
