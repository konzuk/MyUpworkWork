using System;
using System.Deployment.Application;
using System.Reflection;
using System.Windows.Forms;

namespace ASA62
{
    public partial class SettingForm : Form
    {
        private bool dataChanged;

        public SettingForm()
        {
            InitializeComponent();
            Load += AddEditForm_Load;
        }

        public ASA62Setting Asa62Setting { get; set; }


        public string CurrentVersion
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed
                    ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                    : Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        private void AddEditForm_Load(object sender, EventArgs e)
        {
            Text = "Setting";
            BindModel();
        }


        public void BindModel()
        {
            try
            {
                checkBoxTopMost.DataBindings.Clear();
                checkBoxTopMost.DataBindings.Add("Checked", Asa62Setting, "IsTopMost");
                checkBox2.DataBindings.Clear();
                checkBox2.DataBindings.Add("Checked", Asa62Setting, "IsFixLocation");
                textBoxButtonSize.DataBindings.Clear();
                textBoxButtonSize.DataBindings.Add("Text", Asa62Setting, "ButtonSize");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            Asa62Setting.WritePMUSetting();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}