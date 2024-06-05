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
    public partial class History : Form
    {
        private string username;
        private string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
        private bool isBackButtonClicked = false;

        // Declare a DataTable to store the data
        private DataTable dataTable;

        public History(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadRole();

            // Initialize the DataTable and bind it to the DataGridView
            dataTable = new DataTable();
            dataGridView1.DataSource = dataTable;

            // Attach the TextChanged event to the TextBox
            guna2TextBox1.TextChanged += new EventHandler(guna2TextBox1_TextChanged);

            // Load data initially
            LoadData();

            this.FormClosing += History_FormClosing;
        }
        private void History_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isBackButtonClicked) // Check if back button was not clicked
            {
                DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    UpdateAccountStatusToLoggedOut(username);
                }
                else
                {
                    e.Cancel = true; // Prevent the form from closing
                }
            }
        }

        private void UpdateAccountStatusToLoggedOut(string username)
        {
            string query = "UPDATE Accounts SET ACC_STAT = 'LOGGED OUT' WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void LoadRole()
        {
            string query = "SELECT Role FROM Accounts WHERE Username = @username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                       
                    }
                }
            }
        }

        // Load data into the DataTable
        private void LoadData()
        {
            string query = "SELECT * FROM Trsn_Complete";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dataTable);
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox1.Text.Trim();
            dataTable.Clear(); // Clear existing data

            if (!string.IsNullOrEmpty(searchText))
            {
                string query = "SELECT * FROM Trsn_Complete WHERE Transaction_Id LIKE @searchText OR Customer_Id LIKE @searchText";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                    adapter.Fill(dataTable); // Fill with new data
                }
            }
            else
            {
                LoadData(); // If search text is empty, load all data
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string searchText = guna2TextBox1.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                string query = "DELETE FROM Trsn_Complete WHERE Transaction_Id = @searchText OR Customer_Id = @searchText";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchText", searchText);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record(s) deleted successfully.");
                            guna2TextBox1_TextChanged(sender, e); // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No matching records found.");
                        }
                    }
                }
            }
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Trsn_Complete";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("All records deleted successfully.");
                    dataTable.Clear(); // Clear the DataTable
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            isBackButtonClicked = true;
            AdminForm adminForm = new AdminForm(username);
            adminForm.Refresh();
            adminForm.Show();
            this.Close();
        }
    }
}