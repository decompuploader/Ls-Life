using GTA;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSlife.OldCustomers
{
  internal static class OldCustomerHandler
  {
    private static List<OldCustomer> OldCustomers = new List<OldCustomer>();

    public static void OnTick()
    {
      foreach (OldCustomer oldCustomer in OldCustomerHandler.OldCustomers.ToList<OldCustomer>())
      {
        if (oldCustomer == null || !oldCustomer.Ped.Exists())
          OldCustomerHandler.OldCustomers.Remove(oldCustomer);
        else
          oldCustomer.OnTick();
      }
    }

    internal static void RemoveIfPedOldCustomer(Ped ped)
    {
      if (OldCustomerHandler.OldCustomers.Count <= 0)
        return;
      List<OldCustomer> list = OldCustomerHandler.OldCustomers.Where<OldCustomer>((Func<OldCustomer, bool>) (c => (Entity) c.Ped == (Entity) ped)).ToList<OldCustomer>();
      if (list.Count <= 0)
        return;
      list.First<OldCustomer>().Remove();
    }

    public static void AddCustomer(
      Ped _ped,
      bool _inside,
      bool _canBeRival,
      KeyValuePair<string, int> _drug)
    {
      if (_ped.IsPlayer)
        return;
      if (_ped.CurrentPedGroup == LSL.player.Character.CurrentPedGroup)
        _ped.LeaveGroup();
      if (_ped.CurrentBlip != (Blip) null)
        _ped.CurrentBlip.Remove();
      OldCustomer oldCustomer = new OldCustomer(_ped, _inside, _canBeRival, _drug);
      OldCustomerHandler.OldCustomers.Add(oldCustomer);
    }

    internal static int AmountOf() => OldCustomerHandler.OldCustomers.Count;

    public static void RemoveCustomer(OldCustomer o) => OldCustomerHandler.OldCustomers.Remove(o);

    internal static void ClearOldCustomers()
    {
      foreach (OldCustomer oldCustomer in OldCustomerHandler.OldCustomers.ToList<OldCustomer>())
      {
        oldCustomer.Destroy();
        oldCustomer.Remove();
      }
    }

    public static bool IsPedOldCustomer(Ped _ped) => OldCustomerHandler.OldCustomers.Count > 0 && OldCustomerHandler.OldCustomers.Where<OldCustomer>((Func<OldCustomer, bool>) (c => (Entity) c.Ped == (Entity) _ped)).Count<OldCustomer>() > 0;
  }
}
