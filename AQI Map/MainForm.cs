using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

using Utilities;
using GeoUtilities;
using Controller;
using AQIRestService;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using OxyPlot;
//using OxyPlot;
//using OxyPlot.Series;

namespace AQI_Map
{
    public partial class MainForm : Form
    {
        private Controller.Controller controller;
        Logger logger = new Logger( "AQI_Map", Logger.Targets.Console );
        private RectLatLng viewArea;
        private RectLatLng lastViewArea;

        public bool initializing { get; private set; }

        public MainForm( Controller.Controller controller )
        {
            initializing = true;
            logger.Start();

            this.controller = controller;

            //oxy();


            InitializeComponent();

            this.Text = "AQI Finder - purpleAir  v1.93";

            GMap.NET.MapProviders.GoogleMapProvider.Instance.ApiKey = AQI_Map.Properties.Settings.Default.key;

            comboBoxType.DataSource = Enum.GetValues( typeof( AqiPackage.Type ) );

            comboBoxMapType.Items.Add( GMap.NET.MapProviders.GoogleMapProvider.Instance );
            comboBoxMapType.Items.Add( GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance );
            comboBoxMapType.Items.Add( GMap.NET.MapProviders.GoogleTerrainMapProvider.Instance );
            comboBoxMapType.SelectedIndex = 0;

            //comboBoxParticleType.Items.Add( new ComboTypeItem( "Raw PM1.0 in µg/m³", (int)AqiPackage.Names.pm_1 ) );
            comboBoxParticleType.Items.Add( new ComboTypeItem( "US EPA PM2.5 AQI", (int)AqiPackage.Names.PM25 ) );
            comboBoxParticleType.Items.Add( new ComboTypeItem( "US EPA PM10 AQI", (int)AqiPackage.Names.PM10 ) );
            comboBoxParticleType.SelectedIndex = 1;

            comboBoxTime.Items.Add( new ComboTypeItem( "Show Realtime", 0 ) );
            comboBoxTime.Items.Add( new ComboTypeItem( "10 Minute Average", 10 ) );
            comboBoxTime.Items.Add( new ComboTypeItem( "30 Minute Average", 30 ) );
            comboBoxTime.Items.Add( new ComboTypeItem( "60 Minute Average", 60 ) );
            comboBoxTime.Items.Add( new ComboTypeItem( "One Day Average", 1440 ) );
            comboBoxTime.Items.Add( new ComboTypeItem( "One Week Average", 10080 ) );
            comboBoxTime.SelectedIndex = 1;

            gmap.ShowCenter = false;
            GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.DragButton = MouseButtons.Left;
            //
        }

        /*
        //https://csharp.developreference.com/article/18548009/How+do+I+create+and+plot+a+ContourSeries+with+Oxyplot%3F
        //https://csharp.hotexamples.com/examples/-/ContourSeries/-/php-contourseries-class-examples.html
        private static Func<double, double, double> peaks = ( x, y ) =>
           3 * ( 1 - x ) * ( 1 - x ) * Math.Exp( -( x * x ) - ( y + 1 ) * ( y + 1 ) ) - 10 * ( x / 5 - x * x * x - y * y * y * y * y ) * Math.Exp( -x * x - y * y ) - 1.0 / 3 * Math.Exp( -( x + 1 ) * ( x + 1 ) - y * y );

        private double[,] oxy()
        {
        //OxyPlot.Series.ContourSeries contourSeries = new OxyPlot.Series.ContourSeries();
        //contourSeries.Data
        //contourSeries.CalculateContours
        //var model = new OxyPlot.PlotModel { Title = "Peaks" };
        aaa:
            double[] ColumnCoordinate = OxyPlot.ArrayBuilder.CreateVector( -3, 3, 0.05 );
            double[] RowCoordinate = OxyPlot.ArrayBuilder.CreateVector( -3.1, 3.1, 0.05 );

            double[,] data = new double[4, 4];
            var cs = new OxyPlot.Series.ContourSeries
            {
                //Data = (Double[,])data,
                //ColumnCoordinates = OxyPlot.ArrayBuilder.CreateVector( 37.0, 38.0, 0.5 ),
                //RowCoordinates = OxyPlot.ArrayBuilder.CreateVector( -122.0, -121.0, 0.5 )
                ColumnCoordinates = ColumnCoordinate,
                RowCoordinates = RowCoordinate,
                Data = OxyPlot.ArrayBuilder.Evaluate( peaks, ColumnCoordinate, RowCoordinate )

            };


            //cs.Data = OxyPlot.ArrayBuilder.Evaluate( peaks, cs.ColumnCoordinates, cs.RowCoordinates );

            //double[,] temp = OxyPlot.ArrayBuilder.Evaluate( peaks, cs.ColumnCoordinates, cs.RowCoordinates );
            //goto aaa;
            return cs.Data;

            //model.Subtitle = cs.Data.GetLength( 0 ) + "×" + cs.Data.GetLength( 1 );
            //goto aaa;
            //model.Series.Add( cs );
            //return model;
            //
        }
        */

