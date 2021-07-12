using GTA;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSlife
{
  public class LsLPeds
  {
    private int ptime;
    private int pdelay = 7000;

    public List<Ped> GetPeds { get; private set; }

    public LsLPeds()
    {
      this.GetPeds = ((IEnumerable<Ped>) World.GetNearbyPeds(Game.Player.Character, 200f)).Where<Ped>((Func<Ped, bool>) (p => !LSL.CheckIfPedUsed(p, true, true))).ToList<Ped>();
      this.ptime = Game.GameTime;
    }

    public void Ontick()
    {
      if (Game.GameTime > this.ptime + this.pdelay)
      {
        this.ptime = Game.GameTime;
        foreach (Ped _ped in new List<Ped>((IEnumerable<Ped>) World.GetNearbyPeds(Game.Player.Character, 200f)))
        {
          if (!LSL.CheckIfPedUsed(_ped, true, true) && !this.GetPeds.Contains(_ped))
            this.GetPeds.Add(_ped);
        }
      }
      foreach (Ped _ped in this.GetPeds.ToList<Ped>())
      {
        if ((Entity) _ped == (Entity) null || !_ped.Exists() || ((double) LSL.playerPos.DistanceTo(_ped.Position) > 200.0 || LSL.CheckIfPedUsed(_ped, true, true)))
          this.GetPeds.Remove(_ped);
      }
    }
  }
}
