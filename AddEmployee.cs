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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EmployeeManagement
{
    public partial class AddEmployee : Form
    {
        public AddEmployee()
        {
            InitializeComponent();
        }

        Connection con = new Connection();
        SqlCommand cmd;
        SqlConnection conn;
        SqlDataReader rd;

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                conn = con.getCon();
                conn.Open();


                SqlCommand command = new SqlCommand("INSERT INTO OfficeData (name, postion, number) VALUES(@name, @postion, @number)", conn);

                command.Parameters.AddWithValue("@name", textBox1.Text);
                command.Parameters.AddWithValue("@postion", textBox2.Text);
                command.Parameters.AddWithValue("@number", textBox3.Text);


                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Employee Added");
                }
                else
                {
                    MessageBox.Show("Employee not Added");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {

                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}