using GTA;
using GTA.Math;
using GTA.Native;
using iFruitAddon2;
using LSlife.OldCustomers;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace LSlife
{
  internal static class LsFunctions
  {
    public const int pMaxDrugs = 7168;
    public static int pWeedOunce = 0;
    public static int pCrackOunce = 0;
    public static int pCocaineOunce = 0;
    public static List<PedHash> cHashes = new List<PedHash>();
    public static List<DealerSpawnPostion> spawnPostions = new List<DealerSpawnPostion>();
    public static List<PedHash> LikleyCustomers = new List<PedHash>();
    public static Dictionary<string, PedHash> PedModels = new Dictionary<string, PedHash>()
    {
      {
        "ZEE",
        PedHash.ArmBoss01GMM
      }
    };
    private static List<int> AllowedInteriors = new List<int>()
    {
      118018,
      271617,
      92674,
      275201,
      233729
    };

    public static List<Drug> LsDrugs()
    {
      List<Drug> drugList = new List<Drug>();
      if (LSL.hardMode)
      {
        drugList.Add(new Drug("Weed", 10, LSL.AreaType.Normal));
        drugList.Add(new Drug("Crack", 20, LSL.AreaType.Poor));
        drugList.Add(new Drug("Cocaine", 100, LSL.AreaType.Rich));
      }
      else
      {
        drugList.Add(new Drug("Weed", 50, LSL.AreaType.Normal));
        drugList.Add(new Drug("Crack", 200, LSL.AreaType.Poor));
        drugList.Add(new Drug("Cocaine", 400, LSL.AreaType.Rich));
      }
      return drugList;
    }

    public static float StringToFloat(string _float)
    {
      float result;
      return float.TryParse(_float, NumberStyles.Float, (IFormatProvider) new CultureInfo("de-DE"), out result) ? result : float.Parse(_float, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static void LoadSpawnLocations()
    {
      foreach (XElement descendant1 in XDocument.Load("scripts\\LSLife\\LSLife_Spawns.xml").Descendants())
      {
        if (descendant1.Name == (XName) "pos")
        {
          Vector3 zero = Vector3.Zero;
          float result = 0.0f;
          foreach (XElement descendant2 in descendant1.Descendants())
          {
            if (descendant2.Name == (XName) "x")
              float.TryParse(descendant2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero.X);
            if (descendant2.Name == (XName) "y")
              float.TryParse(descendant2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero.Y);
            if (descendant2.Name == (XName) "z")
              float.TryParse(descendant2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero.Z);
            if (descendant2.Name == (XName) "h")
              float.TryParse(descendant2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          }
          if (zero != Vector3.Zero)
            LsFunctions.spawnPostions.Add(new DealerSpawnPostion(zero, result));
        }
      }
    }

    public static void LoadZoneData()
    {
      if (File.Exists("scripts\\LSLife\\zoneData\\LsLife_LSSA.xml"))
      {
        int num = 0;
        foreach (XElement descendant in XDocument.Load("scripts\\LSLife\\zoneData\\LsLife_LSSA.xml").Descendants())
        {
          if (descendant.Name == (XName) "area")
          {
            ++num;
            string key = descendant.FirstAttribute.Value;
            LSL.AreaType result1 = LSL.AreaType.Normal;
            LSL.jurisdictionType result2 = LSL.jurisdictionType.LSPD;
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "wealth")
                Enum.TryParse<LSL.AreaType>(element.Value, out result1);
              if (element.Name == (XName) "police")
                Enum.TryParse<LSL.jurisdictionType>(element.Value, out result2);
            }
            Tuple<LSL.AreaType, LSL.jurisdictionType> tuple = new Tuple<LSL.AreaType, LSL.jurisdictionType>(result1, result2);
            LSL.zoneData.Add(key, tuple);
          }
        }
        LsFunctions.DbugString("LosSantos data loaded " + num.ToString() + " areas added");
      }
      else
        UI.Notify("~r~LsLife_LSSA.xml not found");
      if (!LSL.LCRWMAP)
        return;
      if (File.Exists("scripts\\LSLife\\zoneData\\LsLife_LCRW.xml"))
      {
        int num = 0;
        foreach (XElement descendant in XDocument.Load("scripts\\LSLife\\zoneData\\LsLife_LCRW.xml").Descendants())
        {
          if (descendant.Name == (XName) "area")
          {
            ++num;
            string key = descendant.FirstAttribute.Value;
            LSL.AreaType result1 = LSL.AreaType.Normal;
            LSL.jurisdictionType result2 = LSL.jurisdictionType.LSPD;
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "wealth")
                Enum.TryParse<LSL.AreaType>(element.Value, out result1);
              if (element.Name == (XName) "police")
                Enum.TryParse<LSL.jurisdictionType>(element.Value, out result2);
            }
            Tuple<LSL.AreaType, LSL.jurisdictionType> tuple = new Tuple<LSL.AreaType, LSL.jurisdictionType>(result1, result2);
            LSL.zoneData.Add(key, tuple);
          }
        }
        LsFunctions.DbugString("LibertyCity Rewind data loaded " + num.ToString() + " areas added");
      }
      else
        UI.Notify("~r~LsLife_LCRW.xml not found");
    }

    public static int zeeDebtCap()
    {
      int num = 100000;
      if (LSL.hardMode)
        num = 1000;
      return num + (int) LSL.dealer1Rep;
    }

    public static void SaveDealer()
    {
      LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "dealer1")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "weed")
              element.Value = LSL.dealer1Weed.ToString();
            if (element.Name == (XName) "crack")
              element.Value = LSL.dealer1Crack.ToString();
            if (element.Name == (XName) "cocain")
              element.Value = LSL.dealer1Cocain.ToString();
            if (element.Name == (XName) "debt")
              element.Value = LSL.dealer1Debt.ToString();
            if (element.Name == (XName) "money")
              element.Value = LSL.dealer1Money.ToString();
            if (element.Name == (XName) "rep")
              element.Value = LSL.dealer1Rep.ToString();
          }
        }
      }
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
    }

    public static bool IsPlayerArmoursMaxed() => Game.Player.Character.Armor >= 100 && LSL.playerArmours == 5;

    public static int DealerDelay() => 60000 * (10 - LSL.areas[LSL.areaIndex].GangPresance);

    public static void GiveArmourToPlayer()
    {
      if (Game.Player.Character.Armor < 100)
      {
        Game.Player.Character.Armor = 200;
      }
      else
      {
        if (LSL.playerArmours >= 5)
          return;
        ++LSL.playerArmours;
      }
    }

    public static int AmountOfPedsForDriveBy(int _gangStr)
    {
      if (_gangStr <= 1)
        return 1;
      if (_gangStr <= 4)
        return 2;
      return _gangStr <= 7 ? 3 : 4;
    }

    public static int RandomInt(int start, int end) => new System.Random().Next(start, end);

    public static string OrderMenuDesc() => "Weed:~g~" + LSL.NewOrder.weed.ToString() + "~s~oz" + ("\nCrack:~g~" + LSL.NewOrder.crack.ToString() + "~s~oz") + ("\nCoke:~g~" + LSL.NewOrder.cocaine.ToString() + "~s~oz") + ("\nBill:$" + LsFunctions.IntToMoney(LSL.NewOrder.Bill)) + ("\nDebt:$" + LsFunctions.IntToMoney(LSL.dealer1Debt)) + ("\nCredit:$" + LsFunctions.IntToMoney(LsFunctions.zeeDebtCap()));

    public static void UpdateOrderMenuDesc()
    {
      string str = LsFunctions.OrderMenuDesc();
      LSL.PlaceOrder.Description = str;
      LSL.bushWeed.Description = str;
      LSL.crack.Description = str;
      LSL.cocaine.Description = str;
      LSL.WeedBuy.Description = str;
      LSL.CrackBuy.Description = str;
      LSL.CocainBuy.Description = str;
      LSL.LsMenuPool.FormatDescriptions = true;
      LSL.LsMenuPool.RefreshIndex();
    }

    public static bool CanSee(Ped _ped, Ped _targetPed) => Function.Call<bool>(Hash._0x0267D00AF114F17A, (InputArgument) _ped, (InputArgument) _targetPed);

    public static bool CanHearPlayer(Ped _ped) => Function.Call<bool>(Hash._0xF297383AA91DCA29, (InputArgument) Game.Player.Character, (InputArgument) _ped);

    public static bool IsPolice(Ped p)
    {
      bool flag = false;
      foreach (Enums.LawEnforcePed lawEnforcePed in Enum.GetValues(typeof (Enums.LawEnforcePed)))
      {
        if ((Enums.LawEnforcePed) Function.Call<int>(Hash._0xFF059E1E4C01E63C, (InputArgument) p.Handle) == lawEnforcePed)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public static Vector3 RoadSideFromSpawn(Vector3 spawnPos, Vector3 directionPos)
    {
      Vector3 vector3_1 = directionPos - spawnPos;
      vector3_1 = vector3_1.Normalized;
      Vector3 position1 = spawnPos + vector3_1;
      Vector3 vector3_2 = position1;
      Vector3 zero = Vector3.Zero;
      for (int index = 0; index < 16; ++index)
      {
        position1.Z = World.GetGroundHeight(position1);
        if (Function.Call<bool>(Hash._0x125BF4ABFC536B09, (InputArgument) position1.X, (InputArgument) position1.Y, (InputArgument) position1.Z) || (double) position1.DistanceTo(spawnPos) < (double) position1.DistanceTo(directionPos))
        {
          vector3_2 = position1;
          position1 += vector3_1 * 0.5f;
        }
        else
        {
          Vector3 position2 = vector3_2;
          position2.Z = World.GetGroundHeight(position2);
          return position2;
        }
      }
      return zero;
    }

    public static void SetImmuneToPlayer(Ped ped)
    {
    }

    public static void NotImmuneToPlayer(Ped ped)
    {
      Function.Call(Hash._0xE22D8FDE858B8119, (InputArgument) ped, (InputArgument) true, (InputArgument) LSL.playerRelationship);
      ped.CanBeTargetted = true;
      ped.CanBeShotInVehicle = true;
      Function.Call(Hash._0x66B57B72E0836A76, (InputArgument) ped, (InputArgument) LSL.player, (InputArgument) true);
      Function.Call(Hash._0x63F58F7C80513AAD, (InputArgument) ped, (InputArgument) true);
    }

    public static Vector2 World3DToScreen2d(Vector3 pos)
    {
      OutputArgument outputArgument1 = new OutputArgument();
      OutputArgument outputArgument2 = new OutputArgument();
      Function.Call(Hash._0x34E82F05DF2974F5, (InputArgument) pos.X, (InputArgument) pos.Y, (InputArgument) pos.Z, (InputArgument) outputArgument1, (InputArgument) outputArgument2);
      return new Vector2(outputArgument1.GetResult<float>(), outputArgument2.GetResult<float>());
    }

    public static float Lerp(float start, float end, float time) => (float) ((1.0 - (double) time) * (double) start + (double) time * (double) end);

    public static bool ShouldSpeed(Vector3 vPos) => (double) LsFunctions.GetClosestRoadPos(vPos).DistanceTo(vPos) > 2.40000009536743;

    public static float GetRoadSpeed(Vector3 pos, bool canSpeed, bool _shouldSpeed)
    {
      List<LSL.PathnodeFlags> roadFlags = LSL.GetRoadFlags(pos);
      bool flag = false;
      float num = 8f;
      if (canSpeed)
        flag = _shouldSpeed;
      foreach (LSL.PathnodeFlags pathnodeFlags in roadFlags)
      {
        switch (pathnodeFlags)
        {
          case LSL.PathnodeFlags.Slow:
            num = 9f;
            if (flag)
            {
              num = 30f;
              continue;
            }
            continue;
          case LSL.PathnodeFlags.Two:
            num = 16f;
            if (flag)
            {
              num = 35f;
              continue;
            }
            continue;
          case LSL.PathnodeFlags.Intersection:
            if (flag)
              num = 13f;
            num = 11f;
            continue;
          case LSL.PathnodeFlags.Eight:
            num = 16f;
            continue;
          case LSL.PathnodeFlags.SlowTraffic:
            num = 14f;
            if (flag)
            {
              num = 16f;
              continue;
            }
            continue;
          case LSL.PathnodeFlags.Freeway:
            num = 35f;
            if (flag)
            {
              num = 50f;
              continue;
            }
            continue;
          case LSL.PathnodeFlags.FourWayIntersection:
            num = 11f;
            if (flag)
            {
              num = 13f;
              continue;
            }
            continue;
          default:
            num = 8f;
            if (flag)
            {
              num = 13f;
              continue;
            }
            continue;
        }
      }
      if (Function.Call<bool>(Hash._0x4F5070AA58F69279, (InputArgument) Function.Call<int>(Hash._0x22D7275A79FE8215, (InputArgument) pos.X, (InputArgument) pos.Y, (InputArgument) pos.Z, (InputArgument) 1, (InputArgument) 1, (InputArgument) 200f, (InputArgument) 200f)))
      {
        if ((double) num * 0.5 > 9.0 & flag)
          num *= 0.5f;
        else
          num = 9f;
      }
      return num;
    }

    public static Vector3 GetClosestRoadPos(Vector3 pos)
    {
      OutputArgument outputArgument = new OutputArgument();
      return Function.Call<bool>(Hash._0x240A18690AE96513, (InputArgument) pos.X, (InputArgument) pos.Y, (InputArgument) pos.Z, (InputArgument) outputArgument, (InputArgument) 1, (InputArgument) 3f, (InputArgument) 0) ? outputArgument.GetResult<Vector3>() : pos;
    }

    public static void SetWorkerMenuHealth(Ped p)
    {
      UIResText subtitle = LSL.WorkerMainMenu.Subtitle;
      int num = p.Health;
      string str1 = num.ToString();
      num = p.Armor;
      string str2 = num.ToString();
      string str3 = "Health:" + str1 + " Armour:" + str2;
      subtitle.Caption = str3;
    }

    public static void SetRandomCombatStats(Ped p)
    {
      int num = LSL.areas[LSL.areaIndex].GangPresance;
      if (num < 0)
        num = 0;
      p.Armor = LSL.rnd.Next(0, 1 + num * 10);
      p.ShootRate = LSL.rnd.Next(0, 1 + num * 10);
      p.Accuracy = LSL.rnd.Next(0, 1 + num * 10);
    }

    public static void DoGangGrowth()
    {
      if (LSL.areas.Count <= 0)
        return;
      foreach (Area area in LSL.areas)
      {
        int maxValue = 20;
        if (area.Name == World.GetZoneNameLabel(LSL.HouseHandler.Houses[0].entrancePos))
        {
          maxValue *= 2;
          area.GangPresance = 0;
        }
        if (new System.Random().Next(0, maxValue) == 0 && area.GangPresance < 10 && !area.DemandMet())
          ++area.GangPresance;
      }
    }

    public static void SendTextMsg(string str)
    {
      Game.PlaySound("Text_Arrive_Tone", "Phone_SoundSet_Default");
      UI.Notify(str);
    }

    public static void TextMsg(string _name, string _title, string _msg)
    {
      if (!HeadShotHandler.headShots.ContainsKey(_name))
      {
        Ped ped = World.CreatePed(LsFunctions.RequestModel(LsFunctions.PedModels[_name]), new Vector3(0.0f, 0.0f, 0.0f));
        while ((Entity) ped == (Entity) null)
          Script.Yield();
        HeadShotHandler.MugShot(ped, _name);
        ped.Delete();
      }
      Game.PlaySound("Text_Arrive_Tone", "Phone_SoundSet_Default");
      Function.Call(Hash._0x202709F4C58A0424, (InputArgument) "CELL_EMAIL_BCON");
      Function.Call(Hash._0x6C188BE134E074AA, (InputArgument) _msg);
      Function.Call(Hash._0x1CCD9A37359072CF, (InputArgument) HeadShotHandler.headShots[_name].Texture, (InputArgument) HeadShotHandler.headShots[_name].Texture, (InputArgument) true, (InputArgument) 1, (InputArgument) _title, (InputArgument) _name);
      Script.Yield();
    }

    public static void TextMsg(string _name, Ped _ped, string _title, string _msg)
    {
      if (!HeadShotHandler.headShots.ContainsKey(_name))
        HeadShotHandler.MugShot(_ped, _name);
      else if (HeadShotHandler.headShots.Where<KeyValuePair<string, HeadShot>>((Func<KeyValuePair<string, HeadShot>, bool>) (r => r.Value.PedHash == (PedHash) _ped.Model.Hash)).Count<KeyValuePair<string, HeadShot>>() == 0)
      {
        HeadShotHandler.DeleteHeadShot(_name);
        HeadShotHandler.MugShot(_ped, _name);
      }
      Game.PlaySound("Text_Arrive_Tone", "Phone_SoundSet_Default");
      Function.Call(Hash._0x202709F4C58A0424, (InputArgument) "CELL_EMAIL_BCON");
      Function.Call(Hash._0x6C188BE134E074AA, (InputArgument) _msg);
      Function.Call(Hash._0x1CCD9A37359072CF, (InputArgument) HeadShotHandler.headShots[_name].Texture, (InputArgument) HeadShotHandler.headShots[_name].Texture, (InputArgument) true, (InputArgument) 1, (InputArgument) _title, (InputArgument) _name);
      Script.Yield();
    }

    public static int TotalDrugs(Dictionary<string, int> _drugs)
    {
      int num = 0;
      foreach (KeyValuePair<string, int> drug in _drugs)
        num += _drugs[drug.Key];
      return num + (LsFunctions.pWeedOunce * 28 + LsFunctions.pCrackOunce * 28 + LsFunctions.pCocaineOunce * 28);
    }

    public static void PedBag(Ped ped, bool give)
    {
      if (give)
        Function.Call(Hash._0x262B14F48D29DE80, (InputArgument) ped, (InputArgument) 9, (InputArgument) 1, (InputArgument) 0, (InputArgument) 0);
      else
        Function.Call(Hash._0x262B14F48D29DE80, (InputArgument) ped, (InputArgument) 9, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0);
    }

    public static void DisplayHelpText(string text)
    {
      Function.Call(Hash._0x8509B634FBE7DA11, (InputArgument) "STRING");
      Function.Call(Hash._0x6C188BE134E074AA, (InputArgument) text);
      Function.Call(Hash._0x238FFE5C7B0498A6, (InputArgument) 0, (InputArgument) 0, (InputArgument) 1, (InputArgument) -1);
    }

    public static int Discount(int price)
    {
      int num = 0;
      if (LsFunctions.CurrentRepLvl(LSL.dealer1Rep) >= 1.0)
        num = price / 100 * (int) LsFunctions.CurrentRepLvl(LSL.dealer1Rep);
      return num;
    }

    public static double CurrentRepLvl(double rep) => rep < 0.0 ? 0.0 : 0.0399999991059303 * System.Math.Sqrt(rep);

    public static void AddBlip(
      Entity _ent,
      BlipColor _col,
      string _name,
      bool _friend,
      bool _edge)
    {
      if (!_ent.CurrentBlip.Exists())
        _ent.AddBlip();
      Blip currentBlip = _ent.CurrentBlip;
      currentBlip.IsFriendly = _friend;
      currentBlip.IsShortRange = _edge;
      currentBlip.Color = _col;
      currentBlip.Name = _name;
    }

    public static void RemovePedFromWorld(Ped ped, bool clearTask)
    {
      if ((Entity) ped.LastVehicle == (Entity) LSL.player.Character.LastVehicle)
        Function.Call(Hash._0xBB8DE8CF6A8DD8BB, (InputArgument) ped);
      ped.CurrentBlip?.Remove();
      ped.LeaveGroup();
      if (clearTask)
        ped.Task.ClearAll();
      ped.MarkAsNoLongerNeeded();
      ped.BlockPermanentEvents = false;
      ped.IsPersistent = false;
      LSL.entitiesCleanUp.Add((Entity) ped);
    }

    public static void PlayTheSound() => Game.PlaySound("LOCAL_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");

    public static bool IsInInterior(Entity _entity)
    {
      int num = Function.Call<int>(Hash._0x2107BA504071A6BB, (InputArgument) _entity);
      foreach (int allowedInterior in LsFunctions.AllowedInteriors)
      {
        if (allowedInterior == num)
          return false;
      }
      return num != 0;
    }

    public static bool IsUnderGround(Entity _entity)
    {
      Vector3 position = _entity.Position;
      float z = position.Z;
      return (double) World.GetGroundHeight(position) > (double) z;
    }

    public static void SpawnDriveBy(Ped _target, bool _agressive)
    {
      if (_agressive)
      {
        int num = LSL.areas[LSL.areaIndex].GangPresance;
        if (!LSL.hardMode)
          num = (int) ((double) num * 0.5);
        if (num < 1)
          num = 1;
        for (int index = 0; index < num; ++index)
          LSL.driveByHandler.AddDriveBy(VehicleClass.SUVs, LSL.rnd.Next(0, LsFunctions.AmountOfPedsForDriveBy(LSL.areas[LSL.areaIndex].GangPresance)), _target, _agressive);
      }
      else
        LSL.driveByHandler.AddDriveBy(VehicleClass.SUVs, LSL.rnd.Next(0, LsFunctions.AmountOfPedsForDriveBy(LSL.areas[LSL.areaIndex].GangPresance)), _target, _agressive);
    }

    public static void UpdateSave()
    {
      bool flag = false;
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "dealer1")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "debt" && element.Value != LSL.dealer1Debt.ToString())
            {
              element.Value = LSL.dealer1Debt.ToString();
              flag = true;
            }
            if (element.Name == (XName) "weed" && element.Value != LSL.dealer1Weed.ToString())
            {
              element.Value = LSL.dealer1Weed.ToString();
              flag = true;
            }
            if (element.Name == (XName) "crack" && element.Value != LSL.dealer1Crack.ToString())
            {
              element.Value = LSL.dealer1Crack.ToString();
              flag = true;
            }
            if (element.Name == (XName) "cocain" && element.Value != LSL.dealer1Cocain.ToString())
            {
              element.Value = LSL.dealer1Cocain.ToString();
              flag = true;
            }
            if (element.Name == (XName) "rep" && element.Value != LSL.dealer1Rep.ToString())
            {
              element.Value = LSL.dealer1Rep.ToString();
              flag = true;
            }
            if (element.Name == (XName) "money" && element.Value != LSL.dealer1Money.ToString())
            {
              element.Value = LSL.dealer1Money.ToString();
              flag = true;
            }
          }
        }
      }
      if (!flag)
        return;
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      if (!LSL.DEBUG)
        return;
      UI.Notify("Save Changed");
    }

    public static List<XElement> PedWeapons(Ped _ped)
    {
      List<XElement> xelementList = new List<XElement>();
      foreach (WeaponHash weaponHash in Enum.GetValues(typeof (WeaponHash)))
      {
        if (Function.Call<bool>(Hash._0x8DECB02F88F428BC, (InputArgument) _ped, (InputArgument) (int) weaponHash))
        {
          int num = Function.Call<int>(Hash._0x015A522136D7F951, (InputArgument) _ped, (InputArgument) (int) weaponHash);
          if (num < 500)
            num = 500;
          XElement xelement1 = new XElement((XName) "weapon");
          XElement xelement2 = new XElement((XName) "HASH", (object) weaponHash);
          XElement xelement3 = new XElement((XName) "AMMO", (object) num);
          xelement1.Add((object) xelement2);
          xelement1.Add((object) xelement3);
          foreach (WeaponComponent weaponComponent in Enum.GetValues(typeof (WeaponComponent)))
          {
            if (Function.Call<bool>(Hash._0xC593212475FAE340, (InputArgument) _ped, (InputArgument) (int) weaponHash, (InputArgument) (int) weaponComponent))
            {
              XElement xelement4 = new XElement((XName) "COMPONENT", (object) weaponComponent);
              xelement1.Add((object) xelement4);
            }
          }
          xelementList.Add(xelement1);
        }
      }
      return xelementList;
    }

    public static void GivePedWeapons(Ped _ped, List<XElement> _weapons)
    {
      if (_weapons.Count <= 0)
        return;
      WeaponHash hash = _ped.Weapons.Current.Hash;
      foreach (XElement weapon in _weapons)
      {
        bool flag = false;
        WeaponHash result1 = WeaponHash.Unarmed;
        foreach (XElement element in weapon.Elements())
        {
          if (element.Name == (XName) "HASH")
            Enum.TryParse<WeaponHash>(element.Value, out result1);
          if (element.Name == (XName) "AMMO")
          {
            int result2;
            int.TryParse(element.Value, out result2);
            if (result2 < 500)
              result2 = 500;
            if (!_ped.Weapons.HasWeapon(result1))
              _ped?.Weapons.Give(result1, result2, true, true);
            else
              flag = true;
            _ped?.Weapons.Select(result1);
            _ped.Weapons.Current.Ammo += result2;
            if (LSL.DEBUG)
              UI.Notify("Gave ped" + result1.ToString());
          }
          WeaponComponent result3;
          if (element.Name == (XName) "COMPONENT" && !flag && (Enum.TryParse<WeaponComponent>(element.Value, out result3) && _ped != null))
            _ped.Weapons.Current.SetComponent(result3, true);
        }
      }
      _ped.Weapons.Select(hash);
    }

    public static void UpdateInventory()
    {
      bool flag = false;
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "playerInventory")
        {
          using (IEnumerator<XElement> enumerator = descendant.Elements().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              XElement current = enumerator.Current;
              int num;
              if (current.Name == (XName) "weed")
              {
                string str1 = current.Value;
                num = LSL.PlayerInventory["Weed"];
                string str2 = num.ToString();
                if (str1 != str2)
                {
                  XElement xelement = current;
                  num = LSL.PlayerInventory["Weed"];
                  string str3 = num.ToString();
                  xelement.Value = str3;
                  flag = true;
                }
              }
              else if (current.Name == (XName) "crack")
              {
                string str1 = current.Value;
                num = LSL.PlayerInventory["Crack"];
                string str2 = num.ToString();
                if (str1 != str2)
                {
                  XElement xelement = current;
                  num = LSL.PlayerInventory["Crack"];
                  string str3 = num.ToString();
                  xelement.Value = str3;
                  flag = true;
                }
              }
              else if (current.Name == (XName) "cocain")
              {
                string str1 = current.Value;
                num = LSL.PlayerInventory["Cocaine"];
                string str2 = num.ToString();
                if (str1 != str2)
                {
                  XElement xelement = current;
                  num = LSL.PlayerInventory["Cocaine"];
                  string str3 = num.ToString();
                  xelement.Value = str3;
                  flag = true;
                }
              }
              else if (current.Name == (XName) "weedOz")
              {
                if (current.Value != LsFunctions.pWeedOunce.ToString())
                {
                  current.Value = LsFunctions.pWeedOunce.ToString();
                  flag = true;
                }
              }
              else if (current.Name == (XName) "crackOz")
              {
                if (current.Value != LsFunctions.pCrackOunce.ToString())
                {
                  current.Value = LsFunctions.pCrackOunce.ToString();
                  flag = true;
                }
              }
              else if (current.Name == (XName) "cocaineOz")
              {
                if (current.Value != LsFunctions.pCocaineOunce.ToString())
                {
                  current.Value = LsFunctions.pCocaineOunce.ToString();
                  flag = true;
                }
              }
              else if (current.Name == (XName) "armour" && current.Value != LSL.playerArmours.ToString())
              {
                current.Value = LSL.playerArmours.ToString();
                flag = true;
              }
            }
            break;
          }
        }
      }
      if (!flag)
        return;
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
    }

    public static void DateTimeUpdate()
    {
      if (Function.Call<int>(Hash._0x25223CA6B4D20B7F) != LSL.hour)
      {
        LSL.hour = Function.Call<int>(Hash._0x25223CA6B4D20B7F);
        if ((LSL.dealer1Weed == 0 || LSL.dealer1Crack == 0 || LSL.dealer1Cocain == 0) && (!LSL.dealer1ReloadOrder && !LSL.dealer1Reloaded))
        {
          LSL.dealer1ReloadOrder = true;
          int num1 = 20;
          int num2 = num1 - (int) LsFunctions.CurrentRepLvl(LSL.dealer1Rep) / 2 > 5 ? num1 - (int) LsFunctions.CurrentRepLvl(LSL.dealer1Rep) / 2 : 5;
          int num3;
          bool flag;
          if (LSL.hour + num2 > 24)
          {
            num3 = LSL.hour + num2 - 24;
            flag = true;
          }
          else
          {
            num3 = LSL.hour + num2;
            flag = false;
          }
          LSL.dealer1ReloadHour = num3;
          LSL.dealer1ReloadDay = !flag ? LSL.day : (LSL.day != 6 ? LSL.day + 1 : 0);
          Enums.DayGTA result;
          Enum.TryParse<Enums.DayGTA>(LSL.dealer1ReloadDay.ToString(), out result);
          if (LSL.DEBUG)
            UI.Notify(LSL.dealer1ReloadHour.ToString() ?? "");
          string str = "AM";
          if (num3 > 12)
          {
            num3 -= 12;
            str = "PM";
          }
          LsFunctions.TextMsg("ZEE", "Reload", "order in, should come " + result.ToString() + " at " + num3.ToString() + ":00" + str);
        }
        int num4 = LsFunctions.CurrentRepLvl(LSL.dealer1Rep) <= 36.0 ? (int) LsFunctions.CurrentRepLvl(LSL.dealer1Rep) * 10000 : 360000;
        if (!LSL.dealer1ReloadOrder && LSL.dealer1Reloaded && Game.GameTime > LSL.reloadTimer + (LSL.reloadTimerInterval - num4))
        {
          LSL.dealer1Reloaded = false;
          foreach (XElement descendant in LSL.LSLifeSave.Descendants())
          {
            if (descendant.Name == (XName) "dealer1")
            {
              foreach (XElement element in descendant.Elements())
              {
                if (element.Name == (XName) "reloaded")
                {
                  element.Value = LSL.dealer1Reloaded.ToString();
                  LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
                  break;
                }
              }
            }
          }
        }
        int num5 = LSL.dealer1ReloadDay + 1 != 7 ? LSL.dealer1ReloadDay + 1 : 0;
        if (LSL.day == LSL.dealer1ReloadDay && !LSL.dealer1Reloaded && (LSL.dealer1ReloadOrder && LSL.hour >= LSL.dealer1ReloadHour))
        {
          LSL.dealer1ReloadOrder = false;
          LSL.dealer1Reloaded = true;
          LSL.reloadTimer = Game.GameTime;
          if (LSL.dealer1Debt > 0)
          {
            float num1 = 0.05f;
            float num2 = (float) LSL.dealer1Debt * num1;
            if ((double) num2 < 0.0)
              num2 = 0.0f;
            LSL.dealer1Debt += (int) num2;
            if (LSL.dealer1Debt < 0)
              LSL.dealer1Debt = 0;
            LsFunctions.TextMsg("ZEE", "Debt", "added $" + LsFunctions.IntToMoney((int) num2) + " to your bill, its now $" + LsFunctions.IntToMoney(LSL.dealer1Debt));
          }
          if (LSL.hardMode)
          {
            if (LSL.dealer1Money < 1000)
              LSL.dealer1Money = 5000;
          }
          else if (LSL.dealer1Money < 10000)
            LSL.dealer1Money = 50000;
          float num3 = 0.0f;
          float num6 = 0.0f;
          float num7 = 0.0f;
          if (LSL.dealer1Weed < 40)
          {
            num3 = (float) LSL.dealer1Money * 0.75f;
            LSL.dealer1Money -= (int) num3;
          }
          if (LSL.dealer1Crack < 40)
          {
            num6 = (float) LSL.dealer1Money * 0.6f;
            LSL.dealer1Money -= (int) num6;
          }
          if (LSL.dealer1Cocain < 40)
          {
            num7 = (float) LSL.dealer1Money;
            LSL.dealer1Money -= (int) num7;
          }
          int num8 = LSL.weed1Price / 2 - LsFunctions.Discount(LSL.weed1Price / 2);
          int num9 = LSL.crack1Price / 2 - LsFunctions.Discount(LSL.crack1Price / 2);
          int num10 = LSL.cocain1Price / 2 - LsFunctions.Discount(LSL.cocain1Price / 2);
          int num11 = (int) num3 / num8;
          int num12 = (int) num6 / num9;
          int num13 = (int) num7 / num10;
          LSL.dealer1Weed += num11;
          LSL.dealer1Crack += num12;
          LSL.dealer1Cocain += num13;
          float num14 = num3 - (float) (num11 * num8);
          float num15 = num6 - (float) (num12 * num9);
          float num16 = num7 - (float) (num13 * num10);
          LSL.dealer1Money += (int) num14 + (int) num15 + (int) num16;
          LsFunctions.TextMsg("ZEE", "Reload", "Weed:" + num11.ToString() + "oz, Crack:" + num12.ToString() + "oz, Cocaine:" + num13.ToString() + "oz.");
          LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
          LsFunctions.UpdateSave();
        }
        if (LSL.day == num5 && !LSL.dealer1Reloaded && LSL.dealer1ReloadOrder)
        {
          LSL.dealer1ReloadOrder = false;
          LSL.dealer1Reloaded = true;
          LSL.reloadTimer = Game.GameTime;
          if (LSL.dealer1Debt > 0)
          {
            float num1 = 0.05f;
            float num2 = (float) LSL.dealer1Debt * num1;
            LSL.dealer1Debt += (int) num2;
            if (LSL.dealer1Debt < 0)
              LSL.dealer1Debt = 0;
            LsFunctions.TextMsg("ZEE", "Debt", "added $" + LsFunctions.IntToMoney((int) num2) + " to your bill, its now $" + LsFunctions.IntToMoney(LSL.dealer1Debt));
          }
          if (LSL.dealer1Money < 10000)
            LSL.dealer1Money = 50000;
          float num3 = 0.0f;
          float num6 = 0.0f;
          float num7 = 0.0f;
          if (LSL.dealer1Weed < 40)
          {
            num3 = (float) LSL.dealer1Money * 0.75f;
            LSL.dealer1Money -= (int) num3;
          }
          if (LSL.dealer1Crack < 40)
          {
            num6 = (float) LSL.dealer1Money * 0.6f;
            LSL.dealer1Money -= (int) num6;
          }
          if (LSL.dealer1Cocain < 40)
          {
            num7 = (float) LSL.dealer1Money;
            LSL.dealer1Money -= (int) num7;
          }
          int num8 = LSL.weed1Price / 2 - LsFunctions.Discount(LSL.weed1Price / 2);
          int num9 = LSL.crack1Price / 2 - LsFunctions.Discount(LSL.crack1Price / 2);
          int num10 = LSL.cocain1Price / 2 - LsFunctions.Discount(LSL.cocain1Price / 2);
          int num11 = (int) num3 / num8;
          int num12 = (int) num6 / num9;
          int num13 = (int) num7 / num10;
          LSL.dealer1Weed += num11;
          LSL.dealer1Crack += num12;
          LSL.dealer1Cocain += num13;
          float num14 = num3 - (float) (num11 * num8);
          float num15 = num6 - (float) (num12 * num9);
          float num16 = num7 - (float) (num13 * num10);
          LSL.dealer1Money += (int) num14 + (int) num15 + (int) num16;
          LsFunctions.TextMsg("ZEE", "Reload", "Weed:" + num11.ToString() + "oz, Crack:" + num12.ToString() + "oz, Cocaine:" + num13.ToString() + "oz.");
          if (LSL.DEBUG)
            UI.Notify("Weed:" + num11.ToString() + " Crack:" + num12.ToString() + " Cocaine:" + num13.ToString());
          LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
          LsFunctions.UpdateSave();
        }
        if ((LSL.hour == 12 || LSL.hour == 0 || (LSL.hour == 6 || LSL.hour == 18)) && LSL.areas.Count > 0)
        {
          foreach (Area area in LSL.areas)
          {
            if (area.Name == LSL.areas[LSL.areaIndex].Name)
            {
              if (!LSL.playerWanted)
                area.HeatDecay();
            }
            else
              area.HeatDecay();
          }
          LsFunctions.DoGangGrowth();
          LsFunctions.SaveAreaData();
        }
      }
      if (Function.Call<int>(Hash._0xD972E4BD7AEB235F) == LSL.day)
        return;
      LSL.day = Function.Call<int>(Hash._0xD972E4BD7AEB235F);
      LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
      if (LSL.dealer1Money > 1200000)
        LSL.dealer1Money = 1200000;
      if (LSL.dealer1Weed > 0)
      {
        if (LSL.dealer1Weed == 1)
        {
          LSL.dealer1Weed = 0;
          LSL.dealer1Money += LSL.weed1Price / 2 + LsFunctions.Discount(LSL.weed1Price / 2);
        }
        else
        {
          LSL.dealer1Weed -= LSL.dealer1Weed / 2;
          LSL.dealer1Money += (LSL.weed1Price / 2 + LsFunctions.Discount(LSL.weed1Price / 2)) * LSL.dealer1Weed;
        }
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "dealer1")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weed")
                element.Value = LSL.dealer1Weed.ToString();
              if (element.Name == (XName) "money")
                element.Value = LSL.dealer1Money.ToString();
            }
          }
        }
      }
      if (LSL.dealer1Crack > 0)
      {
        if (LSL.dealer1Crack == 1)
        {
          LSL.dealer1Crack = 0;
          LSL.dealer1Money += LSL.crack1Price / 2 + LsFunctions.Discount(LSL.crack1Price / 2);
        }
        else
        {
          LSL.dealer1Crack -= LSL.dealer1Crack / 2;
          LSL.dealer1Money += (LSL.crack1Price / 2 + LsFunctions.Discount(LSL.crack1Price / 2)) * LSL.dealer1Crack;
        }
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "dealer1")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "crack")
                element.Value = LSL.dealer1Crack.ToString();
              if (element.Name == (XName) "money")
                element.Value = LSL.dealer1Money.ToString();
            }
          }
        }
      }
      if (LSL.dealer1Cocain > 0)
      {
        if (LSL.dealer1Cocain == 1)
        {
          LSL.dealer1Cocain = 0;
          LSL.dealer1Money += LSL.cocain1Price / 2 + LsFunctions.Discount(LSL.cocain1Price / 2);
        }
        else
        {
          LSL.dealer1Cocain -= LSL.dealer1Cocain / 2;
          LSL.dealer1Money += (LSL.cocain1Price / 2 + LsFunctions.Discount(LSL.cocain1Price / 2)) * LSL.dealer1Cocain;
        }
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "dealer1")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "cocain")
                element.Value = LSL.dealer1Cocain.ToString();
              if (element.Name == (XName) "money")
                element.Value = LSL.dealer1Money.ToString();
            }
          }
        }
      }
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      LsFunctions.UpdateSave();
    }

    private static void SaveAreaData()
    {
      XDocument xdocument;
      if (File.Exists("scripts\\LSLife\\LSLifeAreas.xml"))
        xdocument = XDocument.Load("scripts\\LSLife\\LSLifeAreas.xml");
      else
        xdocument = new XDocument(new object[1]
        {
          (object) new XElement((XName) "areas")
        });
      xdocument.Root.RemoveAll();
      foreach (Area area in LSL.areas)
      {
        XElement xelement1 = new XElement((XName) "area");
        XElement xelement2 = new XElement((XName) "name", (object) area.Name);
        XElement xelement3 = new XElement((XName) "cStr", (object) area.CopPresance);
        XElement xelement4 = new XElement((XName) "gStr", (object) area.GangPresance);
        XElement xelement5 = new XElement((XName) "aHeat", (object) area.Heat);
        XElement xelement6 = new XElement((XName) "aRep", (object) area.Reputation);
        xelement1.Add((object) xelement2);
        xelement1.Add((object) xelement3);
        xelement1.Add((object) xelement4);
        xelement1.Add((object) xelement5);
        xelement1.Add((object) xelement6);
        xdocument.Root.Add((object) xelement1);
      }
      xdocument.Save("scripts\\LSLife\\LSLifeAreas.xml");
    }

    public static Vector3 WeedPlaceLocation() => new Vector3(610.726f, -3059.598f, 6.258467f);

    public static Prop LoadWeedPlace()
    {
      int num = 464151082;
      Prop prop1 = (Prop) null;
      Prop[] nearbyProps = World.GetNearbyProps(Game.Player.Character.Position, 50f);
      Vector3 position = LsFunctions.WeedPlaceLocation();
      if (((IEnumerable<Prop>) nearbyProps).Count<Prop>() > 0)
      {
        foreach (Prop prop2 in nearbyProps)
        {
          if (prop2.Model.Hash == num)
          {
            prop1 = prop2;
            prop2.FreezePosition = true;
            if (prop2.Position != position)
              prop2.Position = position;
            if (LSL.DEBUG)
            {
              UI.Notify("Door found");
              break;
            }
            break;
          }
        }
      }
      if ((Entity) prop1 == (Entity) null)
      {
        prop1 = World.CreateProp((Model) num, position, new Vector3(0.0f, 0.0f, 0.0f), false, false);
        if (LSL.DEBUG)
          UI.Notify("Didnt find a door spawning one");
      }
      return prop1;
    }

    public static Vector3 AdjustToGround(Vector3 _vector3)
    {
      Vector3 position = _vector3;
      position.Z = World.GetGroundHeight(position);
      return position;
    }

    public static void ContactAnswered(iFruitContact contact)
    {
      if (contact.Name == "Zee")
      {
        if (LSL.isDealing)
        {
          LSL.isDealing = false;
          LsFunctions.SendTextMsg("Street dealing cancel");
        }
        if (LSL.LsMenuPool != null)
        {
          LSL.zeeMenu.Visible = !LSL.zeeMenu.Visible;
          LsFunctions.UpdateDealerInventory();
          LSL.zeeMenu.CurrentSelection = 0;
        }
      }
      LSL.iFruit.Close(1000);
    }

    private static void UpdateDealerInventory()
    {
      LSL.zeeMenu.Subtitle.Caption = "Rep:" + LsFunctions.IntToMoney((int) LSL.dealer1Rep) + " Level:~g~" + ((int) LsFunctions.CurrentRepLvl(LSL.dealer1Rep)).ToString();
      LSL.payDebt.Text = "Pay Debt $" + LsFunctions.IntToMoney(LSL.dealer1Debt);
      LSL.bushWeed.Maximum = LSL.dealer1Weed;
      LSL.bushWeed.Value = 0;
      LSL.crack.Maximum = LSL.dealer1Crack;
      LSL.crack.Value = 0;
      LSL.cocaine.Maximum = LSL.dealer1Cocain;
      LSL.cocaine.Value = 0;
    }

    public static void MovePed(Ped p, Vector3 target)
    {
      if ((double) p.Position.DistanceTo(target) <= 20.0)
      {
        if (!Function.Call<bool>(Hash._0x125BF4ABFC536B09, (InputArgument) target.X, (InputArgument) target.Y, (InputArgument) target.Z))
        {
          p.Task.GoTo(target);
          return;
        }
      }
      p.Task.RunTo(target);
    }

    public static void ChangeAdealerToPdealer()
    {
      if (LSL.aDealer == null || !((Entity) LSL.aDealer.dPed != (Entity) null))
        return;
      LSL.DealerHandler.AddDealer(new PlayerDealer(LSL.aDealer.dPed));
      LSL.aDealer = (AreaDealer) null;
      LSL.areas[LSL.areaIndex].KillDealer();
    }

    public static int GetLargeStreetAmount()
    {
      List<int> intList = new List<int>() { 7, 14, 28 };
      int num1 = intList[LSL.rnd.Next(0, intList.Count)];
      int num2 = (int) LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation);
      if (num2 > 0)
      {
        int num3 = 1;
        for (int index = 0; index < num2; ++index)
        {
          if (LSL.rnd.Next(0, 10) == 0)
            ++num3;
        }
        num1 *= num3;
      }
      return num1;
    }

    public static void MakeStreetSale(Customer c, bool inCar)
    {
      List<Ped> pedList = new List<Ped>();
      Ped ped1 = c.Ped;
      int num1 = c.amount * c.Drug.PricePerG;
      float num2 = (float) c.amount / 7f;
      if ((double) num2 >= 1.0)
        num1 = c.amount * c.Drug.PricePerG - (int) num2 * c.Drug.PricePerG;
      LSL.PedsCanSeeDeal = 0;
      float num3 = (float) num1 * 0.01f;
      LSL.areas[LSL.areaIndex].Reputation += num3;
      foreach (Ped ped2 in LSL.lPeds.GetPeds.Where<Ped>((Func<Ped, bool>) (r => r.IsAlive)))
      {
        Ped ped = ped2;
        if (LsFunctions.IsPolice(ped))
        {
          pedList.Add(ped);
          ped.Task.LookAt((Entity) Game.Player.Character, 10000);
        }
        if (!ped.IsPlayer && LSL.customers.FindAll((Predicate<Customer>) (s => (Entity) s.Ped == (Entity) ped)).Count == 0 && (!OldCustomerHandler.IsPedOldCustomer(ped) && LsFunctions.CanSee(ped, Game.Player.Character)))
          ++LSL.PedsCanSeeDeal;
      }
      if (!inCar)
      {
        Game.Player.Character.Task.TurnTo((Entity) ped1);
        Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) ped1, (InputArgument) "GENERIC_HOWS_IT_GOING", (InputArgument) "SPEECH_PARAMS_STANDARD", (InputArgument) 0);
        Script.Wait(1000);
        ped1.Task.PlayAnimation("switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_franklin", 8f, 3000, AnimationFlags.UpperBodyOnly);
        Game.Player.Character.Task.PlayAnimation("switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_shopkeeper", 8f, 3000, AnimationFlags.UpperBodyOnly);
        Script.Wait(2000);
        Game.Player.Money += num1;
        c.Ped.Money -= num1;
        LSL.PlayerInventory[c.Drug.Name] -= c.amount;
        LsFunctions.PlayTheSound();
        UI.Notify("Sold " + c.Drug.Name + " " + LsFunctions.GramsToOz(c.amount) + ", you got " + LsFunctions.GramsToOz(LSL.PlayerInventory[c.Drug.Name]) + " left. Gained ~g~" + num3.ToString() + " ~s~rep, total rep ~g~" + LSL.areas[LSL.areaIndex].Reputation.ToString() + "~s~ in " + LSL.areas[LSL.areaIndex].Name);
        Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) ped1, (InputArgument) "GENERIC_THANKS", (InputArgument) "SPEECH_PARAMS_STANDARD", (InputArgument) 0);
        if ((Entity) c.Vehicle != (Entity) null && c.Vehicle.Exists())
          c.Ped.Task.PerformSequence(LSL.PerformTaskSeq(1, c.Vehicle, Vector3.Zero));
        if (c.FarAway)
          LSL.jobs.Remove(c);
        else
          LSL.customers.Remove(c);
      }
      else
      {
        Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) ped1, (InputArgument) "GENERIC_HOWS_IT_GOING", (InputArgument) "SPEECH_PARAMS_STANDARD", (InputArgument) 0);
        Script.Wait(1000);
        ped1.Task.PlayAnimation("switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_franklin", 8f, 3000, AnimationFlags.UpperBodyOnly);
        Script.Wait(3000);
        Game.Player.Money += num1;
        c.Ped.Money -= num1;
        LSL.PlayerInventory[c.Drug.Name] -= c.amount;
        LsFunctions.PlayTheSound();
        UI.Notify("Sold " + c.Drug.Name + " " + LsFunctions.GramsToOz(c.amount) + ", you got " + LsFunctions.GramsToOz(LSL.PlayerInventory[c.Drug.Name]) + " left. Gained ~g~" + num3.ToString() + " ~s~rep, total rep ~g~" + LSL.areas[LSL.areaIndex].Reputation.ToString() + "~s~ in " + LSL.areas[LSL.areaIndex].Name);
        Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) ped1, (InputArgument) "GENERIC_THANKS", (InputArgument) "SPEECH_PARAMS_STANDARD", (InputArgument) 0);
        if ((Entity) c.Vehicle != (Entity) null && c.Vehicle.Exists())
          c.Ped.Task.PerformSequence(LSL.PerformTaskSeq(1, c.Vehicle, Vector3.Zero));
        else
          c.Ped.Task.PerformSequence(LSL.PerformTaskSeq(0, (Vehicle) null, Vector3.Zero));
        if (c.FarAway)
          LSL.jobs.Remove(c);
        else
          LSL.customers.Remove(c);
      }
      LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "playerInventory")
        {
          using (IEnumerator<XElement> enumerator = descendant.Elements().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              XElement current = enumerator.Current;
              int num4;
              if (current.Name == (XName) "weed" && c.Drug.Name == "Weed")
              {
                XElement xelement = current;
                num4 = LSL.PlayerInventory["Weed"];
                string str = num4.ToString();
                xelement.Value = str;
              }
              if (current.Name == (XName) "crack" && c.Drug.Name == "Crack")
              {
                XElement xelement = current;
                num4 = LSL.PlayerInventory["Crack"];
                string str = num4.ToString();
                xelement.Value = str;
              }
              if (current.Name == (XName) "cocain" && c.Drug.Name == "Cocaine")
              {
                XElement xelement = current;
                num4 = LSL.PlayerInventory["Cocaine"];
                string str = num4.ToString();
                xelement.Value = str;
              }
            }
            break;
          }
        }
      }
      bool flag = false;
      if (LSL.aDealer != null && !LSL.aDealer.Detected)
      {
        double num4 = (double) LSL.areas[LSL.areaIndex].GangPresance * 1.5;
        if (LSL.rnd.Next(0, 8 + (int) num4) == 0)
        {
          UI.ShowSubtitle("Hey thought you might like to know, ~r~someone ~w~else selling over on ~y~" + World.GetStreetName(LSL.aDealer.dPed.Position) + "~w~.", 6000);
          LsFunctions.AddBlip((Entity) LSL.aDealer.dPed, BlipColor.Red, "Dealer", false, false);
          LSL.aDealer.Detected = true;
          flag = true;
        }
      }
      if (!flag && LSL.pigs.Count > 0 && (!flag && !LSL.pigs[0].detected) && LSL.rnd.Next(0, 5 + LSL.areas[LSL.areaIndex].GangPresance) == 0)
      {
        UI.ShowSubtitle("I hear ~r~Police ~w~are over on ~y~" + World.GetStreetName(LSL.pigs[0].Ped.Position) + "~w~. They heading this way.", 6000);
        LSL.pigs[0].detected = true;
        flag = true;
      }
      if (!flag && (int) LsFunctions.CurrentRepLvl(LSL.dealer1Rep) > LSL.DealerHandler.dealers.Count)
      {
        double reputation = (double) LSL.areas[LSL.areaIndex].Reputation;
        int num4 = LSL.rnd.Next(0, 110 - System.Math.Min((int) LsFunctions.CurrentRepLvl(reputation), 100));
        if (!flag && LSL.PottentialWorker == null && num4 == 0)
        {
          LSL.PottentialWorker = new HirePed(c.Ped);
          UI.ShowSubtitle("You taking on any workers?", 6000);
          flag = true;
        }
      }
      if (!flag)
        OldCustomerHandler.AddCustomer(ped1, LSL.HouseHandler.Inside(), false, new KeyValuePair<string, int>(c.Drug.Name, c.amount));
      else
        OldCustomerHandler.AddCustomer(ped1, LSL.HouseHandler.Inside(), true, new KeyValuePair<string, int>(c.Drug.Name, c.amount));
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      foreach (Ped ped2 in pedList)
      {
        if (LSL.DEBUG)
          UI.Notify(pedList.Count.ToString() + " | " + Function.Call<int>(Hash._0xFF059E1E4C01E63C, (InputArgument) ped2.Handle).ToString() + " | " + LsFunctions.IsPolice(ped2).ToString());
        if (LsFunctions.CanSee(ped2, Game.Player.Character))
        {
          LsFunctions.SendTextMsg("They spotted you dealing, your gona be hot round there now.");
          LSL.areas[LSL.areaIndex].Heat += 200;
          Game.Player.WantedLevel = 1;
          break;
        }
      }
      if (LSL.aDealer != null && (Entity) LSL.aDealer.dPed != (Entity) null && (LsFunctions.CanSee(LSL.aDealer.dPed, Game.Player.Character) && LSL.rnd.Next(0, 2) == 0) && LSL.driveByHandler.AmountOfDriveBy(true) == 0)
        LsFunctions.SpawnDriveBy(LSL.player.Character, true);
      LsFunctions.CheckIfPartolsCanSee();
      LSL.customerSellTo = (Customer) null;
    }

    public static void HandOverGoods(Ped _from, Ped _too)
    {
      List<Ped> pedList = new List<Ped>();
      LSL.PedsCanSeeDeal = 0;
      foreach (Ped getPed in LSL.lPeds.GetPeds)
      {
        Ped ped = getPed;
        if (ped.IsAlive)
        {
          if (LsFunctions.IsPolice(ped))
          {
            pedList.Add(ped);
            ped.Task.LookAt((Entity) Game.Player.Character, 10000);
          }
          if (!ped.IsPlayer && LSL.customers.FindAll((Predicate<Customer>) (s => (Entity) s.Ped == (Entity) ped)).Count == 0 && (!OldCustomerHandler.IsPedOldCustomer(ped) && LsFunctions.CanSee(ped, Game.Player.Character)))
            ++LSL.PedsCanSeeDeal;
        }
      }
      if (!_from.IsInVehicle() || !_too.IsInVehicle())
      {
        _from.Task.TurnTo((Entity) _too);
        _too.Task.TurnTo((Entity) _from);
        Script.Wait(1000);
      }
      _too.Task.PlayAnimation("switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_franklin", 8f, 2000, AnimationFlags.UpperBodyOnly);
      _from.Task.PlayAnimation("switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_shopkeeper", 8f, 2000, AnimationFlags.UpperBodyOnly);
      Script.Wait(2000);
      foreach (Ped ped in pedList)
      {
        if (LSL.DEBUG)
        {
          string[] strArray = new string[5];
          int num = pedList.Count;
          strArray[0] = num.ToString();
          strArray[1] = " | ";
          num = Function.Call<int>(Hash._0xFF059E1E4C01E63C, (InputArgument) ped.Handle);
          strArray[2] = num.ToString();
          strArray[3] = " | ";
          strArray[4] = LsFunctions.IsPolice(ped).ToString();
          UI.Notify(string.Concat(strArray));
        }
        if ((double) ped.Position.DistanceTo(LSL.playerPos) < 40.0 && LsFunctions.CanSee(ped, Game.Player.Character))
        {
          LsFunctions.SendTextMsg("They spotted you dealing, your gona be hot round there now.");
          LSL.areas[LSL.areaIndex].Heat += 200;
          Game.Player.WantedLevel = 1;
          break;
        }
      }
      if ((Entity) _from == (Entity) LSL.player.Character || (Entity) _too == (Entity) LSL.player.Character)
      {
        LSL.player.Character.Task.ClearLookAt();
        LSL.player.Character.Task.ClearAll();
      }
      if (LSL.aDealer != null && (Entity) LSL.aDealer.dPed != (Entity) null && ((double) LSL.aDealer.dPed.Position.DistanceTo(LSL.playerPos) < 40.0 && LsFunctions.CanSee(LSL.aDealer.dPed, Game.Player.Character) && (LSL.rnd.Next(0, 2) == 0 && LSL.driveByHandler.AmountOfDriveBy(true) == 0)))
      {
        LsFunctions.DbugString("Spawning Driveby, player seen doing deal.");
        LsFunctions.SpawnDriveBy(LSL.player.Character, true);
      }
      LsFunctions.CheckIfPartolsCanSee();
      LSL.lastTimeDealDone = (float) Game.GameTime;
    }

    private static void CheckIfPartolsCanSee()
    {
      if (LSL.driveByHandler.GetDriveBies.Where<DriveBy>((Func<DriveBy, bool>) (r => LsFunctions.CanSee(r.Leader, LSL.player.Character))).Count<DriveBy>() <= 0)
        return;
      LSL.driveByHandler.SetAgressiveTarget(true, LSL.player.Character);
      UI.Notify("Driveby saw deal");
    }

    public static int MoneyToRep(int _money, bool _debt) => _debt ? (int) ((double) (_money / 100) * 0.5) : _money / 100;

    public static void Dealer1AddRep(int _rep)
    {
      if (_rep <= 0)
        return;
      LSL.dealer1Rep += (double) _rep;
    }

    public static Model RequestModel(PedHash _hash)
    {
      Model model = new Model(_hash);
      model.Request(1000);
      if (model.IsInCdImage && model.IsValid)
      {
        while (!model.IsLoaded)
          Script.Yield();
        return model;
      }
      model.MarkAsNoLongerNeeded();
      return model;
    }

    public static Model RequestModel(VehicleHash _hash)
    {
      Model model = new Model(_hash);
      model.Request(1000);
      if (model.IsInCdImage && model.IsValid)
      {
        while (!model.IsLoaded)
          Script.Yield();
        return model;
      }
      model.MarkAsNoLongerNeeded();
      return model;
    }

    public static string GramsToOz(int _grams)
    {
      if (_grams < 28)
        return "~g~" + _grams.ToString() + "~s~g";
      int num1 = _grams / 28;
      int num2 = _grams - num1 * 28;
      return "~g~" + num1.ToString() + "~s~oz, ~g~" + num2.ToString() + "~s~g";
    }

    public static void DbugString(string _string)
    {
      if (!LSL.DEBUG)
        return;
      UI.Notify(_string);
    }

    public static string IntToMoney(int _money)
    {
      StringBuilder stringBuilder = new StringBuilder("~g~" + _money.ToString("#,##0") + "~s~");
      stringBuilder.Replace(",", "~s~,~g~");
      return stringBuilder.ToString();
    }

    public static bool zeeClose()
    {
      Vector3 position;
      if ((Entity) LSL.drugDealer1 != (Entity) null)
      {
        position = LSL.drugDealer1.Position;
        if ((double) position.DistanceTo(LSL.playerPos) < 4.0)
          return true;
      }
      if ((Entity) LSL.weaponDealer1 != (Entity) null)
      {
        position = LSL.weaponDealer1.Position;
        if ((double) position.DistanceTo(LSL.playerPos) < 4.0)
          return true;
      }
      return false;
    }

    public static bool IsCustomerNear() => LSL.customers.FindAll((Predicate<Customer>) (r => (double) r.Ped.Position.DistanceTo(LSL.playerPos) < 20.0)).Count == 0 && LSL.jobs.FindAll((Predicate<Customer>) (r => (double) r.Ped.Position.DistanceTo(LSL.playerPos) < 20.0)).Count == 0;

    public static Dictionary<int, Tuple<int, int>> GetPedVariationData(Ped _ped)
    {
      List<int> intList = new List<int>();
      intList.Add(0);
      intList.Add(1);
      intList.Add(2);
      intList.Add(3);
      intList.Add(4);
      intList.Add(5);
      intList.Add(6);
      intList.Add(8);
      intList.Add(9);
      intList.Add(10);
      intList.Add(11);
      Dictionary<int, Tuple<int, int>> dictionary = new Dictionary<int, Tuple<int, int>>();
      foreach (int key in intList)
      {
        Tuple<int, int> tuple = new Tuple<int, int>(Function.Call<int>(Hash._0x67F3780DD425D4FC, (InputArgument) _ped, (InputArgument) key), Function.Call<int>(Hash._0x04A355E041E004E6, (InputArgument) _ped, (InputArgument) key));
        dictionary.Add(key, tuple);
      }
      return dictionary.Count > 0 ? dictionary : (Dictionary<int, Tuple<int, int>>) null;
    }

    public static void SetPedVariation(Ped _ped, Dictionary<int, Tuple<int, int>> _pData)
    {
      foreach (int key in new List<int>()
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        8,
        9,
        10,
        11
      })
        Function.Call(Hash._0x262B14F48D29DE80, (InputArgument) _ped, (InputArgument) key, (InputArgument) _pData[key].Item1, (InputArgument) _pData[key].Item2, (InputArgument) 0);
    }
  }
}
