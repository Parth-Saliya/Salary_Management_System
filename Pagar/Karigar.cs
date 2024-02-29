using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pagar
{
    public partial class Karigar : Form
    {
        public Karigar()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["pag"].ConnectionString);

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "" && textBox2.Text != "")
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into karigarmaster values('" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox1.Text + "')", con);

                cmd.ExecuteNonQuery();

                con.Close();

                disp_karigar();
                clearall();

                
            }
        }

        public void disp_karigar()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from karigarmaster";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                textBox3.Text = row.Cells[0].Value.ToString();

                textBox1.Text = row.Cells[1].Value.ToString();

                textBox2.Text = row.Cells[2].Value.ToString();

                comboBox1.Text = row.Cells[3].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update karigarmaster set karigar = '" + textBox1.Text + "',no = '" + textBox2.Text + "',type ='"+comboBox1.Text+"' where id = '"+textBox3.Text+"'  ", con);

            cmd.ExecuteNonQuery();
            con.Close();
            disp_karigar();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("delete from karigarmaster where id = '" + textBox3.Text + "'  ", con);

                cmd.ExecuteNonQuery();
                con.Close();
                disp_karigar();
                clearall();


            }
        }

        private void Karigar_Load(object sender, EventArgs e)
        {
            disp_karigar();
        }

        public void clearall()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.ResetText();
        }

    }
}
