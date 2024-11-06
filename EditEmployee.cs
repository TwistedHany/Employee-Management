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

namespace EmployeeManagement
{
    public partial class EditEmployee : Form
    {
        
        public EditEmployee()
        {
            InitializeComponent();
        }
        Connection conn = new Connection();
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader rd;
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con = conn.getCon();
                con.Open();

                // Initialize the SqlCommand 
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;  // Set the connection for the SqlCommand

                // parameters
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                cmd.Parameters.AddWithValue("@postion", textBox2.Text);
                cmd.Parameters.AddWithValue("@number", textBox3.Text);
                cmd.Parameters.AddWithValue("@id", textBox4.Text);  // Make sure this is set appropriately

                // Set the command text (query)
                cmd.CommandText = "UPDATE OfficeData SET name=@name, postion=@postion, number=@number WHERE id=@id";

                // Execute the query
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Employee updated");
                }
                else
                {
                    MessageBox.Show("Employee not updated");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                Close();
            }
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
