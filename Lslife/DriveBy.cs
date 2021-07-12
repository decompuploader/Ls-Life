using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSlife
{
  public class DriveBy
  {
    public List<Ped> Peds = new List<Ped>();
    private Vehicle Vehicle;
    private VehicleHash vehicleHash;
    private VehicleClass vehicleClass;
    private int pedamount;
    private bool enterState;
    public bool Dead;
    private PedGroup Group = new PedGroup();
    internal Ped Leader;
    internal bool Agressive;
    internal int timeSeen;
    private Vector3 LastKnownPosition;
    private Ped Target;
    private int time;
    private Vector3 lastSpawn;

    public Enums.DriveBySates DriveBySate { get; private set; }

    public Vector3 DefendPosition { get; private set; }

    public DriveBy(VehicleClass _class, int _pedAmount, Ped _target, bool _agressive)
    {
      this.DefendPosition = LSL.playerPos;
      this.LastKnownPosition = LSL.playerPos;
      this.vehicleClass = _class;
      this.pedamount = _pedAmount;
      this.Target = _target;
      this.Agressive = _agressive;
      this.SetVehicleClass();
      this.SpawnVehicle();
      this.SpawnPeds();
      int count = this.Peds.Count;
    }

    public void OnTick()
    {
      this.CheckPeds();
      this.CheckVehicle();
      this.DoStuff();
    }

    internal void SetNewTarget(Ped _ped)
    {
      this.Target = _ped;
      this.LastKnownPosition = _ped.Position;
      this.DefendPosition = this.LastKnownPosition;
    }

    internal void ChangeBlip(bool _agg)
    {
      if (_agg)
      {
        if (this.Agressive)
          return;
        this.Vehicle.CurrentBlip.Color = BlipColor.Red;
        this.Vehicle.CurrentBlip.Name = "Driveby Vehicle";
      }
      else
      {
        if (!this.Agressive)
          return;
        this.Vehicle.CurrentBlip.Color = BlipColor.Pink;
        this.Vehicle.CurrentBlip.Name = "Patrol";
      }
    }

    private void DoStuff()
    {
      Vector3 position;
      if (this.Dead || !LSL.playerDead)
      {
        if ((Entity) this.Target != (Entity) null)
        {
          Ped target = this.Target;
          int num;
          if (target == null)
          {
            num = 0;
          }
          else
          {
            position = target.Position;
            num = (double) position.DistanceTo(this.DefendPosition) > 500.0 ? 1 : 0;
          }
          if (num == 0)
            goto label_7;
        }
        else
          goto label_7;
      }
      this.DriveBySate = Enums.DriveBySates.CleanDriveBy;
      this.enterState = true;
label_7:
      if ((Entity) this.Leader != (Entity) null && this.Leader.Exists() && ((Entity) this.Vehicle != (Entity) null && this.Vehicle.Exists()))
      {
        if (this.Agressive)
        {
          this.CheckOtherDrivebys();
          switch (this.DriveBySate)
          {
            case Enums.DriveBySates.VehDrive:
              if (this.enterState)
              {
                this.enterState = false;
                if ((Entity) this.Leader != (Entity) null && this.Leader.Exists() && ((Entity) this.Vehicle != (Entity) null && this.Vehicle.Exists()))
                  this.Leader.Task.DriveTo(this.Vehicle, this.LastKnownPosition, 10f, 17f, 6);
              }
              bool flag = false;
              foreach (Ped ped in this.Peds)
              {
                if (LsFunctions.CanSee(ped, this.Target))
                {
                  flag = true;
                  this.timeSeen = Game.GameTime;
                  break;
                }
              }
              if (flag)
              {
                this.LastKnownPosition = LSL.playerPos;
                this.DriveBySate = Enums.DriveBySates.VehAttack;
                this.enterState = true;
                break;
              }
              if ((Entity) this.Leader != (Entity) null && this.Leader.Exists() && ((Entity) this.Vehicle != (Entity) null && this.Vehicle.Exists()) && (double) this.Leader.Position.DistanceTo(this.LastKnownPosition) < 10.0)
              {
                this.DriveBySate = Enums.DriveBySates.VehPatrol;
                this.enterState = true;
                break;
              }
              break;
            case Enums.DriveBySates.VehPatrol:
              if (this.enterState)
              {
                this.enterState = false;
                this.time = Game.GameTime;
                if ((Entity) this.Leader != (Entity) null && this.Leader.Exists() && ((Entity) this.Vehicle != (Entity) null && this.Vehicle.Exists()))
                  this.Leader?.Task.CruiseWithVehicle(this.Vehicle, 13f, 5);
              }
              if (LsFunctions.CanSee(this.Leader, this.Target))
              {
                this.timeSeen = Game.GameTime;
                this.LastKnownPosition = LSL.playerPos;
                this.DriveBySate = Enums.DriveBySates.VehAttack;
                this.enterState = true;
                break;
              }
              if ((Entity) this.Leader != (Entity) null && this.Leader.Exists() && ((Entity) this.Vehicle != (Entity) null && this.Vehicle.Exists()))
              {
                position = this.Vehicle.Position;
                if ((double) position.DistanceTo(this.LastKnownPosition) > 300.0)
                {
                  this.DriveBySate = Enums.DriveBySates.VehDrive;
                  this.enterState = true;
                  break;
                }
              }
              if (Game.GameTime > this.time + 120000)
              {
                this.DriveBySate = Enums.DriveBySates.CleanDriveBy;
                this.enterState = true;
                break;
              }
              break;
            case Enums.DriveBySates.VehAttack:
              if (this.enterState)
              {
                this.enterState = false;
                this.Leader?.Task.VehicleChase(this.Target);
              }
              if (!LsFunctions.CanSee(this.Leader, this.Target))
              {
                this.timeSeen = Game.GameTime;
                this.LastKnownPosition = LSL.playerPos;
                this.DriveBySate = Enums.DriveBySates.VehDrive;
                this.enterState = true;
                break;
              }
              break;
          }
        }
        else
        {
          switch (this.DriveBySate)
          {
            case Enums.DriveBySates.VehDrive:
              if (this.enterState)
              {
                this.enterState = false;
                this.Leader.Task.DriveTo(this.Vehicle, this.LastKnownPosition, 10f, 17f, 6);
              }
              position = this.Leader.Position;
              if ((double) position.DistanceTo(this.LastKnownPosition) < 20.0)
              {
                this.DriveBySate = Enums.DriveBySates.VehPatrol;
                this.enterState = true;
                break;
              }
              break;
            case Enums.DriveBySates.VehPatrol:
              if (this.enterState)
              {
                this.enterState = false;
                this.time = Game.GameTime;
                this.Leader.Task.CruiseWithVehicle(this.Vehicle, 13f, 5);
              }
              else
              {
                position = this.Vehicle.Position;
                if ((double) position.DistanceTo(this.LastKnownPosition) > 300.0)
                {
                  this.DriveBySate = Enums.DriveBySates.VehDrive;
                  this.enterState = true;
                }
              }
              if (Game.GameTime > this.time + 120000)
              {
                this.DriveBySate = Enums.DriveBySates.CleanDriveBy;
                this.enterState = true;
                break;
              }
              break;
          }
        }
      }
      if (!this.enterState || this.DriveBySate != Enums.DriveBySates.CleanDriveBy)
        return;
      UI.Notify("DriveByCleaned");
      this.enterState = false;
      this.Dead = true;
      this.CleanDriveBy();
    }

    private void CheckOtherDrivebys()
    {
      foreach (DriveBy driveBy in LSL.driveByHandler.GetDriveBies.Where<DriveBy>((Func<DriveBy, bool>) (r => r.timeSeen > this.timeSeen)))
      {
        if (this.DriveBySate == Enums.DriveBySates.VehPatrol || this.DriveBySate == Enums.DriveBySates.VehDrive && driveBy.timeSeen > this.timeSeen)
        {
          this.timeSeen = driveBy.timeSeen;
          this.LastKnownPosition = driveBy.LastKnownPosition;
          this.DriveBySate = Enums.DriveBySates.VehDrive;
          this.enterState = true;
          LsFunctions.DbugString("Driveby updated position from team");
        }
      }
    }

    private void CheckVehicle()
    {
      if (!((Entity) this.Vehicle != (Entity) null) || !this.Vehicle.Exists() || this.Peds.Count != 0)
        return;
      this.RemoveVehicle();
    }

    private void RemoveVehicle()
    {
      this.Vehicle.CurrentBlip.Remove();
      this.Vehicle.MarkAsNoLongerNeeded();
      LSL.entitiesCleanUp.Add((Entity) this.Vehicle);
      this.Vehicle = (Vehicle) null;
    }

    public void CleanDriveBy()
    {
      if (this.Peds.Count > 0)
      {
        foreach (Ped p in this.Peds.ToList<Ped>())
          this.RemovePed(p);
      }
      if ((Entity) this.Vehicle != (Entity) null)
        this.RemoveVehicle();
      this.Group.Dispose();
      LSL.driveByHandler.GetDriveBies.Remove(this);
    }

    private void RemovePed(Ped p)
    {
      if ((Entity) p != (Entity) LSL.attacker)
        LsFunctions.RemovePedFromWorld(p, true);
      this.Peds.Remove(p);
    }

    private void CheckPeds()
    {
      if (this.Peds.Count > 0)
      {
        foreach (Ped ped in this.Peds.ToList<Ped>())
        {
          if ((Entity) ped != (Entity) null && ped.Exists())
          {
            if (!ped.IsInVehicle() && !ped.CurrentBlip.Exists())
              LsFunctions.AddBlip((Entity) ped, BlipColor.Red, "Driveby Ped", false, false);
            if (ped.IsDead)
            {
              if (LSL.areas[LSL.areaIndex].GangPresance > 0)
                --LSL.areas[LSL.areaIndex].GangPresance;
              LsFunctions.RemovePedFromWorld(ped, true);
              this.Peds.Remove(ped);
            }
          }
          else
            this.Peds.Remove(ped);
        }
      }
      if (this.Peds.Count == 0)
      {
        this.DriveBySate = Enums.DriveBySates.CleanDriveBy;
        this.enterState = true;
      }
      else
      {
        if (!((Entity) this.Leader != (Entity) this.Peds.First<Ped>()))
          return;
        this.Leader = this.Peds.First<Ped>();
        foreach (Ped ped in this.Peds)
        {
          if (ped.CurrentPedGroup == this.Group)
            this.Group.Remove(ped);
          if ((Entity) ped == (Entity) this.Leader)
            this.Group.Add(ped, true);
          else
            this.Group.Add(ped, false);
        }
      }
    }

    private void SpawnVehicle()
    {
      Vector3 spawnPos = LSL.GenerateSpawnPos(this.DefendPosition.Around(300f), LSL.Nodetype.Road, false);
      while (spawnPos == this.lastSpawn || (double) spawnPos.DistanceTo(LSL.playerPos) < 200.0)
        spawnPos = LSL.GenerateSpawnPos(this.DefendPosition.Around(300f), LSL.Nodetype.Road, false);
      this.lastSpawn = spawnPos;
      this.Vehicle = World.CreateVehicle(LsFunctions.RequestModel(this.vehicleHash), spawnPos);
      int gameTime = Game.GameTime;
      while ((Entity) this.Vehicle == (Entity) null && Game.GameTime < gameTime + 1000)
        Script.Wait(0);
      if ((Entity) this.Vehicle != (Entity) null)
      {
        if (this.Agressive)
          LsFunctions.AddBlip((Entity) this.Vehicle, BlipColor.Red, "Driveby Vehicle", false, false);
        this.Vehicle.IsPersistent = true;
      }
      else
      {
        if (!LSL.DEBUG)
          return;
        UI.Notify("~r~Spawning DriveBy Failed");
      }
    }

    private void SpawnPeds()
    {
      this.SpawnPed(VehicleSeat.Driver, true);
      if (this.pedamount <= 0)
        return;
      this.SpawnPed(VehicleSeat.Passenger, false);
      if (this.pedamount <= 1)
        return;
      this.SpawnPed(VehicleSeat.RightRear, false);
      if (this.pedamount <= 2)
        return;
      this.SpawnPed(VehicleSeat.LeftRear, false);
    }

    private void SpawnPed(VehicleSeat _seat, bool _leader)
    {
      Ped randomPedOnSeat = this.Vehicle.CreateRandomPedOnSeat(_seat);
      if (!((Entity) randomPedOnSeat != (Entity) null) || !randomPedOnSeat.Exists())
        return;
      randomPedOnSeat.IsPersistent = true;
      randomPedOnSeat.RelationshipGroup = !this.Agressive ? LSL.rival_nutral : LSL.rival_enemy;
      LsFunctions.SetRandomCombatStats(randomPedOnSeat);
      randomPedOnSeat.FiringPattern = FiringPattern.BurstFireDriveby;
      randomPedOnSeat.AlwaysKeepTask = true;
      randomPedOnSeat.CanSwitchWeapons = true;
      Function.Call(Hash._0xC7622C0D36B2FDA8, (InputArgument) randomPedOnSeat, (InputArgument) 100);
      switch (LSL.rnd.Next(0, (int) (1.0 + (double) LSL.areas[LSL.areaIndex].GangPresance * 0.5)))
      {
        case 0:
          randomPedOnSeat.Weapons.Give(WeaponHash.Pistol, 500, true, true);
          break;
        case 1:
          randomPedOnSeat.Weapons.Give(WeaponHash.SawnOffShotgun, 500, false, true);
          randomPedOnSeat.Weapons.Give(WeaponHash.Pistol, 500, true, true);
          break;
        case 2:
          randomPedOnSeat.Weapons.Give(WeaponHash.PumpShotgun, 500, false, true);
          randomPedOnSeat.Weapons.Give(WeaponHash.MachinePistol, 500, true, true);
          break;
        case 3:
          randomPedOnSeat.Weapons.Give(WeaponHash.CompactRifle, 500, false, true);
          randomPedOnSeat.Weapons.Give(WeaponHash.MachinePistol, 500, true, true);
          break;
        case 4:
          randomPedOnSeat.Weapons.Give(WeaponHash.CompactRifle, 500, false, true);
          randomPedOnSeat.Weapons.Give(WeaponHash.MiniSMG, 500, true, true);
          break;
        case 5:
          randomPedOnSeat.Weapons.Give(WeaponHash.AssaultRifle, 500, false, true);
          randomPedOnSeat.Weapons.Give(WeaponHash.MicroSMG, 500, true, true);
          break;
      }
      if (_leader)
      {
        this.Leader = randomPedOnSeat;
        randomPedOnSeat.AlwaysKeepTask = true;
        DrivingStyle drivingStyle = DrivingStyle.Normal;
        if (this.Agressive)
          drivingStyle = DrivingStyle.AvoidTrafficExtremely;
        randomPedOnSeat.Task.DriveTo(this.Vehicle, this.LastKnownPosition, 10f, 20f, (int) drivingStyle);
      }
      else if (this.Agressive)
        randomPedOnSeat.Task.VehicleShootAtPed(this.Target);
      this.Peds.Add(randomPedOnSeat);
    }

    private void SetVehicleClass()
    {
      Vehicle[] nearbyVehicles = World.GetNearbyVehicles(this.Target, 200f);
      this.vehicleHash = VehicleHash.Baller2;
      if (((IEnumerable<Vehicle>) nearbyVehicles).ToList<Vehicle>().Count <= 0)
        return;
      foreach (Vehicle vehicle in nearbyVehicles)
      {
        if (vehicle.ClassType == this.vehicleClass)
        {
          this.vehicleHash = (VehicleHash) vehicle.Model.Hash;
          if (!LSL.DEBUG)
            break;
          UI.Notify("foundSUV");
          break;
        }
      }
    }
  }
}
