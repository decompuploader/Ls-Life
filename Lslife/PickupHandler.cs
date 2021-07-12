using System.Collections.Generic;
using System.Linq;

namespace LSlife
{
  public class PickupHandler
  {
    private List<DroppedBag> Bags = new List<DroppedBag>();

    public void OnSlowTick()
    {
      if (this.Bags == null || this.Bags.Count <= 0)
        return;
      foreach (DroppedBag _bag in this.Bags.ToList<DroppedBag>())
      {
        if (_bag != null)
        {
          if (_bag.CheckIfPickedUp())
          {
            _bag.AddStuffToPlayer();
            _bag.RemovePickup();
            this.RemoveBag(_bag);
            break;
          }
          if (_bag.CanDespawn())
          {
            _bag.RemovePickup();
            this.RemoveBag(_bag);
            if (LSL.DEBUG)
              LsFunctions.SendTextMsg("A bag was removed for being too old.");
          }
        }
        else
        {
          LsFunctions.DbugString("Bag was null");
          this.RemoveBag(_bag);
        }
      }
    }

    public void AddBag(DroppedBag _bag) => this.Bags.Add(_bag);

    private void RemoveBag(DroppedBag _bag) => this.Bags.Remove(_bag);

    public void RemoveBags()
    {
      if (this.Bags == null || this.Bags.Count <= 0)
        return;
      foreach (DroppedBag _bag in this.Bags.ToList<DroppedBag>())
      {
        if (_bag != null)
        {
          _bag.RemovePickup();
          this.RemoveBag(_bag);
        }
      }
    }
  }
}
