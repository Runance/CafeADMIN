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
        private bool ButtonClicked = false;

        public Statistics(string username)
        {
            InitializeComponent();
            this.username = username;

            LoadProfitChart();
            LoadTotalCustomerPerDayChart();
            LoadMostBuyableProductsChart();
            UpdateLabels();
            this.FormClosing += Statistics_FormClosing;
        }

        private void Statistics_FormClosing(object sender, FormClosingEventArgs e)
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
        private void LoadProfitChart()
        {
            string query = @"
        SELECT 
            CONVERT(DATE, Transaction_DateTime) AS Date, 
            SUM(Final_Price) AS TotalProfit 
        FROM 
            Trsn_Complete 
        GROUP BY 
            CONVERT(DATE, Transaction_DateTime) 
        ORDER BY 
            Date;";

            DataTable dataTable = ExecuteQuery(query);

            if (dataTable.Rows.Count > 0)
            {
                ProfitChart.Series.Clear();
                var series = new Series("Profit")
                {
                    XValueType = ChartValueType.Date,
                    YValueType = ChartValueType.Double,
                    ChartType = SeriesChartType.Bar,
                };
                ProfitChart.Series.Add(series);

                foreach (DataRow row in dataTable.Rows)
                {
                    series.Points.AddXY(Convert.ToDateTime(row["Date"]), Convert.ToDouble(row["TotalProfit"]));
                }

                ProfitChart.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy";
                ProfitChart.ChartAreas[0].AxisX.Interval = 1;
                ProfitChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                ProfitChart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                ProfitChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
                ProfitChart.DataBind();
            }
            else
            {
                MessageBox.Show("No data available for the Profit Chart.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadTotalCustomerPerDayChart()
        {
            string query = @"
        SELECT 
            CONVERT(DATE, Entry_Time) AS Date, 
            COUNT(Customer_Id) AS TotalCustomers 
        FROM 
            Customer 
        GROUP BY 
            CONVERT(DATE, Entry_Time) 
        ORDER BY 
            Date;";

            DataTable dataTable = ExecuteQuery(query);

            if (dataTable.Rows.Count > 0)
            {
                TotalCostumerPerday.Series.Clear();
                var series = new Series("Customers")
                {
                    XValueType = ChartValueType.Date,
                    YValueType = ChartValueType.Int32,
                    ChartType = SeriesChartType.Bar,
                };
                TotalCostumerPerday.Series.Add(series);

                foreach (DataRow row in dataTable.Rows)
                {
                    series.Points.AddXY(Convert.ToDateTime(row["Date"]), Convert.ToInt32(row["TotalCustomers"]));
                }

                TotalCostumerPerday.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy";
                TotalCostumerPerday.ChartAreas[0].AxisX.Interval = 1;
                TotalCostumerPerday.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                TotalCostumerPerday.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                TotalCostumerPerday.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
                TotalCostumerPerday.DataBind();
            }
            else
            {
                MessageBox.Show("No data available for the Total Customers Per Day Chart.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadMostBuyableProductsChart()
        {
            string query = @"
        SELECT 
            Product_Id, 
            SUM(Quantity) AS TotalQuantity 
        FROM 
            Transactions 
        GROUP BY 
            Product_Id 
        ORDER BY 
            TotalQuantity DESC;";

            DataTable dataTable = ExecuteQuery(query);

            if (dataTable.Rows.Count > 0)
            {
                MostBuyableProducts.Series.Clear();
                var series = new Series("Products")
                {
                    XValueType = ChartValueType.String,
                    YValueType = ChartValueType.Int32,
                    ChartType = SeriesChartType.Bar,
                };
                MostBuyableProducts.Series.Add(series);

                foreach (DataRow row in dataTable.Rows)
                {
                    series.Points.AddXY(row["Product_Id"].ToString(), Convert.ToInt32(row["TotalQuantity"]));
                }

                MostBuyableProducts.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                MostBuyableProducts.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
                MostBuyableProducts.DataBind();
            }
            else
            {
                MessageBox.Show("No data available for the Most Buyable Products Chart.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateLabels()
        {
            string profitQuery = "SELECT SUM(Final_Price) AS TotalProfit FROM Trsn_Complete;";
            string customerQuery = "SELECT COUNT(DISTINCT Customer_Id) AS TotalCustomers FROM Customer;";
            string mostBuyableProductQuery = @"
                SELECT 
                    TOP 1 Product_Id, 
                    SUM(Quantity) AS TotalQuantity 
                FROM 
                    Transactions 
                GROUP BY 
                    Product_Id 
                ORDER BY 
                    TotalQuantity DESC;";

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
                    label3.Text = $"Most Buyable Product: {mostBuyableProduct}\nTotal Quantity: {totalQuantity}";
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
            ButtonClicked = true;
            AdminForm admin = new AdminForm(username);
            admin.Refresh();
            admin.Show();
            this.Close();
        }
    }
}