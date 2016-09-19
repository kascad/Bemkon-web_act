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
    public class sGraph2
    {

        int width;
        int height;
        double valMax;
        double valMin;
        double valStep;
        List<percentil> datalist = null;
        public sGraph2(int width, int height, List<percentil> data, double valStep)
        {
            this.width = width;
            this.height = height;
            this.datalist = data;
            this.valStep = valStep;
        }
        public void saveImage(string path)
        {
            Bitmap bmp = new Bitmap(width, height);
            System.Drawing.Graphics gBmp = System.Drawing.Graphics.FromImage(bmp);
            Brush backBrush = new SolidBrush(Color.Gray);
            Brush textBrush = new SolidBrush(Color.Black);
            int w, h, offseth, offsetw, valueh;
            offsetw = 50;
            offseth = 90;
            valMax = 100;
            valMin = 0;
            w = (int)(width - 10) - offsetw;
            h = (int)(height * 0.8) - offseth;
            valueh = (int)(h * 0.4);

            Font drawFont = new Font("Microsoft Sans Serif", 13);
            Font drawFont2 = new Font("Microsoft Sans Serif", 8);

            double someMin = 0;
            foreach (var item in datalist)
            {
                if (valMax <= Math.Abs(item.value))
                    valMax = Math.Abs(item.value);
                if (valMin >= Math.Abs(item.value))
                    valMin = Math.Abs(item.value);
            }

            if (valMin < someMin)
                someMin = valMin;

            double k = h / (valMax - someMin);
            if (valStep == 0)
            {
                SizeF sf = gBmp.MeasureString(someMin.ToString("N0"), drawFont);

                valStep = (valMax - someMin) / (h / (sf.Height + 10));
            }
            valStep = (valStep >= 1 ? valStep : 1);
            gBmp.DrawString("Процентили", drawFont, textBrush, w / 3, offseth / 3);
            gBmp.DrawString("В пространстве: " + datalist[0].name + " - " + datalist[datalist.Count - 1].name, drawFont, textBrush, w / 3, offseth / 3 + 25);

            gBmp.FillRectangle(backBrush, new Rectangle(offsetw, offseth, w, h));

            float x = 0;
            float y = 0;

            gBmp.SmoothingMode = SmoothingMode.AntiAlias;
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            double xstep = (w * valStep) / (valMax - someMin);
            //fill lines
            for (double i = someMin; i <= valMax - someMin; i += (valStep))
            {
                double texth = (gBmp.MeasureString((valMax - i).ToString("N0") + "%", new Font("Microsoft Sans Serif", 10)).Height);
                y = (float)((i - someMin) * k + offseth);
                gBmp.DrawLine(new Pen(Color.Black),
                    new PointF(offsetw, y),
                    new PointF(w + offsetw, y));
                try
                {
                    gBmp.DrawString((valMax - i).ToString("N0") + "%", new Font("Microsoft Sans Serif", 10), textBrush, 0, (float)(y - texth / 2));
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
            foreach (var item in datalist)
            {
                y = (float)((valMax - Math.Abs(item.value)) * k + offseth);
                gBmp.FillRectangle(new SolidBrush(Color.PowderBlue),
                new Rectangle(
                    (int)(offsetw + blockWidth + (j - 1) * (w / (datalist.Count - 1) - offsetw - blockWidth)),
                    (int)y,
                    (int)blockWidth,
                    (int)Math.Abs((item.value) * k))
                );
                SizeF sf = gBmp.MeasureString(item.name, drawFont);
                gBmp.DrawString(item.name, drawFont2, textBrush, new PointF((offsetw + blockWidth + (j - 1) * (w / (datalist.Count - 1) - offsetw - blockWidth)), offseth + h));
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
    public class percentil : IComparable<percentil>
    {
        public percentil()
        {   
        }
        public percentil(string name, double value)
        {
            this.name = name;
            this.value = value;
        }
        public string name;
        public double value;

        public int CompareTo(percentil psort)
        {
            return this.value.CompareTo(psort.value);
        }
    }

}
