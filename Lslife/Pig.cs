using GTA;
using GTA.Math;
using GTA.Native;

namespace LSlife
{
  public class Pig
  {
    private readonly bool DEBUG;
    public Ped Ped;
    public Vehicle Vehicle;
    public Pig.States CurrentState = Pig.States.NoState;
    public Pig.States NewState = Pig.States.NoState;
    public Vector3 destination = Vector3.Zero;
    private bool leader;
    private int time;
    private int delay = 20000;
    private int timeSeen;
    private bool sayWait;
    public bool detected;
    public bool seenPlayer;
    private Vector3 pPos = Vector3.Zero;

    public bool AskPlayerToComply { get; private set; }

    public bool SirenActive { get; private set; }

    public bool SirenSoundActive { get; private set; } = true;

    public Pig(Ped _ped, bool _leader)
    {
      this.Ped = _ped;
      this.leader = _leader;
      if ((Entity) this.Ped.CurrentVehicle != (Entity) null)
        this.Vehicle = this.Ped.CurrentVehicle;
      this.destination = this.StopPosition();
      this.Ped.IsPersistent = true;
      this.Ped.AlwaysKeepTask = true;
      this.Ped.BlockPermanentEvents = true;
      Function.Call(Hash._0xB195FFA8042FC5C3, (InputArgument) this.Ped, (InputArgument) 100);
    }

    private Vector3 StopPosition()
    {
      Vector3 positionOnStreet1 = World.GetNextPositionOnStreet(Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3f);
      Vector3 positionOnStreet2 = World.GetNextPositionOnStreet(Game.Player.Character.Position + Game.Player.Character.ForwardVector * -3f * 5f);
      Vector3 zero = Vector3.Zero;
      return (double) positionOnStreet1.DistanceTo(this.Ped.Position) >= (double) positionOnStreet2.DistanceTo(this.Ped.Position) ? positionOnStreet2 : positionOnStreet1;
    }

    public void OnTick()
    {
      this.DoState();
      this.CheckState();
      if (Game.Player.WantedLevel <= 0 && LSL.isDealing)
        return;
      this.DeletePig();
    }

