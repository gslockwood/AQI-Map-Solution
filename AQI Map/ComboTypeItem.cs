namespace AQI_Map
{
    internal class ComboTypeItem
    {
        private string title;
        private int value;

        public ComboTypeItem( string title, int value )
        {
            this.title = title;
            this.value = value;
        }

        public int Value()
        {
            return value;
        }
        public override string ToString()
        {
            return title;
        }
    }
}