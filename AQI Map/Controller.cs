using AQI_Map;
using AQIRestService;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities;

namespace Controller
{
    public class Controller : IDisposable
    {
        Logger logger = new Logger( "AQI_Map Controller", Logger.Targets.Console );
        private AQIRestService.AQIRestService aQIRestService;
        private AqiPackage currentAqiPackage;
        public List<AqiPackage.Data> currentAqiPackageDataList { get; private set; }
        public bool isCurrentAqiPackageDataList { get { return ( ( currentAqiPackageDataList != null ) && ( currentAqiPackageDataList.Count > 0 ) ); } }

        public Controller( AQIRestService.AQIRestService aQIRestService )
        {
            this.aQIRestService = aQIRestService;
            logger.Start();
        }

        internal async System.Threading.Tasks.Task<Object> getDataAsync( int minutes, RectLatLng viewArea )
        {
            //string url = @"https://www.purpleair.com/data.json?module=AQI&conversion=C1&average=10&layer=standard&advanced=false&inside=true&outside=true&mine=true&fetch=true&nwlat=37.8012691,&selat=37.7956018,&nwlng=-122.45118&selng=-122.428591";

            string url = @"https://www.purpleair.com/data.json?module=AQI&conversion=C1&average=10&layer=standard&advanced=false&inside=true&outside=true&mine=true&fetch=true";

            //"https://www.purpleair.com/data.json?opt=1/mAQI/a10/cC0&fetch=true&nwlat=37.79695602555266&selat=37.79332282387712&nwlng=-122.43653449695088&selng=-122.4277160329134&fields=pm_1";
            string freeUrl = @"https://www.purpleair.com/data.json?opt=1/mAQI/a10/cC0&fetch=true";

            freeUrl = freeUrl.Replace( "/a10/", string.Format( "/a{0}/", minutes ) );
            //System.Diagnostics.Debug.WriteLine( freeUrl );
            url = freeUrl;

            String location = "";
            location += "&nwlat=" + viewArea.Top;
            location += "&selat=" + viewArea.Bottom;
            location += "&nwlng=" + viewArea.Left;
            location += "&selng=" + viewArea.Right;

            //McAllister & Broderick Indoors  37.777195,-122.44045
            //Duboce Triangle Noe Street",37.7659,-122.433235
            //location = "&nwlat=37.324777195&selat=37.17659&nwlng=-122.944045&selng=-122.10433235";

            url += location;
            System.Diagnostics.Debug.WriteLine( url );
            object result = await aQIRestService.GetAqiPackageAsync( url );
            if( result is String )
                return result;

            if( result == null )
                return null;

            currentAqiPackage = (AqiPackage)result;


            AqiPackage.Data data = null;
            this.currentAqiPackageDataList = new List<AqiPackage.Data>();
            //IList<AqiPackage.Data> currentData = controller.currentAqiPackageDataList;

            //current_pm_25List = new Dictionary<int, double>();
            //current_pm_10List = new Dictionary<int, double>();

            foreach( Object[] line in currentAqiPackage.data )
            {
                if( line[(int)AqiPackage.Names.Lat] == null )
                    continue;
                if( line[(int)AqiPackage.Names.Lon] == null )
                    continue;

                if( line[(int)AqiPackage.Names.PM25] == null )
                    continue;
                if( line[(int)AqiPackage.Names.PM10] == null )
                    continue;

                data = new AqiPackage.Data( line );
                this.currentAqiPackageDataList.Add( data );

                //current_pm_25List.Add( data.id, data.PM25 );
                //current_pm_10List.Add( data.id, data.PM10 );

            }

            return currentAqiPackage;
            //
        }


        public AqiPackage getCurrentAqiPackage()
        {
            return currentAqiPackage;
        }
        public object[][] getCurrentAqiData()
        {
            return ( currentAqiPackage == null ) ? null : currentAqiPackage.data;
        }

