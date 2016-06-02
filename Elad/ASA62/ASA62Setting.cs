using System;
using System.IO;

namespace ASA62
{
    public class ASA62Setting
    {
        private string MyDocument
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                       @"\ELAD\ASA15\asa62settings.xml";
            }
        }

        public string ButtonSize { get; set; } = "50,28";

        public bool IsTopMost { get; set; }
        public bool IsFixLocation { get; set; }
        public string DesiredLocation { get; set; }

        public ASA62Setting ReadPMUSetting(string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = MyDocument;
            }

            if (!File.Exists(fileName))
            {
                return new ASA62Setting();
            }
            return XMLRWHelper.ReadFromXmlFile<ASA62Setting>(fileName);
        }

        public void WritePMUSetting(string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = MyDocument;
            }
            if (!File.Exists(fileName))
            {
                var path = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            XMLRWHelper.WriteToXmlFile(fileName, this);
        }
    }
}