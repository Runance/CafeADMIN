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
    public partial class Inventory : Form
    {
        private string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
        private string username;
        private DataTable inventoryTable;

        public Inventory(string username)
        {
            InitializeComponent();
            LoadInventoryData();
            this.username = username;
            SearchBox.TextChanged += SearchBox_TextChanged;
            InventoryGrid.SelectionChanged += InventoryGrid_SelectionChanged;

            // Ensure the PictureBox SizeMode is set to StretchImage
            InvenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void LoadInventoryData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Product_Id AS ID, Product_Name AS Name, Description, Price, Stock, IMAGE, 'Product' AS Type FROM Products " +
                                   "UNION " +
                                   "SELECT Addon_Id AS ID, Addon_Name AS Name, Description, Price, Stock, ADO_IMAGE AS IMAGE, 'Addon' AS Type FROM Add_Ons";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    inventoryTable = new DataTable();
                    adapter.Fill(inventoryTable);

                    InventoryGrid.DataSource = inventoryTable;

                    // Hide the IMAGE column
                    InventoryGrid.Columns["IMAGE"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            string filter = $"ID LIKE '%{SearchBox.Text}%' OR Name LIKE '%{SearchBox.Text}%'";
            (InventoryGrid.DataSource as DataTable).DefaultView.RowFilter = filter;

            if (InventoryGrid.Rows.Count > 0)
            {
                // Select the first matching row
                InventoryGrid.Rows[0].Selected = true;
                DisplayDetails(InventoryGrid.Rows[0]);
            }
            else
            {
                // No matching results
                InvenPictureBox.Visible = false;
                InvenflowLayoutPanel.Visible = false;
            }
        }

        private void InventoryGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (InventoryGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = InventoryGrid.SelectedRows[0];
                DisplayDetails(selectedRow);
            }
        }

        private void DisplayDetails(DataGridViewRow row)
        {
            string id = row.Cells["ID"].Value?.ToString() ?? "N/A";
            string name = row.Cells["Name"].Value?.ToString() ?? "N/A";
            string description = row.Cells["Description"].Value?.ToString() ?? "N/A";
            string price = row.Cells["Price"].Value?.ToString() ?? "N/A";
            string stock = row.Cells["Stock"].Value?.ToString() ?? "N/A";
            string type = row.Cells["Type"].Value?.ToString() ?? "N/A";
            byte[] imageData = row.Cells["IMAGE"].Value as byte[];

            InvenPictureBox.Visible = true;
            InvenflowLayoutPanel.Visible = true;

            if (imageData != null && imageData.Length > 0)
            {
                using (var ms = new System.IO.MemoryStream(imageData))
                {
                    InvenPictureBox.Image = Image.FromStream(ms);
                }
            }
            else
            {
                InvenPictureBox.Image = null;
            }

            Font detailsFont = new Font("Rockwell", 15, FontStyle.Italic);
            InvenflowLayoutPanel.Controls.Clear();
            InvenflowLayoutPanel.Controls.Add(new Label { Text = $"ID: {id}", AutoSize = true, Font = detailsFont });
            InvenflowLayoutPanel.Controls.Add(new Label { Text = $"Name: {name}", AutoSize = true, Font = detailsFont });
            InvenflowLayoutPanel.Controls.Add(new Label { Text = $"Description: {description}", AutoSize = true, Font = detailsFont });
            InvenflowLayoutPanel.Controls.Add(new Label { Text = $"Price: {price}", AutoSize = true, Font = detailsFont });
            InvenflowLayoutPanel.Controls.Add(new Label { Text = $"Stock: {stock}", AutoSize = true, Font = detailsFont });
            InvenflowLayoutPanel.Controls.Add(new Label { Text = $"Type: {type}", AutoSize = true, Font = detailsFont });
        }

        private void Back_but_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm(username);
            adminForm.Refresh();
            adminForm.Show();
            this.Close();
           
           
        }

        private void AddProducts_Click(object sender, EventArgs e)
        {
            Addproducts addProducts = new Addproducts(username);
            addProducts.Refresh();
            addProducts.Show();
            this.Close();
        }

        private void Addaddons_Click(object sender, EventArgs e)
        {
            Addaddons addaddons = new Addaddons(username);
            addaddons.Refresh();
            addaddons.Show();
            this.Close();
           
        }
    }
}