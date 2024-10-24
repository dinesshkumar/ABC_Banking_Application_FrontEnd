/**
 * CustomerModel.cs
 * Description: This file contains the class and variables of Loan.
 * Author: Dinessh Kumar
 * Last modified: January 19, 2024
 */

namespace htmlDataTest.Models
{
    public class LoanModel
    {
        public int loan_id { get; set; }
        public string? customer_id { get; set; }
        public string? loan_type { get; set; }
        public string? repayment_duration { get; set; }
        public float amount { get; set; }
        public string? comments { get; set; }
        public DateTime request_date { get; set; }

    }
}
