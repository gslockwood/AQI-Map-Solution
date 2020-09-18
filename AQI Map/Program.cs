using System;
using System.Threading;
using System.Windows.Forms;

namespace AQI_Map
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode( HighDpiMode.SystemAware );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            using( var mutex = new Mutex( false, "AQI Finder" ) )
            {
                // TimeSpan.Zero to test the mutex's signal state and
                // return immediately without blocking
                bool isAnotherInstanceOpen = !mutex.WaitOne( TimeSpan.Zero );
                if( isAnotherInstanceOpen )
                {
                    Console.WriteLine( "Only one instance of this app is allowed." );
                    return;
                }

                // main application entry point
                AQIRestService.AQIRestService aqi = new AQIRestService.AQIRestService();

                Application.Run( new MainForm( new Controller.Controller( aqi ) ) );

                aqi.Dispose();

                mutex.ReleaseMutex();
                //
            }
        }
        //
    }
}

