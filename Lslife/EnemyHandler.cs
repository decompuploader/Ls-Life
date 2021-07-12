using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Linq;

namespace LSlife
{
  internal class EnemyHandler
  {
    private List<Vector3> SpawnLocations = new List<Vector3>()
    {
      new Vector3(-131.695f, -1509.14f, 34.10052f),
      new Vector3(-161.0194f, -1515.375f, 33.68655f),
      new Vector3(-172.2392f, -1492.894f, 32.55872f)
    };
    private Vector3 ClosePos;
    private List<Ped> Enemies = new List<Ped>();
    private int MaxPeds = 15;
    private int TimeBetweenSpawns = 500;
    private int Time;
    private List<PedHash> pedHashes = new List<PedHash>()
    {
      PedHash.AvonGoon,
      PedHash.Juggernaut01M
    };
    private System.Random Rand = new System.Random();

    public bool Started { get; private set; }

    public void Ontick()
    {
      if (this.Started && Game.GameTime > this.Time + this.TimeBetweenSpawns && this.Enemies.Count < this.MaxPeds)
      {
        int index1 = -1;
        bool flag = false;
        this.ClosePos = this.SpawnLocations[0];
        foreach (Vector3 spawnLocation in this.SpawnLocations)
        {
          if ((double) spawnLocation.DistanceTo(Game.Player.Character.Position) < (double) this.ClosePos.DistanceTo(Game.Player.Character.Position))
            this.ClosePos = spawnLocation;
        }
        while (!flag)
        {
          index1 = this.Rand.Next(0, this.SpawnLocations.Count);
          if (this.SpawnLocations[index1] != this.ClosePos)
            flag = true;
        }
        int index2 = this.Rand.Next(0, 2);
        Ped ped = World.CreatePed((Model) this.pedHashes[index2], this.SpawnLocations[index1]);
        while ((Entity) ped == (Entity) null)
          Script.Wait(0);
        switch (index2)
        {
          case 0:
            ped.Weapons.Give(WeaponHash.CarbineRifle, 1000, true, true);
            Function.Call(Hash._0x4D9CA1009AFBD057, (InputArgument) ped, (InputArgument) 2);
            break;
          case 1:
            ped.Weapons.Give(WeaponHash.Minigun, 10000, true, true);
            ped.CanSufferCriticalHits = false;
            ped.Health = 4000;
            Function.Call(Hash._0x4D9CA1009AFBD057, (InputArgument) ped, (InputArgument) 3);
            break;
        }
        Function.Call(Hash._0xC7622C0D36B2FDA8, (InputArgument) ped, (InputArgument) 100);
        ped.FiringPattern = FiringPattern.FullAuto;
        ped.CanWrithe = false;
        ped.Armor = 100;
        ped.AlwaysKeepTask = true;
        ped.Task.FightAgainst(Game.Player.Character);
        ped.AddBlip();
        ped.CurrentBlip.IsFriendly = false;
        this.Enemies.Add(ped);
        this.Time = Game.GameTime;
      }
      this.CheckPeds();
    }

    public void Start() => this.Started = true;

    public void Stop() => this.Started = false;

    private void CheckPeds()
    {
      if (this.Enemies.Count <= 0)
        return;
      foreach (Ped ped in this.Enemies.ToList<Ped>())
      {
        if ((Entity) ped == (Entity) null || !ped.Exists())
          this.Enemies.Remove(ped);
        else if (ped.IsDead)
        {
          ped.CurrentBlip.Remove();
          this.Enemies.Remove(ped);
        }
      }
    }

    private void RemoveEnemies()
    {
      foreach (Ped ped in this.Enemies.ToList<Ped>())
      {
        ped.CurrentBlip.Remove();
        this.Enemies.Remove(ped);
      }
    }
  }
}
