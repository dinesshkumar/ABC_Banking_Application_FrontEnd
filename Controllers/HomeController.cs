/**
 * HomeController.cs
 * Description: This file contains the main logic for the Account Registration and navigation of Pre Login page.
 * Author: Dinessh Kumar
 * Last modified: January 19, 2024
 */

using htmlDataTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Org.BouncyCastle.Security;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Security.Cryptography;
using System.Security.Policy;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt;



namespace ABCBankingApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CustomerLogin()
        {
            return View("~/Views/Login/CustomerLoginPage.cshtml");
        }
        public IActionResult EmployeeLogin()
        {
            return View("~/Views/Login/BankEmployerLoginPage.cshtml");
        }
        public IActionResult Goback()
        {
            return View("~/Views/Home/Index.cshtml");
        }
        public IActionResult CustomerRegister()
        {
            return View("~/Views/Login/CustomerRegisterPage.cshtml");
        }
        public IActionResult CreateCustomerRegister(string firstName, string lastname, string userAddress, string location,string userEmail, string userPhoneNumber, string userPassword)


        {
            //Generating CustomerID
            string customer_id = DateTime.Now.ToString("yyyyMMddhhmmssff");
            long account_number;

            int branch_id = 0;
            string strAccount_number = "";

            //Generating Password
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

            //Generating Branch ID
            switch (location)
            {
                case "London":
                    branch_id = 2004;
                    break;
                case "Birmingham":
                    branch_id = 2003;
                    break;
                case "Leeds":
                    branch_id = 2002;
                    break;
                case "Manchester":
                    branch_id = 2001;
                    break;

            }

            //Getting Previous Account Number
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

            //Increasing Account Number
            account_number++;

            //Inserting Customer Table
            string insertCustomerTable_sql= "INSERT INTO CUSTOMER (customer_id, first_name, last_name, address, email, phone_number, branch_id, active_status, branch_name)" +
                                                          "VALUES ('"+customer_id+"','"+firstName+"','"+lastname+"','"+userAddress+"','"+userEmail+"','"+userPhoneNumber+"','"+branch_id+"',1,'"+location+"')";
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(insertCustomerTable_sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                conn.Close();
            }

            //Inserting Accounts Table
            string insertAccountsTable_sql = "INSERT INTO accounts (customer_id, account_number, account_type, account_code, balance, open_Date, active_status, approve_status)" +
                                             "VALUES ('"+customer_id+"','"+account_number+"','Savings',1001,0,CURRENT_TIMESTAMP, 1,'Approved')";
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(insertAccountsTable_sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                conn.Close();
            }

            //Inserting User Table
            string insertUserTable_sql = "INSERT INTO user_table (email, password, customer_id, role, active_status) values ('"+userEmail+"','"+ hashedPassword + "','"+customer_id+"','Customer',1)";
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(insertUserTable_sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                conn.Close();
            }

            return View("~/Views/Login/CustomerLoginPage.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
