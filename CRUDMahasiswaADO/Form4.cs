using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CRUDMahasiswaADO
{
    public partial class Form4 : Form
    {
        // ── VARIABLE DECLARATIONS ─────────────────────────────────────────
        DAL dbLogic = new DAL();
        bool isInitializing = true;
        DataTable dt;
        int button = 0;

        // ── CONSTRUCTOR ───────────────────────────────────────────────────
        public Form4()
        {
            InitializeComponent();

            // Setup DateTimePicker
            dateTimePicker1.MinDate = new DateTime(2000, 1, 1);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.MaxDate = DateTime.Now;

            // Setup ComboBox
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            var items = new List<KeyValuePair<string, SeriesChartType>>
            {
                new KeyValuePair<string, SeriesChartType>("Kolom", SeriesChartType.Column),
                new KeyValuePair<string, SeriesChartType>("Pie", SeriesChartType.Pie)
            };

            isInitializing = true;
            comboBox1.DataSource = items;
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
            comboBox1.SelectedIndex = 0;
            isInitializing = false;

            loadDataChart();
        }

        // ── LOAD CHART ────────────────────────────────────────────────────
        public void loadDataChart()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Legends.Clear();
            chart1.ChartAreas.Clear();

            ChartArea ca = new ChartArea("MainArea");
            ca.AxisX.Title = "Program Studi";
            ca.AxisY.Title = "Jumlah Mahasiswa";
            ca.AxisX.LabelStyle.Angle = -45;
            ca.BackColor = Color.Transparent;
            chart1.ChartAreas.Add(ca);

            try
            {
                if (button == 1)
                {
                    dt = dbLogic.getDataChartByTahun(dateTimePicker1.Value);
                }
                else
                {
                    dt = dbLogic.getAllDataChart();
                }

                SeriesChartType tipe = (SeriesChartType)comboBox1.SelectedValue;

                if (tipe == SeriesChartType.Column)
                {
                    Series s = new Series("Mahasiswa");
                    s.ChartType = SeriesChartType.Column;

                    foreach (DataRow row in dt.Rows)
                    {
                        string prodi = row["NamaProdi"].ToString();
                        int jumlah = Convert.ToInt32(row["JmlhMhs"]); // ✅ fix
                        s.Points.AddXY(prodi, jumlah);
                    }

                    chart1.Series.Add(s);
                }
                else
                {
                    Series s = new Series("Jumlah Mahasiswa");
                    s.ChartType = tipe;
                    s.IsValueShownAsLabel = true;
                    s.Label = "#VAL";
                    s.LegendText = "#VALX";

                    foreach (DataRow row in dt.Rows)
                    {
                        string prodi = row["NamaProdi"].ToString();
                        int jumlah = Convert.ToInt32(row["JmlhMhs"]); // ✅ fix
                        s.Points.AddXY(prodi, jumlah);
                    }

                    chart1.Series.Add(s);
                }

                Title title = new Title(
                    "Jumlah Mahasiswa per Program Studi",
                    Docking.Top,
                    new Font("Arial", 14, FontStyle.Bold),
                    Color.DarkBlue);
                chart1.Titles.Add(title);

                Legend legend = new Legend("MainLegend");
                legend.Docking = Docking.Right;
                chart1.Legends.Add(legend);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        // ── EVENT: ComboBox SelectedValueChanged ──────────────────────────
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (isInitializing)
                return;

            if (button == 1)
            {
                // tidak reload otomatis
            }
            else
            {
                loadDataChart();
            }
        }

        // ── EVENT: Button Load ────────────────────────────────────────────
        private void btnLoad_Click(object sender, EventArgs e)
        {
            button = 1;
            loadDataChart();
        }

        // ── EVENT: Button Reset ───────────────────────────────────────────
        private void btnReset_Click(object sender, EventArgs e)
        {
            button = 0;
            loadDataChart();
        }

        // ── EVENT: Button Data Mahasiswa ──────────────────────────────────
        private void btnDataMhs_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Show();
            this.Hide();
        }

        // ── EVENT: Label Click ────────────────────────────────────────────
        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}