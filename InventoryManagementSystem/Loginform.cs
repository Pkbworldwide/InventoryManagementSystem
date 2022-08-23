using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class Loginform : Form
    {
        public Loginform()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPass.Checked == false)
                textPass.UseSystemPasswordChar = true;
            else
                textPass.UseSystemPasswordChar= false;
        }

        private void labelclear_Click(object sender, EventArgs e)
        {
            textName.Clear();
            textPass.Clear();
        }

        private void buttonclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
