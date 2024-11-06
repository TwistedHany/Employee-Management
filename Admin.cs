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
    public partial class Admin : Form
    {
        private int id;
        private string selectedeployee, name;
        public Admin()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.Admin_Load);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEmployee add = new AddEmployee();
            Hide();
            add.ShowDialog();
            Show();
            LoadEmployees();
        }
        Connection conn = new Connection();
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader rd;
        private List<Personnel> Personnels = new List<Personnel>();
        private void LoadEmployees()
        {
            try
            {
                con = conn.getCon();
                con.Open();

                SqlCommand command = new SqlCommand("SELECT name FROM OfficeData ORDER BY name ASC", con);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    listBox1.Items.Clear();
                    while (reader.Read())
                    {
                        listBox1.Items.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading employees: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void LoadEmployeeDetails(string employeeName)
        {
            try
            {
                con = conn.getCon();
                con.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM OfficeData WHERE name = @name", con);
                command.Parameters.AddWithValue("@name", employeeName);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("No details found for the selected employee.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading employee details: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void LoadAllOfficeData()
        {
            try
            {
                con = conn.getCon(); // Get the database connection
                con.Open(); // Open the connection

                SqlCommand command = new SqlCommand("SELECT * FROM OfficeData", con); // Select all data from the OfficeData table

                SqlDataAdapter adapter = new SqlDataAdapter(command); // Adapter to fill DataTable with results
                DataTable dt = new DataTable(); // DataTable to hold the data
                adapter.Fill(dt); // Fill the DataTable with data

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("No data found in OfficeData.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void deleteEmployee(string employeeName)
        {
            try
            {
                SqlCommand command = new SqlCommand("DELETE FROM OfficeData WHERE name = @name", con);
                command.Parameters.AddWithValue("@name", employeeName);

                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Employee deleted successfully");
                    LoadEmployees();
                }
                else
                {
                    MessageBox.Show("Employee not found or could not be deleted");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting employee: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditEmployee edit = new EditEmployee();
            Hide();
            edit.ShowDialog();
            Show();
            LoadEmployees();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedeployee = listBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedeployee)) {
                LoadEmployeeDetails(selectedeployee);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            con = conn.getCon();
            con.Open();
            deleteEmployee(selectedeployee);
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EmployeeHistory history = new EmployeeHistory();
            Hide();
            history.ShowDialog();
            Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadAllOfficeData();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }
    }
}
