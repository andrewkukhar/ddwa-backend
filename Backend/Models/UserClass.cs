namespace Backend.Models
{
  public class User
  {
    public User()
    {
      Email = "";
      Password = "";
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }
}
