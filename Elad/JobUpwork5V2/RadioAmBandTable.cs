using System;
using System.Drawing;

namespace JobUpwork5
{
    public class RadioAmBandTable
    {

        public RadioAmBandTable Clone()
        {
            return (RadioAmBandTable)this.MemberwiseClone();
        }

        public string Label { get; set; }
        public string Mode { get; set; }
        public bool Enabled { get; set; }

        public bool IsValid {
            get { return StartFreqSave < StopFreqSave && TuneSave >= StartFreqSave && TuneSave <= StopFreqSave; }
        }

        public string Command { get; set; }
        private double _startFreq;
        public double BW { get; set; }
        private double _tune;
        public double _stopFreq;
        public string Note { get; set; }
        public Color Rgb { get; set; }
        private double Rate = 1000;
        public bool IsKHZ { get; set; }
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

        public double StartFreqSave
        {
            get { return  _startFreq; }
            set { _startFreq = value; }
        }
        public double StopFreqSave
        {
            get { return _stopFreq; }
            set { _stopFreq = value; }
        }

        public double TuneSave
        {
            get { return _tune; }
            set { _tune = value; }
        }

        public double StartFreq
        {
            get { return IsKHZ ? _startFreq / Rate : _startFreq; }
            set
            {
                _startFreq = IsKHZ ? Math.Round(value * Rate) : Math.Round(value) ; 
                
            }
        }

        public double Tune
        {
            get { return IsKHZ ? _tune / Rate : _tune; }
            set { _tune = IsKHZ ? Math.Round(value * Rate)  : Math.Round(value); }
        }

        public double StopFreq
        {
            get { return IsKHZ ? _stopFreq / Rate : _stopFreq; }
            set { _stopFreq = IsKHZ ? Math.Round(value * Rate) : Math.Round(value); }
        }
    }
}