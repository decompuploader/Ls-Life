using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LSlife
{
  public class PedDebugger
  {
    private List<Ped> debugPeds = new List<Ped>();
    private int dTime;
    private int dInterval = 2000;

    public void OnTick()
    {
      if (Game.GameTime > this.dTime + this.dInterval)
      {
        foreach (Ped ped in ((IEnumerable<Ped>) World.GetAllPeds()).ToList<Ped>())
        {
          if (!this.debugPeds.Contains(ped))
            this.debugPeds.Add(ped);
        }
      }
      foreach (Ped _ped in this.debugPeds.ToList<Ped>())
      {
        if ((Entity) _ped != (Entity) null && _ped.Exists())
        {
          if (_ped.IsOnScreen && _ped.IsVisible)
          {
            bool allowed = false;
            if (!LSL.CheckIfPedUsed(_ped, true, true))
              allowed = true;
            string caption = PedDebugger.SetPedDebugtext((Entity) _ped, allowed);
            Vector2 vector2 = LsFunctions.World3DToScreen2d(new Vector3(_ped.GetBoneCoord(Bone.FB_Brow_Centre_000).X, _ped.GetBoneCoord(Bone.FB_Brow_Centre_000).Y, _ped.GetBoneCoord(Bone.FB_Brow_Centre_000).Z + 0.5f));
            vector2.X = 1280f * vector2.X;
            vector2.Y = 720f * vector2.Y;
            Point position = new Point((int) vector2.X, (int) vector2.Y);
            Color whiteSmoke = Color.WhiteSmoke;
            new UIText(caption, position, 0.2f, whiteSmoke, GTA.Font.ChaletLondon, true, true, true).Draw();
          }
        }
        else
          this.debugPeds.Remove(_ped);
      }
      if (LSL.pStashVehicle == null || !((Entity) LSL.pStashVehicle.drugCar != (Entity) null) || !LSL.pStashVehicle.drugCar.Exists())
        return;
      Vector2 vector2_1 = LsFunctions.World3DToScreen2d(LSL.pStashVehicle.drugCar.Position);
      vector2_1.X = 1280f * vector2_1.X;
      vector2_1.Y = 720f * vector2_1.Y;
      new UIText(PedDebugger.SetPedDebugtext((Entity) LSL.pStashVehicle.drugCar, true), new Point((int) vector2_1.X, (int) vector2_1.Y), 0.2f, Color.WhiteSmoke, GTA.Font.ChaletLondon, true, true, true).Draw();
    }

    private static string SetPedDebugtext(Entity p, bool allowed)
    {
      if (LSL.pStashVehicle != null && p == (Entity) LSL.pStashVehicle.drugCar)
      {
        Vehicle vehicle = p as Vehicle;
        return p.Health.ToString() + " | " + LSL.pStashVehicle.drugCarDestroyed.ToString() + " | " + LSL.pStashVehicle.drugCar.IsDead.ToString() + " | " + vehicle.BodyHealth.ToString() + " | " + vehicle.EngineHealth.ToString() + " | " + vehicle.IsDriveable.ToString();
      }
      if (p is Ped && (Entity) (p as Ped) == (Entity) LSL.player.Character)
      {
        string str = "";
        foreach (LSL.PathnodeFlags roadFlag in LSL.GetRoadFlags(LSL.playerPos))
          str = str + " " + roadFlag.ToString();
        return str;
      }
      if (LSL.DealerHandler.dealers.Count > 0 && LSL.DealerHandler.dealers.Find((Predicate<PlayerDealer>) (d => (Entity) d.Ped == p)) != null)
        return LSL.DealerHandler.dealers.Find((Predicate<PlayerDealer>) (d => (Entity) d.Ped == p)).NewState.ToString() + " > " + LSL.DealerHandler.dealers.Find((Predicate<PlayerDealer>) (d => (Entity) d.Ped == p)).CurrentState.ToString();
      if (LSL.aDealer != null)
      {
        if ((Entity) LSL.aDealer.dPed != (Entity) null && p == (Entity) LSL.aDealer.dPed)
        {
          foreach (Enums.eTaskTypeIndex eTaskTypeIndex in Enum.GetValues(typeof (Enums.eTaskTypeIndex)))
          {
            if (Function.Call<bool>(Hash._0xB0760331C7AA4155, (InputArgument) p, (InputArgument) (int) eTaskTypeIndex))
              return eTaskTypeIndex.ToString();
          }
        }
        else if ((Entity) LSL.aDealer.cPed != (Entity) null && p == (Entity) LSL.aDealer.cPed)
          return LSL.aDealer.dealState.ToString();
      }
      if (LSL.driveByHandler != null && LSL.driveByHandler.GetDriveBies.Count > 0)
      {
        foreach (DriveBy getDriveBy in LSL.driveByHandler.GetDriveBies)
        {
          if (getDriveBy.Peds.Count > 0 && (Entity) getDriveBy.Peds.Find((Predicate<Ped>) (r => (Entity) r == p)) != (Entity) null)
            return getDriveBy.DriveBySate.ToString();
        }
      }
      if (p == (Entity) LSL.drugDealer1)
        return LSL.dealer1Rep.ToString();
      if (LSL.pigs.Count > 0)
      {
        foreach (Pig pig in LSL.pigs)
        {
          if ((Entity) pig.Ped == p)
            return pig.CurrentState.ToString();
        }
      }
      return allowed.ToString();
    }
  }
}
