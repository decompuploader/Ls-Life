using GTA;
using System.Xml.Linq;

namespace LSlife
{
  public class DrugOrder
  {
    public bool Placed;
    private bool payed;
    public bool pickedUp;

    public int weed { get; private set; }

    public int crack { get; private set; }

    public int cocaine { get; private set; }

    public int Bill { get; private set; }

    public void OrderWeed(int amount, int price)
    {
      this.weed += amount;
      this.Bill += price;
    }

    public void OrderCrack(int amount, int price)
    {
      this.crack += amount;
      this.Bill += price;
    }

    public void OrderCocaine(int amount, int price)
    {
      this.cocaine += amount;
      this.Bill += price;
    }

    public bool PayBill()
    {
      if (!this.CanPayBill())
        return false;
      this.payed = true;
      int money = LSL.player.Money;
      int num = money >= this.Bill ? this.Bill : money;
      LSL.player.Money -= num;
      LSL.dealer1Money += num;
      if (num > 0)
        LsFunctions.Dealer1AddRep(num / 100);
      if (this.Bill - num > 0)
        LSL.dealer1Debt += this.Bill - num;
      LsFunctions.SaveDealer();
      LsFunctions.PlayTheSound();
      this.payed = true;
      return true;
    }

    public bool CanPayBill()
    {
      int num = this.Bill - LSL.player.Money;
      return LSL.dealer1Debt + num <= LsFunctions.zeeDebtCap();
    }

    public void PickupDrugs()
    {
      if (this.PlayerCanPickup())
      {
        if (this.weed > 0)
          LsFunctions.pWeedOunce += this.weed;
        if (this.crack > 0)
          LsFunctions.pCrackOunce += this.crack;
        if (this.cocaine > 0)
          LsFunctions.pCocaineOunce += this.cocaine;
        LsFunctions.PlayTheSound();
        this.pickedUp = true;
      }
      else if ((Entity) LSL.pStashVehicle?.drugCar != (Entity) null)
      {
        StashVehicle pStashVehicle = LSL.pStashVehicle;
        if ((double) pStashVehicle.drugCar.Position.DistanceTo(LSL.ZeesDealers.dealerCar.Position) < 20.0)
        {
          Game.FadeScreenOut(1000);
          Script.Wait(2000);
          LSL.player.Character.SetIntoVehicle(pStashVehicle.drugCar, VehicleSeat.Driver);
          if (this.weed > 0)
            LSL.pStashVehicle.drugCarWeedOz += this.weed;
          if (this.crack > 0)
            LSL.pStashVehicle.drugCarCrackOz += this.crack;
          if (this.cocaine > 0)
            LSL.pStashVehicle.drugCarCocainOz += this.cocaine;
          LsFunctions.PlayTheSound();
          this.pickedUp = true;
          Game.FadeScreenIn(500);
        }
        else
          UI.ShowSubtitle("Your Vehicle needs to be closer.");
      }
      else
        UI.ShowSubtitle("You need a stash vehicle to pickup an order this big.");
      if (!this.pickedUp)
        return;
      this.SaveOrder();
    }

    private void SaveOrder()
    {
      LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "playerInventory")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "weedOz")
              element.Value = LsFunctions.pWeedOunce.ToString();
            if (element.Name == (XName) "crackOz")
              element.Value = LsFunctions.pCrackOunce.ToString();
            if (element.Name == (XName) "cocaineOz")
              element.Value = LsFunctions.pCocaineOunce.ToString();
          }
        }
        if (descendant.Name == (XName) "drugCarStash")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "weedOz")
              element.Value = LSL.pStashVehicle.drugCarWeedOz.ToString();
            if (element.Name == (XName) "crackOz")
              element.Value = LSL.pStashVehicle.drugCarCrackOz.ToString();
            if (element.Name == (XName) "cocainOz")
              element.Value = LSL.pStashVehicle.drugCarCocainOz.ToString();
          }
        }
        if (descendant.Name == (XName) "dealer1")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "weed")
              element.Value = LSL.dealer1Weed.ToString();
            if (element.Name == (XName) "crack")
              element.Value = LSL.dealer1Crack.ToString();
            if (element.Name == (XName) "cocain")
              element.Value = LSL.dealer1Cocain.ToString();
            if (element.Name == (XName) "debt")
              element.Value = LSL.dealer1Debt.ToString();
            if (element.Name == (XName) "money")
              element.Value = LSL.dealer1Money.ToString();
          }
        }
      }
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
    }

    public bool PlayerCanPickup() => LsFunctions.TotalDrugs(LSL.PlayerInventory) + this.OrderSizeInGrams() < 7168;

    private int OrderSizeInGrams() => this.weed * 28 + this.crack * 28 + this.cocaine * 28;

    internal bool OrderValid()
    {
      if (this.weed != 0 || this.crack != 0 || this.cocaine != 0)
        return true;
      LsFunctions.TextMsg("ZEE", "Order", "Your order is empty");
      return false;
    }

    internal void PutBackDrugs()
    {
      LSL.dealer1Weed += this.weed;
      LSL.dealer1Crack += this.crack;
      LSL.dealer1Cocain += this.cocaine;
      LsFunctions.SaveDealer();
    }

    internal void ResetOrder()
    {
      this.weed = 0;
      this.crack = 0;
      this.cocaine = 0;
      this.Bill = 0;
      this.payed = false;
      this.pickedUp = false;
      this.Placed = false;
    }
  }
}
