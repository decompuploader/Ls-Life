using GTA;
using GTA.Native;

namespace LSlife
{
  internal class HeadShot
  {
    internal int Handle;
    internal string Texture;
    internal PedHash PedHash;
    internal int timeCreated;

    public HeadShot(int _handle, string _texure, PedHash _hash)
    {
      this.Handle = _handle;
      this.Texture = _texure;
      this.PedHash = _hash;
      this.timeCreated = Game.GameTime;
    }
  }
}
