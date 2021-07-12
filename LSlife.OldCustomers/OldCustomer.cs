using GTA;
using System.Collections.Generic;

namespace LSlife.OldCustomers
{
  internal class OldCustomer
  {
    internal Ped Ped;
    internal bool Inside;
    internal bool CanBeRival;
    internal KeyValuePair<string, int> Drug;
    internal bool IsRival;
    private OldCustomer.OldCustomerStates currentState;
    private bool enteredState;

    public OldCustomer(Ped _ped, bool _inside, bool _canBeRival, KeyValuePair<string, int> _drug)
    {
      this.Ped = _ped;
      this.Inside = _inside;
      this.CanBeRival = _canBeRival;
      this.Drug = _drug;
      this.Ped.CanBeTargetted = true;
      this.Ped.CanBeShotInVehicle = true;
    }

    internal void Destroy()
    {
      LsFunctions.RemovePedFromWorld(this.Ped, true);
      if (!this.Inside || !((Entity) this.Ped != (Entity) null))
        return;
      this.Ped.Delete();
    }

    internal void Remove() => OldCustomerHandler.RemoveCustomer(this);

    public void OnTick()
    {
      switch (this.currentState)
      {
        case OldCustomer.OldCustomerStates.None:
          this.SetState(OldCustomer.OldCustomerStates.WalkAway);
          break;
        case OldCustomer.OldCustomerStates.WalkAway:
          if (this.EnteredState())
          {
            this.WalkAway();
            break;
          }
          if (!this.WalkedAway())
            break;
          this.SetState(OldCustomer.OldCustomerStates.Destroy);
          break;
        case OldCustomer.OldCustomerStates.Destroy:
          if (this.EnteredState())
            this.Destroy();
          if (!((Entity) this.Ped == (Entity) LSL.player.Character))
            break;
          LsFunctions.SendTextMsg("ped is player");
          break;
      }
    }

    private bool EnteredState()
    {
      if (!this.enteredState)
        return false;
      this.enteredState = false;
      return true;
    }

    private bool WalkedAway()
    {
      if (this.Inside)
      {
        if ((double) this.Ped.Position.DistanceTo(LSL.HouseHandler.CurrentHouse.exitPos) < 1.0)
          return true;
      }
      else if ((double) this.Ped.Position.DistanceTo(LSL.playerPos) > 6.0)
        return true;
      return false;
    }

    private void WalkAway()
    {
      if (this.Inside)
        this.Ped.Task.GoTo(LSL.HouseHandler.CurrentHouse.exitPos);
      else
        this.Ped.Task.WanderAround();
    }

    private void SetState(OldCustomer.OldCustomerStates _state)
    {
      this.currentState = _state;
      this.enteredState = true;
    }

    private enum OldCustomerStates
    {
      None,
      WalkAway,
      Attack,
      Destroy,
    }
  }
}
