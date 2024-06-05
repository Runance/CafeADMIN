using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CAFECHECKOUT_ADMIN_AND_CASHIER
{
    public partial class CashierForm : Form
    {

        private string username;
        private bool isBackButtonClicked = false;

        public CashierForm(string username)
        {
            InitializeComponent();
            AssignButtonClickEvents();
            SetPlaceholder();
            this.username = username;

            using (var connection = new SqlConnection(@"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;"))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT Role FROM Accounts WHERE Username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader["Role"].ToString();
                            if (role == "ADMIN")
                            {
                                Backbutt.Visible = true;
                                Signout_But.Visible = false;
                            }
                            else
                            {
                                Backbutt.Visible = false;
                                Signout_But.Visible = true;
                            }
                        }
                    }
                }
            }
            this.FormClosing += CashierForm_FormClosing;
        }
        private void CashierForm_FormClosing(object sender, FormClosingEventArgs e)
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
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
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
        private void AssignButtonClickEvents()
        {
            btn1.Click += NumPadButton_Click;
            btn2.Click += NumPadButton_Click;
            btn3.Click += NumPadButton_Click;
            btn4.Click += NumPadButton_Click;
            btn5.Click += NumPadButton_Click;
            btn6.Click += NumPadButton_Click;
            btn7.Click += NumPadButton_Click;
            btn8.Click += NumPadButton_Click;
            btn9.Click += NumPadButton_Click;
            btn0.Click += NumPadButton_Click;
            btnEnter.Click += BtnEnter_Click;
            btnClear.Click += BtnClear_Click;
            Proceed.Click += Proceed_Click;
        }

        private void SetPlaceholder()
        {
            txtQueueNumber.Text = "Enter Queuing num";
            txtQueueNumber.ForeColor = Color.Gray;

            txtQueueNumber.GotFocus += RemovePlaceholder;
            txtQueueNumber.LostFocus += SetPlaceholderOnFocusLost;
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (txtQueueNumber.Text == "Enter Queuing num")
            {
                txtQueueNumber.Text = "";
                txtQueueNumber.ForeColor = Color.Black;
            }
        }

        private void SetPlaceholderOnFocusLost(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQueueNumber.Text))
            {
                txtQueueNumber.Text = "Enter Queuing num";
                txtQueueNumber.ForeColor = Color.Gray;
            }
        }

        private void NumPadButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (txtQueueNumber.Text == "Enter Queuing num")
                {
                    txtQueueNumber.Text = "";
                    txtQueueNumber.ForeColor = Color.Black;
                }
                txtQueueNumber.Text += button.Text;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtQueueNumber.Clear();
            SetPlaceholderOnFocusLost(sender, e); // Ensure placeholder is shown if cleared
        }

        private void BtnEnter_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtQueueNumber.Text, out int queueNumber))
            {
                if (!IsCustomerAvailable(queueNumber))
                {
                    MessageBox.Show("This customer is not available.");
                    return;
                }

                if (IsCustomerCompleted(queueNumber))
                {
                    MessageBox.Show("This customer has already completed the transaction.");
                    return;
                }

                FetchTransactionData(queueNumber);
            }
            else
            {
                MessageBox.Show("Invalid queue number!");
            }
        }
        private bool IsCustomerAvailable(int queueNumber)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string query = @"
            SELECT c.Entry_type, t.Status
            FROM dbo.Customer c
            LEFT JOIN dbo.Transactions t ON c.Customer_Id = t.Customer_Id
            WHERE c.Queuing_num = @QueueNumber";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QueueNumber", queueNumber);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string entryType = reader["Entry_type"].ToString();
                            string status = reader["Status"].ToString();

                            if (entryType == "Cancelled" || status == "Cancelled")
                            {
                                return false; // Customer is not available
                            }
                        }
                    }
                }
            }
            return true; // Customer is available
        }
        private bool IsCustomerCompleted(int queueNumber)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string query = @"
            SELECT COUNT(*)
            FROM dbo.Transactions t
            INNER JOIN dbo.Customer c ON t.Customer_Id = c.Customer_Id
            WHERE c.Queuing_num = @QueueNumber AND t.Status = 'Complete'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QueueNumber", queueNumber);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void FetchTransactionData(int queueNumber)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string query = @"
            SELECT t.Transaction_Id, t.Customer_Id, c.Queuing_num, p.Product_Name, t.Quantity, a.Addon_Name, t.Addon_Quantity, t.Total_Price
            FROM dbo.Transactions t
            INNER JOIN dbo.Customer c ON t.Customer_Id = c.Customer_Id
            INNER JOIN dbo.Products p ON t.Product_Id = p.Product_Id
            LEFT JOIN dbo.Add_Ons a ON t.Addon_Id = a.Addon_Id
            WHERE t.Status <> 'Complete' AND c.Queuing_num = @QueueNumber";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QueueNumber", queueNumber);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        flpTransactionData.Controls.Clear();
                        decimal totalPrice = 0;
                        string customerId = string.Empty;
                        int queuingNum = 0;
                        List<string> productDetails = new List<string>();
                        List<string> addonDetails = new List<string>();

                        while (reader.Read())
                        {
                            customerId = reader["Customer_Id"].ToString();
                            queuingNum = (int)reader["Queuing_num"];
                            string productName = reader["Product_Name"].ToString();
                            int quantity = (int)reader["Quantity"];
                            string addonName = reader["Addon_Name"].ToString();
                            int addonQuantity = (int)reader["Addon_Quantity"];
                            decimal price = (decimal)reader["Total_Price"];
                            totalPrice += price;

                            productDetails.Add($"Product: {productName}, Quantity: {quantity}");
                            addonDetails.Add($"Addon: {addonName}, Addon Quantity: {addonQuantity}");
                        }

                        if (!string.IsNullOrEmpty(customerId))
                        {
                            // Display customer ID and queue number
                            Label customerInfoLabel = new Label
                            {
                                Text = $"Customer ID: {customerId}",
                                AutoSize = true,
                                Font = new Font("Rockwell", 20, FontStyle.Italic)
                            };
                            flpTransactionData.Controls.Add(customerInfoLabel);

                            Label queueNumLabel = new Label
                            {
                                Text = $"Queue Number: {queuingNum}",
                                AutoSize = true,
                                Font = new Font("Rockwell", 20, FontStyle.Italic)
                            };
                            flpTransactionData.Controls.Add(queueNumLabel);

                            // Display products
                            foreach (string productDetail in productDetails)
                            {
                                Label productLabel = new Label
                                {
                                    Text = productDetail,
                                    AutoSize = true,
                                    Font = new Font("Rockwell", 20, FontStyle.Italic)
                                };
                                flpTransactionData.Controls.Add(productLabel);
                            }

                            // Display addons
                            foreach (string addonDetail in addonDetails)
                            {
                                Label addonLabel = new Label
                                {
                                    Text = addonDetail,
                                    AutoSize = true,
                                    Font = new Font("Rockwell", 20, FontStyle.Italic)
                                };
                                flpTransactionData.Controls.Add(addonLabel);
                            }

                            // Display total price
                            Label totalPriceLabel = new Label
                            {
                                Text = $"Total Price: {totalPrice:C}",
                                AutoSize = true,
                                Font = new Font("Rockwell", 20, FontStyle.Italic)
                            };
                            flpTransactionData.Controls.Add(totalPriceLabel);

                            // Make the FlowLayoutPanel scrollable
                            flpTransactionData.AutoScroll = true;
                        }
                        else
                        {
                            MessageBox.Show("No transactions found for the given queue number.");
                        }
                    }
                }
            }
        }
        private void Proceed_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQueueNumber.Text, out int queueNumber))
            {
                MessageBox.Show("Invalid queue number!");
                return;
            }

            if (!decimal.TryParse(txtCustomerMoney.Text, out decimal customerMoney))
            {
                MessageBox.Show("Invalid amount entered!");
                return;
            }

            decimal discountPercent = 0;
            if (!string.IsNullOrWhiteSpace(txtDiscountCode.Text))
            {
                discountPercent = GetDiscountPercent(txtDiscountCode.Text);
                if (discountPercent < 0)
                {
                    MessageBox.Show("Invalid discount code!");
                    return;
                }
            }

            decimal totalPrice = GetTotalPrice(queueNumber);

            // Check if the total price exceeds the customer's money limit
            if (totalPrice > customerMoney)
            {
                MessageBox.Show("Customer's money is not enough to cover the total price!");
                return;
            }

            CompleteTransaction(queueNumber, customerMoney, discountPercent, txtDiscountCode.Text);
        }


        private decimal GetTotalPrice(int queueNumber)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string query = @"
                SELECT SUM(t.Total_Price) AS TotalPrice
                FROM dbo.Transactions t
                INNER JOIN dbo.Customer c ON t.Customer_Id = c.Customer_Id
                WHERE c.Queuing_num = @QueueNumber AND t.Status <> 'Complete'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QueueNumber", queueNumber);
                    object result = command.ExecuteScalar();
                    if (result != null && decimal.TryParse(result.ToString(), out decimal totalPrice))
                    {
                        return totalPrice;
                    }
                }
            }
            return 0;
        }

        private decimal GetDiscountPercent(string discountCode)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string query = "SELECT DiscountPercent FROM dbo.DiscountCodes WHERE Code = @DiscountCode";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DiscountCode", discountCode);
                    object result = command.ExecuteScalar();
                    if (result != null && decimal.TryParse(result.ToString(), out decimal discountPercent))
                    {
                        return discountPercent;
                    }
                }
            }
            return -1;
        }

        private void CompleteTransaction(int queueNumber, decimal customerMoney, decimal discountPercent, string discountCode)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string query = @"
            SELECT t.Customer_Id, t.Transaction_Id, t.Total_Price, t.Product_Id, p.Product_Name, t.Quantity, a.Addon_Id, a.Addon_Name, t.Addon_Quantity, t.Transaction_Time
            FROM dbo.Transactions t
            INNER JOIN dbo.Customer c ON t.Customer_Id = c.Customer_Id
            LEFT JOIN dbo.Products p ON t.Product_Id = p.Product_Id
            LEFT JOIN dbo.Add_Ons a ON t.Addon_Id = a.Addon_Id
            WHERE c.Queuing_num = @QueueNumber AND t.Status <> 'Complete'";

            List<(string CustomerId, string TransactionId, string ProductId, string ProductName, int Quantity, string AddonId, string AddonName, int AddonQuantity, decimal TotalPrice, DateTime TransactionTime, int QueueNumber)> transactions = new List<(string, string, string, string, int, string, string, int, decimal, DateTime, int)>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QueueNumber", queueNumber);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string customerId = reader["Customer_Id"].ToString();
                            string transactionId = reader["Transaction_Id"].ToString();
                            string productId = reader["Product_Id"].ToString();
                            string productName = reader["Product_Name"].ToString();
                            int quantity = (int)reader["Quantity"];
                            string addonId = reader["Addon_Id"].ToString();
                            string addonName = reader["Addon_Name"].ToString();
                            int addonQuantity = (int)reader["Addon_Quantity"];
                            decimal totalPrice = (decimal)reader["Total_Price"];
                            DateTime transactionTime = (DateTime)reader["Transaction_Time"];
                            transactions.Add((customerId, transactionId, productId, productName, quantity, addonId, addonName, addonQuantity, totalPrice, transactionTime, queueNumber));
                        }
                    }
                }

                if (transactions.Count == 0)
                {
                    MessageBox.Show("No transactions found for the given queue number.", "No Transactions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (var (customerId, transactionId, productId, productName, quantity, addonId, addonName, addonQuantity, totalPrice, transactionTime, queueNum) in transactions)
                {
                    decimal discountAmount = totalPrice * (discountPercent / 100);
                    decimal discountedTotal = totalPrice - discountAmount;
                    decimal change = customerMoney - discountedTotal;

                    string insertQuery = @"
                    INSERT INTO dbo.Trsn_Complete (Transaction_Id, Customer_Id, Product_Id, Quantity, Addon_Id, Addon_Quantity, Total_Price, Costumer_Money, Discount_Code, Final_Price, Change_Amount, Transaction_DateTime)
                    VALUES (@TransactionId, @CustomerId, @ProductId, @Quantity, @AddonId, @AddonQuantity, @TotalPrice, @CustomerMoney, @DiscountCode, @FinalPrice, @ChangeAmount, @TransactionDateTime)";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@TransactionId", transactionId);
                        insertCommand.Parameters.AddWithValue("@CustomerId", customerId);
                        insertCommand.Parameters.AddWithValue("@ProductId", productId);
                        insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                        insertCommand.Parameters.AddWithValue("@AddonId", string.IsNullOrWhiteSpace(addonId) ? DBNull.Value : (object)addonId);
                        insertCommand.Parameters.AddWithValue("@AddonQuantity", addonQuantity);
                        insertCommand.Parameters.AddWithValue("@TotalPrice", totalPrice);
                        insertCommand.Parameters.AddWithValue("@CustomerMoney", customerMoney);
                        insertCommand.Parameters.AddWithValue("@DiscountCode", string.IsNullOrWhiteSpace(discountCode) ? DBNull.Value : (object)discountCode);
                        insertCommand.Parameters.AddWithValue("@FinalPrice", discountedTotal);
                        insertCommand.Parameters.AddWithValue("@ChangeAmount", change);
                        insertCommand.Parameters.AddWithValue("@TransactionDateTime", transactionTime);
                        insertCommand.ExecuteNonQuery();
                    }

                    string updateQuery = "UPDATE dbo.Transactions SET Status = 'Complete' WHERE Transaction_Id = @TransactionId";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@TransactionId", transactionId);
                        updateCommand.ExecuteNonQuery();
                    }
                }

                // Print the receipt after all updates are done
                var printTransactions = transactions.Select(t => (t.TransactionId, t.CustomerId, t.ProductName, t.Quantity, t.AddonName, t.AddonQuantity, t.TransactionTime, t.TotalPrice, t.QueueNumber)).ToList();
                PrintReceipt(printTransactions, customerMoney, customerMoney - transactions.Sum(t => t.TotalPrice) + (transactions.Sum(t => t.TotalPrice) * (discountPercent / 100)), discountPercent, discountCode);

                // Show message after printing
                MessageBox.Show("Transaction complete! Receipt printed.", "Transaction Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the display on the flow layout panel
                flpTransactionData.Controls.Clear();
            }
        }

        private void PrintReceipt(List<(string TransactionId, string CustomerId, string ProductName, int Quantity, string AddonName, int AddonQuantity, DateTime TransactionTime, decimal TotalPrice, int QueueNumber)> transactions, decimal customerMoney, decimal change, decimal discountPercent, string discountCode)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                int yPos = 0;
                Font printFont = new Font("Rockwell", 15, FontStyle.Italic);
                Font boldFont = new Font("Rockwell", 15, FontStyle.Italic);

                // Shop Name
                e.Graphics.DrawString("CafeCheckOut", boldFont, Brushes.Black, new PointF(0, yPos));
                yPos += 30;

                // Partition
                e.Graphics.DrawString(new string('-', 40), printFont, Brushes.Black, new PointF(0, yPos));
                yPos += 30;

                var firstTransaction = transactions.FirstOrDefault();
                if (firstTransaction != default)
                {
                    e.Graphics.DrawString($"Customer ID: {firstTransaction.CustomerId}", printFont, Brushes.Black, new PointF(0, yPos));
                    yPos += 30;
                    e.Graphics.DrawString($"Queue Number: {firstTransaction.QueueNumber}", printFont, Brushes.Black, new PointF(0, yPos));
                    yPos += 30;
                }

                decimal totalAmount = 0;

                bool isFirstTransaction = true;
                foreach (var transaction in transactions)
                {
                    e.Graphics.DrawString($"Product Name: {transaction.ProductName}", printFont, Brushes.Black, new PointF(0, yPos));
                    yPos += 30;

                    e.Graphics.DrawString($"Quantity: {transaction.Quantity}", printFont, Brushes.Black, new PointF(0, yPos));
                    yPos += 30;

                    if (!string.IsNullOrEmpty(transaction.AddonName))
                    {
                        e.Graphics.DrawString($"Addon Name: {transaction.AddonName}", printFont, Brushes.Black, new PointF(0, yPos));
                        yPos += 30;

                        e.Graphics.DrawString($"Addon Quantity: {transaction.AddonQuantity}", printFont, Brushes.Black, new PointF(0, yPos));
                        yPos += 30;
                    }

                    if (isFirstTransaction)
                    {
                        e.Graphics.DrawString($"Transaction Time: {transaction.TransactionTime:HH:mm}", printFont, Brushes.Black, new PointF(0, yPos));
                        yPos += 30;
                        isFirstTransaction = false;
                    }

                    e.Graphics.DrawString($"Total Price: {transaction.TotalPrice:C}", printFont, Brushes.Black, new PointF(0, yPos));
                    yPos += 30;

                    totalAmount += transaction.TotalPrice;

                    e.Graphics.DrawString(new string('-', 40), printFont, Brushes.Black, new PointF(0, yPos));
                    yPos += 30;
                }

                decimal discountAmount = totalAmount * (discountPercent / 100);
                decimal finalPrice = totalAmount - discountAmount;

                e.Graphics.DrawString($"Total Amount: {totalAmount:C}", printFont, Brushes.Black, new PointF(0, yPos));
                yPos += 30;

                e.Graphics.DrawString($"Discount Percent: {discountPercent}%", printFont, Brushes.Black, new PointF(0, yPos));
                yPos += 30;

                e.Graphics.DrawString($"Discount Code: {discountCode}", printFont, Brushes.Black, new PointF(0, yPos));
                yPos += 30;

                e.Graphics.DrawString($"Final Price: {finalPrice:C}", printFont, Brushes.Black, new PointF(0, yPos));
                yPos += 30;

                e.Graphics.DrawString($"Customer Money: {customerMoney:C}", printFont, Brushes.Black, new PointF(0, yPos));
                yPos += 30;

                e.Graphics.DrawString($"Change: {change:C}", printFont, Brushes.Black, new PointF(0, yPos));
                yPos += 30;

                // Partition
                e.Graphics.DrawString(new string('-', 40), printFont, Brushes.Black, new PointF(0, yPos));
                yPos += 30;

                // Thank You Message
                e.Graphics.DrawString("Thank you! Please come again.", printFont, Brushes.Black, new PointF(0, yPos));
            };

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog
            {
                Document = printDocument
            };

            if (printPreviewDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }
        private void Backbutt_Click(object sender, EventArgs e)
        {
                isBackButtonClicked = true;
                AdminForm admin = new AdminForm(username);
                admin.Refresh();
                admin.Show();
                this.Close();
            
        }

        private void Signout_But_Click(object sender, EventArgs e)
        {
            isBackButtonClicked = true;
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                UpdateAccountStatusToLoggedOut(username);
                Login_Interface loginForm = new Login_Interface();
                loginForm.Show();
                this.Hide();
            }
        }
    }
}
