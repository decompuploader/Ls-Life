using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSlife
{
  public class Area
  {
    public string Name;
    public List<Drug> Drugs = new List<Drug>();
    public LSL.AreaType AreaType = LSL.AreaType.Normal;
    public LSL.jurisdictionType PoliceType;
    public int CopPresance;
    public int GangPresance;
    public float Reputation;
    public int Heat;
    public bool DealerKilled;
    public int DemandBuffTime;
    public int DemandBuffLength;

    public Area(string _name, List<Drug> _drugs, LSL.AreaType _type) => this.Innit(_name, _drugs, _type);

    public Area(
      string _name,
      List<Drug> _drugs,
      LSL.AreaType _type,
      int _cStr,
      int _gStr,
      int _heat,
      float _rep)
    {
      this.Innit(_name, _drugs, _type);
      this.CopPresance = _cStr;
      this.GangPresance = _gStr;
      this.Heat = _heat;
      this.Reputation = _rep;
    }

    private void Innit(string _name, List<Drug> _drugs, LSL.AreaType _type)
    {
      this.Name = _name;
      this.Drugs = _drugs;
      this.AreaType = _type;
      this.PoliceType = LSL.zoneData[this.Name].Item2;
      switch (this.AreaType)
      {
        case LSL.AreaType.Poor:
          this.CopPresance = 1;
          this.GangPresance = 6;
          break;
        case LSL.AreaType.Normal:
          this.CopPresance = 1;
          this.GangPresance = 5;
          break;
        case LSL.AreaType.Rich:
          this.CopPresance = 1;
          this.GangPresance = 5;
          break;
      }
      foreach (Drug drug in this.Drugs)
      {
        switch (this.AreaType)
        {
          case LSL.AreaType.Poor:
            if (drug.AreaType == LSL.AreaType.Poor)
              drug.Demand = 1f + this.AddRandomAmount();
            if (drug.AreaType == LSL.AreaType.Normal)
              drug.Demand = 0.8f + this.AddRandomAmount();
            if (drug.AreaType == LSL.AreaType.Rich)
              drug.Demand = 0.5f + this.AddRandomAmount();
            drug.Delay = 10000f;
            continue;
          case LSL.AreaType.Normal:
            if (drug.AreaType == LSL.AreaType.Poor)
              drug.Demand = 0.7f + this.AddRandomAmount();
            if (drug.AreaType == LSL.AreaType.Normal)
              drug.Demand = 1f + this.AddRandomAmount();
            if (drug.AreaType == LSL.AreaType.Rich)
              drug.Demand = 0.7f + this.AddRandomAmount();
            drug.Delay = 10000f;
            continue;
          case LSL.AreaType.Rich:
            if (drug.AreaType == LSL.AreaType.Poor)
              drug.Demand = 0.5f + this.AddRandomAmount();
            if (drug.AreaType == LSL.AreaType.Normal)
              drug.Demand = 0.8f + this.AddRandomAmount();
            if (drug.AreaType == LSL.AreaType.Rich)
              drug.Demand = 1f + this.AddRandomAmount();
            drug.Delay = 10000f;
            continue;
          default:
            continue;
        }
      }
      this.Drugs = this.Drugs.OrderByDescending<Drug, float>((Func<Drug, float>) (d => d.Demand)).ToList<Drug>();
    }

    public bool NeedCustomer()
    {
      bool flag = false;
      foreach (Drug drug in this.Drugs)
      {
        if ((double) Game.GameTime > (double) drug.TimeOfLastCustomer + (double) drug.Delay / ((double) drug.Demand / (double) this.Dealers()))
        {
          drug.SelectCustomer = true;
          flag = true;
        }
      }
      return flag;
    }

    internal int Dealers()
    {
      int num = 0 + LSL.DealerHandler.dealers.Where<PlayerDealer>((Func<PlayerDealer, bool>) (d => d.Working && d.area == this.Name)).Count<PlayerDealer>();
      if (this.Name == LSL.areas[LSL.areaIndex].Name)
      {
        if (LSL.isDealing)
          ++num;
        if (LSL.aDealer != null)
          ++num;
      }
      else if (this.GangPresance > 0)
        ++num;
      return num;
    }

    private float AddRandomAmount() => Function.Call<float>(Hash._0x313CE5879CEB6FCD, (InputArgument) 0.0f, (InputArgument) 0.25f);

    public void GetCustomer(List<Ped> peds)
    {
      if (this.Drugs.Where<Drug>((Func<Drug, bool>) (d => d.SelectCustomer)).Count<Drug>() <= 0 || peds.Count <= 0)
        return;
      foreach (Drug drug in this.Drugs.Where<Drug>((Func<Drug, bool>) (d => d.SelectCustomer)))
      {
        Random random = new Random();
        int num = (int) (10.0 * (double) drug.Demand);
        if ((double) drug.Demand > 1.0)
          num = 10;
        if (random.Next(0, 11 - num) == 0)
        {
          int count = peds.Count;
          int index = random.Next(0, count);
          if (count == 1)
            index = 0;
          drug.Customer = peds[index];
          peds.Remove(drug.Customer);
          drug.TimeOfLastCustomer = Game.GameTime;
          if (peds.Count == 0)
            break;
        }
      }
    }

    public void HeatDecay()
    {
      if (this.Heat - 40 < 0)
        this.Heat = 0;
      else
        this.Heat -= 40;
      if (this.CopPresance <= 0)
        return;
      --this.CopPresance;
    }

    public void KillDealer()
    {
      this.DealerKilled = true;
      --this.GangPresance;
      this.DemandBuffTime = Game.GameTime;
      this.DemandBuffLength = 60000 * (10 - this.GangPresance);
    }

    public bool CanSpawnDealer()
    {
      if (!this.DealerKilled)
        return true;
      if (Game.GameTime <= this.DemandBuff())
        return false;
      this.DealerKilled = false;
      return true;
    }

    public int DemandBuff() => this.DemandBuffTime + this.DemandBuffLength;

    internal bool DemandMet()
    {
      foreach (Drug drug in this.Drugs)
      {
        if (!drug.Supplied)
          return false;
      }
      return true;
    }
  }
}
