using FindMyValentine.Attributes;
using FindMyValentine.Models;
using Microsoft.AspNetCore.Mvc;

namespace FindMyValentine.Controllers
{
    public class MatchesController : Controller
    {
        [AdminAuthorizeOnly]
        public IActionResult Index(MatchViewModel req = null)
        {
            MatchViewModel resp = req ?? new MatchViewModel();

            using (var db = new MainDBContext())
            {
                var mats = (from row in db.Matches select row).ToList();
                var allStuds = (from row in db.Students select row).ToList();

                resp.SeniorHighMatches = mats.Where(r=>r.Level == "Senior High").ToList();
                resp.CollegeMatches = mats.Where(r=>r.Level == "College").ToList();

                resp.StudentFemaleCount = (from row in allStuds where row.Gender == "Female" select row).ToList().Count();
                resp.StudentMaleCount = (from row in allStuds where row.Gender == "Male" select row).ToList().Count();
                resp.TotalStudentCount = allStuds.Count();

                var matConfig = (from rw in db.MatchControls where rw.Level == "College" select rw).FirstOrDefault();
                if(matConfig == null)
                {
                    matConfig = db.MatchControls.Add(new MatchControl() {
                        EndOverride = false,
                        IsActive = false,
                        StartDateTime = "",
                        EndtDateTime = "",
                        Level = "College"
                    }).Entity;

                    db.SaveChanges();
                }
                resp.CollegeConfig = matConfig;
                matConfig = (from rw in db.MatchControls where rw.Level == "Senior High" select rw).FirstOrDefault();
                if (matConfig == null)
                {
                    matConfig = db.MatchControls.Add(new MatchControl()
                    {
                        EndOverride = false,
                        IsActive = false,
                        StartDateTime = "",
                        EndtDateTime = "",
                        Level = "Senior High"
                    }).Entity;

                    db.SaveChanges();
                }
                resp.SeniorHighConfig = matConfig;
            }


            return View(resp);
        }

