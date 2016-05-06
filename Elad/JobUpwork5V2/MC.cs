using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClientServerApp;

namespace JobUpwork5
{
    public partial class MC : Form
    {
        private MyFDMSW2BandsXML Fdmsw2BandsXml;
        public bool IsFormFixed { get; set; }
        public string ButtonSize { get; set; }
        public int ButtonCount { get; set; }
        public MC()
        {
            InitializeComponent();
            Fdmsw2BandsXml = new MyFDMSW2BandsXML();
            //this.AddButtons();
            this.AddRABTButtons();
            this.Load += MainForm_Load;
            this.LocationChanged += MainForm_LocationChanged;
        }



        private void MyButtonSetting_Click(object sender, EventArgs e)
        {
            var edit = new AddEditForm();
            edit.mainForm = this;
            edit.ShowDialog(this);
            if (edit.isSave)
            {
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

        private Point _desiredLocation;

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (_desiredLocation == new Point(0, 0))
            {
                _desiredLocation = this.Location;
            }
            if (IsFormFixed && this.Location != _desiredLocation)
            {
                this.Location = _desiredLocation;
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.GetSetting();
        }
        public void GetSetting()
        {
            Fdmsw2BandsXml.GetFormSetting(this);

            this.Text = $"BAND-SW IP: {IPAddress}";

        }
        public void SaveSetting()
        {
            Fdmsw2BandsXml.SaveFormSetting(this);

        }
        public void AddRABTButtons()
        {



            this.GetSetting();
            this.flowLayoutPanel1.Controls.Clear();
            this.flowLayoutPanel2.Controls.Clear();



            var size = Array.ConvertAll(ButtonSize.Split(','), Int32.Parse);

            this.flowLayoutPanel1.MinimumSize = new Size(ButtonCount * (size[0] + 10), 0);
            this.flowLayoutPanel1.MaximumSize = new Size(ButtonCount * (size[0] + 10), 0);
            this.flowLayoutPanel1.Size = new Size(ButtonCount * (size[0] + 10), this.Size.Width);

            foreach (var allRadioAmBandTable in Fdmsw2BandsXml.GetAllRadioAmBandTables())
            {
                var btn = new myButton();
                btn.SetRadioAmBandTable(allRadioAmBandTable);
                btn.MinimumSize = new Size(size[0], size[1]);
                btn.MaximumSize = new Size(size[0], size[1]);
                btn.Click += Btn_Click;
                this.flowLayoutPanel1.Controls.Add(btn);
            }

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
            this.flowLayoutPanel2.Controls.Add(close);

        }


        private void Btn_Click(object sender, EventArgs e)
        {

            string result;
            var client = new Client(System.Net.IPAddress.Parse(IPAddress), int.Parse(Port), 20000);

            var connected = client.Connect(out result);


            if (!connected)
            {
                MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SelectedButton != null)
            {
                SelectedButton.FlatAppearance.BorderColor = Color.White;
            }
            var selected = sender as myButton;
            SelectedButton = selected;
            selected.FlatAppearance.BorderColor = Color.FromArgb(237, 64, 3);

            if (!string.IsNullOrEmpty(SelectedButton.Command))
            {
                var cmds = SelectedButton.Command.Split(';');
                bool isSuccess = true;

                foreach (var cmd in cmds)
                {
                    if (!string.IsNullOrEmpty(cmd) && isSuccess)
                    {
                        string message = cmd + ";";

                        isSuccess = client.SendMessage(message, out result);
                        if (isSuccess)
                        {

                            //MessageBox.Show(result);
                        }
                        else
                        {
                            MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                client.Dispose();
            }

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

