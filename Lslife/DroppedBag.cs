using GTA;
using GTA.Math;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LSlife
{
  public class DroppedBag
  {
    private int weed;
    private int crack;
    private int cocaine;
    private int weedZ;
    private int crackZ;
    private int cocaineZ;
    private int money;
    private int timeSpawned;
    private int timeToLive = 120000;
    private List<XElement> weapons;
    private Vector3 pos;
    private Blip blip;
    private Pickup pickup;
    private bool players;

    public DroppedBag(
      string _subtitleString,
      int _weed,
      int _crack,
      int _cocaine,
      int _weedZ,
      int _crackZ,
      int _cocaineZ,
      int _money,
      Vector3 _pos)
    {
      this.weed = _weed;
      this.crack = _crack;
      this.cocaine = _cocaine;
      this.weedZ = _weedZ;
      this.crackZ = _crackZ;
      this.cocaineZ = _cocaineZ;
      this.money = _money;
      this.pos = _pos;
      this.Innit(_subtitleString);
    }

    public DroppedBag(
      string _subtitleString,
      int _weed,
      int _crack,
      int _cocaine,
      int _money,
      List<XElement> _weapons,
      Vector3 _pos)
    {
      this.weed = _weed;
      this.crack = _crack;
      this.cocaine = _cocaine;
      this.money = _money;
      this.weapons = _weapons;
      this.pos = _pos;
      this.Innit(_subtitleString);
    }

    private void Innit(string _subtitle)
    {
      this.CreateBlip();
      this.CreatePickup();
      UI.ShowSubtitle(_subtitle, 5000);
      this.timeSpawned = Game.GameTime;
    }

    public bool CheckIfPickedUp() => !this.pickup.ObjectExists();

    public void AddStuffToPlayer()
    {
      LSL.PlayerInventory["Weed"] += this.weed;
      LSL.PlayerInventory["Crack"] += this.crack;
      LSL.PlayerInventory["Cocaine"] += this.cocaine;
      LsFunctions.pWeedOunce += this.weedZ;
      LsFunctions.pCrackOunce += this.crackZ;
      LsFunctions.pCocaineOunce += this.cocaineZ;
      Game.Player.Money += this.money;
      if (this.weapons != null)
        LsFunctions.GivePedWeapons(Game.Player.Character, this.weapons);
      UI.ShowSubtitle("You picked up " + LsFunctions.GramsToOz(this.weed) + " Weed, " + LsFunctions.GramsToOz(this.crack) + " Crack, " + LsFunctions.GramsToOz(this.cocaine) + " Cocaine and $~g~" + this.money.ToString(), 5000);
      LsFunctions.UpdateInventory();
    }

    private void CreateBlip()
    {
      this.blip = World.CreateBlip(this.pos);
      this.blip.Color = BlipColor.Green;
      this.blip.Name = "Bag";
      this.blip.IsShortRange = true;
    }

    private void RemoveBlip() => this.blip.Remove();

    public bool CanDespawn()
    {
      if (Game.GameTime > this.timeSpawned + this.timeToLive)
        return true;
      if (Game.GameTime > this.timeSpawned + 10000 && (double) LSL.playerPos.DistanceTo(this.pickup.Position) < 20.0)
      {
        LsFunctions.DbugString("bag timer reset");
        this.timeSpawned = Game.GameTime;
      }
      return false;
    }

    public void RemovePickup()
    {
      this.pickup.Delete();
      this.RemoveBlip();
      if (!LSL.DEBUG)
        return;
      LsFunctions.DbugString("Pickup Deleted");
    }

    private void CreatePickup()
    {
      this.pickup = World.CreatePickup(PickupType.CustomScript, this.pos, new Model("prop_big_bag_01"), 0);
      while (this.pickup == (Pickup) null)
        Script.Wait(0);
    }
  }
}
