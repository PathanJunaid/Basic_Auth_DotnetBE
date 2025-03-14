
namespace Basic_Auth.Model.dto
{
    public class Userdto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class Logindto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class Namedto
    {
        public string Name { get; set; }
    }
}
