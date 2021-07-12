using GTA;
using System.Collections.Generic;

namespace LSlife
{
  public class PedTeam
  {
    private PedGroup DefenceTeam = new PedGroup();
    private List<PlayerDealer> Peds = new List<PlayerDealer>();
    private Vehicle Vehicle;
    private int MaxTeamSize = 1;

    public string Area { get; private set; } = "";

    public PedTeam(PlayerDealer _dealer, string _area)
    {
      this.DefenceTeam.Add(_dealer.Ped, true);
      this.Peds.Add(_dealer);
      this.Area = _area;
    }

    public void AddMember(PlayerDealer _dealer)
    {
      if (this.Peds.Count < this.MaxTeamSize)
      {
        this.DefenceTeam.Add(_dealer.Ped, false);
        this.Peds.Add(_dealer);
      }
      else
        LsFunctions.SendTextMsg("Max Team size reached.");
    }
  }
}
