using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Charts
{
    public class CorectedBallsGraph
    {
        private double minValue;
        private double maxValue;
        private double[] values;
        private string[] textes;

        private int height;
        private const int rowHeight = 30;
        private const int width = 600;

        public CorectedBallsGraph(double minValue, double maxValue, double[] values, string[] textes)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.values = values;
            this.textes = textes;

            height = rowHeight * values.Length + 100;
        }

        public void saveImage(string path)
        {
            // Graph params
            var bottomLineY = height - 50;
            var startGraphX = 230;
            var vertLinesCount = 10;
            var hRowHeight = 30;
            var colGraphWidth = 40;
            
            var bmp = new Bitmap(width, height);
            var gBmp = System.Drawing.Graphics.FromImage(bmp);
            
            var backBrush = new SolidBrush(Color.White);
            var textBrush = new SolidBrush(Color.Black);
            var rowBrush = new SolidBrush(Color.Blue);

            var drawFont = new Font("Microsoft Sans Serif", 11);
            var pen = new Pen(textBrush);
            var boldPen = new Pen(textBrush, 2);


            gBmp.FillRectangle(backBrush, 0, 0, width, height);

            // Draw textes
            for (int i = 0; i < textes.Length; i++)
            {
                var text = GetCutStr(textes[i], 20);
                var txtPoint = new Point(10, hRowHeight * (i + 1));
                gBmp.DrawString(text, drawFont, textBrush, txtPoint);
            }
            
            // Draw lines
            for (int i = 0; i < vertLinesCount; i++)
            {
                var topPoint = new Point(startGraphX + colGraphWidth * i, 40);
                var bottPoint = new Point(startGraphX + colGraphWidth * i, bottomLineY);

                gBmp.DrawLine( i != 0 ? pen : boldPen, topPoint, bottPoint);
            }

            // Draw bottom line
            var startOsPoint = new Point(startGraphX, bottomLineY);
            var endOsPoint = new Point(startGraphX + colGraphWidth * (vertLinesCount - 1) + 20, bottomLineY);            
            gBmp.DrawLine(boldPen, startOsPoint, endOsPoint);
         
            // Draw bottom os texts
            var step = (int)((maxValue - minValue) / vertLinesCount);
            for (int i = 0; i < vertLinesCount; i++)
            {
                var bt = (minValue + step * i).ToString();
                var centDx = bt.Length * 5;
                var btPoint = new Point(startGraphX + colGraphWidth * i - centDx, bottomLineY + 10);

                gBmp.DrawString(bt, drawFont, textBrush, btPoint);
            }

            // Draw rows
            var rowHeigh = 20;
            for (int i = 0; i < values.Length; i++)
            {
                var startRowPoint = new Point(startGraphX, hRowHeight * (i + 1) - 5);
                var rowWidth = (int)(values[i] / (maxValue - minValue) * colGraphWidth * vertLinesCount);

                var rowSize = new Size(rowWidth, rowHeigh);
                var rect = new Rectangle(startRowPoint, rowSize);
                gBmp.FillRectangle(rowBrush, rect);
                
                gBmp.DrawRectangle(pen, rect);
            }

            bmp.Save(path, ImageFormat.Png);
        }

        private string GetCutStr(string str, int len)
        {
            return str.Length > len ? str.Substring(0, len) : str;
        }
       
    }
}
