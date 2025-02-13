namespace FindMyValentine.Models
{
    public class AccountsViewModel
    {
        public string StatusMessage { get; set; }
        public string FormAction { get; set; }
        public List<Account> Accounts { get; set; }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentNumber { get; set; }
        public string Hint { get; set; }
        public string Gender { get; set; }
        public string Level { get; set; }
        public string Course { get; set; }
        public string Year { get; set; }
        public string Section { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
