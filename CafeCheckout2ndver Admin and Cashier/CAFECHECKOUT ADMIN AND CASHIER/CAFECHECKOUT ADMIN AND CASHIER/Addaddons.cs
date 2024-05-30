using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
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
        public partial class Addaddons : Form
        {
            private string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            private string username;
            private byte[] selectedImage = null;

            public Addaddons(string username)
            {
                InitializeComponent();
                this.username = username;

                // Ensure the event handler is attached
                IDtextBox1.TextChanged += IDtextBox1_TextChanged;
            }

        private void AddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(IDtextBox1.Text) ||
                    string.IsNullOrWhiteSpace(NametextBox2.Text) ||
                    string.IsNullOrWhiteSpace(DescriptiontextBox3.Text) ||
                    string.IsNullOrWhiteSpace(PricetextBox4.Text) ||
                    string.IsNullOrWhiteSpace(StockstextBox5.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Do you want to add this addon?", "Confirm Add", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if the addon already exists
                        string checkQuery = "SELECT COUNT(*) FROM Add_Ons WHERE Addon_Id = @AddonId";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@AddonId", IDtextBox1.Text);

                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Addon already exists.");
                            return;
                        }

                        // Add addon to the database
                        string query = "INSERT INTO Add_Ons (Addon_Id, Addon_Name, Description, Price, Stock, ADO_IMAGE) VALUES (@AddonId, @AddonName, @Description, @Price, @Stock, @Image)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@AddonId", IDtextBox1.Text);
                        command.Parameters.AddWithValue("@AddonName", NametextBox2.Text);
                        command.Parameters.AddWithValue("@Description", DescriptiontextBox3.Text);
                        command.Parameters.AddWithValue("@Price", int.Parse(PricetextBox4.Text));
                        command.Parameters.AddWithValue("@Stock", int.Parse(StockstextBox5.Text));
                        command.Parameters.AddWithValue("@Image", selectedImage ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Addon added successfully.");
                        ClearFields();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please Complete All Provided Details");
            }
        }

            private void UpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(IDtextBox1.Text))
                {
                    MessageBox.Show("Please enter an addon ID.");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Do you want to update this addon?", "Confirm Update", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Build the update query dynamically based on which fields are provided
                        string query = "UPDATE Add_Ons SET ";
                        var parameters = new List<SqlParameter>();

                        if (!string.IsNullOrWhiteSpace(NametextBox2.Text))
                        {
                            query += "Addon_Name = @AddonName, ";
                            parameters.Add(new SqlParameter("@AddonName", NametextBox2.Text));
                        }
                        if (!string.IsNullOrWhiteSpace(DescriptiontextBox3.Text))
                        {
                            query += "Description = @Description, ";
                            parameters.Add(new SqlParameter("@Description", DescriptiontextBox3.Text));
                        }
                        if (!string.IsNullOrWhiteSpace(PricetextBox4.Text))
                        {
                            query += "Price = @Price, ";
                            parameters.Add(new SqlParameter("@Price", int.Parse(PricetextBox4.Text)));
                        }
                        if (!string.IsNullOrWhiteSpace(StockstextBox5.Text))
                        {
                            query += "Stock = @Stock, ";
                            parameters.Add(new SqlParameter("@Stock", int.Parse(StockstextBox5.Text)));
                        }
                        if (selectedImage != null)
                        {
                            query += "ADO_IMAGE = @Image, ";
                            parameters.Add(new SqlParameter("@Image", selectedImage));
                        }

                        query = query.TrimEnd(',', ' ') + " WHERE Addon_Id = @AddonId";
                        parameters.Add(new SqlParameter("@AddonId", IDtextBox1.Text));

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddRange(parameters.ToArray());

                        command.ExecuteNonQuery();
                        MessageBox.Show("Addon updated successfully.");
                        ClearFields();
                    }
                }
            } catch 
            {
                MessageBox.Show("Please Complete All Provided Details");
            }

        }

        private void DeleteProduct_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(IDtextBox1.Text))
                {
                    MessageBox.Show("Please enter an addon ID.");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Do you want to delete this addon?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "DELETE FROM Add_Ons WHERE Addon_Id = @AddonId";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@AddonId", IDtextBox1.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Addon deleted successfully.");
                        ClearFields();
                    }
                }
            }

            private void ClearProducts_Click(object sender, EventArgs e)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete all addons?", "Confirm Delete All", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "DELETE FROM Add_Ons";
                        SqlCommand command = new SqlCommand(query, connection);

                        command.ExecuteNonQuery();
                        MessageBox.Show("All addons cleared.");
                        ClearFields();
                    }
                }
            }

            private void pictureBox1_Click(object sender, EventArgs e)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath = openFileDialog.FileName;
                    selectedImage = File.ReadAllBytes(imagePath);
                    AddonBox1.Image = Image.FromFile(imagePath);
                    AddonBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }

            private void IDtextBox1_TextChanged(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(IDtextBox1.Text))
                    return;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Addon_Name, Description, Price, Stock, ADO_IMAGE FROM Add_Ons WHERE Addon_Id = @AddonId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AddonId", IDtextBox1.Text);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NametextBox2.TextChanged -= TextBox_TextChanged;
                            DescriptiontextBox3.TextChanged -= TextBox_TextChanged;
                            PricetextBox4.TextChanged -= TextBox_TextChanged;
                            StockstextBox5.TextChanged -= TextBox_TextChanged;

                            NametextBox2.Text = reader["Addon_Name"].ToString();
                            DescriptiontextBox3.Text = reader["Description"].ToString();
                            PricetextBox4.Text = reader["Price"].ToString();
                            StockstextBox5.Text = reader["Stock"].ToString();

                            if (reader["ADO_IMAGE"] != DBNull.Value)
                            {
                                byte[] imageBytes = (byte[])reader["ADO_IMAGE"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    AddonBox1.Image = Image.FromStream(ms);
                                    AddonBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Ensure the image is stretched
                                }
                                selectedImage = imageBytes;
                            }
                            else
                            {
                                AddonBox1.Image = null;
                                selectedImage = null;
                            }

                            NametextBox2.TextChanged += TextBox_TextChanged;
                            DescriptiontextBox3.TextChanged += TextBox_TextChanged;
                            PricetextBox4.TextChanged += TextBox_TextChanged;
                            StockstextBox5.TextChanged += TextBox_TextChanged;
                        }
                        else
                        {
                            ClearFields();
                        }
                    }
                }
            }

            private void TextBox_TextChanged(object sender, EventArgs e)
            {
                // This method is here to potentially handle changes if needed.
            }

            private void ClearFields()
            {
          
                NametextBox2.Clear();
                DescriptiontextBox3.Clear();
                PricetextBox4.Clear();
                StockstextBox5.Clear();
                AddonBox1.Image = null;
                selectedImage = null;
            }

            private void Back_but_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory(username);
            inventory.Refresh();
            inventory.Show(); 
            this.Close();
           

        }
    }
}