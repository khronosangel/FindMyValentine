using FindMyValentine.Models;
using Microsoft.AspNetCore.Mvc;

namespace FindMyValentine.Controllers
{
    public class MatchesController : Controller
    {
        public IActionResult Index()
        {
            AccountsViewModel resp = new AccountsViewModel();

            using (var db = new MainDBContext())
            {
                var accs = (from row in db.Accounts select row).ToList();

                resp.Accounts = accs;
            }

            return View(resp);
        }
    }
}
