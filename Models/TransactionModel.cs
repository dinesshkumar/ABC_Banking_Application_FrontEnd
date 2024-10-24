/**
 * CustomerModel.cs
 * Description: This file contains the class and variables of Transactions.
 * Author: Dinessh Kumar
 * Last modified: January 19, 2024
 */

namespace htmlDataTest.Models
{
    public class TransactionModel
    {
        public int AccountId { get; set; }
        public string? Transaction_type { get; set; }
        public float Amount { get; set; }
        public DateTime Transaction_date { get; set; }
        public string? Description { get; set; }
        public float Deposit { get; set; }
        public float Withdraw { get; set; }
        public float Balance { get; set; }
        public string? Transaction_status { get; set; }
        public bool Approval_status { get; set; }
        public string? Account_Type { get; set; }


    }
}
