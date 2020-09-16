using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Drawing;

namespace AQI_Map
{
    internal class MyGMarkerGoogle : GMarkerGoogle
    {
        private String text;
        private Font font;
        private Color foreColor;

        public MyGMarkerGoogle( PointLatLng point, GMarkerGoogleType type, String text, Color foreColor ) : base( point, type )
        {
            this.Size = new Size( 24, 24 );
            this.foreColor = foreColor;
            this.text = text;
            font = new System.Drawing.Font( "Sansation", 5.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0 );

        }

        public MyGMarkerGoogle( PointLatLng point, String text, Color foreColor ) : base( point, GMarkerGoogleType.arrow )
        {
            this.Size = new Size( 24, 24 );
            this.foreColor = foreColor;
            this.text = text;
            font = new System.Drawing.Font( "Sansation", 5.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0 );

        }

        public override void OnRender( Graphics g )
        {
            base.OnRender( g );
            SolidBrush brush = new SolidBrush( this.foreColor );
            Rectangle localArea = this.LocalArea;
            //localArea.X -= Size.Width / 2;
            //localArea.Y -= Size.Height / 2;

            g.DrawEllipse( new Pen( Color.Black ), localArea );
            g.FillEllipse( brush, localArea );
            //g.DrawEllipse( new Pen( this.foreColor ), centerX - radius, centerY - radius,
            //          radius + radius, radius + radius );

            //PointF p = new Point( localArea.Width / 2 - 1, localArea.Height / 2 );
            PointF p = new Point( localArea.X + localArea.Width / 2 - 7, localArea.Y + localArea.Height / 2 - 4 );
            g.DrawString( text, font, new SolidBrush( Color.Black ), p );            
            //
        }

    }

}
