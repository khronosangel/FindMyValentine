using FindMyValentine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace FindMyValentine.Controllers
{
    public class UsersController : Controller
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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AdminLogin()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Process(AccountsViewModel req)
        {
            bool upsertStatus = false;
            try
            {
                upsertStatus = true;
                using (var db = new MainDBContext())
                {
                    if (req.FormAction == "NEW")
                    {
                        var newAcc = new Account()
                        {
                            Username = req.Username,
                            Password = req.Password,
                            DisplayName = req.DisplayName
                        };

                        var newStud = new Student()
                        {
                            StudentNumber = req.Username,
                            StudentName = "",
                            Gender = "",
                            Level = "",
                            Course = "",
                            Year = "",
                            Section = "",
                            CreatedDate = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow
                        };

                        db.Accounts.Add(newAcc);
                        db.Students.Add(newStud);
                        db.SaveChanges();
                        req.StatusMessage = "Successfully Added Account!";
                    }
                    else if (req.FormAction == "EDIT")
                    {
                        var acc = (from row in db.Accounts where row.Id == req.AccountId select row).FirstOrDefault();
                        Student? stud = null;
                        if (acc != null)
                        {
                            acc.DisplayName = req.DisplayName;
                            if (!string.IsNullOrWhiteSpace(req.Password))
                            {
                                acc.Password = req.Password;
                            }
                            stud = (from row in db.Students where row.StudentNumber == acc.Username select row).FirstOrDefault();
                            if (stud != null)
                            {
                                stud.StudentName = req.StudentName;
                                stud.Gender = req.Gender;
                                stud.Level = req.Level;
                                stud.Course = req.Course;
                                stud.Year = req.Year;
                                stud.Section = req.Section;
                                stud.ModifiedDate = DateTime.UtcNow;
                            }
                            db.SaveChanges();
                            req.StatusMessage = "Successfully updated record!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                req.StatusMessage = ex.Message;
            }
            return RedirectToAction("Index", "Users", new { Success = upsertStatus, req.StatusMessage });
        }

        [HttpGet]
        public AccountsViewModel GetAccount(int AccountId)
        {
            AccountsViewModel resp = new AccountsViewModel();
            using (var db = new MainDBContext())
            {
                var acc = (from row in db.Accounts where row.Id == AccountId select row).FirstOrDefault();
                Student? stud = null;
                if (acc != null)
                {
                    stud = (from row in db.Students where row.StudentNumber == acc.Username select row).FirstOrDefault();
                    if (stud != null)
                    {
                        resp.StudentId = stud.Id;
                        resp.StudentNumber = acc.Username;
                        resp.StudentName = stud.StudentName;
                        resp.Gender = stud.Gender;
                        resp.Level = stud.Level;
                        resp.Course = stud.Course;
                        resp.Year = stud.Year;
                        resp.Section = stud.Section;
                        resp.CreatedDate = stud.CreatedDate;
                        resp.ModifiedDate = stud.ModifiedDate;
                    }
                }

                resp.AccountId = acc.Id;
                resp.DisplayName = acc.DisplayName;
                resp.Username = acc.Username;

            }
            return resp;
        }


        public IActionResult Delete(int AccountId)
        {
            AccountsViewModel resp = new AccountsViewModel();
            using (var db = new MainDBContext())
            {
                var acc = (from row in db.Accounts where row.Id == AccountId select row).FirstOrDefault();
                if (acc != null)
                {
                    db.Accounts.Remove(acc);
                    db.SaveChanges();
                    resp.StatusMessage = "Successfully deleted Account!";
                }
            }
            return RedirectToAction("Index", "Users", new { Success = true, resp.StatusMessage });
        }
    }
}
