using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManagement
{
    public partial class EmployeeHistory : Form
    {
        public EmployeeHistory()
        {
            InitializeComponent();

        }


        Connection conn = new Connection();
        SqlConnection con;
        private int id;
        private string selectedemployee, name;

        private void button2_Click(object sender, EventArgs e)
        {
            string employeeId = textBox1.Text;

            if (!string.IsNullOrEmpty(employeeId))
            {
                LoadEmployeeWorkHoursById(employeeId);
            }
            else
            {
                MessageBox.Show("Please enter an employee ID.");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadEmployeeWorkHoursById(string employeeId)
        {
            try
            {
                con = conn.getCon();
                con.Open();


                SqlCommand command = new SqlCommand("SELECT * FROM EmployeeWorkHours WHERE id = @id", con);
                command.Parameters.AddWithValue("@id", employeeId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("No work hours found for the given ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading work hours: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close(); 
                }
            }
        }

    }
}
