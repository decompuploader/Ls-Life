using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LSlife
{
  public class DrugDealer
  {
    public Vehicle dealerCar;
    public Ped lieut;
    public Ped goon;
    private Blip blip;
    private DrugDealer.DrugDealSates DealState;
    public bool EnterState;
    private bool Ready;
    private DrugOrder PlayerOrder;
    private DealerSpawnPostion dealerSpawn;
    private Vector3 backPos = new Vector3(0.0f, 0.0f, 0.0f);
    private int time;
    private PedGroup peds = new PedGroup();

    public bool Kill { get; private set; }

    public DrugDealer(DrugOrder _order)
    {
      this.dealerSpawn = this.GenerateDealerSpawn();
      this.PlayerOrder = _order;
      if (this.dealerSpawn != null)
      {
        if (_order != null)
        {
          if (this.blip == (Blip) null)
          {
            this.blip = World.CreateBlip(this.dealerSpawn.Position);
            if (this.blip != (Blip) null)
            {
              this.blip.Sprite = BlipSprite.SpecialCargo;
              this.blip.Name = "Drug Pickup";
              this.blip.ShowRoute = true;
              LsFunctions.DbugString("dealer car spawned");
              LsFunctions.TextMsg("ZEE", "Order", "Ok go meet my people in ~y~" + World.GetZoneName(this.dealerSpawn.Position) + " ~s~, they will sort you out.");
            }
            else
            {
              LsFunctions.SendTextMsg("LsLife: ~r~Failed to create blip");
              this.Destroy();
            }
          }
          LsFunctions.DbugString("dealer spawn found,~n~heading " + this.dealerSpawn.Heading.ToString() + ",~n~pos " + this.dealerSpawn.Position.ToString());
        }
        else
        {
          LsFunctions.SendTextMsg("LsLife: Error Order is null");
          this.Destroy();
        }
      }
      else
      {
        LsFunctions.TextMsg("ZEE", "Order", "We havent got anyone around there.");
        this.DealState = DrugDealer.DrugDealSates.Destroy;
      }
    }

    public void OnTick()
    {
      switch (this.DealState)
      {
        case DrugDealer.DrugDealSates.ReadyToDeal:
          if (!this.Ready || !Game.IsControlJustReleased(2, Control.Context) || !this.PlayerOrder.PayBill())
            break;
          if (LSL.player.Money > 0)
            LsFunctions.HandOverGoods(LSL.player.Character, this.lieut);
          this.DealState = DrugDealer.DrugDealSates.Payed;
          this.EnterState = true;
          this.Ready = false;
          Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) this.lieut, (InputArgument) "GENERIC_THANKS", (InputArgument) "SPEECH_PARAMS_STANDARD", (InputArgument) 0);
          break;
        case DrugDealer.DrugDealSates.Payed:
          World.DrawMarker(MarkerType.UpsideDownCone, this.backPos, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 0.5f), Color.Green);
          if (!this.Ready || !Game.IsControlJustReleased(2, Control.Context))
            break;
          this.PlayerOrder.PickupDrugs();
          if (!this.PlayerOrder.pickedUp)
            break;
          LSL.NewOrder.ResetOrder();
          this.DealState = DrugDealer.DrugDealSates.PickedUp;
          this.EnterState = true;
          this.Ready = false;
          break;
      }
    }

    private static void UpdateZeeDealerMenu()
    {
    }

    public void OnSlowTick()
    {
      switch (this.DealState)
      {
        case DrugDealer.DrugDealSates.SpawnStuff:
          if ((double) LSL.playerPos.DistanceTo(this.dealerSpawn.Position) >= 200.0)
            break;
          if ((Entity) this.dealerCar == (Entity) null)
          {
            this.dealerCar = World.CreateVehicle(LsFunctions.RequestModel(VehicleHash.Dubsta), this.dealerSpawn.Position, this.dealerSpawn.Heading);
            while ((Entity) this.dealerCar == (Entity) null || !this.dealerCar.Exists())
              Script.Yield();
            this.dealerCar.IsPersistent = true;
            this.dealerCar.LockStatus = VehicleLockStatus.LockedForPlayer;
            LsFunctions.DbugString("Dealer Car Spawned");
            break;
          }
          if ((Entity) this.goon == (Entity) null && (Entity) this.dealerCar != (Entity) null)
          {
            this.SpawnPed(VehicleSeat.Driver, false);
            if ((Entity) this.dealerCar.Driver != (Entity) null)
              this.goon = this.dealerCar.Driver;
          }
          if ((Entity) this.lieut == (Entity) null && (Entity) this.dealerCar != (Entity) null)
          {
            this.SpawnPed(VehicleSeat.Passenger, true);
            if ((Entity) this.dealerCar.GetPedOnSeat(VehicleSeat.Passenger) != (Entity) null)
              this.lieut = this.dealerCar.GetPedOnSeat(VehicleSeat.Passenger);
          }
          if (!((Entity) this.lieut != (Entity) null) || !this.lieut.Exists() || (!((Entity) this.goon != (Entity) null) || !this.goon.Exists()))
            break;
          this.EnterState = true;
          this.DealState = DrugDealer.DrugDealSates.ConfigStuff;
          break;
        case DrugDealer.DrugDealSates.ConfigStuff:
          if (this.EnterState)
          {
            this.EnterState = false;
            this.MakeGroup();
            break;
          }
          this.EnterState = true;
          this.DealState = DrugDealer.DrugDealSates.WaitForPlayer;
          break;
        case DrugDealer.DrugDealSates.WaitForPlayer:
          if (this.EnterState)
          {
            this.EnterState = false;
            break;
          }
          if ((double) LSL.playerPos.DistanceTo(this.lieut.Position) >= 20.0 || !LsFunctions.CanSee(LSL.player.Character, this.lieut))
            break;
          this.EnterState = true;
          this.DealState = DrugDealer.DrugDealSates.GetIntoPosition;
          break;
        case DrugDealer.DrugDealSates.GetIntoPosition:
          if (this.EnterState)
          {
            this.EnterState = false;
            this.backPos = Vector3.Add(this.dealerCar.Position, Vector3.Multiply(this.dealerCar.ForwardVector * -1f, (float) ((double) this.dealerCar.Model.GetDimensions().Y / 2.0 + 0.5)));
            this.dealerCar.OpenDoor(VehicleDoor.Trunk, false, false);
            Tasks task1 = this.lieut.Task;
            Vector3 position1 = this.dealerCar.Position;
            Vector3 vector3_1 = this.dealerCar.ForwardVector * -1f;
            Model model = this.dealerCar.Model;
            double num1 = (double) model.GetDimensions().Y / 2.0 + 0.5;
            Vector3 right1 = Vector3.Multiply(vector3_1, (float) num1);
            Vector3 position2 = Vector3.Add(position1, right1);
            task1.LookAt(position2);
            Tasks task2 = this.lieut.Task;
            Vector3 position3 = this.dealerCar.Position;
            Vector3 vector3_2 = this.dealerCar.ForwardVector * -1f;
            model = this.dealerCar.Model;
            double num2 = (double) model.GetDimensions().Y / 2.0 + 0.5;
            Vector3 right2 = Vector3.Multiply(vector3_2, (float) num2);
            Vector3 position4 = Vector3.Add(position3, right2) + this.dealerCar.RightVector * 1f;
            task2.GoTo(position4);
            Tasks task3 = this.goon.Task;
            Vector3 position5 = this.dealerCar.Position;
            Vector3 vector3_3 = this.dealerCar.ForwardVector * -1f;
            model = this.dealerCar.Model;
            double num3 = (double) model.GetDimensions().Y / 2.0 + 0.5;
            Vector3 right3 = Vector3.Multiply(vector3_3, (float) num3);
            Vector3 position6 = Vector3.Add(position5, right3) + this.dealerCar.RightVector * -2f;
            task3.GoTo(position6);
            break;
          }
          if ((double) this.lieut.Position.DistanceTo(Vector3.Add(this.dealerCar.Position, Vector3.Multiply(this.dealerCar.ForwardVector * -1f, (float) ((double) this.dealerCar.Model.GetDimensions().Y / 2.0 + 0.5))) + this.dealerCar.RightVector * 1f) >= 1.0)
            break;
          this.EnterState = true;
          this.DealState = DrugDealer.DrugDealSates.GetReadyToDeal;
          break;
        case DrugDealer.DrugDealSates.GetReadyToDeal:
          if (this.EnterState)
          {
            this.EnterState = false;
            break;
          }
          if (!this.dealerCar.IsDoorOpen(VehicleDoor.Trunk))
            break;
          this.lieut.Task.LookAt((Entity) LSL.player.Character);
          this.goon.Task.LookAt((Entity) LSL.player.Character);
          this.lieut.Task.TurnTo((Entity) LSL.player.Character);
          this.goon.Task.TurnTo((Entity) LSL.player.Character);
          this.goon.Weapons.Select(this.goon.Weapons.BestWeapon);
          this.EnterState = true;
          this.DealState = DrugDealer.DrugDealSates.ReadyToDeal;
          break;
        case DrugDealer.DrugDealSates.Destroy:
          if (this.EnterState)
          {
            this.EnterState = false;
            if ((Entity) this.lieut != (Entity) null && this.lieut.Exists())
              LsFunctions.RemovePedFromWorld(this.lieut, true);
            if ((Entity) this.goon != (Entity) null && this.lieut.Exists())
              LsFunctions.RemovePedFromWorld(this.goon, true);
            if ((Entity) this.dealerCar != (Entity) null && this.dealerCar.Exists())
              this.RemoveVehicle();
            if (this.blip != (Blip) null && this.blip.Exists())
              this.RemoveBlip();
            this.peds.Dispose();
            break;
          }
          this.Kill = true;
          break;
        case DrugDealer.DrugDealSates.ReadyToDeal:
          if (this.EnterState)
            this.EnterState = false;
          if (LSL.player.Character.IsStopped && (double) LSL.player.Character.Position.DistanceTo(this.lieut.Position) < 3.0)
          {
            this.PayBillPromt();
            this.Ready = true;
            break;
          }
          this.Ready = false;
          break;
        case DrugDealer.DrugDealSates.Payed:
          if (this.EnterState)
            this.EnterState = false;
          if (LSL.player.Character.IsStopped && (double) LSL.playerPos.DistanceTo(this.backPos) < 1.5)
          {
            this.PickupPromt();
            this.Ready = true;
            break;
          }
          this.Ready = false;
          break;
        case DrugDealer.DrugDealSates.PickedUp:
          if (this.EnterState)
          {
            Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) this.lieut, (InputArgument) "GENERIC_BYE", (InputArgument) "SPEECH_PARAMS_STANDARD", (InputArgument) 0);
            this.EnterState = false;
            break;
          }
          this.DealState = DrugDealer.DrugDealSates.Dismiss;
          this.EnterState = true;
          break;
        case DrugDealer.DrugDealSates.Dismiss:
          if (!((Entity) this.lieut != (Entity) null) || !((Entity) this.goon != (Entity) null) || !((Entity) this.dealerCar != (Entity) null))
            break;
          if (this.EnterState)
          {
            this.time = Game.GameTime;
            this.EnterState = false;
            this.lieut.Task.EnterVehicle(this.dealerCar, VehicleSeat.Passenger);
            this.goon.Task.EnterVehicle(this.dealerCar, VehicleSeat.Driver);
          }
          if ((!this.goon.IsInVehicle() || !this.lieut.IsInVehicle()) && Game.GameTime <= this.time + 10000)
            break;
          this.dealerCar.CloseDoor(VehicleDoor.Trunk, false);
          this.EnterState = true;
          this.DealState = DrugDealer.DrugDealSates.Destroy;
          if (this.PlayerOrder == null)
            break;
          this.PlayerOrder = (DrugOrder) null;
          break;
      }
    }

    private void PickupPromt()
    {
      if (LSL.LsMenuPool.IsAnyMenuOpen() || (double) Game.GameTime <= (double) LSL.displayHelpTimer + 6000.0)
        return;
      LSL.displayHelpTimer = (float) Game.GameTime;
      LsFunctions.DisplayHelpText("Press ~INPUT_CONTEXT~ to pickup the Drugs");
    }

    public void Dismiss()
    {
      this.EnterState = true;
      this.DealState = DrugDealer.DrugDealSates.Dismiss;
    }

    private void PayBillPromt()
    {
      if (LSL.LsMenuPool.IsAnyMenuOpen() || (double) Game.GameTime <= (double) LSL.displayHelpTimer + 6000.0)
        return;
      LSL.displayHelpTimer = (float) Game.GameTime;
      if (this.PlayerOrder.CanPayBill())
        LsFunctions.DisplayHelpText("Press ~INPUT_CONTEXT~ to do the deal");
      else
        LsFunctions.DisplayHelpText("You do not have enough credit or cash to cover the price.");
    }

    private void RemoveBlip()
    {
      this.blip.Alpha = 0;
      this.blip.Remove();
    }

    private void RemoveVehicle() => this.dealerCar.MarkAsNoLongerNeeded();

    private void SpawnPed(VehicleSeat _seat, bool _leader)
    {
      PedHash _hash = PedHash.ArmLieut01GMM;
      if (!_leader)
        _hash = PedHash.ArmGoon02GMY;
      Ped pedOnSeat = this.dealerCar.CreatePedOnSeat(_seat, LsFunctions.RequestModel(_hash));
      if (!((Entity) pedOnSeat != (Entity) null) || !pedOnSeat.Exists())
        return;
      pedOnSeat.IsPersistent = true;
      pedOnSeat.RelationshipGroup = LSL.playerRelationship;
      LsFunctions.SetRandomCombatStats(pedOnSeat);
      pedOnSeat.FiringPattern = FiringPattern.BurstFireDriveby;
      pedOnSeat.AlwaysKeepTask = true;
      pedOnSeat.CanSwitchWeapons = true;
      pedOnSeat.BlockPermanentEvents = true;
      Function.Call(Hash._0xC7622C0D36B2FDA8, (InputArgument) pedOnSeat, (InputArgument) 100);
      switch (LSL.rnd.Next(0, (int) (1.0 + (double) LSL.areas[LSL.areaIndex].GangPresance * 0.5)))
      {
        case 0:
          pedOnSeat.Weapons.Give(WeaponHash.Pistol, 500, true, true);
          break;
        case 1:
          pedOnSeat.Weapons.Give(WeaponHash.SawnOffShotgun, 500, false, true);
          pedOnSeat.Weapons.Give(WeaponHash.Pistol, 500, true, true);
          break;
        case 2:
          pedOnSeat.Weapons.Give(WeaponHash.PumpShotgun, 500, false, true);
          pedOnSeat.Weapons.Give(WeaponHash.MachinePistol, 500, true, true);
          break;
        case 3:
          pedOnSeat.Weapons.Give(WeaponHash.CompactRifle, 500, false, true);
          pedOnSeat.Weapons.Give(WeaponHash.MachinePistol, 500, true, true);
          break;
        case 4:
          pedOnSeat.Weapons.Give(WeaponHash.CompactRifle, 500, false, true);
          pedOnSeat.Weapons.Give(WeaponHash.MiniSMG, 500, true, true);
          break;
        case 5:
          pedOnSeat.Weapons.Give(WeaponHash.AssaultRifle, 500, false, true);
          pedOnSeat.Weapons.Give(WeaponHash.MicroSMG, 500, true, true);
          break;
      }
    }

    private DealerSpawnPostion GenerateDealerSpawn()
    {
      List<DealerSpawnPostion> list = LsFunctions.spawnPostions.Where<DealerSpawnPostion>((Func<DealerSpawnPostion, bool>) (s => (double) s.Position.DistanceTo(LSL.playerPos) < 1800.0 && (double) s.Position.DistanceTo(LSL.playerPos) > 400.0 - LsFunctions.CurrentRepLvl(LSL.dealer1Rep))).ToList<DealerSpawnPostion>();
      if (list.Count <= 0)
        return (DealerSpawnPostion) null;
      int index = LSL.rnd.Next(0, list.Count);
      return list[index];
    }

    private void MakeGroup()
    {
      if ((Entity) this.lieut != (Entity) null && this.lieut.Exists())
      {
        this.peds.Add(this.lieut, true);
        this.lieut.NeverLeavesGroup = true;
      }
      if (!((Entity) this.goon != (Entity) null) || !this.goon.Exists())
        return;
      this.peds.Add(this.goon, false);
      this.goon.NeverLeavesGroup = true;
    }

    public void Destroy()
    {
      this.EnterState = true;
      this.DealState = DrugDealer.DrugDealSates.Destroy;
    }

    private enum DrugDealSates
    {
      SpawnStuff,
      ConfigStuff,
      WaitForPlayer,
      GetIntoPosition,
      GetReadyToDeal,
      Destroy,
      ReadyToDeal,
      Payed,
      PickedUp,
      Dismiss,
    }
  }
}
