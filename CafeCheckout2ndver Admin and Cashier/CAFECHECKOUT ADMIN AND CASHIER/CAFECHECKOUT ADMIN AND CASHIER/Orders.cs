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
    public partial class Orders : Form
    {
        private string username;
        private string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
        public Orders(string username)
        {
           
            this.username = username;
            InitializeComponent();
            LoadQueuedCustomersAndOrders();
        }

        private void LoadQueuedCustomersAndOrders()
        {
            string query = @"
                SELECT 
                    T.Customer_Id,
                    T.Product_Id,
                    P.Product_Name,
                    T.Quantity,
                    T.Addon_Id,
                    A.Addon_Name,
                    T.Addon_Quantity
                FROM 
                    Transactions T
                LEFT JOIN 
                    Products P ON T.Product_Id = P.Product_Id
                LEFT JOIN 
                    Add_Ons A ON T.Addon_Id = A.Addon_Id
                WHERE 
                    T.Status = 'Queued'
                ORDER BY 
                    T.Transaction_Id";

            DataTable dataTable = ExecuteQuery(query);

            if (dataTable.Rows.Count == 0)
            {
                MessageBox.Show("No queued customers found.");
                return;
            }

            flowLayoutPanel1.Controls.Clear();

            foreach (DataRow row in dataTable.Rows)
            {
                string customerId = row["Customer_Id"].ToString();
                string productName = row["Product_Name"].ToString();
                int quantity = Convert.ToInt32(row["Quantity"]);
                string addonName = row["Addon_Name"] != DBNull.Value ? row["Addon_Name"].ToString() : string.Empty;
                int addonQuantity = row["Addon_Quantity"] != DBNull.Value ? Convert.ToInt32(row["Addon_Quantity"]) : 0;

                string orderDetails = $"Customer ID: {customerId}\nProduct: {productName} (Qty: {quantity})";

                if (!string.IsNullOrEmpty(addonName))
                {
                    orderDetails += $"\nAddon: {addonName} (Qty: {addonQuantity})";
                }

                Label orderLabel = new Label
                {
                    Text = orderDetails,
                    AutoSize = true,
                    Font = new Font("Rockwell", 25, FontStyle.Italic),
                    Margin = new Padding(10)
                };

                flowLayoutPanel1.Controls.Add(orderLabel);
            }

            flowLayoutPanel1.AutoScroll = true;
        }

        private DataTable ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        private void Back_but_Click(object sender, EventArgs e)
        {
            AdminForm admin = new AdminForm(username);
            admin.Refresh();
            admin.Show();
            this.Close();
        }
    }
}
