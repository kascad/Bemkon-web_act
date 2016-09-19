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
    public class hcGraph73
    {
        int exId;
        int width;
        int height;
        double valMax;
        double valMin;
        double valStep = 0;
        List<string> k2 = new List<string>();
        List<string> hc1 = new List<string>(); // Axe x
        List<string> hc2 = new List<string>(); // Axe y
        List<string> hc3 = new List<string>(); // Axe z
        List<double> hc4 = new List<double>(); // Axe x data
        List<double> hc5 = new List<double>(); // Axe x data
        List<double> hc6 = new List<double>(); // Axe y data
        List<double> hc7 = new List<double>(); // Axe y data
        List<double> hc8 = new List<double>(); // Axe z data
        List<double> hc9 = new List<double>(); // Axe z data
        List<string> ScaleNameS = new List<string>();
        List<percentilxyz8> l1 = new List<percentilxyz8>();
        List<percentilxyz8> l2 = new List<percentilxyz8>();
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
        int loc9 = 0;
        int loc10 = 0;
        int loc11 = 0;
        int loc12 = 0;

        public hcGraph73(List<string> k2, List<string> hc1, List<string> hc2, List<string> hc3, List<double> hc4, List<double> hc5, List<double> hc6, List<double> hc7, List<double> hc8, List<double> hc9, int exId, List<string> ScaleNameS, List<percentilxyz8> l1, List<percentilxyz8> l2)
        {
            //this.exId = exId;
            this.k2 = k2;
            this.hc1 = hc1;
            this.hc2 = hc2;
            this.hc3 = hc3;
            this.hc4 = hc4;
            this.hc5 = hc5;
            this.hc6 = hc6;
            this.hc7 = hc7;
            this.hc8 = hc8;
            this.hc9 = hc9;
            this.exId = exId;
            this.ScaleNameS = ScaleNameS;
            //this.valStep = valStep;
        }
        // Axe X
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
            //if (value == hc4[m2]) return System.Drawing.Color.Red;
            if (value == hc4[m2])
            {
                if (loc1 == m2)
                {
                    //loc1 = 0;
                    loc1++;
                    loc2++;
                    return System.Drawing.Color.Red;
                }
            }
            //if (value == hc4[m3]) return System.Drawing.Color.Red;
            if (value == hc4[m3])
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
            //if (value == hc6[m2]) return System.Drawing.Color.Red;
            if (value == hc6[m2])
            {
                if (loc3 == m2)
                {
                    //loc3 = 0;
                    loc3++;
                    loc4++;
                    return System.Drawing.Color.Red;
                    
                }
            }
            //if (value == hc6[m3]) return System.Drawing.Color.Red;
            if (value == hc6[m3])
            {
                if (loc4 == m3)
                {
                    //loc4 = 0;
                    loc3++;
                    loc4++;
                    return System.Drawing.Color.Red;
                    
                }
            }
            loc3++;
            loc4++;
            return System.Drawing.Color.MediumBlue;
        }
