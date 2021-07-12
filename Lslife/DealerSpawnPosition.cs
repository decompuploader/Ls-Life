using GTA.Math;

namespace LSlife
{
  internal class DealerSpawnPostion
  {
    public Vector3 Position;
    public float Heading;

    public DealerSpawnPostion(Vector3 _position, float _heading)
    {
      this.Position = _position;
      this.Heading = _heading;
    }
  }
}