        private void MainForm_Load( object sender, EventArgs e )
        {
            //gmap.SetPositionByKeywords( "San Francisco, USA" );

            //https://www.google.com/maps/@37.7957671,-122.4441536
            //37.7966404,-122.4351951
            //37.79681,-122.42609
            //geoCoordinate = new GeoCoordinate();
            //logger.Info( geoCoordinate );
            //System.Device.Location.CivicAddress dd;

            GeoUtilities.GeoLocation geoLocation = new GeoUtilities.GeoLocation();
            geoLocation.StatusChanged += this.GeoLocation_StatusChanged;
            //
        }

        private void GeoLocation_StatusChanged( object sender, object e )
        {
            if( e is String )
            {
                this.labelMsg.Text = e.ToString();
                return;
            }

            GeoCoordinate geoCoordinate = (GeoCoordinate)e;
            gmap.Position = new GMap.NET.PointLatLng( geoCoordinate.Latitude, geoCoordinate.Longitude );

            GMapOverlay markers = new GMapOverlay( "markers" );
            GMapMarker marker = new GMarkerGoogle(
                gmap.Position, GMarkerGoogleType.lightblue_dot );
            markers.Markers.Add( marker );
            gmap.Overlays.Add( markers );

            Debug.WriteLine( "GeoLocation_StatusChanged: initializing=" + initializing );
            //initializing = false;

            /*
            buttonFind_ClickAsync( this, null );
            if( !controller.isCurrentAqiPackageDataList)
                buttonFind_ClickAsync( this, null );*/
            //
        }

        private void buttonFit_ClickAsync( object sender, EventArgs e )
        {
            if( lastViewArea != null )
                gmap.SetZoomToFitRect( this.lastViewArea );
            //
        }

        private delegate void SafeCallDelegate( object sender, EventArgs e );
        private async void buttonFind_ClickAsync( object sender, EventArgs e )
        {
            if( this.InvokeRequired )
            {
                //Debug.WriteLine( "InvokeRequired" );
                var d = new SafeCallDelegate( buttonFind_ClickAsync );
                this.Invoke( d, new object[] { this, null } );
                return;

            }


            //logger.Info( viewArea.LocationTopLeft + " " + viewArea.LocationRightBottom );

            labelMsg.Text = null;

            //AqiPackage aqiPackage = await controller.getDataAsync( viewArea );

            int minutes = ( (ComboTypeItem)comboBoxTime.SelectedItem ).Value();


            Object results = await controller.getDataAsync( minutes, viewArea );
            if( results is String )
            {
                gmap.Overlays.Clear();
                gmap.Refresh();
                labelMsg.Text = "No stations found: " + results;
                return;
            }

            if( viewArea == null )
                viewArea = gmap.ViewArea;

            lastViewArea = gmap.ViewArea;

            PlotData();
            //
        }

        private void PlotData()
        {
            if( !controller.isCurrentAqiPackageDataList )
            {
                labelMsg.Text = "currentData is empty";
                return;
            }

            labelAverageAqi.Text = null;

            gmap.Overlays.Clear();
            gmap.Refresh();

            int particleIndex = ( (ComboTypeItem)comboBoxParticleType.SelectedItem ).Value();
            AqiPackage.Type currentType = (AqiPackage.Type)comboBoxType.SelectedItem;
            bool filterChecked = this.checkBoxFilter.Checked;
            int filterPercentage = 10;

            Data data = controller.getMarkerData( particleIndex, currentType, filterChecked, filterPercentage );
            if( data == null )
            {
                labelMsg.Text = "data is empty";
                return;
            }

            IList<DataPoint> dataPoints = data.DataPoints;

            double maxValue = 0;
            GMapOverlay markers = new GMapOverlay( "markers" );
            foreach( DataPoint dataPoint1 in dataPoints )
            {
                markers.Markers.Add( BuildMarker( dataPoint1.Lat, dataPoint1.Lon, dataPoint1.Label, dataPoint1.Value, dataPoint1.Color ) );
                if( maxValue < dataPoint1.Value )
                    maxValue = dataPoint1.Value;
                //
            }

            labelAverageAqi.Text = data.averageAqi.ToString();

            gmap.Overlays.Add( markers );

            /*
            // doesn't work
            GMapOverlay routes = Buildroutes( maxValue, dataPoints );
            gmap.Overlays.Add( routes );
            */

            // rerenders the map and values
            gmap.Zoom += 0.000000001;
            //buttonFit_ClickAsync( this, null );
            //
        }

