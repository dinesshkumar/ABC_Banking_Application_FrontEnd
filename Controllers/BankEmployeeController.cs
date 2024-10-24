/**
 * BankEmployeeController.cs
 * Description: This file contains the main logic for the Employee Login application and navigation of Customer Account Page.
 * Author: Dinessh Kumar
 * Last modified: January 19, 2024
 */

using htmlDataTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ABCBankingApplication.Controllers
{
    public class BankEmployeeController : Controller
    {
        public static class Config
        {
            public static string? Username;
            public static string? EmployeeID;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EmployeeSubmit(string userEmail, string userPassword)
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

            string employee_id = "";
            String connectionString = "Data Source=(localdb)\\dinessh_local;Initial Catalog=ABC_database;Integrated Security=True";
            String sql = "SELECT employee_id FROM user_table WHERE email ='" + userEmail + "' and password='" + hashedPassword + "' and role='Employee'";

            //var model = new List<AccountsModel>();
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();


                if (rdr.Read())
                {
                    employee_id = rdr[0].ToString();
                    ViewBag.EmployeeID = employee_id;
                    Config.EmployeeID = employee_id;

                    ViewBag.customer_id = rdr[0].ToString();
                    using (SqlConnection conn2 = new(connectionString))
                    {
                        sql = "select first_name from employee where employee_id = '" + employee_id + "'";
                        cmd = new SqlCommand(sql, conn2);
                        conn2.Open();
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            ViewBag.userName = rdr[0].ToString();
                                

                        }
                        conn2.Close();
                    }

                }
                conn.Close();
            }

            if (employee_id == "")
            {
                return View("~/Views/Login/CustomerLoginPageFailed.cshtml");
            }
            return View("~/Views/Employee/BankEmployerAccountHomePage.cshtml");

        }
        public IActionResult BankEmployerAccountHomePage()
        {
            ViewBag.EmployeeID = Config.EmployeeID;

            return View("~/Views/Employee/BankEmployerAccountHomePage.cshtml");
        }
        public IActionResult BankEmployerAccountPage()
        {
            ViewBag.EmployeeID = Config.EmployeeID;


            return View("~/Views/Employee/BankEmployerAccountPage.cshtml");
        }
        public IActionResult BankEmployerCustomerPage()
        {
            ViewBag.EmployeeID = Config.EmployeeID;


            return View("~/Views/Employee/BankEmployerCustomerPage.cshtml");
        }
        public IActionResult BankEmployerFundTransferPage()
        {
            ViewBag.EmployeeID = Config.EmployeeID;


            return View("~/Views/Employee/BankEmployerFundTransferPage.cshtml");
        }
        public IActionResult BankEmployerPendingApprovalsPage()
        {
            ViewBag.EmployeeID = Config.EmployeeID;


            return View("~/Views/Employee/BankEmployerPendingApprovalsPage.cshtml");
        }
        public IActionResult BankEmployerManageProfilePage()
        {
            ViewBag.EmployeeID = Config.EmployeeID;


            return View("~/Views/Employee/BankEmployerManageProfilePage.cshtml");
        }
    }
}