        internal IList<AqiPackage.Data> getfilteredCurrentAqiData( int index, int currentType, int minimum )
        {
            IList<AqiPackage.Data> list = new List<AqiPackage.Data>( this.currentAqiPackageDataList );
            //logger.Debug( "getfilteredCurrent_pm_1_AqiData: list count=" + list.Count );

            list = list.Where( x => x.Type == currentType ).ToList();
            //logger.Debug( "getfilteredCurrent_pm_1_AqiData: list after type filter count=" + list.Count );

            int number = list.Count * minimum / 200;

            //logger.Debug( "getfilteredCurrent_pm_1_AqiData: number=" + number );

            IEnumerable<AqiPackage.Data> lowest = null;
            IEnumerable<AqiPackage.Data> highest = null;
            if( index == (int)AqiPackage.Names.PM25 )
            {
                lowest = list.OrderBy( x => x.PM25 ).Take( list.Count() - number );
                //System.Diagnostics.Debug.WriteLine( lowest.Count() );
                highest = lowest.OrderByDescending( x => x.PM25 ).Take( lowest.Count() - number );
                //System.Diagnostics.Debug.WriteLine( highest.Count() );
            }
            else if( index == (int)AqiPackage.Names.PM10 )
            {
                lowest = list.OrderBy( x => x.PM10 ).Take( list.Count() - number );
                //System.Diagnostics.Debug.WriteLine( lowest.Count() );
                highest = lowest.OrderByDescending( x => x.PM10 ).Take( lowest.Count() - number );
                //System.Diagnostics.Debug.WriteLine( highest.Count() );
            }


            IList<AqiPackage.Data> removeList = new List<AqiPackage.Data>();
            foreach( AqiPackage.Data dataPoint in list )
                if( !highest.Any( v => v.id.Equals( dataPoint.id ) ) )
                    removeList.Add( dataPoint );

            foreach( AqiPackage.Data dataPoint in removeList )
                list.Remove( dataPoint );
            /*
            if( index == (int)AqiPackage.Names.PM25 )
                logger.Debug( "getfilteredCurrent_pm_1_AqiData: excluding value=" + dataPoint.PM25 );
            else if( index == (int)AqiPackage.Names.PM10 )
                logger.Debug( "getfilteredCurrent_pm_1_AqiData: excluding value=" + dataPoint.PM10 );
            */

            //logger.Debug( "getfilteredCurrent_pm_1_AqiData: list count=" + list.Count );

            return list;

        }

        internal Data getMarkerData( int particleIndex, AqiPackage.Type currentType, bool filterChecked, int filterPercentage )
        {
            //double averagePM2pt5 = 0;
            //double averagePM10 = 0;
            double averageAqi = 0;

            IList<AqiPackage.Data> currentData = currentAqiPackageDataList;
            if( filterChecked )
                currentData = getfilteredCurrentAqiData( particleIndex, (int)currentType, filterPercentage );

            IList<DataPoint> dataPoints = new List<DataPoint>();

            DataPoint dataPoint = null;
            //int counter = 0;
            foreach( AqiPackage.Data item in currentData )
            {
                if( item.Type != (int)currentType )
                    continue;

                IAqiCalc aqiCalc = null;
                if( particleIndex == (int)AqiPackage.Names.PM25 )
                    aqiCalc = new AqiCalcPm2pt5( item.PM25 );

                else if( particleIndex == (int)AqiPackage.Names.PM10 )
                    aqiCalc = new AqiCalcPm10( item.PM10 );

                else
                {
                    continue;
                }

                //counter++;

                double aqi = aqiCalc.getAQI();
                averageAqi += aqi;

                //System.Diagnostics.Debug.WriteLine( aqi );

                dataPoint = new DataPoint( item.Lat, item.Lon, item.Label, aqi, aqiCalc.getColor() );
                if( dataPoint != null )
                    dataPoints.Add( dataPoint );
                //

            }

            if( dataPoints.Count() == 0 )
                return null;

            Data data = new Data( averageAqi / dataPoints.Count(), dataPoints );

            return data;
            //
        }


        public void Dispose()
        {
            logger.Stop();
        }

    }

    public class Data
    {
        public double averageAqi { get; }


        public Data( double averageAqi, IList<DataPoint> dataPoints )
        {
            this.averageAqi = averageAqi;
            DataPoints = dataPoints;
        }

        public IList<DataPoint> DataPoints { get; }

    }

    public class DataPointBase
    {
        public double Lat { get; }
        public double Lon { get; }
        public double Value { get; }
        public DataPointBase( double lat, double lon, double value )
        {
            Lat = lat;
            Lon = lon;
            Value = value;

        }

    }

    public class DataPoint : DataPointBase
    {
        //public double Lat { get; }
        //public double Lon { get; }
        //public double Value { get; }
        public string Label { get; }
        public Color Color { get; internal set; }

        public DataPoint( double lat, double lon, string label, double value, Color color ) : base( lat, lon, value )
        {
            //Lat = lat;
            //Lon = lon;
            //Value = value;
            Label = label;
            Color = color;
            //
        }

    }

    }