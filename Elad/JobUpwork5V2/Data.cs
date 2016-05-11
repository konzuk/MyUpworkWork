using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace JobUpwork5
{
    public class Data
    {
        private XDocument _xDocument;
        private string _xmlFile;
        public Data(string XMLFile)
        {
            _xmlFile = XMLFile;
            _xDocument = XDocument.Load(XMLFile);
        }

        public IList<RadioAmBandTable> GetAllRadioAmBandTables()
        {
            var list = from rabt in _xDocument.Descendants("RadioAmBandTable")
                select new RadioAmBandTable()
                {
                    Label = rabt.Element("Label") == null? "": rabt.Element("Label").Value,
                    Enabled = rabt.Element("Enabledd") == null ? false :rabt.Element("Enabledd").Value == "1",
                    StartFreqSave = Convert.ToDouble(rabt.Element("StartFreq") == null ? "" : rabt.Element("StartFreq").Value),
                    StopFreqSave = Convert.ToDouble(rabt.Element("StopFreq") == null ? "" : rabt.Element("StopFreq").Value),
                    BW = Convert.ToDouble(rabt.Element("BW") == null ? "" : rabt.Element("BW").Value),
                    TuneSave = Convert.ToDouble(rabt.Element("Tune") == null ? "" : rabt.Element("Tune").Value),
                    Note = rabt.Element("Note") == null ? "" : rabt.Element("Note").Value,
                    Mode = rabt.Element("Mode") == null ? "" : rabt.Element("Mode").Value,
                    StringRGB = rabt.Element("Rgb") == null ? "" : rabt.Element("Rgb").Value,
                    Command = rabt.Element("Command") == null ? "" : rabt.Element("Command").Value,
                };
            
            return list.ToList();
        }
        
        public void UpdateTune(RadioAmBandTable selectedRadioAmBandTable)
        {
            //Run query

            var singleOrDefault = _xDocument
                .Descendants("RadioAmBandTable"
                ).SingleOrDefault(rabt => selectedRadioAmBandTable.Label == rabt.Element("Label").Value
                                          && selectedRadioAmBandTable.StartFreqSave.ToString() == rabt.Element("StartFreq").Value
                                          && selectedRadioAmBandTable.StopFreqSave.ToString() == rabt.Element("StopFreq").Value);
            if (singleOrDefault != null)
            {
                singleOrDefault.Element("Tune").Value = selectedRadioAmBandTable.TuneSave.ToString();
            }

            _xDocument.Save(_xmlFile);

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
                        new XElement("StartFreq", radioAmBandTable.StartFreqSave.ToString()),
                        new XElement("StopFreq", radioAmBandTable.StopFreqSave.ToString()),
                        new XElement("BW", radioAmBandTable.BW.ToString()),
                        new XElement("Tune", radioAmBandTable.TuneSave.ToString()),
                        new XElement("Mode", radioAmBandTable.Mode),
                        new XElement("Note", radioAmBandTable.Note),
                        new XElement("Command", radioAmBandTable.Command),
                        new XElement("Rgb", radioAmBandTable.StringRGB));

                _xDocument.Element("DocumentElement").Add(rabt);
            }
            //Run query
            

            _xDocument.Save(_xmlFile);
            
        }
    }
}