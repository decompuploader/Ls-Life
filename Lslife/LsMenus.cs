// Decompiled with JetBrains decompiler
// Type: LSlife.LsMenus
// Assembly: LSlife, Version=0.2.3.9, Culture=neutral, PublicKeyToken=null
// MVID: B799CF66-A2B8-4854-B7C0-16CE0496769B
// Assembly location: C:\Users\EOL\Desktop\GTA\decompiles\LSlife.dll

using GTA;
using GTA.Math;
using GTA.Native;
using LSlife.OldCustomers;
using Microsoft.CSharp.RuntimeBinder;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace LSlife
{
  internal static class LsMenus
  {
    public static void OnstartDealSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item != LSL.StartDeal)
        return;
      LSL.sellingWeed = LSL.sellWeed.Checked;
      LSL.sellingCrack = LSL.sellCrack.Checked;
      LSL.sellingCocaine = LSL.sellCocaine.Checked;
      if (!LSL.sellingWeed && !LSL.sellingCrack && !LSL.sellingCocaine)
      {
        LsFunctions.TextMsg("ZEE", "Work", "You need to tell me what your selling.");
      }
      else
      {
        if (LSL.isDealing)
          return;
        int num = LsFunctions.TotalDrugs(LSL.PlayerInventory) + LSL.HouseHandler.TotalStashDrugs();
        if (LSL.pStashVehicle != null)
          num += LSL.pStashVehicle.TotalDrugs();
        sender.Visible = !sender.Visible;
        if (LSL.ratChance > 0)
          LsFunctions.TextMsg("ZEE", "Work", "People say your to hot round there.");
        else if (num > 0)
        {
          LsFunctions.TextMsg("ZEE", "Work", "Ok, ill put the word out");
          LSL.isDealing = true;
          LSL.lastStreet = World.GetStreetName(new Vector2(Game.Player.Character.Position.X, Game.Player.Character.Position.Y));
          LSL.thisStreet = LSL.lastStreet;
          LSL.timeStartedDealing = Game.GameTime;
        }
        else
          LsFunctions.TextMsg("ZEE", "Work", "You got nothing to sell.");
      }
    }

    public static void OnZeeMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item == LSL.buyWeapons)
      {
        sender.Visible = !sender.Visible;
        if (LSL.ratChance > 0)
          LsFunctions.TextMsg("ZEE", "Work", "Nah, Your to hot");
        else if ((Entity) LSL.weaponDealer1 == (Entity) null)
        {
          LSL.callDealer(true);
          LsFunctions.TextMsg("ZEE", "Work", "Sending someone to meet you.");
        }
        else
        {
          LsFunctions.RemovePedFromWorld(LSL.weaponDealer1, true);
          LSL.weaponDealer1 = (Ped) null;
          LsFunctions.TextMsg("ZEE", "Work", "Ok i will tell them to head home.");
        }
      }
      if (item == LSL.buyLift)
      {
        if (LSL.wepDealer == null)
        {
          sender.Visible = !sender.Visible;
          LSL.wepDealer = new Dealer((Model) PedHash.ArmLieut01GMM, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 2f, (Model) "INGOT", LSL.playerGroup);
          if ((Entity) LSL.wepDealer.ped != (Entity) null)
            LsFunctions.TextMsg("ZEE", LSL.wepDealer.ped, "Work", "Sending someone to meet you.");
        }
        else
        {
          LsFunctions.RemovePedFromWorld(LSL.wepDealer.ped, true);
          LSL.wepDealer.vehicle?.MarkAsNoLongerNeeded();
          LSL.wepDealer.vehicle?.CurrentBlip?.Remove();
          LSL.wepDealer = (Dealer) null;
          LsFunctions.TextMsg("ZEE", "Work", "Ok I will tell them you cant make it.");
        }
      }
      if (item != LSL.payDebt)
        return;
      if (LSL.player.Money > 0)
      {
        int _money = LSL.dealer1Debt >= LSL.player.Money ? LSL.player.Money : LSL.dealer1Debt;
        LSL.dealer1Debt -= _money;
        LSL.player.Money -= _money;
        LSL.dealer1Money += _money;
        LsFunctions.Dealer1AddRep(LsFunctions.MoneyToRep(_money, true));
        LSL.payDebt.Text = "Pay Debt $" + LsFunctions.IntToMoney(LSL.dealer1Debt);
        LsFunctions.PlayTheSound();
        LsFunctions.SaveDealer();
        Script.Wait(500);
        if (LSL.dealer1Debt > 0)
          LsFunctions.TextMsg("ZEE", "Work", "Thanks for paying some of that debt off.");
        else
          LsFunctions.TextMsg("ZEE", "Work", "Thanks, all the debt is payed off");
      }
      else
        LsFunctions.TextMsg("ZEE", "Work", "You havent got any money");
    }

    public static void OnWepMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item == LSL.melee)
      {
        int num = LSL.meleePrice[LSL.melee.Index] - LsFunctions.Discount(LSL.meleePrice[LSL.melee.Index]);
        if (Game.Player.Money >= num)
        {
          // ISSUE: reference to a compiler-generated field
          if (LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, WeaponHash>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "TryParse", (IEnumerable<Type>) new Type[1]
            {
              typeof (WeaponHash)
            }, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
            }));
          }
          WeaponHash weaponHash;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__0, typeof (Enum), LSL.meleeWeps[LSL.melee.Index], ref weaponHash);
          if (!Game.Player.Character.Weapons.HasWeapon(weaponHash))
          {
            LsFunctions.HandOverGoods(LSL.weaponDealer1, LSL.player.Character);
            Game.Player.Money -= num;
            LsFunctions.PlayTheSound();
            LSL.dealer1Money += num;
            LSL.dealer1Rep += (double) (num / 100);
            Game.Player.Character.Weapons.Give(weaponHash, 0, true, true);
            LsFunctions.UpdateSave();
          }
        }
        else if (LSL.dealer1Debt < LsFunctions.zeeDebtCap() && LSL.dealer1Debt + num <= LsFunctions.zeeDebtCap())
        {
          // ISSUE: reference to a compiler-generated field
          if (LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, WeaponHash>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "TryParse", (IEnumerable<Type>) new Type[1]
            {
              typeof (WeaponHash)
            }, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
            }));
          }
          WeaponHash weaponHash;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__1.Target((CallSite) LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__1, typeof (Enum), LSL.meleeWeps[LSL.melee.Index], ref weaponHash);
          if (!Game.Player.Character.Weapons.HasWeapon(weaponHash))
          {
            LsFunctions.HandOverGoods(LSL.weaponDealer1, LSL.player.Character);
            LsFunctions.PlayTheSound();
            LSL.dealer1Debt += num;
            Game.Player.Character.Weapons.Give(weaponHash, 0, true, true);
            LsFunctions.UpdateSave();
          }
        }
      }
      if (item == LSL.pistol)
      {
        int num = LSL.pistolPrice[LSL.pistol.Index] - LsFunctions.Discount(LSL.pistolPrice[LSL.pistol.Index]);
        if (LSL.pistol.Index == 3)
        {
          if (Game.Player.Character.Weapons.Current.Group == WeaponGroup.Pistol)
          {
            if (Game.Player.Money >= num)
            {
              Game.Player.Money -= num;
              LSL.dealer1Money += num;
              LSL.dealer1Rep += (double) (num / 100);
              Game.Player.Character.Weapons.Current.Ammo += 50;
              LsFunctions.UpdateSave();
            }
            else if (LSL.dealer1Debt < LsFunctions.zeeDebtCap() && LSL.dealer1Debt + num <= LsFunctions.zeeDebtCap())
            {
              LSL.dealer1Debt += num;
              Game.Player.Character.Weapons.Current.Ammo += 50;
              LsFunctions.UpdateSave();
            }
          }
          else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.Pistol))
          {
            Game.Player.Character.Weapons.Select(WeaponHash.Pistol);
            if (Game.Player.Money >= num)
            {
              Game.Player.Money -= num;
              LSL.dealer1Money += num;
              LSL.dealer1Rep += (double) (num / 100);
              Game.Player.Character.Weapons.Current.Ammo += 50;
              LsFunctions.UpdateSave();
            }
            else if (LSL.dealer1Debt < LsFunctions.zeeDebtCap() && LSL.dealer1Debt + num <= LsFunctions.zeeDebtCap())
            {
              LSL.dealer1Debt += num;
              Game.Player.Character.Weapons.Current.Ammo += 50;
              LsFunctions.UpdateSave();
            }
          }
          else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.Pistol50))
          {
            Game.Player.Character.Weapons.Select(WeaponHash.Pistol50);
            if (Game.Player.Money >= num)
            {
              Game.Player.Money -= num;
              LSL.dealer1Money += num;
              LSL.dealer1Rep += (double) (num / 100);
              Game.Player.Character.Weapons.Current.Ammo += 50;
              LsFunctions.UpdateSave();
            }
            else if (LSL.dealer1Debt < LsFunctions.zeeDebtCap() && LSL.dealer1Debt + num <= LsFunctions.zeeDebtCap())
            {
              LSL.dealer1Debt += num;
              Game.Player.Character.Weapons.Current.Ammo += 50;
              LsFunctions.UpdateSave();
            }
          }
          else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.HeavyPistol))
          {
            Game.Player.Character.Weapons.Select(WeaponHash.HeavyPistol);
            if (Game.Player.Money >= num)
            {
              Game.Player.Money -= num;
              LSL.dealer1Money += num;
              LSL.dealer1Rep += (double) (num / 100);
              Game.Player.Character.Weapons.Current.Ammo += 50;
              LsFunctions.UpdateSave();
            }
            else if (LSL.dealer1Debt < LsFunctions.zeeDebtCap() && LSL.dealer1Debt + num <= LsFunctions.zeeDebtCap())
            {
              LSL.dealer1Debt += num;
              Game.Player.Character.Weapons.Current.Ammo += 50;
              LsFunctions.UpdateSave();
            }
          }
        }
        else if (Game.Player.Money >= num)
        {
          // ISSUE: reference to a compiler-generated field
          if (LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, WeaponHash>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "TryParse", (IEnumerable<Type>) new Type[1]
            {
              typeof (WeaponHash)
            }, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
            }));
          }
          WeaponHash weaponHash;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__2.Target((CallSite) LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__2, typeof (Enum), LSL.pistolWeps[LSL.pistol.Index], ref weaponHash);
          if (!Game.Player.Character.Weapons.HasWeapon(weaponHash))
          {
            Game.Player.Money -= num;
            LSL.dealer1Money += num;
            LSL.dealer1Rep += (double) (num / 100);
            Game.Player.Character.Weapons.Give(weaponHash, 100, true, true);
            LsFunctions.UpdateSave();
          }
        }
        else if (LSL.dealer1Debt < LsFunctions.zeeDebtCap() && LSL.dealer1Debt + num <= LsFunctions.zeeDebtCap())
        {
          // ISSUE: reference to a compiler-generated field
          if (LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__3 = CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, WeaponHash>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "TryParse", (IEnumerable<Type>) new Type[1]
            {
              typeof (WeaponHash)
            }, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
            }));
          }
          WeaponHash weaponHash;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__3.Target((CallSite) LsMenus.\u003C\u003Eo__2.\u003C\u003Ep__3, typeof (Enum), LSL.pistolWeps[LSL.pistol.Index], ref weaponHash);
          if (!Game.Player.Character.Weapons.HasWeapon(weaponHash))
          {
            LSL.dealer1Debt += num;
            Game.Player.Character.Weapons.Give(weaponHash, 100, true, true);
            LsFunctions.UpdateSave();
          }
        }
      }
      if (item == LSL.armour)
      {
        int num = 250 - LsFunctions.Discount(250);
        if (Game.Player.Money >= num)
        {
          if (LsFunctions.IsPlayerArmoursMaxed())
          {
            UI.ShowSubtitle("You cant carry any more", 4000);
          }
          else
          {
            Game.Player.Money -= num;
            LSL.dealer1Money += num;
            LSL.dealer1Rep += (double) (num / 100);
            LsFunctions.GiveArmourToPlayer();
            LsFunctions.UpdateInventory();
          }
        }
        else if (LSL.dealer1Debt < LsFunctions.zeeDebtCap() && LSL.dealer1Debt + num <= LsFunctions.zeeDebtCap())
        {
          if (LsFunctions.IsPlayerArmoursMaxed())
          {
            UI.ShowSubtitle("You cant carry any more", 4000);
          }
          else
          {
            LSL.dealer1Debt += num;
            LSL.dealer1Rep += (double) (num / 100);
            LsFunctions.GiveArmourToPlayer();
            LsFunctions.UpdateInventory();
          }
        }
      }
      if (item != LSL.dismiss || !((Entity) LSL.weaponDealer1 != (Entity) null))
        return;
      LSL.weaponDealer1.CurrentBlip.Remove();
      LSL.wepMenu.Visible = !LSL.wepMenu.Visible;
      LSL.weaponDealer1.Task.WanderAround();
      LSL.weaponDealer1.MarkAsNoLongerNeeded();
      LSL.weaponDealer1.LeaveGroup();
      LSL.weaponDealer1 = (Ped) null;
      LSL.readyToDeal = false;
    }

    public static void onWeedMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item != LSL.WeedBuy || LSL.dealer1Weed <= 0 || LSL.bushWeed.Value <= 0)
        return;
      int num = LSL.bushWeed.Value * (LSL.weed1Price - LsFunctions.Discount(LSL.weed1Price));
      LSL.NewOrder.OrderWeed(LSL.bushWeed.Value, num);
      LSL.bushWeed.Maximum -= LSL.bushWeed.Value;
      LSL.dealer1Weed = LSL.bushWeed.Maximum;
      LSL.WeedBuy.Text = "Order ~g~" + LSL.bushWeed.Value.ToString() + " ~s~for $" + LsFunctions.IntToMoney(num);
      LSL.PlaceOrder.Text = "Place Order $" + LsFunctions.IntToMoney(LSL.NewOrder.Bill);
      LsFunctions.DbugString("Weed Order changed");
      LsFunctions.UpdateOrderMenuDesc();
    }

    public static void onCrackMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item != LSL.CrackBuy || LSL.dealer1Crack <= 0 || LSL.crack.Value <= 0)
        return;
      int num = LSL.crack.Value * (LSL.crack1Price - LsFunctions.Discount(LSL.crack1Price));
      LSL.NewOrder.OrderCrack(LSL.crack.Value, num);
      LSL.crack.Maximum -= LSL.crack.Value;
      LSL.dealer1Crack = LSL.crack.Maximum;
      LSL.CrackBuy.Text = "Order ~g~" + LSL.crack.Value.ToString() + " ~w~for $~g~" + LsFunctions.IntToMoney(num);
      LSL.PlaceOrder.Text = "Place Order $" + LsFunctions.IntToMoney(LSL.NewOrder.Bill);
      LsFunctions.DbugString("Crack Order changed");
      LsFunctions.UpdateOrderMenuDesc();
    }

    public static void onCocainMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item != LSL.CocainBuy || LSL.dealer1Cocain <= 0)
        return;
      int num = LSL.cocaine.Value * (LSL.cocain1Price - LsFunctions.Discount(LSL.cocain1Price));
      LSL.NewOrder.OrderCocaine(LSL.cocaine.Value, num);
      LSL.cocaine.Maximum -= LSL.cocaine.Value;
      LSL.dealer1Cocain = LSL.cocaine.Maximum;
      LSL.CocainBuy.Text = "Order ~g~" + LSL.cocaine.Value.ToString() + " ~w~for $~g~" + LsFunctions.IntToMoney(num);
      LSL.PlaceOrder.Text = "Place Order $" + LsFunctions.IntToMoney(LSL.NewOrder.Bill);
      LsFunctions.DbugString("Cocaine Order changed");
      LsFunctions.UpdateOrderMenuDesc();
    }

    public static void OnOrderDrugsMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item != LSL.PlaceOrder || LSL.NewOrder == null || !LSL.NewOrder.OrderValid())
        return;
      LSL.NewOrder.Placed = true;
      LSL.callDealer(false);
      LSL.LsMenuPool.CloseAllMenus();
    }

    public static void onPutVehicleMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item == LSL.putWeedVeh)
      {
        LSL.pStashVehicle.drugCarWeed += LSL.putWeedVeh.Value;
        LSL.PlayerInventory["Weed"] -= LSL.putWeedVeh.Value;
        LSL.putWeedVeh.Maximum -= LSL.putWeedVeh.Value;
        LSL.takeWeedVeh.Maximum = LSL.pStashVehicle.drugCarWeed;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weed")
              {
                element.Value = LSL.PlayerInventory["Weed"].ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weed")
              {
                element.Value = LSL.pStashVehicle.drugCarWeed.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.putWeedOzVeh)
      {
        LSL.pStashVehicle.drugCarWeedOz += LSL.putWeedOzVeh.Value;
        LsFunctions.pWeedOunce -= LSL.putWeedOzVeh.Value;
        LSL.putWeedOzVeh.Maximum -= LSL.putWeedOzVeh.Value;
        LSL.takeWeedOzVeh.Maximum = LSL.pStashVehicle.drugCarWeedOz;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weedOz")
              {
                element.Value = LsFunctions.pWeedOunce.ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weedOz")
              {
                element.Value = LSL.pStashVehicle.drugCarWeedOz.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.putCrackVeh)
      {
        LSL.pStashVehicle.drugCarCrack += LSL.putCrackVeh.Value;
        LSL.PlayerInventory["Crack"] -= LSL.putCrackVeh.Value;
        LSL.putCrackVeh.Maximum -= LSL.putCrackVeh.Value;
        LSL.takeCrackVeh.Maximum = LSL.pStashVehicle.drugCarCrack;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "crack")
              {
                element.Value = LSL.PlayerInventory["Crack"].ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "crack")
              {
                element.Value = LSL.pStashVehicle.drugCarCrack.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.putCrackOzVeh)
      {
        LSL.pStashVehicle.drugCarCrackOz += LSL.putCrackOzVeh.Value;
        LsFunctions.pCrackOunce -= LSL.putCrackOzVeh.Value;
        LSL.putCrackOzVeh.Maximum -= LSL.putCrackOzVeh.Value;
        LSL.takeCrackOzVeh.Maximum = LSL.pStashVehicle.drugCarCrackOz;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "crackOz")
              {
                element.Value = LsFunctions.pCrackOunce.ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "crackOz")
              {
                element.Value = LSL.pStashVehicle.drugCarCrackOz.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.putCocainVeh)
      {
        LSL.pStashVehicle.drugCarCocain += LSL.putCocainVeh.Value;
        LSL.PlayerInventory["Cocaine"] -= LSL.putCocainVeh.Value;
        LSL.putCocainVeh.Maximum -= LSL.putCocainVeh.Value;
        LSL.takeCocainVeh.Maximum = LSL.pStashVehicle.drugCarCocain;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "cocain")
              {
                element.Value = LSL.PlayerInventory["Cocaine"].ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "cocain")
              {
                element.Value = LSL.pStashVehicle.drugCarCocain.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.putCocainOzVeh)
      {
        LSL.pStashVehicle.drugCarCocainOz += LSL.putCocainOzVeh.Value;
        LsFunctions.pCocaineOunce -= LSL.putCocainOzVeh.Value;
        LSL.putCocainOzVeh.Maximum -= LSL.putCocainOzVeh.Value;
        LSL.takeCocainOzVeh.Maximum = LSL.pStashVehicle.drugCarCocainOz;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "cocaineOz")
              {
                element.Value = LsFunctions.pCocaineOunce.ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "cocainOz")
              {
                element.Value = LSL.pStashVehicle.drugCarCocainOz.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.putMoneyVeh)
      {
        LSL.pStashVehicle.drugCarMoney += LSL.putMoneyVeh.Value;
        Game.Player.Money -= LSL.putMoneyVeh.Value;
        LSL.putMoneyVeh.Maximum -= LSL.putMoneyVeh.Value;
        LSL.takeMoneyVeh.Maximum = LSL.pStashVehicle.drugCarMoney;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "drugCarStash")
          {
            using (IEnumerator<XElement> enumerator = descendant.Elements().GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                XElement current = enumerator.Current;
                if (current.Name == (XName) "money")
                {
                  current.Value = LSL.pStashVehicle.drugCarMoney.ToString();
                  break;
                }
              }
              break;
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.putWeaponVeh && LSL.putWeaponVeh.Items.Count > 0)
      {
        LSL.vehWeaponsXML = XDocument.Load("scripts\\LSLife\\LsLife_Vehicle_Weapons.xml");
        int num1 = 0;
        int num2 = 0;
        if (LSL.vehWeaponsXML.Descendants().Count<XElement>() > 0)
        {
          foreach (XElement descendant in LSL.vehWeaponsXML.Descendants())
          {
            if (descendant.Name == (XName) "weapon")
            {
              ++num1;
              if (num1.ToString() != descendant.FirstAttribute.Value)
                num2 = num1;
            }
          }
        }
        XElement playerWeapon = LSL.playerWeapons[LSL.putWeaponVeh.Index];
        if (playerWeapon != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (LsMenus.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LsMenus.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, WeaponHash>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "TryParse", (IEnumerable<Type>) null, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
            }));
          }
          WeaponHash weaponHash;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) LsMenus.\u003C\u003Eo__7.\u003C\u003Ep__0, typeof (Enum), LSL.playerWeps[LSL.putWeaponVeh.Index], ref weaponHash);
          if (weaponHash != WeaponHash.Unarmed)
          {
            playerWeapon.SetAttributeValue((XName) "id", (object) num2);
            LSL.vehWeaponsXML.Element((XName) "weapons")?.Add((object) playerWeapon);
            LSL.vehWeaponsXML.Save("scripts\\LSLife\\LsLife_Vehicle_Weapons.xml");
            Game.Player.Character.Weapons.Remove(weaponHash);
            LSL.playerWeapons.Remove(playerWeapon);
            LSL.playerWeps.RemoveAt(LSL.putWeaponVeh.Index);
            LSL.pStashVehicle.vehWeapons.Add(playerWeapon);
            LSL.pStashVehicle.vehWeps.Add(LSL.putWeaponVeh.Items[LSL.putWeaponVeh.Index]);
          }
        }
      }
      LSL.pStashVehicle.UpdateCarStash();
    }

    public static void onTakeVehicleMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item == LSL.takeWeedVeh)
      {
        LSL.PlayerInventory["Weed"] += LSL.takeWeedVeh.Value;
        LSL.pStashVehicle.drugCarWeed -= LSL.takeWeedVeh.Value;
        LSL.takeWeedVeh.Maximum -= LSL.takeWeedVeh.Value;
        LSL.putWeedVeh.Maximum = LSL.PlayerInventory["Weed"];
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weed")
              {
                element.Value = LSL.PlayerInventory["Weed"].ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weed")
              {
                element.Value = LSL.pStashVehicle.drugCarWeed.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.takeWeedOzVeh)
      {
        LsFunctions.pWeedOunce += LSL.takeWeedOzVeh.Value;
        LSL.pStashVehicle.drugCarWeedOz -= LSL.takeWeedOzVeh.Value;
        LSL.takeWeedOzVeh.Maximum -= LSL.takeWeedOzVeh.Value;
        LSL.putWeedOzVeh.Maximum = LsFunctions.pWeedOunce;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weedOz")
              {
                element.Value = LsFunctions.pWeedOunce.ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weedOz")
              {
                element.Value = LSL.pStashVehicle.drugCarWeedOz.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.takeCrackVeh)
      {
        LSL.PlayerInventory["Crack"] += LSL.takeCrackVeh.Value;
        LSL.pStashVehicle.drugCarCrack -= LSL.takeCrackVeh.Value;
        LSL.takeCrackVeh.Maximum -= LSL.takeCrackVeh.Value;
        LSL.putCrackVeh.Maximum = LSL.PlayerInventory["Crack"];
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "crack")
              {
                element.Value = LSL.PlayerInventory["Crack"].ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "crack")
              {
                element.Value = LSL.pStashVehicle.drugCarCrack.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.takeCrackOzVeh)
      {
        LsFunctions.pCrackOunce += LSL.takeCrackOzVeh.Value;
        LSL.pStashVehicle.drugCarCrackOz -= LSL.takeCrackOzVeh.Value;
        LSL.takeCrackOzVeh.Maximum -= LSL.takeCrackOzVeh.Value;
        LSL.putCrackOzVeh.Maximum = LsFunctions.pCrackOunce;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "crackOz")
              {
                element.Value = LsFunctions.pCrackOunce.ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "crackOz")
              {
                element.Value = LSL.pStashVehicle.drugCarCrackOz.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.takeCocainVeh)
      {
        LSL.PlayerInventory["Cocaine"] += LSL.takeCocainVeh.Value;
        LSL.pStashVehicle.drugCarCocain -= LSL.takeCocainVeh.Value;
        LSL.takeCocainVeh.Maximum -= LSL.takeCocainVeh.Value;
        LSL.putCocainStash.Maximum = LSL.PlayerInventory["Cocaine"];
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "cocain")
              {
                element.Value = LSL.PlayerInventory["Cocaine"].ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "cocain")
              {
                element.Value = LSL.pStashVehicle.drugCarCocain.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.takeCocainOzVeh)
      {
        LsFunctions.pCocaineOunce += LSL.takeCocainOzVeh.Value;
        LSL.pStashVehicle.drugCarCocainOz -= LSL.takeCocainOzVeh.Value;
        LSL.takeCocainOzVeh.Maximum -= LSL.takeCocainOzVeh.Value;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "cocainOz")
              {
                element.Value = LsFunctions.pCocaineOunce.ToString();
                break;
              }
            }
          }
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "cocainOz")
              {
                element.Value = LSL.pStashVehicle.drugCarCocainOz.ToString();
                break;
              }
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.takeMoneyVeh)
      {
        Game.Player.Money += LSL.takeMoneyVeh.Value;
        LSL.pStashVehicle.drugCarMoney -= LSL.takeMoneyVeh.Value;
        LSL.takeMoneyVeh.Maximum -= LSL.takeMoneyVeh.Value;
        LSL.putMoneyVeh.Maximum = Game.Player.Money;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "drugCarStash")
          {
            using (IEnumerator<XElement> enumerator = descendant.Elements().GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                XElement current = enumerator.Current;
                if (current.Name == (XName) "money")
                {
                  current.Value = LSL.pStashVehicle.drugCarMoney.ToString();
                  break;
                }
              }
              break;
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (item == LSL.takeWeaponVeh && LSL.takeWeaponVeh.Items.Count > 0)
      {
        LSL.vehWeaponsXML = XDocument.Load("scripts\\LSLife\\LsLife_Vehicle_Weapons.xml");
        XElement vehWeapon = LSL.pStashVehicle.vehWeapons[LSL.takeWeaponVeh.Index];
        XElement xelement = (XElement) null;
        foreach (XElement element in vehWeapon.Elements())
        {
          if (element.Name == (XName) "HASH")
            xelement = element;
        }
        if (xelement != null)
        {
          WeaponHash result1;
          Enum.TryParse<WeaponHash>(xelement.Value, out result1);
          if (result1 != WeaponHash.Unarmed)
          {
            foreach (XElement descendant in LSL.vehWeaponsXML.Descendants())
            {
              int result2 = 0;
              if (descendant.Name == (XName) "HASH" && descendant.Value == xelement.Value)
              {
                XElement parent = descendant.Parent;
                foreach (XElement element in parent.Elements())
                {
                  if (element.Name == (XName) "AMMO")
                    int.TryParse(element.Value, out result2);
                }
                Game.Player.Character.Weapons.Give(result1, result2, true, false);
                Game.Player.Character.Weapons.Select(result1, true);
                foreach (XElement element in parent.Elements())
                {
                  if (element.Name == (XName) "COMPONENT")
                  {
                    WeaponComponent result3;
                    Enum.TryParse<WeaponComponent>(element.Value, out result3);
                    Game.Player.Character.Weapons.Current.SetComponent(result3, true);
                  }
                }
                LSL.playerWeapons.Add(descendant);
                LSL.playerWeps.Add((object) descendant.Value);
                LSL.pStashVehicle.vehWeapons.Remove(vehWeapon);
                parent.Remove();
                LSL.pStashVehicle.vehWeps.RemoveAt(LSL.takeWeaponVeh.Index);
                LSL.vehWeaponsXML.Save("scripts\\LSLife\\LsLife_Vehicle_Weapons.xml");
                break;
              }
            }
          }
        }
      }
      LSL.pStashVehicle.UpdateCarStash();
    }

    public static void onSliderChange(UIMenuSliderItem sender, int index)
    {
      if (sender == LSL.takeWeedVeh)
        LSL.vehStashTakeMenu.Subtitle.Caption = "Take " + LsFunctions.GramsToOz(LSL.takeWeedVeh.Value);
      else if (sender == LSL.putWeedVeh)
        LSL.vehStashPutMenu.Subtitle.Caption = "Put " + LsFunctions.GramsToOz(LSL.putWeedVeh.Value);
      if (sender == LSL.takeWeedOzVeh)
        LSL.vehStashTakeMenu.Subtitle.Caption = "Take ~r~" + LSL.takeWeedOzVeh.Value.ToString() + "~s~z";
      else if (sender == LSL.putWeedOzVeh)
        LSL.vehStashPutMenu.Subtitle.Caption = "Put ~r~" + LSL.putWeedOzVeh.Value.ToString() + "~s~z";
      else if (sender == LSL.takeCrackVeh)
        LSL.vehStashTakeMenu.Subtitle.Caption = "Take " + LsFunctions.GramsToOz(LSL.takeCrackVeh.Value);
      else if (sender == LSL.putCrackVeh)
        LSL.vehStashPutMenu.Subtitle.Caption = "Put " + LsFunctions.GramsToOz(LSL.putCrackVeh.Value);
      if (sender == LSL.takeCrackOzVeh)
        LSL.vehStashTakeMenu.Subtitle.Caption = "Take ~r~" + LSL.takeCrackOzVeh.Value.ToString() + "~s~z";
      else if (sender == LSL.putCrackOzVeh)
        LSL.vehStashPutMenu.Subtitle.Caption = "Put ~r~" + LSL.putCrackOzVeh.Value.ToString() + "~s~z";
      else if (sender == LSL.takeCocainVeh)
        LSL.vehStashTakeMenu.Subtitle.Caption = "Take " + LsFunctions.GramsToOz(LSL.takeCocainVeh.Value);
      else if (sender == LSL.putCocainVeh)
        LSL.vehStashPutMenu.Subtitle.Caption = "Put " + LsFunctions.GramsToOz(LSL.putCocainVeh.Value);
      else if (sender == LSL.takeCocainOzVeh)
        LSL.vehStashTakeMenu.Subtitle.Caption = "Take ~r~" + LSL.takeCocainOzVeh.Value.ToString() + "~s~z";
      else if (sender == LSL.putCocainOzVeh)
        LSL.vehStashPutMenu.Subtitle.Caption = "Put ~r~" + LSL.putCocainOzVeh.Value.ToString() + "~s~z";
      else if (sender == LSL.putMoneyVeh)
        LSL.vehStashPutMenu.Subtitle.Caption = "Put $" + LsFunctions.IntToMoney(LSL.putMoneyVeh.Value);
      else if (sender == LSL.takeMoneyVeh)
        LSL.vehStashTakeMenu.Subtitle.Caption = "Take $" + LsFunctions.IntToMoney(LSL.takeMoneyVeh.Value);
      else if (sender == LSL.takeWeedStash)
        LSL.stashTakeMenu.Subtitle.Caption = "Take " + LsFunctions.GramsToOz(LSL.takeWeedStash.Value);
      else if (sender == LSL.putWeedStash)
        LSL.stashPutMenu.Subtitle.Caption = "Put " + LsFunctions.GramsToOz(LSL.putWeedStash.Value);
      else if (sender == LSL.takeCrackStash)
        LSL.stashTakeMenu.Subtitle.Caption = "Take " + LsFunctions.GramsToOz(LSL.takeCrackStash.Value);
      else if (sender == LSL.putCrackStash)
        LSL.stashPutMenu.Subtitle.Caption = "Put " + LsFunctions.GramsToOz(LSL.putCrackStash.Value);
      else if (sender == LSL.takeCocainStash)
        LSL.stashTakeMenu.Subtitle.Caption = "Take " + LsFunctions.GramsToOz(LSL.takeCocainStash.Value);
      else if (sender == LSL.putCocainStash)
        LSL.stashPutMenu.Subtitle.Caption = "Put " + LsFunctions.GramsToOz(LSL.putCocainStash.Value);
      else if (sender == LSL.takeMoneyStash)
        LSL.stashTakeMenu.Subtitle.Caption = "Take $" + LsFunctions.IntToMoney(LSL.takeMoneyStash.Value);
      else if (sender == LSL.putWeedOzStash)
        LSL.stashPutMenu.Subtitle.Caption = "Put ~r~" + LSL.putWeedOzStash.Value.ToString() + "~s~z";
      else if (sender == LSL.putCrackOzStash)
        LSL.stashPutMenu.Subtitle.Caption = "Put ~r~" + LSL.putCrackOzStash.Value.ToString() + "~s~z";
      else if (sender == LSL.putCocainOzStash)
        LSL.stashPutMenu.Subtitle.Caption = "Put ~r~" + LsFunctions.pCocaineOunce.ToString() + "~s~z";
      else if (sender == LSL.putMoneyStash)
        LSL.stashPutMenu.Subtitle.Caption = "Put $" + LsFunctions.IntToMoney(LSL.putMoneyStash.Value);
      else if (sender == LSL.bushWeed)
      {
        int _money = LSL.bushWeed.Value * (LSL.weed1Price - LsFunctions.Discount(LSL.weed1Price));
        LSL.WeedBuy.Text = "Order ~g~" + LSL.bushWeed.Value.ToString() + " ~s~for $" + LsFunctions.IntToMoney(_money);
      }
      else if (sender == LSL.crack)
      {
        int _money = LSL.crack.Value * (LSL.crack1Price - LsFunctions.Discount(LSL.crack1Price));
        LSL.CrackBuy.Text = "Order ~g~" + LSL.crack.Value.ToString() + " ~s~for $" + LsFunctions.IntToMoney(_money);
      }
      else
      {
        if (sender != LSL.cocaine)
          return;
        int _money = LSL.cocaine.Value * (LSL.cocain1Price - LsFunctions.Discount(LSL.cocain1Price));
        LSL.CocainBuy.Text = "Order ~g~" + LSL.cocaine.Value.ToString() + " ~s~for $" + LsFunctions.IntToMoney(_money);
      }
    }

    public static void onListChange(UIMenuListItem sender, int index)
    {
      if (sender == LSL.takeWeaponStash)
        LSL.stashTakeMenu.Subtitle.Caption = "Take " + sender.Items[LSL.takeWeaponStash.Index]?.ToString();
      else if (sender == LSL.putWeaponStash)
        LSL.stashPutMenu.Subtitle.Caption = "Put " + sender.Items[LSL.putWeaponStash.Index]?.ToString();
      else if (sender == LSL.takeWeaponVeh)
        LSL.vehStashTakeMenu.Subtitle.Caption = "Take " + sender.Items[LSL.takeWeaponVeh.Index]?.ToString();
      else if (sender == LSL.putWeaponVeh)
        LSL.vehStashPutMenu.Subtitle.Caption = "Put " + sender.Items[LSL.putWeaponVeh.Index]?.ToString();
      else if (sender == LSL.stashPutMultiplier)
      {
        UIMenuSliderItem putWeedStash = LSL.putWeedStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num1 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__0, LSL.multiplier[LSL.stashPutMultiplier.Index]);
        putWeedStash.Multiplier = num1;
        UIMenuSliderItem putCrackStash = LSL.putCrackStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num2 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__1.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__1, LSL.multiplier[LSL.stashPutMultiplier.Index]);
        putCrackStash.Multiplier = num2;
        UIMenuSliderItem putCocainStash = LSL.putCocainStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num3 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__2.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__2, LSL.multiplier[LSL.stashPutMultiplier.Index]);
        putCocainStash.Multiplier = num3;
        UIMenuSliderItem putMoneyStash = LSL.putMoneyStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num4 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__3.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__3, LSL.multiplier[LSL.stashPutMultiplier.Index]);
        putMoneyStash.Multiplier = num4;
        UIMenuSliderItem putWeedOzStash = LSL.putWeedOzStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num5 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__4.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__4, LSL.multiplier[LSL.stashPutMultiplier.Index]);
        putWeedOzStash.Multiplier = num5;
        UIMenuSliderItem putCrackOzStash = LSL.putCrackOzStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num6 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__5.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__5, LSL.multiplier[LSL.stashPutMultiplier.Index]);
        putCrackOzStash.Multiplier = num6;
        UIMenuSliderItem putCocainOzStash = LSL.putCocainOzStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num7 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__6.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__6, LSL.multiplier[LSL.stashPutMultiplier.Index]);
        putCocainOzStash.Multiplier = num7;
      }
      else if (sender == LSL.stashTakeMultiplier)
      {
        UIMenuSliderItem takeWeedStash = LSL.takeWeedStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num1 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__7.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__7, LSL.multiplier[LSL.stashTakeMultiplier.Index]);
        takeWeedStash.Multiplier = num1;
        UIMenuSliderItem takeCrackStash = LSL.takeCrackStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num2 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__8.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__8, LSL.multiplier[LSL.stashTakeMultiplier.Index]);
        takeCrackStash.Multiplier = num2;
        UIMenuSliderItem takeCocainStash = LSL.takeCocainStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num3 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__9.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__9, LSL.multiplier[LSL.stashTakeMultiplier.Index]);
        takeCocainStash.Multiplier = num3;
        UIMenuSliderItem takeMoneyStash = LSL.takeMoneyStash;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num4 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__10.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__10, LSL.multiplier[LSL.stashTakeMultiplier.Index]);
        takeMoneyStash.Multiplier = num4;
      }
      else if (sender == LSL.vehPutMultiplier)
      {
        UIMenuSliderItem putWeedVeh = LSL.putWeedVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num1 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__11.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__11, LSL.multiplier[LSL.vehPutMultiplier.Index]);
        putWeedVeh.Multiplier = num1;
        UIMenuSliderItem putCrackVeh = LSL.putCrackVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num2 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__12.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__12, LSL.multiplier[LSL.vehPutMultiplier.Index]);
        putCrackVeh.Multiplier = num2;
        UIMenuSliderItem putCocainVeh = LSL.putCocainVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num3 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__13.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__13, LSL.multiplier[LSL.vehPutMultiplier.Index]);
        putCocainVeh.Multiplier = num3;
        UIMenuSliderItem putMoneyVeh = LSL.putMoneyVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num4 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__14.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__14, LSL.multiplier[LSL.vehPutMultiplier.Index]);
        putMoneyVeh.Multiplier = num4;
        UIMenuSliderItem putWeedOzVeh = LSL.putWeedOzVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num5 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__15.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__15, LSL.multiplier[LSL.vehPutMultiplier.Index]);
        putWeedOzVeh.Multiplier = num5;
        UIMenuSliderItem putCrackOzVeh = LSL.putCrackOzVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num6 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__16.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__16, LSL.multiplier[LSL.vehPutMultiplier.Index]);
        putCrackOzVeh.Multiplier = num6;
        UIMenuSliderItem putCocainOzVeh = LSL.putCocainOzVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__17 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num7 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__17.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__17, LSL.multiplier[LSL.vehPutMultiplier.Index]);
        putCocainOzVeh.Multiplier = num7;
      }
      else if (sender == LSL.vehTakeMultiplier)
      {
        UIMenuSliderItem takeWeedVeh = LSL.takeWeedVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__18 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num1 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__18.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__18, LSL.multiplier[LSL.vehTakeMultiplier.Index]);
        takeWeedVeh.Multiplier = num1;
        UIMenuSliderItem takeCrackVeh = LSL.takeCrackVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__19 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num2 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__19.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__19, LSL.multiplier[LSL.vehTakeMultiplier.Index]);
        takeCrackVeh.Multiplier = num2;
        UIMenuSliderItem takeCocainVeh = LSL.takeCocainVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__20 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num3 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__20.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__20, LSL.multiplier[LSL.vehTakeMultiplier.Index]);
        takeCocainVeh.Multiplier = num3;
        UIMenuSliderItem takeMoneyVeh = LSL.takeMoneyVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__21 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num4 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__21.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__21, LSL.multiplier[LSL.vehTakeMultiplier.Index]);
        takeMoneyVeh.Multiplier = num4;
        UIMenuSliderItem takeWeedOzVeh = LSL.takeWeedOzVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__22 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num5 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__22.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__22, LSL.multiplier[LSL.vehTakeMultiplier.Index]);
        takeWeedOzVeh.Multiplier = num5;
        UIMenuSliderItem takeCrackOzVeh = LSL.takeCrackOzVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__23 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num6 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__23.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__23, LSL.multiplier[LSL.vehTakeMultiplier.Index]);
        takeCrackOzVeh.Multiplier = num6;
        UIMenuSliderItem takeCocainOzVeh = LSL.takeCocainOzVeh;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__24 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (LsMenus)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num7 = LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__24.Target((CallSite) LsMenus.\u003C\u003Eo__10.\u003C\u003Ep__24, LSL.multiplier[LSL.vehTakeMultiplier.Index]);
        takeCocainOzVeh.Multiplier = num7;
      }
      else if (sender == LSL.melee)
      {
        LSL.melee.Text = "Mele $" + LsFunctions.IntToMoney(LSL.meleePrice[LSL.melee.Index] - LsFunctions.Discount(LSL.meleePrice[LSL.melee.Index]));
      }
      else
      {
        if (sender != LSL.pistol)
          return;
        LSL.pistol.Text = "Pistol $" + LsFunctions.IntToMoney(LSL.pistolPrice[LSL.pistol.Index] - LsFunctions.Discount(LSL.pistolPrice[LSL.pistol.Index]));
      }
    }

    public static void onStashMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item == LSL.putWeedStash)
      {
        LSL.HouseHandler.CurrentHouse.Weed += LSL.putWeedStash.Value;
        LSL.PlayerInventory["Weed"] -= LSL.putWeedStash.Value;
      }
      else if (item == LSL.putCrackStash)
      {
        LSL.HouseHandler.CurrentHouse.Crack += LSL.putCrackStash.Value;
        LSL.PlayerInventory["Crack"] -= LSL.putCrackStash.Value;
      }
      else if (item == LSL.putCocainStash)
      {
        LSL.HouseHandler.CurrentHouse.Cocain += LSL.putCocainStash.Value;
        LSL.PlayerInventory["Cocaine"] -= LSL.putCocainStash.Value;
      }
      else if (item == LSL.putMoneyStash)
      {
        LSL.HouseHandler.CurrentHouse.Money += LSL.putMoneyStash.Value;
        Game.Player.Money -= LSL.putMoneyStash.Value;
      }
      else if (item == LSL.putWeedOzStash)
      {
        LSL.HouseHandler.CurrentHouse.Weed += LSL.putWeedOzStash.Value * 28;
        LsFunctions.pWeedOunce -= LSL.putWeedOzStash.Value;
      }
      else if (item == LSL.putCrackOzStash)
      {
        LSL.HouseHandler.CurrentHouse.Crack += LSL.putCrackOzStash.Value * 28;
        LsFunctions.pCrackOunce -= LSL.putCrackOzStash.Value;
      }
      else if (item == LSL.putCocainOzStash)
      {
        LSL.HouseHandler.CurrentHouse.Cocain += LSL.putCocainOzStash.Value * 28;
        LsFunctions.pCocaineOunce -= LSL.putCocainOzStash.Value;
      }
      else if (item == LSL.putArmourStash)
      {
        int index1 = LSL.putArmourStash.Index;
        if (index1 > 0 && LSL.playerArmours > 0)
        {
          LSL.playerArmours -= index1;
          LSL.HouseHandler.CurrentHouse.Armor += index1;
          foreach (XElement descendant in LSL.LSLifeSave.Descendants())
          {
            if (descendant.Name == (XName) "playerInventory")
            {
              using (IEnumerator<XElement> enumerator = descendant.Elements().GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  XElement current = enumerator.Current;
                  if (current.Name == (XName) "armour")
                  {
                    current.Value = LSL.playerArmours.ToString();
                    break;
                  }
                }
                break;
              }
            }
          }
          LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
        }
      }
      else if (item == LSL.putWeaponStash && LSL.putWeaponStash.Items.Count > 0)
      {
        int num1 = 0;
        int num2 = 0;
        if (LSL.HouseHandler.CurrentHouse.Weapons.Count > 0)
        {
          foreach (XElement weapon in LSL.HouseHandler.CurrentHouse.Weapons)
          {
            if (weapon.Name == (XName) "weapon")
            {
              ++num1;
              if (num1.ToString() != weapon.FirstAttribute.Value)
                num2 = num1;
            }
          }
        }
        XElement playerWeapon = LSL.playerWeapons[LSL.putWeaponStash.Index];
        if (playerWeapon != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (LsMenus.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LsMenus.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, WeaponHash>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "TryParse", (IEnumerable<Type>) null, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
            }));
          }
          WeaponHash weaponHash;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) LsMenus.\u003C\u003Eo__11.\u003C\u003Ep__0, typeof (Enum), LSL.playerWeps[LSL.putWeaponStash.Index], ref weaponHash);
          if (weaponHash != WeaponHash.Unarmed)
          {
            playerWeapon.SetAttributeValue((XName) "id", (object) num2);
            LSL.HouseHandler.CurrentHouse.Weapons.Add(playerWeapon);
            LSL.HouseHandler.CurrentHouse.Weps.Add(LSL.putWeaponStash.Items[LSL.putWeaponStash.Index]);
            Game.Player.Character.Weapons.Remove(weaponHash);
            LSL.playerWeapons.Remove(playerWeapon);
            LSL.playerWeps.RemoveAt(LSL.putWeaponStash.Index);
          }
        }
      }
      LsFunctions.UpdateInventory();
      LSL.HouseHandler.UpdateMenu(LSL.HouseHandler.CurrentHouse);
      LSL.HouseHandler.SaveHouse(LSL.HouseHandler.CurrentHouse);
    }

    public static void onPlayerMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item == LSL.takeWeedStash)
      {
        LSL.PlayerInventory["Weed"] += LSL.takeWeedStash.Value;
        LSL.HouseHandler.CurrentHouse.Weed -= LSL.takeWeedStash.Value;
      }
      else if (item == LSL.takeCrackStash)
      {
        LSL.PlayerInventory["Crack"] += LSL.takeCrackStash.Value;
        LSL.HouseHandler.CurrentHouse.Crack -= LSL.takeCrackStash.Value;
      }
      else if (item == LSL.takeCocainStash)
      {
        LSL.PlayerInventory["Cocaine"] += LSL.takeCocainStash.Value;
        LSL.HouseHandler.CurrentHouse.Cocain -= LSL.takeCocainStash.Value;
      }
      else if (item == LSL.takeMoneyStash)
      {
        Game.Player.Money += LSL.takeMoneyStash.Value;
        LSL.HouseHandler.CurrentHouse.Money -= LSL.takeMoneyStash.Value;
      }
      else if (item == LSL.takeArmourStash)
      {
        int index1 = LSL.takeArmourStash.Index;
        int num = 5;
        if (Game.Player.Character.Armor < 100)
          num = 6;
        if (index1 > 0 && LSL.HouseHandler.CurrentHouse.Armor > 0 && index1 + LSL.playerArmours <= num)
        {
          if (Game.Player.Character.Armor < 100)
          {
            Game.Player.Character.Armor = 100;
            --index1;
          }
          LSL.playerArmours += index1;
          LSL.HouseHandler.CurrentHouse.Armor -= index1;
          foreach (XElement descendant in LSL.LSLifeSave.Descendants())
          {
            if (descendant.Name == (XName) "playerInventory")
            {
              using (IEnumerator<XElement> enumerator = descendant.Elements().GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  XElement current = enumerator.Current;
                  if (current.Name == (XName) "armour")
                  {
                    current.Value = LSL.playerArmours.ToString();
                    break;
                  }
                }
                break;
              }
            }
          }
          LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
        }
      }
      else if (item == LSL.takeWeaponStash && LSL.takeWeaponStash.Items.Count > 0)
      {
        XElement weapon = LSL.HouseHandler.CurrentHouse.Weapons[LSL.takeWeaponStash.Index];
        XElement xelement = (XElement) null;
        foreach (XElement element in weapon.Elements())
        {
          if (element.Name == (XName) "HASH")
            xelement = element;
        }
        if (xelement != null)
        {
          WeaponHash result1;
          Enum.TryParse<WeaponHash>(xelement.Value, out result1);
          if (result1 != WeaponHash.Unarmed)
          {
            bool flag = false;
            int result2 = 0;
            foreach (XElement descendant in weapon.Descendants())
            {
              if (descendant.Name == (XName) "AMMO")
              {
                int.TryParse(descendant.Value, out result2);
                break;
              }
            }
            if (!Game.Player.Character.Weapons.HasWeapon(result1))
            {
              Game.Player.Character.Weapons.Give(result1, result2, true, true);
              Game.Player.Character.Weapons.Select(result1);
            }
            else
            {
              flag = true;
              Game.Player.Character.Weapons.Select(result1);
              Game.Player.Character.Weapons.Current.Ammo += result2;
            }
            if (!flag)
            {
              LSL.playerWeapons.Add(weapon);
              LSL.playerWeps.Add((object) xelement.Value);
              foreach (XElement descendant in weapon.Descendants())
              {
                if (descendant.Name == (XName) "COMPONENT")
                {
                  WeaponComponent result3;
                  Enum.TryParse<WeaponComponent>(descendant.Value, out result3);
                  Game.Player.Character.Weapons.Current.SetComponent(result3, true);
                }
              }
            }
            LSL.HouseHandler.CurrentHouse.Weapons.Remove(weapon);
            LSL.HouseHandler.CurrentHouse.Weps.RemoveAt(LSL.takeWeaponStash.Index);
          }
        }
      }
      LsFunctions.UpdateInventory();
      LSL.HouseHandler.UpdateMenu(LSL.HouseHandler.CurrentHouse);
      LSL.HouseHandler.SaveHouse(LSL.HouseHandler.CurrentHouse);
    }

    public static void onCustomerMenuSelect(UIMenu sender, UIMenuItem item, int index)
    {
      if (item == LSL.sell)
      {
        LSL.jobOfferTimer = Game.GameTime;
        if (LSL.PlayerInventory[LSL.customerSellTo.Drug.Name] < LSL.customerSellTo.amount)
          return;
        foreach (Drug drug in LSL.areas[LSL.areaIndex].Drugs)
        {
          if (drug.Name == LSL.customerSellTo.Drug.Name)
            drug.Supplied = true;
        }
        if (LSL.customerSellTo.FarAway && LSL.jobAccepted)
          LSL.jobAccepted = false;
        if (Game.Player.Character.IsInVehicle())
        {
          LsFunctions.MakeStreetSale(LSL.customerSellTo, true);
          LSL.customerMainMenu.Visible = !LSL.customerMainMenu.Visible;
          LSL.lastTimeDealDone = (float) Game.GameTime;
        }
        else
        {
          LsFunctions.MakeStreetSale(LSL.customerSellTo, false);
          LSL.customerMainMenu.Visible = !LSL.customerMainMenu.Visible;
          LSL.lastTimeDealDone = (float) Game.GameTime;
        }
      }
      else
      {
        if (item != LSL.turnAway)
          return;
        foreach (Drug drug in LSL.areas[LSL.areaIndex].Drugs)
        {
          if (drug.Name == LSL.customerSellTo.Drug.Name)
            drug.Supplied = false;
        }
        LSL.jobOfferTimer = Game.GameTime;
        LSL.customerMainMenu.Visible = !LSL.customerMainMenu.Visible;
        if (LSL.customerSellTo.FarAway)
        {
          if (LSL.jobAccepted)
            LSL.jobAccepted = false;
          LSL.jobs.Remove(LSL.customerSellTo);
          LSL.lastTimeDealDone = (float) Game.GameTime;
          OldCustomerHandler.AddCustomer(LSL.customerSellTo.Ped, LSL.HouseHandler.Inside(), true, new KeyValuePair<string, int>(LSL.customerSellTo.Drug.Name, LSL.customerSellTo.amount));
        }
        else
        {
          if ((Entity) LSL.customerSellTo.Vehicle != (Entity) null && LSL.customerSellTo.Vehicle.Exists())
            LSL.customerSellTo.Ped.Task.PerformSequence(LSL.PerformTaskSeq(1, LSL.customerSellTo.Vehicle, Vector3.Zero));
          LSL.customers.Remove(LSL.customerSellTo);
          LSL.lastTimeDealDone = (float) Game.GameTime;
          OldCustomerHandler.AddCustomer(LSL.customerSellTo.Ped, LSL.HouseHandler.Inside(), true, new KeyValuePair<string, int>(LSL.customerSellTo.Drug.Name, LSL.customerSellTo.amount));
          LSL.customerSellTo = (Customer) null;
        }
      }
    }

    public static void HireDealerFollowSelect(UIMenu sender, UIMenuItem selectedItem)
    {
      LSL.hireDealerMainMenu.Visible = false;
      if (LSL.aDealer == null)
        return;
      LSL.aDealer.NewState = Enums.DStates.Normal;
    }

    public static void HireDealerOfferSelect(UIMenu sender, UIMenuItem selectedItem)
    {
      LSL.hireDealerMainMenu.Visible = false;
      if (LSL.aDealer == null)
        return;
      LSL.aDealer.NewState = Enums.DStates.Normal;
      if (LSL.aDealer.LastOffer < LSL.hireDealerOfferAmount.Value)
      {
        if (LSL.aDealer.DecidePlayerOffer(LSL.hireDealerOfferAmount.Value))
        {
          Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) LSL.aDealer.dPed, (InputArgument) "GENERIC_YES", (InputArgument) "SPEECH_PARAMS_FORCE");
          LsFunctions.ChangeAdealerToPdealer();
          UI.ShowSubtitle("Sure i will work for you.", 4000);
          Game.Player.Money -= LSL.hireDealerOfferAmount.Value;
          LsFunctions.PlayTheSound();
          LSL.hireDealerOfferAmount.Maximum = Game.Player.Money;
          if (LSL.driveByHandler.AmountOfDriveBy(true) != 0 || LSL.rnd.Next(0, 5) != 0)
            return;
          LsFunctions.SpawnDriveBy(LSL.player.Character, true);
        }
        else if (LSL.rnd.Next(0, 3) == 0)
        {
          LSL.aDealer.NewState = Enums.DStates.AttackPlayer;
          Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) LSL.aDealer.dPed, (InputArgument) "GENERIC_CURSE_HIGH", (InputArgument) "SPEECH_PARAMS_FORCE");
          if (LSL.driveByHandler.AmountOfDriveBy(true) != 0 || LSL.rnd.Next(0, 5) != 0)
            return;
          LsFunctions.SpawnDriveBy(LSL.player.Character, true);
        }
        else
        {
          Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) LSL.aDealer.dPed, (InputArgument) "GENERIC_NO", (InputArgument) "SPEECH_PARAMS_FORCE");
          UI.ShowSubtitle("Your kidding me, Im worth more than that!", 4000);
          LSL.aDealer.SetLastOfferAmount(LSL.hireDealerOfferAmount.Value);
        }
      }
      else if (LSL.rnd.Next(0, 3) == 0)
      {
        LSL.aDealer.NewState = Enums.DStates.AttackPlayer;
        Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) LSL.aDealer.dPed, (InputArgument) "GENERIC_CURSE_HIGH", (InputArgument) "SPEECH_PARAMS_FORCE");
        UI.ShowSubtitle("Nah, im just gona kill you and take your money.", 4000);
        if (LSL.driveByHandler.AmountOfDriveBy(true) != 0 || LSL.rnd.Next(0, 5) != 0)
          return;
        LsFunctions.SpawnDriveBy(LSL.player.Character, true);
      }
      else
        UI.ShowSubtitle("Offer me more and i might accept.", 4000);
    }

    public static void HireDealerOfferAmount_OnSliderChanged(UIMenuSliderItem sender, int newIndex) => LSL.hireDealerMakeOffer.Text = "Offer $" + LsFunctions.IntToMoney(sender.Value);

    public static void WArmour_Activated(UIMenu sender, UIMenuItem selectedItem)
    {
      if (LSL.playerArmours <= 0)
        return;
      LsFunctions.HandOverGoods(LSL.player.Character, LSL.DealerHandler.currentDealer.Ped);
      PlayerDealer currentDealer = LSL.DealerHandler.currentDealer;
      int num1;
      if (currentDealer == null)
      {
        num1 = 0;
      }
      else
      {
        int? armor = currentDealer.Ped?.Armor;
        int num2 = 200;
        num1 = armor.GetValueOrDefault() < num2 & armor.HasValue ? 1 : 0;
      }
      if (num1 != 0)
      {
        LSL.DealerHandler.currentDealer.Ped.Armor = 200;
        LSL.DealerHandler.currentDealer.dArmour = LSL.DealerHandler.currentDealer.Ped.Armor;
        --LSL.playerArmours;
        if (LSL.playerArmours == 0)
          selectedItem.Text = "No armour left";
        LsFunctions.PlayTheSound();
        LsFunctions.UpdateInventory();
        LsFunctions.SetWorkerMenuHealth(LSL.DealerHandler.currentDealer.Ped);
      }
      else
        UI.ShowSubtitle("I already have enough armour.", 4000);
    }

    public static void WWeapons_Activated(UIMenu sender, UIMenuItem selectedItem)
    {
      if (LSL.DEBUG)
        UI.Notify("Transfer weapon");
      if (LSL.wWeapons.Items.Count > 0)
      {
        LsFunctions.HandOverGoods(LSL.player.Character, LSL.DealerHandler.currentDealer.Ped);
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p1 = LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__0 = CallSite<\u003C\u003EF\u007B00000008\u007D<CallSite, Type, object, WeaponHash, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "TryParse", (IEnumerable<Type>) null, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
          }));
        }
        WeaponHash weaponHash;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__0.Target((CallSite) LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__0, typeof (Enum), LSL.playerWeps[LSL.wWeapons.Index], ref weaponHash);
        if (target1((CallSite) p1, obj1))
        {
          if (weaponHash == WeaponHash.Unarmed)
            return;
          XElement playerWeapon = LSL.playerWeapons[LSL.wWeapons.Index];
          if (playerWeapon != null)
          {
            XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Dealers.xml");
            if (xdocument.Descendants().Count<XElement>() <= 0)
              return;
            foreach (XElement descendant1 in xdocument.Descendants())
            {
              if (descendant1.Name == (XName) "dealer")
              {
                bool flag1 = false;
                foreach (XElement element in descendant1.Elements())
                {
                  if (element.Name == (XName) "id" && element.Value == LSL.DealerHandler.currentDealer.ID.ToString())
                  {
                    flag1 = true;
                    break;
                  }
                }
                if (flag1)
                {
                  bool flag2 = false;
                  XElement xelement = (XElement) null;
                  foreach (XElement descendant2 in descendant1.Descendants())
                  {
                    if (descendant2.Name == (XName) "HASH" && descendant2.Value == weaponHash.ToString())
                    {
                      UI.Notify("Dealer found " + weaponHash.ToString());
                      flag2 = true;
                      xelement = descendant2.Parent;
                      break;
                    }
                  }
                  if (flag2)
                    xelement.ReplaceWith((object) playerWeapon);
                  else
                    descendant1.Add((object) playerWeapon);
                  xdocument.Save("scripts\\LSLife\\LSLife_Dealers.xml");
                  int result1 = 1000;
                  foreach (XElement descendant2 in playerWeapon.Descendants())
                  {
                    if (descendant2.Name == (XName) "AMMO")
                    {
                      int.TryParse(descendant2.Value, out result1);
                      LSL.DealerHandler.currentDealer?.Ped.Weapons.Give(weaponHash, result1, true, true);
                      UI.Notify("Gave dealer " + weaponHash.ToString());
                      LSL.DealerHandler.currentDealer?.Ped?.Weapons.Select(weaponHash);
                      LsFunctions.PlayTheSound();
                    }
                    WeaponComponent result2;
                    if (descendant2.Name == (XName) "COMPONENT" && Enum.TryParse<WeaponComponent>(descendant2.Value, out result2))
                      LSL.DealerHandler.currentDealer?.Ped?.Weapons.Current.SetComponent(result2, true);
                  }
                  Game.Player.Character.Weapons.Remove(weaponHash);
                  LSL.playerWeapons.Remove(playerWeapon);
                  LSL.playerWeps.RemoveAt(LSL.wWeapons.Index);
                  LSL.wWeapons.Items = LSL.playerWeps;
                  LSL.wWeapons.Index = 0;
                  if (!LSL.DEBUG)
                    break;
                  UI.Notify("Saving Dealer Weapon " + weaponHash.ToString() + " ammo:" + result1.ToString());
                  break;
                }
              }
            }
          }
          else
          {
            if (!LSL.DEBUG)
              return;
            UI.Notify("No match found in playerWeapons");
          }
        }
        else
        {
          if (!LSL.DEBUG)
            return;
          // ISSUE: reference to a compiler-generated field
          if (LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__3 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Notify", (IEnumerable<Type>) null, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, Type, object> target2 = LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, Type, object>> p3 = LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__3;
          Type type = typeof (UI);
          // ISSUE: reference to a compiler-generated field
          if (LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__2 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (LsMenus), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__2.Target((CallSite) LsMenus.\u003C\u003Eo__18.\u003C\u003Ep__2, "No such Hash found ", LSL.playerWeps[LSL.wWeapons.Index]);
          target2((CallSite) p3, type, obj2);
        }
      }
      else
      {
        if (!LSL.DEBUG)
          return;
        UI.Notify("NO weapons found");
      }
    }

    internal static void WOrder_Activated(UIMenu sender, UIMenuItem selectedItem)
    {
      if (LSL.DealerHandler.currentDealer == null)
        return;
      Enum.TryParse<Enums.PdStates>(LSL.wOrder.Items[LSL.wOrder.Index].ToString(), out LSL.DealerHandler.currentDealer.NewState);
      LSL.wOrder.Items = LSL.DealerHandler.currentDealer.Orders();
      LSL.wOrder.Index = 0;
    }

    public static void WConfirm_Activated(UIMenu sender, UIMenuItem selectedItem)
    {
      int index = 0;
      foreach (PlayerDealer dealer in LSL.DealerHandler.dealers)
      {
        if (dealer != LSL.DealerHandler.currentDealer)
          ++index;
        else
          break;
      }
      bool flag = false;
      if (LSL.wWeed.Value > 0)
      {
        LSL.DealerHandler.dealers[index].Drugs["Weed"] += LSL.wWeed.Value;
        LSL.PlayerInventory["Weed"] -= LSL.wWeed.Value;
        LSL.wWeed.Value = 0;
        LSL.wWeed.Text = "Weed " + LsFunctions.GramsToOz(LSL.wWeed.Value);
        flag = true;
      }
      if (LSL.wCrack.Value > 0)
      {
        LSL.DealerHandler.dealers[index].Drugs["Crack"] += LSL.wCrack.Value;
        LSL.PlayerInventory["Crack"] -= LSL.wCrack.Value;
        LSL.wCrack.Value = 0;
        LSL.wCrack.Text = "Crack " + LsFunctions.GramsToOz(LSL.wCrack.Value);
        flag = true;
      }
      if (LSL.wCocaine.Value > 0)
      {
        LSL.DealerHandler.dealers[index].Drugs["Cocaine"] += LSL.wCocaine.Value;
        LSL.PlayerInventory["Cocaine"] -= LSL.wCocaine.Value;
        LSL.wCocaine.Value = 0;
        LSL.wCocaine.Text = "Coke " + LsFunctions.GramsToOz(LSL.wCocaine.Value);
        flag = true;
      }
      if (!flag)
        return;
      LsFunctions.HandOverGoods(LSL.player.Character, LSL.DealerHandler.currentDealer.Ped);
      LsFunctions.UpdateInventory();
      LSL.DealerHandler.dealers[index].SetDrugDesc();
      LSL.DealerHandler.needsSave = true;
    }

    public static void WCollect_Activated(UIMenu sender, UIMenuItem selectedItem)
    {
      int index = 0;
      foreach (PlayerDealer dealer in LSL.DealerHandler.dealers)
      {
        if (dealer != LSL.DealerHandler.currentDealer)
          ++index;
        else
          break;
      }
      if (LSL.DealerHandler.dealers[index].Money < 100)
        return;
      LsFunctions.HandOverGoods(LSL.player.Character, LSL.DealerHandler.currentDealer.Ped);
      LSL.DealerHandler.dealers[index].GiveMoneyToPlayer();
      LSL.DealerHandler.dealers[index].SetDrugDesc();
      LSL.WorkerMainMenu.CurrentSelection = 3;
    }

    public static void WCocaine_OnSliderChanged(UIMenuSliderItem sender, int newIndex) => LSL.wCocaine.Text = "Coke " + LsFunctions.GramsToOz(sender.Value);

    public static void WCrack_OnSliderChanged(UIMenuSliderItem sender, int newIndex) => LSL.wCrack.Text = "Crack " + LsFunctions.GramsToOz(sender.Value);

    public static void WWeed_OnSliderChanged(UIMenuSliderItem sender, int newIndex) => LSL.wWeed.Text = "Weed " + LsFunctions.GramsToOz(sender.Value);
  }
}