        private GMapOverlay Buildroutes( double maxValue, IList<DataPoint> dataPoints )
        {
            double[] xArray = new Double[dataPoints.Count];
            double[] yArray = new Double[dataPoints.Count];
            double[] zArray = new Double[dataPoints.Count];

            int max =( int)maxValue / 1 + 1;
            for( int index = 0; index < max; index++ )
                zArray[index] = index;

            Double[,] array = new Double[dataPoints.Count, 3];
            int counter = 0;
            foreach( DataPoint dataPoint1 in dataPoints )
            {
                array[counter, 0] = dataPoint1.Lat;
                array[counter, 1] = dataPoint1.Lon;
                array[counter, 2] = dataPoint1.Value;
                //zArray[counter] = dataPoint1.Value;

                counter++;

            }
            int upperBound = array.GetUpperBound( 0 );

            double topLeftLat = this.viewArea.LocationTopLeft.Lat;
            double rightBottomLat = this.viewArea.LocationRightBottom.Lat;
            double increment = ( topLeftLat - rightBottomLat ) / 10;
            for( int index = 0; index < dataPoints.Count; index++ )
                xArray[index] = topLeftLat - index * increment;

            double topLeftLng = this.viewArea.LocationTopLeft.Lng;
            double rightBottomLng = this.viewArea.LocationRightBottom.Lng;

            increment = ( topLeftLng - rightBottomLng ) / 10;
            for( int index = 0; index < dataPoints.Count; index++ )
                yArray[index] = topLeftLng + index * increment;

            GMapOverlay routes = new GMapOverlay( "routes" );
            List<PointLatLng> points = null;// new List<PointLatLng>();

            IDictionary<double, IList<Controller.DataPointBase>> results = Conrec.Contour( array, xArray, yArray, zArray );
            foreach( double key in results.Keys )
            {
                points = new List<PointLatLng>();
                IList<DataPointBase> list = results[key];
                foreach( DataPointBase dpoint in list )
                    points.Add( new PointLatLng( dpoint.Lat, dpoint.Lon ) );

                GMapRoute route = new GMapRoute( points, key.ToString() );
                route.Stroke = new Pen( Color.Red, 1 );
                routes.Routes.Add( route );

            }

            return routes;
            //
        }

        private GMapMarker BuildMarker( double lat, double lon, string label, double value, Color color )
        {
            GMapMarker marker = new MyGMarkerGoogle( new GMap.NET.PointLatLng( lat, lon ), value.ToString( "0.##" ), color );
            marker.ToolTipText = label + "\n" + value.ToString();
            return marker;
            //
        }

        private void gmap_OnTileLoadComplete( long elapsedMilliseconds )
        {
            viewArea = gmap.ViewArea;
            logger.Info( "gmap_OnTileLoadComplete: " + viewArea.LocationTopLeft + " " + viewArea.LocationRightBottom );

            if( initializing )
            {
                initializing = false;

                buttonFind_ClickAsync( this, null );
                if( !controller.isCurrentAqiPackageDataList )
                    buttonFind_ClickAsync( this, null );
            }

        }

        private void gmap_DoubleClick( object sender, EventArgs ea )
        {
            MouseEventArgs e = (MouseEventArgs)ea;
            PointLatLng pt = gmap.FromLocalToLatLng( e.X, e.Y );
            gmap.Position = pt;

            if( e.Button.Equals( MouseButtons.Left ) )
                gmap.Zoom += 1;

            else if( e.Button.Equals( MouseButtons.Right ) )
                gmap.Zoom -= 1;

            //viewArea = gmap.ViewArea;
            gmap.ReloadMap();

        }

        private void MainForm_FormClosed( object sender, FormClosedEventArgs e )
        {
        }

        private void buttonClose_Click( object sender, EventArgs e )
        {
            System.Windows.Forms.Application.Exit();
        }

        private void gmap_OnMapDrag()
        {
            //this.buttonFind_ClickAsync( this, null );
            gmap.ReloadMap();
        }

        private void comboBoxMapType_SelectedIndexChanged( object sender, EventArgs e )
        {
            gmap.MapProvider = (GMap.NET.MapProviders.GMapProvider)comboBoxMapType.SelectedItem;
        }

        private void comboBoxParticleType_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !isInitializing() )
                PlotData();
            //
        }

        private void comboBoxType_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !isInitializing() )
                PlotData();
        }

        private void checkBoxFilter_CheckedChanged( object sender, EventArgs e )
        {
            if( !isInitializing() )
                PlotData();
            //
        }

        private void TextBoxLocation_KeyUp( object sender, KeyEventArgs e )
        {
            if( e.KeyCode.Equals( Keys.Enter ) )
            {
                gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
                GeoCoderStatusCode code = gmap.SetPositionByKeywords( this.textBoxLocation.Text );
                /*
                GeoCoderStatusCode status = GeoCoderStatusCode.UNKNOWN_ERROR;
                GeocodingProvider gp = gmap.MapProvider as GeocodingProvider;
                var pt = gp.GetPoint( "Paris, France", out status );
                if( pt != null)
                gmap.Position = pt.Value;// new GMap.NET.PointLatLng( 48.8589507, 2.2775175 );
                */

                if( code == GeoCoderStatusCode.OK )
                    buttonFind_ClickAsync( this, null );
                //
            }

            //if( textBoxLocation.Text.EndsWith( "\r\n" ) )
            //    gmap.SetPositionByKeywords( textBoxLocation.Text );

        }


        private void textBoxLocation_TextChanged( object sender, EventArgs e )
        {
            if( textBoxLocation.Text.EndsWith( "\r\n" ) )
                gmap.SetPositionByKeywords( textBoxLocation.Text );
        }
        public bool isInitializing()
        {
            return initializing;
        }


        //
    }

}
