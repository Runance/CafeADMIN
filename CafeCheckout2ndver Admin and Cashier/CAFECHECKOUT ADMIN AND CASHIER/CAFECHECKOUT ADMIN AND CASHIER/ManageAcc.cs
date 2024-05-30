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
    public partial class ManageAcc : Form
    {
        private string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
        private string username;
        private byte[] selectedImage = null;

        public ManageAcc(string username)
        {
            InitializeComponent();
            this.username = username;

            // Ensure the event handler is attached
            IDtextbox.TextChanged += IDtextbox_TextChanged;
        }

        private void Add_Account_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(IDtextbox.Text) ||
                    string.IsNullOrWhiteSpace(LastnameTB.Text) ||
                    string.IsNullOrWhiteSpace(MiddleNameTB.Text) ||
                    string.IsNullOrWhiteSpace(FirstNameTB.Text) ||
                    string.IsNullOrWhiteSpace(ContactNumTB.Text) ||
                    string.IsNullOrWhiteSpace(LifeStatusTB.Text) ||
                    string.IsNullOrWhiteSpace(AgeTB.Text) ||
                    string.IsNullOrWhiteSpace(UsernameTB.Text) ||
                    string.IsNullOrWhiteSpace(PasswordTB.Text) ||
                    string.IsNullOrWhiteSpace(RoleTB.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Do you want to add this account?", "Confirm Add", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if the account already exists
                        string checkQuery = "SELECT COUNT(*) FROM Accounts WHERE ID = @ID";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@ID", int.Parse(IDtextbox.Text));

                        // Set the command timeout to 60 seconds
                        checkCommand.CommandTimeout = 60;

                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Account already exists.");
                            return;
                        }

                        // Add account to the database
                        string query = "INSERT INTO Accounts (ID, Image, Last_Name, Middle_Name, First_Name, Contact_Num, Life_Status, Age, Username, Password, Role, ACC_STAT) VALUES (@ID, @Image, @Last_Name, @Middle_Name, @First_Name, @Contact_Num, @Life_Status, @Age, @Username, @Password, @Role, @ACC_STAT)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ID", int.Parse(IDtextbox.Text));
                        command.Parameters.AddWithValue("@Last_Name", LastnameTB.Text);
                        command.Parameters.AddWithValue("@Middle_Name", MiddleNameTB.Text);
                        command.Parameters.AddWithValue("@First_Name", FirstNameTB.Text);
                        command.Parameters.AddWithValue("@Contact_Num", long.Parse(ContactNumTB.Text));
                        command.Parameters.AddWithValue("@Life_Status", LifeStatusTB.Text);
                        command.Parameters.AddWithValue("@Age", int.Parse(AgeTB.Text));
                        command.Parameters.AddWithValue("@Username", UsernameTB.Text);
                        command.Parameters.AddWithValue("@Password", PasswordTB.Text);
                        command.Parameters.AddWithValue("@Role", RoleTB.Text);
                        command.Parameters.AddWithValue("@ACC_STAT", "LOGGED OUT");
                        command.Parameters.AddWithValue("@Image", selectedImage ?? (object)DBNull.Value);

                        // Set the command timeout to 60 seconds
                        command.CommandTimeout = 60;

                        command.ExecuteNonQuery();
                        MessageBox.Show("Account added successfully.");
                        ClearFields();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please Complete All Provided Details");
            }
        }

        private void Update_Account_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(IDtextbox.Text))
                {
                    MessageBox.Show("Please enter an account ID.");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Do you want to update this account?", "Confirm Update", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Build the update query dynamically based on which fields are provided
                        string query = "UPDATE Accounts SET ";
                        var parameters = new List<SqlParameter>();

                        if (!string.IsNullOrWhiteSpace(LastnameTB.Text))
                        {
                            query += "Last_Name = @Last_Name, ";
                            parameters.Add(new SqlParameter("@Last_Name", LastnameTB.Text));
                        }
                        if (!string.IsNullOrWhiteSpace(MiddleNameTB.Text))
                        {
                            query += "Middle_Name = @Middle_Name, ";
                            parameters.Add(new SqlParameter("@Middle_Name", MiddleNameTB.Text));
                        }
                        if (!string.IsNullOrWhiteSpace(FirstNameTB.Text))
                        {
                            query += "First_Name = @First_Name, ";
                            parameters.Add(new SqlParameter("@First_Name", FirstNameTB.Text));
                        }
                        if (!string.IsNullOrWhiteSpace(ContactNumTB.Text))
                        {
                            query += "Contact_Num = @Contact_Num, ";
                            parameters.Add(new SqlParameter("@Contact_Num", long.Parse(ContactNumTB.Text)));
                        }
                        if (!string.IsNullOrWhiteSpace(LifeStatusTB.Text))
                        {
                            query += "Life_Status = @Life_Status, ";
                            parameters.Add(new SqlParameter("@Life_Status", LifeStatusTB.Text));
                        }
                        if (!string.IsNullOrWhiteSpace(AgeTB.Text))
                        {
                            query += "Age = @Age, ";
                            parameters.Add(new SqlParameter("@Age", int.Parse(AgeTB.Text)));
                        }
                        if (!string.IsNullOrWhiteSpace(UsernameTB.Text))
                        {
                            query += "Username = @Username, ";
                            parameters.Add(new SqlParameter("@Username", UsernameTB.Text));
                        }
                        if (!string.IsNullOrWhiteSpace(PasswordTB.Text))
                        {
                            query += "Password = @Password, ";
                            parameters.Add(new SqlParameter("@Password", PasswordTB.Text));
                        }
                        if (!string.IsNullOrWhiteSpace(RoleTB.Text))
                        {
                            query += "Role = @Role, ";
                            parameters.Add(new SqlParameter("@Role", RoleTB.Text));
                        }
                        if (selectedImage != null)
                        {
                            query += "Image = @Image, ";
                            parameters.Add(new SqlParameter("@Image", selectedImage));
                        }

                        query = query.TrimEnd(',', ' ') + " WHERE ID = @ID";
                        parameters.Add(new SqlParameter("@ID", int.Parse(IDtextbox.Text)));

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddRange(parameters.ToArray());

                        command.ExecuteNonQuery();
                        MessageBox.Show("Account updated successfully.");
                        ClearFields();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please Complete All Provided Details");
            }
        }

        private void Delete_Account_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(IDtextbox.Text))
                {
                    MessageBox.Show("Please enter an account ID.");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Do you want to delete this account?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "DELETE FROM Accounts WHERE ID = @ID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ID", int.Parse(IDtextbox.Text));

                        command.ExecuteNonQuery();
                        MessageBox.Show("Account deleted successfully.");
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the account: " + ex.Message);
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath = openFileDialog.FileName;
                    selectedImage = File.ReadAllBytes(imagePath);
                    PictureBox1.Image = Image.FromFile(imagePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while selecting the image: " + ex.Message);
            }
        }

        private void IDtextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(IDtextbox.Text, out int id))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "SELECT * FROM Accounts WHERE ID = @ID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ID", id);

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            LastnameTB.Text = reader["Last_Name"].ToString();
                            MiddleNameTB.Text = reader["Middle_Name"].ToString();
                            FirstNameTB.Text = reader["First_Name"].ToString();
                            ContactNumTB.Text = reader["Contact_Num"].ToString();
                            LifeStatusTB.Text = reader["Life_Status"].ToString();
                            AgeTB.Text = reader["Age"].ToString();
                            UsernameTB.Text = reader["Username"].ToString();
                            PasswordTB.Text = reader["Password"].ToString();
                            RoleTB.Text = reader["Role"].ToString();

                            if (reader["Image"] != DBNull.Value)
                            {
                                byte[] imageData = (byte[])reader["Image"];
                                using (MemoryStream ms = new MemoryStream(imageData))
                                {
                                    PictureBox1.Image = Image.FromStream(ms);
                                }
                            }
                            else
                            {
                                PictureBox1.Image = null;
                            }
                        }
                        else
                        {
                            ClearFields();
                        }
                    }
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching account details: " + ex.Message);
            }
        }

        private void ClearFields()
        {
           
            LastnameTB.Clear();
            MiddleNameTB.Clear();
            FirstNameTB.Clear();
            ContactNumTB.Clear();
            LifeStatusTB.Clear();
            AgeTB.Clear();
            UsernameTB.Clear();
            PasswordTB.Clear();
            RoleTB.Clear();
            PictureBox1.Image = null;
            selectedImage = null;
        }
        private void Back_but_Click(object sender, EventArgs e)
        {
            AccountForm accountForm = new AccountForm(username);
            accountForm.Refresh();
            accountForm.Show();
            this.Close();
        }
    }
}