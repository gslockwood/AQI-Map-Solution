using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;

namespace GeoLocation1
{
    public class GeoLocation1
    {
        private GeoCoordinateWatcher Watcher = null;
        GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

        public event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;

        public GeoLocation1()
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
                // Display the latitude and longitude.
                if( Watcher.Position.Location.IsUnknown )
                {
                    Console.WriteLine( "Cannot find location data");
                }
                else
                {
                    Console.WriteLine( Watcher.Position.Location.Latitude.ToString() );
                    Console.WriteLine( Watcher.Position.Location.Longitude.ToString() );
                }
            }
        }
        /*
        static void GetLocationProperty()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart( false, TimeSpan.FromMilliseconds( 1000 ) );

            System.Device.Location.GeoCoordinate coord = watcher.Position.Location;

            if( coord.IsUnknown != true )
            {
                Console.WriteLine( "Lat: {0}, Long: {1}",
                    coord.Latitude,
                    coord.Longitude );
            }
            else
            {
                Console.WriteLine( "Unknown latitude and longitude." );
            }
        }*/

    }
}
