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
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }

        Connection conn = new Connection();
        SqlCommand cmd;
        SqlConnection con;
        DataTable dt;
        SqlDataAdapter ad;
        SqlDataReader rd;
        private List<Personnel> Personnels = new List<Personnel>();


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                con = conn.getCon();
                con.Open();

                int employeeId = Personnels[listBox1.SelectedIndex].id;
                DateTime currentDate = DateTime.Now.Date;

                // Check the Time for today
                SqlCommand checkCommand = new SqlCommand("SELECT timein FROM OfficeData WHERE id = @id AND date = @currentDate", con);
                checkCommand.Parameters.AddWithValue("@id", employeeId);
                checkCommand.Parameters.AddWithValue("@currentDate", currentDate);

                object timeInValue = checkCommand.ExecuteScalar();
                object timeoutValue = checkCommand.ExecuteScalar();


                if (timeInValue == null)
                {
                    MessageBox.Show("You must Time In before you can Time Out.");
                    return; // Exit if time did not exist
                }


                if (timeoutValue != null) // if time exist
                {
                    SqlCommand command = new SqlCommand("UPDATE OfficeData SET timeout = @timeout WHERE id = @id", con);
                    command.Parameters.AddWithValue("@timeout", DateTime.Now.TimeOfDay);
                    command.Parameters.AddWithValue("@id", employeeId);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Time Out recorded successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to record Time Out.");
                    }
                }
                else
                {
                    MessageBox.Show("Time out already recorded for the day.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con = conn.getCon();
                con.Open();

                int employeeId = Personnels[listBox1.SelectedIndex].id;
                DateTime currentDate = DateTime.Now.Date;

                // Check if Time In already exists for today
                SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM OfficeData WHERE id = @id AND date = @currentDate", con);
                checkCommand.Parameters.AddWithValue("@id", employeeId);
                checkCommand.Parameters.AddWithValue("@currentDate", currentDate);

                int count = (int)checkCommand.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Time In has already been recorded for today.");
                    return; // Exit  if Time In already exists
                }

                SqlCommand command = new SqlCommand("UPDATE OfficeData SET timein = @timeIn, date = @dateIn WHERE id = @id", con);
                command.Parameters.AddWithValue("@timeIn", DateTime.Now.TimeOfDay);
                command.Parameters.AddWithValue("@dateIn", currentDate);
                command.Parameters.AddWithValue("@id", employeeId);

                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Time In recorded successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to record Time In.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            textBox2.Text = Personnels[listBox1.SelectedIndex].name;
            textBox3.Text = Personnels[listBox1.SelectedIndex].number.ToString();
            textBox4.Text = Personnels[listBox1.SelectedIndex].postion;
            textBox5.Text = Personnels[listBox1.SelectedIndex].id.ToString();

        }

        private void updateList(String search = "")  //update the list box also
        {
            con = conn.getCon();
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.Parameters.AddWithValue("@search", search + "%");
            command.CommandText = "SELECT * FROM OfficeData WHERE name LIKE @search or id LIKE @search";


            SqlDataReader reader = command.ExecuteReader();
            Personnels.Clear();
            listBox1.Items.Clear();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string number = reader.GetString(2);
                string postion = reader.GetString(3);
                Personnels.Add(new Personnel(id, name, postion, number));
                listBox1.Items.Add(name);
            }
            con.Close();
        }

        private void Employee_Load(object sender, EventArgs e)
        {
            updateList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // connection for data base
                con = conn.getCon();
                con.Open();

                int employeeId = Personnels[listBox1.SelectedIndex].id;
                DateTime currentDate = DateTime.Now.Date;

                // Fetch employee details for today
                SqlCommand command = new SqlCommand("SELECT name, postion, number, timein, timeout FROM OfficeData WHERE id = @id AND date = @currentDate", con);
                command.Parameters.AddWithValue("@id", employeeId);
                command.Parameters.AddWithValue("@currentDate", currentDate);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string name = reader.GetString(0);
                    string position = reader.GetString(1);
                    string number = reader.GetString(2);
                    TimeSpan timeIn = reader.GetTimeSpan(3);
                    TimeSpan? timeOut = reader.IsDBNull(4) ? (TimeSpan?)null : reader.GetTimeSpan(4);
                    reader.Close();

                    if (timeOut.HasValue)
                    {
                        // Calculate total hours
                        double totalHours = (timeOut.Value - timeIn).TotalHours;

                        // Save to another table in the data base which is (EmployeeWorkHours)
                        SqlCommand saveCommand = new SqlCommand(
                            "INSERT INTO EmployeeWorkHours (id, name, postion, number, timeinhistory, timeouthistory, totalhourshistory, datehistory) " +
                            "VALUES (@id, @name, @postion, @number, @timein, @timeout, @totalhours, @datetoday)", con);

                        saveCommand.Parameters.AddWithValue("@id", employeeId);
                        saveCommand.Parameters.AddWithValue("@name", name);
                        saveCommand.Parameters.AddWithValue("@postion", position);
                        saveCommand.Parameters.AddWithValue("@number", number);
                        saveCommand.Parameters.AddWithValue("@timein", timeIn);
                        saveCommand.Parameters.AddWithValue("@timeout", timeOut);
                        saveCommand.Parameters.AddWithValue("@totalhours", totalHours);
                        saveCommand.Parameters.AddWithValue("@datetoday", currentDate);

                        saveCommand.ExecuteNonQuery();


                        SqlCommand nullifyCommand = new SqlCommand(
                        "UPDATE OfficeData SET timein = NULL, timeout = NULL, date = NULL WHERE id = @id", con);
                        nullifyCommand.Parameters.AddWithValue("@id", employeeId);
                        nullifyCommand.ExecuteNonQuery();

                        MessageBox.Show("Data successfully saved to history, and current fields cleared.");
                    }
                    else
                    {
                        MessageBox.Show("Employee has not timed out yet.");
                    }
                }
                else
                {
                    MessageBox.Show("No data found for this employee today.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateList(textBox1.Text);

        }
    }
}
