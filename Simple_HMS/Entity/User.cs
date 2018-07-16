using Simple_HMS.Interface;

namespace Simple_HMS.Entity
{
    public class User : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}