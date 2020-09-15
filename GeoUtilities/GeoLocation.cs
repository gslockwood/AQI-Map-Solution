using System;
using System.Device.Location;

namespace GeoUtilities
{
    public class GeoLocation
    {
        private GeoCoordinateWatcher Watcher = null;
        GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

        public delegate void ThresholdReachedEventHandler( object sender, object e );
        public event ThresholdReachedEventHandler StatusChanged;

        public GeoLocation()
        {
            Watcher = new GeoCoordinateWatcher();
            // Catch the StatusChanged event.
            Watcher.StatusChanged += Watcher_StatusChanged;
            // Start the watcher.
            Watcher.Start();
            //
        }

        private void Watcher_StatusChanged( object sender, GeoPositionStatusChangedEventArgs e )
        {
            if( e.Status == GeoPositionStatus.Ready )
            {
                if( Watcher.Position.Location.IsUnknown )
                    StatusChanged?.Invoke( this, "Cannot find location data" );
                
                else
                    StatusChanged?.Invoke( this, new GeoCoordinate( Watcher.Position.Location.Latitude , Watcher.Position.Location.Longitude ) );

            }
        }

    }

    public class GeoCoordinate : IEquatable<GeoCoordinate>
    {
        public static readonly GeoCoordinate Unknown;
        public double Altitude { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool IsUnknown { get; }

        public GeoCoordinate() { }
        public GeoCoordinate( double latitude, double longitude ) 
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public GeoCoordinate( double latitude, double longitude, double altitude ) 
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        public bool Equals( GeoCoordinate other )
        {
            return true;
        }
    }

    }
