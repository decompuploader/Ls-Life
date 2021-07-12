using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSlife
{
  internal static class HeadShotHandler
  {
    public static Dictionary<string, HeadShot> headShots = new Dictionary<string, HeadShot>();

    public static void HandleHeadShots()
    {
      if (HeadShotHandler.headShots.Count <= 0)
        return;
      foreach (HeadShot headShot in HeadShotHandler.headShots.Values)
      {
        if (Game.GameTime > headShot.timeCreated + 4000)
        {
          HeadShotHandler.DeleteHeadShot(headShot.Handle);
        }
        else
        {
          if (Function.Call<bool>(Hash._0x7085228842B13A67, (InputArgument) headShot.Handle))
          {
            if (Function.Call<bool>(Hash._0xA0A9668F158129A2, (InputArgument) headShot.Handle))
              continue;
          }
          HeadShotHandler.DeleteHeadShot(headShot.Handle);
        }
      }
    }

    public static void DeleteHeadShot(string _name)
    {
      Function.Call(Hash._0x96B1361D9B24C2FF, (InputArgument) HeadShotHandler.headShots[_name].Handle);
      HeadShotHandler.headShots.Remove(_name);
    }

    private static void DeleteHeadShot(int _handle)
    {
      Function.Call(Hash._0x96B1361D9B24C2FF, (InputArgument) _handle);
      HeadShotHandler.headShots.Remove(HeadShotHandler.headShots.First<KeyValuePair<string, HeadShot>>((Func<KeyValuePair<string, HeadShot>, bool>) (r => r.Value.Handle == _handle)).Key);
    }

    public static bool HeadShotValid(HeadShot _h)
    {
      if (Function.Call<bool>(Hash._0x7085228842B13A67, (InputArgument) _h.Handle))
      {
        if (Function.Call<bool>(Hash._0xA0A9668F158129A2, (InputArgument) _h.Handle))
          return true;
      }
      return false;
    }

    public static void MugShot(Ped _ped, string _name)
    {
      int _handle = Function.Call<int>(Hash._0x4462658788425076, (InputArgument) _ped);
      while (true)
      {
        if (Function.Call<bool>(Hash._0x7085228842B13A67, (InputArgument) _handle))
          goto label_3;
label_1:
        Script.Yield();
        continue;
label_3:
        if (!Function.Call<bool>(Hash._0xA0A9668F158129A2, (InputArgument) _handle))
          goto label_1;
        else
          break;
      }
      string _texure = Function.Call<string>(Hash._0xDB4EACD4AD0A5D6B, (InputArgument) _handle);
      HeadShotHandler.headShots.Add(_name, new HeadShot(_handle, _texure, (PedHash) _ped.Model.Hash));
    }
  }
}
