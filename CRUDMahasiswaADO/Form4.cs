using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class DataMahasiswa : Form
    {
        // ── LANGKAH 14a: Deklarasi DAL (taruh di atas, sebelum constructor) ──
        DAL dbLogic = new DAL()

        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
