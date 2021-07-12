using GTA;
using GTA.Math;
using GTA.Native;

namespace LSlife
{
  public class Dealer
  {
    public float maxSpeed;
    public Vector3 destination;
    public bool doneTutorial;
    public Dealer.States state;
    public Ped ped;
    public Vector3 pos;
    private Model vehModel;
    public Vehicle vehicle;
    internal bool canSpeed;
    internal bool shouldSpeed;

    public Dealer(Model pedModel, Vector3 pos, Model vehModel, PedGroup group)
    {
      this.pos = pos;
      this.vehModel = vehModel;
      this.vehicle = World.CreateVehicle(this.vehModel, LSL.GenerateSpawnPos(pos.Around(80f), LSL.Nodetype.Road, false));
      while ((Entity) this.vehicle == (Entity) null)
        Script.Wait(0);
      this.vehicle.IsPersistent = true;
      LsFunctions.AddBlip((Entity) this.vehicle, BlipColor.Blue, "Zee's Driver", true, false);
      this.ped = World.CreatePed(pedModel, this.vehicle.Position);
      while ((Entity) this.ped == (Entity) null)
        Script.Wait(0);
      this.ped.IsPersistent = true;
      this.ped.SetIntoVehicle(this.vehicle, VehicleSeat.Driver);
      Function.Call(Hash._0xB195FFA8042FC5C3, (InputArgument) this.ped.Handle, (InputArgument) 100f);
      Function.Call(Hash._0xB5C51B5502E85E83, (InputArgument) this.vehicle.Handle, (InputArgument) this.ped.Handle);
      this.destination = World.GetNextPositionOnStreet(pos, true);
      this.ped.Task.DriveTo(this.vehicle, this.destination, 5f, 25f, 5);
      this.state = Dealer.States.Driving;
      this.ped.StaysInVehicleWhenJacked = true;
      this.ped.BlockPermanentEvents = true;
      this.maxSpeed = Function.Call<float>(Hash._0xF417C2502FFFED43, (InputArgument) this.vehicle) * 0.8f;
    }

    internal void SetSpeed()
    {
      if ((this.state == Dealer.States.Driving ? 1 : (this.state == Dealer.States.Fast ? 1 : 0)) == 0)
        return;
      Vector3 vector3 = this.vehicle.Position + this.vehicle.Velocity * 2f;
      this.shouldSpeed = LsFunctions.ShouldSpeed(vector3);
      float roadSpeed = LsFunctions.GetRoadSpeed(vector3, this.canSpeed, this.shouldSpeed);
      if ((double) this.maxSpeed > (double) roadSpeed)
        this.maxSpeed -= 10f * Game.LastFrameTime;
      else
        this.maxSpeed = roadSpeed;
      this.ped.DrivingSpeed = this.maxSpeed;
      this.ped.MaxDrivingSpeed = this.maxSpeed;
    }

    public enum States
    {
      Driving,
      Ariving,
      Leaving,
      Arrived,
      Faster,
      Fast,
      Slower,
    }
  }
}
