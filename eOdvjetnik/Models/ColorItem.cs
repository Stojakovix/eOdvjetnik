using System;
using System.Drawing;
using Brush = Microsoft.Maui.Controls.Brush;
using Color = Microsoft.Maui.Graphics.Color;

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
                SelectedBrush = new SolidColorBrush(BojaPozadine);
            }
        }
        public string VrstaDogadaja { get; set; }
        public Color BojaPozadine { get; set; }
        public Color SelectedColor { get; set; }
        public Brush SelectedBrush { get; set; }


    }
}