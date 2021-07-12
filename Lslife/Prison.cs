using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Linq;

namespace LSlife
{
  internal static class Prison
  {
    public static LsTimer SentanceTime = (LsTimer) null;
    private static int BailAmount = -1;
    private static Prison.PrisonStates NewState = Prison.PrisonStates.None;
    public static bool Started = false;
    public static bool Ready = false;
    private static Ped cop = (Ped) null;
    private static Vehicle copCar = (Vehicle) null;
    private static Vector3 PrisonPos = new Vector3(1946.057f, 2617.254f, 45.63307f);
    private static Vector3 BarrierWaitPos = new Vector3(1899.201f, 2609.867f, 45.34259f);
    private static Vector3 Gate1WaitPos = new Vector3(1851.723f, 2608.119f, 45.23964f);
    private static Vector3 Gate2WaitPos = new Vector3(1824.335f, 2607.923f, 45.18952f);
    private static Vector3 Gate2ExitPos = new Vector3(1798.774f, 2606.744f, 45.16935f);
    private static Vector3 FinalPos = new Vector3(1690.612f, 2604.468f, 45.16944f);
    private static Prison.Gate PriGate1 = (Prison.Gate) null;
    private static Prison.Gate PriGate2 = (Prison.Gate) null;
    private static bool DEBUG = LSL.DEBUG;
    private static float Speed = 0.0f;

    public static Prison.PrisonStates CurrentState { get; set; } = Prison.PrisonStates.ApproachGate2;

