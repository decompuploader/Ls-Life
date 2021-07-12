using GTA;

namespace LSlife
{
  public class Drug
  {
    public string Name;
    public int PricePerG;
    public LSL.AreaType AreaType;
    public float Demand;
    public bool Supplied;
    public float Delay = 15000f;
    public int TimeOfLastCustomer = Game.GameTime;
    public bool SelectCustomer;
    public Ped Customer;

    public Drug(string _name, int _price, LSL.AreaType _area)
    {
      this.Name = _name;
      this.PricePerG = _price;
      this.AreaType = _area;
    }
  }
}
