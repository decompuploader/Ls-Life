using GTA;
using GTA.Math;

namespace LSlife
{
  internal class CustomerPed : DefaultPed
  {
    private Vehicle Vehicle;
    public Drug Drug;
    public int amount;
    private bool walkToPlayer;
    private CustomerPed.CustomerStates cState;

    public CustomerPed(
      DefaultPed.PedType type,
      Ped ped,
      Vector3 target,
      bool working,
      int areaIndex,
      Vehicle _vehicle,
      Drug _drug,
      int _drugAmount,
      bool _walk)
      : base(type, ped, target)
    {
      this.Vehicle = _vehicle;
      this.Drug = _drug;
      this.amount = _drugAmount;
      this.walkToPlayer = _walk;
    }

    public new void Tick()
    {
      switch (this.cState)
      {
        case CustomerPed.CustomerStates.None:
          if (this.walkToPlayer)
          {
            this.MovePed();
            this.cState = CustomerPed.CustomerStates.WalkTo;
            break;
          }
          this.Wait();
          this.cState = CustomerPed.CustomerStates.Waiting;
          break;
        case CustomerPed.CustomerStates.Waiting:
          if ((double) Game.Player.Character.Position.DistanceTo(this.Ped.Position) >= 10.0)
            break;
          this.MovePed();
          this.cState = CustomerPed.CustomerStates.WalkTo;
          break;
        case CustomerPed.CustomerStates.WalkTo:
          if (Game.Player.Character.Position != this.target)
          {
            this.target = Game.Player.Character.Position;
            this.MovePed();
          }
          double num = (double) Game.Player.Character.Position.DistanceTo(this.Ped.Position);
          break;
      }
    }

    private void BuyDrugs()
    {
    }

    private enum CustomerStates
    {
      None,
      Waiting,
      WalkTo,
      Buying,
      WalktAway,
    }
  }
}
