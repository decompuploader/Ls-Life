using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Linq;

namespace LSlife
{
  public class AreaDealer
  {
    private bool DEBUG;
    public Ped dPed;
    public Vehicle Vehicle;
    public Enums.DStates CurrentState;
    public Enums.DStates NewState = Enums.DStates.Normal;
    public Enums.DealState dealState;
    public bool Detected;
    private Vector3 servePos = Vector3.Zero;
    public LsTimer cTimer = new LsTimer(5, true, false);
    public Ped cPed;
    private int time;
    public string Area;

    public int LastCustomer { get; private set; }

    public int LastOffer { get; private set; }

    public AreaDealer(Ped p)
    {
      this.dPed = p;
      this.dPed.IsPersistent = true;
      this.dPed.Armor = 100;
      WeaponHash[] weaponHashArray = new WeaponHash[4]
      {
        WeaponHash.Pistol,
        WeaponHash.PumpShotgun,
        WeaponHash.SawnOffShotgun,
        WeaponHash.MicroSMG
      };
      this.dPed.Weapons.Give(weaponHashArray[LSL.rnd.Next(0, ((IEnumerable<WeaponHash>) weaponHashArray).Count<WeaponHash>())], 100, false, true);
      this.dPed.RelationshipGroup = LSL.rival_nutral;
      LsFunctions.SetRandomCombatStats(this.dPed);
      this.dPed.FiringPattern = FiringPattern.FullAuto;
      this.dPed.CanSwitchWeapons = true;
      Function.Call(Hash._0xC7622C0D36B2FDA8, (InputArgument) this.dPed, (InputArgument) 100);
      if (this.dPed.IsInVehicle())
        this.Vehicle = this.dPed.CurrentVehicle;
      this.DoState();
      this.CheckState();
      if (this.DEBUG)
        this.dPed.AddBlip();
      this.dPed.Money = LSL.rnd.Next(200, 2000);
      this.Area = World.GetZoneNameLabel(this.dPed.Position);
    }

    public void CheckState()
    {
      this.servePos = World.GetNextPositionOnSidewalk(this.dPed.Position + this.dPed.ForwardVector);
      switch (this.CurrentState)
      {
        case Enums.DStates.Normal:
          this.NewState = (double) this.dPed.Position.DistanceTo(this.servePos) >= 1.0 ? Enums.DStates.WalkToPath : Enums.DStates.Dealing;
          if (this.playerNear())
            this.NewState = Enums.DStates.StopForPlayer;
          if (!this.ShouldAttackPlayer())
            break;
          this.NewState = Enums.DStates.AttackPlayer;
          break;
        case Enums.DStates.Dealing:
          if ((Entity) this.cPed == (Entity) null && (double) this.dPed.Position.DistanceTo(this.servePos) > 2.5)
            this.NewState = Enums.DStates.WalkToPath;
          if (this.playerNear())
            this.NewState = Enums.DStates.StopForPlayer;
          if (!this.ShouldAttackPlayer())
            break;
          this.NewState = Enums.DStates.AttackPlayer;
          break;
        case Enums.DStates.StopForPlayer:
          if (this.dPed.IsStopped)
            this.NewState = Enums.DStates.LookAtPlayer;
          if (!this.playerNear())
            this.NewState = Enums.DStates.Normal;
          if (this.ShouldAttackPlayer())
            this.NewState = Enums.DStates.AttackPlayer;
          if (!((Entity) this.cPed != (Entity) null))
            break;
          LsFunctions.RemovePedFromWorld(this.cPed, true);
          this.cPed.CurrentBlip?.Remove();
          this.cPed = (Ped) null;
          break;
        case Enums.DStates.LookAtPlayer:
          if (!this.dPed.IsHeadtracking((Entity) Game.Player.Character))
            this.dPed.Task.LookAt((Entity) Game.Player.Character);
          else
            this.NewState = Enums.DStates.TalkToPlayer;
          if (!this.playerNear())
            this.NewState = Enums.DStates.Normal;
          if (!this.ShouldAttackPlayer())
            break;
          this.NewState = Enums.DStates.AttackPlayer;
          break;
        case Enums.DStates.TalkToPlayer:
          if (!this.playerNear())
          {
            this.NewState = Enums.DStates.Normal;
            Function.Call(Hash._0xC6941B4A3A8FBBB9, (InputArgument) this.dPed, (InputArgument) "GENERIC_BYE", (InputArgument) "SPEECH_PARAMS_FORCE");
          }
          if (!this.ShouldAttackPlayer())
            break;
          this.NewState = Enums.DStates.AttackPlayer;
          break;
        case Enums.DStates.AttackPlayer:
          if (this.Area == LSL.areas[LSL.areaIndex].Name && LSL.driveByHandler.GetDriveBies.Count < 1 && LSL.rnd.Next(0, LSL.areas[LSL.areaIndex].GangPresance) + 1 != 0)
            LsFunctions.SpawnDriveBy(LSL.player.Character, true);
          if (this.ShouldAttackPlayer())
            break;
          this.NewState = Enums.DStates.Normal;
          if ((this.Detected || !(this.dPed.CurrentBlip != (Blip) null)) && !this.dPed.CurrentBlip.Exists())
            break;
          this.dPed.CurrentBlip.Remove();
          break;
        case Enums.DStates.WalkToPath:
          if ((double) this.dPed.Position.DistanceTo(this.servePos) < 1.0)
            this.NewState = Enums.DStates.Normal;
          if (!this.playerNear())
            break;
          this.NewState = Enums.DStates.StopForPlayer;
          break;
      }
    }

