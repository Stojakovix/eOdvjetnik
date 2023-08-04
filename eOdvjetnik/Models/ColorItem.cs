using System;

namespace eOdvjetnik.Model
{
    public class ColorItem
    {
        private string _boja;

        public int Id { get; set; }
        public string NazivBoje { get; set; }
        public string Boja
        {
            get { return _boja; }
            set
            {
                _boja = value;
                BojaPozadine = Color.FromArgb(value);
            }
        }
        public string VrstaDogadaja { get; set; }
        public Color BojaPozadine { get; set; }
        public Color SelectedColor { get; set; }

    }
}