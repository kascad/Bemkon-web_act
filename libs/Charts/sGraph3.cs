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
    public class sGraph3
    {

        int width;
        int height;
        double valMax;
        double valMin;
        double valStep = 0;
        List<percentil> datalist = null;
        public sGraph3(int width, int height, List<percentil> data, double valStep)
        {
            this.width = width;
            this.height = height;
            this.datalist = data;
            this.valStep = valStep;
        }
        public void saveImage(string path)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics gBmp = System.Drawing.Graphics.FromImage(bmp);
            Brush backBrush = new SolidBrush(Color.Gray);
            Brush textBrush = new SolidBrush(Color.Black);
            int w, h, offseth, offsetw, valueh;
            offsetw = 50;
            offseth = 90;
            valMax = 0;
            double reserv = 20;
            w = (int)(width - 10) - offsetw;
            h = (int)(height - offseth - reserv);
            valueh = (int)(h * 0.4);
            
            Font drawFont = new Font("Microsoft Sans Serif", 13);
            Font drawFont2 = new Font("Microsoft Sans Serif", 8);

            double someMin = 0;
            foreach (var item in datalist)
            {
                if (valMax <= item.value)
                    valMax = item.value;
                if (valMin >= item.value)
                    valMin = item.value;
            }

            if (valMin < someMin)
                someMin = valMin;

            double k = (h) / (valMax - someMin);
            SizeF sf = gBmp.MeasureString(someMin.ToString("N1"), drawFont);
            if (valStep == 0)
            {
                valStep = (valMax - someMin) / ((h) / (sf.Height + 10));
            }
            valStep = (valStep >= 1 ? valStep : 1);
            gBmp.DrawString("Индексы", drawFont, textBrush, w / 3, offseth / 3);
            gBmp.DrawString("В пространстве: " + datalist[0].name + " - " + datalist[datalist.Count - 1].name, drawFont, textBrush, w / 3, offseth / 3 + 25);
            gBmp.FillRectangle(backBrush, new Rectangle(offsetw, offseth, w, h));

            float x = 0;
            float y = 0;

            gBmp.SmoothingMode = SmoothingMode.AntiAlias;
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            //fill lines
            double valX = 0;
            for (double i = someMin; i <= valMax; i += (valStep))
            {
                if (i + valStep > valMax)
                    valX = valMax;
                else
                    valX = i;
                double texth = (gBmp.MeasureString((valX).ToString("N1") + "", new Font("Microsoft Sans Serif", 10)).Height);
                y = (float)(((valMax - valX)) * k) + offseth;
                gBmp.DrawLine(new Pen(Color.Black),
                    new PointF(offsetw, y),
                    new PointF(w + offsetw, y));
                try
                {
                    gBmp.DrawString((valX).ToString("N1"), new Font("Microsoft Sans Serif", 10), textBrush, 0, (float)(y - texth / 2));
                }
                catch (Exception)
                {

                    //throw;
                }
            }
            int j = 1;
            int blockWidth = (int)((w - offsetw) / datalist.Count);
            if (blockWidth > 50) blockWidth = 50;
            else if (blockWidth < 10)
                blockWidth = 10;
            float offsetdiff = ((w - offsetw - blockWidth / 2) / datalist.Count);
            foreach (var item in datalist)
            {
                x = (offsetw + offsetw / 4 + (j - 1) * (offsetdiff + offsetw));
                if (item.value < 0)
                {
                    y = (float)(((valMax - 0)) * k) + offseth;
                    gBmp.FillRectangle(new SolidBrush(Color.PowderBlue),
                new Rectangle(
                    (int)x,
                    (int)y,
                    (int)blockWidth,
                    (int)Math.Abs((((item.value)) * k))
                    )
                );
                }
                else
                {
                    y = (float)(((valMax - item.value)) * k) + offseth;

                    gBmp.FillRectangle(new SolidBrush(Color.PowderBlue),
                new Rectangle(
                     (int)x,
                    (int)y,
                    (int)blockWidth,
                    (int)Math.Abs((((item.value)) * k))
                    ));
                }
                //SizeF sf1 = gBmp.MeasureString(item.name,drawFont);
                //gBmp.DrawString(item.name, drawFont, textBrush, new PointF((offsetw + blockWidth + (j - 1) * (w / (datalist.Count - 1) - offsetw - blockWidth)), (float)(((valMax - 0)) * k) + offseth));
                SizeF sf1 = gBmp.MeasureString(item.name, drawFont);
                gBmp.DrawString(item.name, drawFont2, textBrush, new RectangleF(new PointF(x - sf1.Width / 3, (float)(((valMax - 0)) * k) + offseth), new SizeF(200, 100)));
                j++;

            }
            drawZeroLine(gBmp, (float)w, (float)k, (float)offsetw, (float)offseth);
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
