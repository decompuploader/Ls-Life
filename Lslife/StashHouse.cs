using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace LSlife
{
  public class StashHouse
  {
    public int id;
    private Vector3 stashPos;
    private Blip blip;
    private bool purchased;
    private int price;
    private Color color = Color.Yellow;
    public bool inside;
    public int Weed;
    public int Crack;
    public int Cocain;
    public int WeedOz;
    public int CrackOz;
    public int CocainOz;
    public int Money;
    public int Armor;
    public List<XElement> Weapons;
    public List<object> Weps;
    public Vector3 houseHealthPickPos;
    public bool pickupSpawned;
    public static Pickup healthPickup;

    public Vector3 entrancePos { get; private set; }

    public Vector3 exitPos { get; private set; }

    public StashHouse(
      int _id,
      Vector3 _entrancePos,
      Vector3 _exitPos,
      Vector3 _stashPos,
      Vector3 _health,
      bool _purchased,
      int _price,
      bool _inside,
      int _Weed,
      int _Crack,
      int _Cocain,
      int _Money,
      int _Armor,
      List<XElement> _weapons,
      List<object> _weps)
    {
      this.id = _id;
      this.entrancePos = _entrancePos;
      this.exitPos = _exitPos;
      this.stashPos = _stashPos;
      this.purchased = _purchased;
      this.price = _price;
      this.blip = World.CreateBlip(this.entrancePos);
      this.blip.Sprite = BlipSprite.Safehouse;
      this.blip.Name = "Stash House";
      this.SetBlip();
      this.inside = _inside;
      this.Weapons = _weapons;
      this.Weps = _weps;
      this.Weed = _Weed;
      this.Crack = _Crack;
      this.Cocain = _Cocain;
      this.Money = _Money;
      this.Armor = _Armor;
      this.houseHealthPickPos = _health;
      if (!this.purchased)
        return;
      this.color = Color.Green;
    }

    public void OnTick()
    {
      if (!this.inside)
      {
        if (this.pickupSpawned && StashHouse.healthPickup != (Pickup) null && StashHouse.healthPickup.ObjectExists())
        {
          LsFunctions.DbugString("Health Deleted");
          StashHouse.healthPickup.Delete();
          this.pickupSpawned = false;
        }
        if (LSL.DrawMarkers && (double) LSL.playerPos.DistanceTo(this.entrancePos) < 30.0)
          World.DrawMarker(MarkerType.VerticalCylinder, this.entrancePos, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), this.color);
        if (!LSL.player.Character.IsStopped || (double) LSL.playerPos.DistanceTo(this.entrancePos) >= 2.0)
          return;
        if (this.purchased)
        {
          if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0)
          {
            Function.Call(Hash._0x6178F68A87A4D3A0);
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("Press ~INPUT_CONTEXT~ to enter StashHouse.");
          }
          if (!Game.IsControlJustReleased(2, Control.Context))
            return;
          Function.Call(Hash._0x6178F68A87A4D3A0);
          LSL.player.Character.FreezePosition = true;
          foreach (PlayerDealer dealer in LSL.DealerHandler.dealers)
          {
            if (dealer.NewState == Enums.PdStates.FollowingPlayer)
            {
              dealer.NewState = Enums.PdStates.FollowPlayer;
              dealer.Ped.LeaveGroup();
            }
          }
          Game.FadeScreenOut(300);
          Script.Wait(300);
          LSL.player.Character.Position = this.exitPos;
          Script.Wait(400);
          LSL.player.Character.FreezePosition = false;
          Game.FadeScreenIn(300);
          this.inside = true;
          LSL.HouseHandler.CurrentHouse = this;
          foreach (Entity getPed in LSL.lPeds.GetPeds)
          {
            PedHash hash = (PedHash) getPed.Model.Hash;
            if (!new List<PedHash>()
            {
              PedHash.Cop01SMY,
              PedHash.Cop01SFY,
              PedHash.Swat01SMY,
              PedHash.Ranger01SFY,
              PedHash.Ranger01SMY,
              PedHash.Sheriff01SFY,
              PedHash.Sheriff01SMY
            }.Contains(hash) && !LsFunctions.cHashes.Contains(hash))
              LsFunctions.cHashes.Add(hash);
          }
          this.SetInside(true);
        }
        else
        {
          if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0)
          {
            Function.Call(Hash._0x6178F68A87A4D3A0);
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("Press ~INPUT_CONTEXT~ To Purchase For $" + LsFunctions.IntToMoney(this.price));
          }
          if (!Game.IsControlJustReleased(2, Control.Context))
            return;
          if (LSL.player.Money >= this.price)
          {
            LSL.player.Money -= this.price;
            this.purchased = true;
            this.SetBlip();
            this.SetPurchased(true);
          }
          else
            UI.ShowSubtitle("You do not have enough money to purchase this property");
        }
      }
      else
      {
        if (LSL.HouseHandler.CurrentHouse == null)
          LSL.HouseHandler.CurrentHouse = this;
        if ((double) LSL.playerPos.DistanceTo(this.exitPos) > (double) LSL.playerPos.DistanceTo(this.entrancePos))
          this.SetInside(false);
        if (LSL.DrawMarkers)
          World.DrawMarker(MarkerType.UpsideDownCone, new Vector3(this.stashPos.X, this.stashPos.Y, this.stashPos.Z - 0.5f), Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 0.5f), Color.Green);
        if (this.pickupSpawned)
        {
          if (StashHouse.healthPickup != (Pickup) null && !StashHouse.healthPickup.ObjectExists())
          {
            LsFunctions.DbugString("Health Deleted");
            StashHouse.healthPickup.Delete();
            StashHouse.healthPickup = (Pickup) null;
            this.pickupSpawned = false;
          }
        }
        else
        {
          this.pickupSpawned = true;
          StashHouse.healthPickup = World.CreatePickup(PickupType.Health, this.houseHealthPickPos, Vector3.Zero, new Model("prop_ld_health_pack"), 100);
          LsFunctions.DbugString("Spawned Health");
        }
        if (!LSL.LsMenuPool.IsAnyMenuOpen() && (double) LSL.playerPos.DistanceTo(this.stashPos) <= 1.0)
        {
          if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0)
          {
            Function.Call(Hash._0x6178F68A87A4D3A0);
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("Press ~INPUT_CONTEXT~ to open stash.");
          }
          if (Game.IsControlJustReleased(2, Control.Context))
          {
            Function.Call(Hash._0x6178F68A87A4D3A0);
            LsFunctions.UpdateInventory();
            LSL.HouseHandler.UpdateMenu(this);
            LSL.HouseHandler.CurrentHouse = this;
            LSL.stashMainMenu.Visible = !LSL.stashMainMenu.Visible;
          }
        }
        if ((double) LSL.playerPos.DistanceTo(this.exitPos) < 2.0)
        {
          if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 10000.0)
          {
            Function.Call(Hash._0x6178F68A87A4D3A0);
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("Press ~INPUT_CONTEXT~ to exit StashHouse.");
          }
          if (Game.IsControlJustReleased(2, Control.Context))
          {
            Function.Call(Hash._0x6178F68A87A4D3A0);
            LSL.player.Character.FreezePosition = true;
            Game.FadeScreenOut(300);
            Script.Wait(300);
            LSL.player.Character.Position = this.entrancePos;
            Script.Wait(400);
            LSL.player.Character.FreezePosition = false;
            Game.FadeScreenIn(300);
            this.inside = false;
            this.SetInside(false);
          }
        }
        if (!LSL.stashMainMenu.Visible || (double) LSL.playerPos.DistanceTo(this.stashPos) <= 1.0)
          return;
        LSL.stashMainMenu.Visible = false;
      }
    }

    private void SetInside(bool v)
    {
      XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_StashHouses.xml");
      foreach (XElement descendant in xdocument.Descendants())
      {
        if (descendant.Name == (XName) "house" && descendant.FirstAttribute.Value == this.id.ToString())
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "inside")
            {
              element.Value = v.ToString();
              xdocument.Save("scripts\\LSLife\\LSLife_StashHouses.xml");
              break;
            }
          }
        }
      }
    }

    private void SetPurchased(bool v)
    {
      XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_StashHouses.xml");
      foreach (XElement descendant in xdocument.Descendants())
      {
        if (descendant.Name == (XName) "house" && descendant.FirstAttribute.Value == this.id.ToString())
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "purchased")
            {
              element.Value = v.ToString();
              xdocument.Save("scripts\\LSLife\\LSLife_StashHouses.xml");
              break;
            }
          }
        }
      }
    }

    public void RemoveBlip() => this.blip.Remove();

    public void SetBlip()
    {
      if (this.purchased)
      {
        this.color = Color.Green;
        this.blip.Color = BlipColor.Green;
      }
      else
        this.blip.Color = BlipColor.WhiteNotPure;
    }
  }
}