// Axe Y
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
           // if (value == hc5[m2]) return System.Drawing.Color.Red;
            if (value == hc5[m2])
            {
                if (loc5 == m2)
                {
                    //loc5 = 0;
                    loc5++;
                    loc6++;
                    return System.Drawing.Color.Red;
                    
                }
            }
            //if (value == hc5[m3]) return System.Drawing.Color.Red;
            if (value == hc5[m3])
            {
                if (loc6 == m3)
                {
                    //loc6 = 0;
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
            //if (value == hc7[m2]) return System.Drawing.Color.Red;
            if (value == hc7[m2])
            {
                if (loc7 == m2)
                {
                    //loc7 = 0;
                    loc7++;
                    loc8++;
                    return System.Drawing.Color.Red;
                }
            }
            //if (value == hc7[m3]) return System.Drawing.Color.Red;
            if (value == hc7[m3])
            {
                if (loc8 == m3)
                {
                    //loc8 = 0;
                    loc7++;
                    loc8++;
                    return System.Drawing.Color.Red;
                    
                }
            }
            loc7++;
            loc8++;
            return System.Drawing.Color.MediumBlue;
        }
        // Axe Z
        private System.Drawing.Color GetBarColour5(double value)
        {

            int m2 = 0;
            int m3 = 0;
            for (int m = 0; m < hc3.Count; m++)
            {
                if (hc1[m] == k2[4])
                {
                    m2 = m;
                }
                if (hc1[m] == k2[5])
                {
                    m3 = m;
                }
            }
            //double dat1 = hc5[m2];
            //double dat2 = hc5[m3];
           // if (value == hc8[m2]) return System.Drawing.Color.Red;
            if (value == hc8[m2])
            {
                if (loc9 == m2)
                {
                    //loc9 = false;
                    loc9++;
                    loc10++;
                    return System.Drawing.Color.Red;
                    
                }
            }
            //if (value == hc8[m3]) return System.Drawing.Color.Red;
            if (value == hc8[m3])
            {
                if (loc10 == m3)
                {
                    //loc10 = false;
                    loc9++;
                    loc10++;
                    return System.Drawing.Color.Red;
                    
                }
            }
            loc9++;
            loc10++;
            return System.Drawing.Color.Brown;
        }

        private System.Drawing.Color GetBarColour6(double value)
        {

            int m2 = 0;
            int m3 = 0;
            for (int m = 0; m < hc3.Count; m++)
            {
                if (hc2[m] == k2[4])
                {
                    m2 = m;
                }
                if (hc2[m] == k2[5])
                {
                    m3 = m;
                }
            }
            //double dat1 = hc6[m2];
            //double dat2 = hc6[m3];
            if (value == hc9[m2])
            {
                if (loc11 == m2)
                {
                    //loc11 = false;
                    loc11++;
                    loc12++;
                    return System.Drawing.Color.Red;
                    
                }
            }
            if (value == hc9[m3])
            {
                if (loc12 == m3)
                {
                    //loc12 = false;
                    loc11++;
                    loc12++;
                    return System.Drawing.Color.Red;
                    
                }
            }
            loc11++;
            loc12++;
            return System.Drawing.Color.Brown;
        }

        public void runhc(List<string> result)
        {
           // string chartRes2 = null;
           // string chartRes3 = null;
            string chartRes4 = null;
            string chartRes5 = null;
            var uniqId = exId;

            Data data1 = new Data(
            hc4.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour(y), Y = y }).ToArray()
                );

            Data data2 = new Data(
            hc6.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour2(y), Y = y }).ToArray()
                );

            Data data3 = new Data(
            hc5.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour3(y), Y = y }).ToArray()
                );

            Data data4 = new Data(
            hc7.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour4(y), Y = y }).ToArray()
                );
            Data data5 = new Data(
            hc8.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour5(y), Y = y }).ToArray()
                );
            Data data6 = new Data(
            hc9.Select(y => new DotNet.Highcharts.Options.Point { Color = GetBarColour6(y), Y = y }).ToArray()
                );

            DotNet.Highcharts.Highcharts chart4 = new DotNet.Highcharts.Highcharts("chart4_" + uniqId)
        //DotNet.Highcharts.Highcharts chart2 = new DotNet.Highcharts.Highcharts("chart2_")
        .InitChart(new Chart
        {
            Type = ChartTypes.Column,
            Height = Math.Max(hc2.Count() * 50, 300)
        })
        .SetTitle(new Title { Text = "Пространство шкал: процентили " + k2[0] + " - " + k2[1] + " vs " + k2[2] + " - " + k2[3] + " vs " + k2[4] + " - " + k2[5] + " " + " </ th >< th > Ось" + ScaleNameS[0] + " - " + ScaleNameS[1] + " </ th >< th > Ось" + ScaleNameS[2] + " - " + ScaleNameS[3] + " </ th >< th > Ось" + ScaleNameS[4] + " - " + ScaleNameS[5] })
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
                            Name = "процентили " + k2[0] + "-" + k2[1],
                            Data = data3,
                            Color = Color.LightGreen
                        },
                        new Series
                        {
                            Name = "процентили " + k2[2] + "-" + k2[3],
                            Data = data4,
                            Color = Color.MediumBlue
                        },

                        new Series
                        {
                            Name = "процентили " + k2[4] + "-" + k2[5],
                            Data = data6,
                            Color = Color.Brown
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
        .SetTitle(new Title { Text = "Пространство шкал: индексы " + k2[0] + " - " + k2[1] + " vs " + k2[2] + " - " + k2[3] + " vs " + k2[4] + " - " + k2[5] + " </ th >< th > Ось" + ScaleNameS[0] + " - " + ScaleNameS[1] + " </ th >< th > Ось" + ScaleNameS[2] + " - " + ScaleNameS[3] + " </ th >< th > Ось" + ScaleNameS[4] + " - " + ScaleNameS[5] })
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
                            Name = "индексы " + k2[0] + "-" + k2[1],
                            Data = data1,
                            Color = Color.LightGreen
                        },
                        new Series
                        {
                            Name = "индексы " + k2[2] + "-" + k2[3],
                            Data = data2,
                            Color = Color.MediumBlue
                        },

                        new Series
                        {
                            Name = "индексы " + k2[4] + "-" + k2[5],
                            Data = data5,
                            Color = Color.Brown
                        }

        });

            chartRes5 = chart5.ToHtmlString();
            result.Add(chartRes5);
            result.Add(chartRes4);

        }
    }

}
