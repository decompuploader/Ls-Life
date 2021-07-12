namespace LSlife.Inventory
{
  internal class Cocaine : ItemObject
  {
    public Cocaine(string _name, bool _stack, string _desc)
      : base(_name, _stack, _desc)
    {
    }

    private void Innit(string _name, bool _stack, string _desc)
    {
      this.name = _name;
      this.stackable = _stack;
      this.description = _desc;
    }
  }
}
