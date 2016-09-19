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
    public class sGraph8
    {

        int width;

        int height;
        double valMax;
        double valMin;
        double valMaxX;
        double valMaxY;
        double valMinX;
        double valMinY;
        double valMaxZ;
        double valMinZ;
        double valStepX = 0;
        double valStepY = 0;
        double valStepZ = 0;
        double valStep = 0;
        double expInf = 0.0000000000001;
        List<percentilxyz8> datalist = null;
        List<string> scales = null;
        List<string> scalesname = null;
        int mode;
        public sGraph8(int width, int height, List<percentilxyz8> data, double valStep, List<string> scales, List<string> scales2,  int mode)
        {
            this.width = width;
            this.height = height;
            this.datalist = data;
            //this.valStepX = valStepX ;
            //this.valStepY = valStepY;
            //this.valStepZ = valStepZ;
            this.valStep = valStep;
            this.scales = scales;
            this.scalesname = scales2;
            this.mode = mode;
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
            List<double> angles = new List<double>();
            List<double> nangles = new List<double>();
            double xa, ya, za;
            double nxa, nya, nza;
            xa = 0; ya = 90 + 90 + 45; za = 90;
            nxa = 180; nya = 45; nza = 90 * 3;
            angles.Add(xa); angles.Add(ya); angles.Add(za);
            nangles.Add(nxa); nangles.Add(nya); nangles.Add(nza);
            offsetw = 50;
            offseth = 90;

            //valMaxZ = (mode==1 ? 48.5 : 100);
            //valMaxX = (mode == 1 ? 49 : 100);
            //valMaxY = (mode == 1 ? 25.5 : 100);
            //valMinZ = -14.4;
            //valMinX = -14.5;
            //valMinY = -14.6;
            //valMin = -14.6;
            valMaxZ = (mode == 1 ? 0 : 100);
            valMaxX = (mode == 1 ? 0 : 100);
            valMaxY = (mode == 1 ? 0 : 100);

            valMax = (mode == 1 ? 0 : 100);

            valMinZ = 0;
            valMinX = 0;
            valMinY = 0;
            valMin = 0;
            double reserv = 20;
            w = (int)(width - 10) - offsetw;
            h = (int)(height - offseth - reserv);
            //valueh = (int)(h * 0.4);
            Font drawFont = new Font("Tahoma", 7);
            Font labelFont = new Font("Tahoma", 13, FontStyle.Bold);
            //double someMin = 0;



            //gBmp.DrawLine(new Pen(Color.Black), cPoint, getLinePoint(h / 2, cPoint, 90));
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
                if (valMaxZ <= item.valuez)
                    valMaxZ = item.valuez;
                if (valMinZ > item.valuez)
                    valMinZ = item.valuez;

                if (item.valuex < valMin)
                    valMin = item.valuex;
                if (item.valuey < valMin)
                    valMin = item.valuey;
                if (item.valuez < valMin)
                    valMin = item.valuez;

                if (item.valuex > valMax)
                    valMax = item.valuex;
                if (item.valuey > valMax)
                    valMax = item.valuey;
                if (item.valuez > valMax)
                    valMax = item.valuez;
            }

            //valMinY -= valStepY;
            //valMinX -= valStepX;
            //valMaxX += valStepX;
            //valMaxY += valStepY;
            //valMaxZ += valStepZ;
            //valMinZ -= valStepZ;
            //if (valMin < someMin)
            //    someMin = valMin;

            PointF lastPoint = new PointF();
            gBmp.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, width, height));

            gBmp.FillRectangle(backBrush, new Rectangle(offsetw, offseth, w, h + (int)reserv));
            //gBmp.DrawString("Расположение понятий в пространстве " + scales[0] + "-" + scales[1] + " " + scalesname[0] + "-" + scalesname[1] + " vs " + scales[2] +  "-" + scales[3] + " " + scalesname[2] + "-" + scalesname[3] + " vs " + scales[4] + "-" + scales[5] + " " + scalesname[4] + "-" + scalesname[5], labelFont, textBrush, 10, offseth / 4);
            gBmp.DrawString("Расположение понятий в пространстве " + scalesname[0] + "-" + scalesname[1] + " vs " + scalesname[2] + "-" + scalesname[3] + " vs " + scalesname[4] + "-" + scalesname[5], labelFont, textBrush, 10, offseth / 4);

            h -= (int)reserv * 2;
            Font digFont = new Font("Tahoma", 7, FontStyle.Regular);
            PointF cPoint = new PointF(offsetw + w / 2, offseth + (offseth + h) / 2);
            if (mode == 1)
                gBmp.DrawString("Индексы", labelFont, textBrush, w / 3, offseth / 2);
            if (mode == 2)
                gBmp.DrawString("Процентили", labelFont, textBrush, w / 3, offseth / 2);
            PointF newpoint;

            newpoint = getLinePoint(h / 2, cPoint, xa);
            Pen dashedPen = new Pen(Color.Black, 2);
            dashedPen.DashStyle = DashStyle.Dash;
            gBmp.DrawLine(new Pen(Color.Black, 2), cPoint, newpoint);
            gBmp.DrawLine(dashedPen, cPoint, getLinePoint(h / 2, cPoint, nxa));
            gBmp.DrawString(scales[0] + "-" + scales[1], labelFont, textBrush, new PointF(newpoint.X, newpoint.Y - gBmp.MeasureString(scales[0] + "-" + scales[1], labelFont).Height / 2));

            newpoint = getLinePoint(h / 2, cPoint, ya);
            gBmp.DrawString(scales[2] + "-" + scales[3], labelFont, textBrush, new PointF(newpoint.X - gBmp.MeasureString(scales[2] + "-" + scales[3], labelFont).Width / 2, newpoint.Y + 5));
            gBmp.DrawLine(new Pen(Color.Black, 2), cPoint, newpoint);
            gBmp.DrawLine(dashedPen, cPoint, getLinePoint(h / 2, cPoint, nya));

            newpoint = getLinePoint(h / 2, cPoint, za);
            gBmp.DrawLine(new Pen(Color.Black, 2), cPoint, newpoint);
            gBmp.DrawLine(dashedPen, cPoint, getLinePoint(h / 2, cPoint, nza));
            gBmp.DrawString(scales[4] + "-" + scales[5], labelFont, textBrush, new PointF(newpoint.X - gBmp.MeasureString(scales[4] + "-" + scales[5], labelFont).Width / 2, newpoint.Y - gBmp.MeasureString(scales[4] + "-" + scales[5], labelFont).Height * 2));

            double k = (h / 2) / Math.Abs(valMax + expInf);
            double ak = (h / 2) / Math.Abs(valMin + expInf);
            double kx = (h / 2) / (valMaxX - valMinX + expInf);
            double ky = (h / 2) / (valMaxY - valMinY + expInf);
            double kz = (h / 2) / (valMaxZ - valMinZ + expInf);
            double nvalStep = 0;
            if (valStep == 0)
            {
                valStep = Math.Abs(valMax) / 5;
                nvalStep = Math.Abs(valMin) / 5;
            }
            else nvalStep = valStep;


            if (valStepX == 0)
            {
                valStepX = (valMaxX - valMinX) / 5;
            }
            if (valStepY == 0)
            {
                valStepY = (valMaxY - valMinY) / 5;
            }
            if (valStepZ == 0)
            {
                valStepZ = (valMaxZ - valMinZ) / 5;
            }
            //valStepX = (valStepX >= 1 ? valStepX : 1);
            //valStepY = (valStepY >= 1 ? valStepY : 1);
            //valStepZ = (valStepZ >= 1 ? valStepZ : 1);
            double valX = 0;
            //
            //lastPoint = getLinePoint(Math.Abs(valMinX) * kx, cPoint, xa);
            //gBmp.DrawLine(new Pen(Color.Black, 2), getLinePoint(3, lastPoint, za), getLinePoint(3, lastPoint, za * 3));
            SizeF textf = (gBmp.MeasureString((0).ToString("N1") + "", digFont));
            gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, cPoint.X - textf.Width / 2, cPoint.Y + textf.Height / 2);
            gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, cPoint.X, cPoint.Y);
            gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, cPoint.X - textf.Width, cPoint.Y - textf.Height / 2);
            float radius = 2;
            for (double j = 0; j <= valMax; j += valStep)
                for (int i = 0; i < angles.Count; i++)
                {
                    if (j + valStep > valMax)
                        valX = valMax;
                    else
                        valX = j;
                    lastPoint = getLinePoint(Math.Abs(valX) * k, cPoint, angles[i]);
                    //gBmp.DrawLine(new Pen(Color.Black, 2), getLinePoint(3, lastPoint, za), getLinePoint(3, lastPoint, za * 3));
                    gBmp.FillEllipse(new SolidBrush(Color.Black), new RectangleF(
                   new PointF(
                       Math.Abs(lastPoint.X) - radius,
                       Math.Abs(lastPoint.Y) - radius),
                       new SizeF(radius * 2, radius * 2)));
                    textf = (gBmp.MeasureString((valX).ToString("N1") + "", digFont));
                    gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, lastPoint.X - textf.Width / 2, lastPoint.Y + textf.Height / 2);
                }
            for (double j = valMin; j < 0; j += nvalStep)
                for (int i = 0; i < nangles.Count; i++)
                {
                    if (j + nvalStep > 0)
                        valX = 0;
                    else
                        valX = j;
                    valX = j;
                    lastPoint = getLinePoint(Math.Abs(valX) * ak, cPoint, nangles[i]);
                    //gBmp.DrawLine(new Pen(Color.Black, 2), getLinePoint(3, lastPoint, za), getLinePoint(3, lastPoint, za * 3));
                    gBmp.FillEllipse(new SolidBrush(Color.Black), new RectangleF(
                   new PointF(
                       Math.Abs(lastPoint.X) - radius,
                       Math.Abs(lastPoint.Y) - radius),
                       new SizeF(radius * 2, radius * 2)));
                    textf = (gBmp.MeasureString((valX).ToString("N1") + "", digFont));
                    gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, lastPoint.X - textf.Width / 2, lastPoint.Y + textf.Height / 2);
                }
            //if (valMaxX - valMinX >0)
            //for (double j = valMinX; j <= valMaxX; j += valStepX)
            //{
            //    if (j + valStepX > valMaxX)
            //        valX = valMaxX;
            //    else
            //        valX = j;
            //    lastPoint = getLinePoint(Math.Abs(valMinX - valX) * kx, cPoint, xa);
            //    gBmp.DrawLine(new Pen(Color.Black,2), getLinePoint(3, lastPoint, za), getLinePoint(3, lastPoint, za*3));
            //    SizeF textf = (gBmp.MeasureString((valX).ToString("N1") + "", digFont));
            //    gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, lastPoint.X- textf.Width/2, lastPoint.Y + textf.Height / 2);
            //}
            //if (valMaxY - valMinY > 0)
            //for (double j = valMinY; j <= valMaxY; j += valStepY)
            //{
            //    if (j + valStepY > valMaxY)
            //        valX = valMaxY;
            //    else
            //        valX = j;
            //    lastPoint = getLinePoint(Math.Abs(valMinY - valX) * ky, cPoint, ya);
            //    gBmp.DrawLine(new Pen(Color.Black,2), getLinePoint(3, lastPoint, 135), getLinePoint(3, lastPoint, 330));
            //    SizeF textf = (gBmp.MeasureString((valX).ToString("N1") + "", digFont));
            //    gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, lastPoint.X, lastPoint.Y );
            //}
            //if (valMaxZ - valMinZ > 0)
            //for (double j = valMinZ; j <= valMaxZ; j += valStepZ)
            //{
            //    if (j + valStepZ > valMaxZ)
            //        valX = valMaxZ;
            //    else
            //        valX = j;
            //    lastPoint = getLinePoint(Math.Abs(valMinZ - valX) * kz, cPoint, za);
            //    gBmp.DrawLine(new Pen(Color.Black,2), getLinePoint(3, lastPoint, 180), getLinePoint(3, lastPoint, 0));
            //    SizeF textf = (gBmp.MeasureString((valX).ToString("N1") + "", digFont));
            //    gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, lastPoint.X - textf.Width, lastPoint.Y - textf.Height / 2);
            //}
            PointF p1, p2, p3, p12, p23, p31, p123;
            Color c = Color.Red;
            radius = 4;
            Font fieldFont = new Font("Tahoma", 10);
            foreach (var item in datalist)
            {
                if (item.valuex >= 0)
                    p1 = getLinePoint(Math.Abs(item.valuex) * k, cPoint, xa);
                else p1 = getLinePoint(Math.Abs(item.valuex) * k, cPoint, nxa);
                //drawPoint(gBmp, p1, 4, Color.Green);
                if (item.valuey >= 0)
                    p2 = getLinePoint(Math.Abs(item.valuey) * k, p1, ya);
                else p2 = getLinePoint(Math.Abs(item.valuey) * k, p1, nya);
                //drawPoint(gBmp, p2, 4, Color.Yellow);
                if (item.valuez >= 0)
                    p3 = getLinePoint(Math.Abs(item.valuez) * k, p2, za);
                else p3 = getLinePoint(Math.Abs(item.valuez) * k, p2, nza);
                p123 = p3;
                gBmp.FillEllipse(new SolidBrush(c), new RectangleF(
                   new PointF(
                       Math.Abs(p123.X) - radius,
                       Math.Abs(p123.Y) - radius),
                       new SizeF(radius * 2, radius * 2)));
                gBmp.DrawEllipse(new Pen(Color.Red), new RectangleF(
                    new PointF(
                        Math.Abs(p123.X) - radius,
                        Math.Abs(p123.Y) - radius),
                        new SizeF(radius * 2, radius * 2)));
                gBmp.DrawString(item.name, fieldFont, new SolidBrush(Color.Yellow), new PointF(
                        Math.Abs(p123.X) + radius,
                        Math.Abs(p123.Y) - radius));

            }
            //{
            //    //c = CreateRandomColor(c);
            //    //p1 = getLinePoint(Math.Abs(valMinX - item.valuex) * kx, cPoint, xa);
            //    //p2 = getLinePoint(Math.Abs(valMinY - item.valuey) * ky, cPoint, ya);
            //    //p3 = getLinePoint(Math.Abs(valMinZ - item.valuez) * kz, cPoint, za);
            //    //gBmp.DrawLine(new Pen(c), p1, p2);
            //    //gBmp.DrawLine(new Pen(c), p2, p3);
            //    //gBmp.DrawLine(new Pen(c), p3, p1);
            //    //p12 = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            //    //p23 = new PointF((p2.X + p3.X) / 2, (p2.Y + p3.Y) / 2);
            //    //p31 = new PointF((p3.X + p1.X) / 2, (p3.Y + p1.Y) / 2);
            //    ////gBmp.DrawLine(new Pen(c), p12, p3);
            //    ////gBmp.DrawLine(new Pen(c), p23, p1);
            //    ////gBmp.DrawLine(new Pen(c), p31, p2);
            //    //p123 = getXYCross(p1.X, p23.X, p2.X, p31.X, p1.Y, p23.Y, p2.Y, p31.Y);
            //    //if (Math.Abs(p123.X) == cPoint.X && Math.Abs(p123.Y) == cPoint.Y)
            //    //    p123 = getXYCross(p3.X, p12.X, p2.X, p31.X, p3.Y, p12.Y, p2.Y, p31.Y);
            //    //p123 = getXYCross(p3.X, p12.X, p1.X, p23.X, p3.Y, p12.Y, p1.Y, p23.Y);
            //    //p123 = getXYCross(p3,p12,p1,p23);
            //    p1 = getLinePoint(Math.Abs(valMinX - item.valuex) * kx, cPoint, xa);
            //    gBmp.FillEllipse(new SolidBrush(Color.Green), new RectangleF(
            //       new PointF(
            //           Math.Abs(p1.X) - radius,
            //           Math.Abs(p1.Y) - radius),
            //           new SizeF(radius * 2, radius * 2)));
            //    gBmp.DrawEllipse(new Pen(Color.Red), new RectangleF(
            //        new PointF(
            //            Math.Abs(p1.X) - radius,
            //            Math.Abs(p1.Y) - radius),
            //            new SizeF(radius * 2, radius * 2)));
            //    p2 = getLinePoint(Math.Abs(valMinZ - item.valuez) * kz, p1, za);
            //    gBmp.FillEllipse(new SolidBrush(Color.Green), new RectangleF(
            //       new PointF(
            //           Math.Abs(p2.X) - radius,
            //           Math.Abs(p2.Y) - radius),
            //           new SizeF(radius * 2, radius * 2)));
            //    gBmp.DrawEllipse(new Pen(Color.Red), new RectangleF(
            //        new PointF(
            //            Math.Abs(p2.X) - radius,
            //            Math.Abs(p2.Y) - radius),
            //            new SizeF(radius * 2, radius * 2)));
            //    p3 = getLinePoint(Math.Abs(valMinY - item.valuey) * ky, p2, ya);

            //    p123 = p3;
            //    gBmp.FillEllipse(new SolidBrush(c), new RectangleF(
            //       new PointF(
            //           Math.Abs(p123.X) - radius,
            //           Math.Abs(p123.Y) - radius),
            //           new SizeF(radius * 2, radius * 2)));
            //    gBmp.DrawEllipse(new Pen(Color.Red), new RectangleF(
            //        new PointF(
            //            Math.Abs(p123.X) - radius,
            //            Math.Abs(p123.Y) - radius),
            //            new SizeF(radius * 2, radius * 2)));
            //    gBmp.DrawString(item.name, fieldFont, new SolidBrush(Color.Yellow), new PointF(
            //            Math.Abs(p123.X) + radius,
            //            Math.Abs(p123.Y) - radius));

            //    //-------
            //    p1 = getLinePoint(Math.Abs(valMinX - item.valuex) * kx, cPoint, xa);
            //    gBmp.FillEllipse(new SolidBrush(Color.Green), new RectangleF(
            //       new PointF(
            //           Math.Abs(p1.X) - radius,
            //           Math.Abs(p1.Y) - radius),
            //           new SizeF(radius * 2, radius * 2)));
            //    gBmp.DrawEllipse(new Pen(Color.Red), new RectangleF(
            //        new PointF(
            //            Math.Abs(p1.X) - radius,
            //            Math.Abs(p1.Y) - radius),
            //            new SizeF(radius * 2, radius * 2)));
            //    p2 = getLinePoint(Math.Abs(valMinY - item.valuey) * ky, p1, ya);
            //    gBmp.FillEllipse(new SolidBrush(Color.Yellow), new RectangleF(
            //       new PointF(
            //           Math.Abs(p2.X) - radius,
            //           Math.Abs(p2.Y) - radius),
            //           new SizeF(radius * 2, radius * 2)));
            //    gBmp.DrawEllipse(new Pen(Color.Yellow), new RectangleF(
            //        new PointF(
            //            Math.Abs(p2.X) - radius,
            //            Math.Abs(p2.Y) - radius),
            //            new SizeF(radius * 2, radius * 2)));
            //    p3 = getLinePoint(Math.Abs(valMinZ - item.valuez) * kz, p2, za);

            //    p123 = p3;
            //    gBmp.FillEllipse(new SolidBrush(Color.Blue), new RectangleF(
            //       new PointF(
            //           Math.Abs(p123.X) - radius,
            //           Math.Abs(p123.Y) - radius),
            //           new SizeF(radius * 2, radius * 2)));
            //    gBmp.DrawEllipse(new Pen(Color.Red), new RectangleF(
            //        new PointF(
            //            Math.Abs(p123.X) - radius,
            //            Math.Abs(p123.Y) - radius),
            //            new SizeF(radius * 2, radius * 2)));
            //    gBmp.DrawString(item.name, fieldFont, new SolidBrush(Color.Yellow), new PointF(
            //            Math.Abs(p123.X) + radius,
            //            Math.Abs(p123.Y) - radius));

            //    //gBmp.DrawEllipse(new Pen(c), new RectangleF(new PointF((p12.X+p3.X)/2,(p12.Y+p3.Y)/2), new SizeF(5, 5)));
            //    //gBmp.DrawEllipse(new Pen(c), new RectangleF(new PointF((p23.X + p1.X) / 2, (p23.Y + p1.Y) / 2), new SizeF(5, 5)));
            //    //gBmp.DrawEllipse(new Pen(c), new RectangleF(new PointF((p31.X + p2.X) / 2, (p31.Y + p2.Y) / 2), new SizeF(5, 5)));
            //}
            //for (double j = valMin; j <=valMax ; j+=valStep)
            //{
            //    //lastPoint = new PointF(getLocalX(((valMax ) - j) * k, cPoint, 90), getLocalY(((valMax ) - j) * k, cPoint, 90));//getLinePoint(j * k, cPoint, 90 + 0 * 120);
            //    if (j + valStep > valMax)
            //        valX = valMax;
            //    else
            //        valX = j;
            //    lastPoint = getLinePoint(Math.Abs(valMin - valX) * k, cPoint, 90);
            //    SizeF textf = (gBmp.MeasureString((valX).ToString("N1") + "", digFont));
            //    gBmp.DrawString(valX.ToString("N1"), digFont, textBrush, lastPoint.X - textf.Width, lastPoint.Y - textf.Height / 2);
            //    for (int i = 1; i < 4; i++)
            //    {
            //        gBmp.DrawLine(new Pen(Color.Black), lastPoint, getLinePoint(Math.Abs(valMin - valX) * k, cPoint, 90 + i * 120));
            //        lastPoint = getLinePoint(Math.Abs(valMin - valX) * k, cPoint, 90 + i * 120);

            //    }

            //}
            //int ik = 0;
            ////return;
            ////float realValue = 0;
            //newpoint = new PointF(0, 0);
            //foreach (var item in datalist)
            //{

            //    if (newpoint.X == 0)
            //        newpoint = getLinePoint(Math.Abs(valMin - item.value) * k, cPoint, 90 + ik * 120);
            //    else
            //    {
            //        gBmp.DrawLine(new Pen(Color.Red), newpoint, getLinePoint(Math.Abs(valMin - item.value) * k, cPoint, 90 + ik * 120));
            //        newpoint = getLinePoint(Math.Abs(valMin - item.value) * k, cPoint, 90 + ik * 120);
            //    }
            //    //gBmp.DrawEllipse(new Pen(Color.Red), new RectangleF(new PointF(newpoint.X-5,newpoint.Y-5), new SizeF(5, 5)));

            //    ik++;
            //}
            //gBmp.DrawLine(new Pen(Color.Red), newpoint, getLinePoint(Math.Abs(valMin - datalist[0].value) * k, cPoint, 90 + ik * 120));

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
        void drawPoint(System.Drawing.Graphics gBmp, PointF p, float radius, Color c)
        {
            gBmp.FillEllipse(new SolidBrush(c), new RectangleF(
                   new PointF(
                       Math.Abs(p.X) - radius,
                       Math.Abs(p.Y) - radius),
                       new SizeF(radius * 2, radius * 2)));
            gBmp.DrawEllipse(new Pen(c), new RectangleF(
                new PointF(
                    Math.Abs(p.X) - radius,
                    Math.Abs(p.Y) - radius),
                    new SizeF(radius * 2, radius * 2)));
        }
        PointF getXYCross(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float x, y;
            PointF p = new PointF();
            x = ((p1.X * p2.Y - p2.X * p1.Y) * (p4.X - p3.X) - (p3.X * p4.Y - p4.X * p3.Y) * (p2.X - p1.X)) / ((p1.Y - p2.Y) * (p4.X - p3.X) - (p3.Y - p4.Y) * (p2.X - p1.X));
            y = ((p3.Y - p4.Y) * x - (p3.X * p4.Y - p4.X * p3.Y)) / (p4.X - p3.X);
            p1.X = x;
            p.Y = y;
            return p;
        }
        PointF getXYCross(float x1, float x2, float x3, float x4, float y1, float y2, float y3, float y4)
        {
            float x, y;
            PointF p = new PointF();
            x = ((x1 * y2 - x2 * y1) * (x4 - x3) - (x3 * y4 - x4 * y3) * (x2 - x1)) / ((y1 - y2) * (x4 - x3) - (y3 - y4) * (x2 - x1));
            y = ((y3 - y4) * x - (x3 * y4 - x4 * y3)) / (x4 - x3);
            p.X = x;
            p.Y = y;
            return p;
        }
        private Color CreateRandomColor(Color oldcolor)
        {
            Random randonGen = new Random();
            Color randomColor;
            do
            {
                randomColor = Color.FromArgb(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
            } while (oldcolor == randomColor);


            return randomColor;
        }

        //void drawZeroLine(Graphics gBmp, float w,  float k, float offsetw, float offseth)
        //{
        //    float y = (float)(((valMax - 0)) * k) + offseth;
        //    gBmp.DrawLine(new Pen(Color.Green,2),
        //            new PointF(offsetw, y),
        //            new PointF(w + offsetw, y));
        //}
    }
    public class percentilxyz8
    {
        public percentilxyz8(string name, double valuex, double valuey, double valuez)
        {
            this.name = name;
            this.valuex = valuex;
            this.valuey = valuey;
            this.valuez = valuez;
        }
        public string name;
        public double valuex;
        public double valuey;
        public double valuez;
    }
}
