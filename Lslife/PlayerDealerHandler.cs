using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LSlife
{
  public class PlayerDealerHandler
  {
    private int gameHour = Function.Call<int>(Hash._0x25223CA6B4D20B7F);
    public bool needsSave;
    private List<int> allocatedIDs = new List<int>();
    public PlayerDealer currentDealer;
    public List<PedTeam> teams = new List<PedTeam>();
    public List<Vector3> HealLocations = new List<Vector3>()
    {
      new Vector3(342.7698f, -1397.948f, 32.50927f),
      new Vector3(298.8214f, -584.4205f, 43.26083f),
      new Vector3(355.9612f, -596.3049f, 28.77687f),
      new Vector3(-448.425f, -340.619f, 34.50178f),
      new Vector3(1839.235f, 3673.061f, 34.27673f)
    };
    public Enums.HealPositionTypes HealPositionType;

    public List<PlayerDealer> dealers { get; set; } = new List<PlayerDealer>();

    public PlayerDealerHandler() => this.LoadDealers();

    public void OnTick()
    {
      if (this.currentDealer != null && (double) this.currentDealer.Ped.Position.DistanceTo(LSL.playerPos) < 5.0)
        World.DrawMarker(MarkerType.UpsideDownCone, this.currentDealer.Ped.Position + new Vector3(0.0f, 0.0f, 1.5f), Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), Color.Green);
      if (this.dealers.Count <= 0)
        return;
      foreach (PlayerDealer dealer in this.dealers)
      {
        if (dealer == null)
        {
          this.dealers.Remove(dealer);
          break;
        }
        dealer.OnTick();
      }
      if (!this.needsSave)
        return;
      this.SaveDealers();
      this.needsSave = false;
    }

    public void OnSlowTick()
    {
      if (this.dealers.Count <= 0)
        return;
      int num = Function.Call<int>(Hash._0x25223CA6B4D20B7F);
      int _hours = 0;
      if (this.gameHour != num)
      {
        _hours = num <= this.gameHour ? 24 - this.gameHour + num : num - this.gameHour;
        this.gameHour = num;
        if (LSL.DEBUG)
          UI.Notify("DealerSim Hours Passed " + _hours.ToString());
      }
      foreach (PlayerDealer playerDealer in this.dealers.ToList<PlayerDealer>())
      {
        if (playerDealer == null)
        {
          this.dealers.Remove(playerDealer);
        }
        else
        {
          if (playerDealer.CurrentState != Enums.PdStates.Dead && playerDealer.CurrentState != Enums.PdStates.FollowPlayer && (playerDealer.CurrentState != Enums.PdStates.FollowingPlayer && _hours > 0))
          {
            playerDealer.SimulateSales(_hours);
            this.needsSave = true;
          }
          if (playerDealer.CurrentState == Enums.PdStates.Dead)
          {
            this.DeleteDealer(playerDealer.ID);
            this.dealers.Remove(playerDealer);
            break;
          }
          switch (playerDealer)
          {
            case null:
            case null:
              continue;
            default:
              playerDealer.OnSlowTick();
              continue;
          }
        }
      }
    }

    private void LoadDealers()
    {
      XDocument xdocument;
      if (!File.Exists("scripts\\LSLife\\LSLife_Dealers.xml"))
      {
        xdocument = new XDocument(new object[1]
        {
          (object) new XElement((XName) "dealers")
        });
        xdocument.Save("scripts\\LSLife\\LSLife_Dealers.xml");
      }
      else
        xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Dealers.xml");
      if (xdocument.Descendants().ToList<XElement>().Count <= 0)
        return;
      List<int> intList = new List<int>();
      foreach (XElement descendant in xdocument.Descendants())
      {
        if (descendant.Name == (XName) "dealer")
        {
          foreach (XElement element in descendant.Elements())
          {
            bool flag = true;
            if (element.Name == (XName) "id")
            {
              int id;
              int.TryParse(element.Value, out id);
              if (intList.Count > 0)
              {
                while (flag)
                {
                  if (intList.FindAll((Predicate<int>) (d => d == id)).Count == 0)
                    flag = false;
                  else
                    id++;
                }
              }
              else
                flag = false;
              if (!flag)
              {
                intList.Add(id);
                element.Value = id.ToString();
              }
            }
          }
        }
      }
      xdocument.Save("scripts\\LSLife\\LSLife_Dealers.xml");
      foreach (XElement descendant in xdocument.Descendants())
      {
        if (descendant.Name == (XName) "dealer")
        {
          int result1 = 0;
          PedHash result2 = PedHash.Acult01AMM;
          Vector3 pos = Vector3.Zero;
          int result3 = 0;
          int result4 = 0;
          int result5 = 0;
          int result6 = 0;
          int result7 = -1;
          List<XElement> _weapons = new List<XElement>();
          Dictionary<int, Tuple<int, int>> _pData = new Dictionary<int, Tuple<int, int>>();
          foreach (XElement element1 in descendant.Elements())
          {
            if (element1.Name == (XName) "id")
              int.TryParse(element1.Value, out result1);
            if (element1.Name == (XName) "hash")
              Enum.TryParse<PedHash>(element1.Value, out result2);
            if (element1.Name == (XName) "posX")
              pos.X = LsFunctions.StringToFloat(element1.Value);
            if (element1.Name == (XName) "posY")
              pos.Y = LsFunctions.StringToFloat(element1.Value);
            if (element1.Name == (XName) "posZ")
              pos.Z = LsFunctions.StringToFloat(element1.Value);
            if (element1.Name == (XName) "weed")
              int.TryParse(element1.Value, out result3);
            if (element1.Name == (XName) "crack")
              int.TryParse(element1.Value, out result4);
            if (element1.Name == (XName) "cocaine")
              int.TryParse(element1.Value, out result5);
            if (element1.Name == (XName) "money")
              int.TryParse(element1.Value, out result6);
            if (element1.Name == (XName) "armour")
              int.TryParse(element1.Value, out result7);
            if (element1.Name == (XName) "weapon")
              _weapons.Add(element1);
            if (element1.Name == (XName) "pedData")
            {
              int result8 = 0;
              int result9 = 0;
              int result10 = 0;
              int result11 = 0;
              int result12 = 0;
              int result13 = 0;
              int result14 = 0;
              int result15 = 0;
              int result16 = 0;
              int result17 = 0;
              int result18 = 0;
              int result19 = 0;
              int result20 = 0;
              int result21 = 0;
              int result22 = 0;
              int result23 = 0;
              int result24 = 0;
              int result25 = 0;
              int result26 = 0;
              int result27 = 0;
              int result28 = 0;
              int result29 = 0;
              foreach (XElement element2 in element1.Elements())
              {
                if (element2.Name == (XName) "headD")
                  int.TryParse(element2.Value, out result8);
                if (element2.Name == (XName) "headT")
                  int.TryParse(element2.Value, out result9);
                if (element2.Name == (XName) "beardD")
                  int.TryParse(element2.Value, out result10);
                if (element2.Name == (XName) "beardT")
                  int.TryParse(element2.Value, out result11);
                if (element2.Name == (XName) "hairD")
                  int.TryParse(element2.Value, out result12);
                if (element2.Name == (XName) "hairT")
                  int.TryParse(element2.Value, out result13);
                if (element2.Name == (XName) "torsoD")
                  int.TryParse(element2.Value, out result14);
                if (element2.Name == (XName) "torsoT")
                  int.TryParse(element2.Value, out result15);
                if (element2.Name == (XName) "legsD")
                  int.TryParse(element2.Value, out result16);
                if (element2.Name == (XName) "legsT")
                  int.TryParse(element2.Value, out result17);
                if (element2.Name == (XName) "handsD")
                  int.TryParse(element2.Value, out result18);
                if (element2.Name == (XName) "handsT")
                  int.TryParse(element2.Value, out result19);
                if (element2.Name == (XName) "footD")
                  int.TryParse(element2.Value, out result20);
                if (element2.Name == (XName) "footT")
                  int.TryParse(element2.Value, out result21);
                if (element2.Name == (XName) "acces1D")
                  int.TryParse(element2.Value, out result22);
                if (element2.Name == (XName) "acces1T")
                  int.TryParse(element2.Value, out result23);
                if (element2.Name == (XName) "acces2D")
                  int.TryParse(element2.Value, out result24);
                if (element2.Name == (XName) "acces2T")
                  int.TryParse(element2.Value, out result25);
                if (element2.Name == (XName) "decalD")
                  int.TryParse(element2.Value, out result26);
                if (element2.Name == (XName) "decalT")
                  int.TryParse(element2.Value, out result27);
                if (element2.Name == (XName) "torsoAuxD")
                  int.TryParse(element2.Value, out result28);
                if (element2.Name == (XName) "torsoAuxT")
                  int.TryParse(element2.Value, out result29);
              }
              _pData.Add(0, new Tuple<int, int>(result8, result9));
              _pData.Add(1, new Tuple<int, int>(result10, result11));
              _pData.Add(2, new Tuple<int, int>(result12, result13));
              _pData.Add(3, new Tuple<int, int>(result14, result15));
              _pData.Add(4, new Tuple<int, int>(result16, result17));
              _pData.Add(5, new Tuple<int, int>(result18, result19));
              _pData.Add(6, new Tuple<int, int>(result20, result21));
              _pData.Add(8, new Tuple<int, int>(result22, result23));
              _pData.Add(9, new Tuple<int, int>(result24, result25));
              _pData.Add(10, new Tuple<int, int>(result26, result27));
              _pData.Add(11, new Tuple<int, int>(result28, result29));
            }
          }
          if (pos != Vector3.Zero)
          {
            this.allocatedIDs.Add(result1);
            if (LSL.DEBUG)
              UI.Notify("Added " + result1.ToString() + " to allocated ID's, Size of pool =" + this.allocatedIDs.Count.ToString());
            if (this.dealers.FindAll((Predicate<PlayerDealer>) (d => d.area == World.GetZoneNameLabel(pos))).Count == 0)
              this.AddDealer(new PlayerDealer(result1, result2, Enums.PdStates.Work, pos, result3, result4, result5, result6, result7, _weapons, _pData));
            else
              this.AddDealer(new PlayerDealer(result1, result2, Enums.PdStates.FollowPlayer, pos, result3, result4, result5, result6, result7, _weapons, _pData));
            if (LSL.DEBUG)
              UI.Notify("Loaded dealer" + result1.ToString());
          }
        }
      }
    }

    private void SaveDealers()
    {
      XDocument _doc = XDocument.Load("scripts\\LSLife\\LSLife_Dealers.xml");
      foreach (PlayerDealer dealer in this.dealers)
        this.SaveDealer(dealer, _doc);
      _doc.Save("scripts\\LSLife\\LSLife_Dealers.xml");
    }

    public Vector3 GetCloseHealPosition(Vector3 _pedPos)
    {
      Vector3 vector3_1 = Vector3.Zero;
      if (LSL.HouseHandler.Houses.Count > 0)
      {
        List<StashHouse> list = LSL.HouseHandler.Houses.Where<StashHouse>((Func<StashHouse, bool>) (h => (double) h.entrancePos.DistanceTo(_pedPos) < 40.0)).ToList<StashHouse>();
        if (list != null && list.Count > 0)
        {
          StashHouse stashHouse1 = list[0];
          foreach (StashHouse stashHouse2 in list)
          {
            Vector3 entrancePos = stashHouse1.entrancePos;
            double num1 = (double) entrancePos.DistanceTo(_pedPos);
            entrancePos = stashHouse2.entrancePos;
            double num2 = (double) entrancePos.DistanceTo(_pedPos);
            if (num1 < num2)
              stashHouse1 = stashHouse2;
          }
          vector3_1 = stashHouse1.entrancePos;
        }
      }
      Vector3 vector3_2 = Vector3.Zero;
      if (LSL.DealerHandler.HealLocations.Count > 0)
      {
        List<Vector3> list = LSL.DealerHandler.HealLocations.Where<Vector3>((Func<Vector3, bool>) (h => (double) h.DistanceTo(LSL.playerPos) < 50.0)).ToList<Vector3>();
        if (list != null && list.Count > 0)
        {
          Vector3 vector3_3 = list[0];
          foreach (Vector3 vector3_4 in list)
          {
            if ((double) vector3_4.DistanceTo(LSL.playerPos) < (double) vector3_3.DistanceTo(LSL.playerPos))
              vector3_3 = vector3_4;
          }
          vector3_2 = vector3_3;
        }
      }
      if (vector3_1 != Vector3.Zero && vector3_2 != Vector3.Zero)
      {
        if ((double) vector3_1.DistanceTo(LSL.playerPos) < (double) vector3_2.DistanceTo(LSL.playerPos))
        {
          this.HealPositionType = Enums.HealPositionTypes.House;
          return vector3_1;
        }
        this.HealPositionType = Enums.HealPositionTypes.Hospital;
        return vector3_2;
      }
      if (vector3_1 != Vector3.Zero)
      {
        this.HealPositionType = Enums.HealPositionTypes.House;
        return vector3_1;
      }
      if (vector3_2 != Vector3.Zero)
      {
        this.HealPositionType = Enums.HealPositionTypes.Hospital;
        return vector3_2;
      }
      this.HealPositionType = Enums.HealPositionTypes.None;
      return Vector3.Zero;
    }

    public int GetNewID()
    {
      int id = 0;
      if (this.allocatedIDs.FindAll((Predicate<int>) (i => i == id)).Count != 0)
      {
        id++;
        while (this.allocatedIDs.FindAll((Predicate<int>) (i => i == id)).Count != 0)
          id++;
      }
      if (LSL.DEBUG)
        UI.Notify("New ID " + id.ToString());
      this.allocatedIDs.Add(id);
      return id;
    }

    public void SaveDealer(PlayerDealer _dealer, XDocument _doc)
    {
      foreach (XElement descendant in _doc.Descendants())
      {
        if (descendant.Name == (XName) "id" && descendant.Value == _dealer.ID.ToString())
        {
          bool flag1 = false;
          bool flag2 = false;
          foreach (XElement element in descendant.Parent.Elements())
          {
            if (element.Name == (XName) "posX")
              element.Value = _dealer.Pos.X.ToString();
            if (element.Name == (XName) "posY")
              element.Value = _dealer.Pos.Y.ToString();
            if (element.Name == (XName) "posZ")
              element.Value = _dealer.Pos.Z.ToString();
            if (element.Name == (XName) "weed")
              element.Value = _dealer.Drugs["Weed"].ToString();
            if (element.Name == (XName) "crack")
              element.Value = _dealer.Drugs["Crack"].ToString();
            if (element.Name == (XName) "cocaine")
              element.Value = _dealer.Drugs["Cocaine"].ToString();
            if (element.Name == (XName) "money")
              element.Value = _dealer.Money.ToString();
            if (element.Name == (XName) "armour")
            {
              flag1 = true;
              element.Value = _dealer.dArmour.ToString();
            }
            if (element.Name == (XName) "pedData")
              flag2 = true;
          }
          if (!flag1)
          {
            XElement xelement = new XElement((XName) "armour", (object) _dealer.dArmour);
            descendant.Parent.Add((object) xelement);
          }
          if (flag2 || !((Entity) _dealer.Ped != (Entity) null))
            break;
          XElement xelement1 = new XElement((XName) "pedData");
          _dealer.pData = LsFunctions.GetPedVariationData(_dealer.Ped);
          XElement xelement2 = new XElement((XName) "headD", (object) _dealer.pData[0].Item1.ToString());
          XName name1 = (XName) "headT";
          int num = _dealer.pData[0].Item2;
          string str1 = num.ToString();
          XElement xelement3 = new XElement(name1, (object) str1);
          xelement1.Add((object) xelement2);
          xelement1.Add((object) xelement3);
          XName name2 = (XName) "beardD";
          num = _dealer.pData[1].Item1;
          string str2 = num.ToString();
          XElement xelement4 = new XElement(name2, (object) str2);
          XName name3 = (XName) "beardT";
          num = _dealer.pData[1].Item2;
          string str3 = num.ToString();
          XElement xelement5 = new XElement(name3, (object) str3);
          xelement1.Add((object) xelement4);
          xelement1.Add((object) xelement5);
          XName name4 = (XName) "hairD";
          num = _dealer.pData[2].Item1;
          string str4 = num.ToString();
          XElement xelement6 = new XElement(name4, (object) str4);
          XName name5 = (XName) "hairT";
          num = _dealer.pData[2].Item2;
          string str5 = num.ToString();
          XElement xelement7 = new XElement(name5, (object) str5);
          xelement1.Add((object) xelement6);
          xelement1.Add((object) xelement7);
          XName name6 = (XName) "torsoD";
          num = _dealer.pData[3].Item1;
          string str6 = num.ToString();
          XElement xelement8 = new XElement(name6, (object) str6);
          XName name7 = (XName) "torsoT";
          num = _dealer.pData[3].Item2;
          string str7 = num.ToString();
          XElement xelement9 = new XElement(name7, (object) str7);
          xelement1.Add((object) xelement8);
          xelement1.Add((object) xelement9);
          XName name8 = (XName) "legsD";
          num = _dealer.pData[4].Item1;
          string str8 = num.ToString();
          XElement xelement10 = new XElement(name8, (object) str8);
          XName name9 = (XName) "legsT";
          num = _dealer.pData[4].Item2;
          string str9 = num.ToString();
          XElement xelement11 = new XElement(name9, (object) str9);
          xelement1.Add((object) xelement10);
          xelement1.Add((object) xelement11);
          XName name10 = (XName) "handsD";
          num = _dealer.pData[5].Item1;
          string str10 = num.ToString();
          XElement xelement12 = new XElement(name10, (object) str10);
          XName name11 = (XName) "handsT";
          num = _dealer.pData[5].Item2;
          string str11 = num.ToString();
          XElement xelement13 = new XElement(name11, (object) str11);
          xelement1.Add((object) xelement12);
          xelement1.Add((object) xelement13);
          XName name12 = (XName) "footD";
          num = _dealer.pData[6].Item1;
          string str12 = num.ToString();
          XElement xelement14 = new XElement(name12, (object) str12);
          XName name13 = (XName) "footT";
          num = _dealer.pData[6].Item2;
          string str13 = num.ToString();
          XElement xelement15 = new XElement(name13, (object) str13);
          xelement1.Add((object) xelement14);
          xelement1.Add((object) xelement15);
          XName name14 = (XName) "acces1D";
          num = _dealer.pData[8].Item1;
          string str14 = num.ToString();
          XElement xelement16 = new XElement(name14, (object) str14);
          XName name15 = (XName) "acces1T";
          num = _dealer.pData[8].Item2;
          string str15 = num.ToString();
          XElement xelement17 = new XElement(name15, (object) str15);
          xelement1.Add((object) xelement16);
          xelement1.Add((object) xelement17);
          XName name16 = (XName) "acces2D";
          num = _dealer.pData[9].Item1;
          string str16 = num.ToString();
          XElement xelement18 = new XElement(name16, (object) str16);
          XName name17 = (XName) "acces2T";
          num = _dealer.pData[9].Item2;
          string str17 = num.ToString();
          XElement xelement19 = new XElement(name17, (object) str17);
          xelement1.Add((object) xelement18);
          xelement1.Add((object) xelement19);
          XName name18 = (XName) "decalD";
          num = _dealer.pData[10].Item1;
          string str18 = num.ToString();
          XElement xelement20 = new XElement(name18, (object) str18);
          XName name19 = (XName) "decalT";
          num = _dealer.pData[10].Item2;
          string str19 = num.ToString();
          XElement xelement21 = new XElement(name19, (object) str19);
          xelement1.Add((object) xelement20);
          xelement1.Add((object) xelement21);
          XName name20 = (XName) "torsoAuxD";
          num = _dealer.pData[11].Item1;
          string str20 = num.ToString();
          XElement xelement22 = new XElement(name20, (object) str20);
          XName name21 = (XName) "torsoAuxT";
          num = _dealer.pData[11].Item2;
          string str21 = num.ToString();
          XElement xelement23 = new XElement(name21, (object) str21);
          xelement1.Add((object) xelement22);
          xelement1.Add((object) xelement23);
          descendant.Parent.Add((object) xelement1);
          break;
        }
      }
    }

    private void DeleteDealer(int _id)
    {
      XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Dealers.xml");
      if (xdocument.Descendants().ToList<XElement>().Count <= 0 || this.dealers.Count <= 0)
        return;
      foreach (PlayerDealer playerDealer in this.dealers.ToList<PlayerDealer>())
      {
        if (playerDealer.ID == _id)
        {
          this.dealers.Remove(playerDealer);
          XElement xelement = (XElement) null;
          foreach (XElement descendant in xdocument.Descendants())
          {
            if (descendant.Name == (XName) "dealer")
            {
              foreach (XElement element in descendant.Elements())
              {
                if (element.Value == _id.ToString())
                {
                  xelement = element.Parent;
                  break;
                }
              }
            }
          }
          if (xelement != null)
          {
            xelement.Remove();
            break;
          }
          break;
        }
      }
      xdocument.Save("scripts\\LSLife\\LSLife_Dealers.xml");
    }

    public void AddDealer(PlayerDealer dealer) => this.dealers.Add(dealer);

    public bool CreateTeam(PlayerDealer _dealer)
    {
      if (this.teams.Count > 0)
      {
        if (this.teams.Where<PedTeam>((Func<PedTeam, bool>) (t => t.Area == LSL.playerArea)).Count<PedTeam>() == 0)
        {
          if (LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation) >= 1.0)
          {
            this.teams.Add(new PedTeam(_dealer, LSL.playerArea));
            return true;
          }
          LsFunctions.SendTextMsg("You need to earn more reputation in this area before putting a team on patrol here.");
        }
        else
          LsFunctions.SendTextMsg("There is a team in this area already");
      }
      return false;
    }
  }
}
