using GTA.Math;
using GTA.Native;
using System.Linq;

namespace LSlife
{
  internal static class DropDrugs
  {
    public static void Drop()
    {
      if (LSL.TotalDrugsCarried() <= 0)
        return;
      Vector3 _pos = LSL.playerPos + LSL.player.Character.ForwardVector * 2f;
      Function.Call<Vector3>(Hash._0x6E16BC2503FF1FF0, (InputArgument) _pos.X, (InputArgument) _pos.Y, (InputArgument) _pos.Z, (InputArgument) 0, (InputArgument) 0);
      LSL.pickupHandler.AddBag(new DroppedBag("You have dropped " + LsFunctions.GramsToOz(LSL.TotalDrugsCarried()) + " of drugs!", LSL.PlayerInventory["Weed"], LSL.PlayerInventory["Crack"], LSL.PlayerInventory["Cocaine"], LsFunctions.pWeedOunce, LsFunctions.pCrackOunce, LsFunctions.pCocaineOunce, 0, _pos));
      LsFunctions.PlayTheSound();
      DropDrugs.RemovePlayerDrugs();
      LsFunctions.UpdateInventory();
    }

    private static void RemovePlayerDrugs()
    {
      foreach (string key in LSL.PlayerInventory.Keys.ToList<string>())
        LSL.PlayerInventory[key] = 0;
      LsFunctions.pCocaineOunce = 0;
      LsFunctions.pWeedOunce = 0;
      LsFunctions.pCrackOunce = 0;
    }
  }
}