    public static void PrisonTick()
    {
      Prison.DriverSpeed(Prison.cop, Prison.copCar);
      Prison.CancelWantedLevels();
      if (Prison.NewState != Prison.CurrentState)
      {
        bool flag = false;
        switch (Prison.NewState)
        {
          case Prison.PrisonStates.None:
            if (Prison.Ready)
            {
              Game.Player.IgnoredByPolice = true;
              Game.Player.IgnoredByEveryone = true;
              Vector3 position = Game.Player.Character.Position;
              OutputArgument outputArgument1 = new OutputArgument();
              OutputArgument outputArgument2 = new OutputArgument();
              if (Function.Call<bool>(Hash._0xFF071FB798B803B0, (InputArgument) position.X, (InputArgument) position.Y, (InputArgument) position.Z, (InputArgument) outputArgument1, (InputArgument) outputArgument2, (InputArgument) 0, (InputArgument) 1077936128, (InputArgument) 0))
              {
                float result1 = outputArgument2.GetResult<float>();
                Vector3 result2 = outputArgument1.GetResult<Vector3>();
                if ((Entity) Prison.copCar == (Entity) null)
                {
                  Prison.copCar = World.CreateVehicle((Model) VehicleHash.Police, result2, result1);
                  Prison.copCar.AddBlip();
                  Prison.copCar.CurrentBlip.Sprite = BlipSprite.CopCar;
                  if (Prison.DEBUG)
                    UI.Notify("transport Spawned" + Prison.copCar?.ToString());
                }
              }
              if ((Entity) Prison.cop == (Entity) null)
                Prison.cop = World.CreatePed((Model) PedHash.Cop01SMY, Game.Player.Character.Position + Game.Player.Character.ForwardVector * -1f * 1.5f);
              if ((Entity) Prison.copCar != (Entity) null && (Entity) Prison.cop != (Entity) null)
              {
                Function.Call(Hash._0xD3BD40951412FEF6, (InputArgument) "mp_arresting");
                while (true)
                {
                  if (!Function.Call<bool>(Hash._0xD031A9162D01088C, (InputArgument) "mp_arresting"))
                    Script.Wait(100);
                  else
                    break;
                }
                Function.Call(Hash._0xEA47FE3719165B94, (InputArgument) Game.Player.Character, (InputArgument) "mp_arresting", (InputArgument) "idle", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 49, (InputArgument) 0.1f, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0);
                Game.Player.Character.Task.EnterVehicle(Prison.copCar, VehicleSeat.RightRear);
                if (Function.Call<bool>(Hash._0x49C32D60007AFA47))
                  Function.Call(Hash._0x8D32347D6D4C40A2, (InputArgument) Game.Player.Handle, (InputArgument) false, (InputArgument) 256);
                Function.Call(Hash._0x304AE42E357B8C7E, (InputArgument) Prison.cop.Handle, (InputArgument) Game.Player.Character.Handle, (InputArgument) 1, (InputArgument) 1, (InputArgument) 0, (InputArgument) 1, (InputArgument) -1, (InputArgument) 10, (InputArgument) true);
                Prison.CurrentState = Prison.PrisonStates.None;
                flag = true;
                break;
              }
              break;
            }
            break;
          case Prison.PrisonStates.PlayerEnterCar:
            Prison.cop.Task.ClearAll();
            Prison.cop.AlwaysKeepTask = true;
            Prison.cop.BlockPermanentEvents = true;
            Prison.cop.Task.EnterVehicle(Prison.copCar, VehicleSeat.Driver);
            Prison.CurrentState = Prison.PrisonStates.PlayerEnterCar;
            flag = true;
            break;
          case Prison.PrisonStates.CopEnterCar:
            Function.Call(Hash._0xB195FFA8042FC5C3, (InputArgument) Prison.cop.Handle, (InputArgument) 100f);
            Prison.cop.Task.DriveTo(Prison.copCar, Prison.PrisonPos, 5f, Prison.Speed, 5);
            Prison.CurrentState = Prison.PrisonStates.CopEnterCar;
            flag = true;
            break;
          case Prison.PrisonStates.ApproachBarrier:
            Prison.cop.Task.DriveTo(Prison.copCar, Prison.BarrierWaitPos, 1f, Prison.Speed, 2883621);
            Prison.CurrentState = Prison.PrisonStates.ApproachBarrier;
            flag = true;
            break;
          case Prison.PrisonStates.ApproachGate1:
            Prison.cop.Task.DriveTo(Prison.copCar, Prison.Gate1WaitPos, 1f, Prison.Speed, 262144);
            Prison.CurrentState = Prison.PrisonStates.ApproachGate1;
            flag = true;
            break;
          case Prison.PrisonStates.ApproachGate2:
            Prison.cop.Task.DriveTo(Prison.copCar, Prison.Gate2WaitPos, 1f, Prison.Speed, 262144);
            Prison.CurrentState = Prison.PrisonStates.ApproachGate2;
            flag = true;
            break;
          case Prison.PrisonStates.FinalApproach:
            Prison.cop.Task.DriveTo(Prison.copCar, Prison.Gate2ExitPos, 1f, Prison.Speed, 262144);
            Prison.CurrentState = Prison.PrisonStates.FinalApproach;
            flag = true;
            break;
          case Prison.PrisonStates.FinalPos:
            Prison.cop.Task.DriveTo(Prison.copCar, Prison.FinalPos, 1f, Prison.Speed, 262144);
            Prison.CurrentState = Prison.PrisonStates.FinalPos;
            flag = true;
            break;
        }
        if (!flag)
          return;
        UI.Notify("Switching to state " + Prison.NewState.ToString());
      }
      else
      {
        switch (Prison.CurrentState)
        {
          case Prison.PrisonStates.None:
            if (!Game.Player.Character.IsInVehicle(Prison.copCar) || Prison.cop.IsInVehicle(Prison.copCar))
              break;
            Prison.NewState = Prison.PrisonStates.PlayerEnterCar;
            break;
          case Prison.PrisonStates.PlayerEnterCar:
            if (!Prison.cop.IsInVehicle(Prison.copCar))
              break;
            Script.Wait(1000);
            Prison.NewState = Prison.PrisonStates.CopEnterCar;
            break;
          case Prison.PrisonStates.CopEnterCar:
            if ((double) Prison.copCar.Position.DistanceTo(Prison.PrisonPos) >= 50.0)
              break;
            Prison.NewState = Prison.PrisonStates.ApproachBarrier;
            break;
          case Prison.PrisonStates.ApproachBarrier:
            if ((double) Prison.copCar.Position.DistanceTo(Prison.BarrierWaitPos) > 2.0)
              break;
            Prison.NewState = Prison.PrisonStates.ApproachGate1;
            break;
          case Prison.PrisonStates.ApproachGate1:
            if ((double) Prison.copCar.Position.DistanceTo(Prison.Gate1WaitPos) > 5.0 || Prison.PriGate1 != null || !((Entity) Prison.pGate1() != (Entity) null))
              break;
            Prison.PriGate1 = new Prison.Gate(Prison.pGate1());
            Prison.PriGate1.Prop.FreezePosition = false;
            Prison.NewState = Prison.PrisonStates.ApproachGate2;
            UI.Notify("Gate Found " + Prison.pGate1().Model.Hash.ToString());
            break;
          case Prison.PrisonStates.ApproachGate2:
            Prison.PriGate1.Unlock();
            if ((double) Prison.copCar.Position.DistanceTo(Prison.Gate2WaitPos) > 3.0 || Prison.PriGate2 != null || !((Entity) Prison.pGate2() != (Entity) null))
              break;
            Prison.PriGate2 = new Prison.Gate(Prison.pGate2());
            Prison.PriGate2.Prop.FreezePosition = false;
            Prison.NewState = Prison.PrisonStates.FinalApproach;
            break;
          case Prison.PrisonStates.FinalApproach:
            Prison.PriGate2.Unlock();
            if ((double) Prison.copCar.Position.DistanceTo(Prison.Gate2ExitPos) > 3.0)
              break;
            Prison.NewState = Prison.PrisonStates.FinalPos;
            break;
          case Prison.PrisonStates.FinalPos:
            if ((double) Prison.copCar.Position.DistanceTo2D(Prison.FinalPos) >= 10.0)
              break;
            Prison.PlayerExitPrison();
            break;
        }
      }
    }

