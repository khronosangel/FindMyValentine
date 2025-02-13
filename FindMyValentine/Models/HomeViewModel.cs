namespace FindMyValentine.Models
{
    public class HomeViewModel
    {
        public Account? CurrentAccount { get; set; }
        public Student? CurrentStudent { get; set; }
        public Match? MatchDetail { get; set; }
        public string QRCodeData { get; set; }
        public string YourHint { get; set; }
        public string PartnersHint { get; set; }
        public Student? PartnersDetail { get; set; }
        public string MatchMeWith { get; set; }
    }
}
