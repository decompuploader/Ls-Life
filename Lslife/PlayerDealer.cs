using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LSlife
{
  public class PlayerDealer
  {
    public List<XElement> dWeapons = new List<XElement>();
    public Enums.DealState dealState;
    public int ID;
    public Dictionary<string, int> Drugs = new Dictionary<string, int>();
    public int Money;
    public Enums.PdStates CurrentState;
    public Enums.PdStates NewState = Enums.PdStates.Work;
    private Enums.HealPositionTypes HealPosType;
    public PedHash pHash;
    public Vector3 Pos;
    private Blip blip;
    private int time;
    private Vector3 Target;
    public Dictionary<int, Tuple<int, int>> pData;
    public bool Working;
    private int health;

    public int dArmour { get; set; }

    public string area { get; private set; }

    public Ped Ped { get; private set; }

    public Ped cPed { get; private set; }

    public Ped lPed { get; private set; }

    public PlayerDealer(Ped ped)
    {
      if (!((Entity) ped != (Entity) null))
        return;
      this.Ped = ped;
      this.Ped.IsPersistent = true;
      LsFunctions.SetRandomCombatStats(this.Ped);
      this.SetAbilities();
      this.Ped.RelationshipGroup = LSL.playerRelationship;
      LsFunctions.SetImmuneToPlayer(this.Ped);
      this.dArmour = this.Ped.Armor;
      this.GetFreshPedsWeapons();
      this.Ped.Weapons.Select(this.Ped.Weapons.BestWeapon);
      this.Ped.CanSwitchWeapons = true;
      this.area = LSL.areas[LSL.areaIndex].Name;
      this.ID = LSL.DealerHandler.GetNewID();
      this.SetStartDrugs();
      this.pData = LsFunctions.GetPedVariationData(this.Ped);
      this.SaveFreshDealer();
      this.NewState = Enums.PdStates.FollowPlayer;
      LsFunctions.AddBlip((Entity) ped, BlipColor.Blue, "Dealer", true, true);
      this.SetBlip();
    }

    public PlayerDealer(
      int id,
      PedHash hash,
      Enums.PdStates _newState,
      Vector3 pos,
      int weed,
      int crack,
      int cocaine,
      int money,
      int _armour,
      List<XElement> _weapons,
      Dictionary<int, Tuple<int, int>> _pData)
    {
      this.area = World.GetZoneNameLabel(pos);
      this.ID = id;
      this.pHash = hash;
      this.Pos = pos;
      this.Drugs.Add("Weed", weed);
      this.Drugs.Add("Crack", crack);
      this.Drugs.Add("Cocaine", cocaine);
      this.Money = money;
      this.dArmour = _armour;
      if (_weapons.Count > 0)
      {
        for (int index = 0; index < _weapons.Count; ++index)
          this.dWeapons.Add(_weapons[index]);
      }
      this.NewState = _newState;
      this.CreateLocationBlip();
      this.pData = _pData;
    }

    private void Innit()
    {
    }

    internal List<object> Orders()
    {
      List<object> objectList = new List<object>();
      objectList.Add((object) "None");
      if (this.Working)
        objectList.Add((object) Enums.PdStates.FollowPlayer.ToString());
      else if (LSL.DealerHandler.dealers.Where<PlayerDealer>((Func<PlayerDealer, bool>) (d => d != this && d.area == this.area && d.Working)).Count<PlayerDealer>() == 0)
        objectList.Add((object) Enums.PdStates.Work.ToString());
      return objectList;
    }

    private void GetFreshPedsWeapons()
    {
      if (!((Entity) this.Ped != (Entity) null))
        return;
      this.dWeapons = LsFunctions.PedWeapons(this.Ped);
    }

    private void SaveFreshDealer()
    {
      XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Dealers.xml");
      this.Money = 0;
      this.Pos = this.Ped.Position;
      this.pHash = (PedHash) this.Ped.Model.Hash;
      XElement xelement1 = new XElement((XName) "dealer");
      XElement xelement2 = new XElement((XName) "id");
      xelement2.Value = this.ID.ToString();
      XElement xelement3 = new XElement((XName) "hash");
      xelement3.Value = this.pHash.ToString();
      XElement xelement4 = new XElement((XName) "posX");
      xelement4.Value = this.Ped.Position.X.ToString();
      XElement xelement5 = new XElement((XName) "posY");
      xelement5.Value = this.Ped.Position.Y.ToString();
      XElement xelement6 = new XElement((XName) "posZ");
      xelement6.Value = this.Ped.Position.Z.ToString();
      XElement xelement7 = new XElement((XName) "weed");
      xelement7.Value = this.Drugs["Weed"].ToString();
      XElement xelement8 = new XElement((XName) "crack");
      xelement8.Value = this.Drugs["Crack"].ToString();
      XElement xelement9 = new XElement((XName) "cocaine");
      xelement9.Value = this.Drugs["Cocaine"].ToString();
      XElement xelement10 = new XElement((XName) "money");
      xelement10.Value = this.Money.ToString();
      XElement xelement11 = new XElement((XName) "armour");
      xelement11.Value = this.dArmour.ToString();
      XElement xelement12 = new XElement((XName) "pedData");
      bool flag = false;
      Dictionary<int, Tuple<int, int>> pData = this.pData;
      // ISSUE: explicit non-virtual call
      if ((pData != null ? (__nonvirtual (pData.Count) > 0 ? 1 : 0) : 0) != 0)
      {
        flag = true;
        XName name1 = (XName) "headD";
        int num = this.pData[0].Item1;
        string str1 = num.ToString();
        XElement xelement13 = new XElement(name1, (object) str1);
        XName name2 = (XName) "headT";
        num = this.pData[0].Item2;
        string str2 = num.ToString();
        XElement xelement14 = new XElement(name2, (object) str2);
        xelement12.Add((object) xelement13);
        xelement12.Add((object) xelement14);
        XName name3 = (XName) "beardD";
        num = this.pData[1].Item1;
        string str3 = num.ToString();
        XElement xelement15 = new XElement(name3, (object) str3);
        XName name4 = (XName) "beardT";
        num = this.pData[1].Item2;
        string str4 = num.ToString();
        XElement xelement16 = new XElement(name4, (object) str4);
        xelement12.Add((object) xelement15);
        xelement12.Add((object) xelement16);
        XName name5 = (XName) "hairD";
        num = this.pData[2].Item1;
        string str5 = num.ToString();
        XElement xelement17 = new XElement(name5, (object) str5);
        XName name6 = (XName) "hairT";
        num = this.pData[2].Item2;
        string str6 = num.ToString();
        XElement xelement18 = new XElement(name6, (object) str6);
        xelement12.Add((object) xelement17);
        xelement12.Add((object) xelement18);
        XName name7 = (XName) "torsoD";
        num = this.pData[3].Item1;
        string str7 = num.ToString();
        XElement xelement19 = new XElement(name7, (object) str7);
        XName name8 = (XName) "torsoT";
        num = this.pData[3].Item2;
        string str8 = num.ToString();
        XElement xelement20 = new XElement(name8, (object) str8);
        xelement12.Add((object) xelement19);
        xelement12.Add((object) xelement20);
        XName name9 = (XName) "legsD";
        num = this.pData[4].Item1;
        string str9 = num.ToString();
        XElement xelement21 = new XElement(name9, (object) str9);
        XName name10 = (XName) "legsT";
        num = this.pData[4].Item2;
        string str10 = num.ToString();
        XElement xelement22 = new XElement(name10, (object) str10);
        xelement12.Add((object) xelement21);
        xelement12.Add((object) xelement22);
        XName name11 = (XName) "handsD";
        num = this.pData[5].Item1;
        string str11 = num.ToString();
        XElement xelement23 = new XElement(name11, (object) str11);
        XName name12 = (XName) "handsT";
        num = this.pData[5].Item2;
        string str12 = num.ToString();
        XElement xelement24 = new XElement(name12, (object) str12);
        xelement12.Add((object) xelement23);
        xelement12.Add((object) xelement24);
        XName name13 = (XName) "footD";
        num = this.pData[6].Item1;
        string str13 = num.ToString();
        XElement xelement25 = new XElement(name13, (object) str13);
        XName name14 = (XName) "footT";
        num = this.pData[6].Item2;
        string str14 = num.ToString();
        XElement xelement26 = new XElement(name14, (object) str14);
        xelement12.Add((object) xelement25);
        xelement12.Add((object) xelement26);
        XName name15 = (XName) "acces1D";
        num = this.pData[8].Item1;
        string str15 = num.ToString();
        XElement xelement27 = new XElement(name15, (object) str15);
        XName name16 = (XName) "acces1T";
        num = this.pData[8].Item2;
        string str16 = num.ToString();
        XElement xelement28 = new XElement(name16, (object) str16);
        xelement12.Add((object) xelement27);
        xelement12.Add((object) xelement28);
        XName name17 = (XName) "acces2D";
        num = this.pData[9].Item1;
        string str17 = num.ToString();
        XElement xelement29 = new XElement(name17, (object) str17);
        XName name18 = (XName) "acces2T";
        num = this.pData[9].Item2;
        string str18 = num.ToString();
        XElement xelement30 = new XElement(name18, (object) str18);
        xelement12.Add((object) xelement29);
        xelement12.Add((object) xelement30);
        XName name19 = (XName) "decalD";
        num = this.pData[10].Item1;
        string str19 = num.ToString();
        XElement xelement31 = new XElement(name19, (object) str19);
        XName name20 = (XName) "decalT";
        num = this.pData[10].Item2;
        string str20 = num.ToString();
        XElement xelement32 = new XElement(name20, (object) str20);
        xelement12.Add((object) xelement31);
        xelement12.Add((object) xelement32);
        XName name21 = (XName) "torsoAuxD";
        num = this.pData[11].Item1;
        string str21 = num.ToString();
        XElement xelement33 = new XElement(name21, (object) str21);
        XName name22 = (XName) "torsoAuxT";
        num = this.pData[11].Item2;
        string str22 = num.ToString();
        XElement xelement34 = new XElement(name22, (object) str22);
        xelement12.Add((object) xelement33);
        xelement12.Add((object) xelement34);
      }
      xelement1.Add((object) xelement2);
      xelement1.Add((object) xelement3);
      xelement1.Add((object) xelement4);
      xelement1.Add((object) xelement5);
      xelement1.Add((object) xelement6);
      xelement1.Add((object) xelement7);
      xelement1.Add((object) xelement8);
      xelement1.Add((object) xelement9);
      xelement1.Add((object) xelement10);
      xelement1.Add((object) xelement11);
      List<XElement> dWeapons = this.dWeapons;
      // ISSUE: explicit non-virtual call
      if ((dWeapons != null ? (__nonvirtual (dWeapons.Count) > 0 ? 1 : 0) : 0) != 0)
      {
        foreach (XElement dWeapon in this.dWeapons)
        {
          if (dWeapon.Name == (XName) "weapon")
          {
            foreach (XElement element in dWeapon.Elements())
            {
              if (element.Name == (XName) "HASH" && element.Value != "Unarmed")
                xelement1.Add((object) element.Parent);
            }
          }
        }
      }
      if (flag)
        xelement1.Add((object) xelement12);
      xdocument.Root.Add((object) xelement1);
      xdocument.Save("scripts\\LSLife\\LSLife_Dealers.xml");
    }

    public void OnTick()
    {
      if (!((Entity) this.Ped != (Entity) null))
        return;
      switch (this.CurrentState)
      {
        case Enums.PdStates.TalkToPlayer:
        case Enums.PdStates.FollowPlayer:
        case Enums.PdStates.FollowingPlayer:
        case Enums.PdStates.Defend:
          this.PromtPlayerToTalk();
          break;
        default:
          this.PromtToFollow();
          break;
      }
    }

    public void OnSlowTick()
    {
      if ((Entity) this.Ped != (Entity) null)
      {
        if (this.Ped.RelationshipGroup != LSL.playerRelationship)
        {
          UI.Notify("~r~Dealer was removed from companion group");
          this.Ped.RelationshipGroup = LSL.playerRelationship;
        }
        if (!this.isFollowing() && LSL.driveByHandler.AmountOfDriveBy(true) > 0 && !this.playerNear())
          this.NewState = Enums.PdStates.Defend;
        if (this.Pos != this.Ped.Position)
          this.Pos = this.Ped.Position;
        if (this.area != World.GetZoneNameLabel(this.Ped.Position))
          this.area = World.GetZoneNameLabel(this.Ped.Position);
        this.dArmour = this.Ped.Armor;
        if (LSL.DealerHandler.currentDealer == this)
        {
          bool flag1 = (double) this.Pos.DistanceTo(LSL.playerPos) > 3.0;
          bool flag2 = this.CurrentState != Enums.PdStates.TalkToPlayer && this.CurrentState != Enums.PdStates.FollowPlayer && this.CurrentState != Enums.PdStates.FollowingPlayer;
          int num1 = !LSL.WorkerMainMenu.Visible ? 1 : 0;
          bool flag3 = this.Ped.IsInVehicle() && (double) this.Ped.CurrentVehicle.Speed > 0.0;
          int num2 = flag2 ? 1 : 0;
          if ((num1 | num2 | (flag1 ? 1 : 0) | (flag3 ? 1 : 0)) != 0)
            LSL.DealerHandler.currentDealer = (PlayerDealer) null;
          if (LSL.DealerHandler.currentDealer == null && LSL.WorkerMainMenu.Visible)
            LSL.WorkerMainMenu.Visible = false;
        }
        if (!this.Ped.IsAlive && this.NewState != Enums.PdStates.Dead)
          this.NewState = Enums.PdStates.Dead;
        Vector3 position;
        if (this.NewState != this.CurrentState)
        {
          if ((Entity) this.cPed != (Entity) null)
            LsFunctions.RemovePedFromWorld(this.cPed, true);
          if (LSL.DEBUG)
            UI.Notify("PDealer state = " + this.NewState.ToString());
          switch (this.NewState)
          {
            case Enums.PdStates.Work:
              this.Working = true;
              this.Target = World.GetNextPositionOnSidewalk(this.Ped.Position);
              if ((Entity) this.Ped != (Entity) null)
                this.SetBlip();
              if (this.Ped.CurrentPedGroup == Game.Player.Character.CurrentPedGroup)
                this.Ped.LeaveGroup();
              if ((Entity) this.Ped.CurrentVehicle != (Entity) null && (Entity) this.Ped.CurrentVehicle == (Entity) Game.Player.Character.CurrentVehicle)
              {
                TaskSequence sequence = new TaskSequence();
                sequence.AddTask.LeaveVehicle();
                sequence.AddTask.GuardCurrentPosition();
                this.Ped.Task.PerformSequence(sequence);
                sequence.Dispose();
              }
              else
                this.Ped.Task.GuardCurrentPosition();
              this.CurrentState = Enums.PdStates.Work;
              break;
            case Enums.PdStates.Dealing:
              this.CurrentState = Enums.PdStates.Dealing;
              break;
            case Enums.PdStates.StopForPlayer:
              this.Ped.Task.StandStill(50000);
              this.CurrentState = Enums.PdStates.StopForPlayer;
              break;
            case Enums.PdStates.LookAtPlayer:
              this.Ped.Task.TurnTo((Entity) Game.Player.Character);
              this.CurrentState = Enums.PdStates.LookAtPlayer;
              break;
            case Enums.PdStates.TalkToPlayer:
              this.CurrentState = Enums.PdStates.TalkToPlayer;
              Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) this.Ped, (InputArgument) "GENERIC_HOWS_IT_GOING", (InputArgument) "SPEECH_PARAMS_FORCE");
              break;
            case Enums.PdStates.WalkToPath:
              this.Target = World.GetNextPositionOnSidewalk(this.Ped.Position);
              this.Ped.Task.GoTo(this.Target);
              this.CurrentState = Enums.PdStates.WalkToPath;
              this.time = Game.GameTime;
              break;
            case Enums.PdStates.Dead:
              this.CurrentState = Enums.PdStates.Dead;
              string _subtitleString = "One of your dealers dropped a ~g~bag.";
              LSL.pickupHandler.AddBag(new DroppedBag(_subtitleString, this.Drugs["Weed"], this.Drugs["Crack"], this.Drugs["Cocaine"], this.Money, this.dWeapons.ToList<XElement>(), this.Pos));
              this.RemoveBlip();
              this.Ped.IsPersistent = false;
              this.Ped.MarkAsNoLongerNeeded();
              break;
            case Enums.PdStates.FollowPlayer:
              this.Working = false;
              if ((Entity) this.Ped != (Entity) null)
                this.SetBlip();
              if (this.Ped.CurrentPedGroup == LSL.player.Character.CurrentPedGroup)
                this.Ped.LeaveGroup();
              this.Ped.Task.ClearAll();
              this.CurrentState = Enums.PdStates.FollowPlayer;
              break;
            case Enums.PdStates.FollowingPlayer:
              this.CurrentState = Enums.PdStates.FollowingPlayer;
              break;
            case Enums.PdStates.Rest:
              this.Target = LSL.DealerHandler.GetCloseHealPosition(this.Ped.Position);
              if (LSL.DealerHandler.HealPositionType != Enums.HealPositionTypes.None && (double) this.Target.DistanceTo(this.Ped.Position) < 30.0)
              {
                if (!this.Ped.IsInVehicle() || this.Ped.IsInVehicle() && (double) this.Ped.CurrentVehicle.Speed < 1.0)
                {
                  if (LSL.DealerHandler.HealPositionType == Enums.HealPositionTypes.Hospital && Game.Player.Money >= (this.Ped.MaxHealth - this.Ped.Health) * 50)
                  {
                    this.Ped.LeaveGroup();
                    this.Ped.BlockPermanentEvents = true;
                    this.HealPosType = LSL.DealerHandler.HealPositionType;
                    this.Ped.Task.RunTo(this.Target);
                    this.CurrentState = Enums.PdStates.Rest;
                    break;
                  }
                  if (LSL.DealerHandler.HealPositionType == Enums.HealPositionTypes.House)
                  {
                    this.Ped.LeaveGroup();
                    this.Ped.BlockPermanentEvents = true;
                    this.HealPosType = LSL.DealerHandler.HealPositionType;
                    this.Ped.Task.RunTo(this.Target);
                    this.CurrentState = Enums.PdStates.Rest;
                    break;
                  }
                  break;
                }
                break;
              }
              if (LSL.player.Character.IsDead || this.Ped.CurrentPedGroup != LSL.player.Character.CurrentPedGroup || (double) LSL.playerPos.DistanceTo(this.Ped.Position) > 100.0)
              {
                this.Ped.LeaveGroup();
                this.NewState = Enums.PdStates.FollowPlayer;
                break;
              }
              break;
            case Enums.PdStates.Resting:
              XDocument _doc = XDocument.Load("scripts\\LSLife\\LSLife_Dealers.xml");
              LSL.DealerHandler.SaveDealer(this, _doc);
              _doc.Save("scripts\\LSLife\\LSLife_Dealers.xml");
              this.CurrentState = Enums.PdStates.Resting;
              break;
            case Enums.PdStates.Defend:
              this.CurrentState = Enums.PdStates.Defend;
              break;
            case Enums.PdStates.LeadTeam:
              if (LSL.DealerHandler.CreateTeam(this))
              {
                this.CurrentState = Enums.PdStates.LeadTeam;
                this.Ped.LeaveGroup();
                this.Ped.AlwaysKeepTask = true;
                break;
              }
              this.NewState = Enums.PdStates.FollowPlayer;
              break;
          }
        }
        else
        {
          switch (this.CurrentState)
          {
            case Enums.PdStates.Work:
              if (this.playerNear())
              {
                this.NewState = Enums.PdStates.StopForPlayer;
                break;
              }
              position = this.Ped.Position;
              this.NewState = (double) position.DistanceTo(this.Target) <= 1.0 ? Enums.PdStates.Dealing : Enums.PdStates.WalkToPath;
              break;
            case Enums.PdStates.Dealing:
              this.Target = World.GetNextPositionOnSidewalk(this.Pos + this.Ped.Velocity);
              if (this.playerNear())
              {
                this.NewState = Enums.PdStates.StopForPlayer;
                break;
              }
              if ((double) this.Pos.DistanceTo(this.Target) > 2.29999995231628)
              {
                if (Function.Call<bool>(Hash._0x125BF4ABFC536B09, (InputArgument) this.Pos.X, (InputArgument) this.Pos.Y, (InputArgument) this.Pos.Z))
                {
                  this.NewState = Enums.PdStates.WalkToPath;
                  break;
                }
              }
              if (LsFunctions.TotalDrugs(this.Drugs) > 0)
              {
                this.Deal();
                break;
              }
              break;
            case Enums.PdStates.StopForPlayer:
              if (this.Ped.IsStopped)
                this.NewState = Enums.PdStates.LookAtPlayer;
              if (!this.playerNear())
              {
                this.NewState = Enums.PdStates.Work;
                break;
              }
              break;
            case Enums.PdStates.LookAtPlayer:
              if (!this.Ped.IsHeadtracking((Entity) LSL.player.Character))
                this.Ped.Task.LookAt((Entity) LSL.player.Character);
              else
                this.NewState = Enums.PdStates.TalkToPlayer;
              if (!this.playerNear())
              {
                this.NewState = Enums.PdStates.Work;
                break;
              }
              break;
            case Enums.PdStates.TalkToPlayer:
              if (!this.playerNear())
              {
                if (LSL.DealerHandler.currentDealer == this && LSL.WorkerMainMenu.Visible)
                  LSL.WorkerMainMenu.Visible = false;
                this.NewState = Enums.PdStates.Work;
                Function.Call(Hash._0xC6941B4A3A8FBBB9, (InputArgument) this.Ped, (InputArgument) "GENERIC_BYE", (InputArgument) "SPEECH_PARAMS_FORCE");
                break;
              }
              break;
            case Enums.PdStates.WalkToPath:
              position = this.Ped.Position;
              if ((double) position.DistanceTo(this.Target) <= 2.0)
                this.NewState = Enums.PdStates.Work;
              if (this.playerNear())
              {
                this.NewState = Enums.PdStates.StopForPlayer;
                break;
              }
              break;
            case Enums.PdStates.FollowPlayer:
              if (LSL.player.Character.CurrentPedGroup != this.Ped.CurrentPedGroup)
              {
                position = LSL.player.Character.Position;
                if ((double) position.DistanceTo(this.Pos) < 100.0)
                {
                  this.Ped.BlockPermanentEvents = false;
                  LSL.player.Character.CurrentPedGroup.Add(this.Ped, false);
                  Function.Call(Hash._0x2E2F4240B3F24647, (InputArgument) this.Ped.Handle, (InputArgument) LSL.player.Character.CurrentPedGroup.Handle, (InputArgument) false);
                  this.NewState = Enums.PdStates.FollowingPlayer;
                  break;
                }
              }
              if (LSL.player.Character.CurrentPedGroup == this.Ped.CurrentPedGroup)
              {
                this.Ped.BlockPermanentEvents = false;
                Function.Call(Hash._0x2E2F4240B3F24647, (InputArgument) this.Ped.Handle, (InputArgument) LSL.player.Character.CurrentPedGroup.Handle, (InputArgument) false);
                this.NewState = Enums.PdStates.FollowingPlayer;
                break;
              }
              break;
            case Enums.PdStates.FollowingPlayer:
              if (LSL.player.Character.IsDead || this.Ped.CurrentPedGroup != LSL.player.Character.CurrentPedGroup || (double) LSL.playerPos.DistanceTo(this.Pos) > 100.0)
              {
                this.Ped.LeaveGroup();
                this.NewState = Enums.PdStates.FollowPlayer;
              }
              else if (this.Ped.Health < this.Ped.MaxHealth && !this.Ped.IsInCombat && (double) LSL.DealerHandler.GetCloseHealPosition(this.Pos).DistanceTo(this.Pos) < 30.0)
                this.NewState = Enums.PdStates.Rest;
              bool flag4 = this.Ped.Weapons.Current.Hash == WeaponHash.Unarmed;
              bool flag5 = LSL.player.Character.Weapons.Current.Hash == WeaponHash.Unarmed;
              if (!flag4 & flag5)
              {
                this.Ped.Weapons.Select(WeaponHash.Unarmed, true);
                this.Ped.CanSwitchWeapons = false;
                break;
              }
              if (flag4 && !flag5)
              {
                this.Ped.Weapons.Select(this.Ped.Weapons.BestWeapon);
                this.Ped.CanSwitchWeapons = true;
                break;
              }
              break;
            case Enums.PdStates.Rest:
              if ((double) this.Pos.DistanceTo(this.Target) < 2.0)
              {
                int healPosType = (int) this.HealPosType;
                this.health = this.Ped.Health;
                this.NewState = Enums.PdStates.Resting;
                break;
              }
              break;
            case Enums.PdStates.Resting:
              if (this.Ped.Health < this.Ped.MaxHealth)
              {
                if (this.HealPosType == Enums.HealPositionTypes.Hospital)
                {
                  this.Ped.Health = this.Ped.MaxHealth;
                  Game.Player.Money -= (this.Ped.MaxHealth - this.Ped.Health) * 50;
                  LsFunctions.PlayTheSound();
                  break;
                }
                ++this.Ped.Health;
                break;
              }
              this.NewState = Enums.PdStates.FollowPlayer;
              break;
            case Enums.PdStates.Defend:
              if (LSL.driveByHandler.AmountOfDriveBy(true) == 0 || this.playerNear())
              {
                this.NewState = Enums.PdStates.Work;
                break;
              }
              break;
          }
        }
        position = this.Ped.Position;
        if ((double) position.DistanceTo(LSL.playerPos) <= 200.0 || !(this.area != LSL.playerArea) || (this.NewState == Enums.PdStates.FollowPlayer || this.NewState == Enums.PdStates.FollowingPlayer))
          return;
        this.DespawnPed();
      }
      else
      {
        if ((double) this.Pos.DistanceTo(LSL.playerPos) > 200.0 || this.CurrentState == Enums.PdStates.Dead)
          return;
        this.SpawnPed();
      }
    }

    public void GiveArmourToDealer()
    {
      this.Ped.Armor = 200;
      this.dArmour = 200;
    }

    private bool CheckIfDead()
    {
      if (this.Ped.IsAlive)
        return false;
      this.RemoveBlip();
      this.Ped.IsPersistent = false;
      this.Ped = (Ped) null;
      return true;
    }

    private void Deal()
    {
      if ((Entity) this.Ped != (Entity) null)
      {
        if ((Entity) this.cPed != (Entity) null && !this.cPed.Exists())
        {
          this.lPed = this.cPed;
          this.cPed = (Ped) null;
          if (LSL.DEBUG)
            UI.Notify("pDealer Customer doesnt exist");
        }
        if ((Entity) this.cPed != (Entity) null && this.cPed.Exists() && this.cPed.IsDead)
        {
          this.lPed = this.cPed;
          this.cPed = (Ped) null;
          if (LSL.DEBUG)
            UI.Notify("pDealer Customer doesnt exist");
        }
        if ((Entity) this.cPed == (Entity) null)
        {
          if (LSL.rnd.Next(0, 5) != 0)
            return;
          if (LSL.DEBUG)
            UI.Notify("pDealer Getting Customer");
          List<Ped> getPeds = LSL.lPeds.GetPeds;
          if (getPeds.ToList<Ped>().Count > 0)
          {
            foreach (Ped _ped in getPeds)
            {
              if (!LSL.CheckIfPedUsed(_ped, false, false) && (Entity) _ped != (Entity) this.lPed)
              {
                this.cPed = _ped;
                this.cPed.FreezePosition = false;
                this.cPed.IsPersistent = true;
                if (!LSL.DEBUG)
                  break;
                this.cPed.AddBlip();
                break;
              }
            }
          }
          else
          {
            if (!LSL.DEBUG)
              return;
            UI.Notify("pDealer found no peds");
          }
        }
        else
        {
          switch (this.dealState)
          {
            case Enums.DealState.GetNear:
              Vector3 position = this.cPed.Position;
              if ((double) position.DistanceTo(this.Ped.Position) > 100.0)
              {
                LsFunctions.RemovePedFromWorld(this.cPed, false);
                this.cPed = (Ped) null;
                break;
              }
              position = this.cPed.Position;
              if ((double) position.DistanceTo(this.Ped.Position) > 10.0)
              {
                LsFunctions.MovePed(this.cPed, this.Ped.Position);
                break;
              }
              this.dealState = Enums.DealState.Approach;
              this.cPed.Task.LookAt((Entity) this.Ped);
              this.Ped.Task.TurnTo((Entity) this.cPed);
              break;
            case Enums.DealState.Approach:
              if ((double) this.cPed.Position.DistanceTo(this.Ped.Position) > 2.0)
              {
                LsFunctions.MovePed(this.cPed, this.Ped.Position);
                break;
              }
              this.cPed.Task.TurnTo((Entity) this.Ped);
              this.Ped.Task.TurnTo((Entity) this.cPed);
              this.dealState = Enums.DealState.InPosition;
              this.time = Game.GameTime;
              break;
            case Enums.DealState.InPosition:
              if (Game.GameTime <= this.time + 1000)
                break;
              if (LSL.DEBUG)
                UI.Notify("Doing Deal");
              this.dealState = Enums.DealState.DoingDeal;
              this.cPed.Task.PlayAnimation("switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_franklin", 8f, 3000, AnimationFlags.UpperBodyOnly);
              this.Ped.Task.PlayAnimation("switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_shopkeeper", 8f, 3000, AnimationFlags.UpperBodyOnly);
              this.time = Game.GameTime;
              break;
            case Enums.DealState.DoingDeal:
              if (Game.GameTime <= this.time + 2000)
                break;
              this.dealState = Enums.DealState.WalkingAway;
              Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) this.cPed, (InputArgument) "GENERIC_THANKS", (InputArgument) "SPEECH_PARAMS_STANDARD");
              this.cPed.Task.WanderAround();
              this.Ped.Task.GuardCurrentPosition();
              break;
            case Enums.DealState.WalkingAway:
              if ((double) this.cPed.Position.DistanceTo(this.Ped.Position) <= 4.0)
                break;
              this.dealState = Enums.DealState.GetNear;
              if (LSL.DEBUG)
                UI.Notify("Deal Finished");
              LsFunctions.RemovePedFromWorld(this.cPed, false);
              this.cPed.CurrentBlip.Remove();
              this.lPed = this.cPed;
              this.cPed = (Ped) null;
              break;
          }
        }
      }
      else
      {
        if (!((Entity) this.cPed != (Entity) null))
          return;
        if (LSL.DEBUG)
          UI.Notify("aDealer Removing Customer to Far Away");
        LsFunctions.RemovePedFromWorld(this.cPed, false);
        this.cPed = (Ped) null;
        if (this.dealState == Enums.DealState.GetNear)
          return;
        this.dealState = Enums.DealState.GetNear;
      }
    }

    private void SetAbilities()
    {
      Function.Call(Hash._0xC7622C0D36B2FDA8, (InputArgument) this.Ped, (InputArgument) 100);
      Function.Call(Hash._0x4D9CA1009AFBD057, (InputArgument) this.Ped, (InputArgument) 2);
      this.Ped.Accuracy = 100;
      this.Ped.ShootRate = 100;
      this.Ped.FiringPattern = FiringPattern.FullAuto;
      this.Ped.CanWrithe = false;
    }

    public void SpawnPed()
    {
      this.Ped = World.CreatePed((Model) this.pHash, this.Pos);
      if (!((Entity) this.Ped != (Entity) null))
        return;
      Function.Call(Hash._0x2E2F4240B3F24647, (InputArgument) this.Ped.Handle, (InputArgument) Game.Player.Character.CurrentPedGroup.Handle, (InputArgument) false);
      LsFunctions.SetImmuneToPlayer(this.Ped);
      LsFunctions.AddBlip((Entity) this.Ped, BlipColor.Blue, "Dealer", true, true);
      this.SetBlip();
      this.Ped.Armor = this.dArmour;
      this.GiveSavedWeapons();
      this.RemoveLocationBlip();
      this.Ped.Health = 200;
      this.Ped.IsPersistent = true;
      this.SetAbilities();
      this.Ped.RelationshipGroup = LSL.playerRelationship;
      this.Ped.Weapons.Select(this.Ped.Weapons.BestWeapon);
      this.Ped.CanSwitchWeapons = true;
      if (this.CurrentState == Enums.PdStates.FollowPlayer && Game.Player.Character.CurrentPedGroup != this.Ped.CurrentPedGroup && (double) LSL.playerPos.DistanceTo(this.Ped.Position) < 100.0)
      {
        Game.Player.Character.CurrentPedGroup.Add(this.Ped, false);
        Function.Call(Hash._0x2E2F4240B3F24647, (InputArgument) this.Ped.Handle, (InputArgument) Game.Player.Character.CurrentPedGroup.Handle, (InputArgument) false);
      }
      if (this.pData.Count <= 0)
        return;
      LsFunctions.SetPedVariation(this.Ped, this.pData);
    }

    private bool isFollowing() => this.NewState == Enums.PdStates.FollowPlayer || this.NewState == Enums.PdStates.FollowingPlayer || (this.CurrentState == Enums.PdStates.FollowPlayer || this.CurrentState == Enums.PdStates.FollowingPlayer);

    private void GiveSavedWeapons()
    {
      LsFunctions.GivePedWeapons(this.Ped, this.dWeapons);
      this.Ped.Weapons.Select(this.Ped.Weapons.BestWeapon);
    }

    public void DespawnPed()
    {
      if (!((Entity) this.Ped != (Entity) null))
        return;
      LsFunctions.NotImmuneToPlayer(this.Ped);
      if (LSL.DEBUG)
        UI.Notify("Dealer DESPAWNED");
      this.Pos = this.Ped.Position;
      this.RemoveBlip();
      this.Ped.Delete();
      this.Ped = (Ped) null;
      this.CreateLocationBlip();
    }

    public void SimulateSales(int _hours)
    {
      int num1 = 0;
      Area area1 = LSL.areas.Find((Predicate<Area>) (a => a.Name == this.area));
      if (area1 != null)
        num1 = area1.Dealers();
      int money = this.Money;
      if (area1 == null)
      {
        if (LSL.zoneData.ContainsKey(this.area))
          LSL.areas.Add(new Area(this.area, LsFunctions.LsDrugs(), LSL.zoneData[this.area].Item1));
        else
          UI.Notify("~r~LsLife - Error getting area type for Area " + this.area);
        if (LSL.DEBUG)
          UI.Notify("~r~Added area for dealer " + this.area);
      }
      if (area1 == null || area1.Drugs.Count <= 0)
        return;
      System.Random random = new System.Random();
      for (int index = 0; index < _hours * 3; ++index)
      {
        foreach (Drug drug in area1.Drugs)
        {
          int num2 = (int) (10.0 * (double) drug.Demand);
          if (num1 > 0)
            num2 = (int) (10.0 * ((double) drug.Demand / (double) num1));
          if ((double) drug.Demand > 1.0)
            num2 = 10;
          if (random.Next(0, 11 - num2) <= 3)
          {
            if (this.Drugs[drug.Name] > 0)
            {
              Area area2 = LSL.areas.Find((Predicate<Area>) (a => a.Name == this.area));
              int maxValue = 7 + (int) (LsFunctions.CurrentRepLvl((double) area2.Reputation) * 0.5);
              if (maxValue <= 1)
                maxValue = 7;
              int num3 = LSL.rnd.Next(1, maxValue);
              if (this.Drugs[drug.Name] < num3)
                num3 = this.Drugs[drug.Name];
              this.Money += num3 * drug.PricePerG;
              area2.Reputation += (float) (this.Money / 100);
              this.Drugs[drug.Name] -= num3;
              drug.Supplied = true;
            }
            else
              drug.Supplied = false;
          }
        }
      }
      if (!LSL.DEBUG)
        return;
      if (money != this.Money)
        UI.Notify("Made $~g~" + (this.Money - money).ToString());
      else
        UI.Notify("Made nothing");
    }

    public void GiveMoneyToPlayer()
    {
      this.Money -= this.Money / 100 * 20;
      Game.Player.Money += this.Money;
      this.Money = 0;
      LsFunctions.PlayTheSound();
    }

    private void PromtPlayerToTalk()
    {
      if (LSL.LsMenuPool.IsAnyMenuOpen() || LsFunctions.zeeClose() || (LSL.jobOffer || LSL.PottentialWorker != null) || (!LSL.player.Character.IsStopped || (double) LSL.playerPos.DistanceTo(this.Ped.Position) >= 2.5 || !LsFunctions.IsCustomerNear()))
        return;
      if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 5000.0)
      {
        Function.Call(Hash._0x6178F68A87A4D3A0);
        LSL.displayHelpTimer = (float) Game.GameTime;
        LsFunctions.DisplayHelpText("Press ~INPUT_CONTEXT~ to Talk to this person.");
      }
      if (!Game.IsControlJustReleased(2, Control.Context))
        return;
      Function.Call(Hash._0x6178F68A87A4D3A0);
      LSL.WorkerMainMenu.CurrentSelection = 3;
      LSL.DealerHandler.currentDealer = this;
      this.SetDrugDesc();
      this.SetDealerOrders();
      this.SetArmourButton();
      if (LSL.WorkerMainMenu.Visible)
        return;
      LSL.WorkerMainMenu.Visible = true;
    }

    private void PromtToFollow()
    {
      if (LSL.LsMenuPool.IsAnyMenuOpen() || !LSL.player.Character.IsStopped || ((double) LSL.playerPos.DistanceTo(this.Ped.Position) >= 10.0 || !LSL.player.Character.IsInVehicle()) || (!(this.Ped.CurrentPedGroup != LSL.player.Character.CurrentPedGroup) || !LsFunctions.IsCustomerNear()))
        return;
      if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 5000.0)
      {
        Function.Call(Hash._0x6178F68A87A4D3A0);
        LSL.displayHelpTimer = (float) Game.GameTime;
        LsFunctions.DisplayHelpText("Press ~INPUT_VEH_HORN~ to make your dealer Follow you");
      }
      if (!Game.IsControlJustPressed(2, Control.VehicleHorn))
        return;
      Function.Call(Hash._0x6178F68A87A4D3A0);
      LSL.DealerHandler.currentDealer = this;
      this.NewState = Enums.PdStates.FollowPlayer;
    }

    private void SetDealerOrders()
    {
      LSL.wOrder.Items = this.Orders();
      LSL.wOrder.Index = 0;
    }

    private void SetArmourButton()
    {
      if (LSL.playerArmours == 0)
        LSL.wArmour.Text = "No armour left";
      else
        LSL.wArmour.Text = "Give armour";
    }

    public void SetDrugDesc()
    {
      LsFunctions.SetWorkerMenuHealth(this.Ped);
      LSL.wWeed.Maximum = LSL.PlayerInventory["Weed"];
      LSL.wCrack.Maximum = LSL.PlayerInventory["Crack"];
      LSL.wCocaine.Maximum = LSL.PlayerInventory["Cocaine"];
      LSL.wWeed.Description = "Has " + LsFunctions.GramsToOz(this.Drugs["Weed"]) + " Weed.";
      LSL.wCrack.Description = "Has " + LsFunctions.GramsToOz(this.Drugs["Crack"]) + " Crack.";
      LSL.wCocaine.Description = "Has " + LsFunctions.GramsToOz(this.Drugs["Cocaine"]) + " Cocaine.";
      int _money = this.Money - this.Money / 100 * 20;
      if (this.Money > 100)
        LSL.wCollect.Description = "Dealer has $" + LsFunctions.IntToMoney(this.Money) + " gets 20% cut, Collect $" + LsFunctions.IntToMoney(_money) + ".";
      else
        LSL.wCollect.Description = "Dealer hasnt made any money yet";
    }

    private bool playerNear() => (double) this.Ped.Position.DistanceTo(LSL.playerPos) <= 5.0;

    private void CreateLocationBlip()
    {
      if (!(this.blip == (Blip) null))
        return;
      this.blip = World.CreateBlip(this.Pos);
      this.blip.IsShortRange = true;
      this.blip.Name = "Dealer Location";
    }

    public void RemoveLocationBlip()
    {
      if (!(this.blip != (Blip) null))
        return;
      this.blip.Remove();
      this.blip = (Blip) null;
    }

    private void SetBlip()
    {
      if (this.NewState == Enums.PdStates.FollowPlayer || this.CurrentState == Enums.PdStates.FollowPlayer)
        this.Ped.CurrentBlip.Color = BlipColor.BlueDark;
      else
        this.Ped.CurrentBlip.Color = BlipColor.Purple;
      this.Ped.CurrentBlip.Name = "Dealer";
      this.Ped.CurrentBlip.Scale = 0.5f;
      this.Ped.CurrentBlip.IsShortRange = true;
    }

    private void RemoveBlip() => this.Ped.CurrentBlip.Remove();

    private void SetStartDrugs()
    {
      this.Drugs.Add("Weed", LSL.rnd.Next(0, 28));
      this.Drugs.Add("Crack", LSL.rnd.Next(0, 28));
      this.Drugs.Add("Cocaine", LSL.rnd.Next(0, 28));
    }
  }
}
