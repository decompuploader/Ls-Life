using GTA;

namespace LSlife
{
  public class LsTimer
  {
    private int Time;
    private int TotalTime;
    private bool TimeOut;
    public bool StartTimer;
    private int lastTime;

    public LsTimer(int time, bool timeOut, bool start)
    {
      this.TotalTime = time;
      this.TimeOut = timeOut;
      this.lastTime = Game.GameTime;
      this.StartTimer = start;
    }

    public void Tick()
    {
      if (!this.StartTimer || Game.GameTime <= this.lastTime + 1000)
        return;
      this.lastTime = Game.GameTime;
      if (!this.Finished())
      {
        ++this.Time;
      }
      else
      {
        if (!this.TimeOut)
          return;
        this.Time = 0;
      }
    }

    public bool Finished() => this.Time >= this.TotalTime;

    public int Remaining() => this.Time < this.TotalTime ? this.Time - this.TotalTime : 0;

    public int Elapsed() => this.Time;

    public void Start()
    {
      this.StartTimer = true;
      this.lastTime = Game.GameTime;
    }

    public void Stop() => this.StartTimer = false;

    public void Reset()
    {
      this.Time = 0;
      this.lastTime = Game.GameTime;
    }
  }
}
