using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using ClientServerApp;

namespace JobUpwork5
{
    
    public partial class BS : Form
    {
        private Data Fdmsw2BandsXml;
        private Setting Setting;
        public bool IsFormFixed { get; set; }
        public bool IsKHZ { get; set; }
        public string ButtonSize { get; set; }
        public int ButtonCount { get; set; }
        public string FileName { get; private set; }

        public void SaveFileName(string fn)
        {
            FileName = fn;
        }

        public BS()
        {
            InitializeComponent();
            var settingFile = $"{MyDocument}\\ELAD\\ESW3#Band\\Setting.xml";
            FileName = $"{MyDocument}\\ELAD\\ESW3#Band\\Band.xml";

            if (!File.Exists(FileName))
            {
                var path = Path.GetDirectoryName(FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var doc = new XDocument();
                XElement rabt = new XElement("DocumentElement");
                doc.Add(rabt);
                doc.Save(FileName);
            }

            if (!File.Exists(settingFile))
            {
                var path2 = Path.GetDirectoryName(settingFile);
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
                InitSetting(settingFile);
            }
            Setting = new Setting(settingFile);
            
            this.GetSetting();


            
            Fdmsw2BandsXml = new Data(FileName);
            this.AddRABTButtons();
            this.Load += MainForm_Load;
            this.LocationChanged += MainForm_LocationChanged;
        }

        private void InitSetting(string fileName)
        {
            var doc = new XDocument();
            XElement rabt = 
                new XElement("DocumentElement",
                    new XElement("FormSetting",
                        new XElement("IsFormTopMost", 0),
                        new XElement("IsFormFixed", 0),
                        new XElement("IPAddress", "127.0.0.1"),
                        new XElement("Port", "1893"),
                        new XElement("ButtonSize", "50,28"),
                        new XElement("DesiredLocation", "200,200"),
                        new XElement("IsKHZ", 0),
                        new XElement("ButtonCount", 4)));
            doc.Add(rabt);
            doc.Save(fileName);
        }

        private string MyDocument
        {
            get
            {
                return  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            
        }

        private void MyButtonSetting_Click(object sender, EventArgs e)
        {
            var edit = new AddEditForm();
            edit.mainForm = this;
            edit.ShowDialog(this);
            if (edit.isSave)
            {
                Fdmsw2BandsXml = new Data(FileName);
                Fdmsw2BandsXml.UpdateRadioAmBandTable(edit.AllRadioAmBandTables);
                SaveSetting();
                GetSetting();
                AddRABTButtons();

            }
            else
            {
                GetSetting();
            }
        }

        public string DesiredLocation;

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (IsFormFixed)
            {
                var locat = Array.ConvertAll(DesiredLocation.Split(','), Int32.Parse);
                this.Location = new Point(locat[0],locat[1]);
            }
            else
            {
                DesiredLocation = $"{this.Location.X},{this.Location.Y}";
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.GetSetting();
        }
        public void GetSetting()
        {
            Setting.GetFormSetting(this);

            this.Text = $"BAND-SW IP: {IPAddress}";

        }
        public void SaveSetting()
        {
            Setting.SaveFormSetting(this);

        }
        public void AddRABTButtons()
        {

            
            this.flowLayoutPanel1.Controls.Clear();
            this.flowLayoutPanel2.Controls.Clear();
            this.flowLayoutPanel3.Controls.Clear();

            var size = Array.ConvertAll(ButtonSize.Split(','), Int32.Parse);
            
            var set = new myButton();

            set.MinimumSize = new Size(size[0], size[1]);
            set.MaximumSize = new Size(size[0], size[1]);
            set.Click += MyButtonSetting_Click;
            set.Text = "SET";
            this.flowLayoutPanel2.Controls.Add(set);

            var close = new myButton();

            close.MinimumSize = new Size(size[0], size[1]);
            close.MaximumSize = new Size(size[0], size[1]);
            close.Click += buttonClose_Click;
            close.Text = "QUIT";

          

            this.flowLayoutPanel3.Controls.Add(close);


            panel1.Height = size[1] + 6 + 15;

            flowLayoutPanel2.MinimumSize = new Size((2 * (size[0] + 6)) + 10, 0);
            flowLayoutPanel2.MaximumSize = new Size((2 * (size[0] + 6)) + 10, 0);
            this.flowLayoutPanel1.Size = new Size((2 * (size[0] + 6)) + 10, this.Size.Width);


            this.flowLayoutPanel1.MinimumSize = new Size((ButtonCount * (size[0] + 6)) + 10, 0);
            this.flowLayoutPanel1.MaximumSize = new Size((ButtonCount * (size[0] + 6)) + 10, 0);
            this.flowLayoutPanel1.Size = new Size((ButtonCount * (size[0] + 6)) + 10, this.Size.Width);


            foreach (var allRadioAmBandTable in Fdmsw2BandsXml.GetAllRadioAmBandTables())
            {
                var btn = new myButton();
                btn.SetRadioAmBandTable(allRadioAmBandTable);
                btn.MinimumSize = new Size(size[0], size[1]);
                btn.MaximumSize = new Size(size[0], size[1]);
                btn.Click += Btn_Click;
                this.flowLayoutPanel1.Controls.Add(btn);
            }

           

        }

        private void Btn_Click(object sender, EventArgs e)
        {

            string message = "CF00;";
            string result = "";

            var client = new Client(System.Net.IPAddress.Parse(IPAddress), int.Parse(Port), 20000);
            var connected = client.Connect(out result);

            if (!connected)
            {

                MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SelectedButton != null)
            {
                var success = client.SendMessage(message, out result);
                if (success)
                {
                    //MessageBox.Show(result);
                    var test = result.Substring(4, result.Length - 4).Replace(";", "");

                    double dd;
                    double.TryParse(test, out dd);
                    if (dd > SelectedButton.StartFreq && dd < SelectedButton.StopFreq)
                    {
                        SelectedButton.Tune = dd;
                        Fdmsw2BandsXml.UpdateTune(SelectedButton.GetRadioAmBandTable());
                    }
                }
                else
                {
                    MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                SelectedButton.FlatAppearance.BorderColor = Color.White;
            }

            var selected = sender as myButton;
            SelectedButton = selected;
            selected.FlatAppearance.BorderColor = Color.FromArgb(237, 64, 3);

            if (SelectedButton.Tune == 0 || SelectedButton.Tune < SelectedButton.StartFreq ||
                SelectedButton.Tune > SelectedButton.StopFreq)
            {
                SelectedButton.Tune = (SelectedButton.StartFreq + SelectedButton.StopFreq) / 2;
                Fdmsw2BandsXml.UpdateTune(SelectedButton.GetRadioAmBandTable());
            }



            message = string.Format("CF00{0:00000000000};", SelectedButton.Tune);


            var isSuccess = client.SendMessage(message, out result);
            if (isSuccess)
            {

                //MessageBox.Show(result);
            }
            else
            {
                MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            client.Dispose();
        }


        public string IPAddress { get; set; }
        public string Port { get; set; }

        //public void AddButtons()
        //{

        //}

        private myButton SelectedButton;

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BS_Load(object sender, EventArgs e)
        {

        }


        //private void CloseButton_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void AddButton_Click(object sender, EventArgs e)
        //{
        //    var add = new AddEditForm();
        //    add.ShowDialog(this);
        //    if (add.isSave)
        //    {
        //        Fdmsw2BandsXml.AddRadioAmBandTable(add.MyButton.GetRadioAmBandTable());
        //        add.MyButton.SelectedRadioAmBandTable = add.MyButton.GetRadioAmBandTable();
        //        add.MyButton.Click += Btn_Click;
        //        this.flowLayoutPanel1.Controls.Add(add.MyButton);
        //    }
        //}
        //private void EditButton_Click(object sender, EventArgs e)
        //{
        //    var edit = new AddEditForm();
        //    if (SelectedButton == null)
        //    {
        //        MessageBox.Show("Please select one");
        //        return;
        //    }
        //    edit.MyButton = new myButton();
        //    edit.MyButton.SelectedRadioAmBandTable = SelectedButton.SelectedRadioAmBandTable;
        //    edit.ShowDialog(this);
        //    if (edit.isSave)
        //    {
        //        Fdmsw2BandsXml.EditRadioAmBandTable(edit.MyButton.GetRadioAmBandTable(), SelectedButton.SelectedRadioAmBandTable);
        //    }
        //}
        //private void DeleteButton_Click(object sender, EventArgs e)
        //{
        //    if (SelectedButton == null)
        //    {
        //        MessageBox.Show("Please select one");
        //        return;
        //    }
        //    Fdmsw2BandsXml.RemoveRadioAmBandTable(SelectedButton.SelectedRadioAmBandTable);
        //    this.flowLayoutPanel1.Controls.Remove(SelectedButton);
        //    SelectedButton = null;


        //}
    }
}
