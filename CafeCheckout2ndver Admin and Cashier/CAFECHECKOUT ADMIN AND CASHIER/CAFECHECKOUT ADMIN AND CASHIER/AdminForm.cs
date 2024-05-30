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

        public AdminForm(string username)
        {
            InitializeComponent();
            this.username = username; // Set the username

            // Set the ACC_STAT to "LOGGED IN" when the form is created
            UpdateAccountStatus("LOGGED IN");
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
            AccountForm accountForm = new AccountForm(username);
            accountForm.Refresh();
            accountForm.Show();
            this.Close();
        }

        private void HIstoryButton_Click(object sender, EventArgs e)
        {
            History history = new History(username);
            history.Refresh();
            history.Show();
            this.Close();
        }

        private void cashier_Click(object sender, EventArgs e)
        {
            CashierForm cashier = new CashierForm(username);
            cashier.Refresh();
            cashier.Show();
            this.Close();
        }

        private void Statistics_Click(object sender, EventArgs e)
        {
            Statistics statistics = new Statistics(username);
            statistics.Refresh();
            statistics.Show();
            this.Close();
        }

        private void Inventory_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory(username);
            inventory.Refresh();
            inventory.Show();
            this.Close();
        }

        private void Orders_Butt_Click(object sender, EventArgs e)
        {
            Orders orders = new Orders(username);
            orders.Refresh();
            orders.Show();
            this.Close();
        }
    }
}