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
    public partial class AdminForm : Form
    {
       
        private string username; // Store the username
        private bool ButtonClicked = false;

        public AdminForm(string username)
        {
            InitializeComponent();
            this.username = username; // Set the username

            // Set the ACC_STAT to "LOGGED IN" when the form is created
            UpdateAccountStatus("LOGGED IN");
            this.FormClosing += AdminForm_FormClosing;
        }
        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ButtonClicked) // Check if back button was not clicked
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
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
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
        private void UpdateAccountStatus(string status)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string query = "UPDATE Accounts SET ACC_STAT = @status WHERE Username = @username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
            }
        }

        private void Signout_But_Click(object sender, EventArgs e)
        {
            ButtonClicked = true;
            // Display confirmation dialog
            DialogResult result = MessageBox.Show("Do you want to logout?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Check user's choice
            if (result == DialogResult.Yes)
            {
                // Set the ACC_STAT to "LOGGED OUT"
                UpdateAccountStatus("LOGGED OUT");

                Login_Interface assignedForm = new Login_Interface();
                assignedForm.Show();

                // Close the current form
                this.Close();
            }
        }

        private void Account_Butt_Click(object sender, EventArgs e)
        {
            ButtonClicked = true;
            AccountForm accountForm = new AccountForm(username);
            accountForm.Refresh();
            accountForm.Show();
            this.Close();
        }

        private void HIstoryButton_Click(object sender, EventArgs e)
        {
            ButtonClicked = true;
            History history = new History(username);
            history.Refresh();
            history.Show();
            this.Close();
        }

        private void cashier_Click(object sender, EventArgs e)
        {
            ButtonClicked = true;
            CashierForm cashier = new CashierForm(username);
            cashier.Refresh();
            cashier.Show();
            this.Close();
        }

        private void Statistics_Click(object sender, EventArgs e)
        {
            ButtonClicked = true;
            Statistics statistics = new Statistics(username);
            statistics.Refresh();
            statistics.Show();
            this.Close();
        }

        private void Inventory_Click(object sender, EventArgs e)
        {
            ButtonClicked = true;
            Inventory inventory = new Inventory(username);
            inventory.Refresh();
            inventory.Show();
            this.Close();
        }

        private void Orders_Butt_Click(object sender, EventArgs e)
        {
            ButtonClicked = true;
            Orders orders = new Orders(username);
            orders.Refresh();
            orders.Show();
            this.Close();
        }
    }
}