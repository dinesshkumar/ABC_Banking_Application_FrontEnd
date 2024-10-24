/**
 * AccountController.cs
 * Description: This file contains the main logic for the Customer Login application, Recent transaction and navigation of Customer Account Page.
 * Author: Dinessh Kumar
 * Last modified: January 19, 2024
 */



using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using htmlDataTest.Models;
using System.Security.Cryptography;
using System.Text;

namespace htmlDataTest.Controllers
{
    public static class Config
    {
        public static string? Username;
        public static string? Customer_id;
        public static List<TransactionModel> transactionModels = new List<TransactionModel>();
    }
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CustomerSubmit(string userEmail, string userPassword)
        {
            ViewBag.UserEmail = userEmail;

            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(userPassword);
            byte[] hash = sha512.ComputeHash(bytes);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            var hashedPassword = result.ToString();
            hashedPassword = hashedPassword.Substring(0, 45);

            string customer_id="";
            String connectionString = "Data Source=(localdb)\\dinessh_local;Initial Catalog=ABC_database;Integrated Security=True";
            String sql = "SELECT customer_id FROM user_table WHERE email ='" + userEmail + "' and password='"+ hashedPassword + "'";

            //var model = new List<AccountsModel>();
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();


                if (rdr.Read())
                {
                    customer_id = rdr[0].ToString();

                    ViewBag.customer_id = rdr[0].ToString();
                    Config.Customer_id = customer_id;
                    using (SqlConnection conn2 = new(connectionString))
                    {
                        sql = "select first_name from customer where customer_id = '" + customer_id + "'";
                        cmd = new SqlCommand(sql, conn2);
                        conn2.Open();
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            ViewBag.userName = rdr[0].ToString();
                            Config.Username = ViewBag.userName;
                            

                        }
                        conn2.Close();
                    }

                }
                conn.Close();
            }
            Config.Customer_id = customer_id;
            
            if (customer_id == "") { 
                return View("~/Views/Login/CustomerLoginPageFailed.cshtml");
            }
            connectionString = "Data Source=(localdb)\\dinessh_local;Initial Catalog=ABC_database;Integrated Security=True";
            ///String sql = "SELECT * FROM accounts";
            ///String sql = "SELECT[customer_id] FROM[ABC_database].[dbo].[user_table] WHERE[USERNAME] = 'dinesshapp@gmail.com'";
            ///String sql = "SELECT[customer_id] FROM user_table WHERE USERNAME = 'dinesshapp@gmail.com'";
            Console.WriteLine(User.Identity?.Name);

            sql = "SELECT * FROM transaction_table";

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
                        //AccountId = (int)Convert.ToSingle(rdr["account_id"]),
                        Transaction_type = (string)rdr["transaction_type"],
                        //Amount = (float)rdr["amount"],
                        Transaction_date = (DateTime)rdr["transaction_date"],
                        Description = (string)rdr["Description"],
                       // Deposit = (float)rdr["deposit"],
                        //Withdraw = (float)rdr["withdraw"],
                        //Balance = (float)rdr["balance"],
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
            Config.transactionModels = listTransaction;
 

            return View("~/Views/Account/CustomerAccountHomePage.cshtml");

        }
        
        public IActionResult HomePage()
        {
            ViewBag.Customer_id = Config.Customer_id;
            ViewBag.userName = Config.Username;
            return View("~/Views/Account/CustomerAccountHomePage.cshtml"); 
        }
        public IActionResult AccountsPage()
        {
            ViewBag.Customer_id = Config.Customer_id;
            ViewBag.userName = Config.Username; 
            return View("~/Views/Account/CustomerAccountAccountsPage.cshtml"); 
        }

        public IActionResult AccountAccountsPage()
        {
            ViewBag.Customer_id = Config.Customer_id;
            ViewBag.userName = Config.Username;
            return View("~/Views//AccountsPage/Accounts.cshtml");
        }

        public IActionResult FundTransferPage()
        {
            ViewBag.Customer_id = Config.Customer_id;
            ViewBag.userName = Config.Username;
            return View("~/Views/Account/CustomerAccountFundTransferPage.cshtml");
            //return View("~/Views/AccountsPage/LoanRequest.cshtml"); ;
        }
        public IActionResult RecentTransactions()
        {
            ViewBag.Customer_id = Config.Customer_id;
            ViewBag.userName = Config.Username;
            string connectionString = "Data Source=(localdb)\\dinessh_local;Initial Catalog=ABC_database;Integrated Security=True";

            Console.WriteLine(Config.Customer_id);

            //string sql = "SELECT * FROM TRANSACTION_TABLE";// WHERE customer_id=" + Config.Customer_id;
            string sql = "select * from transaction_table where account_number=(select TOP 1 account_number from accounts where customer_id='" + Config.Customer_id + "')";
            //string sql = "SELECT transaction_table.transaction_date, transaction_table.description ,transaction_table.amount ,transaction_table.balance, accounts.account_type FROM transaction_table FULL OUTER JOIN accounts ON transaction_table.account_number=accounts.account_number WHERE accounts.customer_id = '2024011912131691'";// + ViewBag.Customer_id + "'";
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

                            Amount = Convert.ToSingle(rdr["amount"]),
                            Transaction_date = (DateTime)rdr["transaction_date"],
                            Description = (string)rdr["Description"],

                            Balance = Convert.ToSingle(rdr["balance"]),
                            //Account_Type = (string)rdr["account_type"]
                        };


                        ///student.accountId = (int)rdr["account_id"];
                        ///student.accountNumber = (string)rdr["account_number"];
                        ///student.accountType = (string)rdr["account_type"];

                        listTransaction.Add(transaction);
                    }

                ViewBag.transactionDetails = listTransaction;
                conn.Close();
            }
            //Config.transactionModels = listTransaction;
            return View("~/Views/Account/CustomerAccountRecentTransactions.cshtml");
            //return View("~/Views/AccountsPage/LoanRequest.cshtml"); ;
        }
        public IActionResult ManageProfilePage()
        {
            ViewBag.Customer_id = Config.Customer_id;
            ViewBag.userName = Config.Username;
            return View("~/Views/Account/CustomerAccountManageProfilePage.cshtml");
        }
        public IActionResult LogOff()
        {
            return View("~/Views/Home/Index.cshtml");
        }
        public IActionResult FailedLogIn()
        {
            return View("~/Views/Home/Index.cshtml");
        }
        public IActionResult NewAccountRequest()
        {
            return View("~/Views/Account/CustomerAccountAccountsPage.cshtml"); 
        }
        public IActionResult RemoveAccountRequest()
        {
            return View("~/Views/Account/CustomerAccountAccountsPage.cshtml");
        }

        public IActionResult LoanRequest(string loanType, string durationSelect, string otherDurations,string loanAmount, string comments )
        {
            //Getting Previous Account Number
            string? customer_id = Config.Customer_id;
            string strAccount_number = "";
            long account_number;
            string connectionString = "Data Source=(localdb)\\dinessh_local;Initial Catalog=ABC_database;Integrated Security=True";
            string accountNumber_sql = "select account_number from accounts where account_id=(SELECT MAX(account_id) FROM accounts)";
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(accountNumber_sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    strAccount_number = rdr[0].ToString();
                }
                conn.Close();
            }
            account_number = long.Parse(strAccount_number);


            durationSelect = durationSelect + otherDurations;
            //if (durationSelect == "other")
            //{
            //    durationSelect = otherDurations; 
            //}
            //Inserting Loan Table
            string insertLoanTable_sql = "INSERT INTO loan_table (customer_id, loan_type, repayment_duration, amount, comments, request_date, active_status, approve_status)" +
                                             "VALUES ('" + Config.Customer_id + "','"+loanType+"','"+durationSelect+"','"+loanAmount+"','"+comments+"',CURRENT_TIMESTAMP, 2, 'Pending')";
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(insertLoanTable_sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                conn.Close();
            }
            return View("~/Views/Account/CustomerAccountAccountsPage.cshtml");
        }

    }
}
