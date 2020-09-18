using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Utilities;
using System.Threading.Tasks;

namespace AQIRestService
{
    public class AQIRestService : IDisposable
    {
        private readonly HttpClient client = new HttpClient();
        Logger logger = new Logger( "AQIRestService", Logger.Targets.Console );

        public AQIRestService()
        {
            logger.Start();
        }

        public void Dispose()
        {
            logger.Stop();
        }

        public async Task<Object> GetAqiPackageAsync( string url )
        {
            //Object results = null;

            try
            {
                //Stream streamTask = await client.GetStreamAsync( url );
                //results = await JsonSerializer.DeserializeAsync<AqiPackage>( streamTask );

                //GET https://www.purpleair.com/map?opt=1/i/mAQI/a10/cC0 HTTP/1.1
                var html = await client.GetStringAsync( @"https://www.purpleair.com/map?opt=1/i/mAQI/a10/cC0" );

                var content = await client.GetStringAsync( url );
                /*
                logger.Debug( content );

                var extendedCharPresent = content.Any( ( b ) => b == 124 );
                var extendedCharPresent1 = content.Contains( "|" );

                System.Diagnostics.Debug.WriteLine( "Success |" + content[19] + "|" );
                System.Diagnostics.Debug.WriteLine( "Success |" + content[18] + "|" );
                System.Diagnostics.Debug.WriteLine( "Success |" + content[20] + "|" );

                System.Diagnostics.Debug.WriteLine( "Success int=|" + (int)content[20] + "|" );

                System.Diagnostics.Debug.WriteLine( "Success |" + content[21] + "|" );
                System.Diagnostics.Debug.WriteLine( "Success |" + content[22] + "|" );


                if( extendedCharPresent || extendedCharPresent1 )
                {
                    content = content.Replace( "|", "" );

                    Match match = Regex.Match( content, @"([0-9]+)(\[)([0-9]+)" );
                    if( match.Success )
                    {
                        //System.Diagnostics.Debug.WriteLine( "Success" );
                        string temp = string.Format( "{0}],[{1}", match.Groups[1].Value, match.Groups[3].Value );
                        System.Diagnostics.Debug.WriteLine( temp );
                        content = content.Replace( match.Value, temp );
                    }
                }
                */
                return JsonConvert.DeserializeObject<AqiPackage>( content );

            }
            catch( Exception ex )
            {
                return ex.Message;
            }

            //return results;
            //
        }

    }


    public class AqiPackage
    {
        public enum Type
        {
            Outside,Inside
        }

        public string version { get; set; }
        public string[] fields { get; set; }
        //public IList<Data> data { get; set; }
        public object[][] data { get; set; }
        public int count { get; set; }

        // never change this order
        public enum Names
        {
            ID, pm, age, pm_0, PM25, pm_2, pm_3, pm_4, pm_5, pm_6,
            conf, pm1, PM10,
            particles1, particles2, particles3, particles4, particles5, particles6,
            Humidity, Temperature, Pressure, Elevation, Type, Label,
            Lat, Lon, Icon, isOwner, Flags, Voc, Ozone1,
            Adc, CH//, Unk1, Unk2
        }

        public class Data
        {
            public Data( object[] line )
            {
                this.id = Convert.ToInt32( line[(int)Names.ID] );
                this.age = Convert.ToInt32( line[(int)Names.age] );

                this.pm_0 = Convert.ToDouble( line[(int)Names.pm_0] );
                this.PM25 = Convert.ToDouble( line[(int)Names.PM25] );//also Raw PM2.5 in µg/m³
                this.pm_2 = Convert.ToInt32( line[(int)Names.pm_2] );
                this.pm_3 = Convert.ToDouble( line[(int)Names.pm_3] );
                this.pm_4 = Convert.ToDouble( line[(int)Names.pm_4] );
                this.pm_5 = Convert.ToDouble( line[(int)Names.pm_5] );
                this.pm_6 = Convert.ToDouble( line[(int)Names.pm_6] );
                this.PM10 = Convert.ToDouble( line[(int)Names.PM10] );// also Raw PM10 in µg/m³

                this.conf = Convert.ToInt32( line[(int)Names.conf] );

                this.pm = Convert.ToDouble( line[(int)Names.pm] );
                this.pm1 = Convert.ToDouble( line[(int)Names.pm1] );//Raw PM1.0 in µg/m³

                this.particles1 = Convert.ToDouble( line[(int)Names.particles1] );//Particles &gt;=0.3µm: count/dl
                this.particles2 = Convert.ToDouble( line[(int)Names.particles2] );//Particles &gt;=0.5µm: count/dl
                this.particles3 = Convert.ToDouble( line[(int)Names.particles3] );//Particles &gt;=1.0µm: count/dl
                this.particles4 = Convert.ToDouble( line[(int)Names.particles3] );//Particles &gt;=2.5µm: count/dl
                this.particles5 = Convert.ToDouble( line[(int)Names.particles5] );//Particles &gt;=5.0µm: count/dl
                this.particles6 = Convert.ToDouble( line[(int)Names.particles6] );//Particles &gt;=10µm: count/dl

                this.Humidity = Convert.ToDouble( line[(int)Names.Humidity] );
                this.Temperature = Convert.ToDouble( line[(int)Names.Temperature] );
                this.Pressure = Convert.ToDouble( line[(int)Names.Pressure] );
                this.Elevation = Convert.ToInt32( line[(int)Names.Elevation] );

                this.Type = Convert.ToInt32( line[(int)Names.Type] );
                this.Label = line[(int)Names.Label].ToString();

                this.Lat = Convert.ToDouble( line[(int)Names.Lat] );
                this.Lon = Convert.ToDouble( line[(int)Names.Lon] );
                this.Icon = line[(int)Names.Icon];

                this.isOwner = Convert.ToBoolean( line[(int)Names.Icon] );
                this.Flags = Convert.ToInt32( line[(int)Names.Icon] );
                this.Voc = Convert.ToDouble( line[(int)Names.Voc] );
                this.Ozone1 = Convert.ToDouble( line[(int)Names.Ozone1] );
                this.Adc = Convert.ToDouble( line[(int)Names.Adc] );
                this.CH = Convert.ToInt32( line[(int)Names.CH] );

            }

            public int id { get; set; }
            public double pm { get; set; }
            public int age { get; set; }
            public double pm_0 { get; set; }
            public double PM25 { get; set; }
            public int pm_2 { get; set; }
            public double pm_3 { get; set; }
            public double pm_4 { get; set; }
            public double pm_5 { get; set; }
            public double pm_6 { get; set; }
            public double conf { get; set; }
            public double pm1 { get; set; }
            public double PM10 { get; set; }
            public double particles1 { get; set; }
            public double particles2 { get; set; }
            public double particles3 { get; set; }
            public double particles4 { get; set; }
            public double particles5 { get; set; }
            public double particles6 { get; set; }
            public double Humidity { get; set; }
            public double Temperature { get; set; }
            public double Pressure { get; set; }
            public int Elevation { get; set; }
            public double Type { get; set; }
            public string Label { get; set; }
            public double Lat { get; set; }
            public double Lon { get; set; }
            public object Icon { get; set; }
            public bool isOwner { get; set; }
            public int Flags { get; set; }
            public double Voc { get; set; }
            public double Ozone1 { get; set; }
            public double Adc { get; set; }
            public int CH { get; set; }

        }
        public class Application
        {
            public string version { get; set; }
            public IList<string> fields { get; set; }
            public IList<Data> data { get; set; }
            public int count { get; set; }

        }

    }


}
