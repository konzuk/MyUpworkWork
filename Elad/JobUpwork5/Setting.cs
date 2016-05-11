using System;
using System.Linq;
using System.Xml.Linq;

namespace JobUpwork5
{
    public class Setting
    {
        private XDocument _xDocument;
        public Setting()
        {
            _xDocument = XDocument.Load("Setting.xml");
        }
        public void GetFormSetting(BS mainForm)
        {
            var test = _xDocument.Descendants("FormSetting").SingleOrDefault();
            if (test != null)
            {
                mainForm.TopMost = test.Element("IsFormTopMost").Value == "1";
                mainForm.IsFormFixed = test.Element("IsFormFixed").Value == "1";
                mainForm.IPAddress = test.Element("IPAddress").Value;
                mainForm.Port = test.Element("Port").Value;
                mainForm.ButtonSize = test.Element("ButtonSize").Value;
                mainForm.ButtonCount = Convert.ToInt32(test.Element("ButtonCount").Value);
                mainForm.DesiredLocation = test.Element("DesiredLocation").Value;
                mainForm.IsKHZ = test.Element("IsKHZ").Value == "1";
            }
        }

        public void SaveFormSetting(BS mainForm)
        {
            var test = _xDocument.Descendants("FormSetting").SingleOrDefault();
            if (test != null)
            {
                test.Element("IsFormTopMost").Value = Convert.ToInt32(mainForm.TopMost).ToString();
                test.Element("IsFormFixed").Value = Convert.ToInt32(mainForm.IsFormFixed).ToString();
                test.Element("IPAddress").Value = mainForm.IPAddress;
                test.Element("Port").Value = mainForm.Port;
                test.Element("ButtonSize").Value = mainForm.ButtonSize;
                test.Element("DesiredLocation").Value = mainForm.DesiredLocation;
                test.Element("IsKHZ").Value = Convert.ToInt32(mainForm.IsKHZ).ToString();
                test.Element("ButtonCount").Value = mainForm.ButtonCount.ToString();

                _xDocument.Save("Setting.xml");
            }
        }
    }
}