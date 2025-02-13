namespace FindMyValentine.Models
{
    public class MatchViewModel
    {
        public int TotalStudentCount { get; set; }
        public int StudentMaleCount { get; set; }
        public int StudentFemaleCount { get; set; }
        public string MatchAction { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public MatchControl CollegeConfig { get; set; }
        public MatchControl SeniorHighConfig { get; set; }
        public List<Match> CollegeMatches { get; set; }
        public List<Match> SeniorHighMatches { get; set; }
    }
}
