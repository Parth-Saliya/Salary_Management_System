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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["pag"].ConnectionString);

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "" && textBox2.Text != "")
            {
                con.Open();
                SqlCommand cmdl = new SqlCommand("select count(*) from userm where username = '" + textBox1.Text + "' and password = '" + textBox2.Text + "'", con);


                cmdl.ExecuteNonQuery();
                var resl = cmdl.ExecuteScalar();
                var USERNAME = textBox1.Text;
                int gg = Convert.ToInt32(resl.ToString());

                con.Close();

                if (gg == 1)
                {
                    this.Hide();
                    Home f7 = new Home ();
                    f7.Show();
                }
                else
                {
                    MessageBox.Show("USERNAME AND PASSWORD NOT MATCHED");
                }
            }
        }
    }
}
