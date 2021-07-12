using GTA;
using LSlife.OldCustomers;

namespace LSlife
{
  public class HirePed
  {
    private Ped PotentialWorker;
    private int TimeOfOffer = Game.GameTime;
    private bool Declined;
    private bool Accepted;
    private bool TimedOut;

    public bool Destroy { get; private set; }

    public HirePed(Ped _ped)
    {
      this.PotentialWorker = _ped;
      OldCustomerHandler.RemoveIfPedOldCustomer(_ped);
      if (LSL.entitiesCleanUp.Contains((Entity) _ped))
        LSL.entitiesCleanUp.Remove((Entity) _ped);
      this.PotentialWorker.Task.ClearAll();
      this.PotentialWorker.BlockPermanentEvents = true;
      this.PotentialWorker.IsPersistent = true;
      this.PotentialWorker.Task.Wait(10000);
    }

    public void OnTick()
    {
      if (!this.Accepted)
      {
        if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 2000.0)
        {
          LSL.displayHelpTimer = (float) Game.GameTime;
          LsFunctions.DisplayHelpText("Hire worker ~INPUT_CONTEXT_SECONDARY~ to reject or ~INPUT_CONTEXT~to accept");
        }
        if (Game.GameTime > this.TimeOfOffer + 10000)
          this.TimedOut = true;
        else if (Game.IsControlJustPressed(2, Control.ContextSecondary))
          this.Declined = true;
        else if (Game.IsControlJustPressed(2, Control.Context))
        {
          this.Accepted = true;
          UI.ShowSubtitle("Cool. Ill follow you.", 6000);
        }
        if (!this.Declined && !this.TimedOut)
          return;
        this.Destroy = true;
        LsFunctions.RemovePedFromWorld(this.PotentialWorker, true);
        UI.ShowSubtitle("Ok, maybe next time.", 6000);
      }
      else
      {
        LSL.DealerHandler.AddDealer(new PlayerDealer(this.PotentialWorker));
        this.Destroy = true;
      }
    }
  }
}
