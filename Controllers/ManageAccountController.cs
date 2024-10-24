/**
 * ManageAccountController.cs
 * Description: This file contains the main logic for sending money to bank account.
 * Author: Dinessh Kumar
 * Last modified: January 19, 2024
 */


using Microsoft.AspNetCore.Mvc;

namespace htmlDataTest.Controllers
{
    public class ManageAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SendMoney()
        {
            return View();
        }
    }
}