    public void CheckState()
    {
      switch (this.CurrentState)
      {
        case Pig.States.NoState:
          this.NewState = Pig.States.driveToPlayer;
          break;
        case Pig.States.driveToPlayer:
          Vector3 vector3 = this.StopPosition();
          if (this.destination != vector3)
          {
            this.destination = vector3;
            this.NewState = Pig.States.NoState;
            break;
          }
          if ((double) this.Vehicle.Position.DistanceTo(this.destination) < 15.0 && this.Vehicle.IsStopped)
          {
            if ((double) this.Vehicle.Position.DistanceTo(LSL.playerPos) > 50.0)
            {
              if (LSL.ratChance > 25)
              {
                this.destination = this.StopPosition();
                this.NewState = Pig.States.NoState;
              }
              else
                this.NewState = Pig.States.Destroy;
            }
            else if ((Entity) Game.Player.Character.CurrentVehicle != (Entity) null)
            {
              if ((double) Game.Player.Character.CurrentVehicle.Speed < 1.0)
              {
                this.NewState = Pig.States.ApproachPlayer;
              }
              else
              {
                if (Game.GameTime > this.time + 3000)
                {
                  this.time = Game.GameTime;
                  Function.Call(Hash._0x1B9025BDA76822B6, (InputArgument) this.Vehicle.Handle);
                }
                if (!this.seenPlayer && LsFunctions.CanSee(this.Ped, Game.Player.Character))
                {
                  this.seenPlayer = true;
                  this.timeSeen = Game.GameTime;
                }
                else if (!LsFunctions.CanSee(this.Ped, Game.Player.Character))
                {
                  this.seenPlayer = false;
                  if (Game.Player.WantedLevel < 1)
                    Game.Player.WantedLevel = 1;
                  LSL.areas[LSL.areaIndex].Heat += 10;
                }
                else if (Game.GameTime > this.timeSeen + this.delay + 10000)
                {
                  if (Game.Player.WantedLevel < 1)
                    Game.Player.WantedLevel = 1;
                  LSL.areas[LSL.areaIndex].Heat += 10;
                }
              }
            }
            else
              this.NewState = Pig.States.ApproachPlayer;
          }
          if (!this.leader || (double) this.Vehicle.Position.DistanceTo(this.destination) >= 50.0)
            break;
          if (!this.SirenActive)
          {
            Function.Call(Hash._0xF4924635A19EB37D, (InputArgument) this.Vehicle.Handle, (InputArgument) true);
            this.SirenActive = true;
          }
          if (!this.SirenSoundActive)
            break;
          this.SirenSoundActive = false;
          Function.Call(Hash._0xD8050E0EB60CF274, (InputArgument) this.Vehicle.Handle, (InputArgument) true);
          break;
        case Pig.States.Normal:
          if ((double) this.Ped.Position.DistanceTo(Game.Player.Character.Position) <= 80.0)
            break;
          this.NewState = Pig.States.ApproachPlayer;
          break;
        case Pig.States.ApproachPlayer:
          if (Game.Player.WantedLevel > 0)
          {
            this.NewState = Pig.States.Destroy;
            break;
          }
          if (Game.Player.WantedLevel < 1 && (double) LSL.playerPos.DistanceTo(this.pPos) > 8.0)
          {
            LsFunctions.DbugString("Wanted level for not stopping for police");
            Game.Player.WantedLevel = 2;
            LSL.areas[LSL.areaIndex].Heat += 200;
            break;
          }
          if ((double) this.Ped.Position.DistanceTo(this.destination) < 1.0)
          {
            this.pPos = LSL.playerPos;
            this.NewState = Pig.States.WatchPlayer;
          }
          if (this.sayWait || this.Ped.IsInVehicle() || (!this.Ped.IsWalking || (double) LSL.playerPos.DistanceTo(this.Ped.Position) >= 10.0))
            break;
          this.sayWait = true;
          Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) this.Ped, (InputArgument) "WAIT", (InputArgument) "SPEECH_PARAMS_STANDARD");
          break;
        case Pig.States.WatchPlayer:
          if (Game.Player.WantedLevel > 0)
          {
            this.NewState = Pig.States.Destroy;
            break;
          }
          if (Game.Player.WantedLevel < 1)
          {
            if ((Entity) Game.Player.Character.CurrentVehicle != (Entity) null && !Game.Player.Character.CurrentVehicle.IsStopped)
            {
              LsFunctions.DbugString("Wanted level for not stopping for police in vehicle");
              Game.Player.WantedLevel = 1;
              LSL.areas[LSL.areaIndex].Heat += 200;
              break;
            }
            if (Game.GameTime > this.time + this.delay || (double) LSL.playerPos.DistanceTo(this.pPos) > 2.0)
            {
              Game.Player.WantedLevel = 1;
              LSL.areas[LSL.areaIndex].Heat += 200;
              break;
            }
          }
          if (!LSL.PlayerComplied || Game.Player.Character.IsInVehicle())
            break;
          this.AskPlayerToComply = false;
          this.NewState = Pig.States.SearchPlayer;
          break;
        case Pig.States.SearchPlayer:
          if (Game.GameTime <= this.time + 3000)
            break;
          if (LSL.TotalDrugsCarried() > 0)
          {
            if (this.leader)
            {
              UI.ShowSubtitle("Your busted.", 3000);
              Game.Player.WantedLevel = 2;
            }
            this.NewState = Pig.States.Destroy;
            break;
          }
          if (this.leader)
            UI.ShowSubtitle("Ok we got our eye on you, get outa here.", 4000);
          LSL.areas[LSL.areaIndex].HeatDecay();
          Game.Player.Character.Task.ClearAll();
          this.NewState = Pig.States.Destroy;
          break;
      }
    }

    public void DoState()
    {
      if (this.NewState == this.CurrentState)
        return;
      if (this.DEBUG)
        UI.Notify("Police new State " + this.NewState.ToString());
      switch (this.NewState)
      {
        case Pig.States.Destroy:
          LsFunctions.RemovePedFromWorld(this.Ped, true);
          this.Ped.Task.Arrest(Game.Player.Character);
          this.Ped = (Ped) null;
          if ((Entity) this.Vehicle != (Entity) null)
            this.Vehicle.IsPersistent = false;
          LSL.entitiesCleanUp.Add((Entity) this.Vehicle);
          this.Vehicle = (Vehicle) null;
          this.CurrentState = Pig.States.Destroy;
          break;
        case Pig.States.NoState:
          this.CurrentState = Pig.States.NoState;
          break;
        case Pig.States.driveToPlayer:
          this.destination = this.StopPosition();
          if ((Entity) this.Ped == (Entity) this.Vehicle.Driver)
            this.Ped.Task.DriveTo(this.Vehicle, this.destination, 10f, 12f, 786468);
          this.CurrentState = Pig.States.driveToPlayer;
          break;
        case Pig.States.Normal:
          if ((Entity) this.Vehicle != (Entity) null)
            this.Ped.Task.CruiseWithVehicle(this.Vehicle, 9f, 786603);
          else
            this.Ped.Task.WanderAround();
          this.CurrentState = Pig.States.Normal;
          if (!this.DEBUG)
            break;
          UI.Notify("Changing to normal");
          break;
        case Pig.States.ApproachPlayer:
          this.destination = !((Entity) Game.Player.Character.CurrentVehicle != (Entity) null) ? (!this.leader ? LSL.playerPos + Game.Player.Character.RightVector * -1f * 2f + Game.Player.Character.ForwardVector * 2f : LSL.playerPos + Game.Player.Character.ForwardVector * 2f) : (!this.leader ? LSL.playerPos + Game.Player.Character.RightVector * -1f * 2f + Game.Player.Character.ForwardVector * -1f * 2f : LSL.playerPos + Game.Player.Character.RightVector * -1f * 2f);
          this.Ped.Task.GoTo(this.destination);
          this.pPos = LSL.playerPos;
          this.Ped.Task.LookAt((Entity) Game.Player.Character);
          this.CurrentState = Pig.States.ApproachPlayer;
          break;
        case Pig.States.WatchPlayer:
          this.Ped.Task.ClearAll();
          this.Ped.Weapons.Give(WeaponHash.CombatPistol, 100, true, true);
          this.Ped.Task.AimAt((Entity) Game.Player.Character, 20000);
          this.time = Game.GameTime;
          this.CurrentState = Pig.States.WatchPlayer;
          if ((Entity) Game.Player.Character.CurrentVehicle != (Entity) null)
          {
            if (this.leader)
              UI.ShowSubtitle("Sir, please step out of the vehicle.", 4000);
            this.AskPlayerToComply = true;
          }
          else
          {
            if (this.leader)
              UI.ShowSubtitle("Get down on the ground", 4000);
            this.AskPlayerToComply = true;
          }
          if (!this.DEBUG)
            break;
          UI.Notify("Changing to watch");
          break;
        case Pig.States.SearchPlayer:
          if (this.leader)
          {
            TaskSequence sequence = new TaskSequence();
            sequence.AddTask.GoTo(this.Ped.Position + this.Ped.ForwardVector * 0.5f, false, 3000);
            sequence.AddTask.TurnTo((Entity) Game.Player.Character, 1000);
            sequence.AddTask.PlayAnimation("missexile3", "ex03_dingy_search_case_base_michael");
            this.Ped.Task.PerformSequence(sequence);
          }
          this.time = Game.GameTime;
          this.CurrentState = Pig.States.SearchPlayer;
          break;
      }
    }

    public void DeletePig() => this.NewState = Pig.States.Destroy;

    public enum States
    {
      Destroy,
      NoState,
      driveToPlayer,
      Normal,
      ApproachPlayer,
      WatchPlayer,
      SearchPlayer,
    }
  }
}
