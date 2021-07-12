using GTA;
using GTA.Math;
using System.Collections.Generic;

namespace LSlife
{
  public class Customer
  {
    private Ped ped;
    private Vector3 target;
    private Vehicle vehicle;
    public List<string> drugs = new List<string>();
    public Drug Drug;
    public int amount;
    private bool farAway;
    private System.Random rnd = new System.Random();
    internal int oldRelationShip;

    public Customer(
      Ped ped,
      Vector3 target,
      bool farAway,
      Vehicle vehicle,
      Drug drug,
      int _playerLevel)
    {
      this.FarAway = farAway;
      this.Ped = ped;
      this.Target = target;
      this.Vehicle = vehicle;
      this.Drug = drug;
      LsFunctions.AddBlip((Entity) ped, BlipColor.Blue, nameof (Customer), true, true);
      ped.CurrentBlip.Scale = 0.5f;
      ped.IsPersistent = true;
      ped.Task.ClearAll();
      ped.BlockPermanentEvents = true;
      if (this.FarAway)
      {
        this.amount = LsFunctions.GetLargeStreetAmount();
      }
      else
      {
        int maxValue = 7 + (int) (LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation) * 0.5);
        if (maxValue <= 1)
          maxValue = 7;
        this.amount = this.rnd.Next(1, maxValue);
      }
      this.Ped.Money = this.amount * this.Drug.PricePerG + this.rnd.Next(0, 500);
      if (this.FarAway || !((Entity) vehicle == (Entity) null))
        return;
      this.Ped.Task.GoTo(target);
    }

    public Vector3 Target
    {
      get => this.target;
      set => this.target = value;
    }

    public Ped Ped
    {
      get => this.ped;
      set => this.ped = value;
    }

    public Vehicle Vehicle
    {
      get => this.vehicle;
      set => this.vehicle = value;
    }

    public bool FarAway
    {
      get => this.farAway;
      set => this.farAway = value;
    }
  }
}
