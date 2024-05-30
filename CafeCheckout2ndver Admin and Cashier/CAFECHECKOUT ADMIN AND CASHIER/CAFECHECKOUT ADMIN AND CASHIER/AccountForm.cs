using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAFECHECKOUT_ADMIN_AND_CASHIER
{
    public partial class AccountForm : Form
    {
        private string username;
        private int currentIndex = 0;
        private List<Account> accounts;

        public AccountForm(string username)
        {
            InitializeComponent();
            this.username = username;
            InitializeFlowLayoutPanel();
            LoadAccounts();
            DisplayAccount(currentIndex);
        }

        private void InitializeFlowLayoutPanel()
        {
            AccountDetails.FlowDirection = FlowDirection.TopDown;
            AccountDetails.WrapContents = false;
            AccountDetails.AutoScroll = true;
            AccountDetails.Size = new Size(550, 365); // Set a fixed size
        }

        private void LoadAccounts()
        {
            accounts = GetAccountsFromDatabase();
        }

        private void DisplayAccount(int index)
        {
            if (index < 0 || index >= accounts.Count) return;

            var account = accounts[index];

            // Clear previous details
            AccountDetails.Controls.Clear();

            // Display image
            if (account.Image != null)
            {
                using (var ms = new MemoryStream(account.Image))
                {
                    AccountPicture.Image = Image.FromStream(ms);
                }
            }
            else
            {
                AccountPicture.Image = null;
            }

            // Display text details line by line with separators
            AddDetailLine("ID: " + account.ID);
            AddDetailLine("Last Name: " + account.LastName);
            AddDetailLine("Middle Name: " + account.MiddleName);
            AddDetailLine("First Name: " + account.FirstName);
            AddDetailLine("Contact Number: 0" + (account.ContactNumber.HasValue ? account.ContactNumber.Value.ToString() : "N/A"));
            AddDetailLine("Life Status: " + account.LifeStatus);
            AddDetailLine("Age: " + (account.Age.HasValue ? account.Age.Value.ToString() : "N/A"));
            AddDetailLine("Username: " + account.Username);
            AddDetailLine("Role: " + account.Role);
            AddDetailLine("Account Status: " + account.AccountStatus);
        }

        private void AddDetailLine(string text)
        {
            var label = new Label
            {
                Text = text,
                Font = new Font("Rockwell", 20, FontStyle.Italic),
                AutoSize = true
            };
            AccountDetails.Controls.Add(label);

            var separator = new Label
            {
                Text = "--------------------------------",
                Font = new Font("Rockwell", 20, FontStyle.Italic),
                AutoSize = true
            };
            AccountDetails.Controls.Add(separator);
        }

        private List<Account> GetAccountsFromDatabase()
        {
            var accounts = new List<Account>();
            using (var connection = new SqlConnection("Data Source=LAPTOP-R45B7D8N\\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True"))
            {
                var command = new SqlCommand("SELECT * FROM Accounts", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        accounts.Add(new Account
                        {
                            ID = reader.GetInt32(0),
                            Image = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null,
                            LastName = reader["Last_Name"] != DBNull.Value ? reader.GetString(2) : string.Empty,
                            MiddleName = reader["Middle_Name"] != DBNull.Value ? reader.GetString(3) : string.Empty,
                            FirstName = reader["First_Name"] != DBNull.Value ? reader.GetString(4) : string.Empty,
                            ContactNumber = reader["Contact_Num"] != DBNull.Value ? (long?)reader.GetInt64(5) : null,
                            LifeStatus = reader["Life_Status"] != DBNull.Value ? reader.GetString(6) : string.Empty,
                            Age = reader["Age"] != DBNull.Value ? (int?)reader.GetInt32(7) : null,
                            Username = reader["Username"] != DBNull.Value ? reader.GetString(8) : string.Empty,
                            Role = reader["Role"] != DBNull.Value ? reader.GetString(10) : string.Empty,
                            AccountStatus = reader["ACC_STAT"] != DBNull.Value ? reader.GetString(11) : string.Empty
                        });
                    }
                }
            }
            return accounts;
        }



        private void NextButt_Click(object sender, EventArgs e)
        {
            if (currentIndex < accounts.Count - 1)
            {
                currentIndex++;
                DisplayAccount(currentIndex);
            }
        }

        private void PreviousButt_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayAccount(currentIndex);
            }
        }

        private void Back_but_Click(object sender, EventArgs e)
        {
            AdminForm admin = new AdminForm(username);
            admin.Refresh();
            admin.Show();
            this.Close();
        }

        private void ManageAccButt_Click(object sender, EventArgs e)
        {
            ManageAcc manageAcc = new ManageAcc(username);
            manageAcc.Refresh();
            manageAcc.Show();
            this.Close();
        }

        private void AccountForm_Load(object sender, EventArgs e)
        {

        }
    }

    public class Account
    {
        public int ID { get; set; }
        public byte[] Image { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public long? ContactNumber { get; set; }
        public string LifeStatus { get; set; }
        public int? Age { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string AccountStatus { get; set; }
    }
}