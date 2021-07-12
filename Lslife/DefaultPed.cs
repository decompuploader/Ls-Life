using GTA;
using GTA.Math;
using LSlife.Inventory;
using System;

namespace LSlife
{
  internal class DefaultPed
  {
    public DefaultPed.PedType type;
    public Ped Ped;
    public Vector3 target;
    public bool working;
    private int areaIndex;
    private InventoryObject pInventory = new InventoryObject();

    public DefaultPed(DefaultPed.PedType type, Ped ped, Vector3 target)
    {
      this.type = type;
      this.Ped = ped ?? throw new ArgumentNullException(nameof (ped));
      this.target = target;
    }

    public void MovePed()
    {
      this.Ped.Task.GoTo(this.target);
      this.pInventory.AddItem((ItemObject) new Cocaine("Coke", true, ""), 420);
    }

    public void Wait() => this.Ped.Task.PlayAnimation("friends@", "pickupwait", 8f, -1, AnimationFlags.Loop);

    public void Tick()
    {
    }

    public enum PedType
    {
      Default,
      NewCustomer,
      OldCustomer,
    }
  }
}
