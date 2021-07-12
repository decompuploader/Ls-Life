using System.Collections.Generic;
using System.Linq;

namespace LSlife.Inventory
{
  internal class InventoryObject
  {
    private List<ItemSlot> Inventory = new List<ItemSlot>();

    public bool AddItem(ItemObject _item, int _amount)
    {
      bool flag = false;
      if (_item.stackable && this.Inventory.Count > 0)
      {
        foreach (ItemSlot itemSlot in this.Inventory)
        {
          if (itemSlot.Item.GetType() == _item.GetType())
          {
            itemSlot.ItemAmount += _amount;
            flag = true;
          }
        }
      }
      else
        flag = true;
      return flag;
    }

    public bool RemoveItem(ItemObject _item, int _amount)
    {
      bool flag = false;
      if (this.Inventory.Count > 0)
      {
        foreach (ItemSlot itemSlot in this.Inventory.ToList<ItemSlot>())
        {
          if (itemSlot.Item.name == _item.name)
          {
            if (itemSlot.ItemAmount > _amount)
            {
              itemSlot.ItemAmount -= _amount;
              flag = true;
              break;
            }
            if (itemSlot.ItemAmount == _amount)
            {
              this.Inventory.Remove(itemSlot);
              flag = true;
              break;
            }
          }
        }
      }
      return flag;
    }
  }
}
