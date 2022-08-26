using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryManagementSystem
{
    public partial class OrderModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HP\OneDrive\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        int qty = 0;
        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadProduct();
        }

        private void buttonclose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            
        }

       
        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cmd = new SqlCommand("SELECT cid,cname FROM tbCustomer WHERE CONCAT(cid,cname) LIKE '%" + textSearchCust.Text + "%'", con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString());

            }
            dr.Close();
            con.Close();
        }
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cmd = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pid,pname,pprice,pdescription,pcategory) LIKE '%" + textSearchProd.Text + "%' ", con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());

            }
            dr.Close();
            con.Close();
        }

        private void textSearchCust_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void textSearchProd_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            GetQty();
            if(Convert.ToInt16(numericQty.Value)> qty)
            {
                MessageBox.Show("Instock quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numericQty.Value = numericQty.Value - 1;
                return;
            }
            if (Convert.ToInt16(numericQty.Value) > 0)
            {
                int total = Convert.ToInt16(textPrice.Text) * Convert.ToInt16(numericQty.Value);
                textTotal.Text = total.ToString();
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textCid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            textCname.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textPid.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
            textPname.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
            textPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();

        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (textCid.Text == "")
                {
                    MessageBox.Show("Please select customer", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (textPid.Text == "")
                {
                    MessageBox.Show("Please select product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to Insert this order?", "Inserting Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new SqlCommand("INSERT INTO tbOrder(odate,pid,cid,qty,price,total)VALUES(@odate,@pid,@cid,@qty,@price,@total)", con);
                    cmd.Parameters.AddWithValue("@odate", dateOrder.Value);
                    cmd.Parameters.AddWithValue("@pid", Convert.ToInt16(textPid.Text));
                    cmd.Parameters.AddWithValue("@cid", Convert.ToInt16(textCid.Text));
                    cmd.Parameters.AddWithValue("@qty", Convert.ToInt16(numericQty.Text));
                    cmd.Parameters.AddWithValue("@price", textPrice.Text);
                    cmd.Parameters.AddWithValue("@total", Convert.ToInt16(textTotal.Text));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Order has been successfully inserted.");
                    

                    cmd = new SqlCommand("UPDATE tbProduct SET pqty = (pqty - @pqty) WHERE pid LIKE '" + textPid.Text + "'", con);
                    cmd.Parameters.AddWithValue("@pqty",Convert.ToInt16(numericQty.Text));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Clear();
                    LoadProduct();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void Clear()
        {
            textCid.Clear();
            textCname.Clear();

            textPid.Clear();
            textPname.Clear();

            textPrice.Clear();
            numericQty.Value = 1;
            textTotal.Clear();
            dateOrder.Value = DateTime.Now;

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();                    
        }
        public void GetQty()
        {
            cmd = new SqlCommand("SELECT pqty FROM tbProduct WHERE pid = '" + textPid.Text + "' ", con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                qty = Convert.ToInt32(dr[0].ToString());

            }
            dr.Close();
            con.Close();
        }

       
    }
}
