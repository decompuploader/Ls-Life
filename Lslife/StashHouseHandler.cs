using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LSlife
{
  public class StashHouseHandler
  {
    public StashHouse CurrentHouse;

    public List<StashHouse> Houses { get; private set; } = new List<StashHouse>();

    public StashHouseHandler()
    {
      this.LoadHouses();
      if (!LSL.DEBUG)
        return;
      UI.Notify("Loaded " + this.Houses.Count.ToString() + " houses");
    }

    public void Ontick()
    {
      foreach (StashHouse stashHouse in this.Houses.Where<StashHouse>((Func<StashHouse, bool>) (r => (double) r.entrancePos.DistanceTo2D(LSL.playerPos) < 30.0 || (double) r.exitPos.DistanceTo(LSL.playerPos) < 30.0)))
        stashHouse.OnTick();
    }

    private void LoadHouses()
    {
      if (!File.Exists("scripts\\LSLife\\LSLife_StashHouses.xml"))
      {
        UI.Notify("~r~Missing StashHouses.xml");
      }
      else
      {
        XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_StashHouses.xml");
        foreach (XElement descendant1 in xdocument.Descendants())
        {
          if (descendant1.Name == (XName) "house")
          {
            int result1 = 0;
            int.TryParse(descendant1.FirstAttribute.Value, out result1);
            Vector3 zero1 = Vector3.Zero;
            Vector3 zero2 = Vector3.Zero;
            Vector3 zero3 = Vector3.Zero;
            Vector3 zero4 = Vector3.Zero;
            int result2 = 0;
            bool result3 = false;
            int result4 = 0;
            int result5 = 0;
            int result6 = 0;
            int result7 = 0;
            int result8 = 0;
            bool result9 = false;
            bool flag = false;
            foreach (XElement descendant2 in LSL.LSLifeSave.Descendants())
            {
              if (descendant2.Name == (XName) "playerStash")
              {
                flag = true;
                descendant2.Remove();
                LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
                break;
              }
            }
            List<XElement> _weapons = new List<XElement>();
            List<object> _weps = new List<object>();
            foreach (XElement element1 in descendant1.Elements())
            {
              if (element1.Name == (XName) "entrancePos")
              {
                foreach (XElement element2 in element1.Elements())
                {
                  if (element2.Name == (XName) "X")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero1.X);
                  if (element2.Name == (XName) "Y")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero1.Y);
                  if (element2.Name == (XName) "Z")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero1.Z);
                }
              }
              if (element1.Name == (XName) "exitPos")
              {
                foreach (XElement element2 in element1.Elements())
                {
                  if (element2.Name == (XName) "X")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero2.X);
                  if (element2.Name == (XName) "Y")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero2.Y);
                  if (element2.Name == (XName) "Z")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero2.Z);
                }
              }
              if (element1.Name == (XName) "stashPos")
              {
                foreach (XElement element2 in element1.Elements())
                {
                  if (element2.Name == (XName) "X")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero3.X);
                  if (element2.Name == (XName) "Y")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero3.Y);
                  if (element2.Name == (XName) "Z")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero3.Z);
                }
              }
              if (element1.Name == (XName) "healthPos")
              {
                foreach (XElement element2 in element1.Elements())
                {
                  if (element2.Name == (XName) "X")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero4.X);
                  if (element2.Name == (XName) "Y")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero4.Y);
                  if (element2.Name == (XName) "Z")
                    float.TryParse(element2.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out zero4.Z);
                }
              }
              if (element1.Name == (XName) "price")
                int.TryParse(element1.Value, out result2);
              if (element1.Name == (XName) "purchased")
                bool.TryParse(element1.Value, out result3);
              if (element1.Name == (XName) "inside")
                bool.TryParse(element1.Value, out result9);
              if (element1.Name == (XName) "stash")
              {
                foreach (XElement element2 in element1.Elements())
                {
                  if (element2.Name == (XName) "weed")
                  {
                    if (flag)
                    {
                      element2.Value = result4.ToString();
                    }
                    else
                    {
                      int.TryParse(element2.Value, out result4);
                      if (LSL.DEBUG)
                        UI.Notify("~y~Weed" + result4.ToString());
                    }
                  }
                  if (element2.Name == (XName) "crack")
                  {
                    if (flag)
                      element2.Value = result5.ToString();
                    else
                      int.TryParse(element2.Value, out result5);
                  }
                  if (element2.Name == (XName) "cocain")
                  {
                    if (flag)
                      element2.Value = result6.ToString();
                    else
                      int.TryParse(element2.Value, out result6);
                  }
                  if (element2.Name == (XName) "money")
                  {
                    if (flag)
                      element2.Value = result8.ToString();
                    else
                      int.TryParse(element2.Value, out result8);
                  }
                  if (element2.Name == (XName) "armour")
                  {
                    if (flag)
                      element2.Value = result7.ToString();
                    else
                      int.TryParse(element2.Value, out result7);
                  }
                  if (element2.Name == (XName) "weapons")
                  {
                    foreach (XElement element3 in element2.Elements())
                    {
                      if (element3.Name == (XName) "weapon")
                      {
                        foreach (XElement element4 in element3.Elements())
                        {
                          if (element4.Name == (XName) "HASH")
                          {
                            _weapons.Add(element3);
                            _weps.Add((object) element4.Value);
                            break;
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            if (flag)
              xdocument.Save("scripts\\LSLife\\LSLife_StashHouses.xml");
            this.Houses.Add(new StashHouse(result1, zero1, zero2, zero3, zero4, result3, result2, result9, result4, result5, result6, result8, result7, _weapons, _weps));
          }
        }
      }
    }

    private void CreateStashHouseXML()
    {
    }

    public void SaveHouse(StashHouse _house)
    {
      XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_StashHouses.xml");
      foreach (XElement descendant in xdocument.Descendants())
      {
        if (descendant.Name == (XName) "house" && descendant.FirstAttribute.Value == _house.id.ToString())
        {
          foreach (XElement element1 in descendant.Elements())
          {
            if (element1.Name == (XName) "stash")
            {
              foreach (XElement element2 in element1.Elements())
              {
                if (element2.Name == (XName) "weed")
                  element2.Value = _house.Weed.ToString();
                if (element2.Name == (XName) "crack")
                  element2.Value = _house.Crack.ToString();
                if (element2.Name == (XName) "cocain")
                  element2.Value = _house.Cocain.ToString();
                if (element2.Name == (XName) "money")
                  element2.Value = _house.Money.ToString();
                if (element2.Name == (XName) "armour")
                  element2.Value = _house.Armor.ToString();
                XElement xelement = new XElement((XName) "weapons");
                foreach (XElement weapon in _house.Weapons)
                  xelement.Add((object) weapon);
                if (element2.Name == (XName) "weapons")
                {
                  element2.Remove();
                  element1.Add((object) xelement);
                  break;
                }
              }
            }
          }
          xdocument.Save("scripts\\LSLife\\LSLife_StashHouses.xml");
        }
      }
    }

    public int TotalStashDrugs()
    {
      int num = 0;
      foreach (StashHouse house in this.Houses)
        num += house.Weed + house.Crack + house.Cocain;
      return num;
    }

    public void UpdateMenu(StashHouse _house)
    {
      LSL.takeWeedStash.Maximum = _house.Weed;
      LSL.takeCrackStash.Maximum = _house.Crack;
      LSL.takeCocainStash.Maximum = _house.Cocain;
      LSL.takeMoneyStash.Maximum = _house.Money;
      LSL.takeWeedStash.Text = "Weed " + LsFunctions.GramsToOz(_house.Weed);
      LSL.takeCrackStash.Text = "Crack " + LsFunctions.GramsToOz(_house.Crack);
      LSL.takeCocainStash.Text = "Cocaine " + LsFunctions.GramsToOz(_house.Cocain);
      LSL.takeMoneyStash.Text = "Money $" + LsFunctions.IntToMoney(_house.Money);
      LSL.takeWeaponStash.Items = _house.Weps;
      LSL.takeWeaponStash.Index = 0;
      LSL.takeArmourStash.Items = new List<object>();
      for (int index = 0; index < _house.Armor + 1; ++index)
        LSL.takeArmourStash.Items.Add((object) index);
      LSL.putWeedStash.Maximum = LSL.PlayerInventory["Weed"];
      LSL.putCrackStash.Maximum = LSL.PlayerInventory["Crack"];
      LSL.putCocainStash.Maximum = LSL.PlayerInventory["Cocaine"];
      LSL.putWeedOzStash.Maximum = LsFunctions.pWeedOunce;
      LSL.putCrackOzStash.Maximum = LsFunctions.pCrackOunce;
      LSL.putCocainOzStash.Maximum = LsFunctions.pCocaineOunce;
      LSL.putMoneyStash.Maximum = LSL.player.Money;
      LSL.putWeedStash.Text = "Weed " + LsFunctions.GramsToOz(LSL.PlayerInventory["Weed"]);
      LSL.putCrackStash.Text = "Crack " + LsFunctions.GramsToOz(LSL.PlayerInventory["Crack"]);
      LSL.putCocainStash.Text = "Cocaine " + LsFunctions.GramsToOz(LSL.PlayerInventory["Cocaine"]);
      LSL.putWeedOzStash.Text = "Weed ~r~" + LsFunctions.pWeedOunce.ToString() + "~s~z";
      LSL.putCrackOzStash.Text = "Crack ~r~" + LsFunctions.pCrackOunce.ToString() + "~s~z";
      LSL.putCocainOzStash.Text = "Cocaine ~r~" + LsFunctions.pCocaineOunce.ToString() + "~s~z";
      LSL.putMoneyStash.Text = "Money $" + LsFunctions.IntToMoney(LSL.player.Money);
      LSL.putWeaponStash.Index = 0;
      LSL.putWeaponStash.Items = LSL.playerWeps;
      LSL.putArmourStash.Items = new List<object>();
      for (int index = 0; index < LSL.playerArmours + 1; ++index)
        LSL.putArmourStash.Items.Add((object) index);
    }

    internal bool Inside() => this.CurrentHouse != null && this.CurrentHouse.inside;
  }
}
