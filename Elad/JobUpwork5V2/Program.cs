using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JobUpwork5
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MC());


        }
    }

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
        public string Command { get; set; }
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

    public class MyFDMSW2BandsXML
    {
        private XDocument _xDocument;
        public MyFDMSW2BandsXML()
        {
            _xDocument = XDocument.Load("FDMSW2Bands.xml");
        }

        public IList<RadioAmBandTable> GetAllRadioAmBandTables()
        {
            var list = from rabt in _xDocument.Descendants("RadioAmBandTable")
                       select new RadioAmBandTable()
                       {
                           Label = rabt.Element("Label") == null? "": rabt.Element("Label").Value,
                           Enabled = rabt.Element("Enabledd") == null ? false :rabt.Element("Enabledd").Value == "1",
                           StartFreq = Convert.ToDouble(rabt.Element("StartFreq") == null ? "" : rabt.Element("StartFreq").Value),
                           StopFreq = Convert.ToDouble(rabt.Element("StopFreq") == null ? "" : rabt.Element("StopFreq").Value),
                           BW = Convert.ToDouble(rabt.Element("BW") == null ? "" : rabt.Element("BW").Value),
                           Tune = Convert.ToDouble(rabt.Element("Tune") == null ? "" : rabt.Element("Tune").Value),
                           Note = rabt.Element("Note") == null ? "" : rabt.Element("Note").Value,
                           Mode = rabt.Element("Mode") == null ? "" : rabt.Element("Mode").Value,
                           StringRGB = rabt.Element("Rgb") == null ? "" : rabt.Element("Rgb").Value,
                           Command = rabt.Element("Command") == null ? "" : rabt.Element("Command").Value,
                       };
            
            return list.ToList();
        }

        public void GetFormSetting(MC mainForm)
        {
            var test = _xDocument.Descendants("FormSetting").SingleOrDefault();
            if (test != null)
            {
                mainForm.TopMost = test.Element("IsFormTopMost").Value == "1";
                mainForm.IsFormFixed = test.Element("IsFormFixed").Value == "1";
                mainForm.IPAddress = test.Element("IPAddress").Value;
                mainForm.Port = test.Element("Port").Value;
                mainForm.ButtonSize = test.Element("ButtonSize").Value;
                mainForm.ButtonCount = Convert.ToInt32(test.Element("ButtonCount").Value) ;
            }
        }

        public void SaveFormSetting(MC mainForm)
        {
            var test = _xDocument.Descendants("FormSetting").SingleOrDefault();
            if (test != null)
            {
                test.Element("IsFormTopMost").Value = Convert.ToInt32(mainForm.TopMost).ToString();
                test.Element("IsFormFixed").Value = Convert.ToInt32(mainForm.IsFormFixed).ToString();
                test.Element("IPAddress").Value = mainForm.IPAddress;
                test.Element("Port").Value = mainForm.Port;
                test.Element("ButtonSize").Value = mainForm.ButtonSize;
                test.Element("ButtonCount").Value = mainForm.ButtonCount.ToString();

                _xDocument.Save("FDMSW2Bands.xml");
            }
        }

        

        public void UpdateRadioAmBandTable(IList<RadioAmBandTable> radioAmBandTables)
        {
            var model = _xDocument.Descendants("RadioAmBandTable");
            model.Remove();
            foreach (var radioAmBandTable in radioAmBandTables)
            {
                XElement rabt =
                new XElement("RadioAmBandTable",
                    new XElement("Label", radioAmBandTable.Label),
                    new XElement("Enabledd", Convert.ToInt32(radioAmBandTable.Enabled).ToString()),
                    new XElement("StartFreq", radioAmBandTable.StartFreq.ToString()),
                    new XElement("StopFreq", radioAmBandTable.StopFreq.ToString()),
                    new XElement("BW", radioAmBandTable.BW.ToString()),
                    new XElement("Tune", radioAmBandTable.Tune.ToString()),
                    new XElement("Mode", radioAmBandTable.Mode),
                    new XElement("Note", radioAmBandTable.Note),
                    new XElement("Command", radioAmBandTable.Command),
                    new XElement("Rgb", radioAmBandTable.StringRGB));

                _xDocument.Element("DocumentElement").Add(rabt);
            }
            //Run query
            

            _xDocument.Save("FDMSW2Bands.xml");
            
        }
        
    }


}
