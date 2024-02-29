using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Pagar
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["pag"].ConnectionString);

        private void addNewKarigarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Karigar kar = new Karigar();
            kar.Show();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            getprice();
            // TODO: This line of code loads data into the 'pagarDataSet.karigarmaster' table. You can move, or remove it, as needed.
            this.karigarmasterTableAdapter.Fill(this.pagarDataSet.karigarmaster);

            FILLCOMBO();

        }

        public void FILLCOMBO()
        {
            con.Open();
            SqlCommand sc = new SqlCommand("select karigar from karigarmaster where type = '"+comboBox1.Text+"'", con);
            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("karigar", typeof(string));
          //  dt.Columns.Add("contactname", typeof(string));
            dt.Load(reader);

            comboBox2.ValueMember = "karigar";
            comboBox2.DisplayMember = "karigar";
            comboBox2.DataSource = dt;

            con.Close();
        }

        public void getprice()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from def where id = '1' ";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            con.Close();

             label13.Text = Convert.ToDouble(dataGridView2.Rows[0].Cells[1].Value).ToString();
             label14.Text = Convert.ToDouble(dataGridView2.Rows[0].Cells[2].Value).ToString();
             label15.Text = Convert.ToDouble(dataGridView2.Rows[0].Cells[3].Value).ToString();

        }

        public void countpcs()
        {

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from def where id = '1' ";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            con.Close();

            double label13 = Convert.ToDouble(dataGridView2.Rows[0].Cells[0].Value);
            double label14 = Convert.ToDouble(dataGridView2.Rows[0].Cells[1].Value);
            double label15 = Convert.ToDouble(dataGridView2.Rows[0].Cells[2].Value);

            double Totalpcs = 0;
            double a = 4.75;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                Totalpcs += Convert.ToDouble(dataGridView1.Rows[i].Cells["pcs"].Value);

            }

            label7.Text = Totalpcs.ToString();
            double totalAmount = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                totalAmount += Convert.ToDouble(dataGridView1.Rows[i].Cells["total"].Value);

            }
            label8.Text = totalAmount.ToString();

            /*
            if(comboBox1.Text == "Taliya")
            {
                label8.Text = (Totalpcs * taliya).ToString();
            }
            else if (comboBox1.Text == "Mathala")
            {
                label8.Text = (Totalpcs * mathala).ToString();
            }
            else if (comboBox1.Text == "Pahel")
            {
                label8.Text = (Totalpcs * pahel).ToString();

            

            }
            */



        }

        public void disp_entry()
        {
            DateTime now = DateTime.Now;
            int a = now.Month;
            
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from pagarentry where name = '"+comboBox2.Text+"' and MONTH(dt) = "+Convert.ToString(a )+" ";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
            countpcs();
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            FILLCOMBO();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox2.Text != "")
            {
                TextBox price = new TextBox();
                if (comboBox1.Text == "Taliya")
                {
                    price.Text = label13.Text;
                }
                if (comboBox1.Text == "Mathala")
                {
                    price.Text = label14.Text;
                }
                if (comboBox1.Text == "Pahel")
                {
                    price.Text = label15.Text;
                }
                double a = Convert.ToDouble( price.Text) ;
                double b = Convert.ToDouble(textBox1.Text);
                double c = a * b;
                
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into pagarentry values('" + comboBox2 .Text + "',convert(date,'" + dateTimePicker1.Text + "',5),'" + textBox1.Text + "','" + comboBox1.Text + "','"+price.Text+"','"+Convert.ToString(c )+"')", con);

                cmd.ExecuteNonQuery();

                con.Close();

                disp_entry();
                clearall();
                comboBox2.Focus();

            }
        }

        public void  clearall()
        {
            textBox1.Text = "";
            comboBox2.ResetText();
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            disp_entry();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Really Want to Update ??", label9.Text, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE pagarentry SET name='" + comboBox2.Text + "', dt=convert(date,'" + dateTimePicker1.Text + "',5), pcs ='" + textBox1.Text + "',kar_type = '"+comboBox1.Text+"' where (id='" + label9.Text + "')";

                cmd.ExecuteNonQuery();

                con.Close();
                disp_entry();
                MessageBox.Show("Entry Updated successfully");
                clearall();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                label9.Text = row.Cells[0].Value.ToString();
                comboBox2.Text = row.Cells[1].Value.ToString();
                dateTimePicker1.Text = row.Cells[2].Value.ToString();
                textBox1.Text = row.Cells[3].Value.ToString();
                comboBox1.Text = row.Cells[4].Value.ToString();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Really Want to Delete ??", label9.Text, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from pagarentry where (id='" + label9.Text + "')";

                cmd.ExecuteNonQuery();

                con.Close();
                disp_entry();
                MessageBox.Show("Entry Updated successfully");
                clearall();
            }
        }

        private void dateWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reportForm rf = new reportForm();
            rf.Show();
        }
    }
}
