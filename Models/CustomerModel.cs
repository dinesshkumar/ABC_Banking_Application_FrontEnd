/**
 * CustomerModel.cs
 * Description: This file contains the class and variables of Customer.
 * Author: Dinessh Kumar
 * Last modified: January 19, 2024
 */

namespace htmlDataTest.Models
{
    public class CustomerModel
    {
        public string? customer_id { get; set; }
        public string? first_name { get; set; }

        public string? last_name { get; set; }

        public string? address { get; set; }
        public string? email { get; set; }
        public string? phone_number { get; set; }
        public int active_status { get; set; }
        public int branch_code { get; set; }


        public string? branch_name { get; set; }



    }
}
