using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            AQIRestService.AQIRestService aqi = new AQIRestService.AQIRestService();

            Application.Run( new MainForm(new Controller.Controller( aqi ) ) );

            aqi.Dispose();
            //
        }
    }
}
