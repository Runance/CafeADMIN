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
        private bool isBackButtonClicked = false; 

        public Orders(string username)
        {
            this.username = username;
            InitializeComponent();
            LoadQueuedCustomersAndOrders();
            this.FormClosing += Orders_FormClosing;
        }
        private void Orders_FormClosing(object sender, FormClosingEventArgs e)
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

        private void LoadQueuedCustomersAndOrders()
        {
            string query = @"
    SELECT 
        T.Customer_Id,
        P.Product_Name,
        T.Quantity,
        A.Addon_Name,
        T.Addon_Quantity
    FROM 
        Transactions T
    LEFT JOIN 
        Products P ON T.Product_Id = P.Product_Id
    LEFT JOIN 
        Add_Ons A ON T.Addon_Id = A.Addon_Id
    INNER JOIN 
        Customer C ON T.Customer_Id = C.Customer_Id
    WHERE 
        T.Status = 'Queued'
        AND C.Entry_type != 'Cancelled'
    ORDER BY 
        T.Customer_Id ASC, T.Transaction_Id ASC";

            DataTable dataTable = ExecuteQuery(query);

            if (dataTable.Rows.Count == 0)
            {
                MessageBox.Show("No queued customers found.");
                return;
            }

            flowLayoutPanel1.Controls.Clear();
            var customerOrders = new Dictionary<string, List<string>>();

            foreach (DataRow row in dataTable.Rows)
            {
                string customerId = row["Customer_Id"].ToString();
                string productName = row["Product_Name"].ToString();
                int quantity = Convert.ToInt32(row["Quantity"]);
                string addonName = row["Addon_Name"] != DBNull.Value ? row["Addon_Name"].ToString() : string.Empty;
                int addonQuantity = row["Addon_Quantity"] != DBNull.Value ? Convert.ToInt32(row["Addon_Quantity"]) : 0;

                string orderDetails = $"- Product: {productName} (Qty: {quantity})";

                if (!string.IsNullOrEmpty(addonName))
                {
                    orderDetails += $", Addon: {addonName} (Qty: {addonQuantity})";
                }

                if (!customerOrders.ContainsKey(customerId))
                {
                    customerOrders[customerId] = new List<string>();
                }

                customerOrders[customerId].Add(orderDetails);
            }

            var sortedCustomerOrders = customerOrders.OrderBy(c => c.Key);

            foreach (var customerOrder in sortedCustomerOrders)
            {
                string customerId = customerOrder.Key;
                var orders = customerOrder.Value;

                StringBuilder orderDetailsBuilder = new StringBuilder();
                orderDetailsBuilder.AppendLine($"Customer ID: {customerId}");

                foreach (var order in orders)
                {
                    orderDetailsBuilder.AppendLine(order);
                }

                Label orderLabel = new Label
                {
                    Text = orderDetailsBuilder.ToString(),
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
            isBackButtonClicked = true; // Set flag to true when back button is clicked
            AdminForm admin = new AdminForm(username);
            admin.Refresh();
            admin.Show();
            this.Close();
        }
    }
}