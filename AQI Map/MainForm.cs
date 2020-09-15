using AQIRestService;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Windows.Forms;
using Utilities;
using GeoUtilities;
using System.Collections.Generic;
using Controller;

namespace AQI_Map
{
    public partial class MainForm : Form
    {
        private Controller.Controller controller;
        Logger logger = new Logger( "AQI_Map", Logger.Targets.Console );
        private RectLatLng viewArea;
        private RectLatLng lastViewArea;

        public bool initializing { get; private set; }
        public bool isInitializing()
        {
            return initializing;
        }

        public MainForm( Controller.Controller controller )
        {
            initializing = true;
            logger.Start();

            this.controller = controller;


            InitializeComponent();

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

            initializing = false;

            buttonFind_ClickAsync( this, null );
            //
        }

        private void buttonFit_ClickAsync( object sender, EventArgs e )
        {
            if( lastViewArea != null )
                gmap.SetZoomToFitRect( this.lastViewArea );
            //
        }

        private async void buttonFind_ClickAsync( object sender, EventArgs e )
        {
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
            labelAverageAqi.Text = null;

            IList<AqiPackage.Data> currentData = controller.currentAqiPackageDataList;
            if( ( currentData == null ) || ( currentData.Count == 0 ) )
            {
                labelMsg.Text = "currentData is empty";
                return;
            }

            gmap.Overlays.Clear();
            gmap.Refresh();

            int particleIndex = ( (ComboTypeItem)comboBoxParticleType.SelectedItem ).Value();
            AqiPackage.Type currentType = (AqiPackage.Type)comboBoxType.SelectedItem;
            bool filterChecked = this.checkBoxFilter.Checked;

            Data data = controller.getMarkerData( particleIndex, currentType, filterChecked, 10 );
            if( data == null )
            {
                labelMsg.Text = "data is empty";
                return;
            }

            IList<DataPoint> dataPoints = data.DataPoints;

            GMapOverlay markers = new GMapOverlay( "markers" );
            GMapMarker marker = null;
            foreach( DataPoint dataPoint1 in dataPoints )
            {
                marker = BuildMarker( particleIndex, dataPoint1.Lat, dataPoint1.Lon, dataPoint1.Label, dataPoint1.Value );
                if( marker == null )
                    continue;

                markers.Markers.Add( marker );

            }

            labelAverageAqi.Text = data.averageAqi.ToString();

            gmap.Overlays.Add( markers );

            buttonFit_ClickAsync( this, null );
            //
        }

        private GMapMarker BuildMarker( int varIndex, double lat, double lon, string label, double value )
        {
            Double variable = value;
            IAqiCalc aqiCalc = null;
            if( varIndex == (int)AqiPackage.Names.PM25 )
                aqiCalc = new AqiCalcPm2pt5( variable );

            else if( varIndex == (int)AqiPackage.Names.PM10 )
                aqiCalc = new AqiCalcPm10( variable );

            else
            {
                labelMsg.Text = "BuildMarker: aqiCalc = null";
                return null;
            }

            double aqi = aqiCalc.getAQI();
            GMarkerGoogleType type = aqiCalc.getMarkerType();

            //logger.Debug( label + " " + aqi );

            PointLatLng point = new GMap.NET.PointLatLng( lat, lon );

            GMapMarker marker = new MyGMarkerGoogle( point, type, aqi.ToString( "0.##" ), aqiCalc.getColor() );

            marker.ToolTipText = label + "\n" + aqi.ToString();

            return marker;
            //
        }

        private void gmap_OnTileLoadComplete( long elapsedMilliseconds )
        {
            viewArea = gmap.ViewArea;
            //logger.Info( "gmap_OnTileLoadComplete: " + viewArea.LocationTopLeft + " " + viewArea.LocationRightBottom );
        }

        /*
        private void gmap_OnMapDoubleClick( PointLatLng pointClick, MouseEventArgs e )
        {
            PointLatLng pt = gmap.FromLocalToLatLng( e.X, e.Y );
            gmap.Position = pt;

            if( e.Button.Equals( MouseButtons.Left ) )
                gmap.Zoom += 1;

            else if( e.Button.Equals( MouseButtons.Right ) )
                gmap.Zoom -= 1;
            //
        }*/

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
            //gmap.Dispose();
            //logger.Stop();
        }

        private void buttonClose_Click( object sender, EventArgs e )
        {
            System.Windows.Forms.Application.Exit();
        }

        private void gmap_OnMapDrag()
        {
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
        //
    }

}
