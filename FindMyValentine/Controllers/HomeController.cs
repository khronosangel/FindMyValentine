using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using FindMyValentine.Attributes;
using FindMyValentine.Models;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace FindMyValentine.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [StudentAuthorizeOnly]
        public IActionResult Index(string MatchMeWith = null)
        {
            HomeViewModel resp = new HomeViewModel();
            using (var db = new MainDBContext())
            {
                resp.CurrentAccount = (Account)ViewData["User"] ?? null;
                if (resp.CurrentAccount != null)
                {
                    resp.MatchMeWith = MatchMeWith;
                    resp.CurrentStudent = (from row in db.Students
                                           where row.StudentNumber == resp.CurrentAccount.Username
                                           select row).FirstOrDefault();
                    resp.MatchDetail = (from row in db.Matches
                                        where 
                                        row.StudentNumberFemale == resp.CurrentAccount.Username
                                        ||
                                        row.StudentNumberMale == resp.CurrentAccount.Username
                                        select row).FirstOrDefault();

                    if(resp.MatchDetail== null)
                    {
                        return View("NotReady", resp);
                    }

                    if (resp.CurrentStudent != null)
                    {
                        resp.YourHint = resp.CurrentStudent.Hint;
                    }

                    if (resp.MatchDetail != null)
                    {
                        //get opposite hint
                        string oppoSNO = (resp.CurrentStudent.Gender == "Male") ? resp.MatchDetail.StudentNumberFemale : resp.MatchDetail.StudentNumberMale;
                        resp.PartnersDetail = (from row in db.Students
                                               where row.StudentNumber == oppoSNO
                                        select row).FirstOrDefault();
                        resp.PartnersHint = resp.PartnersDetail.Hint;

                        
                        //update match status
                        if (!string.IsNullOrWhiteSpace(MatchMeWith))
                        {
                            if (MatchMeWith == resp.PartnersDetail.StudentNumber)
                            {
                                var matchup = (from row in db.Matches
                                               where
                                               row.StudentNumberFemale == resp.CurrentAccount.Username
                                               ||
                                               row.StudentNumberMale == resp.CurrentAccount.Username
                                               select row).FirstOrDefault();

                                matchup.Matched = true;
                                db.SaveChanges();
                            }
                        }
                    }




                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    //using (QRCodeData qrCodeData = qrGenerator.CreateQrCode($"http://angels7890-001-site1.ktempurl.com/Home/Index?MatchMeWith={resp.CurrentAccount.Username}", QRCodeGenerator.ECCLevel.Q))
                    using (QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://findmyvalentine.azurewebsites.net/Home/Index?MatchMeWith={resp.CurrentAccount.Username}", QRCodeGenerator.ECCLevel.Q))
                    using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                    //using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        //Bitmap icon = new Bitmap("/wwwroot/imgs/favicon.png");
                        //Bitmap qrCodeImageLogo = qrCode.GetGraphic(20, Color.Black, Color.White, icon);
                        //using (MemoryStream ms = new MemoryStream())
                        //{
                        //    qrCodeImageLogo.Save(ms, ImageFormat.Png);
                        //    string base64String = Convert.ToBase64String(ms.ToArray());
                        //    resp.QRCodeData = "data:image/png;base64," + base64String;
                        //}
                        byte[] qrCodeImage = qrCode.GetGraphic(20);
                        string base64 = Convert.ToBase64String(qrCodeImage);
                        resp.QRCodeData = base64;
                    }
                }
                
            }
            return View(resp);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
