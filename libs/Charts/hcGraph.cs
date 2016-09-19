using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Attributes;
using System.Drawing;
using DotNet.Highcharts;

namespace Charts
{
    public class hcGraph
    {
        int exId;
        int width;
        int height;
        double valMax;
        double valMin;
        double valStep = 0;
        List<string> k2 = new List<string>();
        List<string> hc1 = new List<string>();
        List<string> hc2 = new List<string>();
        List<double> hc3 = new List<double>();
        List<double> hc4 = new List<double>();
        List<double> hc5 = new List<double>();
        List<double> hc6 = new List<double>();
        List<double> hc31 = new List<double>();
        List<double> hc41 = new List<double>();
        List<double> hc51 = new List<double>();
        List<double> hc61 = new List<double>();
        List<double> hc32 = new List<double>();
        List<double> hc42 = new List<double>();
        List<double> hc52 = new List<double>();
        List<double> hc62 = new List<double>();
        List<string> ScaleNameS = new List<string>();
        //List<percentilxy> datalist = null;
        //List<percentilxy> datalist2 = null;
        List<string> result = new List<string>();
        //Color color = new Color();
        //string color = null;
        //List<double> data = new List<double>();
        List<Series> hc3Series = new List<Series>();
        List<Series> hc4Series = new List<Series>();
        List<Series> hc5Series = new List<Series>();
        List<Series> hc6Series = new List<Series>();
        int loc1 = 0;
        int loc2 = 0;
        int loc3 = 0;
        int loc4 = 0;
        int loc5 = 0;
        int loc6 = 0;
        int loc7 = 0;
        int loc8 = 0;

        public hcGraph(List<string> k2, List<string> hc1, List<string> hc2, List<double> hc3, List<double> hc4, List<double> hc5, List<double> hc6, int exId, List<string> ScaleNameS)
        {
            //this.exId = exId;
            this.k2 = k2;
            this.hc1 = hc1;
            this.hc2 = hc2;
            this.hc3 = hc3;
            this.hc4 = hc4;
            this.hc5 = hc5;
            this.hc6 = hc6;
            this.exId = exId;
            this.ScaleNameS = ScaleNameS;
            //this.valStep = valStep;
        }

        private System.Drawing.Color GetBarColour(double value)
        {
            int m2 = 0;
            int m3 = 0;
            for (int m = 0; m < hc1.Count; m++)
            {
                if (hc1[m] == k2[0])
                {
                    m2 = m;
                }
                if (hc1[m] == k2[1])
                {
                    m3 = m;
                }
            }
            //double dat1 = hc3[m2];
            //double dat2 = hc3[m3];
            if (value == hc3[m2])
            {
                if (loc1 == m2)
                {
                    //loc2 = 0;
                    loc1++;
                    loc2++;
                    return System.Drawing.Color.Red;
                }
            }
            if (value == hc3[m3])
            {
                if (loc2 == m3)
                {
                    //loc2 = 0;
                    loc1++;
                    loc2++;
                    return System.Drawing.Color.Red;
                }
            }
            loc1++;
            loc2++;
            return System.Drawing.Color.LightGreen;
        }

        private System.Drawing.Color GetBarColour2(double value)
        {
            int m2 = 0;
            int m3 = 0;
            for (int m = 0; m < hc1.Count; m++)
            {
                if (hc1[m] == k2[2])
                    {
                        m2 = m;
                    }
                if (hc1[m] == k2[3])
                    {
                        m3 = m;
                    }
            }
            //double dat1 = hc4[m2];
            //double dat2 = hc4[m3];
            if (value == hc4[m2])
            {
                if (loc3 == m2)
                {
                    //loc2 = 0;
                    loc3++;
                    loc4++;
                    return System.Drawing.Color.Red;
                }
            }
            if (value == hc4[m3])
            {
                if (loc4 == m3)
                {
                    //loc2 = 0;
                    loc3++;
                    loc4++;
                    return System.Drawing.Color.Red;
                }
            }
            loc3++;
            loc4++;
            return System.Drawing.Color.MediumBlue;
        }

        private System.Drawing.Color GetBarColour3(double value)
        {
            int m2 = 0;
            int m3 = 0;
            for (int m = 0; m < hc2.Count; m++)
            {
                if (hc2[m] == k2[0])
                {
                    m2 = m;
                }
                if (hc2[m] == k2[1])
                {
                    m3 = m;
                }
            }
            //double dat1 = hc5[m2];
            //double dat2 = hc5[m3];
            if (value == hc5[m2])
            {
                if (loc5 == m2)
                {
                    //loc2 = 0;
                    loc5++;
                    loc6++;
                    return System.Drawing.Color.Red;
                }
            }
            if (value == hc5[m3])
            {
                if (loc6 == m3)
                {
                    //loc2 = 0;
                    loc5++;
                    loc6++;
                    return System.Drawing.Color.Red;
                }
            }
            loc5++;
            loc6++;
            return System.Drawing.Color.LightGreen;
        }

