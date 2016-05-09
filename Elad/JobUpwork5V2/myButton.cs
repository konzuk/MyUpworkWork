using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JobUpwork5
{
    public class myButton : Button
    {
        public myButton()
        {
            this.InitializeComponent();

            radioAmBandTable = new RadioAmBandTable();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // myButton
            // 
            this.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.MaximumSize = new System.Drawing.Size(50, 28);
            this.MinimumSize = new System.Drawing.Size(50, 28);
            this.Size = new System.Drawing.Size(50, 28);
            this.ResumeLayout(false);

        }




        public RadioAmBandTable GetRadioAmBandTable()
        {
            return radioAmBandTable;
        }
        public void SetRadioAmBandTable(RadioAmBandTable rabt)
        {
            radioAmBandTable = rabt;
        }

        public override string Text
        {
            get { return radioAmBandTable.Label; }
            set { radioAmBandTable.Label = value; }
        }

        public bool Enabledd
        {
            get { return radioAmBandTable.Enabled; }
            set { radioAmBandTable.Enabled = value; }
        }
        public double StartFreq
        {
            get { return radioAmBandTable.StartFreq; }
            set { radioAmBandTable.StartFreq = value; }
        }
        public double StopFreq
        {
            get { return radioAmBandTable.StopFreq; }
            set { radioAmBandTable.StopFreq = value; }
        }
        public double Tune
        {
            get { return radioAmBandTable.Tune; }
            set { radioAmBandTable.Tune = value; }
        }
        public string Note
        {
            get { return radioAmBandTable.Note; }
            set { radioAmBandTable.Note = value; }
        }
        public string Command
        {
            get { return radioAmBandTable.Command; }
            set { radioAmBandTable.Command = value; }
        }
        public Color RGB 
        {
            get { return radioAmBandTable.Rgb; }
            set { radioAmBandTable.Rgb = value; }
        }

        private RadioAmBandTable radioAmBandTable { get; set; }

        
        
    }
}
