sing GTA;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSlife
{
  public class DriveByHandler
  {
    public List<DriveBy> GetDriveBies = new List<DriveBy>();

    public void OnTick()
    {
      if (this.GetDriveBies.Count <= 0)
        return;
      foreach (DriveBy driveBy in this.GetDriveBies.ToList<DriveBy>())
      {
        if (driveBy.Dead)
          this.GetDriveBies.Remove(driveBy);
        else
          driveBy.OnTick();
      }
    }

    public void AddDriveBy(VehicleClass _class, int _amount, Ped _target, bool _agres) => this.GetDriveBies.Add(new DriveBy(_class, _amount, _target, _agres));

    public int AmountOfDriveBy(bool _agg) => _agg ? this.GetDriveBies.Where<DriveBy>((Func<DriveBy, bool>) (r => r.Agressive)).Count<DriveBy>() : this.GetDriveBies.Count;

    public List<Ped> GetAllPeds()
    {
      Ped[] pedArray = new Ped[0];
      if (this.GetDriveBies.Count > 0)
      {
        foreach (DriveBy getDriveBy in this.GetDriveBies)
        {
          foreach (Ped element in getDriveBy.Peds.Where<Ped>((Func<Ped, bool>) (r => (Entity) r != (Entity) null && r.Exists())))
            ((IEnumerable<Ped>) pedArray).Append<Ped>(element);
        }
      }
      return ((IEnumerable<Ped>) pedArray).Count<Ped>() > 0 ? ((IEnumerable<Ped>) pedArray).ToList<Ped>() : (List<Ped>) null;
    }

    public void CleanDriveBys()
    {
      foreach (DriveBy driveBy in this.GetDriveBies.ToList<DriveBy>())
      {
        driveBy.CleanDriveBy();
        this.GetDriveBies.Remove(driveBy);
      }
    }

    internal void SetAgressiveTarget(bool _agg, Ped _ped)
    {
      foreach (DriveBy getDriveBy in this.GetDriveBies)
      {
        if (!getDriveBy.Agressive)
        {
          getDriveBy.Agressive = true;
          getDriveBy.ChangeBlip(true);
          getDriveBy.SetNewTarget(_ped);
        }
      }
    }
  }
}
