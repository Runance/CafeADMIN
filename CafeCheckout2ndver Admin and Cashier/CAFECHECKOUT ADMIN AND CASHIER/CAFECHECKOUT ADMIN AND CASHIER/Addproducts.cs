using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO; // Make sure this namespace is included
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAFECHECKOUT_ADMIN_AND_CASHIER
{
        public partial class Addproducts : Form
    {
        private string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
        private string username;
        private byte[] selectedImage = null;

        public Addproducts(string username)
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

                DialogResult dialogResult = MessageBox.Show("Do you want to add this product?", "Confirm Add", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if the product already exists
                        string checkQuery = "SELECT COUNT(*) FROM Products WHERE Product_Id = @ProductId";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@ProductId", IDtextBox1.Text);

                        // Set the command timeout to 60 seconds
                        checkCommand.CommandTimeout = 60;

                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Product already exists.");
                            return;
                        }

                        // Add product to the database
                        string query = "INSERT INTO Products (Product_Id, Product_Name, Description, Price, Stock, IMAGE) VALUES (@ProductId, @ProductName, @Description, @Price, @Stock, @Image)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ProductId", IDtextBox1.Text);
                        command.Parameters.AddWithValue("@ProductName", NametextBox2.Text);
                        command.Parameters.AddWithValue("@Description", DescriptiontextBox3.Text);
                        command.Parameters.AddWithValue("@Price", int.Parse(PricetextBox4.Text));
                        command.Parameters.AddWithValue("@Stock", int.Parse(StockstextBox5.Text));
                        command.Parameters.AddWithValue("@Image", selectedImage ?? (object)DBNull.Value);

                        // Set the command timeout to 60 seconds
                        command.CommandTimeout = 60;

                        command.ExecuteNonQuery();
                        MessageBox.Show("Product added successfully.");
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
                    MessageBox.Show("Please enter a product ID.");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Do you want to update this product?", "Confirm Update", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Build the update query dynamically based on which fields are provided
                        string query = "UPDATE Products SET ";
                        var parameters = new List<SqlParameter>();

                        if (!string.IsNullOrWhiteSpace(NametextBox2.Text))
                        {
                            query += "Product_Name = @ProductName, ";
                            parameters.Add(new SqlParameter("@ProductName", NametextBox2.Text));
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
                            query += "IMAGE = @Image, ";
                            parameters.Add(new SqlParameter("@Image", selectedImage));
                        }

                        query = query.TrimEnd(',', ' ') + " WHERE Product_Id = @ProductId";
                        parameters.Add(new SqlParameter("@ProductId", IDtextBox1.Text));

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddRange(parameters.ToArray());

                        command.ExecuteNonQuery();
                        MessageBox.Show("Product updated successfully.");
                        ClearFields();
                        
                    }
                }
            }
            catch 
            {
                MessageBox.Show("Please Complete All Provided Details");
            }
        }

        private void DeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(IDtextBox1.Text))
                {
                    MessageBox.Show("Please enter a product ID.");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Do you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "DELETE FROM Products WHERE Product_Id = @ProductId";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ProductId", IDtextBox1.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Product deleted successfully.");
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the product: " + ex.Message);
            }
        }

        private void ClearProducts_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete all products?", "Confirm Delete All", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "DELETE FROM Products";
                        SqlCommand command = new SqlCommand(query, connection);

                        command.ExecuteNonQuery();
                        MessageBox.Show("All products cleared.");
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while clearing all products: " + ex.Message);
            }
        }

        private void ProductBox1_Click(object sender, EventArgs e)
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
                    ProductBox1.Image = Image.FromFile(imagePath);
                    ProductBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while selecting the image: " + ex.Message);
            }
        }

        private void IDtextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(IDtextBox1.Text))
                    return;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Product_Name, Description, Price, Stock, IMAGE FROM Products WHERE Product_Id = @ProductId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductId", IDtextBox1.Text);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NametextBox2.TextChanged -= TextBox_TextChanged;
                            DescriptiontextBox3.TextChanged -= TextBox_TextChanged;
                            PricetextBox4.TextChanged -= TextBox_TextChanged;
                            StockstextBox5.TextChanged -= TextBox_TextChanged;

                            NametextBox2.Text = reader["Product_Name"].ToString();
                            DescriptiontextBox3.Text = reader["Description"].ToString();
                            PricetextBox4.Text = reader["Price"].ToString();
                            StockstextBox5.Text = reader["Stock"].ToString();

                            if (reader["IMAGE"] != DBNull.Value)
                            {
                                byte[] imageBytes = (byte[])reader["IMAGE"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    ProductBox1.Image = Image.FromStream(ms);
                                    ProductBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Ensure the image is stretched
                                }
                                selectedImage = imageBytes;
                            }
                            else
                            {
                                ProductBox1.Image = null;
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
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading product data: " + ex.Message);
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
            ProductBox1.Image = null;
            selectedImage = null;
        }

        private void Back_but_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory(username);
            inventory.Show();
            this.Close();
        }
    }
}