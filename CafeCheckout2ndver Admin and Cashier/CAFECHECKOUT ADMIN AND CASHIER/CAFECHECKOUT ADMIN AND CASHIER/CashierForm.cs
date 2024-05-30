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
        public CashierForm(string username)
        {
            InitializeComponent();
            AssignButtonClickEvents();
            SetPlaceholder();
            this.username = username;

            // Check the user's role and set the Backbutt visibility accordingly
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
                FetchTransactionData(queueNumber);
            }
            else
            {
                MessageBox.Show("Invalid queue number!");
            }
        }

        private void FetchTransactionData(int queueNumber)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string query = @"
                SELECT t.Transaction_Id, t.Customer_Id, t.Product_Id, t.Quantity, t.Addon_Id, t.Addon_Quantity, t.Total_Price, t.Status, t.Transaction_Time
                FROM dbo.Transactions t
                INNER JOIN dbo.Customer c ON t.Customer_Id = c.Customer_Id
                WHERE c.Queuing_num = @QueueNumber AND t.Status <> 'Complete'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@QueueNumber", queueNumber);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                flpTransactionData.Controls.Clear();

                while (reader.Read())
                {
                    string transactionText = $"Customer ID: {reader["Customer_Id"]}\n" +
                                             $"Transaction ID: {reader["Transaction_Id"]}\n" +
                                             $"Product ID: {reader["Product_Id"]}\n" +
                                             $"Quantity: {reader["Quantity"]}\n" +
                                             $"Addon ID: {reader["Addon_Id"]}\n" +
                                             $"Addon Quantity: {reader["Addon_Quantity"]}\n" +
                                             $"Total Price: {reader["Total_Price"]}\n" +
                                             $"Status: {reader["Status"]}\n" +
                                             $"Transaction Time: {reader["Transaction_Time"]}";

                    Label lblTransaction = new Label
                    {
                        Text = transactionText,
                        AutoSize = true,
                        Font = new Font("Rockwell", 20, FontStyle.Italic),
                        Padding = new Padding(0, 0, 0, 0) // Add some padding between transactions
                    };
                    flpTransactionData.Controls.Add(lblTransaction);
                }

                reader.Close();
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

            CompleteTransaction(queueNumber, customerMoney, discountPercent, txtDiscountCode.Text);
        }

        private decimal GetDiscountPercent(string discountCode)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string query = "SELECT DiscountPercent FROM dbo.DiscountCodes WHERE Code = @Code";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Code", discountCode);
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && decimal.TryParse(result.ToString(), out decimal discountPercent))
                {
                    return discountPercent;
                }
            }

            return -1; // Invalid discount code
        }

        private void CompleteTransaction(int queueNumber, decimal customerMoney, decimal discountPercent, string discountCode)
        {
            string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
            string fetchQuery = @"
        SELECT t.Transaction_Id, t.Customer_Id, t.Product_Id, t.Quantity, t.Addon_Id, t.Addon_Quantity, t.Total_Price
        FROM dbo.Transactions t
        INNER JOIN dbo.Customer c ON t.Customer_Id = c.Customer_Id
        INNER JOIN dbo.Products p ON t.Product_Id = p.Product_Id
        WHERE c.Queuing_num = @QueueNumber AND t.Status <> 'Complete'";

            string insertQuery = @"
        INSERT INTO dbo.Trsn_Complete (Transaction_Id, Customer_Id, Product_Id, Quantity, Addon_Id, Addon_Quantity, Total_Price, Costumer_Money, Discount_Code, Final_Price, Change_Amount, Transaction_DateTime)
        VALUES (@Transaction_Id, @Customer_Id, @Product_Id, @Quantity, @Addon_Id, @Addon_Quantity, @Total_Price, @Customer_Money, @Discount_Code, @Final_Price, @Change_Amount, @Transaction_DateTime)";

            string updateStatusQuery = @"
        UPDATE dbo.Transactions
        SET Status = 'Complete'
        WHERE Transaction_Id = @Transaction_Id";

            List<Tuple<int, string, string, int, string, int?, decimal>> transactions = new List<Tuple<int, string, string, int, string, int?, decimal>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand fetchCommand = new SqlCommand(fetchQuery, connection);
                fetchCommand.Parameters.AddWithValue("@QueueNumber", queueNumber);

                using (SqlDataReader reader = fetchCommand.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No transactions found for the given queue number!");
                        return;
                    }

                    while (reader.Read())
                    {
                        int transactionId = (int)reader["Transaction_Id"];
                        string customerId = reader["Customer_Id"].ToString();
                        string productId = reader["Product_Id"].ToString();
                        int quantity = (int)reader["Quantity"];
                        string addonId = reader["Addon_Id"] as string;
                        int? addonQuantity = reader["Addon_Quantity"] as int?;
                        decimal totalPrice = (decimal)reader["Total_Price"];

                        transactions.Add(Tuple.Create(transactionId, customerId, productId, quantity, addonId, addonQuantity, totalPrice));
                    }
                }

                foreach (var transaction in transactions)
                {
                    int transactionId = transaction.Item1;
                    string customerId = transaction.Item2;
                    string productId = transaction.Item3;
                    int quantity = transaction.Item4;
                    string addonId = transaction.Item5;
                    int? addonQuantity = transaction.Item6;
                    decimal totalPrice = transaction.Item7;

                    decimal finalPrice = totalPrice * (1 - discountPercent / 100);
                    finalPrice = Math.Max(finalPrice, 0); // Ensure final price is not negative

                    decimal changeAmount = customerMoney - finalPrice;

                    if (finalPrice > customerMoney)
                    {
                        MessageBox.Show("Customer money is not enough to cover the total price!");
                        return;
                    }

                    // Ensure no cashback
                    if (discountPercent >= 100)
                    {
                        finalPrice = 0;
                        changeAmount = customerMoney; // In case of 100% discount, change should be equal to customer money
                    }

                    // Get current date and time
                    DateTime transactionDateTime = DateTime.Now;

                    // Insert into Trsn_Complete
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Transaction_Id", transactionId);
                    insertCommand.Parameters.AddWithValue("@Customer_Id", customerId);
                    insertCommand.Parameters.AddWithValue("@Product_Id", productId);
                    insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                    insertCommand.Parameters.AddWithValue("@Addon_Id", addonId ?? (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@Addon_Quantity", addonQuantity ?? (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@Total_Price", totalPrice);
                    insertCommand.Parameters.AddWithValue("@Customer_Money", customerMoney);
                    insertCommand.Parameters.AddWithValue("@Discount_Code", string.IsNullOrWhiteSpace(discountCode) ? (object)DBNull.Value : discountCode);
                    insertCommand.Parameters.AddWithValue("@Final_Price", finalPrice);
                    insertCommand.Parameters.AddWithValue("@Change_Amount", changeAmount);
                    insertCommand.Parameters.AddWithValue("@Transaction_DateTime", transactionDateTime);

                    insertCommand.ExecuteNonQuery();

                    // Update status in Transactions
                    SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, connection);
                    updateStatusCommand.Parameters.AddWithValue("@Transaction_Id", transactionId);
                    updateStatusCommand.ExecuteNonQuery();

                    // Print receipt
                    PrintReceipt(transactionId, customerId, productId, quantity, addonId, addonQuantity, totalPrice, finalPrice, discountPercent, customerMoney, changeAmount, transactionDateTime);
                }
            }

            MessageBox.Show("Transaction completed successfully!");
        }

        private void PrintReceipt(int transactionId, string customerId, string productId, int quantity, string addonId,int? addonQuantity, decimal totalPrice, decimal finalPrice, decimal discountPercent, decimal customerMoney, decimal changeAmount, DateTime transactionDateTime)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                StringBuilder receipt = new StringBuilder();
                receipt.AppendLine("CafeCheckOut Reciept");
                receipt.AppendLine("-------------------------------------");
                receipt.AppendLine($"Transaction ID: {transactionId}");
                receipt.AppendLine($"Customer ID: {customerId}");
                receipt.AppendLine("-------------------------------------");
                receipt.AppendLine($"Product ID: {productId}");
                receipt.AppendLine($"Quantity: {quantity}");
                if (!string.IsNullOrWhiteSpace(addonId))
                {
                receipt.AppendLine($"Addon ID: {addonId}");
                receipt.AppendLine($"Addon Quantity: {addonQuantity}");
                }
                receipt.AppendLine($"Total Price: {totalPrice:C}");
                if (discountPercent > 0)
                {
               receipt.AppendLine($"Discount: {discountPercent}%");
                }
                receipt.AppendLine($"Customer Money: {customerMoney:C}");
                receipt.AppendLine($"Final Price: {finalPrice:C}");
                receipt.AppendLine($"Change Amount: {changeAmount:C}");
                receipt.AppendLine($"Date and Time: {transactionDateTime}");
                receipt.AppendLine("-------------------------------------");
                receipt.AppendLine("Thank you!");
           

                // Define font
                Font font = new Font("Rockwell", 25, FontStyle.Italic);
                float yPos = 5;
                float leftMargin = e.MarginBounds.Left;

                // Split the receipt text into lines
                string[] lines = receipt.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    e.Graphics.DrawString(line, font, Brushes.Black, new PointF(leftMargin, yPos), new StringFormat
                    {
                        Alignment = StringAlignment.Near
                    });

                    yPos += font.GetHeight(e.Graphics);
                }
            };

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument
            };

            previewDialog.ShowDialog();
        }

        private void Signout_But_Click(object sender, EventArgs e)
        {
      
            DialogResult result = MessageBox.Show("Do you want to logout?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

      
            if (result == DialogResult.Yes)
            {
                string connectionString = @"Data Source=LAPTOP-R45B7D8N\SQLEXPRESS;Initial Catalog=Cafedatabase;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Accounts SET ACC_STAT = 'LOGGED OUT' WHERE Username = @username";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.ExecuteNonQuery();
                    }
                }

                Login_Interface assignedForm = new Login_Interface();
                assignedForm.Refresh();
                assignedForm.Show();

               
                this.Close();
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