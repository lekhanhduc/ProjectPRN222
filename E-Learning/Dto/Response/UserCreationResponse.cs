namespace E_Learning.Models.Response
{
    public class UserCreationResponse
    {
        public String Email { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateOnly? Dob { get; set; }
        public string UserType { get; set; }

        public UserCreationResponse() { }

        public UserCreationResponse(String Email, String FirstName, String LastName, DateOnly Dob, string UserType)
        {
            this.Email = Email;
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.Dob = Dob;
            this.UserType = UserType;
        }
    }
}
