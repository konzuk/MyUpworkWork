using System;
using System.Drawing;

namespace JobUpwork5
{
    public class RadioAmBandTable
    {
        
        public string Label { get; set; }
        public string Mode { get; set; }
        public bool Enabled { get; set; }
        public double StartFreq { get; set; }
        public double BW { get; set; }
        public double Tune { get; set; }
        public double StopFreq { get; set; }
        public string Note { get; set; }
        public Color Rgb { get; set; } 
        public string StringRGB
        {
            get { return string.Format("{0};{1};{2}", Rgb.R, Rgb.G, Rgb.B); }
            set
            {
                var s = value.Split(';');
                int[] a = Array.ConvertAll(s, Int32.Parse);
                Rgb = Color.FromArgb(a[0], a[1], a[2]);
            }
        }
    }
}