namespace SignupLogin.API.Models
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string reg_date { get; set; }
        public string lan_id { get; set; }
        public string? token { get; set; }
        /// <summary>
        /// 1 = yes, 2 = no
        /// </summary>
        public int on_notification { get; set; }

        public User()
        {
            
        }

        public User(User other)
        {
            Copy(other);
        }

        public void Copy(User other)
        {
            username = other.username;
            password = other.password;
            reg_date = other.reg_date;
            lan_id = other.lan_id;
            on_notification = other.on_notification;
        }
    }
}
