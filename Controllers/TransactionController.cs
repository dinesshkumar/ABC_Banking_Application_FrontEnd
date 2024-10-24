/**
 * TransactionController.cs
 * Description: This file contains the main logic for the Getting Transaction from database.
 * Author: Dinessh Kumar
 * Last modified: January 19, 2024
 */

using Microsoft.AspNetCore.Mvc;
using htmlDataTest.Models;
using Microsoft.Data.SqlClient;


namespace htmlDataTest.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetTransaction()
        {
            String connectionString = "Data Source=(localdb)\\dinessh_local;Initial Catalog=ABC_database;Integrated Security=True";
            ///String sql = "SELECT * FROM accounts";
            ///String sql = "SELECT[customer_id] FROM[ABC_database].[dbo].[user_table] WHERE[USERNAME] = 'dinesshapp@gmail.com'";
            ///String sql = "SELECT[customer_id] FROM user_table WHERE USERNAME = 'dinesshapp@gmail.com'";
            Console.WriteLine(User.Identity?.Name);

            String sql = "SELECT * FROM transaction_table";

            var listTransaction = new List<TransactionModel>();
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var transaction = new TransactionModel
                    {
                        AccountId = (int)(rdr["account_id"]),
                        Transaction_type = (string)rdr["transaction_type"],
                        Amount = (float)rdr["amount"],
                        Transaction_date = (DateTime)rdr["transaction_date"],
                        Description = (string)rdr["Description"],
                        Deposit = (float)rdr["deposit"],
                        Withdraw = (float)rdr["withdraw"],
                        Balance = (float)rdr["balance"],
                        Transaction_status = (string)rdr["transaction_status"],
                        Approval_status = (bool)rdr["approval_status"]
                    };


                    ///student.accountId = (int)rdr["account_id"];
                    ///student.accountNumber = (string)rdr["account_number"];
                    ///student.accountType = (string)rdr["account_type"];

                    listTransaction.Add(transaction);
                }
                conn.Close();
            }
            return View(listTransaction);
        }
    }
}