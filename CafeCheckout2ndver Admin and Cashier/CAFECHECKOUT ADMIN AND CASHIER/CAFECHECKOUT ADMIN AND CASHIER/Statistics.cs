using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace CAFECHECKOUT_ADMIN_AND_CASHIER
{
    public partial class Statistics : Form
    {
        private string username;
        private string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";

        public Statistics(string username)
        {
            InitializeComponent();
            this.username = username;

            LoadProfitChart();
            LoadTotalCustomerPerDayChart();
            LoadMostBuyableProductsChart();
            UpdateLabels();
        }

        private void LoadProfitChart()
        {
            string query = "SELECT CONVERT(DATE, Transaction_DateTime) AS Date, SUM(Final_Price) AS TotalProfit FROM Trsn_Complete GROUP BY CONVERT(DATE, Transaction_DateTime) ORDER BY Date;";
            DataTable dataTable = ExecuteQuery(query);

            ProfitChart.Series.Clear();
            ProfitChart.Series.Add("Profit");
            ProfitChart.Series["Profit"].XValueMember = "Date";
            ProfitChart.Series["Profit"].YValueMembers = "TotalProfit";
            ProfitChart.DataSource = dataTable;
            ProfitChart.DataBind();
        }

        private void LoadTotalCustomerPerDayChart()
        {
            string query = "SELECT CONVERT(DATE, Entry_Time) AS Date, COUNT(Customer_Id) AS TotalCustomers FROM Customer GROUP BY CONVERT(DATE, Entry_Time) ORDER BY Date;";
            DataTable dataTable = ExecuteQuery(query);

            TotalCostumerPerday.Series.Clear();
            TotalCostumerPerday.Series.Add("Customers");
            TotalCostumerPerday.Series["Customers"].XValueMember = "Date";
            TotalCostumerPerday.Series["Customers"].YValueMembers = "TotalCustomers";
            TotalCostumerPerday.DataSource = dataTable;
            TotalCostumerPerday.DataBind();
        }

        private void LoadMostBuyableProductsChart()
        {
            string query = "SELECT Product_Id, SUM(Quantity) AS TotalQuantity FROM Transactions GROUP BY Product_Id ORDER BY TotalQuantity DESC;";
            DataTable dataTable = ExecuteQuery(query);

            MostBuyableProducts.Series.Clear();
            MostBuyableProducts.Series.Add("Products");
            MostBuyableProducts.Series["Products"].XValueMember = "Product_Id";
            MostBuyableProducts.Series["Products"].YValueMembers = "TotalQuantity";
            MostBuyableProducts.DataSource = dataTable;
            MostBuyableProducts.DataBind();
        }

        private void UpdateLabels()
        {
            string profitQuery = "SELECT SUM(Final_Price) AS TotalProfit FROM Trsn_Complete;";
            string customerQuery = "SELECT COUNT(DISTINCT Customer_Id) AS TotalCustomers FROM Customer;";
            string mostBuyableProductQuery = "SELECT TOP 1 Product_Id, SUM(Quantity) AS TotalQuantity FROM Transactions GROUP BY Product_Id ORDER BY TotalQuantity DESC;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Calculate total profit
                SqlCommand profitCommand = new SqlCommand(profitQuery, connection);
                object profitResult = profitCommand.ExecuteScalar();
                decimal totalProfit = (profitResult != DBNull.Value) ? Convert.ToDecimal(profitResult) : 0;
                label1.Text = $"Total Profit: {totalProfit:C}";

                // Calculate total customers
                SqlCommand customerCommand = new SqlCommand(customerQuery, connection);
                object customerResult = customerCommand.ExecuteScalar();
                int totalCustomers = (customerResult != DBNull.Value) ? Convert.ToInt32(customerResult) : 0;
                label2.Text = $"Total Customers: {totalCustomers}";

                // Get most buyable product
                SqlCommand mostBuyableProductCommand = new SqlCommand(mostBuyableProductQuery, connection);
                SqlDataReader reader = mostBuyableProductCommand.ExecuteReader();
                if (reader.Read())
                {
                    string mostBuyableProduct = reader["Product_Id"].ToString();
                    int totalQuantity = Convert.ToInt32(reader["TotalQuantity"]);
                    label3.Text = $"(Product: {mostBuyableProduct}) \n" +
                                  $"(Total Quantity: {totalQuantity})";
                }
                else
                {
                    label3.Text = "Most Buyable Product: N/A";
                }
                reader.Close();
            }
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

        private void Backbutt_Click(object sender, EventArgs e)
        {
            AdminForm admin = new AdminForm(username);
            admin.Refresh();
            admin.Show();
            this.Close();
        }
    }
}