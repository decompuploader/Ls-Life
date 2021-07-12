namespace LSlife.Inventory
{
  internal class ItemSlot
  {
    private int SlotNumber;
    private bool empty = true;
    private ItemObject item;
    private int itemAmount;

    public ItemSlot(ItemObject _item, int _amount)
    {
      this.item = _item;
      this.itemAmount = _amount;
      this.empty = false;
    }

    public bool Empty
    {
      get => this.empty;
      set => this.empty = value;
    }

    public ItemObject Item
    {
      get => this.item;
      set => this.item = value;
    }

    public int ItemAmount
    {
      get => this.itemAmount;
      set => this.itemAmount = value;
    }
  }
}
