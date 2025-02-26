namespace E_Learning.Models.Request
{
    public class UserCreationRequest
    {
        public String Email { get; set; }
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateOnly Dob { get; set; }

        public UserCreationRequest() { }

        public UserCreationRequest(String Email, String Password, String FirstName, String LastName, DateOnly Dob)
        {
            this.Email = Email;
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.Password = Password;
            this.Dob = Dob;
        }
    }
}
