using System;
using System.Drawing;
using System.Windows.Forms;

namespace ASA62
{
    public partial class ASA62 : Form
    {
        private ASA62Setting asa62Setting;


        public ASA62()
        {
            InitializeComponent();


            asa62Setting = new ASA62Setting();

            GetSetting();

            Load += MainForm_Load;
            LocationChanged += MainForm_LocationChanged;
        }


        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (asa62Setting.IsFixLocation)
            {
                var locat = Array.ConvertAll(asa62Setting.DesiredLocation.Split(','), int.Parse);
                Location = new Point(locat[0], locat[1]);
            }
            else
            {
                asa62Setting.DesiredLocation = $"{Location.X},{Location.Y}";
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void GetSetting()
        {
            asa62Setting = asa62Setting.ReadPMUSetting();

            TopMost = asa62Setting.IsTopMost;
            AddRABTButtons();
        }

        private void AddRABTButtons()
        {
            #region FormSizeSet

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel3.Controls.Clear();

            var size = Array.ConvertAll(asa62Setting.ButtonSize.Split(','), int.Parse);

            var btnSize = new Size(size[0], size[1]);

            panel1.Height = size[1] + 6 + 15;

            flowLayoutPanel2.MinimumSize = new Size(2*(size[0] + 6) + 10, 0);
            flowLayoutPanel2.MaximumSize = new Size(2*(size[0] + 6) + 10, 0);
            flowLayoutPanel1.Size = new Size(2*(size[0] + 6) + 10, Size.Width);


            flowLayoutPanel1.MinimumSize = new Size(7*(size[0] + 6) + 10, 0);
            flowLayoutPanel1.MaximumSize = new Size(7*(size[0] + 6) + 10, 0);
            flowLayoutPanel1.Size = new Size(7*(size[0] + 6) + 10, Size.Width);

            #endregion

            #region SettingButton

            var set = new myButton();

            set.MinimumSize = btnSize;
            set.MaximumSize = btnSize;
            set.Click += MyButtonSetting_Click;
            set.Text = "SET";
            flowLayoutPanel2.Controls.Add(set);

            #endregion

            #region CloseButton

            var close = new myButton();

            close.MinimumSize = btnSize;
            close.MaximumSize = btnSize;
            close.Click += buttonClose_Click;
            close.Text = "QUIT";

            flowLayoutPanel3.Controls.Add(close);

            #endregion

            #region StaticButton

            myButton btnRX1Loc = new myButton(btnSize, "LOC"),
                btnRX1_1 = new myButton(btnSize, "1"),
                btnRX1_2 = new myButton(btnSize, "2"),
                btnRX1_3 = new myButton(btnSize, "3"),
                btnRX1_4 = new myButton(btnSize, "4"),
                btnRX1_5 = new myButton(btnSize, "5"),
                btnRX1_6 = new myButton(btnSize, "6"),
                btnRX2Loc = new myButton(btnSize, "LOC"),
                btnRX2_1 = new myButton(btnSize, "1"),
                btnRX2_2 = new myButton(btnSize, "2"),
                btnRX2_3 = new myButton(btnSize, "3"),
                btnRX2_4 = new myButton(btnSize, "4"),
                btnRX2_5 = new myButton(btnSize, "5"),
                btnRX2_6 = new myButton(btnSize, "6");

            flowLayoutPanel1.Controls.Add(btnRX1Loc);
            flowLayoutPanel1.Controls.Add(btnRX1_1);
            flowLayoutPanel1.Controls.Add(btnRX1_2);
            flowLayoutPanel1.Controls.Add(btnRX1_3);
            flowLayoutPanel1.Controls.Add(btnRX1_4);
            flowLayoutPanel1.Controls.Add(btnRX1_5);
            flowLayoutPanel1.Controls.Add(btnRX1_6);
            flowLayoutPanel1.Controls.Add(btnRX2Loc);
            flowLayoutPanel1.Controls.Add(btnRX2_1);
            flowLayoutPanel1.Controls.Add(btnRX2_2);
            flowLayoutPanel1.Controls.Add(btnRX2_3);
            flowLayoutPanel1.Controls.Add(btnRX2_4);
            flowLayoutPanel1.Controls.Add(btnRX2_5);
            flowLayoutPanel1.Controls.Add(btnRX2_6);

            #endregion

            #region StaticButtonEvent

            btnRX1Loc.Click += BtnRX1Loc_Click;
            btnRX1_1.Click += BtnRX1_1_Click;
            btnRX1_2.Click += BtnRX1_2_Click;
            btnRX1_3.Click += BtnRX1_3_Click;
            btnRX1_4.Click += BtnRX1_4_Click;
            btnRX1_5.Click += BtnRX1_5_Click;
            btnRX1_6.Click += BtnRX1_6_Click;
            btnRX2Loc.Click += BtnRX2Loc_Click;
            btnRX2_1.Click += BtnRX2_1_Click;
            btnRX2_2.Click += BtnRX2_2_Click;
            btnRX2_3.Click += BtnRX2_3_Click;
            btnRX2_4.Click += BtnRX2_4_Click;
            btnRX2_5.Click += BtnRX2_5_Click;
            btnRX2_6.Click += BtnRX2_6_Click;

            #endregion
        }

        private void BtnRX2_6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX2_6_Click");
        }

        private void BtnRX2_5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX2_5_Click");
        }

        private void BtnRX2_4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX2_4_Click");
        }

        private void BtnRX2_3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX2_3_Click");
        }

        private void BtnRX2_2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX2_2_Click");
        }

        private void BtnRX2_1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX2_1_Click");
        }

        private void BtnRX2Loc_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX2Loc_Click");
        }

        private void BtnRX1_6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX1_6_Click");
        }

        private void BtnRX1_5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX1_5_Click");
        }

        private void BtnRX1_4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX1_4_Click");
        }

        private void BtnRX1_3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX1_3_Click");
        }

        private void BtnRX1_2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX1_2_Click");
        }

        private void BtnRX1_1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX1_1_Click");
        }

        private void BtnRX1Loc_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implement in BtnRX1Loc_Click");
        }

        private void MyButtonSetting_Click(object sender, EventArgs e)
        {
            var edit = new SettingForm();
            edit.Asa62Setting = asa62Setting;
            edit.ShowDialog(this);
            GetSetting();
        }


        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}