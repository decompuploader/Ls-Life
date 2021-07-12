using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace LSlife
{
  public class StashVehicle
  {
    public bool drugCarWanted;
    public bool drugCarDestroyed = true;
    private bool drugCarSaved = true;
    public Blip drugCarBlip;
    public int drugCarWeed;
    public int drugCarWeedOz;
    public int drugCarCrack;
    public int drugCarCrackOz;
    public int drugCarCocain;
    public int drugCarCocainOz;
    public int drugCarMoney;
    public int drugCarArmour;
    public Vehicle drugCar;
    public Vector3 CurrentPosition;
    private string reg;
    private int drugCarHash = -1;
    public List<XElement> vehWeapons = new List<XElement>();
    public List<object> vehWeps = new List<object>();

    public StashVehicle()
    {
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "drugCarStash")
        {
          bool flag1 = false;
          bool flag2 = false;
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "weed")
              int.TryParse(element.Value, out this.drugCarWeed);
            if (element.Name == (XName) "weedOz")
            {
              int.TryParse(element.Value, out this.drugCarWeedOz);
              flag2 = true;
            }
            if (element.Name == (XName) "crack")
              int.TryParse(element.Value, out this.drugCarCrack);
            if (element.Name == (XName) "crackOz")
            {
              int.TryParse(element.Value, out this.drugCarCrackOz);
              flag2 = true;
            }
            if (element.Name == (XName) "cocain")
              int.TryParse(element.Value, out this.drugCarCocain);
            if (element.Name == (XName) "cocainOz")
            {
              int.TryParse(element.Value, out this.drugCarCocainOz);
              flag2 = true;
            }
            if (element.Name == (XName) "money")
              int.TryParse(element.Value, out this.drugCarMoney);
            if (element.Name == (XName) "armour")
            {
              flag1 = true;
              int.TryParse(element.Value, out this.drugCarArmour);
            }
          }
          if (!flag1)
          {
            descendant.Add((object) new XElement((XName) "armour", (object) 0));
            LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
          }
          if (!flag2)
          {
            descendant.Add((object) new XElement((XName) "weedOz", (object) 0));
            descendant.Add((object) new XElement((XName) "crackOz", (object) 0));
            descendant.Add((object) new XElement((XName) "cocainOz", (object) 0));
            LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
          }
        }
      }
      foreach (XElement descendant in LSL.VehicleXml.Descendants())
      {
        if (descendant.Name == (XName) "Vehicle")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "Destroyed")
              bool.TryParse(element.Value, out this.drugCarDestroyed);
            if (element.Name == (XName) "Hash")
              int.TryParse(element.Value, out this.drugCarHash);
            if (element.Name == (XName) "PosX")
            {
              float.TryParse(element.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out this.CurrentPosition.X);
              if ((double) this.CurrentPosition.X == 0.0)
                float.TryParse(element.Value, out this.CurrentPosition.X);
            }
            if (element.Name == (XName) "PosY")
            {
              float.TryParse(element.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out this.CurrentPosition.Y);
              if ((double) this.CurrentPosition.Y == 0.0)
                float.TryParse(element.Value, out this.CurrentPosition.Y);
            }
            if (element.Name == (XName) "PosZ")
            {
              float.TryParse(element.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out this.CurrentPosition.Z);
              if ((double) this.CurrentPosition.Z == 0.0)
                float.TryParse(element.Value, out this.CurrentPosition.Z);
            }
            if (element.Name == (XName) "NumPlate")
              this.reg = element.Value;
          }
        }
      }
      foreach (XElement descendant in LSL.vehWeaponsXML.Descendants())
      {
        if (descendant.Name == (XName) "weapon")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "HASH")
            {
              this.vehWeapons.Add(descendant);
              this.vehWeps.Add((object) element.Value);
            }
          }
        }
      }
    }

    public void OnTick()
    {
      if (!((Entity) this.drugCar != (Entity) null) || !this.drugCar.Exists())
        return;
      Vector3 vector3 = Vector3.Add(this.drugCar.Position, Vector3.Multiply(this.drugCar.ForwardVector * -1f, (float) ((double) this.drugCar.Model.GetDimensions().Y / 2.0 + 0.5)));
      if (LSL.DrawMarkers && (Entity) this.drugCar.Driver != (Entity) LSL.player.Character && (double) LSL.playerPos.DistanceTo(this.drugCar.Position) < 10.0)
        World.DrawMarker(MarkerType.UpsideDownCone, vector3, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 0.5f), Color.Green);
      if (!this.drugCar.CurrentBlip.IsShortRange)
      {
        this.drugCar.CurrentBlip.IsShortRange = true;
        this.drugCar.CurrentBlip.Sprite = BlipSprite.PersonalVehicleCar;
        this.drugCar.CurrentBlip.Name = "Drug Car";
        this.drugCar.CurrentBlip.Color = BlipColor.Green;
      }
      if (LSL.player.Character.IsInVehicle() || (double) LSL.player.Character.Position.DistanceTo(vector3) > 1.0)
        return;
      bool flag = false;
      if (this.drugCar.HasBone("boot"))
        flag = true;
      if (flag)
      {
        if (!this.drugCar.IsDoorOpen(VehicleDoor.Trunk))
        {
          if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 5000.0 && !LSL.LsMenuPool.IsAnyMenuOpen() && LSL.player.Character.IsStopped)
          {
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("~INPUT_SPRINT~ to open trunk.");
          }
          if (!Game.IsControlJustPressed(2, Control.Sprint) || LSL.LsMenuPool.IsAnyMenuOpen() || !LSL.player.Character.IsStopped)
            return;
          this.drugCar.OpenDoor(VehicleDoor.Trunk, false, false);
          if (LSL.PlayerInventory.Count > 0)
          {
            LSL.putWeedVeh.Maximum = LSL.PlayerInventory["Weed"];
            LSL.putCrackVeh.Maximum = LSL.PlayerInventory["Crack"];
            LSL.putCocainVeh.Maximum = LSL.PlayerInventory["Cocaine"];
          }
          LSL.putMoneyVeh.Maximum = LSL.player.Money;
          LSL.takeWeaponVeh.Items = this.vehWeps;
          LSL.takeMoneyVeh.Maximum = this.drugCarMoney;
          LSL.putArmourVeh.Items = new List<object>();
          for (int index = 0; index < LSL.playerArmours + 1; ++index)
            LSL.putArmourVeh.Items.Add((object) index);
          LSL.takeArmourVeh.Items = new List<object>();
          for (int index = 0; index < this.drugCarArmour + 1; ++index)
            LSL.takeArmourVeh.Items.Add((object) index);
          this.UpdateCarStash();
          LSL.vehStashMainMenu.Visible = !LSL.vehStashMainMenu.Visible;
        }
        else
        {
          if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 5000.0 && !LSL.LsMenuPool.IsAnyMenuOpen() && LSL.player.Character.IsStopped)
          {
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("~INPUT_SPRINT~ to close trunk.");
          }
          if (!Game.IsControlJustPressed(2, Control.Sprint) || LSL.LsMenuPool.IsAnyMenuOpen() || !LSL.player.Character.IsStopped)
            return;
          this.drugCar.CloseDoor(VehicleDoor.Trunk, false);
        }
      }
      else if (!this.drugCar.IsDoorOpen(VehicleDoor.BackLeftDoor) && !LSL.LsMenuPool.IsAnyMenuOpen() && LSL.player.Character.IsStopped)
      {
        if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0)
        {
          LSL.displayHelpTimer = (float) Game.GameTime;
          LsFunctions.DisplayHelpText("~INPUT_SPRINT~ to back doors.");
        }
        if (!Game.IsControlJustPressed(2, Control.Sprint) || LSL.LsMenuPool.IsAnyMenuOpen() || !LSL.player.Character.IsStopped)
          return;
        this.drugCar.OpenDoor(VehicleDoor.BackLeftDoor, true, false);
        this.drugCar.OpenDoor(VehicleDoor.BackRightDoor, true, false);
        if (LSL.PlayerInventory.Count > 0)
        {
          LSL.putWeedVeh.Maximum = LSL.PlayerInventory["Weed"];
          LSL.putCrackVeh.Maximum = LSL.PlayerInventory["Crack"];
          LSL.putCocainVeh.Maximum = LSL.PlayerInventory["Cocaine"];
        }
        LSL.putMoneyVeh.Maximum = LSL.player.Money;
        LSL.takeWeaponVeh.Items = this.vehWeps;
        LSL.takeMoneyVeh.Maximum = this.drugCarMoney;
        LSL.putArmourVeh.Items = new List<object>();
        for (int index = 0; index < LSL.playerArmours + 1; ++index)
          LSL.putArmourVeh.Items.Add((object) index);
        LSL.takeArmourVeh.Items = new List<object>();
        for (int index = 0; index < this.drugCarArmour + 1; ++index)
          LSL.takeArmourVeh.Items.Add((object) index);
        this.UpdateCarStash();
        LSL.vehStashMainMenu.Visible = !LSL.vehStashMainMenu.Visible;
      }
      else
      {
        if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0 && !LSL.LsMenuPool.IsAnyMenuOpen() && LSL.player.Character.IsStopped)
        {
          LSL.displayHelpTimer = (float) Game.GameTime;
          LsFunctions.DisplayHelpText("~INPUT_SPRINT~ to close back doors.");
        }
        if (!Game.IsControlJustPressed(2, Control.Sprint) || LSL.LsMenuPool.IsAnyMenuOpen() || !LSL.player.Character.IsStopped)
          return;
        this.drugCar.CloseDoor(VehicleDoor.BackLeftDoor, false);
        this.drugCar.CloseDoor(VehicleDoor.BackRightDoor, false);
      }
    }

    public void OnDelayedTick()
    {
      if (!this.drugCarDestroyed && ((Entity) this.drugCar == (Entity) null || !this.drugCar.Exists()))
      {
        if ((double) LSL.playerPos.DistanceTo(this.CurrentPosition) <= 200.0)
        {
          if (this.drugCarBlip != (Blip) null)
          {
            this.drugCarBlip.Remove();
            this.drugCarBlip = (Blip) null;
          }
          foreach (Vehicle allVehicle in World.GetAllVehicles())
          {
            if ((double) allVehicle.Position.DistanceTo(this.CurrentPosition) < 6.0 && allVehicle.NumberPlate == this.reg && allVehicle.Model.Hash == this.drugCarHash)
            {
              if (allVehicle.CurrentBlip != (Blip) null)
                allVehicle.CurrentBlip.Remove();
              this.drugCar = allVehicle;
              this.drugCar.IsPersistent = true;
              this.drugCar.EngineHealth = 1000f;
              this.drugCar.Health = 1000;
              this.drugCar.Repair();
              if (this.drugCar.CurrentBlip != (Blip) null)
                this.drugCar.CurrentBlip.Remove();
              this.drugCar.AddBlip();
              this.drugCar.CurrentBlip.IsShortRange = true;
              this.drugCar.CurrentBlip.IsFriendly = true;
              this.drugCar.CurrentBlip.Sprite = BlipSprite.PersonalVehicleCar;
              this.drugCar.CurrentBlip.Name = "Drug Car";
              this.drugCar.CurrentBlip.Color = BlipColor.Green;
            }
          }
          if ((Entity) this.drugCar == (Entity) null || !this.drugCar.Exists())
          {
            if (LSL.DEBUG)
              UI.Notify("Trying to load vehicle");
            this.VehicleLoad();
          }
        }
        else if (this.drugCarBlip == (Blip) null)
        {
          this.drugCarBlip = World.CreateBlip(this.CurrentPosition);
          this.drugCarBlip.IsShortRange = true;
          this.drugCarBlip.IsFriendly = true;
          this.drugCarBlip.Sprite = BlipSprite.PersonalVehicleCar;
          this.drugCarBlip.Name = "Drug Car";
          this.drugCarBlip.Color = BlipColor.Green;
        }
      }
      if (!((Entity) this.drugCar != (Entity) null) || !this.drugCar.Exists())
        return;
      if (this.drugCar.CurrentBlip.Exists() && (Entity) this.drugCar.Driver == (Entity) LSL.player.Character)
        this.drugCar.CurrentBlip.Remove();
      if (LSL.player.Character.IsInVehicle())
      {
        if (LSL.playerWanted && this.drugCar.Handle == LSL.player.Character.CurrentVehicle.Handle && !this.drugCarWanted)
          this.drugCarWanted = true;
        if (this.drugCarSaved && (Entity) this.drugCar.Driver == (Entity) LSL.player.Character)
        {
          this.CurrentPosition = this.drugCar.Position;
          this.drugCarSaved = false;
          if (this.drugCar.IsInvincible)
            this.drugCar.IsInvincible = false;
        }
      }
      if (!LSL.playerWanted && this.drugCarWanted)
        this.drugCarWanted = false;
      if (!this.drugCarSaved && this.CurrentPosition != this.drugCar.Position && !LSL.player.Character.IsInVehicle())
      {
        this.CurrentPosition = this.drugCar.Position;
        this.VehicleSave(this.drugCar);
        this.drugCarSaved = true;
      }
      if (this.drugCar.IsDead || (double) this.drugCar.EngineHealth <= -500.0)
      {
        LsFunctions.DbugString("Vehicle Destroyed");
        this.drugCarWanted = false;
        this.drugCar.IsPersistent = false;
        this.drugCar.MarkAsNoLongerNeeded();
        this.drugCar.CurrentBlip.Remove();
        this.drugCar = (Vehicle) null;
        this.drugCarDestroyed = true;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "drugCarStash")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weed")
                element.Value = "0";
              if (element.Name == (XName) "crack")
                element.Value = "0";
              if (element.Name == (XName) "cocain")
                element.Value = "0";
              if (element.Name == (XName) "money")
                element.Value = "0";
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
        XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Vehicles.xml");
        foreach (XElement descendant in xdocument.Descendants())
        {
          if (descendant.Name == (XName) "Destroyed")
            descendant.Value = this.drugCarDestroyed.ToString();
        }
        xdocument.Save("scripts\\LSLife\\LSLife_Vehicles.xml");
      }
      if (this.drugCarDestroyed || !((Entity) this.drugCar != (Entity) null) || ((double) LSL.playerPos.DistanceTo(this.CurrentPosition) <= 200.0 || !this.drugCar.Exists()))
        return;
      if (LSL.player.Character.IsInVehicle() && (Entity) LSL.player.Character.CurrentVehicle != (Entity) this.drugCar)
      {
        if (this.drugCarBlip == (Blip) null)
        {
          this.drugCarBlip = World.CreateBlip(this.drugCar.Position);
          this.drugCarBlip.IsShortRange = true;
          this.drugCarBlip.IsFriendly = true;
          this.drugCarBlip.Sprite = BlipSprite.PersonalVehicleCar;
          this.drugCarBlip.Name = "Drug Car";
          this.drugCarBlip.Color = BlipColor.Green;
        }
        this.drugCar.CurrentBlip.Remove();
        this.drugCar.Delete();
        this.drugCar = (Vehicle) null;
      }
      if (LSL.player.Character.IsInVehicle())
        return;
      if (this.drugCarBlip == (Blip) null)
      {
        this.drugCarBlip = World.CreateBlip(this.drugCar.Position);
        this.drugCarBlip.IsShortRange = true;
        this.drugCarBlip.IsFriendly = true;
        this.drugCarBlip.Sprite = BlipSprite.PersonalVehicleCar;
        this.drugCarBlip.Name = "Drug Car";
        this.drugCarBlip.Color = BlipColor.Green;
      }
      this.drugCar.CurrentBlip.Remove();
      this.drugCar.Delete();
      this.drugCar = (Vehicle) null;
    }

    public int TotalDrugs() => this.drugCarWeed + this.drugCarCrack + this.drugCarCocain;

    public void SpawnCar()
    {
      if (!((Entity) this.drugCar == (Entity) null))
        return;
      this.VehicleLoad();
    }

    public void RemoveOldCar()
    {
      this.drugCarDestroyed = true;
      if (this.drugCarBlip != (Blip) null)
        this.drugCarBlip.Remove();
      this.drugCar.IsPersistent = false;
      this.drugCar.MarkAsNoLongerNeeded();
      if (this.drugCar.CurrentBlip != (Blip) null)
        this.drugCar.CurrentBlip.Remove();
      XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Vehicles.xml");
      foreach (XElement descendant in xdocument.Descendants())
      {
        if (descendant.Name == (XName) "Destroyed")
          descendant.Value = this.drugCarDestroyed.ToString();
      }
      xdocument.Save("scripts\\LSLife\\LSLife_Vehicles.xml");
      this.drugCar = (Vehicle) null;
    }

    public void ImpoundCar()
    {
      if (!this.drugCarWanted || !((Entity) this.drugCar != (Entity) null))
        return;
      LsFunctions.SendTextMsg("Storage Vehicle has been confiscated, and its contents destroyed");
      this.drugCarDestroyed = true;
      XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Vehicles.xml");
      foreach (XElement descendant in xdocument.Descendants())
      {
        if (descendant.Name == (XName) "Destroyed")
        {
          descendant.Value = this.drugCarDestroyed.ToString();
          break;
        }
      }
      xdocument.Save("scripts\\LSLife\\LSLife_Vehicles.xml");
      this.drugCarWanted = false;
      this.drugCar.IsPersistent = false;
      this.drugCar.MarkAsNoLongerNeeded();
      this.drugCar.CurrentBlip.Remove();
      this.drugCar.Delete();
      this.drugCar = (Vehicle) null;
      LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "drugCar")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "HASH")
            {
              element.Value = "";
              break;
            }
          }
        }
        if (descendant.Name == (XName) "drugCarStash")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "weed")
              element.Value = "0";
            if (element.Name == (XName) "crack")
              element.Value = "0";
            if (element.Name == (XName) "cocain")
              element.Value = "0";
            if (element.Name == (XName) "money")
              element.Value = "0";
          }
        }
      }
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
    }

    private void VehicleLoad()
    {
      string name = "";
      float result1 = 0.0f;
      VehicleColor result2 = VehicleColor.Blue;
      VehicleColor result3 = VehicleColor.Blue;
      int result4 = 1000;
      int result5 = 1000;
      VehicleWheelType result6 = VehicleWheelType.Tuner;
      VehicleColor result7 = VehicleColor.BrushedSteel;
      bool result8 = false;
      NumberPlateType result9 = NumberPlateType.BlueOnWhite1;
      string str = "";
      VehicleWindowTint result10 = VehicleWindowTint.None;
      float result11 = 0.0f;
      float result12 = 0.0f;
      int result13 = -1;
      bool result14 = false;
      XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Vehicles.xml");
      foreach (XElement descendant in xdocument.Descendants())
      {
        if (descendant.Name == (XName) "Vehicle")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "Name")
              name = element.Value;
            if (element.Name == (XName) "Hash")
              int.TryParse(element.Value, out this.drugCarHash);
            if (element.Name == (XName) "PosX")
            {
              float result15;
              float.TryParse(element.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result15);
              if ((double) result15 == 0.0)
                float.TryParse(element.Value, out result15);
              if ((double) result15 != 0.0)
                this.CurrentPosition.X = result15;
            }
            if (element.Name == (XName) "PosY")
            {
              float result15;
              float.TryParse(element.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result15);
              if ((double) result15 == 0.0)
                float.TryParse(element.Value, out result15);
              if ((double) result15 != 0.0)
                this.CurrentPosition.Y = result15;
            }
            if (element.Name == (XName) "PosZ")
            {
              float result15;
              float.TryParse(element.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result15);
              if ((double) result15 == 0.0)
                float.TryParse(element.Value, out result15);
              if ((double) result15 != 0.0)
                this.CurrentPosition.Z = result15;
            }
            if (element.Name == (XName) "Heading")
            {
              float.TryParse(element.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result1);
              if ((double) result1 == 0.0)
                float.TryParse(element.Value, out result1);
            }
            if (element.Name == (XName) "PrimaryCol")
              Enum.TryParse<VehicleColor>(element.Value, out result2);
            if (element.Name == (XName) "SecondaryCol")
              Enum.TryParse<VehicleColor>(element.Value, out result3);
            if (element.Name == (XName) "Health")
            {
              int.TryParse(element.Value, out result4);
              if (result4 < 100)
                result4 = 1000;
            }
            if (element.Name == (XName) "EngineHealth")
            {
              int.TryParse(element.Value, out result5);
              if (result5 < 100)
                result5 = 1000;
            }
            if (element.Name == (XName) "WheelType")
              Enum.TryParse<VehicleWheelType>(element.Value, out result6);
            if (element.Name == (XName) "RimCol")
              Enum.TryParse<VehicleColor>(element.Value, out result7);
            if (element.Name == (XName) "NumPlateType")
              Enum.TryParse<NumberPlateType>(element.Value, out result9);
            if (element.Name == (XName) "NumPlate")
            {
              str = element.Value;
              this.reg = str;
            }
            if (element.Name == (XName) "WindowTint")
              Enum.TryParse<VehicleWindowTint>(element.Value, out result10);
            if (element.Name == (XName) "DirtLevel")
              float.TryParse(element.Value, out result11);
            if (element.Name == (XName) "CanTiresBurst")
              bool.TryParse(element.Value, out result8);
            if (element.Name == (XName) "Fuel")
              float.TryParse(element.Value, out result12);
            if (element.Name == (XName) "Livery")
              int.TryParse(element.Value, out result13);
            if (element.Name == (XName) "CustomTire")
              bool.TryParse(element.Value, out result14);
          }
        }
      }
      Model model = new Model(name);
      List<Vehicle> all = ((IEnumerable<Vehicle>) World.GetAllVehicles()).ToList<Vehicle>().FindAll((Predicate<Vehicle>) (v => v.NumberPlate == this.reg && v.Model.Hash == this.drugCarHash));
      if (LSL.DEBUG)
      {
        UI.Notify(all.Count.ToString() ?? "");
        UI.Notify("~r~expected cord:" + this.CurrentPosition.ToString());
      }
      if (all.Count == 0)
      {
        if (model.IsInCdImage)
        {
          this.drugCar = World.CreateVehicle(new Model(name), this.CurrentPosition, result1);
        }
        else
        {
          model = new Model(this.drugCarHash);
          if (model.IsInCdImage)
            this.drugCar = World.CreateVehicle(model, this.CurrentPosition, result1);
        }
      }
      else
        this.drugCar = all[0];
      Script.Wait(5);
      if (LSL.DEBUG)
        UI.Notify(name + " | " + this.CurrentPosition.ToString());
      if ((Entity) this.drugCar != (Entity) null)
      {
        this.drugCar.InstallModKit();
        this.drugCar.PrimaryColor = result2;
        this.drugCar.SecondaryColor = result3;
        this.drugCar.Health = result4;
        this.drugCar.EngineHealth = (float) result5;
        this.drugCar.WheelType = result6;
        this.drugCar.EngineRunning = false;
        this.drugCar.IsInvincible = false;
        this.drugCar.CanTiresBurst = result8;
        this.drugCar.NumberPlateType = result9;
        this.drugCar.NumberPlate = str;
        this.drugCar.WindowTint = result10;
        this.drugCar.DirtLevel = result11;
        this.drugCar.FuelLevel = result12;
        this.drugCar.Livery = result13;
        this.drugCar.IsPersistent = true;
        this.drugCar.RimColor = result7;
        this.drugCar.Repair();
        if (this.drugCar.CurrentBlip != (Blip) null)
          this.drugCar.CurrentBlip.Remove();
        this.drugCar.AddBlip();
        this.drugCar.CurrentBlip.IsShortRange = true;
        this.drugCar.CurrentBlip.IsFriendly = true;
        this.drugCar.CurrentBlip.Sprite = BlipSprite.PersonalVehicleCar;
        this.drugCar.CurrentBlip.Name = "Drug Car";
        this.drugCar.CurrentBlip.Color = BlipColor.Green;
        foreach (XElement descendant in xdocument.Descendants())
        {
          if (descendant.Name == (XName) "Mods")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "FrontWheels")
              {
                VehicleMod result15;
                Enum.TryParse<VehicleMod>(element.Name.ToString(), out result15);
                int result16;
                int.TryParse(element.Value, out result16);
                this.drugCar.SetMod(result15, result16, result14);
                this.drugCar.RimColor = result7;
              }
              else
              {
                VehicleMod result15;
                Enum.TryParse<VehicleMod>(element.Name.ToString(), out result15);
                int result16;
                int.TryParse(element.Value, out result16);
                this.drugCar.SetMod(result15, result16, false);
              }
            }
          }
          if (descendant.Name == (XName) "ToggleMods")
          {
            foreach (XElement element in descendant.Elements())
            {
              VehicleToggleMod result15;
              Enum.TryParse<VehicleToggleMod>(element.Name.ToString(), out result15);
              bool result16;
              bool.TryParse(element.Value, out result16);
              this.drugCar.ToggleMod(result15, result16);
            }
          }
        }
      }
      else
        UI.Notify("~r~Failed to load vehicle");
    }

    public void VehicleSave(Vehicle veh)
    {
      if (!veh.Model.IsCar && !veh.Model.IsBike && (!veh.Model.IsBoat && !veh.Model.IsHelicopter) && !veh.Model.IsPlane)
        return;
      XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Vehicles.xml");
      XElement root = xdocument.Root;
      root.RemoveAll();
      XElement xelement1 = new XElement((XName) "Vehicle");
      XElement xelement2 = new XElement((XName) "Mods");
      XElement xelement3 = new XElement((XName) "ToggleMods");
      XElement xelement4 = new XElement((XName) "Neons");
      xelement1.Add((object) new XElement((XName) "Destroyed", (object) "false"));
      xelement1.Add((object) new XElement((XName) "Handle", (object) veh.Handle.ToString()));
      xelement1.Add((object) new XElement((XName) "Name", (object) veh.DisplayName));
      xelement1.Add((object) new XElement((XName) "Hash", (object) veh.Model.Hash));
      xelement1.Add((object) new XElement((XName) "PosX", (object) veh.Position.X));
      xelement1.Add((object) new XElement((XName) "PosY", (object) veh.Position.Y));
      xelement1.Add((object) new XElement((XName) "PosZ", (object) veh.Position.Z));
      xelement1.Add((object) new XElement((XName) "Heading", (object) veh.Heading));
      xelement1.Add((object) new XElement((XName) "PrimaryCol", (object) veh.PrimaryColor));
      xelement1.Add((object) new XElement((XName) "SecondaryCol", (object) veh.SecondaryColor));
      xelement1.Add((object) new XElement((XName) "Health", (object) veh.BodyHealth));
      xelement1.Add((object) new XElement((XName) "EngineHealth", (object) veh.EngineHealth));
      xelement1.Add((object) new XElement((XName) "WheelType", (object) veh.WheelType));
      xelement1.Add((object) new XElement((XName) "RimCol", (object) veh.RimColor));
      xelement1.Add((object) new XElement((XName) "SmokeCol", (object) veh.TireSmokeColor));
      xelement1.Add((object) new XElement((XName) "NeonCol", (object) veh.NeonLightsColor));
      xelement1.Add((object) new XElement((XName) "TrimCol", (object) veh.TrimColor));
      xelement1.Add((object) new XElement((XName) "DashCol", (object) veh.DashboardColor));
      xelement1.Add((object) new XElement((XName) "NumPlateType", (object) veh.NumberPlateType));
      xelement1.Add((object) new XElement((XName) "NumPlate", (object) veh.NumberPlate));
      xelement1.Add((object) new XElement((XName) "WindowTint", (object) veh.WindowTint));
      xelement1.Add((object) new XElement((XName) "DirtLevel", (object) veh.DirtLevel));
      xelement1.Add((object) new XElement((XName) "CanTiresBurst", (object) veh.CanTiresBurst));
      xelement1.Add((object) new XElement((XName) "Fuel", (object) veh.FuelLevel));
      xelement1.Add((object) new XElement((XName) "Livery", (object) veh.Livery));
      if (Function.Call<bool>(Hash._0xB3924ECD70E095DC, (InputArgument) veh.Handle, (InputArgument) 23))
        xelement1.Add((object) new XElement((XName) "CustomTire", (object) true));
      else
        xelement1.Add((object) new XElement((XName) "CustomTire", (object) false));
      foreach (VehicleMod modType in Enum.GetValues(typeof (VehicleMod)))
        xelement2.Add((object) new XElement((XName) modType.ToString(), (object) veh.GetMod(modType)));
      foreach (VehicleToggleMod toggleMod in Enum.GetValues(typeof (VehicleToggleMod)))
        xelement3.Add((object) new XElement((XName) toggleMod.ToString(), (object) veh.IsToggleModOn(toggleMod)));
      foreach (VehicleNeonLight light in Enum.GetValues(typeof (VehicleNeonLight)))
        xelement4.Add((object) new XElement((XName) light.ToString(), (object) veh.IsNeonLightsOn(light)));
      xelement1.Add((object) xelement2);
      xelement1.Add((object) xelement3);
      xelement1.Add((object) xelement4);
      root.Add((object) xelement1);
      xdocument.Save("scripts\\LSLife\\LSLife_Vehicles.xml");
      this.drugCar = veh;
      this.drugCar.IsPersistent = true;
      if (this.drugCar.CurrentBlip != (Blip) null)
        this.drugCar.CurrentBlip.Remove();
      this.drugCar.AddBlip();
      this.drugCar.CurrentBlip.IsShortRange = true;
      this.drugCar.CurrentBlip.IsFriendly = true;
      this.drugCar.CurrentBlip.Sprite = BlipSprite.PersonalVehicleCar;
      this.drugCar.CurrentBlip.Name = "Drug Car";
      this.drugCar.CurrentBlip.Color = BlipColor.Green;
      this.drugCarDestroyed = false;
      this.CurrentPosition = this.drugCar.Position;
    }

    public void UpdateCarStash()
    {
      LSL.vehStashPutMenu.FormatDescriptions = true;
      LSL.vehStashTakeMenu.FormatDescriptions = true;
      LSL.takeWeedVeh.Maximum = this.drugCarWeed;
      LSL.takeCrackVeh.Maximum = this.drugCarCrack;
      LSL.takeCocainVeh.Maximum = this.drugCarCocain;
      string str1 = "Grams of Weed. Ready to be sold on the streets or given to a worker.";
      LSL.putWeedVeh.Description = str1;
      LSL.takeWeedVeh.Description = str1;
      string str2 = "Grams of Crack. Ready to be sold on the streets or given to a worker.";
      LSL.putCrackVeh.Description = str2;
      LSL.takeCrackVeh.Description = str2;
      string str3 = "Grams of Cocaine. Ready to be sold on the streets or given to a worker.";
      LSL.putCocainVeh.Description = str3;
      LSL.takeCocainVeh.Description = str3;
      LSL.putWeedVeh.Text = "Weed " + LsFunctions.GramsToOz(LSL.PlayerInventory["Weed"]);
      LSL.takeWeedVeh.Text = "Weed " + LsFunctions.GramsToOz(this.drugCarWeed);
      LSL.putCrackVeh.Text = "Crack " + LsFunctions.GramsToOz(LSL.PlayerInventory["Crack"]);
      LSL.takeCrackVeh.Text = "Crack " + LsFunctions.GramsToOz(this.drugCarCrack);
      LSL.putCocainVeh.Text = "Cocaine " + LsFunctions.GramsToOz(LSL.PlayerInventory["Cocaine"]);
      LSL.takeCocainVeh.Text = "Cocaine " + LsFunctions.GramsToOz(this.drugCarCocain);
      LSL.takeWeedOzVeh.Maximum = this.drugCarWeedOz;
      LSL.takeCrackOzVeh.Maximum = this.drugCarCrackOz;
      LSL.takeCocainOzVeh.Maximum = this.drugCarCocainOz;
      LSL.putWeedOzVeh.Maximum = LsFunctions.pWeedOunce;
      LSL.putCrackOzVeh.Maximum = LsFunctions.pCrackOunce;
      LSL.putCocainOzVeh.Maximum = LsFunctions.pCocaineOunce;
      string str4 = "Oz packs of Weed. Must be deposited at a Stash house before it can be sold.";
      LSL.putWeedOzVeh.Description = str4;
      LSL.takeWeedOzVeh.Description = str4;
      string str5 = "Oz packs of Crack. Must be deposited at a Stash house before it can be sold.";
      LSL.putCrackOzVeh.Description = str5;
      LSL.takeCrackOzVeh.Description = str5;
      string str6 = "Oz packs of Cocaine. Must be deposited at a Stash house before it can be sold.";
      LSL.putCocainOzVeh.Description = str6;
      LSL.takeCocainOzVeh.Description = str6;
      LSL.putWeedOzVeh.Text = "Weed ~r~" + LsFunctions.pWeedOunce.ToString() + "~s~z";
      LSL.takeWeedOzVeh.Text = "Weed ~r~" + this.drugCarWeedOz.ToString() + "~s~z";
      LSL.putCrackOzVeh.Text = "Crack ~r~" + LsFunctions.pCrackOunce.ToString() + "~s~z";
      LSL.takeCrackOzVeh.Text = "Crack ~r~" + this.drugCarCrackOz.ToString() + "~s~z";
      LSL.putCocainOzVeh.Text = "Cocaine ~r~" + LsFunctions.pCocaineOunce.ToString() + "~s~z";
      LSL.takeCocainOzVeh.Text = "Cocaine ~r~" + this.drugCarCocainOz.ToString() + "~s~z";
      LSL.putMoneyVeh.Text = "Money $" + LsFunctions.IntToMoney(LSL.player.Money);
      LSL.takeMoneyVeh.Text = "Money $" + LsFunctions.IntToMoney(this.drugCarMoney);
      LSL.takeMoneyStash.Maximum = this.drugCarMoney;
      if (LSL.DEBUG)
        LsFunctions.DbugString("dmon:" + this.drugCarMoney.ToString() + " max:" + LSL.takeMoneyVeh.Maximum.ToString());
      LSL.takeMoneyVeh.Multiplier = 1;
      LSL.playerWeapons.Clear();
      LSL.playerWeps.Clear();
      int num1 = 0;
      foreach (WeaponHash weaponHash in Enum.GetValues(typeof (WeaponHash)))
      {
        bool flag = Function.Call<bool>(Hash._0x8DECB02F88F428BC, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash);
        int num2 = Function.Call<int>(Hash._0x015A522136D7F951, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash);
        if (flag)
        {
          XElement xelement1 = new XElement((XName) "weapon");
          xelement1.SetAttributeValue((XName) "id", (object) num1);
          XElement xelement2 = new XElement((XName) "HASH", (object) weaponHash);
          XElement xelement3 = new XElement((XName) "AMMO", (object) num2);
          xelement1.Add((object) xelement2);
          xelement1.Add((object) xelement3);
          foreach (WeaponComponent weaponComponent in Enum.GetValues(typeof (WeaponComponent)))
          {
            if (Function.Call<bool>(Hash._0xC593212475FAE340, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash, (InputArgument) (int) weaponComponent))
            {
              XElement xelement4 = new XElement((XName) "COMPONENT", (object) weaponComponent);
              xelement1.Add((object) xelement4);
            }
          }
          LSL.playerWeapons.Add(xelement1);
          LSL.playerWeps.Add((object) weaponHash.ToString());
          ++num1;
        }
      }
      this.vehWeapons.Clear();
      this.vehWeps.Clear();
      LSL.vehWeaponsXML = XDocument.Load("scripts\\LSLife\\LsLife_Vehicle_Weapons.xml");
      foreach (XElement descendant in LSL.vehWeaponsXML.Descendants())
      {
        if (descendant.Name == (XName) "weapon")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "HASH")
            {
              this.vehWeapons.Add(descendant);
              this.vehWeps.Add((object) element.Value);
            }
          }
        }
      }
    }
  }
}
