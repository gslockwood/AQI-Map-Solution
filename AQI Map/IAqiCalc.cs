using GMap.NET.WindowsForms.Markers;
using System.Drawing;

namespace Controller
{
    public interface IAqiCalc
    {
        double getAQI();
        GMarkerGoogleType getMarkerType();
        Color getColor();
    }
}