        private System.Drawing.Color GetBarColour4(double value)
        {
            int m2 = 0;
            int m3 = 0;
            for (int m = 0; m < hc2.Count; m++)
            {
                if (hc2[m] == k2[2])
                {
                    m2 = m;
                }
                if (hc2[m] == k2[3])
                {
                    m3 = m;
                }
            }
            //double dat1 = hc6[m2];
            //double dat2 = hc6[m3];
            if (value == hc6[m2])
            {
                if (loc7 == m2)
                {
                    //loc2 = 0;
                    loc7++;
                    loc8++;
                    return System.Drawing.Color.Red;
                }
            }
            if (value == hc6[m3])
            {
                if (loc8 == m3)
                {
                    //loc2 = 0;
                    loc7++;
                    loc8++;
                    return System.Drawing.Color.Red;
                }
            }
            loc7++;
            loc8++;
            return System.Drawing.Color.MediumBlue;
        }


        public void runhc(List<string> result)
        {
            string chartRes2 = null;
            string chartRes3 = null;
            string chartRes4 = null;
            string chartRes5 = null;
            var uniqId = exId;

            Data data1 = new Data(
            hc3.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour(y), Y = y }).ToArray()
                );

            Data data2 = new Data(
            hc4.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour2(y), Y = y }).ToArray()
                );

            Data data3 = new Data(
            hc5.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour3(y), Y = y }).ToArray()
                );

            Data data4 = new Data(
            hc6.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour4(y), Y = y }).ToArray()
                );

            DotNet.Highcharts.Highcharts chart4 = new DotNet.Highcharts.Highcharts("chart4_" + uniqId)
        //DotNet.Highcharts.Highcharts chart2 = new DotNet.Highcharts.Highcharts("chart2_")
        .InitChart(new Chart
        {
            Type = ChartTypes.Column,
            Height = Math.Max(hc2.Count() * 50, 300)
        })
        .SetTitle(new Title { Text = "Пространство шкал: индексы " + k2[0] + " - " + k2[1] + " vs " + k2[2] + " - " + k2[3] + " </ th >< th > Ось" + ScaleNameS[0] + " - " + ScaleNameS[1] + " </ th >< th > Ось" + ScaleNameS[2] + " - " + ScaleNameS[3] })
        .SetCredits(new Credits()
        {
            Enabled = false
        })
        .SetXAxis(new XAxis
        {
            Categories = hc2.ToArray(),
            Title = null
        })

        .SetSeries(new Series[]
        {               
                        new Series
                        {
                            Name = "Индексы " + k2[0] + "-" + k2[1],
                            Data = data3,
                            Color = Color.LightGreen
                        },
                        new Series
                        {
                            Name = "Индексы " + k2[2] + "-" + k2[3],
                            Data = data4,
                            Color = Color.MediumBlue
                        }

        });

            chartRes4 = chart4.ToHtmlString();

            DotNet.Highcharts.Highcharts chart5 = new DotNet.Highcharts.Highcharts("chart5_" + uniqId)
            //DotNet.Highcharts.Highcharts chart3 = new DotNet.Highcharts.Highcharts("chart3_")
        .InitChart(new Chart
        {
            Type = ChartTypes.Column,
            Height = Math.Max(hc1.Count() * 50, 300)
        })
        .SetTitle(new Title { Text = "Пространство шкал: процентили " + k2[0] + " - " + k2[1] + " vs " + k2[2] + " - " + k2[3] + " </ th >< th > Ось" + ScaleNameS[0] + " - " + ScaleNameS[1] + " </ th >< th > Ось" + ScaleNameS[2] + " - " + ScaleNameS[3] })
        .SetPlotOptions(new PlotOptions
        {
            Bar = new PlotOptionsBar
            {
                DataLabels = new PlotOptionsBarDataLabels { Enabled = true }
            },
            Column = new PlotOptionsColumn
            {
                
                ColorByPoint = false,
            }
        })
        .SetCredits(new Credits()
        {
            Enabled = false
        })
        .SetXAxis(new XAxis
        {
            Categories = hc1.ToArray(),
            Title = null
        })
        //Series = new List<Series>
        .SetSeries(new Series[]
        {               new Series
                        {
                            Name = "Процентили " + k2[0] + "-" + k2[1],
                            Data = data1,
                            Color = Color.LightGreen
                        },
                        new Series
                        {
                            Name = "Процентили " + k2[2] + "-" + k2[3],
                            Data = data2,
                            Color = Color.MediumBlue
                        }

        });

            chartRes5 = chart5.ToHtmlString();
            result.Add(chartRes4);
            result.Add(chartRes5);

        }
    }

}