        public IActionResult Process(MatchViewModel req)
        {
            MatchViewModel resp = new MatchViewModel();
            try
            {
                using (var db = new MainDBContext())
                {
                    

                    if(req != null)
                    {
                        if (!string.IsNullOrWhiteSpace(req.MatchAction))
                        {
                            if (req.MatchAction == "POPCOL")
                            {
                                var mats = (from row in db.Matches where row.Level == "College" select row).ToList();
                                if (mats.Count == 0)
                                {
                                    Random rnd = new Random();
                                    var randomCollegeGirls = (from row in db.Students
                                                              where row.Gender == "Female"
                                                              && row.Level == "College"
                                                              select row.StudentNumber).ToList();
                                    var randomCollegeBoys = (from row in db.Students
                                                              where row.Gender == "Male"
                                                              && row.Level == "College"
                                                              select row.StudentNumber).ToList();

                                    randomCollegeGirls = randomCollegeGirls.OrderBy(_ => rnd.Next()).ToList();
                                    randomCollegeBoys = randomCollegeBoys.OrderBy(_ => rnd.Next()).ToList();

                                    if(randomCollegeGirls.Count == randomCollegeBoys.Count)
                                    {
                                        for (var ctr = 0; ctr < randomCollegeBoys.Count; ctr++)
                                        {
                                            db.Matches.Add(new Match()
                                            {
                                                Level = "College",
                                                Matched = false,
                                                CreatedDate = DateTime.UtcNow,
                                                ModifiedDate = DateTime.UtcNow,
                                                StudentNumberMale = randomCollegeBoys[ctr],
                                                StudentNumberFemale = randomCollegeGirls[ctr],
                                                FemaleHint = "",
                                                MaleHint = ""
                                            });
                                        }

                                        db.SaveChanges();
                                    }
                                }
                            }

                            if (req.MatchAction == "POPSEN")
                            {
                                var mats = (from row in db.Matches where row.Level == "Senior High" select row).ToList();
                                if (mats.Count == 0)
                                {
                                    Random rnd = new Random();
                                    var randomSenHighGirls = (from row in db.Students
                                                              where row.Gender == "Female"
                                                              && row.Level == "Senior High"
                                                              select row.StudentNumber).ToList();
                                    var randomSenHighBoys = (from row in db.Students
                                                             where row.Gender == "Male"
                                                             && row.Level == "Senior High"
                                                             select row.StudentNumber).ToList();

                                    randomSenHighGirls = randomSenHighGirls.OrderBy(_ => rnd.Next()).ToList();
                                    randomSenHighBoys = randomSenHighBoys.OrderBy(_ => rnd.Next()).ToList();

                                    if (randomSenHighGirls.Count == randomSenHighBoys.Count)
                                    {
                                        for (var ctr = 0; ctr < randomSenHighBoys.Count; ctr++)
                                        {
                                            db.Matches.Add(new Match()
                                            {
                                                Level = "Senior High",
                                                Matched = false,
                                                CreatedDate = DateTime.UtcNow,
                                                ModifiedDate = DateTime.UtcNow,
                                                StudentNumberMale = randomSenHighBoys[ctr],
                                                StudentNumberFemale = randomSenHighGirls[ctr],
                                                FemaleHint ="",
                                                MaleHint=""
                                            });
                                        }

                                        db.SaveChanges();
                                    }
                                }
                            }

                            if (req.MatchAction == "STARTCOL")
                            {
                                var matConfig = (from rw in db.MatchControls where rw.Level == "College" select rw).FirstOrDefault();
                                if (matConfig != null)
                                {
                                    matConfig.EndOverride = false;
                                    matConfig.IsActive = true;
                                    matConfig.StartDateTime = DateTime.UtcNow.ToString();
                                    matConfig.EndtDateTime = DateTime.UtcNow.AddHours(8).ToString();
                                    db.SaveChanges();
                                }
                            }

                            if (req.MatchAction == "STARTSEN")
                            {
                                var matConfig = (from rw in db.MatchControls where rw.Level == "Senior High" select rw).FirstOrDefault();
                                if (matConfig != null)
                                {
                                    matConfig.EndOverride = false;
                                    matConfig.IsActive = true;
                                    matConfig.StartDateTime = DateTime.UtcNow.ToString();
                                    matConfig.EndtDateTime = DateTime.UtcNow.AddHours(8).ToString();
                                    db.SaveChanges();
                                }
                            }

                            if (req.MatchAction == "ENDCOL")
                            {
                                var matConfig = (from rw in db.MatchControls where rw.Level == "College" select rw).FirstOrDefault();
                                if (matConfig != null)
                                {
                                    matConfig.EndOverride = true;
                                    matConfig.IsActive = false;
                                    db.SaveChanges();
                                }
                            }

                            if (req.MatchAction == "ENDSEN")
                            {
                                var matConfig = (from rw in db.MatchControls where rw.Level == "Senior High" select rw).FirstOrDefault();
                                if (matConfig != null)
                                {
                                    matConfig.EndOverride = true;
                                    matConfig.IsActive = false;
                                    db.SaveChanges();
                                }
                            }

                            if (req.MatchAction == "CLEARCOL")
                            {
                                var mats = (from row in db.Matches where row.Level == "College" select row).ToList();
                                db.Matches.RemoveRange(mats);
                                db.SaveChanges();
                            }

                            if (req.MatchAction == "CLEARSEN")
                            {
                                var mats = (from row in db.Matches where row.Level == "Senior High" select row).ToList();
                                db.Matches.RemoveRange(mats);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                req.SuccessMessage = $"Action:<b>{req.MatchAction}</b> done successfully";
                return RedirectToAction("Index", "Matches", new { req.SuccessMessage });
            }
            catch(Exception ex)
            {
                req.ErrorMessage = ex.Message;
                return RedirectToAction("Index", "Matches", new { req.ErrorMessage });
            }
        }
    }
}
