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
    public partial class reportForm : Form
    {
        public reportForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["pag"].ConnectionString);
        public void FILLCOMBO()
        {
            con.Open();
            SqlCommand sc = new SqlCommand("select karigar from karigarmaster where type = '" + comboBox1.Text + "'", con);
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

        private void reportForm_Load(object sender, EventArgs e)
        {
            FILLCOMBO();
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            FILLCOMBO();
        }
        public void getreport()
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string str="";
            string mno = "";
            if(comboBox2.Text != "ALL")
            {
                str = " name = '" + comboBox2.Text + "' AND";
            }
            if (comboBox1.Text != "ALL")
            {
                mno  = " and kar_type = '" + comboBox1.Text + "' ";
            }
            if(comboBox1.Text == "ALL")
            {
                str = "";
                mno = "";
            }
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from pagarentry where "+str.ToString()+" dt Between '" + startDt.Value.ToString("MM/dd/yyyy") + "' AND '" + EndDt.Value.ToString("MM/dd/yyyy") + "' "+mno.ToString()+" ";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
