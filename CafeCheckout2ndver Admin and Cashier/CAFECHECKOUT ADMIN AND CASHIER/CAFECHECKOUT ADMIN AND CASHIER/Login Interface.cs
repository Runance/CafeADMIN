using Guna.UI2.WinForms;
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


namespace CAFECHECKOUT_ADMIN_AND_CASHIER
{
    public partial class Login_Interface : Form
    {
        public Login_Interface()
        {
            InitializeComponent();
            InitializeTextBoxes();
        }

        private void InitializeTextBoxes()
        {
            TextBox1.Text = "Enter Username";
            TextBox1.ForeColor = Color.Gray;

            TextBox2.Text = "Enter Password";
            TextBox2.ForeColor = Color.Gray;

            TextBox1.Enter += Guna2TextBox_Enter;
            TextBox1.Leave += Guna2TextBox_Leave;

            TextBox2.Enter += Guna2TextBox_Enter;
            TextBox2.Leave += Guna2TextBox_Leave;
        }

        private void Guna2TextBox_Enter(object sender, EventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "Enter Username" || textBox.Text == "Enter Password")
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            }
        }

        private void Guna2TextBox_Leave(object sender, EventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;
            if (textBox != null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox == TextBox1)
                    {
                        textBox.Text = "Enter Username";
                        textBox.ForeColor = Color.Gray;
                    }
                    else if (textBox == TextBox2)
                    {
                        textBox.Text = "Enter Password";
                        textBox.ForeColor = Color.Gray;
                    }
                }
            }
        }

        private void Login_butt_Click(object sender, EventArgs e)
        {
            string username = TextBox1.Text;
            string password = TextBox2.Text;

            // Connection string to your database
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL query to check if the account is already logged on
                string checkStatusQuery = "SELECT ACC_STAT FROM Accounts WHERE Username = @username";

                using (SqlCommand checkStatusCommand = new SqlCommand(checkStatusQuery, connection))
                {
                    checkStatusCommand.Parameters.AddWithValue("@username", username);

                    var result = checkStatusCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string accountStatus = (string)result;

                        if (accountStatus == "LOGGED IN")
                        {
                            MessageBox.Show("This account is already logged on.");
                            return; // Exit the method to prevent further processing
                        }
                    }
                }

                // SQL query to check username and password
                string query = "SELECT Role FROM Accounts WHERE Username = @username AND Password = @password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string role = reader["Role"].ToString();
                        reader.Close(); // Close the reader before executing the next command

                        // Update the ACC_STAT column to "log on"
                        string updateQuery = "UPDATE Accounts SET ACC_STAT = 'LOGGED IN' WHERE Username = @username";

                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@username", username);
                            updateCommand.ExecuteNonQuery();
                        }

                        // Open the corresponding form based on the role
                        if (role == "CASHIER")
                        {
                            CashierForm cashierForm = new CashierForm(username);
                            cashierForm.Refresh();
                            cashierForm.Show();
                        }
                        else if (role == "ADMIN")
                        {
                            AdminForm adminForm = new AdminForm(username);
                            adminForm.Refresh();
                            adminForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid role detected.");
                        }

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                    }
                }
            }
        }
    }
}