namespace BarcodeService.Application.ApplicationUserContext
{
    public class CurrentUser
    {
        public CurrentUser(string id, string login)
        {
            Id = id;
            Login = login;
        }

        public string Id { get; set; }
        public string Login { get; set; }
    }
}