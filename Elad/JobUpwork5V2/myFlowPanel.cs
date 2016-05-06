using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JobUpwork5
{
    public class myFlowPanel : FlowLayoutPanel
    {
        public myFlowPanel()
        {
            this.InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // myButton
            // 
           

        }

        public bool Enabled { get; set; }
        public double StartFreq { get; set; }
        public double StopFreq { get; set; }
        public string Note { get; set; }
        public Color RGB { get; set; }
    }
}
