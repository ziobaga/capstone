using Capstone.Models;
using Capstone.Models.Auth;

public class HomeViewModel
{
    public Users User { get; set; }
    public List<Matches> PartiteCreate { get; set; }
    public List<Bookings> PartitePartecipate { get; set; }
    public bool ProfiloIncompleto { get; set; }
}