    public void DoState()
    {
      if ((Entity) this.dPed != (Entity) null)
      {
        if (this.dPed.RelationshipGroup != LSL.rival_nutral && !this.dPed.IsInCombat)
        {
          if (this.dPed.RelationshipGroup != LSL.rival_enemy)
          {
            LsFunctions.DbugString("~g~ Rival dealer relasion ship changed to rival_enemy");
          }
          else
          {
            LsFunctions.DbugString("~r~ Rival dealer relasion ship changed to something. ~n~Changing it to rival_nutral");
            this.dPed.RelationshipGroup = LSL.rival_nutral;
          }
        }
        if (this.NewState != this.CurrentState)
        {
          if (this.DEBUG)
            UI.Notify("adealer state = " + this.NewState.ToString());
          switch (this.NewState)
          {
            case Enums.DStates.NoState:
              this.CurrentState = Enums.DStates.NoState;
              break;
            case Enums.DStates.Normal:
              if ((Entity) this.Vehicle != (Entity) null)
              {
                if (this.dPed.CurrentVehicle.Driver.Handle != this.dPed.Handle)
                  this.dPed.CurrentVehicle.Driver.Task.DriveTo(this.dPed.CurrentVehicle, World.GetNextPositionOnSidewalk(this.dPed.CurrentVehicle.Position), 3f, 5f);
                else
                  this.dPed.Task.DriveTo(this.dPed.CurrentVehicle, World.GetNextPositionOnSidewalk(this.dPed.CurrentVehicle.Position), 3f, 5f);
              }
              else
                this.dPed.Task.WanderAround();
              this.CurrentState = Enums.DStates.Normal;
              break;
            case Enums.DStates.Dealing:
              this.cTimer.Reset();
              this.cTimer.Start();
              this.dPed.Task.GuardCurrentPosition();
              this.CurrentState = Enums.DStates.Dealing;
              break;
            case Enums.DStates.StopForPlayer:
              if (this.dPed.IsWalking || this.dPed.IsRunning)
                this.dPed.Task.Wait(20000);
              this.CurrentState = Enums.DStates.StopForPlayer;
              break;
            case Enums.DStates.LookAtPlayer:
              this.dPed.Task.TurnTo((Entity) Game.Player.Character);
              this.CurrentState = Enums.DStates.LookAtPlayer;
              break;
            case Enums.DStates.TalkToPlayer:
              Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) this.dPed, (InputArgument) "GENERIC_HOWS_IT_GOING", (InputArgument) "SPEECH_PARAMS_FORCE");
              this.CurrentState = Enums.DStates.TalkToPlayer;
              break;
            case Enums.DStates.AttackPlayer:
              this.dPed.RelationshipGroup = LSL.rival_enemy;
              this.dPed.Task.ClearAll();
              LsFunctions.AddBlip((Entity) this.dPed, BlipColor.Red, "Rival Dealer", false, false);
              this.dPed.CanSwitchWeapons = true;
              this.dPed.Weapons.Select(this.dPed.Weapons.BestWeapon);
              this.dPed.Task.FightAgainst(Game.Player.Character);
              this.CurrentState = Enums.DStates.AttackPlayer;
              break;
            case Enums.DStates.WalkToPath:
              this.dPed.Task.ClearAll();
              this.dPed.Task.GoTo(this.servePos);
              this.CurrentState = Enums.DStates.WalkToPath;
              break;
          }
        }
        else
        {
          if (this.CurrentState != Enums.DStates.Dealing)
            return;
          if (!this.cTimer.Finished())
            this.cTimer.Tick();
          else
            this.Deal();
        }
      }
      else
        UI.Notify("~r~Area Dealer Ped NULL something wrong");
    }

    private bool ShouldAttackPlayer() => LsFunctions.CanSee(this.dPed, Game.Player.Character) ? (double) this.dPed.Position.DistanceTo(LSL.playerPos) < 200.0 && (this.CurrentState == Enums.DStates.AttackPlayer || (Entity) Game.Player.Character.GetMeleeTarget() == (Entity) this.dPed || Game.Player.IsTargetting((Entity) this.dPed) || (double) this.dPed.Position.DistanceTo(LSL.playerPos) < 50.0 && (this.dPed.IsInCombatAgainst(Game.Player.Character) || Game.Player.IsAiming)) : this.CurrentState == Enums.DStates.AttackPlayer && LSL.player.Character.IsAlive;

    public void PromtPlayerToTalk()
    {
      if (LSL.LsMenuPool.IsAnyMenuOpen())
        return;
      bool flag = true;
      string text = "";
      if (flag && LSL.DealerHandler.dealers.Count >= (int) LsFunctions.CurrentRepLvl(LSL.dealer1Rep))
      {
        flag = false;
        text = "You need more rep before hiring more dealers.";
      }
      if (flag)
      {
        if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 4000.0)
        {
          Function.Call(Hash._0x6178F68A87A4D3A0);
          LSL.displayHelpTimer = (float) Game.GameTime;
          LsFunctions.DisplayHelpText("Press ~INPUT_SPRINT~ to Talk to this person.");
        }
        if (!Game.IsControlJustPressed(2, Control.Sprint))
          return;
        Function.Call(Hash._0x6178F68A87A4D3A0);
        LSL.hireDealerMainMenu.Subtitle.Caption = "Test";
        LSL.hireDealerMainMenu.Visible = !LSL.hireDealerMainMenu.Visible;
        LSL.hireDealerMainMenu.CurrentSelection = 0;
        LSL.hireDealerOfferAmount.Maximum = Game.Player.Money;
      }
      else
      {
        if ((double) Game.GameTime <= (double) LSL.displayHelpTimer + 4000.0)
          return;
        Function.Call(Hash._0x6178F68A87A4D3A0);
        LSL.displayHelpTimer = (float) Game.GameTime;
        LsFunctions.DisplayHelpText(text);
      }
    }

    private bool playerNear() => (double) this.dPed.Position.DistanceTo(LSL.playerPos) < 4.0 && LsFunctions.CanSee(this.dPed, Game.Player.Character) || LsFunctions.CanHearPlayer(this.dPed);

    public bool DecidePlayerOffer(int amount)
    {
      int num = 100 - amount / 1000;
      if (num < 1)
        num = 1;
      return LSL.rnd.Next(0, num + LSL.areas[LSL.areaIndex].GangPresance) < 20;
    }

    public void SetLastOfferAmount(int _amount) => this.LastOffer = _amount;

    private void Deal()
    {
      if ((double) Game.Player.Character.Position.DistanceTo(this.dPed.Position) < 300.0)
      {
        if ((Entity) this.cPed != (Entity) null && !this.cPed.Exists())
        {
          this.cPed = (Ped) null;
          if (this.DEBUG)
            UI.Notify("aDealer Customer doesnt exist");
        }
        if ((Entity) this.cPed == (Entity) null)
        {
          if (this.DEBUG)
            UI.Notify("aDealer Getting Customer");
          List<Ped> getPeds = LSL.lPeds.GetPeds;
          if (getPeds.ToList<Ped>().Count > 0)
          {
            foreach (Ped _ped in getPeds)
            {
              if (!LSL.CheckIfPedUsed(_ped, false, false))
              {
                this.cPed = _ped;
                this.cPed.FreezePosition = false;
                this.cPed.IsPersistent = true;
                this.LastCustomer = this.cPed.Handle;
                if (!this.DEBUG)
                  break;
                this.cPed.AddBlip();
                break;
              }
            }
          }
          else
          {
            if (this.DEBUG)
              UI.Notify("aDealer found no peds");
            this.cTimer.Reset();
          }
        }
        else
        {
          switch (this.dealState)
          {
            case Enums.DealState.GetNear:
              this.servePos = this.dPed.Position;
              if ((double) this.cPed.Position.DistanceTo(this.servePos) > 100.0)
              {
                LsFunctions.RemovePedFromWorld(this.cPed, false);
                this.cPed = (Ped) null;
                this.cTimer.Reset();
                break;
              }
              if ((double) this.cPed.Position.DistanceTo(this.servePos) > 10.0)
              {
                LsFunctions.MovePed(this.cPed, this.servePos);
                break;
              }
              this.dealState = Enums.DealState.Approach;
              this.cPed.Task.LookAt((Entity) this.dPed);
              this.dPed.Task.TurnTo((Entity) this.cPed);
              break;
            case Enums.DealState.Approach:
              this.servePos = this.dPed.Position;
              if ((double) this.cPed.Position.DistanceTo(this.servePos) > 2.0)
              {
                LsFunctions.MovePed(this.cPed, this.servePos);
                break;
              }
              this.cPed.Task.TurnTo((Entity) this.dPed);
              this.dPed.Task.TurnTo((Entity) this.cPed);
              this.dealState = Enums.DealState.InPosition;
              this.time = Game.GameTime;
              break;
            case Enums.DealState.InPosition:
              if (Game.GameTime <= this.time + 1000)
                break;
              if (this.DEBUG)
                UI.Notify("Doing Deal");
              this.dealState = Enums.DealState.DoingDeal;
              this.cPed.Task.PlayAnimation("switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_franklin", 8f, 3000, AnimationFlags.UpperBodyOnly);
              this.dPed.Task.PlayAnimation("switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_shopkeeper", 8f, 3000, AnimationFlags.UpperBodyOnly);
              this.time = Game.GameTime;
              break;
            case Enums.DealState.DoingDeal:
              if (Game.GameTime <= this.time + 2000)
                break;
              this.dealState = Enums.DealState.WalkingAway;
              Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) this.cPed, (InputArgument) "GENERIC_THANKS", (InputArgument) "SPEECH_PARAMS_STANDARD");
              this.cPed.Task.WanderAround();
              this.dPed.Task.GuardCurrentPosition();
              break;
            case Enums.DealState.WalkingAway:
              this.servePos = this.dPed.Position;
              if ((double) this.cPed.Position.DistanceTo(this.servePos) <= 4.0)
                break;
              this.dealState = Enums.DealState.GetNear;
              if (this.DEBUG)
                UI.Notify("Deal Finished");
              LsFunctions.RemovePedFromWorld(this.cPed, false);
              this.cPed.CurrentBlip.Remove();
              this.cPed = (Ped) null;
              this.cTimer.Reset();
              this.cTimer.Start();
              break;
          }
        }
      }
      else
      {
        if (!((Entity) this.cPed != (Entity) null))
          return;
        if (this.DEBUG)
          UI.Notify("aDealer Removing Customer to Far Away");
        LsFunctions.RemovePedFromWorld(this.cPed, false);
        this.cPed = (Ped) null;
        if (this.dealState == Enums.DealState.GetNear)
          return;
        this.dealState = Enums.DealState.GetNear;
      }
    }
  }
}
