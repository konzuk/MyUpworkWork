﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace JobUpwork5
{
    public partial class AddEditForm : Form
    {
        private MyFDMSW2BandsXML fdmsw2BandsXml { get; set; }
        public MC mainForm { get; set; }
        public AddEditForm()
        {
            InitializeComponent();
            this.Load += AddEditForm_Load;
            fdmsw2BandsXml = new MyFDMSW2BandsXML();
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            dataGridView1.CellValidating += DataGridView1_CellValidating;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

        public RadioAmBandTable SelectedRadioAmBandTable
        {
            get
            {
                try
                {
                    if (this.dataGridView1.SelectedCells.Count > 0)
                    {
                        return this.dataGridView1.SelectedCells[0].OwningRow.DataBoundItem  as RadioAmBandTable;
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
            if (this.dataGridView1.Columns[e.ColumnIndex] == this.Color)
            {
                if (AllRadioAmBandTables != null && AllRadioAmBandTables.Any())
                {
                    var model = AllRadioAmBandTables[e.RowIndex];
                    e.CellStyle.BackColor = model.Rgb;
                }
            }
        }

        private void AddEditForm_Load(object sender, EventArgs e)
        {
            this.Text = "Setting";    
            
            this.BindModel();
        }

        public IList<RadioAmBandTable> AllRadioAmBandTables; 
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

                this.textBoxButtonSize.DataBindings.Clear();
                this.textBoxButtonSize.DataBindings.Add("Text", this.mainForm, "ButtonSize");
                this.textBoxButtonCount.DataBindings.Clear();
                this.textBoxButtonCount.DataBindings.Add("Text", this.mainForm, "ButtonCount");

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
                this.Command.DataPropertyName = "Command";

                AllRadioAmBandTables = fdmsw2BandsXml.GetAllRadioAmBandTables();
                this.dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = AllRadioAmBandTables;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool isSave = false;
        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            isSave = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            isSave = false;
            this.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var model = new RadioAmBandTable();
            model.Rgb = System.Drawing.Color.Black;
            AllRadioAmBandTables.Add(model);
            dataGridView1.EndEdit();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = AllRadioAmBandTables;
            
            dataGridView1.CurrentCell = dataGridView1.Rows[AllRadioAmBandTables.Count - 1].Cells[this.Label.Index];
            dataGridView1.BeginEdit(true);

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            AllRadioAmBandTables.Remove(SelectedRadioAmBandTable);
            dataGridView1.EndEdit();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = AllRadioAmBandTables;
            if(AllRadioAmBandTables.Any())
            dataGridView1.BeginEdit(true);
        }
    }
}