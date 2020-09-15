using GMap.NET.WindowsForms.Markers;
using System;
using System.Drawing;

namespace Controller
{
    public class AqiCalcPm2pt5 : AqiCalc
    {
        public AqiCalcPm2pt5( double variable ) : base( variable )
        {
            if( variable <= 12.0 )
            {
                AQIlow = 0; AQIhigh = 50;
                Clow = 0; Chigh = 12.0;
            }

            else if( variable <= 35.4 )
            {
                AQIlow = 51; AQIhigh = 100;
                Clow = 12.1; Chigh = 35.4;
            }

            else if( variable <= 55.4 )
            {
                AQIlow = 101; AQIhigh = 150;
                Clow = 35.5; Chigh = 55.4;
            }

            else if( variable <= 150.4 )
            {
                AQIlow = 151; AQIhigh = 200;
                Clow = 55.5; Chigh = 150.4;
            }
            else if( variable <= 250.4 )
            {
                AQIlow = 201; AQIhigh = 300;
                Clow = 150.5; Chigh = 250.4;
            }

            else if( variable <= 350.4 )
            {
                AQIlow = 301; AQIhigh = 400;
                Clow = 250.5; Chigh = 350.4;
            }

            else
            {
                AQIlow = 401; AQIhigh = 500;
                Clow = 350.5; Chigh = 500.4;
            }

            base.Calc();

        }

    }

    public abstract class AqiCalc : IAqiCalc
    {
        //https://en.wikipedia.org/wiki/Air_quality_index
        protected double variable;
        protected double Clow = 0, Chigh = 54;
        protected double AQIlow = 0, AQIhigh = 50;
        private GMarkerGoogleType type = GMarkerGoogleType.arrow;
        private double aqi;
        private Color color;

        protected void Calc()
        {
            aqi = AQIlow + ( AQIhigh - AQIlow ) / ( Chigh - Clow ) * ( variable - Clow );

            if( aqi <= 50 )
            {
                type = GMarkerGoogleType.green_small;
                //rgba(104,225,67,1.0);
                color = Color.FromArgb( 104, 225, 67 );
            }
            else if( aqi <= 100 )
            {
                type = GMarkerGoogleType.yellow_small;
                color = Color.FromArgb( 255, 255, 85 );
            }
            else if( aqi <= 150 )
            {
                type = GMarkerGoogleType.orange_small;
                color = Color.FromArgb( 239, 133, 51 );
            }
            else if( aqi <= 200 )
            {
                type = GMarkerGoogleType.red_small;
                color = Color.FromArgb( 234, 51, 36 );
            }
            else if( aqi <= 300 )
            {
                type = GMarkerGoogleType.purple_small;
                color = Color.FromArgb( 140, 26, 75 );
            }
            //else if( aqi <= 400 )
            //{
            //    type = GMarkerGoogleType.purple_small;
            //    color = Color.FromArgb( 115, 20, 37 );
            //}
            else
            {
                type = GMarkerGoogleType.brown_small;
                color = Color.FromArgb( 115, 20, 37 );
            }

            type = GMarkerGoogleType.arrow;

        }

        protected AqiCalc( double variable )
        {
            this.variable = variable;
        }

        public double getAQI()
        {
            return aqi;
        }

        public GMarkerGoogleType getMarkerType()
        {
            return type;
        }

        public Color getColor()
        {
            return this.color;
        }
    }

    public class AqiCalcPm10 : AqiCalc
    {
        public AqiCalcPm10( double variable ) : base( variable )
        {
            if( variable <= 54 )
            {
                AQIlow = 0; AQIhigh = 50;
                Clow = 0; Chigh = 54;
            }

            else if( variable <= 154 )
            {
                AQIlow = 51; AQIhigh = 100;
                Clow = 55; Chigh = 154;
            }

            else if( variable <= 254 )
            {
                AQIlow = 101; AQIhigh = 150;
                Clow = 155; Chigh = 254;
            }

            else if( variable <= 354 )
            {
                AQIlow = 151; AQIhigh = 200;
                Clow = 255; Chigh = 354;
            }
            else if( variable <= 424 )
            {
                AQIlow = 201; AQIhigh = 300;
                Clow = 355; Chigh = 424;
            }

            else if( variable <= 504 )
            {
                AQIlow = 301; AQIhigh = 400;
                Clow = 425; Chigh = 504;
            }

            else
            {
                AQIlow = 401; AQIhigh = 500;
                Clow = 505; Chigh = 604;
            }

            base.Calc();

        }

    }
}