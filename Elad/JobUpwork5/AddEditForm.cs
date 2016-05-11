using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace JobUpwork5
{
    public partial class AddEditForm : Form
    {
        private Data fdmsw2BandsXml { get; set; }
        public BS mainForm { get; set; }

        private bool dataChanged;

        public AddEditForm()
        {
            InitializeComponent();
            this.Load += AddEditForm_Load;
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            dataGridView1.CellValidating += DataGridView1_CellValidating;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;


        }

        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dataChanged = true;
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dataChanged = true;
        }

        public RadioAmBandTable SelectedRadioAmBandTable
        {
            get
            {
                try
                {
                    if (this.dataGridView1.SelectedCells.Count > 0)
                    {
                        return this.dataGridView1.SelectedCells[0].OwningRow.DataBoundItem as RadioAmBandTable;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == Color.Index)
            {
                var result = colorDialog1.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    SelectedRadioAmBandTable.Rgb = colorDialog1.Color;
                    dataGridView1.Refresh();
                    dataGridView1.ClearSelection();
                }
            }
        }

        private void DataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == this.Start.Index || e.ColumnIndex == this.Stop.Index || e.ColumnIndex == this.BW.Index ||
                e.ColumnIndex == this.Tune.Index)
            {
                double value;
                if (this.dataGridView1[e.ColumnIndex, e.RowIndex].IsInEditMode)
                {
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        this.dataGridView1.CancelEdit();
                    }
                    else if (!double.TryParse(e.FormattedValue.ToString(), out value))
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dataGridView1.Columns[e.ColumnIndex] == this.Color)
                {
                    if (AllRadioAmBandTables != null && AllRadioAmBandTables.Any())
                    {
                        var model = AllRadioAmBandTables[e.RowIndex];
                        e.CellStyle.BackColor = model.Rgb;
                    }
                }
                if (this.dataGridView1.Columns[e.ColumnIndex] == this.Tune || this.dataGridView1.Columns[e.ColumnIndex] == this.Start || this.dataGridView1.Columns[e.ColumnIndex] == this.Stop)
                {

                    //double value = e.Value as double? ?? 0;
                    //e.Value = /*model.IsKHZ ?*/ $"{value:#,##0.000}" /*: $"{value:#,##0}"*/;

                    if (AllRadioAmBandTables != null && AllRadioAmBandTables.Any())
                    {
                        var model = AllRadioAmBandTables[e.RowIndex];



                        if (model.IsValid)
                        {
                            e.CellStyle.ForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            e.CellStyle.ForeColor = System.Drawing.Color.Red;
                        }
                    }


                }
            }
            catch
            {


            }



        }
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
            this.labelVersion.Text = $"Version: {CurrentVersion}";
            this.Start.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight;
            this.BW.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight;
            this.Stop.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight;
            this.Tune.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight;

            this.Start.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            this.BW.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            this.Stop.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            this.Tune.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            this.Text = "Setting";
            fdmsw2BandsXml = new Data(mainForm.FileName);
            this.BindModel();
        }

        public IList<RadioAmBandTable> AllRadioAmBandTables;

        private void BindGrid(IList<RadioAmBandTable> arbt)
        {
            if (!arbt.Any()) return;

            this.Enabled.DataPropertyName = "Enabled";
            this.Color.ReadOnly = true;
            //this.Color.DataPropertyName = "Rgb";
            this.Label.DataPropertyName = "Label";
            this.Start.DataPropertyName = "StartFreq";
            this.Stop.DataPropertyName = "StopFreq";
            this.Mode.DataPropertyName = "Mode";
            this.BW.DataPropertyName = "BW";
            this.Tune.DataPropertyName = "Tune";
            this.Note.DataPropertyName = "Note";
            //this.Command.DataPropertyName = "Command";

            this.dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = arbt;
        }

        public void BindModel()
        {
            try
            {
                this.checkBoxTopMost.DataBindings.Clear();
                this.checkBoxTopMost.DataBindings.Add("Checked", this.mainForm, "TopMost");
                this.checkBox2.DataBindings.Clear();
                this.checkBox2.DataBindings.Add("Checked", this.mainForm, "IsFormFixed");
                this.textBoxIP.DataBindings.Clear();
                this.textBoxIP.DataBindings.Add("Text", this.mainForm, "IPAddress");
                this.textBoxPort.DataBindings.Clear();
                this.textBoxPort.DataBindings.Add("Text", this.mainForm, "Port");
                this.textBoxFileName.DataBindings.Clear();
                this.textBoxFileName.DataBindings.Add("Text", this.mainForm, "FileName");


                this.textBoxButtonSize.DataBindings.Clear();
                this.textBoxButtonSize.DataBindings.Add("Text", this.mainForm, "ButtonSize");
                this.textBoxButtonCount.DataBindings.Clear();
                this.textBoxButtonCount.DataBindings.Add("Text", this.mainForm, "ButtonCount");

                AllRadioAmBandTables = fdmsw2BandsXml.GetAllRadioAmBandTables();
                BindGrid(AllRadioAmBandTables);


                this.checkBoxKHZ.DataBindings.Clear();
                this.checkBoxKHZ.DataBindings.Add("Checked", this.mainForm, "IsKHZ");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool isSave = false;

        private void buttonSave_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            if (AllRadioAmBandTables.All(s => s.IsValid) || MessageBox.Show("The start frequency, start frequency and tune is not valid. \nDo you want to save anyway?", "Invalid Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                isSave = true;
                this.mainForm.SaveFileName(this.textBoxFileName.Text);
                this.Close();
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            isSave = false;
            this.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            RadioAmBandTable model = null;
            if (SelectedRadioAmBandTable != null)
            {
                model = SelectedRadioAmBandTable.Clone();
            }
            else
            {
                model = new RadioAmBandTable();
                model.Rgb = System.Drawing.Color.Black;
                model.IsKHZ = this.checkBoxKHZ.Checked;
            }

            dataGridView1.EndEdit();
            dataGridView1.DataSource = null;
            AllRadioAmBandTables.Add(model);

            BindGrid(AllRadioAmBandTables);
            var row = dataGridView1.Rows[AllRadioAmBandTables.Count - 1].Cells[this.Label.Index];
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = row;
            dataChanged = true;
            dataGridView1.BeginEdit(true);

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            AllRadioAmBandTables.Remove(SelectedRadioAmBandTable);
            dataGridView1.EndEdit();
            dataGridView1.DataSource = null;
            BindGrid(AllRadioAmBandTables);
            dataChanged = true;
            if (AllRadioAmBandTables.Any())
                dataGridView1.BeginEdit(true);
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            var cell = dataGridView1.CurrentCell.ColumnIndex;

            var ind = AllRadioAmBandTables.IndexOf(SelectedRadioAmBandTable);
            if (ind == 0) return;
            var old = AllRadioAmBandTables[ind - 1];
            AllRadioAmBandTables[ind - 1] = AllRadioAmBandTables[ind];
            AllRadioAmBandTables[ind] = old;


            dataGridView1.EndEdit();
            dataGridView1.DataSource = null;
            BindGrid(AllRadioAmBandTables);

            var row = dataGridView1.Rows[ind - 1].Cells[cell];
            dataGridView1.CurrentCell = row;

            if (AllRadioAmBandTables.Any() && dataGridView1.CurrentCell is DataGridViewTextBoxCell)
                dataGridView1.BeginEdit(true);
            dataChanged = true;
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            var cell = dataGridView1.CurrentCell.ColumnIndex;

            var ind = AllRadioAmBandTables.IndexOf(SelectedRadioAmBandTable);
            if (ind == AllRadioAmBandTables.Count - 1) return;
            var old = AllRadioAmBandTables[ind + 1];
            AllRadioAmBandTables[ind + 1] = AllRadioAmBandTables[ind];
            AllRadioAmBandTables[ind] = old;


            dataGridView1.EndEdit();
            dataGridView1.DataSource = null;
            BindGrid(AllRadioAmBandTables);

            var row = dataGridView1.Rows[ind + 1].Cells[cell];
            dataGridView1.CurrentCell = row;

            if (AllRadioAmBandTables.Any() && dataGridView1.CurrentCell is DataGridViewTextBoxCell)
                dataGridView1.BeginEdit(true);
            dataChanged = true;
        }

        private void checkBoxKHZ_CheckedChanged(object sender, EventArgs e)
        {
            if (AllRadioAmBandTables != null && AllRadioAmBandTables.Any())
            {
                var check = (sender as CheckBox).Checked;
                foreach (var allRadioAmBandTable in AllRadioAmBandTables)
                {
                    allRadioAmBandTable.IsKHZ = check;
                }
                dataGridView1.ClearSelection();
                dataGridView1.Refresh();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();

            var dic = Path.GetDirectoryName(textBoxFileName.Text);


            openFileDialog1.InitialDirectory = dic;


            openFileDialog1.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog1.FileName = textBoxFileName.Text;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if (openFileDialog1.FileName.Equals(textBoxFileName.Text)) return;
                textBoxFileName.Text = openFileDialog1.FileName;
                //mainForm.FileName = openFileDialog1.FileName;


                if (dataChanged)
                {
                    var save = MessageBox.Show("You have make some chnage in current XML fiel. Do you want to save it?",
                        "Confirm Change File", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (save == DialogResult.Yes)
                    {
                        fdmsw2BandsXml.UpdateRadioAmBandTable(AllRadioAmBandTables);
                    }

                    dataChanged = false;
                }

                fdmsw2BandsXml = new Data(textBoxFileName.Text);
                dataGridView1.DataSource = null;
                AllRadioAmBandTables = fdmsw2BandsXml.GetAllRadioAmBandTables();
                foreach (var allRadioAmBandTable in AllRadioAmBandTables)
                {
                    allRadioAmBandTable.IsKHZ = checkBoxKHZ.Checked;
                    dataGridView1.ClearSelection();
                    dataGridView1.Refresh();
                }

                BindGrid(AllRadioAmBandTables);

            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            var dic = Path.GetDirectoryName(textBoxFileName.Text);


            saveFileDialog1.InitialDirectory = dic;


            saveFileDialog1.Filter = "XML Files (*.xml)|*.xml";
            saveFileDialog1.FileName = "";
            saveFileDialog1.CheckPathExists = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if (saveFileDialog1.FileName.Equals(textBoxFileName.Text)) return;
                textBoxFileName.Text = saveFileDialog1.FileName;

                var doc = new XDocument();
                XElement rabt = new XElement("DocumentElement");
                doc.Add(rabt);
                doc.Save(textBoxFileName.Text);

                fdmsw2BandsXml = new Data(textBoxFileName.Text);

                fdmsw2BandsXml.UpdateRadioAmBandTable(AllRadioAmBandTables);
                
                dataGridView1.DataSource = null;
                AllRadioAmBandTables = fdmsw2BandsXml.GetAllRadioAmBandTables();
                foreach (var allRadioAmBandTable in AllRadioAmBandTables)
                {
                    allRadioAmBandTable.IsKHZ = checkBoxKHZ.Checked;
                    dataGridView1.ClearSelection();
                    dataGridView1.Refresh();
                }

                BindGrid(AllRadioAmBandTables);

                dataChanged = false;
            }
        }
    }
}
