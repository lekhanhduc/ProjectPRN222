namespace E_Learning.Models.Request
{
    public class UserCreationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Dob { get; set; }

        public UserCreationRequest() { }

        public UserCreationRequest(string Email, string Password, string FirstName, string LastName, DateOnly Dob)
        {
            this.Email = Email;
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.Password = Password;
            this.Dob = Dob;
        }
    }
}
