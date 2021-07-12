namespace LSlife.Inventory
{
  public abstract class ItemObject
  {
    public string name;
    public bool stackable;
    public string description;

    public ItemObject(string _name, bool _stack, string _desc) => this.Innit(_name, _stack, _desc);

    private void Innit(string _name, bool _stack, string _desc)
    {
      this.name = _name;
      this.stackable = _stack;
      this.description = _desc;
    }
  }
}
