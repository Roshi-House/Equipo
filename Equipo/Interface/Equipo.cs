using Equipo.Conexion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Equipo
{
    public partial class Equipo : Form
    {

        MarcaTabla obje = new MarcaTabla();

        public Equipo()
        {
            InitializeComponent();
        }

        private void Equipo_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = obje.VistaTabla();

        }
    }
}
