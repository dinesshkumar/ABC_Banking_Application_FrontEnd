/**
 * FundTransferController.cs
 * Description: This file contains the main logic for the displaying Loan Details and Customer Details from database.
 * Author: Dinessh Kumar
 * Last modified: January 19, 2024
 */


using Microsoft.AspNetCore.Mvc;
using htmlDataTest.Models;
using Microsoft.Data.SqlClient;



namespace htmlDataTest.Controllers
{
    public class FundTransferController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LoanDetails()
        {
            String connectionString = "Data Source=(localdb)\\dinessh_local;Initial Catalog=ABC_database;Integrated Security=True";
            ///String sql = "SELECT * FROM accounts";
            ///String sql = "SELECT[customer_id] FROM[ABC_database].[dbo].[user_table] WHERE[USERNAME] = 'dinesshapp@gmail.com'";
            ///String sql = "SELECT[customer_id] FROM user_table WHERE USERNAME = 'dinesshapp@gmail.com'";


            String sql = "select * from loan_table where active_status =2 order by loan_id DESC;";

            var listLoan = new List<LoanModel>();
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var loan = new LoanModel
                    {
                        loan_id= (int)(rdr["loan_id"]),
                        customer_id = (string)(rdr["customer_id"]),
                        loan_type = (string)rdr["loan_type"],
                        repayment_duration = (string)rdr["repayment_duration"],
                        amount =Convert.ToSingle(rdr["amount"]),
                        comments = (string)rdr["comments"],
                        request_date = (DateTime)rdr["request_date"]

                    };


                    ///student.accountId = (int)rdr["account_id"];
                    ///student.accountNumber = (string)rdr["account_number"];
                    ///student.accountType = (string)rdr["account_type"];

                    listLoan.Add(loan);
                    
                }
                //listLoan.Reverse();
                ViewBag.loanDetails = listLoan;
                conn.Close();

            }
            return View("~/Views/Employee/BankEmployerLoanRequest.cshtml");


        }
        public IActionResult CustomerDetails()
        {
            String connectionString = "Data Source=(localdb)\\dinessh_local;Initial Catalog=ABC_database;Integrated Security=True";
            ///String sql = "SELECT * FROM accounts";
            ///String sql = "SELECT[customer_id] FROM[ABC_database].[dbo].[user_table] WHERE[USERNAME] = 'dinesshapp@gmail.com'";
            ///String sql = "SELECT[customer_id] FROM user_table WHERE USERNAME = 'dinesshapp@gmail.com'";


            String sql = "select * from customer";

            var listCustomer = new List<CustomerModel>();
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var customer = new CustomerModel
                    {
                        customer_id = (string)(rdr["customer_id"]),
                        first_name = (string)(rdr["first_name"]),
                        last_name = (string)rdr["last_name"],
                        address = (string)(rdr["address"]),
                        email = (string)(rdr["email"]),
                        phone_number = (string)(rdr["phone_number"]),

                        //amount = Convert.ToSingle(rdr["amount"]),
                        branch_code = (int)(rdr["branch_id"]),
                        //active_status = Convert.ToSingle(rdr["active_status"]),
                        branch_name = (string)(rdr["branch_name"])

                    };


                    ///student.accountId = (int)rdr["account_id"];
                    ///student.accountNumber = (string)rdr["account_number"];
                    ///student.accountType = (string)rdr["account_type"];

                    listCustomer.Add(customer);

                }
                //listLoan.Reverse();
                ViewBag.listCustomer = listCustomer;
                conn.Close();

            }
            return View("~/Views/Employee/BankEmployerCustomerDetails.cshtml");


        }
    }
}