    private static void DriverSpeed(Ped p, Vehicle v)
    {
      if (!((Entity) v != (Entity) null) || !((Entity) p != (Entity) null) || v.Driver.Handle != p.Handle)
        return;
      float roadSpeed = LsFunctions.GetRoadSpeed(v.Position + v.Velocity, false, false);
      if ((double) roadSpeed == (double) Prison.Speed)
        return;
      float num = 0.01f;
      Prison.Speed = roadSpeed;
      if ((double) v.Speed > (double) Prison.Speed)
      {
        p.MaxDrivingSpeed = v.Speed - num * Game.LastFrameTime;
        p.DrivingSpeed = v.Speed - num * Game.LastFrameTime;
      }
      else
      {
        p.MaxDrivingSpeed = Prison.Speed;
        p.DrivingSpeed = Prison.Speed;
      }
    }

    private static void CancelWantedLevels()
    {
      if (Game.MaxWantedLevel == 0)
        return;
      Game.MaxWantedLevel = 0;
    }

    private static void PlayerExitPrison()
    {
      Function.Call(Hash._0x8D32347D6D4C40A2, (InputArgument) Game.Player.Handle, (InputArgument) true, (InputArgument) 256);
      Game.Player.CanControlCharacter = true;
      LSL.entitiesCleanUp.Add((Entity) Prison.copCar);
      LSL.entitiesCleanUp.Add((Entity) Prison.cop);
      Prison.cop = (Ped) null;
      Prison.copCar = (Vehicle) null;
      Game.MaxWantedLevel = 5;
      Game.Player.Character.Position = Prison.Gate1WaitPos;
      Prison.Started = false;
      Prison.NewState = Prison.PrisonStates.None;
    }

    public static Prop pGate1()
    {
      int num = 741314661;
      Prop prop1 = (Prop) null;
      Prop[] nearbyProps = World.GetNearbyProps(new Vector3(1844.998f, 2604.813f, 44.6398f), 10f);
      if (((IEnumerable<Prop>) nearbyProps).Count<Prop>() > 0)
      {
        foreach (Prop prop2 in nearbyProps)
        {
          if (prop2.Model.Hash == num)
          {
            prop1 = prop2;
            UI.Notify("Gate1 found");
            break;
          }
        }
      }
      return prop1;
    }

    public static Prop pGate2()
    {
      int num = 741314661;
      Prop prop1 = (Prop) null;
      Prop[] nearbyProps = World.GetNearbyProps(new Vector3(1818.543f, 2604.813f, 44.611f), 10f);
      if (((IEnumerable<Prop>) nearbyProps).Count<Prop>() > 0)
      {
        foreach (Prop prop2 in nearbyProps)
        {
          if (prop2.Model.Hash == num)
          {
            prop1 = prop2;
            UI.Notify("Gate2 found");
            break;
          }
        }
      }
      return prop1;
    }

    private static float changeSpeed(float fromVal, float toVal, float duration) => 0.0f;

    private static float MaxSpeed()
    {
      LSL.GetRoadFlags(Prison.copCar.Position);
      return 0.0f;
    }

    public enum PrisonStates
    {
      None,
      PlayerEnterCar,
      CopEnterCar,
      ApproachBarrier,
      ApproachGate1,
      ApproachGate2,
      FinalApproach,
      FinalPos,
    }

    public class Gate
    {
      private readonly bool DEBUG = true;
      public Prop Prop;
      private bool Locked = true;

      public Gate(Prop prop) => this.Prop = prop;

      public void Unlock()
      {
        Script.Wait(10);
        Function.Call(Hash._0x9B12F9A24FABEDB0, (InputArgument) this.Prop.Model.Hash, (InputArgument) this.Prop.Position.X, (InputArgument) this.Prop.Position.Y, (InputArgument) this.Prop.Position.Z, (InputArgument) false, (InputArgument) 0.0f, (InputArgument) 50f, (InputArgument) 0.0f);
      }

      public void Lock()
      {
        Script.Wait(10);
        Function.Call(Hash._0x9B12F9A24FABEDB0, (InputArgument) this.Prop.Model.Hash, (InputArgument) this.Prop.Position.X, (InputArgument) this.Prop.Position.Y, (InputArgument) this.Prop.Position.Z, (InputArgument) true, (InputArgument) 0.0f, (InputArgument) 50f, (InputArgument) 0.0f);
      }
    }
  }
}
