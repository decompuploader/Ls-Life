using GTA;
using GTA.Math;
using GTA.Native;
using iFruitAddon2;
using LSlife.OldCustomers;
using Microsoft.CSharp.RuntimeBinder;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LSlife
{
  public class LSL : Script
  {
    public static bool hardMode = false;
    public static bool DEBUG = true;
    public static bool UseSprites = true;
    public static bool DrawMarkers = true;
    public static bool started = false;
    public static bool EnableBlips;
    public static bool AreaDebug;
    public static bool LCRWMAP = false;
    public static ScriptSettings iniSettings;
    public static HirePed PottentialWorker = (HirePed) null;
    public static MenuPool LsMenuPool;
    public static PlayerDealerHandler DealerHandler;
    public static StashHouseHandler HouseHandler;
    public static PickupHandler pickupHandler = new PickupHandler();
    public static DriveByHandler driveByHandler = new DriveByHandler();
    public static LsLPeds lPeds;
    public static int CustomerRel = World.AddRelationshipGroup("LSCUSTOMER");
    public static int rival_nutral = World.AddRelationshipGroup("RIVAL_N");
    public static int rival_enemy = World.AddRelationshipGroup("RIVAL_E");
    public static int playerRelationship = Function.Call<int>(Hash._0xD24D37CC275948CC, (InputArgument) "PLAYER");
    public PedDebugger PedDebugger = new PedDebugger();
    public static UIMenu hireDealerMainMenu;
    public static UIMenuSliderItem hireDealerOfferAmount;
    public static UIMenuItem hireDealerMakeOffer;
    public static UIMenuItem hireDealerFollowButton;
    public static UIMenu WorkerMainMenu;
    public static UIMenuSliderItem wWeed;
    public static UIMenuSliderItem wCrack;
    public static UIMenuSliderItem wCocaine;
    public static UIMenuListItem wMultiplier;
    public static UIMenuItem wConfirm;
    public static UIMenuItem wCollect;
    public static UIMenuListItem wOrder;
    public static UIMenuItem wArmour;
    public static UIMenuListItem wWeapons;
    public static UIMenu zeeMenu;
    public static UIMenu startDealing;
    public static UIMenuCheckboxItem sellWeed;
    public static UIMenuCheckboxItem sellCrack;
    public static UIMenuCheckboxItem sellCocaine;
    public static UIMenuItem StartDeal;
    public static UIMenu Options;
    public static UIMenuSliderItem InventoryPosX;
    public static UIMenuSliderItem InventoryPosY;
    public static UIMenu OrderDrugs;
    private UIMenuListItem OrderMultipliers;
    public static UIMenu weedMenu;
    public static UIMenuSliderItem bushWeed;
    public static UIMenu crackMenu;
    public static UIMenuSliderItem crack;
    public static UIMenu cocainMenu;
    public static UIMenuSliderItem cocaine;
    public static UIMenuItem WeedBuy;
    public static UIMenuItem CrackBuy;
    public static UIMenuItem CocainBuy;
    public static UIMenuItem PlaceOrder;
    public static UIMenuItem buyWeapons;
    public static UIMenuItem buyLift;
    public static UIMenuItem payDebt;
    public static UIMenu wepMenu;
    public static UIMenuListItem melee;
    public static List<object> meleeWeps = new List<object>()
    {
      (object) "Bat",
      (object) "Knife",
      (object) "Dagger"
    };
    public static List<int> meleePrice = new List<int>()
    {
      5,
      100,
      2000
    };
    public static UIMenuListItem pistol;
    public static List<object> pistolWeps = new List<object>()
    {
      (object) "Pistol",
      (object) "Pistol50",
      (object) "HeavyPistol",
      (object) "Ammo"
    };
    public static List<int> pistolPrice = new List<int>()
    {
      350,
      3750,
      4000,
      50
    };
    public static UIMenuListItem shotgun;
    public static List<object> shotgunWeps = new List<object>()
    {
      (object) "SawnOffShotgun",
      (object) "PumpShotgun"
    };
    public static UIMenuListItem assaultRifle;
    public static List<object> assaultRifleWeps = new List<object>()
    {
      (object) "AssaultRifle",
      (object) "CarbineRifle"
    };
    public static UIMenuListItem smg;
    public static List<object> smgWeps = new List<object>()
    {
      (object) "MicroSMG",
      (object) "SMG"
    };
    public static UIMenuListItem mg;
    public static List<object> mgWeps = new List<object>()
    {
      (object) "MG"
    };
    public static UIMenuListItem sniper;
    public static List<object> sniperWeps = new List<object>()
    {
      (object) "SniperRifle"
    };
    public static UIMenuListItem heavy;
    public static List<object> heavyWeps = new List<object>()
    {
      (object) "RPG"
    };
    public static UIMenuListItem thrown;
    public static List<object> thrownWeps = new List<object>()
    {
      (object) "Grenade"
    };
    public static UIMenuItem armour;
    public static UIMenuItem dismiss;
    public static UIMenu stashMainMenu;
    public static UIMenu stashTakeMenu;
    public static UIMenu stashPutMenu;
    public static UIMenuSliderItem putWeedStash;
    public static UIMenuSliderItem takeWeedStash;
    public static UIMenuSliderItem putCrackStash;
    public static UIMenuSliderItem takeCrackStash;
    public static UIMenuSliderItem putCocainStash;
    public static UIMenuSliderItem takeCocainStash;
    public static UIMenuSliderItem putWeedOzStash;
    public static UIMenuSliderItem takeWeedOzStash;
    public static UIMenuSliderItem putCrackOzStash;
    public static UIMenuSliderItem takeCrackOzStash;
    public static UIMenuSliderItem putCocainOzStash;
    public static UIMenuSliderItem takeCocainOzStash;
    public static UIMenuSliderItem putMoneyStash;
    public static UIMenuSliderItem takeMoneyStash;
    public static UIMenuListItem putArmourStash;
    public static UIMenuListItem takeArmourStash;
    public static UIMenuListItem putWeaponStash;
    public static UIMenuListItem takeWeaponStash;
    public static UIMenuListItem stashPutMultiplier;
    public static UIMenuListItem stashTakeMultiplier;
    public static UIMenu vehStashMainMenu;
    public static UIMenu vehStashTakeMenu;
    public static UIMenu vehStashPutMenu;
    public static UIMenuSliderItem putWeedVeh;
    public static UIMenuSliderItem takeWeedVeh;
    public static UIMenuSliderItem putCrackVeh;
    public static UIMenuSliderItem takeCrackVeh;
    public static UIMenuSliderItem putCocainVeh;
    public static UIMenuSliderItem takeCocainVeh;
    public static UIMenuSliderItem putWeedOzVeh;
    public static UIMenuSliderItem takeWeedOzVeh;
    public static UIMenuSliderItem putCrackOzVeh;
    public static UIMenuSliderItem takeCrackOzVeh;
    public static UIMenuSliderItem putCocainOzVeh;
    public static UIMenuSliderItem takeCocainOzVeh;
    public static UIMenuSliderItem putMoneyVeh;
    public static UIMenuSliderItem takeMoneyVeh;
    public static UIMenuListItem putArmourVeh;
    public static UIMenuListItem takeArmourVeh;
    public static UIMenuListItem putWeaponVeh;
    public static UIMenuListItem takeWeaponVeh;
    public static UIMenuListItem vehPutMultiplier;
    public static UIMenuListItem vehTakeMultiplier;
    public static readonly List<object> multiplier = new List<object>()
    {
      (object) 1,
      (object) 10,
      (object) 28,
      (object) 100,
      (object) 280,
      (object) 1000,
      (object) 2800,
      (object) 10000,
      (object) 100000
    };
    public static UIMenu customerMainMenu;
    public static UIMenuItem sell;
    public static UIMenuItem turnAway;
    public static Keys MenuKey;
    public static Keys StopDealing;
    public static Keys SetDrugCar;
    public static XDocument vehWeaponsXML;
    public static XDocument stashWeaponsXML;
    public static XDocument LSLifeSave;
    public static XDocument VehicleXml;
    public static float displayHelpTimer = (float) Game.GameTime;
    public static int day;
    public static int hour;
    public static int SlowTickTime;
    public static CustomiFruit iFruit;
    public static iFruitContact contactZee;
    public static DrugDealer ZeesDealers = (DrugDealer) null;
    public static DrugOrder NewOrder = new DrugOrder();
    public static Ped drugDealer1;
    public static Ped weaponDealer1;
    public static int dealerTimer;
    public static bool readyToDeal;
    public static bool dealer1Angry;
    public static int weed1Price = 500;
    public static int crack1Price = 2500;
    public static int cocain1Price = 5000;
    public static int dealer1Weed;
    public static int dealer1Crack;
    public static int dealer1Cocain;
    public static int dealer1Money;
    public static int dealer1ReloadDay;
    public static bool dealer1ReloadOrder = false;
    public static int dealer1ReloadHour = 20;
    public static bool dealer1Reloaded = false;
    public static int dealer1Debt;
    public static double dealer1Rep;
    public static int reloadTimer;
    public static int reloadTimerInterval = 600000;
    public static List<Pig> pigs = new List<Pig>();
    public static AreaDealer aDealer = (AreaDealer) null;
    public static StashVehicle pStashVehicle = (StashVehicle) null;
    public static Vector3 playerPos = Vector3.Zero;
    public static Player player = Game.Player;
    public static bool removedStuff = true;
    public static bool isDealing = false;
    public static bool chilling = false;
    public static int areaCheckTimer;
    public static Dictionary<string, Tuple<LSL.AreaType, LSL.jurisdictionType>> zoneData = new Dictionary<string, Tuple<LSL.AreaType, LSL.jurisdictionType>>();
    public static List<Area> areas = new List<Area>();
    public static int areaIndex = 0;
    public static string playerArea = "SanAnd";
    public static System.Random rnd = new System.Random();
    public static Dictionary<string, int> PlayerInventory = new Dictionary<string, int>();
    public static int playerArmours = 0;
    public static bool playerWanted = false;
    public static bool notFined = true;
    public static bool playerDead = false;
    public static List<object> playerWeps = new List<object>();
    public static int newWepAmount = 0;
    public static List<XElement> playerWeapons = new List<XElement>();
    public static bool playerHasBag = false;
    public static bool canSprint = true;
    public static int randomAttackTimer;
    public static List<Entity> entitiesCleanUp = new List<Entity>();
    public static bool sellingWeed = false;
    public static bool sellingCrack = false;
    public static bool sellingCocaine = false;
    public static PedGroup playerGroup;
    public static int timeStartedDealing;
    public static int timeDealing = 0;
    public static Vector3 pedTarget;
    public static Ped attacker;
    public static bool attackerAttacking = false;
    public static bool attackerPickupSpawned = false;
    public static Pickup attackerPickup;
    public static int attackCheckTime = 0;
    public static List<XElement> attackerWeapons = new List<XElement>();
    public int attackerWeed;
    public int attackerCrack;
    public int attackerCocaine;
    public int attackerMoney;
    public static List<Ped> angryPeds = new List<Ped>();
    public static List<Customer> customers = new List<Customer>();
    public static List<Customer> jobs = new List<Customer>();
    public static string thisStreet;
    public static string lastStreet;
    public static float lastTimePedSelected = (float) Game.GameTime;
    public static float lastTimeDealDone = (float) Game.GameTime;
    public static float lastAngryTime = (float) Game.GameTime;
    public static float ratTimer = 0.0f;
    public static int ratChance;
    public static int rat;
    public static int lastCustomerCheckTimer;
    public static Customer customerSellTo;
    public static int jobOfferTimer;
    public static int jobAcceptTimer;
    public static bool jobOffer = false;
    public static bool jobAccepted = false;
    public static int oneSecDelay;
    public static bool firstRun = true;
    public static Dealer wepDealer;
    public static UIText myUIText;
    public static int PedsCanSeeDeal = 0;
    private bool showInventory;
    private bool pSaved;
    private uint lsDebug;
    private uint areaDebug;
    private uint spawnP;

    public static int TotalDrugsCarried() => LsFunctions.TotalDrugs(LSL.PlayerInventory);

    public static bool PlayerComplied { get; private set; }

    public LSL()
    {
      this.Setup();
      this.Aborted += new EventHandler(this.OnAbort);
      this.Tick += new EventHandler(this.onTick);
      this.KeyDown += new KeyEventHandler(this.onKeyDown);
      this.Interval = 3;
    }

    private void Setup()
    {
      this.lsDebug = Function.Call<uint>(Hash._0xD24D37CC275948CC, (InputArgument) "lsdebug");
      this.areaDebug = Function.Call<uint>(Hash._0xD24D37CC275948CC, (InputArgument) "areadebug");
      this.spawnP = Function.Call<uint>(Hash._0xD24D37CC275948CC, (InputArgument) "spawnP");
      LSL.player = Game.Player;
      LSL.player.Character.CurrentPedGroup.SeparationRange = 100f;
      LSL.iniSettings = ScriptSettings.Load("scripts\\LSlife.ini");
      LSL.DEBUG = LSL.iniSettings.GetValue<bool>("OPTIONS", "DEBUG", false);
      LSL.UseSprites = LSL.iniSettings.GetValue<bool>("OPTIONS", "SPRITES", true);
      LSL.hardMode = LSL.iniSettings.GetValue<bool>("OPTIONS", "HARD", false);
      LSL.DrawMarkers = LSL.iniSettings.GetValue<bool>("OPTIONS", "MARKERS", true);
      LSL.AreaDebug = LSL.iniSettings.GetValue<bool>("OPTIONS", "AREADEBUG", false);
      LSL.LCRWMAP = LSL.iniSettings.GetValue<bool>("OPTIONS", "LCRWMAP", false);
      if (LSL.hardMode)
      {
        LSL.weed1Price = 180;
        LSL.crack1Price = 400;
        LSL.cocain1Price = 1300;
      }
      LsFunctions.LoadZoneData();
      LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "playerInventory")
        {
          bool flag1 = false;
          bool flag2 = false;
          foreach (XElement element in descendant.Elements())
          {
            int result;
            if (element.Name == (XName) "weed")
            {
              int.TryParse(element.Value, out result);
              LSL.PlayerInventory.Add("Weed", result);
            }
            if (element.Name == (XName) "weedOz")
            {
              int.TryParse(element.Value, out LsFunctions.pWeedOunce);
              flag2 = true;
            }
            if (element.Name == (XName) "crack")
            {
              int.TryParse(element.Value, out result);
              LSL.PlayerInventory.Add("Crack", result);
            }
            if (element.Name == (XName) "crackOz")
            {
              int.TryParse(element.Value, out LsFunctions.pCrackOunce);
              flag2 = true;
            }
            if (element.Name == (XName) "cocain")
            {
              int.TryParse(element.Value, out result);
              LSL.PlayerInventory.Add("Cocaine", result);
            }
            if (element.Name == (XName) "cocaineOz")
            {
              int.TryParse(element.Value, out LsFunctions.pCocaineOunce);
              flag2 = true;
            }
            if (element.Name == (XName) "armour")
            {
              int.TryParse(element.Value, out LSL.playerArmours);
              flag1 = true;
            }
          }
          if (!flag1)
          {
            descendant.Add((object) new XElement((XName) "armour", (object) LSL.playerArmours.ToString()));
            LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
          }
          if (!flag2)
          {
            descendant.Add((object) new XElement((XName) "weedOz", (object) LsFunctions.pWeedOunce.ToString()));
            descendant.Add((object) new XElement((XName) "crackOz", (object) LsFunctions.pCrackOunce.ToString()));
            descendant.Add((object) new XElement((XName) "cocaineOz", (object) LsFunctions.pCocaineOunce.ToString()));
            LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
          }
        }
        if (descendant.Name == (XName) "dealer1")
        {
          bool flag = false;
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "rep")
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            descendant.Add((object) new XElement((XName) "rep")
            {
              Value = "1"
            });
            LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
          }
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "debt")
            {
              int.TryParse(element.Value, out LSL.dealer1Debt);
              if (LSL.dealer1Debt > 200000)
              {
                LSL.dealer1Debt = 200000;
              }
              else
              {
                LSL.dealer1Angry = false;
                if (LSL.dealer1Debt < 0)
                  LSL.dealer1Debt = 0;
              }
            }
            if (element.Name == (XName) "weed")
              int.TryParse(element.Value, out LSL.dealer1Weed);
            if (element.Name == (XName) "crack")
              int.TryParse(element.Value, out LSL.dealer1Crack);
            if (element.Name == (XName) "cocain")
              int.TryParse(element.Value, out LSL.dealer1Cocain);
            if (element.Name == (XName) "money")
            {
              int result;
              int.TryParse(element.Value, out result);
              LSL.dealer1Money = result <= 10000000 ? result : 10000000;
            }
            if (element.Name == (XName) "reloaded")
              bool.TryParse(element.Value, out LSL.dealer1Reloaded);
            if (element.Name == (XName) "rep")
              double.TryParse(element.Value, out LSL.dealer1Rep);
          }
        }
      }
      LSL.VehicleXml = XDocument.Load("scripts\\LSLife\\LSLife_Vehicles.xml");
      Keys result1;
      Enum.TryParse<Keys>(LSL.iniSettings.GetValue("KEYS", "MAIN", "F10"), out result1);
      LSL.MenuKey = result1;
      Keys result2;
      Enum.TryParse<Keys>(LSL.iniSettings.GetValue("KEYS", "STOPDEALING", "F11"), out result2);
      LSL.StopDealing = result2;
      Keys result3;
      Enum.TryParse<Keys>(LSL.iniSettings.GetValue("KEYS", "SETDRUGCAR", "F12"), out result3);
      LSL.SetDrugCar = result3;
      LSL.EnableBlips = true;
      LSL.stashWeaponsXML = XDocument.Load("scripts\\LSLife\\LsLife_Weapons.xml");
      foreach (XElement descendant in LSL.stashWeaponsXML.Descendants())
      {
        if (descendant.Name == (XName) "weapon")
        {
          foreach (XElement element in descendant.Elements())
          {
            int num = element.Name == (XName) "HASH" ? 1 : 0;
          }
        }
      }
      LSL.vehWeaponsXML = XDocument.Load("scripts\\LSLife\\LsLife_Vehicle_Weapons.xml");
      int num1 = 0;
      foreach (WeaponHash weaponHash in Enum.GetValues(typeof (WeaponHash)))
      {
        if (Function.Call<bool>(Hash._0x8DECB02F88F428BC, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash))
        {
          int num2 = Function.Call<int>(Hash._0x015A522136D7F951, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash);
          XElement xelement1 = new XElement((XName) "weapon");
          xelement1.SetAttributeValue((XName) "id", (object) num1);
          XElement xelement2 = new XElement((XName) "HASH", (object) weaponHash);
          int num3 = LSL.DEBUG ? 1 : 0;
          XElement xelement3 = new XElement((XName) "AMMO", (object) num2);
          xelement1.Add((object) xelement2);
          xelement1.Add((object) xelement3);
          foreach (WeaponComponent weaponComponent in Enum.GetValues(typeof (WeaponComponent)))
          {
            if (Function.Call<bool>(Hash._0xC593212475FAE340, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash, (InputArgument) (int) weaponComponent))
            {
              XElement xelement4 = new XElement((XName) "COMPONENT", (object) weaponComponent);
              xelement1.Add((object) xelement4);
            }
          }
          LSL.playerWeapons.Add(xelement1);
          LSL.playerWeps.Add((object) weaponHash.ToString());
          ++num1;
        }
      }
      LSL.LsMenuPool = new MenuPool();
      Color.FromArgb(114, 204, 114);
      LSL.zeeMenu = new UIMenu("Zee", "What you want?");
      if (LSL.UseSprites)
        LSL.zeeMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_zee.png");
      LSL.LsMenuPool.Add(LSL.zeeMenu);
      LSL.startDealing = LSL.LsMenuPool.AddSubMenu(LSL.zeeMenu, "Dealing.");
      if (LSL.UseSprites)
        LSL.startDealing.SetBannerType("scripts\\LSLife\\sprites\\banner_zee.png");
      LSL.sellWeed = new UIMenuCheckboxItem("Sell Weed", true);
      LSL.sellCrack = new UIMenuCheckboxItem("Sell Crack", true);
      LSL.sellCocaine = new UIMenuCheckboxItem("Sell Cocaine", true);
      LSL.StartDeal = new UIMenuItem("Start Dealing.");
      LSL.startDealing.AddItem((UIMenuItem) LSL.sellWeed);
      LSL.startDealing.AddItem((UIMenuItem) LSL.sellCrack);
      LSL.startDealing.AddItem((UIMenuItem) LSL.sellCocaine);
      LSL.startDealing.AddItem(LSL.StartDeal);
      LSL.startDealing.OnItemSelect += new ItemSelectEvent(LsMenus.OnstartDealSelect);
      LSL.OrderDrugs = LSL.LsMenuPool.AddSubMenu(LSL.zeeMenu, "Order Drugs.");
      if (LSL.UseSprites)
        LSL.OrderDrugs.SetBannerType("scripts\\LSLife\\sprites\\banner_zee.png");
      LSL.OrderDrugs.BannerSprite.Color = Color.Black;
      LSL.OrderDrugs.Title.DropShadow = true;
      LSL.OrderDrugs.Title.Outline = true;
      LSL.OrderDrugs.Title.Shadow = true;
      LSL.OrderDrugs.OnMenuOpen += new MenuOpenEvent(this.OrderDrugs_OnMenuOpen);
      LSL.buyWeapons = new UIMenuItem("Buy Weapons.");
      LSL.zeeMenu.AddItem(LSL.buyWeapons);
      LSL.buyLift = new UIMenuItem("Ask for a Lift.");
      LSL.zeeMenu.AddItem(LSL.buyLift);
      LSL.payDebt = new UIMenuItem("Pay Debt");
      LSL.zeeMenu.AddItem(LSL.payDebt);
      LSL.zeeMenu.OnItemSelect += new ItemSelectEvent(LsMenus.OnZeeMenuSelect);
      LSL.hireDealerMainMenu = new UIMenu("Hire Dealer", "Select an option");
      if (LSL.UseSprites)
        LSL.hireDealerMainMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_dealer.png");
      LSL.LsMenuPool.Add(LSL.hireDealerMainMenu);
      LSL.hireDealerOfferAmount = new UIMenuSliderItem("Offer Amount");
      LSL.hireDealerOfferAmount.Maximum = LSL.player.Money;
      LSL.hireDealerOfferAmount.Multiplier = 1000;
      LSL.hireDealerMakeOffer = new UIMenuItem("Make Offer");
      LSL.hireDealerFollowButton = new UIMenuItem("Cancel Offer");
      LSL.hireDealerMainMenu.AddItem((UIMenuItem) LSL.hireDealerOfferAmount);
      LSL.hireDealerMainMenu.AddItem(LSL.hireDealerMakeOffer);
      LSL.hireDealerMainMenu.AddItem(LSL.hireDealerFollowButton);
      LSL.hireDealerOfferAmount.OnSliderChanged += new ItemSliderEvent(LsMenus.HireDealerOfferAmount_OnSliderChanged);
      LSL.hireDealerMakeOffer.Activated += new ItemActivatedEvent(LsMenus.HireDealerOfferSelect);
      LSL.hireDealerFollowButton.Activated += new ItemActivatedEvent(LsMenus.HireDealerFollowSelect);
      LSL.WorkerMainMenu = new UIMenu("Worker", "Give stuff to your Dealer");
      if (LSL.UseSprites)
        LSL.WorkerMainMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_worker.png");
      LSL.LsMenuPool.Add(LSL.WorkerMainMenu);
      LSL.wWeed = new UIMenuSliderItem("Weed " + LsFunctions.GramsToOz(0));
      LSL.wWeed.Maximum = LSL.PlayerInventory["Weed"];
      LSL.wCrack = new UIMenuSliderItem("Crack " + LsFunctions.GramsToOz(0));
      LSL.wCrack.Maximum = LSL.PlayerInventory["Crack"];
      LSL.wCocaine = new UIMenuSliderItem("Coke " + LsFunctions.GramsToOz(0));
      LSL.wCocaine.Maximum = LSL.PlayerInventory["Cocaine"];
      LSL.wMultiplier = new UIMenuListItem("Multiplier", LSL.multiplier, 0);
      LSL.wConfirm = new UIMenuItem("Confirm");
      LSL.wCollect = new UIMenuItem("Collect");
      LSL.wOrder = new UIMenuListItem("Orders", new List<object>()
      {
        (object) "Nothing"
      }, 0);
      LSL.wArmour = new UIMenuItem("Give Armour");
      LSL.wWeapons = new UIMenuListItem("Give weapon", LSL.playerWeps, 0);
      LSL.WorkerMainMenu.AddItem((UIMenuItem) LSL.wWeed);
      LSL.WorkerMainMenu.AddItem((UIMenuItem) LSL.wCrack);
      LSL.WorkerMainMenu.AddItem((UIMenuItem) LSL.wCocaine);
      LSL.WorkerMainMenu.AddItem((UIMenuItem) LSL.wMultiplier);
      LSL.WorkerMainMenu.AddItem(LSL.wConfirm);
      LSL.WorkerMainMenu.AddItem(LSL.wCollect);
      LSL.WorkerMainMenu.AddItem((UIMenuItem) LSL.wOrder);
      LSL.WorkerMainMenu.AddItem(LSL.wArmour);
      LSL.WorkerMainMenu.AddItem((UIMenuItem) LSL.wWeapons);
      LSL.wWeed.OnSliderChanged += new ItemSliderEvent(LsMenus.WWeed_OnSliderChanged);
      LSL.wCrack.OnSliderChanged += new ItemSliderEvent(LsMenus.WCrack_OnSliderChanged);
      LSL.wCocaine.OnSliderChanged += new ItemSliderEvent(LsMenus.WCocaine_OnSliderChanged);
      LSL.wMultiplier.OnListChanged += new ItemListEvent(this.WMultiplier_OnListChanged);
      LSL.wConfirm.Activated += new ItemActivatedEvent(LsMenus.WConfirm_Activated);
      LSL.wCollect.Activated += new ItemActivatedEvent(LsMenus.WCollect_Activated);
      LSL.wOrder.Activated += new ItemActivatedEvent(LsMenus.WOrder_Activated);
      LSL.wArmour.Activated += new ItemActivatedEvent(LsMenus.WArmour_Activated);
      LSL.wWeapons.Activated += new ItemActivatedEvent(LsMenus.WWeapons_Activated);
      int _money1 = LSL.meleePrice[0] - LsFunctions.Discount(LSL.meleePrice[0]);
      int _money2 = LSL.pistolPrice[0] - LsFunctions.Discount(LSL.pistolPrice[0]);
      LSL.wepMenu = new UIMenu("Wepons", "What you want?");
      if (LSL.UseSprites)
        LSL.wepMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_dealer.png");
      LSL.LsMenuPool.Add(LSL.wepMenu);
      LSL.melee = new UIMenuListItem("Melee $" + LsFunctions.IntToMoney(_money1), LSL.meleeWeps, 0);
      LSL.wepMenu.AddItem((UIMenuItem) LSL.melee);
      LSL.pistol = new UIMenuListItem("Pistol $" + LsFunctions.IntToMoney(_money2), LSL.pistolWeps, 0);
      LSL.wepMenu.AddItem((UIMenuItem) LSL.pistol);
      LSL.armour = new UIMenuItem("Armour $" + LsFunctions.IntToMoney(250 - LsFunctions.Discount(250)));
      LSL.wepMenu.AddItem(LSL.armour);
      LSL.dismiss = new UIMenuItem("Dismiss");
      LSL.wepMenu.AddItem(LSL.dismiss);
      LSL.wepMenu.OnItemSelect += new ItemSelectEvent(LsMenus.OnWepMenuSelect);
      LSL.melee.OnListChanged += new ItemListEvent(LsMenus.onListChange);
      LSL.pistol.OnListChanged += new ItemListEvent(LsMenus.onListChange);
      this.OrderMultipliers = new UIMenuListItem("Multiplier", LSL.multiplier, 0);
      this.OrderMultipliers.OnListChanged += new ItemListEvent(this.DMultiplier_OnListChanged);
      LSL.weedMenu = LSL.LsMenuPool.AddSubMenu(LSL.OrderDrugs, "Weed");
      if (LSL.UseSprites)
        LSL.weedMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_zee.png");
      LSL.bushWeed = new UIMenuSliderItem("Weed $" + LsFunctions.IntToMoney(LSL.weed1Price - LsFunctions.Discount(LSL.weed1Price)) + "oz");
      LSL.weedMenu.AddItem((UIMenuItem) LSL.bushWeed);
      LSL.bushWeed.Maximum = LSL.dealer1Weed;
      LSL.bushWeed.Multiplier = 1;
      LSL.weedMenu.AddItem((UIMenuItem) this.OrderMultipliers);
      LSL.WeedBuy = new UIMenuItem("Order: " + LSL.bushWeed.Value.ToString());
      LSL.weedMenu.AddItem(LSL.WeedBuy);
      LSL.bushWeed.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.weedMenu.OnItemSelect += new ItemSelectEvent(LsMenus.onWeedMenuSelect);
      LSL.crackMenu = LSL.LsMenuPool.AddSubMenu(LSL.OrderDrugs, "Crack");
      if (LSL.UseSprites)
        LSL.crackMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_zee.png");
      LSL.crack = new UIMenuSliderItem("Crack $" + LsFunctions.IntToMoney(LSL.crack1Price - LsFunctions.Discount(LSL.crack1Price)) + "oz");
      LSL.crackMenu.AddItem((UIMenuItem) LSL.crack);
      LSL.crack.Maximum = LSL.dealer1Crack;
      LSL.crack.Multiplier = 1;
      LSL.crackMenu.AddItem((UIMenuItem) this.OrderMultipliers);
      LSL.CrackBuy = new UIMenuItem("Buy: " + LSL.crack.Value.ToString());
      LSL.crackMenu.AddItem(LSL.CrackBuy);
      LSL.crack.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.crackMenu.OnItemSelect += new ItemSelectEvent(LsMenus.onCrackMenuSelect);
      LSL.cocainMenu = LSL.LsMenuPool.AddSubMenu(LSL.OrderDrugs, "Cocaine");
      if (LSL.UseSprites)
        LSL.cocainMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_zee.png");
      LSL.cocaine = new UIMenuSliderItem("Cocaine $" + LsFunctions.IntToMoney(LSL.cocain1Price - LsFunctions.Discount(LSL.cocain1Price)) + "oz");
      LSL.cocainMenu.AddItem((UIMenuItem) LSL.cocaine);
      LSL.cocaine.Maximum = LSL.dealer1Cocain;
      LSL.cocaine.Multiplier = 1;
      LSL.cocainMenu.AddItem((UIMenuItem) this.OrderMultipliers);
      LSL.CocainBuy = new UIMenuItem("Buy: " + LSL.cocaine.Value.ToString());
      LSL.cocainMenu.AddItem(LSL.CocainBuy);
      LSL.cocaine.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.cocainMenu.OnItemSelect += new ItemSelectEvent(LsMenus.onCocainMenuSelect);
      LSL.PlaceOrder = new UIMenuItem("Place Order $" + LsFunctions.IntToMoney(0));
      LSL.OrderDrugs.AddItem(LSL.PlaceOrder);
      LSL.OrderDrugs.OnItemSelect += new ItemSelectEvent(LsMenus.OnOrderDrugsMenuSelect);
      LSL.stashMainMenu = new UIMenu("Safehouse Stash", "Select an option.");
      if (LSL.UseSprites)
        LSL.stashMainMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_stash.png");
      LSL.LsMenuPool.Add(LSL.stashMainMenu);
      LSL.stashTakeMenu = LSL.LsMenuPool.AddSubMenu(LSL.stashMainMenu, "Take");
      if (LSL.UseSprites)
        LSL.stashTakeMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_stash.png");
      LSL.stashPutMenu = LSL.LsMenuPool.AddSubMenu(LSL.stashMainMenu, "Stash");
      if (LSL.UseSprites)
        LSL.stashPutMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_stash.png");
      LSL.putWeedStash = new UIMenuSliderItem("Weed");
      LSL.stashPutMenu.AddItem((UIMenuItem) LSL.putWeedStash);
      LSL.putWeedStash.Maximum = LSL.PlayerInventory["Weed"];
      LSL.putWeedStash.Multiplier = 1;
      LSL.takeWeedStash = new UIMenuSliderItem("Weed");
      LSL.stashTakeMenu.AddItem((UIMenuItem) LSL.takeWeedStash);
      LSL.takeWeedStash.Maximum = 0;
      LSL.takeWeedStash.Multiplier = 1;
      LSL.putWeedStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeWeedStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putCrackStash = new UIMenuSliderItem("Crack");
      LSL.stashPutMenu.AddItem((UIMenuItem) LSL.putCrackStash);
      LSL.putCrackStash.Maximum = LSL.PlayerInventory["Crack"];
      LSL.putCrackStash.Multiplier = 1;
      LSL.takeCrackStash = new UIMenuSliderItem("Crack");
      LSL.stashTakeMenu.AddItem((UIMenuItem) LSL.takeCrackStash);
      LSL.takeCrackStash.Maximum = 0;
      LSL.takeCrackStash.Multiplier = 1;
      LSL.putCrackStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeCrackStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putCocainStash = new UIMenuSliderItem("Cocaine");
      LSL.stashPutMenu.AddItem((UIMenuItem) LSL.putCocainStash);
      LSL.putCocainStash.Maximum = LSL.PlayerInventory["Cocaine"];
      LSL.putCocainStash.Multiplier = 1;
      LSL.takeCocainStash = new UIMenuSliderItem("Cocaine");
      LSL.stashTakeMenu.AddItem((UIMenuItem) LSL.takeCocainStash);
      LSL.takeCocainStash.Maximum = 0;
      LSL.takeCocainStash.Multiplier = 1;
      LSL.putCocainStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeCocainStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putWeedOzStash = new UIMenuSliderItem("Weed Z's");
      LSL.stashPutMenu.AddItem((UIMenuItem) LSL.putWeedOzStash);
      LSL.putWeedOzStash.Maximum = LsFunctions.pWeedOunce;
      LSL.putWeedOzStash.Multiplier = 1;
      LSL.putWeedOzStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putCrackOzStash = new UIMenuSliderItem("Crack Z's");
      LSL.stashPutMenu.AddItem((UIMenuItem) LSL.putCrackOzStash);
      LSL.putCrackOzStash.Maximum = LsFunctions.pCrackOunce;
      LSL.putCrackOzStash.Multiplier = 1;
      LSL.putCrackStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putCocainOzStash = new UIMenuSliderItem("Cocaine Oz");
      LSL.stashPutMenu.AddItem((UIMenuItem) LSL.putCocainOzStash);
      LSL.putCocainOzStash.Maximum = LsFunctions.pCocaineOunce;
      LSL.putCocainStash.Multiplier = 1;
      LSL.putCocainStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putMoneyStash = new UIMenuSliderItem("Money $");
      LSL.stashPutMenu.AddItem((UIMenuItem) LSL.putMoneyStash);
      LSL.putMoneyStash.Maximum = LSL.player.Money;
      LSL.putMoneyStash.Multiplier = 100;
      LSL.takeMoneyStash = new UIMenuSliderItem("Money $");
      LSL.stashTakeMenu.AddItem((UIMenuItem) LSL.takeMoneyStash);
      LSL.takeMoneyStash.Maximum = 0;
      LSL.takeMoneyStash.Multiplier = 100;
      LSL.putMoneyStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeMoneyStash.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.stashPutMultiplier = new UIMenuListItem("Multiplier", LSL.multiplier, 0);
      LSL.stashPutMenu.AddItem((UIMenuItem) LSL.stashPutMultiplier);
      LSL.stashPutMultiplier.OnListChanged += new ItemListEvent(LsMenus.onListChange);
      LSL.stashTakeMultiplier = new UIMenuListItem("Multiplier", LSL.multiplier, 0);
      LSL.stashTakeMenu.AddItem((UIMenuItem) LSL.stashTakeMultiplier);
      LSL.stashTakeMultiplier.OnListChanged += new ItemListEvent(LsMenus.onListChange);
      InventoryText.Innit();
      LSL.stashMainMenu.FormatDescriptions = true;
      string str1 = "Weed. ";
      string str2 = "Crack. ";
      string str3 = "Cocaine. ";
      string str4 = "Grams of ";
      string str5 = "Oz's of ";
      string str6 = "Ready to sell on the streets or give to a dealer.";
      string str7 = "Must be deposited in a Stash house before it can be sold.";
      LSL.takeWeedStash.Description = str4 + str1 + str6;
      LSL.takeCrackStash.Description = str4 + str2 + str6;
      LSL.takeCocainStash.Description = str4 + str3 + str6;
      LSL.putWeedStash.Description = str4 + str1 + str6;
      LSL.putCrackStash.Description = str4 + str2 + str6;
      LSL.putCocainStash.Description = str4 + str3 + str6;
      LSL.putWeedOzStash.Description = str5 + str1 + str7;
      LSL.putCrackOzStash.Description = str5 + str2 + str7;
      LSL.putCocainOzStash.Description = str5 + str3 + str7;
      List<object> items1 = new List<object>();
      for (int index = 0; index < LSL.playerArmours + 1; ++index)
        items1.Add((object) index);
      LSL.putArmourStash = new UIMenuListItem("Stash Armour", items1, 0);
      LSL.stashPutMenu.AddItem((UIMenuItem) LSL.putArmourStash);
      List<object> items2 = new List<object>();
      for (int index = 0; index < 1; ++index)
        items2.Add((object) index);
      LSL.takeArmourStash = new UIMenuListItem("Take Armour", items2, 0);
      LSL.stashTakeMenu.AddItem((UIMenuItem) LSL.takeArmourStash);
      if (LSL.playerWeapons.Count > 0)
      {
        LSL.putWeaponStash = new UIMenuListItem("Stash Weapon: ", LSL.playerWeps, 0);
        LSL.stashPutMenu.AddItem((UIMenuItem) LSL.putWeaponStash);
        LSL.putWeaponStash.OnListChanged += new ItemListEvent(LsMenus.onListChange);
        LSL.takeWeaponStash = new UIMenuListItem("Take Weapon:", LSL.playerWeps, 0);
        LSL.stashTakeMenu.AddItem((UIMenuItem) LSL.takeWeaponStash);
        LSL.takeWeaponStash.OnListChanged += new ItemListEvent(LsMenus.onListChange);
      }
      LSL.stashPutMenu.OnItemSelect += new ItemSelectEvent(LsMenus.onStashMenuSelect);
      LSL.stashTakeMenu.OnItemSelect += new ItemSelectEvent(LsMenus.onPlayerMenuSelect);
      LSL.vehStashMainMenu = new UIMenu("Vehicle Stash", "Select");
      if (LSL.UseSprites)
        LSL.vehStashMainMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_stash.png");
      LSL.LsMenuPool.Add(LSL.vehStashMainMenu);
      LSL.vehStashTakeMenu = LSL.LsMenuPool.AddSubMenu(LSL.vehStashMainMenu, "Take");
      if (LSL.UseSprites)
        LSL.vehStashTakeMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_stash.png");
      LSL.vehStashPutMenu = LSL.LsMenuPool.AddSubMenu(LSL.vehStashMainMenu, "Stash");
      if (LSL.UseSprites)
        LSL.vehStashPutMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_stash.png");
      LSL.putWeedVeh = new UIMenuSliderItem("Weed " + LsFunctions.GramsToOz(LSL.PlayerInventory["Weed"]));
      LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.putWeedVeh);
      LSL.putWeedVeh.Maximum = LSL.PlayerInventory["Weed"];
      LSL.putWeedVeh.Multiplier = 1;
      LSL.takeWeedVeh = new UIMenuSliderItem("Weed drugCarWeedg");
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.takeWeedVeh);
      LSL.takeWeedVeh.Maximum = 0;
      LSL.takeWeedVeh.Multiplier = 1;
      LSL.putWeedVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeWeedVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putCrackVeh = new UIMenuSliderItem("Crack " + LsFunctions.GramsToOz(LSL.PlayerInventory["Crack"]));
      LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.putCrackVeh);
      LSL.putCrackVeh.Maximum = LSL.PlayerInventory["Crack"];
      LSL.putCrackVeh.Multiplier = 1;
      LSL.takeCrackVeh = new UIMenuSliderItem("Crack drugCarCrackg");
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.takeCrackVeh);
      LSL.takeCrackVeh.Maximum = 0;
      LSL.takeCrackVeh.Multiplier = 1;
      LSL.putCrackVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeCrackVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putCocainVeh = new UIMenuSliderItem("Cocaine " + LsFunctions.GramsToOz(LSL.PlayerInventory["Cocaine"]));
      LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.putCocainVeh);
      LSL.putCocainVeh.Maximum = LSL.PlayerInventory["Cocaine"];
      LSL.putCocainVeh.Multiplier = 1;
      LSL.takeCocainVeh = new UIMenuSliderItem("Cocaine drugCarCocaing");
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.takeCocainVeh);
      LSL.takeCocainVeh.Maximum = 0;
      LSL.takeCocainVeh.Multiplier = 1;
      LSL.putCocainVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeCocainVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putWeedOzVeh = new UIMenuSliderItem("Weed Z's~g~" + LsFunctions.pWeedOunce.ToString() + "~s~oz");
      LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.putWeedOzVeh);
      LSL.putWeedOzVeh.Maximum = LsFunctions.pWeedOunce;
      LSL.putWeedOzVeh.Multiplier = 1;
      LSL.takeWeedOzVeh = new UIMenuSliderItem("Weed Z'sdrugCarWeedoz");
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.takeWeedOzVeh);
      LSL.takeWeedOzVeh.Maximum = 0;
      LSL.takeWeedOzVeh.Multiplier = 1;
      LSL.putWeedOzVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeWeedOzVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putCrackOzVeh = new UIMenuSliderItem("Crack Z's~g~" + LsFunctions.pCrackOunce.ToString() + "~s~oz");
      LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.putCrackOzVeh);
      LSL.putCrackOzVeh.Maximum = LsFunctions.pCrackOunce;
      LSL.putCrackOzVeh.Multiplier = 1;
      LSL.takeCrackOzVeh = new UIMenuSliderItem("Crack Z'sdrugCarCrackoz");
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.takeCrackOzVeh);
      LSL.takeCrackOzVeh.Maximum = 0;
      LSL.takeCrackOzVeh.Multiplier = 1;
      LSL.putCrackOzVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeCrackOzVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putCocainOzVeh = new UIMenuSliderItem("Cocaine Z's~g~" + LsFunctions.pCocaineOunce.ToString() + "~s~oz");
      LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.putCocainOzVeh);
      LSL.putCocainOzVeh.Maximum = LsFunctions.pCocaineOunce;
      LSL.putCocainOzVeh.Multiplier = 1;
      LSL.takeCocainOzVeh = new UIMenuSliderItem("Cocaine Z'sdrugCarCocaing");
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.takeCocainOzVeh);
      LSL.takeCocainOzVeh.Maximum = 0;
      LSL.takeCocainOzVeh.Multiplier = 1;
      LSL.putCocainOzVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeCocainOzVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.putMoneyVeh = new UIMenuSliderItem("Money $" + LsFunctions.IntToMoney(LSL.player.Money));
      LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.putMoneyVeh);
      LSL.putMoneyVeh.Maximum = LSL.player.Money;
      LSL.putMoneyVeh.Multiplier = 100;
      LSL.takeMoneyVeh = new UIMenuSliderItem("Money $drugCarMoney");
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.takeMoneyVeh);
      LSL.takeMoneyVeh.Maximum = 0;
      LSL.takeMoneyVeh.Multiplier = 100;
      LSL.putMoneyVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.takeMoneyVeh.OnSliderChanged += new ItemSliderEvent(LsMenus.onSliderChange);
      LSL.vehPutMultiplier = new UIMenuListItem("Multiplier", LSL.multiplier, 0);
      LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.vehPutMultiplier);
      LSL.vehPutMultiplier.OnListChanged += new ItemListEvent(LsMenus.onListChange);
      LSL.vehTakeMultiplier = new UIMenuListItem("Multiplier", LSL.multiplier, 0);
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.vehTakeMultiplier);
      LSL.vehTakeMultiplier.OnListChanged += new ItemListEvent(LsMenus.onListChange);
      LSL.takeWeaponVeh = new UIMenuListItem("Take Weapon :", new List<object>()
      {
        (object) "EROaR!"
      }, 0);
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.takeWeaponVeh);
      LSL.takeWeaponVeh.OnListChanged += new ItemListEvent(LsMenus.onListChange);
      LSL.putArmourVeh = new UIMenuListItem("Stash Armour", items1, 0);
      LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.putArmourVeh);
      LSL.putArmourVeh.Activated += new ItemActivatedEvent(this.PutArmourVeh_Activated);
      List<object> items3 = new List<object>();
      for (int index = 0; index < 1; ++index)
        items3.Add((object) index);
      LSL.takeArmourVeh = new UIMenuListItem("Take Armour", items3, 0);
      LSL.vehStashTakeMenu.AddItem((UIMenuItem) LSL.takeArmourVeh);
      LSL.takeArmourVeh.Activated += new ItemActivatedEvent(this.TakeArmourVeh_Activated);
      if (LSL.playerWeapons.Count > 0)
      {
        LSL.putWeaponVeh = new UIMenuListItem("Stash Weapon: ", LSL.playerWeps, 0);
        LSL.vehStashPutMenu.AddItem((UIMenuItem) LSL.putWeaponVeh);
        LSL.putWeaponVeh.OnListChanged += new ItemListEvent(LsMenus.onListChange);
      }
      LSL.vehStashPutMenu.OnItemSelect += new ItemSelectEvent(LsMenus.onPutVehicleMenuSelect);
      LSL.vehStashTakeMenu.OnItemSelect += new ItemSelectEvent(LsMenus.onTakeVehicleMenuSelect);
      LSL.customerMainMenu = new UIMenu("Customer", "Select an option");
      if (LSL.UseSprites)
        LSL.customerMainMenu.SetBannerType("scripts\\LSLife\\sprites\\banner_customer.png");
      LSL.LsMenuPool.Add(LSL.customerMainMenu);
      LSL.sell = new UIMenuItem("drug");
      LSL.customerMainMenu.AddItem(LSL.sell);
      LSL.turnAway = new UIMenuItem("cya");
      LSL.customerMainMenu.AddItem(LSL.turnAway);
      LSL.customerMainMenu.OnItemSelect += new ItemSelectEvent(LsMenus.onCustomerMenuSelect);
      LSL.LsMenuPool.FormatDescriptions = true;
      LSL.LsMenuPool.RefreshIndex();
      LSL.iFruit = new CustomiFruit();
      LSL.contactZee = new iFruitContact("Zee");
      LSL.contactZee.Answered += new ContactAnsweredEvent(LsFunctions.ContactAnswered);
      LSL.contactZee.DialTimeout = 1000;
      LSL.contactZee.Active = true;
      LSL.contactZee.Icon = ContactIcon.Blank;
      LSL.iFruit.Contacts.Add(LSL.contactZee);
      LSL.DealerHandler = new PlayerDealerHandler();
      LSL.HouseHandler = new StashHouseHandler();
      LsFunctions.LoadSpawnLocations();
      LSL.day = Function.Call<int>(Hash._0xD972E4BD7AEB235F);
      LSL.hour = Function.Call<int>(Hash._0x25223CA6B4D20B7F);
      LSL.SlowTickTime = Game.GameTime;
      LSL.jobOfferTimer = Game.GameTime;
      LSL.randomAttackTimer = Game.GameTime;
      LSL.reloadTimer = Game.GameTime;
    }

    private void OrderDrugs_OnMenuOpen(UIMenu sender)
    {
      if (LSL.NewOrder.Placed)
        return;
      LSL.PlaceOrder.Text = "Place Order $" + LsFunctions.IntToMoney(LSL.NewOrder.Bill);
      LsFunctions.UpdateOrderMenuDesc();
    }

    private void DMultiplier_OnListChanged(UIMenuListItem sender, int newIndex)
    {
      int num = (int) sender.Items[sender.Index];
      LSL.bushWeed.Multiplier = num;
      LSL.crack.Multiplier = num;
      LSL.cocaine.Multiplier = num;
    }

    private void WMultiplier_OnListChanged(UIMenuListItem sender, int newIndex)
    {
      UIMenuSliderItem wWeed = LSL.wWeed;
      // ISSUE: reference to a compiler-generated field
      if (LSL.\u003C\u003Eo__249.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LSL.\u003C\u003Eo__249.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (LSL)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num1 = LSL.\u003C\u003Eo__249.\u003C\u003Ep__0.Target((CallSite) LSL.\u003C\u003Eo__249.\u003C\u003Ep__0, LSL.multiplier[LSL.wMultiplier.Index]);
      wWeed.Multiplier = num1;
      UIMenuSliderItem wCrack = LSL.wCrack;
      // ISSUE: reference to a compiler-generated field
      if (LSL.\u003C\u003Eo__249.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LSL.\u003C\u003Eo__249.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (LSL)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num2 = LSL.\u003C\u003Eo__249.\u003C\u003Ep__1.Target((CallSite) LSL.\u003C\u003Eo__249.\u003C\u003Ep__1, LSL.multiplier[LSL.wMultiplier.Index]);
      wCrack.Multiplier = num2;
      UIMenuSliderItem wCocaine = LSL.wCocaine;
      // ISSUE: reference to a compiler-generated field
      if (LSL.\u003C\u003Eo__249.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LSL.\u003C\u003Eo__249.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (LSL)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num3 = LSL.\u003C\u003Eo__249.\u003C\u003Ep__2.Target((CallSite) LSL.\u003C\u003Eo__249.\u003C\u003Ep__2, LSL.multiplier[LSL.wMultiplier.Index]);
      wCocaine.Multiplier = num3;
    }

    private void TakeArmourVeh_Activated(UIMenu sender, UIMenuItem selectedItem)
    {
      int index1 = LSL.takeArmourVeh.Index;
      int num = 5;
      if (LSL.player.Character.Armor < 100)
        num = 6;
      if (index1 <= 0 || LSL.playerArmours + index1 > num)
        return;
      if (LSL.player.Character.Armor < 100)
      {
        LSL.player.Character.Armor = 100;
        --index1;
      }
      LSL.playerArmours += index1;
      LSL.pStashVehicle.drugCarArmour -= index1;
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "drugCarStash")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "armour")
            {
              element.Value = LSL.pStashVehicle.drugCarArmour.ToString();
              break;
            }
          }
        }
        if (descendant.Name == (XName) "playerInventory")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "armour")
            {
              element.Value = LSL.playerArmours.ToString();
              break;
            }
          }
        }
      }
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      LSL.putArmourVeh.Items = new List<object>();
      for (int index2 = 0; index2 < LSL.playerArmours + 1; ++index2)
        LSL.putArmourVeh.Items.Add((object) index2);
      LSL.takeArmourVeh.Items = new List<object>();
      for (int index2 = 0; index2 < LSL.pStashVehicle.drugCarArmour + 1; ++index2)
        LSL.takeArmourVeh.Items.Add((object) index2);
    }

    private void PutArmourVeh_Activated(UIMenu sender, UIMenuItem selectedItem)
    {
      int index1 = LSL.putArmourVeh.Index;
      if (index1 <= 0 || LSL.playerArmours <= 0)
        return;
      LSL.playerArmours -= index1;
      LSL.pStashVehicle.drugCarArmour += index1;
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "drugCarStash")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "armour")
            {
              element.Value = LSL.pStashVehicle.drugCarArmour.ToString();
              break;
            }
          }
        }
        if (descendant.Name == (XName) "playerInventory")
        {
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "armour")
            {
              element.Value = LSL.playerArmours.ToString();
              break;
            }
          }
        }
      }
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      LSL.putArmourVeh.Index = 0;
      LSL.putArmourVeh.Items = new List<object>();
      for (int index2 = 0; index2 < LSL.playerArmours + 1; ++index2)
        LSL.putArmourVeh.Items.Add((object) index2);
      LSL.takeArmourVeh.Items = new List<object>();
      for (int index2 = 0; index2 < LSL.pStashVehicle.drugCarArmour + 1; ++index2)
        LSL.takeArmourVeh.Items.Add((object) index2);
    }

    private void onTick(object sender, EventArgs e)
    {
      if (Game.IsLoading)
        return;
      if (LSL.firstRun && (Entity) LSL.player.Character != (Entity) null)
      {
        if (File.Exists("scripts\\LSLife\\LSLifeAreas.xml"))
          LSL.LoadAreaData();
        UI.Notify("~g~LsLife: 0.2.42.19");
        UI.Notify("~g~LsLife: " + LSL.areas.Count.ToString() + " Areas loaded");
        LSL.lPeds = new LsLPeds();
        World.SetRelationshipBetweenGroups(Relationship.Neutral, LSL.playerRelationship, LSL.CustomerRel);
        World.SetRelationshipBetweenGroups(Relationship.Neutral, LSL.playerRelationship, LSL.rival_nutral);
        World.SetRelationshipBetweenGroups(Relationship.Hate, LSL.playerRelationship, LSL.rival_enemy);
        World.SetRelationshipBetweenGroups(Relationship.Neutral, LSL.rival_nutral, LSL.CustomerRel);
        World.SetRelationshipBetweenGroups(Relationship.Neutral, LSL.rival_nutral, LSL.playerRelationship);
        World.SetRelationshipBetweenGroups(Relationship.Respect, LSL.rival_nutral, LSL.rival_enemy);
        World.SetRelationshipBetweenGroups(Relationship.Respect, LSL.rival_enemy, LSL.CustomerRel);
        World.SetRelationshipBetweenGroups(Relationship.Hate, LSL.rival_enemy, LSL.playerRelationship);
        World.SetRelationshipBetweenGroups(Relationship.Respect, LSL.rival_enemy, LSL.rival_nutral);
        World.SetRelationshipBetweenGroups(Relationship.Companion, LSL.CustomerRel, LSL.playerRelationship);
        World.SetRelationshipBetweenGroups(Relationship.Companion, LSL.CustomerRel, LSL.rival_nutral);
        World.SetRelationshipBetweenGroups(Relationship.Companion, LSL.CustomerRel, LSL.rival_enemy);
        LSL.ratChance = -100;
        Function.Call(Hash._0x262B14F48D29DE80, (InputArgument) LSL.player.Character, (InputArgument) 9, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0);
        LSL.playerHasBag = false;
        LSL.canSprint = true;
        LSL.playerArea = World.GetZoneNameLabel(LSL.playerPos);
        if (LSL.areas.Where<Area>((Func<Area, bool>) (a => a.Name != LSL.playerArea)).Count<Area>() == 0)
        {
          if (LSL.zoneData.ContainsKey(LSL.playerArea))
            LSL.areas.Add(new Area(LSL.playerArea, LsFunctions.LsDrugs(), LSL.zoneData[LSL.playerArea].Item1));
          else
            UI.Notify("~r~LsLife - Error getting area type for Area " + LSL.playerArea);
        }
        Area area = LSL.areas.Find((Predicate<Area>) (a => a.Name == LSL.playerArea));
        if (area != null)
          LSL.areaIndex = LSL.areas.IndexOf(area);
        LSL.firstRun = false;
      }
      if (LSL.DEBUG)
      {
        string caption = "0";
        if (Game.GameTime < LSL.areas[LSL.areaIndex].DemandBuff())
          caption = (LSL.areas[LSL.areaIndex].DemandBuff() - Game.GameTime).ToString();
        LSL.myUIText = new UIText(caption, new Point(10, 10), 0.4f, Color.WhiteSmoke, GTA.Font.ChaletLondon, false);
        LSL.myUIText.Draw();
      }
      if (Prison.Started && Prison.CurrentState > Prison.PrisonStates.ApproachBarrier)
      {
        Function.Call(Hash._0x9DC711BC69C548DF, (InputArgument) "restrictedareas");
        Function.Call(Hash._0x9DC711BC69C548DF, (InputArgument) "re_prison");
      }
      if (LSL.LsMenuPool != null)
        LSL.LsMenuPool.ProcessMenus();
      LSL.iFruit.Update();
      if ((Entity) LSL.player.Character != (Entity) null && LSL.player.Character.IsAlive && LSL.player.IsPlaying)
      {
        LSL.playerPos = LSL.player.Character.Position;
        if (LSL.DEBUG)
          this.PedDebugger.OnTick();
        if (LSL.DealerHandler != null)
          LSL.DealerHandler.OnTick();
        if (LSL.HouseHandler != null)
          LSL.HouseHandler.Ontick();
        if (LSL.ZeesDealers != null)
          LSL.ZeesDealers.OnTick();
        this.HandlePotentialWorker();
        if (this.showInventory)
        {
          InventoryText.InventoryGui();
          if (LSL.AreaDebug && !LSL.LsMenuPool.IsAnyMenuOpen())
          {
            this.areaInfoUI();
            this.ZeeInfoUI();
            this.PlayerUI();
          }
          LSL.player.GetTargetedEntity();
        }
        if (LSL.pigs.Count > 0 && LSL.pigs[0].AskPlayerToComply)
        {
          if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0 && !LSL.LsMenuPool.IsAnyMenuOpen())
          {
            Function.Call(Hash._0x6178F68A87A4D3A0);
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("~INPUT_CONTEXT~ to comply.");
          }
          if (Game.IsControlJustPressed(2, GTA.Control.Context))
          {
            LSL.PlayerComplied = true;
            TaskSequence sequence = new TaskSequence();
            if ((Entity) LSL.player.Character.CurrentVehicle != (Entity) null)
              sequence.AddTask.LeaveVehicle();
            sequence.AddTask.TurnTo(LSL.pigs[0].Ped.ForwardVector + LSL.playerPos, 1000);
            sequence.AddTask.HandsUp(20000);
            LSL.player.Character.Task.PerformSequence(sequence);
          }
        }
        this.HandleAreaDealers();
        if (!LSL.LsMenuPool.IsAnyMenuOpen() && (Entity) LSL.weaponDealer1 != (Entity) null && (LSL.readyToDeal && Game.IsControlJustPressed(2, GTA.Control.Context)) && (LSL.LsMenuPool != null && !LSL.LsMenuPool.IsAnyMenuOpen()))
          LSL.wepMenu.Visible = !LSL.wepMenu.Visible;
        if (LSL.pStashVehicle != null)
          LSL.pStashVehicle.OnTick();
        if ((double) Game.GameTime > (double) LSL.ratTimer + 1000.0)
        {
          LSL.timeDealing = !LSL.isDealing ? 0 : Game.GameTime - LSL.timeStartedDealing;
          LSL.thisStreet = World.GetStreetName(new Vector2(LSL.playerPos.X, LSL.playerPos.Y));
          LSL.ratTimer = (float) Game.GameTime;
          if (LSL.thisStreet != LSL.lastStreet)
          {
            LSL.lastStreet = LSL.thisStreet;
            if (LSL.ratChance - 10 >= -100)
              LSL.ratChance -= 10;
            else
              LSL.ratChance = -100;
            if (LSL.isDealing)
              LSL.timeStartedDealing = Game.GameTime;
          }
          else if ((double) Game.GameTime > (double) LSL.lastTimeDealDone + 10000.0)
          {
            if (LSL.ratChance > -100 && LSL.player.WantedLevel == 0)
            {
              if (LSL.isDealing)
              {
                if (LSL.ratChance < 100)
                  LSL.ratChance += LSL.timeDealing / 180000;
                if (LSL.ratChance > 100)
                  LSL.ratChance = 100;
              }
              --LSL.ratChance;
            }
          }
          else if (LSL.ratChance < 100)
          {
            int num1 = System.Math.Min(10, System.Math.Max((int) ((double) (LSL.timeDealing / 60000) + ((double) LSL.areas[LSL.areaIndex].Heat * 0.1 + (double) LSL.areas[LSL.areaIndex].CopPresance) * 0.5 - System.Math.Min(10.0, LsFunctions.CurrentRepLvl((double) (int) LSL.areas[LSL.areaIndex].Reputation))), 1));
            int num2 = LSL.ratChance + num1;
            LSL.ratChance = num2 <= 100 ? (num2 >= -100 ? num2 : -100) : 100;
            LSL.areas[LSL.areaIndex].Heat += LSL.PedsCanSeeDeal;
          }
        }
        LSL.HandleOldCutomers();
        if (LSL.isDealing)
        {
          if (LSL.removedStuff)
            LSL.removedStuff = false;
          LSL.DropOffers();
          if (LSL.customers.Count < 3 && LSL.jobs.Count == 0 && (double) Game.GameTime > (double) LSL.lastTimePedSelected + 1000.0)
          {
            LSL.lastTimePedSelected = (float) Game.GameTime;
            if (LSL.rnd.Next(0, 100) > 60 && LSL.areas[LSL.areaIndex].NeedCustomer())
            {
              if (LSL.HouseHandler.CurrentHouse != null && LSL.HouseHandler.CurrentHouse.inside)
              {
                foreach (Drug drug in LSL.areas[LSL.areaIndex].Drugs)
                {
                  if (LSL.customers.Count < 3)
                  {
                    string name = drug.Name;
                    if (!(name == "Weed"))
                    {
                      if (!(name == "Crack"))
                      {
                        if (name == "Cocaine")
                        {
                          if (!LSL.sellingCocaine)
                          {
                            drug.Supplied = false;
                            drug.SelectCustomer = false;
                          }
                          else if (drug.SelectCustomer)
                          {
                            System.Random random = new System.Random();
                            int num = (int) (10.0 * (double) drug.Demand);
                            if ((double) drug.Demand > 1.0)
                              num = 10;
                            int maxValue = 15 - num;
                            if (random.Next(0, maxValue) == 0)
                            {
                              PedHash _hash = PedHash.BurgerDrug;
                              if (LsFunctions.cHashes.Count > 0)
                                _hash = LsFunctions.cHashes[LSL.rnd.Next(0, LsFunctions.cHashes.Count)];
                              Ped ped = World.CreatePed(LsFunctions.RequestModel(_hash), LSL.HouseHandler.CurrentHouse.exitPos);
                              if ((Entity) ped != (Entity) null)
                                LSL.customers.Add(new Customer(ped, LSL.playerPos, false, (Vehicle) null, drug, (int) LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation)));
                            }
                          }
                        }
                      }
                      else if (!LSL.sellingCrack)
                      {
                        drug.Supplied = false;
                        drug.SelectCustomer = false;
                      }
                      else if (drug.SelectCustomer)
                      {
                        System.Random random = new System.Random();
                        int num = (int) (10.0 * (double) drug.Demand);
                        if ((double) drug.Demand > 1.0)
                          num = 10;
                        int maxValue = 15 - num;
                        if (random.Next(0, maxValue) == 0)
                        {
                          PedHash _hash = PedHash.BurgerDrug;
                          if (LsFunctions.cHashes.Count > 0)
                            _hash = LsFunctions.cHashes[LSL.rnd.Next(0, LsFunctions.cHashes.Count)];
                          Ped ped = World.CreatePed(LsFunctions.RequestModel(_hash), LSL.HouseHandler.CurrentHouse.exitPos);
                          if ((Entity) ped != (Entity) null)
                            LSL.customers.Add(new Customer(ped, LSL.playerPos, false, (Vehicle) null, drug, (int) LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation)));
                        }
                      }
                    }
                    else if (!LSL.sellingWeed)
                    {
                      drug.Supplied = false;
                      drug.SelectCustomer = false;
                    }
                    else if (drug.SelectCustomer)
                    {
                      System.Random random = new System.Random();
                      int num = (int) (10.0 * (double) drug.Demand);
                      if ((double) drug.Demand > 1.0)
                        num = 10;
                      int maxValue = 15 - num;
                      if (random.Next(0, maxValue) == 0)
                      {
                        PedHash _hash = PedHash.BurgerDrug;
                        if (LsFunctions.cHashes.Count > 0)
                          _hash = LsFunctions.cHashes[LSL.rnd.Next(0, LsFunctions.cHashes.Count)];
                        Ped ped = World.CreatePed(LsFunctions.RequestModel(_hash), LSL.HouseHandler.CurrentHouse.exitPos);
                        if ((Entity) ped != (Entity) null)
                          LSL.customers.Add(new Customer(ped, LSL.playerPos, false, (Vehicle) null, drug, (int) LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation)));
                      }
                    }
                  }
                  else
                    break;
                }
              }
              else
              {
                List<Ped> peds = new List<Ped>();
                foreach (Ped getPed in LSL.lPeds.GetPeds)
                {
                  if (!LSL.CheckIfPedUsed(getPed, true, true))
                    peds.Add(getPed);
                }
                if (peds.Count > 0)
                {
                  LSL.areas[LSL.areaIndex].GetCustomer(peds);
                  foreach (Drug drug in LSL.areas[LSL.areaIndex].Drugs)
                  {
                    bool flag = true;
                    string name = drug.Name;
                    if (!(name == "Weed"))
                    {
                      if (!(name == "Crack"))
                      {
                        if (name == "Cocaine" && !LSL.sellingCocaine)
                        {
                          drug.Supplied = false;
                          flag = false;
                          drug.SelectCustomer = false;
                          drug.Customer = (Ped) null;
                        }
                      }
                      else if (!LSL.sellingCrack)
                      {
                        drug.Supplied = false;
                        flag = false;
                        drug.SelectCustomer = false;
                        drug.Customer = (Ped) null;
                      }
                    }
                    else if (!LSL.sellingWeed)
                    {
                      drug.Supplied = false;
                      flag = false;
                      drug.SelectCustomer = false;
                      drug.Customer = (Ped) null;
                    }
                    if (flag && drug.SelectCustomer)
                    {
                      Ped customer = drug.Customer;
                      drug.Customer = (Ped) null;
                      drug.SelectCustomer = false;
                      if ((Entity) customer != (Entity) null)
                      {
                        if (customer.IsInVehicle() && (Entity) customer.CurrentVehicle.Driver == (Entity) customer)
                        {
                          Vehicle currentVehicle = customer.CurrentVehicle;
                          LSL.customers.Add(new Customer(customer, LSL.playerPos, false, currentVehicle, drug, (int) LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation)));
                        }
                        else
                          LSL.customers.Add(new Customer(customer, LSL.playerPos, false, (Vehicle) null, drug, (int) LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation)));
                      }
                    }
                  }
                }
              }
            }
          }
          if (LSL.customers.Count > 0)
          {
            if (Game.GameTime > LSL.lastCustomerCheckTimer + 370)
            {
              foreach (Customer c in LSL.customers.ToList<Customer>())
              {
                if (c != null)
                {
                  if (c.Ped.Exists() && c.Ped.IsAlive && !c.Ped.IsPlayer)
                  {
                    if ((double) c.Ped.Position.DistanceTo(LSL.playerPos) > 80.0)
                    {
                      LSL.customers.Remove(c);
                      LsFunctions.RemovePedFromWorld(c.Ped, true);
                      break;
                    }
                    if (c.Ped.CurrentPedGroup != LSL.player.Character.CurrentPedGroup)
                    {
                      if ((double) c.Ped.Position.DistanceTo(LSL.playerPos) < 3.0)
                      {
                        c.Ped.Task.ClearAll();
                        Function.Call(Hash._0x9F3480FE65DB31B5, (InputArgument) c.Ped.Handle, (InputArgument) LSL.playerGroup);
                        Function.Call(Hash._0x8E04FEDD28D42462, (InputArgument) c.Ped, (InputArgument) "GENERIC_HI", (InputArgument) "SPEECH_PARAMS_STANDARD");
                      }
                      else if (c.Ped.IsInVehicle())
                      {
                        if (c.Target != World.GetNextPositionOnStreet(LSL.playerPos))
                        {
                          c.Target = World.GetNextPositionOnStreet(LSL.playerPos);
                          c.Ped.Task.PerformSequence(LSL.PerformTaskSeq(2, c.Ped.CurrentVehicle, c.Target));
                        }
                        LSL.lastCustomerCheckTimer = Game.GameTime;
                      }
                      else
                      {
                        Vector3 forwardVector = LSL.player.Character.ForwardVector;
                        Vector3 rightVector = LSL.player.Character.RightVector;
                        if (LSL.player.Character.IsInVehicle())
                          LSL.MoveCustomer(c, LSL.playerPos + rightVector * 2f);
                        else
                          LSL.MoveCustomer(c, LSL.playerPos + forwardVector * 2f);
                        LSL.lastCustomerCheckTimer = Game.GameTime;
                      }
                    }
                  }
                  else
                  {
                    LSL.customers.Remove(c);
                    LsFunctions.RemovePedFromWorld(c.Ped, true);
                  }
                }
              }
            }
            Customer customer = !LSL.player.Character.IsInVehicle() ? this.FindClosestCustomer() : this.FindClosestCustomerInVehicle();
            if (customer != null && !LSL.LsMenuPool.IsAnyMenuOpen())
            {
              if (LSL.player.Character.IsInVehicle() && customer.Ped.IsInVehicle() && (double) LSL.player.Character.CurrentVehicle.Speed < 1.0)
              {
                if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0 && !LSL.jobOffer && LSL.PottentialWorker == null)
                {
                  Function.Call(Hash._0x6178F68A87A4D3A0);
                  LSL.displayHelpTimer = (float) Game.GameTime;
                  LsFunctions.DisplayHelpText("~INPUT_SPRINT~ to sell from car.");
                }
                if (Game.IsControlJustPressed(2, GTA.Control.Sprint))
                {
                  Function.Call(Hash._0x6178F68A87A4D3A0);
                  LSL.customerSellTo = customer;
                  LSL.sell.Text = "Sell " + LsFunctions.GramsToOz(LSL.customerSellTo.amount) + " of " + LSL.customerSellTo.Drug.Name + "?";
                  LSL.customerMainMenu.Subtitle.Caption = "You have " + LsFunctions.GramsToOz(LSL.PlayerInventory[LSL.customerSellTo.Drug.Name]) + " of " + LSL.customerSellTo.Drug.Name;
                  LSL.customerMainMenu.Visible = !LSL.customerMainMenu.Visible;
                  LSL.customerMainMenu.CurrentSelection = 0;
                }
              }
              if (!LSL.player.Character.IsInVehicle() && LSL.player.Character.IsStopped && !LSL.player.Character.IsInCombat)
              {
                if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0 && !LSL.jobOffer && LSL.PottentialWorker == null)
                {
                  Function.Call(Hash._0x6178F68A87A4D3A0);
                  LSL.displayHelpTimer = (float) Game.GameTime;
                  LsFunctions.DisplayHelpText("Press ~INPUT_SPRINT~ to sell to this person.");
                }
                if (Game.IsControlJustPressed(2, GTA.Control.Sprint) && !LSL.LsMenuPool.IsAnyMenuOpen())
                {
                  Function.Call(Hash._0x6178F68A87A4D3A0);
                  LSL.customerSellTo = customer;
                  LSL.sell.Text = "Sell " + LsFunctions.GramsToOz(LSL.customerSellTo.amount) + " of " + LSL.customerSellTo.Drug.Name + "?";
                  LSL.customerMainMenu.Subtitle.Caption = "You have " + LsFunctions.GramsToOz(LSL.PlayerInventory[LSL.customerSellTo.Drug.Name]) + " of " + LSL.customerSellTo.Drug.Name;
                  LSL.customerMainMenu.Visible = !LSL.customerMainMenu.Visible;
                  LSL.customerMainMenu.CurrentSelection = 0;
                }
              }
            }
          }
        }
        if (LSL.wepDealer != null)
        {
          LSL.wepDealer.SetSpeed();
          if (!LSL.player.Character.IsInVehicle())
          {
            if ((double) LSL.wepDealer.vehicle.Position.DistanceTo(LSL.playerPos) < 3.0)
            {
              if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0)
              {
                Function.Call(Hash._0x6178F68A87A4D3A0);
                LSL.displayHelpTimer = (float) Game.GameTime;
                LsFunctions.DisplayHelpText("Press ~INPUT_VEH_EXIT~ to enter.");
              }
              if (Game.IsControlJustPressed(2, GTA.Control.VehicleExit))
              {
                Function.Call(Hash._0x6178F68A87A4D3A0);
                LSL.player.Character.Task.EnterVehicle(LSL.wepDealer.vehicle, VehicleSeat.Passenger);
              }
            }
          }
          else if (LSL.player.Character.IsInVehicle() && LSL.player.Character.CurrentVehicle.Handle == LSL.wepDealer.vehicle.Handle)
          {
            if (Game.IsControlJustPressed(2, GTA.Control.ContextSecondary))
            {
              LSL.player.Character.Task.LeaveVehicle(LSL.player.Character.CurrentVehicle, true);
              Script.Wait(5500);
              LSL.wepDealer.ped.CurrentVehicle.SoundHorn(15);
              Script.Wait(15);
              LSL.wepDealer.ped.CurrentVehicle.SoundHorn(12);
              LSL.wepDealer.ped.IsPersistent = false;
              LSL.wepDealer.vehicle.IsPersistent = false;
              LSL.wepDealer.vehicle.CurrentBlip.Remove();
              Function.Call(Hash._0x480142959D337D00, (InputArgument) LSL.wepDealer.ped.Handle, (InputArgument) LSL.wepDealer.vehicle.Handle, (InputArgument) 20f, (InputArgument) 786603);
              LSL.wepDealer = (Dealer) null;
            }
            if (Game.IsControlJustPressed(2, GTA.Control.Context))
            {
              if (LSL.wepDealer.state == Dealer.States.Driving)
                LSL.wepDealer.state = Dealer.States.Faster;
              if (LSL.wepDealer.state == Dealer.States.Fast)
                LSL.wepDealer.state = Dealer.States.Slower;
            }
          }
        }
      }
      if (Game.GameTime > LSL.oneSecDelay + 10001)
      {
        LSL.oneSecDelay = Game.GameTime;
        if (LSL.player.IsPlaying)
        {
          if (!LSL.dealer1Angry && (double) LSL.dealer1Debt > (double) LsFunctions.zeeDebtCap() + (double) LsFunctions.zeeDebtCap() * 0.5)
          {
            if (LSL.DEBUG)
              UI.Notify("dealer1 angry");
            LSL.dealer1Angry = true;
          }
          if (LSL.driveByHandler.AmountOfDriveBy(true) == 0)
          {
            if (LSL.isDealing && LSL.areas[LSL.areaIndex].GangPresance > 4 && (LSL.timeDealing > 240000 && LSL.rnd.Next(0, 60) == 0))
            {
              if (LSL.DEBUG)
                UI.Notify("poor area drive-by");
              LsFunctions.SpawnDriveBy(LSL.player.Character, true);
            }
            if (LSL.dealer1Angry && LSL.driveByHandler.AmountOfDriveBy(true) == 0 && ((Entity) LSL.drugDealer1 == (Entity) null && LSL.rnd.Next(0, 10) == 0))
            {
              if (LSL.DEBUG)
                UI.Notify("dealer1 drive-by");
              LsFunctions.SpawnDriveBy(LSL.player.Character, true);
              LsFunctions.TextMsg("ZEE", "Debt", "Pay your debt and i will call them off.");
            }
          }
          if (LSL.entitiesCleanUp.Count > 0)
          {
            foreach (Entity entity in LSL.entitiesCleanUp.ToList<Entity>())
            {
              if (entity == (Entity) null || !entity.Exists())
                LSL.entitiesCleanUp.Remove(entity);
              else if (entity.Exists())
              {
                if (entity.CurrentBlip != (Blip) null || entity.CurrentBlip.Exists())
                  entity.CurrentBlip.Remove();
                if (entity == (Entity) LSL.attacker)
                {
                  LSL.entitiesCleanUp.Remove(entity);
                  if (LSL.DEBUG)
                    UI.Notify("stoped delete attacker");
                }
                else if ((double) entity.Position.DistanceTo(LSL.playerPos) > 200.0 && !entity.IsOnScreen)
                {
                  LSL.entitiesCleanUp.Remove(entity);
                  if (LSL.DEBUG)
                    UI.Notify("Delete:" + entity?.ToString());
                  entity.Delete();
                }
              }
            }
          }
        }
      }
      if (Game.GameTime > LSL.areaCheckTimer + 999)
      {
        LSL.newWepAmount = 0;
        foreach (WeaponHash weaponHash in Enum.GetValues(typeof (WeaponHash)))
        {
          if (Function.Call<bool>(Hash._0x8DECB02F88F428BC, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash))
          {
            ++LSL.newWepAmount;
            if (LSL.newWepAmount > LSL.playerWeapons.Count)
              break;
          }
        }
        if (LSL.newWepAmount != LSL.playerWeps.Count)
        {
          if (LSL.DEBUG)
            UI.Notify("Player weapon change detected");
          LSL.playerWeps.Clear();
          LSL.playerWeapons.Clear();
          int num1 = 0;
          foreach (WeaponHash weaponHash in Enum.GetValues(typeof (WeaponHash)))
          {
            bool flag = Function.Call<bool>(Hash._0x8DECB02F88F428BC, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash);
            int num2 = Function.Call<int>(Hash._0x015A522136D7F951, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash);
            if (flag)
            {
              XElement xelement1 = new XElement((XName) "weapon");
              xelement1.SetAttributeValue((XName) "id", (object) num1);
              XElement xelement2 = new XElement((XName) "HASH", (object) weaponHash);
              if (LSL.DEBUG)
                UI.Notify("Added " + weaponHash.ToString() + " to playerWeapons list.");
              XElement xelement3 = new XElement((XName) "AMMO", (object) num2);
              xelement1.Add((object) xelement2);
              xelement1.Add((object) xelement3);
              foreach (WeaponComponent weaponComponent in Enum.GetValues(typeof (WeaponComponent)))
              {
                if (Function.Call<bool>(Hash._0xC593212475FAE340, (InputArgument) LSL.player.Character, (InputArgument) (int) weaponHash, (InputArgument) (int) weaponComponent))
                {
                  XElement xelement4 = new XElement((XName) "COMPONENT", (object) weaponComponent);
                  xelement1.Add((object) xelement4);
                }
              }
              LSL.playerWeapons.Add(xelement1);
              LSL.playerWeps.Add((object) weaponHash.ToString());
              ++num1;
            }
          }
        }
        LSL.areaCheckTimer = Game.GameTime;
        string zoneNameLabel = World.GetZoneNameLabel(LSL.playerPos);
        if (LSL.aDealer != null && LSL.playerArea != LSL.aDealer.Area && (double) LSL.playerPos.DistanceTo(LSL.aDealer.dPed.Position) > 300.0)
        {
          if ((Entity) LSL.aDealer.cPed != (Entity) null)
            LsFunctions.RemovePedFromWorld(LSL.aDealer.cPed, false);
          LsFunctions.RemovePedFromWorld(LSL.aDealer.dPed, false);
          LSL.aDealer = (AreaDealer) null;
        }
        if (LSL.playerArea != zoneNameLabel)
        {
          LSL.playerArea = zoneNameLabel;
          if (LSL.areas.FindAll((Predicate<Area>) (a => a.Name == LSL.playerArea)).Count == 0)
          {
            if (LSL.zoneData.ContainsKey(LSL.playerArea))
              LSL.areas.Add(new Area(LSL.playerArea, LsFunctions.LsDrugs(), LSL.zoneData[LSL.playerArea].Item1));
            else
              UI.Notify("~r~LsLife - Error getting area type for Area " + LSL.playerArea);
            int num = 0;
            foreach (Area area in LSL.areas)
            {
              if (area.Name == LSL.playerArea)
              {
                LSL.areaIndex = num;
                break;
              }
              ++num;
            }
          }
          else
          {
            string str = LSL.HouseHandler.CurrentHouse == null || !LSL.HouseHandler.CurrentHouse.inside ? LSL.playerArea : World.GetZoneNameLabel(LSL.HouseHandler.CurrentHouse.entrancePos);
            int num = 0;
            foreach (Area area in LSL.areas)
            {
              if (area.Name == str)
              {
                LSL.areaIndex = num;
                break;
              }
              ++num;
            }
          }
        }
        else if (LSL.aDealer == null && LSL.areas[LSL.areaIndex].CanSpawnDealer())
        {
          LsFunctions.DbugString("demandBuff 0");
          if (LSL.areas[LSL.areaIndex].GangPresance > 0 && LSL.areas[LSL.areaIndex].Name != World.GetZoneNameLabel(LSL.HouseHandler.Houses[0].entrancePos))
          {
            List<Ped> all = LSL.lPeds.GetPeds.FindAll((Predicate<Ped>) (r => World.GetZoneNameLabel(r.Position) == LSL.areas[LSL.areaIndex].Name && (double) r.Position.DistanceTo(LSL.playerPos) > 20.0));
            if (all.Count > 0)
            {
              foreach (Ped ped in all)
              {
                if (!LSL.CheckIfPedUsed(ped, false, false))
                {
                  LSL.aDealer = new AreaDealer(ped);
                  if (LSL.DEBUG)
                  {
                    UI.Notify("Area Dealer Spawned using ~g~" + ped.Model.ToString());
                    break;
                  }
                  break;
                }
              }
            }
          }
        }
        else
          LsFunctions.DbugString("Cant spawn dealer dead :" + LSL.areas[LSL.areaIndex].DealerKilled.ToString());
      }
      if (Game.GameTime <= LSL.SlowTickTime + 350)
        return;
      LSL.SlowTickTime = Game.GameTime;
      LSL.player = Game.Player;
      LSL.playerGroup = LSL.player.Character.CurrentPedGroup;
      if (Function.Call<bool>(Hash._0x557E43C447E700A8, (InputArgument) this.lsDebug))
        LSL.DEBUG = !LSL.DEBUG;
      else if (Function.Call<bool>(Hash._0x557E43C447E700A8, (InputArgument) this.areaDebug))
        LSL.AreaDebug = !LSL.AreaDebug;
      else if (Function.Call<bool>(Hash._0x557E43C447E700A8, (InputArgument) this.spawnP))
        LsFunctions.SpawnDriveBy(LSL.player.Character, false);
      if (LSL.lPeds != null)
        LSL.lPeds.Ontick();
      if (LSL.DealerHandler != null)
        LSL.DealerHandler?.OnSlowTick();
      if (LSL.pickupHandler != null)
        LSL.pickupHandler?.OnSlowTick();
      if (LSL.driveByHandler != null)
        LSL.driveByHandler.OnTick();
      if (LSL.ZeesDealers != null)
      {
        LSL.ZeesDealers.OnSlowTick();
        if (LSL.ZeesDealers.Kill)
          LSL.ZeesDealers = (DrugDealer) null;
      }
      if (LSL.aDealer != null)
      {
        if ((Entity) LSL.aDealer.cPed != (Entity) null && LSL.aDealer.cPed.IsDead)
        {
          Ped ped = (Ped) null;
          if (LSL.aDealer.dPed.GetKiller() is Ped)
            ped = LSL.aDealer.dPed.GetKiller() as Ped;
          if ((Entity) ped != (Entity) null && ped.CurrentPedGroup == LSL.player.Character.CurrentPedGroup)
          {
            LSL.aDealer.NewState = Enums.DStates.AttackPlayer;
            if (LSL.driveByHandler.AmountOfDriveBy(true) == 0 && LSL.rnd.Next(0, 3) == 0)
              LsFunctions.SpawnDriveBy(LSL.player.Character, true);
          }
          LsFunctions.RemovePedFromWorld(LSL.aDealer.cPed, false);
          LSL.aDealer.cPed = (Ped) null;
        }
        if (LSL.aDealer != null && LSL.aDealer.dPed.IsDead)
        {
          LSL.areas[LSL.areaIndex].KillDealer();
          if (LSL.aDealer.dPed.WasKilledByTakedown && LsFunctions.CanSee(LSL.aDealer.dPed, LSL.player.Character) && LSL.rnd.Next(0, 3) == 0)
          {
            if (LSL.driveByHandler.AmountOfDriveBy(true) == 0 && LSL.rnd.Next(0, 3) == 0)
              LsFunctions.SpawnDriveBy(LSL.player.Character, true);
            if (LSL.DEBUG)
              UI.Notify("Was seen killing dealer");
          }
          Ped ped = (Ped) null;
          if (LSL.aDealer.dPed.GetKiller() is Ped)
            ped = LSL.aDealer.dPed.GetKiller() as Ped;
          if ((Entity) ped != (Entity) null && ped.CurrentPedGroup == LSL.player.Character.CurrentPedGroup)
            LSL.pickupHandler.AddBag(new DroppedBag("A ~r~rival dealer~s~ dropped a ~g~bag.", LSL.rnd.Next(1, 50), LSL.rnd.Next(1, 50), LSL.rnd.Next(1, 50), LSL.rnd.Next(100, 5000), LsFunctions.PedWeapons(LSL.aDealer.dPed), LSL.aDealer.dPed.Position));
          if ((Entity) LSL.aDealer.cPed != (Entity) null)
          {
            LsFunctions.RemovePedFromWorld(LSL.aDealer.cPed, true);
            LSL.aDealer.cPed = (Ped) null;
          }
          if ((Entity) LSL.aDealer.dPed != (Entity) null)
          {
            LsFunctions.RemovePedFromWorld(LSL.aDealer.dPed, true);
            LSL.aDealer.dPed = (Ped) null;
          }
          LSL.aDealer = (AreaDealer) null;
        }
        if (LSL.aDealer != null && (Entity) LSL.aDealer.dPed != (Entity) null && (LSL.aDealer.dPed.Exists() && LSL.aDealer.dPed.IsAlive))
        {
          LSL.aDealer.DoState();
          LSL.aDealer.CheckState();
        }
      }
      if (LSL.wepDealer != null && LSL.wepDealer.ped.IsAlive)
      {
        if ((Entity) LSL.player.Character.CurrentVehicle != (Entity) LSL.wepDealer.vehicle && (double) LSL.wepDealer.ped.Position.DistanceTo(LSL.playerPos) < 10.0 || (double) LSL.wepDealer.ped.Position.DistanceTo(LSL.wepDealer.destination) < 10.0)
        {
          Vector3 vector3 = LSL.wepDealer.vehicle.Position + LSL.wepDealer.vehicle.Velocity;
          int num = Function.Call<int>(Hash._0x22D7275A79FE8215, (InputArgument) vector3.X, (InputArgument) vector3.Y, (InputArgument) vector3.Z, (InputArgument) 1, (InputArgument) 1, (InputArgument) 1f, (InputArgument) 0.0f);
          OutputArgument outputArgument = new OutputArgument();
          Function.Call(Hash._0x703123E5E7D429C2, (InputArgument) num, (InputArgument) outputArgument);
          Vector3 result = outputArgument.GetResult<Vector3>();
          LSL.wepDealer.destination = result;
        }
        if ((LSL.wepDealer.state == Dealer.States.Driving || LSL.wepDealer.state == Dealer.States.Fast) && (double) LSL.wepDealer.vehicle.Position.DistanceTo(LSL.wepDealer.destination) < 80.0)
        {
          LSL.wepDealer.ped.Task.DriveTo(LSL.wepDealer.vehicle, LSL.wepDealer.destination, 4f, 10f, 786603);
          LSL.wepDealer.state = Dealer.States.Ariving;
        }
        if (LSL.wepDealer.state == Dealer.States.Ariving && (double) LSL.wepDealer.vehicle.Position.DistanceTo(LSL.wepDealer.destination) < 20.0)
        {
          LSL.wepDealer.doneTutorial = false;
          Vector3 destination = LSL.wepDealer.destination;
          int num = Function.Call<int>(Hash._0x22D7275A79FE8215, (InputArgument) destination.X, (InputArgument) destination.Y, (InputArgument) destination.Z, (InputArgument) 1, (InputArgument) 1, (InputArgument) 1f, (InputArgument) 0.0f);
          OutputArgument outputArgument = new OutputArgument();
          Function.Call(Hash._0x703123E5E7D429C2, (InputArgument) num, (InputArgument) outputArgument);
          Vector3 result = outputArgument.GetResult<Vector3>();
          LSL.wepDealer.ped.Task.DriveTo(LSL.wepDealer.vehicle, result, 2f, 6f, 786603);
          LSL.wepDealer.destination = Vector3.Zero;
          LSL.wepDealer.state = Dealer.States.Arrived;
        }
        if (LSL.wepDealer.state == Dealer.States.Arrived && LSL.player.Character.IsInVehicle() && LSL.player.Character.CurrentVehicle.Handle == LSL.wepDealer.vehicle.Handle)
        {
          if (!LSL.wepDealer.doneTutorial)
          {
            Function.Call(Hash._0x6178F68A87A4D3A0);
            LSL.wepDealer.doneTutorial = true;
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("Press ~INPUT_CONTEXT~to interact or ~INPUT_CONTEXT_SECONDARY~to dismiss.");
          }
          if (Game.IsWaypointActive && LSL.wepDealer.destination != World.GetNextPositionOnStreet(World.GetWaypointPosition()))
          {
            LSL.wepDealer.destination = World.GetNextPositionOnStreet(World.GetWaypointPosition());
            LSL.wepDealer.vehicle.IsInvincible = true;
            LSL.wepDealer.ped.Task.ClearAll();
            LSL.wepDealer.ped.Task.DriveTo(LSL.wepDealer.vehicle, LSL.wepDealer.destination, 1f, 10f, 786603);
            UI.ShowSubtitle("Ok, we heading to ~y~" + World.GetStreetName(LSL.wepDealer.destination));
            LSL.wepDealer.state = Dealer.States.Leaving;
          }
        }
        if (LSL.wepDealer.state == Dealer.States.Leaving)
        {
          Script.Wait(5000);
          LSL.wepDealer.ped.Task.DriveTo(LSL.wepDealer.vehicle, LSL.wepDealer.destination, 1f, 40f, 786603);
          LSL.wepDealer.state = Dealer.States.Driving;
        }
        if (LSL.wepDealer.state == Dealer.States.Faster)
        {
          LSL.wepDealer.ped.Task.DriveTo(LSL.wepDealer.vehicle, LSL.wepDealer.destination, 4f, 50f, 6);
          LSL.wepDealer.state = Dealer.States.Fast;
          UI.ShowSubtitle("Ok, putting my foot down.", 2000);
          LSL.wepDealer.canSpeed = true;
        }
        if (LSL.wepDealer.state == Dealer.States.Slower)
        {
          LSL.wepDealer.ped.Task.DriveTo(LSL.wepDealer.vehicle, LSL.wepDealer.destination, 4f, 20f, 786603);
          LSL.wepDealer.state = Dealer.States.Driving;
          UI.ShowSubtitle("Ok, Slowing it down.", 2000);
          LSL.wepDealer.canSpeed = false;
        }
      }
      if (LSL.dealer1Angry && LSL.dealer1Debt < 170000)
        LSL.dealer1Angry = false;
      LsFunctions.DateTimeUpdate();
      if (LSL.attackerPickupSpawned && !LSL.attackerPickup.ObjectExists())
      {
        LSL.attackerPickup.Delete();
        LSL.attackerPickupSpawned = false;
        WeaponHash result1 = WeaponHash.Unarmed;
        int result2 = 0;
        WeaponHash hash = LSL.player.Character.Weapons.Current.Hash;
        foreach (XElement attackerWeapon in LSL.attackerWeapons)
        {
          if (attackerWeapon.Name == (XName) "weapon")
          {
            foreach (XElement element1 in attackerWeapon.Elements())
            {
              if (element1.Name == (XName) "HASH")
                Enum.TryParse<WeaponHash>(element1.Value, out result1);
              if (element1.Name == (XName) "AMMO")
                int.TryParse(element1.Value, out result2);
              if (result1 != WeaponHash.Unarmed)
              {
                if (result2 < 100)
                  result2 = 100;
                LSL.player.Character.Weapons.Give(result1, result2, true, true);
                LSL.player.Character.Weapons.Select(result1, true);
                foreach (XElement element2 in attackerWeapon.Elements())
                {
                  if (element2.Name == (XName) "COMPONENT")
                  {
                    WeaponComponent result3;
                    Enum.TryParse<WeaponComponent>(element2.Value, out result3);
                    LSL.player.Character.Weapons.Current.SetComponent(result3, true);
                  }
                }
                if (LSL.DEBUG)
                  UI.Notify("Gave " + result1.ToString() + " ammo = " + result2.ToString());
              }
            }
          }
        }
        LSL.attackerWeapons.Clear();
        LSL.player.Character.Weapons.Select(hash);
        LSL.PlayerInventory["Weed"] += this.attackerWeed;
        LSL.PlayerInventory["Crack"] += this.attackerCrack;
        LSL.PlayerInventory["Cocaine"] += this.attackerCocaine;
        LSL.player.Money += this.attackerMoney;
        this.attackerWeed = 0;
        this.attackerCrack = 0;
        this.attackerCocaine = 0;
        LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
        foreach (XElement descendant in LSL.LSLifeSave.Descendants())
        {
          if (descendant.Name == (XName) "playerInventory")
          {
            foreach (XElement element in descendant.Elements())
            {
              if (element.Name == (XName) "weed")
                element.Value = LSL.PlayerInventory["Weed"].ToString();
              if (element.Name == (XName) "crack")
                element.Value = LSL.PlayerInventory["Crack"].ToString();
              if (element.Name == (XName) "cocain")
                element.Value = LSL.PlayerInventory["Cocaine"].ToString();
            }
          }
        }
        LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      }
      if (LSL.areas.Count > 0 && LSL.playerWanted && LSL.areas[LSL.areaIndex].Heat < 5000)
        (LSL.HouseHandler.CurrentHouse == null || !LSL.HouseHandler.CurrentHouse.inside ? LSL.areas[LSL.areaIndex] : LSL.areas.Find((Predicate<Area>) (a => a.Name == World.GetZoneNameLabel(LSL.HouseHandler.CurrentHouse.entrancePos)))).Heat += LSL.player.WantedLevel;
      if (LSL.player.IsPlaying && !LSL.notFined)
        LSL.notFined = true;
      if ((Entity) LSL.player.Character != (Entity) null && !LSL.player.Character.IsAlive && !LSL.playerDead)
      {
        LSL.driveByHandler.CleanDriveBys();
        LSL.playerDead = true;
        if (LSL.isDealing)
          LSL.isDealing = false;
        Entity killer = LSL.player.Character.GetKiller();
        if (killer != (Entity) null && killer is Ped p2 && ((Entity) p2 != (Entity) null && p2.IsHuman) && (!p2.IsPlayer && !LsFunctions.IsPolice(p2) && (Entity) LSL.attacker == (Entity) null))
        {
          LsFunctions.TextMsg("ZEE", "info", "I found that person for you, i sent you his location.");
          LSL.attacker = p2;
          LSL.attacker.IsPersistent = true;
          this.attackerMoney = LSL.player.Money - (int) ((double) LSL.player.Money * 0.1);
          LSL.player.Money -= LSL.player.Money;
          if (LSL.angryPeds.Contains(LSL.attacker))
            LSL.angryPeds.Remove(LSL.attacker);
          if (LSL.driveByHandler.GetDriveBies.Count > 0)
          {
            foreach (DriveBy getDriveBy in LSL.driveByHandler.GetDriveBies)
            {
              if (getDriveBy.Peds.Count > 0 && getDriveBy.Peds.Contains(LSL.attacker))
              {
                getDriveBy.Peds.Remove(LSL.attacker);
                break;
              }
            }
          }
          if ((Entity) LSL.aDealer?.dPed == (Entity) LSL.attacker)
            LSL.aDealer = (AreaDealer) null;
          LSL.attacker.BlockPermanentEvents = true;
          LSL.attacker.IsPersistent = true;
          LsFunctions.AddBlip((Entity) LSL.attacker, BlipColor.Red, "Robber", false, false);
          WeaponHash result1 = WeaponHash.Unarmed;
          int result2 = 0;
          LSL.player.Character.Weapons.RemoveAll();
          LSL.attacker.Armor = 100;
          foreach (XElement playerWeapon in LSL.playerWeapons)
          {
            if (playerWeapon.Name == (XName) "weapon")
            {
              LSL.attackerWeapons.Add(playerWeapon);
              foreach (XElement element in playerWeapon.Elements())
              {
                if (element.Name == (XName) "HASH")
                  Enum.TryParse<WeaponHash>(element.Value, out result1);
                if (element.Name == (XName) "AMMO")
                  int.TryParse(element.Value, out result2);
                if (result1 != WeaponHash.Unarmed)
                  LSL.attacker.Weapons.Give(result1, result2, true, true);
              }
            }
          }
          LSL.playerWeapons.Clear();
          LSL.playerWeps.Clear();
          LSL.attacker.Weapons.Select(LSL.attacker.Weapons.BestWeapon);
          LSL.attacker.DropsWeaponsOnDeath = false;
          if (LSL.DEBUG)
            UI.Notify("Killer has equiped " + LSL.attacker.Weapons.Current.Name + " & and has equiped Body Armour");
          this.attackerWeed = LSL.PlayerInventory["Weed"];
          this.attackerCrack = LSL.PlayerInventory["Crack"];
          this.attackerCocaine = LSL.PlayerInventory["Cocaine"];
          foreach (string key in LSL.PlayerInventory.Keys.ToList<string>())
            LSL.PlayerInventory[key] = 0;
        }
      }
      if ((Entity) LSL.player.Character != (Entity) null && !LSL.player.IsPlaying)
      {
        if (LSL.isDealing)
          LSL.isDealing = false;
        if (LSL.notFined && LSL.playerWanted)
        {
          LSL.notFined = false;
          LSL.playerWanted = false;
          int num = LSL.TotalDrugsCarried();
          if (LSL.pStashVehicle != null && LSL.pStashVehicle.drugCarWanted && LSL.pStashVehicle.TotalDrugs() > 0)
          {
            num += LSL.pStashVehicle.TotalDrugs();
            LSL.pStashVehicle.ImpoundCar();
          }
          if ((Entity) LSL.attacker == (Entity) null && num > 0)
          {
            LsFunctions.SendTextMsg("They took $" + LsFunctions.IntToMoney(LSL.player.Money) + ", all your Drugs and Weapons.");
            LSL.player.Character.Weapons.RemoveAll();
            LSL.player.Money -= LSL.player.Money;
            if (LSL.PlayerInventory.Keys.ToList<string>().Count > 0)
            {
              foreach (string key in LSL.PlayerInventory.Keys.ToList<string>())
                LSL.PlayerInventory[key] = 0;
              LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
              foreach (XElement descendant in LSL.LSLifeSave.Descendants())
              {
                if (descendant.Name == (XName) "playerInventory")
                {
                  foreach (XElement element in descendant.Elements())
                  {
                    if (element.Name == (XName) "weed")
                      element.Value = "0";
                    if (element.Name == (XName) "crack")
                      element.Value = "0";
                    if (element.Name == (XName) "cocain")
                      element.Value = "0";
                  }
                }
              }
              LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
            }
          }
        }
        else if (LSL.playerDead)
        {
          LSL.playerDead = false;
          if ((Entity) LSL.attacker == (Entity) null && LSL.TotalDrugsCarried() > 0)
          {
            LSL.player.Character.Weapons.RemoveAll();
            LsFunctions.SendTextMsg("Police confisated $" + LsFunctions.IntToMoney(LSL.player.Money) + ", all your Drugs and Weapons.");
            LSL.player.Money -= LSL.player.Money;
            if (LSL.PlayerInventory.Keys.ToList<string>().Count > 0)
            {
              foreach (string key in LSL.PlayerInventory.Keys.ToList<string>())
                LSL.PlayerInventory[key] = 0;
              LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
              foreach (XElement descendant in LSL.LSLifeSave.Descendants())
              {
                if (descendant.Name == (XName) "playerInventory")
                {
                  foreach (XElement element in descendant.Elements())
                  {
                    if (element.Name == (XName) "weed")
                      element.Value = "0";
                    if (element.Name == (XName) "crack")
                      element.Value = "0";
                    if (element.Name == (XName) "cocain")
                      element.Value = "0";
                  }
                }
              }
              LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
            }
          }
        }
      }
      if (LSL.angryPeds.Count > 0)
      {
        int index = 0;
        foreach (Ped angryPed in LSL.angryPeds)
        {
          ++index;
          if ((Entity) angryPed == (Entity) null)
          {
            LSL.angryPeds.RemoveAt(index);
            break;
          }
          if ((Entity) angryPed != (Entity) null)
          {
            if (angryPed.IsDead)
            {
              LsFunctions.RemovePedFromWorld(angryPed, false);
              LSL.angryPeds.Remove(angryPed);
              if (LSL.areas[LSL.areaIndex].GangPresance > 0)
              {
                --LSL.areas[LSL.areaIndex].GangPresance;
                break;
              }
              break;
            }
            if ((double) LSL.playerPos.DistanceTo(angryPed.Position) > 100.0)
            {
              LsFunctions.RemovePedFromWorld(angryPed, false);
              LSL.angryPeds.Remove(angryPed);
              break;
            }
            if (!angryPed.IsInCombatAgainst(LSL.player.Character) || !angryPed.IsInMeleeCombat)
            {
              angryPed.Task.FightAgainst(LSL.player.Character);
              if (!angryPed.IsPersistent)
                angryPed.IsPersistent = true;
              angryPed.BlockPermanentEvents = true;
              if (angryPed.CurrentBlip.Exists())
              {
                angryPed.CurrentBlip.Name = "Robber";
                angryPed.CurrentBlip.IsFriendly = false;
              }
            }
          }
        }
      }
      if (!((Entity) LSL.player.Character != (Entity) null) || !LSL.player.Character.IsAlive || !LSL.player.IsPlaying)
        return;
      if (LSL.pigs.Count == 0)
      {
        if (LSL.PlayerComplied)
          LSL.PlayerComplied = false;
        if ((double) LSL.ratChance > 50.0 && LSL.rnd.Next(0, 100) == 0)
          LSL.SpawnPigs();
        if ((double) LSL.ratChance > 95.0 && LSL.rnd.Next(0, 5) == 0)
          LSL.SpawnPigs();
      }
      else
        LSL.HandlePigs();
      if (LSL.pStashVehicle == null)
        LSL.pStashVehicle = new StashVehicle();
      else
        LSL.pStashVehicle.OnDelayedTick();
      if (Prison.Started)
      {
        if (Function.Call<bool>(Hash._0x49C32D60007AFA47) && !Prison.Ready)
        {
          Function.Call(Hash._0x8D32347D6D4C40A2, (InputArgument) LSL.player.Handle, (InputArgument) false, (InputArgument) 256);
          UI.Notify("READY");
          Prison.Ready = true;
        }
        Prison.PrisonTick();
      }
      Vector3 vector3_1;
      if ((Entity) LSL.attacker != (Entity) null)
      {
        if (LSL.attacker.CurrentBlip == (Blip) null || !LSL.attacker.CurrentBlip.Exists())
          LsFunctions.AddBlip((Entity) LSL.attacker, BlipColor.Red, "Robber", false, false);
        if (LSL.attacker.IsDead)
        {
          LsFunctions.RemovePedFromWorld(LSL.attacker, true);
          if (!LSL.attackerPickupSpawned)
          {
            LSL.attackerAttacking = false;
            LSL.attackerPickupSpawned = true;
            LSL.attackerPickup = World.CreatePickup(PickupType.MoneyMedBag, LSL.attacker.Position, Vector3.Zero, new Model("prop_big_bag_01"), 100);
          }
          LSL.attacker = (Ped) null;
        }
        else
        {
          if (!LSL.attacker.IsPersistent)
            LSL.attacker.IsPersistent = true;
          vector3_1 = LSL.attacker.Position;
          float num1 = vector3_1.DistanceTo(LSL.playerPos);
          if (!LSL.attackerAttacking && Game.GameTime > LSL.attackCheckTime + 1000)
          {
            LSL.attackCheckTime = Game.GameTime;
            if ((double) num1 < 60.0 && LsFunctions.CanSee(LSL.attacker, LSL.player.Character))
            {
              int num2 = Function.Call<int>(Hash._0xD24D37CC275948CC, (InputArgument) "HATES_PLAYER");
              Function.Call(Hash._0xC80A74AC829DDD92, (InputArgument) LSL.attacker, (InputArgument) num2);
              LSL.attackerAttacking = true;
              LSL.attacker.Task.ClearAll();
              LSL.attacker.Task.FightAgainst(LSL.player.Character);
            }
            else
            {
              LSL.attacker.Task.ClearAll();
              LSL.attacker.Task.WanderAround();
              LSL.attackerAttacking = false;
            }
          }
        }
      }
      bool flag1 = false;
      if (!LSL.playerHasBag)
      {
        if (!LSL.canSprint)
        {
          LSL.canSprint = true;
          flag1 = true;
        }
        if (LSL.TotalDrugsCarried() / 28 + (LSL.playerWeps.Count - 1) + LSL.player.Money / 50000 > 15)
        {
          LsFunctions.PedBag(LSL.player.Character, true);
          LSL.playerHasBag = true;
        }
      }
      if (LSL.playerHasBag)
      {
        if (!LSL.canSprint)
        {
          if ((Entity) LSL.player.Character.CurrentVehicle != (Entity) null && LSL.player.Character.CurrentVehicle.Model.IsBicycle)
          {
            LSL.canSprint = true;
            flag1 = true;
          }
        }
        else if ((Entity) LSL.player.Character.CurrentVehicle == (Entity) null)
        {
          LSL.canSprint = false;
          flag1 = true;
        }
        if (LSL.TotalDrugsCarried() / 28 + (LSL.playerWeps.Count - 1) + LSL.player.Money / 50000 <= 15)
        {
          LsFunctions.PedBag(LSL.player.Character, false);
          LSL.playerHasBag = false;
        }
      }
      if (flag1)
      {
        if (LSL.canSprint)
          Function.Call(Hash._0xA01B8075D8B92DF4, (InputArgument) LSL.player, (InputArgument) true);
        else
          Function.Call(Hash._0xA01B8075D8B92DF4, (InputArgument) LSL.player, (InputArgument) false);
      }
      if (LSL.player.WantedLevel > 0 && !LSL.playerWanted)
        LSL.playerWanted = true;
      if (LSL.playerWanted && LSL.player.WantedLevel == 0)
        LSL.playerWanted = false;
      if ((Entity) LSL.drugDealer1 != (Entity) null && LSL.drugDealer1.IsDead)
      {
        LsFunctions.RemovePedFromWorld(LSL.drugDealer1, true);
        LSL.drugDealer1 = (Ped) null;
      }
      if ((Entity) LSL.weaponDealer1 != (Entity) null && LSL.weaponDealer1.IsDead)
      {
        LsFunctions.RemovePedFromWorld(LSL.weaponDealer1, true);
        LSL.weaponDealer1 = (Ped) null;
      }
      if (LSL.player.WantedLevel > 0 && LSL.isDealing)
        LSL.isDealing = false;
      if (!LSL.isDealing)
      {
        LSL.jobOffer = false;
        LSL.jobAccepted = false;
        if (!LSL.removedStuff)
        {
          LSL.removedStuff = true;
          if (LSL.customers.Count > 0)
          {
            foreach (Customer customer in LSL.customers)
            {
              if (customer != null)
              {
                foreach (Drug drug in LSL.areas[LSL.areaIndex].Drugs)
                {
                  if (drug.Name == customer.Drug.Name)
                    drug.Supplied = false;
                }
                if (customer.Ped.Exists())
                  LsFunctions.RemovePedFromWorld(customer.Ped, true);
              }
            }
            LSL.customers.Clear();
          }
          if (LSL.jobs.Count > 0)
          {
            foreach (Customer job in LSL.jobs)
            {
              if (job != null && job.Ped.Exists())
              {
                job.Ped.CurrentBlip.ShowRoute = false;
                LsFunctions.RemovePedFromWorld(job.Ped, true);
              }
            }
            LSL.jobs.Clear();
          }
          if (LSL.angryPeds.Count > 0)
          {
            foreach (Ped angryPed in LSL.angryPeds)
            {
              if ((Entity) angryPed != (Entity) null && angryPed.Exists())
                LsFunctions.RemovePedFromWorld(angryPed, true);
            }
            LSL.angryPeds.Clear();
          }
        }
      }
      if (LSL.isDealing)
      {
        int num1 = LSL.TotalDrugsCarried() + LSL.HouseHandler.TotalStashDrugs();
        if (LSL.pStashVehicle != null)
          num1 += LSL.pStashVehicle.TotalDrugs();
        if (LSL.PottentialWorker == null && num1 > 56 && (Game.GameTime > LSL.jobOfferTimer + 20000 && LSL.jobs.ToList<Customer>().Count == 0))
        {
          LSL.jobOfferTimer = Game.GameTime;
          if ((double) LSL.rnd.Next(0, 100) > 70.0 - LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation) * 2.0)
          {
            Vector3 vector3_2 = LSL.GenerateSpawnPos(LSL.playerPos.Around(275f), LSL.Nodetype.Offroad, true);
            if (vector3_2 == Vector3.Zero)
            {
              if (LSL.DEBUG)
                UI.Notify("spawning new job, that one guy was found.");
              vector3_2 = LSL.GenerateSpawnPos(LSL.playerPos.Around(200f), LSL.Nodetype.Offroad, true);
            }
            for (int index = 0; index < 100; ++index)
            {
              if (Function.Call<bool>(Hash._0x125BF4ABFC536B09, (InputArgument) vector3_2.X, (InputArgument) vector3_2.Y, (InputArgument) vector3_2.Z))
                vector3_2 = World.GetNextPositionOnSidewalk(LSL.GenerateSpawnPos(LSL.playerPos.Around(200f), LSL.Nodetype.Offroad, true));
              else
                break;
            }
            List<Ped> getPeds = LSL.lPeds.GetPeds;
            if (getPeds.ToList<Ped>().Count > 0)
            {
              foreach (Ped ped1 in getPeds)
              {
                if (!ped1.IsPlayer && ped1.IsHuman && LSL.rnd.Next(0, 10) > 3)
                {
                  Ped ped2 = World.CreatePed(ped1.Model, vector3_2);
                  if (LSL.DEBUG)
                  {
                    vector3_1 = vector3_2;
                    string str1 = vector3_1.ToString();
                    vector3_1 = ped2.Position;
                    string str2 = vector3_1.ToString();
                    UI.Notify("jobPos:" + str1 + " pedPos:" + str2);
                  }
                  if ((Entity) ped2 != (Entity) null)
                  {
                    ped2.RelationshipGroup = ped1.RelationshipGroup;
                    List<string> stringList = new List<string>();
                    if (LSL.sellingWeed)
                      stringList.Add("Weed");
                    if (LSL.sellingCrack)
                      stringList.Add("Crack");
                    if (LSL.sellingCocaine)
                      stringList.Add("Cocaine");
                    Drug drug = (Drug) null;
                    string str = stringList[LSL.rnd.Next(0, stringList.Count)];
                    if (!(str == "Weed"))
                    {
                      if (!(str == "Crack"))
                      {
                        if (str == "Cocaine")
                          drug = LSL.areas[LSL.areaIndex].Drugs.Find((Predicate<Drug>) (d => d.Name == "Cocaine"));
                      }
                      else
                        drug = LSL.areas[LSL.areaIndex].Drugs.Find((Predicate<Drug>) (d => d.Name == "Crack"));
                    }
                    else
                      drug = LSL.areas[LSL.areaIndex].Drugs.Find((Predicate<Drug>) (d => d.Name == "Weed"));
                    LSL.jobs.Add(new Customer(ped2, vector3_2, true, (Vehicle) null, drug, (int) LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation)));
                    if (LSL.jobs.Count > 0)
                    {
                      Customer job = LSL.jobs[0];
                      float num2 = (float) (job.amount / 7);
                      int _money = job.amount * job.Drug.PricePerG;
                      if ((double) num2 >= 1.0)
                        _money -= (int) num2 * job.Drug.PricePerG;
                      LsFunctions.TextMsg("Drop", job.Ped, "Text message", "Can you do ~g~" + LsFunctions.GramsToOz(job.amount) + "~w~ of ~g~" + job.Drug.Name + " ~w~for $" + LsFunctions.IntToMoney(_money) + " to ~y~" + World.GetStreetName(job.Ped.Position) + " ~w~in ~y~" + World.GetZoneName(job.Ped.Position) + "~w~?");
                      LSL.jobAcceptTimer = Game.GameTime;
                      LSL.jobOffer = true;
                      break;
                    }
                  }
                }
              }
            }
          }
        }
      }
      if (!((Entity) LSL.weaponDealer1 != (Entity) null) || !LSL.weaponDealer1.IsAlive)
        return;
      float distance = World.GetDistance(LSL.weaponDealer1.Position, LSL.playerPos);
      if ((double) distance > 15.0 && LSL.weaponDealer1.CurrentPedGroup != LSL.player.Character.CurrentPedGroup)
      {
        if (Game.GameTime > LSL.dealerTimer + 2000 && LSL.playerPos != LSL.pedTarget)
        {
          LSL.dealerTimer = Game.GameTime;
          LSL.pedTarget = LSL.playerPos;
          LSL.weaponDealer1.Task.GoTo((Entity) LSL.player.Character);
        }
      }
      else if (LSL.weaponDealer1.CurrentPedGroup != LSL.player.Character.CurrentPedGroup)
      {
        LSL.weaponDealer1.Task.ClearAll();
        Function.Call(Hash._0x9F3480FE65DB31B5, (InputArgument) LSL.weaponDealer1.Handle, (InputArgument) LSL.playerGroup);
      }
      if (!LSL.jobOffer && LSL.weaponDealer1.IsStopped && ((double) distance <= 3.0 && LSL.player.Character.IsStopped) && (LsFunctions.IsCustomerNear() && LSL.PottentialWorker == null))
      {
        if (LSL.lPeds.GetPeds.FindAll((Predicate<Ped>) (r => (double) r.Position.DistanceTo(LSL.playerPos) < 10.0)).Count<Ped>() - this.PedsInPlayersGroup() > 3)
        {
          if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 5000.0)
          {
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("Find some where quiter.");
          }
          LSL.readyToDeal = false;
          if (LSL.wepMenu.Visible)
            LSL.wepMenu.Visible = !LSL.wepMenu.Visible;
        }
        else if (!LSL.readyToDeal && !LSL.LsMenuPool.IsAnyMenuOpen())
        {
          if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 5000.0)
          {
            LSL.displayHelpTimer = (float) Game.GameTime;
            LsFunctions.DisplayHelpText("Press ~INPUT_CONTEXT~ to interact");
          }
          LSL.readyToDeal = true;
        }
      }
      else
        LSL.readyToDeal = false;
      if (!((Entity) LSL.weaponDealer1 != (Entity) null) || LSL.weaponDealer1.IsAlive)
        return;
      LSL.weaponDealer1.IsPersistent = false;
      LSL.weaponDealer1.CurrentBlip.Remove();
      LSL.weaponDealer1.MarkAsNoLongerNeeded();
      LsFunctions.SendTextMsg("Dealer is dead!");
      LSL.weaponDealer1 = (Ped) null;
      LSL.readyToDeal = false;
    }

    private static void HandleOldCutomers() => OldCustomerHandler.OnTick();

    private static void DropOffers()
    {
      if (LSL.jobs.Count <= 0 || LSL.jobs[0] == null || LSL.LsMenuPool.IsAnyMenuOpen())
        return;
      if (LSL.jobOffer)
      {
        if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0 && !LSL.LsMenuPool.IsAnyMenuOpen())
        {
          Function.Call(Hash._0x6178F68A87A4D3A0);
          LSL.displayHelpTimer = (float) Game.GameTime;
          LsFunctions.DisplayHelpText("~INPUT_CONTEXT_SECONDARY~ to reject or ~INPUT_CONTEXT~ to accept");
        }
        if (Game.IsControlJustPressed(2, GTA.Control.ContextSecondary) && !LSL.LsMenuPool.IsAnyMenuOpen())
        {
          Function.Call(Hash._0x6178F68A87A4D3A0);
          LSL.jobOffer = false;
          if ((Entity) LSL.jobs[0].Ped != (Entity) null)
          {
            LsFunctions.TextMsg("Drop", LSL.jobs[0].Ped, "Text message", "ok ill let you know if i want anymore");
            Function.Call(Hash._0x6178F68A87A4D3A0);
            LsFunctions.RemovePedFromWorld(LSL.jobs[0].Ped, true);
            LSL.jobs.Clear();
          }
        }
        if (Game.IsControlJustPressed(2, GTA.Control.Context) && !LSL.LsMenuPool.IsAnyMenuOpen())
        {
          Function.Call(Hash._0x6178F68A87A4D3A0);
          LSL.jobOffer = false;
          LSL.jobs[0].Ped.CurrentBlip.IsShortRange = false;
          LSL.jobs[0].Ped.CurrentBlip.ShowRoute = true;
          LSL.jobAccepted = true;
          LsFunctions.TextMsg("Drop", LSL.jobs[0].Ped, "Text message", "See you there");
          if (LSL.PlayerInventory[LSL.jobs[0].Drug.Name] >= LSL.jobs[0].amount)
            UI.ShowSubtitle("Deliver " + LsFunctions.GramsToOz(LSL.jobs[0].amount) + " of ~g~" + LSL.jobs[0].Drug.Name + "~w~ to ~y~" + World.GetStreetName(LSL.jobs[0].Ped.Position) + ".", 10000);
          else
            UI.ShowSubtitle("You do not have enough ~g~" + LSL.jobs[0].Drug.Name + "~w~ go get ~g~" + LsFunctions.GramsToOz(LSL.jobs[0].amount) + "~w~ and deliver it to ~y~" + World.GetStreetName(LSL.jobs[0].Ped.Position) + ".", 10000);
        }
        if (Game.GameTime > LSL.jobAcceptTimer + 15000)
        {
          LsFunctions.TextMsg("Drop", LSL.jobs[0].Ped, "Text message", "i got someone dont worry");
          if ((double) LSL.areas[LSL.areaIndex].Reputation > 0.0)
          {
            float num = LSL.areas[LSL.areaIndex].Reputation * 0.1f;
            LSL.areas[LSL.areaIndex].Reputation -= num;
          }
          Function.Call(Hash._0x6178F68A87A4D3A0);
          LSL.jobOffer = false;
          if ((Entity) LSL.jobs[0].Ped != (Entity) null)
          {
            LsFunctions.RemovePedFromWorld(LSL.jobs[0].Ped, true);
            LSL.jobs.Clear();
          }
        }
      }
      if (LSL.jobs.Count <= 0 || !LSL.jobAccepted)
        return;
      foreach (Customer job in LSL.jobs)
      {
        if (job != null)
        {
          if (job.Ped.CurrentPedGroup != LSL.player.Character.CurrentPedGroup && (double) job.Ped.Position.DistanceTo(LSL.playerPos) <= 10.0)
          {
            job.Ped.Task.ClearAll();
            Function.Call(Hash._0x9F3480FE65DB31B5, (InputArgument) job.Ped.Handle, (InputArgument) LSL.playerGroup);
            job.Ped.CurrentBlip.ShowRoute = false;
          }
          if (!LSL.LsMenuPool.IsAnyMenuOpen() && (double) job.Ped.Position.DistanceTo(LSL.playerPos) <= 2.0)
          {
            if (LSL.player.Character.IsInVehicle() && job.Ped.IsInVehicle() && (double) LSL.player.Character.CurrentVehicle.Speed < 1.0)
            {
              if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0)
              {
                Function.Call(Hash._0x6178F68A87A4D3A0);
                LSL.displayHelpTimer = (float) Game.GameTime;
                LsFunctions.DisplayHelpText("~INPUT_SPRINT~ to sell to this person.");
              }
              if (Game.IsControlJustPressed(2, GTA.Control.Sprint) && !LSL.LsMenuPool.IsAnyMenuOpen())
              {
                Function.Call(Hash._0x6178F68A87A4D3A0);
                LSL.customerSellTo = job;
                LSL.sell.Text = "Sell " + LsFunctions.GramsToOz(job.amount) + " of " + job.Drug.Name + "?";
                LSL.customerMainMenu.Subtitle.Caption = "You have " + LsFunctions.GramsToOz(LSL.PlayerInventory[job.Drug.Name]) + " of " + job.Drug.Name;
                LSL.customerMainMenu.Visible = !LSL.customerMainMenu.Visible;
                LSL.customerMainMenu.CurrentSelection = 0;
                break;
              }
            }
            if (!LSL.player.Character.IsInVehicle() && LSL.player.Character.IsStopped && !LSL.player.Character.IsInCombat)
            {
              if ((double) Game.GameTime > (double) LSL.displayHelpTimer + 3000.0)
              {
                Function.Call(Hash._0x6178F68A87A4D3A0);
                LSL.displayHelpTimer = (float) Game.GameTime;
                LsFunctions.DisplayHelpText("Press ~INPUT_SPRINT~ to sell to this person.");
              }
              if (Game.IsControlJustPressed(2, GTA.Control.Sprint) && !LSL.LsMenuPool.IsAnyMenuOpen())
              {
                Function.Call(Hash._0x6178F68A87A4D3A0);
                LSL.customerSellTo = job;
                LSL.sell.Text = "Sell " + LsFunctions.GramsToOz(job.amount) + " of " + job.Drug.Name + "?";
                LSL.customerMainMenu.Subtitle.Caption = "You have " + LsFunctions.GramsToOz(LSL.PlayerInventory[job.Drug.Name]) + " of " + job.Drug.Name;
                LSL.customerMainMenu.Visible = !LSL.customerMainMenu.Visible;
                LSL.customerMainMenu.CurrentSelection = 0;
                break;
              }
            }
          }
        }
      }
    }

    private void HandlePotentialWorker()
    {
      if (LSL.PottentialWorker == null)
        return;
      LSL.PottentialWorker.OnTick();
      if (!LSL.PottentialWorker.Destroy)
        return;
      LSL.PottentialWorker = (HirePed) null;
    }

    private static void LoadAreaData()
    {
      foreach (XElement descendant in XDocument.Load("scripts\\LSLife\\LSLifeAreas.xml").Descendants())
      {
        if (descendant.Name == (XName) "area")
        {
          string str = "";
          int result1 = -1;
          int result2 = -1;
          int result3 = -1;
          float result4 = -1f;
          foreach (XElement element in descendant.Elements())
          {
            if (element.Name == (XName) "name")
              str = element.Value;
            if (element.Name == (XName) "cStr")
              int.TryParse(element.Value, out result1);
            if (element.Name == (XName) "gStr")
              int.TryParse(element.Value, out result2);
            if (element.Name == (XName) "aHeat")
              int.TryParse(element.Value, out result3);
            if (element.Name == (XName) "aRep")
              float.TryParse(element.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result4);
          }
          if (str != "" && result1 != -1 && result2 != -1)
          {
            LSL.AreaType _type = LSL.AreaType.Normal;
            if (LSL.zoneData.ContainsKey(str))
              _type = LSL.zoneData[str].Item1;
            else
              UI.Notify("~r~LsLife - Error getting area type for Area " + str);
            LSL.areas.Add(new Area(str, LsFunctions.LsDrugs(), _type, result1, result2, result3, result4));
          }
        }
      }
    }

    private void SavedDetected()
    {
      int num1 = Function.Call<bool>(Hash._0x69240733738C19A0) ? 1 : 0;
      bool flag1 = UI.IsHudComponentActive(HudComponent.Saving);
      bool flag2 = Function.Call<bool>(Hash._0xBBB6AD006F1BBEA3);
      bool flag3 = Function.Call<bool>(Hash._0xB0034A223497FFCB);
      bool flag4 = Function.Call<bool>(Hash._0xEA2F2061875EED90);
      bool flag5 = Function.Call<bool>(Hash._0xB2A592B04648A9CB);
      int num2 = flag1 ? 1 : 0;
      if ((num1 | num2 | (flag2 ? 1 : 0) | (flag3 ? 1 : 0) | (flag4 ? 1 : 0) | (flag5 ? 1 : 0)) != 0)
        this.pSaved = true;
      string caption = this.pSaved.ToString();
      Size screenResolution = Game.ScreenResolution;
      int x = screenResolution.Width / 2;
      screenResolution = Game.ScreenResolution;
      int y = screenResolution.Height / 2;
      Point position = new Point(x, y);
      new UIText(caption, position, 1f).Draw();
    }

    private Customer FindClosestCustomer()
    {
      float num1 = 100f;
      Customer customer1 = (Customer) null;
      foreach (Customer customer2 in LSL.customers.Where<Customer>((Func<Customer, bool>) (r => r.Ped.CurrentPedGroup == LSL.playerGroup)))
      {
        float num2 = customer2.Ped.Position.DistanceTo(LSL.playerPos + LSL.player.Character.ForwardVector);
        if ((double) num2 < 3.0 && (double) num2 < (double) num1)
        {
          num1 = num2;
          customer1 = customer2;
        }
      }
      return customer1;
    }

    private Customer FindClosestCustomerInVehicle()
    {
      float num1 = 100f;
      Customer customer1 = (Customer) null;
      foreach (Customer customer2 in LSL.customers.Where<Customer>((Func<Customer, bool>) (r => r.Ped.CurrentPedGroup == LSL.playerGroup && (Entity) r.Ped.CurrentVehicle == (Entity) LSL.player.Character.CurrentVehicle)))
      {
        float num2 = customer2.Ped.Position.DistanceTo(LSL.playerPos);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          customer1 = customer2;
        }
      }
      return customer1;
    }

    private void areaInfoUI()
    {
      UIContainer uiContainer = new UIContainer(new Point(10, 10), new Size(80, 184), Color.FromArgb(100, Color.Black));
      UIText uiText1 = new UIText("--AREA--", new Point(2, 2), 0.2f);
      UIText uiText2 = new UIText("name:" + LSL.areas[LSL.areaIndex].Name, new Point(2, 12), 0.2f);
      UIText uiText3 = new UIText("type:" + LSL.areas[LSL.areaIndex].AreaType.ToString(), new Point(2, 22), 0.2f);
      UIText uiText4 = new UIText("cStr:" + LSL.areas[LSL.areaIndex].CopPresance.ToString(), new Point(2, 32), 0.2f);
      UIText uiText5 = new UIText("gStr:" + LSL.areas[LSL.areaIndex].GangPresance.ToString(), new Point(2, 42), 0.2f);
      UIText uiText6 = new UIText("heat:" + LSL.areas[LSL.areaIndex].Heat.ToString(), new Point(2, 52), 0.2f);
      UIText uiText7 = new UIText("nDealer:" + LSL.areas[LSL.areaIndex].Dealers().ToString(), new Point(2, 62), 0.2f);
      UIText uiText8 = new UIText("dMet:" + LSL.areas[LSL.areaIndex].DemandMet().ToString(), new Point(2, 72), 0.2f);
      UIText uiText9 = new UIText("aRep:" + LSL.areas[LSL.areaIndex].Reputation.ToString() + " Lvl :" + ((int) LsFunctions.CurrentRepLvl((double) LSL.areas[LSL.areaIndex].Reputation)).ToString(), new Point(2, 82), 0.2f);
      UIText uiText10 = new UIText("--" + LSL.areas[LSL.areaIndex].Drugs[0].Name + "--", new Point(2, 92), 0.2f);
      UIText uiText11 = new UIText("Dem:" + LSL.areas[LSL.areaIndex].Drugs[0].Demand.ToString(), new Point(2, 102), 0.2f);
      UIText uiText12 = new UIText("Sup:" + LSL.areas[LSL.areaIndex].Drugs[0].Supplied.ToString(), new Point(2, 112), 0.2f);
      UIText uiText13 = new UIText("--" + LSL.areas[LSL.areaIndex].Drugs[1].Name + "--", new Point(2, 122), 0.2f);
      UIText uiText14 = new UIText("Dem:" + LSL.areas[LSL.areaIndex].Drugs[1].Demand.ToString(), new Point(2, 132), 0.2f);
      UIText uiText15 = new UIText("Sup:" + LSL.areas[LSL.areaIndex].Drugs[1].Supplied.ToString(), new Point(2, 142), 0.2f);
      UIText uiText16 = new UIText("--" + LSL.areas[LSL.areaIndex].Drugs[2].Name + "--", new Point(2, 152), 0.2f);
      UIText uiText17 = new UIText("Dem:" + LSL.areas[LSL.areaIndex].Drugs[2].Demand.ToString(), new Point(2, 162), 0.2f);
      UIText uiText18 = new UIText("Sup:" + LSL.areas[LSL.areaIndex].Drugs[2].Supplied.ToString(), new Point(2, 172), 0.2f);
      uiContainer.Items.Add((UIElement) uiText1);
      uiContainer.Items.Add((UIElement) uiText2);
      uiContainer.Items.Add((UIElement) uiText3);
      uiContainer.Items.Add((UIElement) uiText4);
      uiContainer.Items.Add((UIElement) uiText5);
      uiContainer.Items.Add((UIElement) uiText6);
      uiContainer.Items.Add((UIElement) uiText7);
      uiContainer.Items.Add((UIElement) uiText8);
      uiContainer.Items.Add((UIElement) uiText9);
      uiContainer.Items.Add((UIElement) uiText10);
      uiContainer.Items.Add((UIElement) uiText11);
      uiContainer.Items.Add((UIElement) uiText12);
      uiContainer.Items.Add((UIElement) uiText13);
      uiContainer.Items.Add((UIElement) uiText14);
      uiContainer.Items.Add((UIElement) uiText15);
      uiContainer.Items.Add((UIElement) uiText16);
      uiContainer.Items.Add((UIElement) uiText17);
      uiContainer.Items.Add((UIElement) uiText18);
      uiContainer.Draw();
    }

    private void ZeeInfoUI()
    {
      UIContainer uiContainer = new UIContainer(new Point(10, 194), new Size(80, 104), Color.FromArgb(100, Color.Black));
      UIText uiText1 = new UIText("--Zee--", new Point(2, 2), 0.2f);
      UIText uiText2 = new UIText("money:" + LSL.dealer1Money.ToString(), new Point(2, 12), 0.2f);
      UIText uiText3 = new UIText("reload:" + LSL.dealer1ReloadOrder.ToString(), new Point(2, 22), 0.2f);
      UIText uiText4 = new UIText("reloaded:" + LSL.dealer1Reloaded.ToString(), new Point(2, 32), 0.2f);
      UIText uiText5 = new UIText("weed:" + LSL.dealer1Weed.ToString(), new Point(2, 42), 0.2f);
      UIText uiText6 = new UIText("crack:" + LSL.dealer1Crack.ToString(), new Point(2, 52), 0.2f);
      UIText uiText7 = new UIText("coke:" + LSL.dealer1Cocain.ToString(), new Point(2, 62), 0.2f);
      uiContainer.Items.Add((UIElement) uiText1);
      uiContainer.Items.Add((UIElement) uiText2);
      uiContainer.Items.Add((UIElement) uiText3);
      uiContainer.Items.Add((UIElement) uiText4);
      uiContainer.Items.Add((UIElement) uiText5);
      uiContainer.Items.Add((UIElement) uiText6);
      uiContainer.Items.Add((UIElement) uiText7);
      uiContainer.Draw();
    }

    private void PlayerUI()
    {
      UIContainer uiContainer = new UIContainer(new Point(10, 294), new Size(80, 100), Color.FromArgb(100, Color.Black));
      UIText uiText1 = new UIText("--Player--", new Point(2, 2), 0.2f);
      UIText uiText2 = new UIText("heat:" + LSL.ratChance.ToString(), new Point(2, 12), 0.2f);
      UIText uiText3 = new UIText("dealing:" + LSL.isDealing.ToString(), new Point(2, 22), 0.2f);
      UIText uiText4 = new UIText("tPeds:" + LSL.lPeds.GetPeds.Count.ToString(), new Point(2, 32), 0.2f);
      UIText uiText5 = new UIText("oPeds:" + OldCustomerHandler.AmountOf().ToString(), new Point(2, 42), 0.2f);
      uiContainer.Items.Add((UIElement) uiText1);
      uiContainer.Items.Add((UIElement) uiText2);
      uiContainer.Items.Add((UIElement) uiText3);
      uiContainer.Items.Add((UIElement) uiText4);
      uiContainer.Items.Add((UIElement) uiText5);
      if (LSL.wepDealer != null)
      {
        UIText uiText6 = new UIText("dSpeed:" + LSL.wepDealer.maxSpeed.ToString() + "|" + LSL.wepDealer.ped.CurrentVehicle.Speed.ToString(), new Point(2, 52), 0.2f);
        uiContainer.Items.Add((UIElement) uiText6);
        UIText uiText7 = new UIText("cSpeed:" + LSL.wepDealer.canSpeed.ToString(), new Point(2, 62), 0.2f);
        uiContainer.Items.Add((UIElement) uiText7);
        UIText uiText8 = new UIText("sSpeed:" + LSL.wepDealer.shouldSpeed.ToString(), new Point(2, 72), 0.2f);
        uiContainer.Items.Add((UIElement) uiText8);
      }
      int count = LSL.driveByHandler.GetDriveBies.Count;
      if (count > 0)
      {
        UIText uiText6 = new UIText("drvAmount:" + count.ToString(), new Point(2, 82), 0.2f);
        uiContainer.Items.Add((UIElement) uiText6);
      }
      uiContainer.Draw();
    }

    private static void HandlePigs()
    {
      if (LSL.pigs.Count <= 0)
        return;
      foreach (Pig pig in LSL.pigs.ToList<Pig>())
      {
        if ((Entity) pig.Ped != (Entity) null && pig.Ped.Exists())
        {
          if (!pig.Ped.IsAlive || ((double) pig.Ped.Position.DistanceTo(LSL.playerPos) > 1050.0 || LSL.ratChance == -100))
          {
            pig.DeletePig();
            if (LSL.DEBUG)
              UI.Notify("Removed spawned Cop");
          }
          pig.DoState();
          pig.CheckState();
        }
        else
          LSL.pigs.Remove(pig);
      }
    }

    private static void SpawnPigs()
    {
      Vehicle vehicle = World.CreateVehicle((Model) Enums.GetPoliceVehType(), LSL.GenerateSpawnPos(LSL.playerPos.Around(200f), LSL.Nodetype.Road, false));
      if ((Entity) vehicle != (Entity) null)
      {
        if (LSL.DEBUG)
        {
          UI.Notify("COP SPAWNED");
          vehicle.AddBlip();
          vehicle.CurrentBlip.Name = "Police Car";
        }
        Function.Call(Hash._0xF4924635A19EB37D, (InputArgument) vehicle.Handle, (InputArgument) true);
        LSL.pigs.Add(new Pig(vehicle.CreatePedOnSeat(VehicleSeat.Driver, (Model) Enums.GetPolicePedType()), true));
        LSL.pigs.Add(new Pig(vehicle.CreatePedOnSeat(VehicleSeat.Passenger, (Model) Enums.GetPolicePedType()), false));
      }
      if (LSL.pigs.Count <= 0)
        return;
      foreach (Pig pig in LSL.pigs)
      {
        if ((Entity) pig.Ped != (Entity) null)
        {
          Function.Call(Hash._0xBB03C38DD3FB7FFD, (InputArgument) pig.Ped, (InputArgument) true);
          if (LSL.areas[LSL.areaIndex].CopPresance < 10)
            ++LSL.areas[LSL.areaIndex].CopPresance;
        }
      }
    }

    private void onKeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == LSL.SetDrugCar && LSL.player.Character.IsAlive)
      {
        if (LSL.player.Character.IsInVehicle())
        {
          if (LSL.pStashVehicle != null && (Entity) LSL.pStashVehicle.drugCar != (Entity) null)
          {
            LsFunctions.SendTextMsg(LSL.pStashVehicle.drugCar.DisplayName + " is no longer a stash Vehicle.");
            LSL.pStashVehicle.RemoveOldCar();
            LSL.pStashVehicle = (StashVehicle) null;
          }
          else if (LSL.player.Character.CurrentVehicle.Model.IsCar || LSL.player.Character.CurrentVehicle.Model.IsBike || (LSL.player.Character.CurrentVehicle.Model.IsBoat || LSL.player.Character.CurrentVehicle.Model.IsHelicopter) || LSL.player.Character.CurrentVehicle.Model.IsPlane)
          {
            LSL.pStashVehicle = new StashVehicle();
            LSL.pStashVehicle.VehicleSave(LSL.player.Character.CurrentVehicle);
            if ((Entity) LSL.pStashVehicle.drugCar != (Entity) null)
              LsFunctions.SendTextMsg(LSL.pStashVehicle.drugCar.DisplayName + " is now youre Stash Vehicle.");
          }
          else
            LsFunctions.SendTextMsg("You can not use this vehicle as a stash.");
        }
        else
        {
          LsFunctions.SendTextMsg("Please ener a vehicle before trying to set a stash vehicle.");
          DropDrugs.Drop();
        }
      }
      if (e.KeyCode == LSL.StopDealing && LSL.player.Character.IsAlive)
      {
        if (LSL.isDealing)
        {
          LSL.isDealing = false;
          LsFunctions.SendTextMsg("Street dealing cancel");
        }
        else if (!LSL.isDealing)
          LsFunctions.SendTextMsg("Street dealing is not in progress.");
      }
      if (e.KeyCode == Keys.J && !LSL.started)
      {
        if (LSL.DEBUG)
        {
          if (LSL.player.Character.IsInVehicle())
          {
            LsFunctions.PlayTheSound();
            Vector3 position = LSL.player.Character.CurrentVehicle.Position;
            float heading = LSL.player.Character.CurrentVehicle.Heading;
            XDocument xdocument = XDocument.Load("scripts\\LSLife\\LSLife_Spawns.xml");
            XElement xelement = new XElement((XName) "pos");
            xelement.Add((object) new XElement((XName) "x", (object) position.X));
            xelement.Add((object) new XElement((XName) "y", (object) position.Y));
            xelement.Add((object) new XElement((XName) "z", (object) position.Z));
            xelement.Add((object) new XElement((XName) "h", (object) heading));
            xdocument.Root.Add((object) xelement);
            xdocument.Save("scripts\\LSLife\\LSLife_Spawns.xml");
            UI.Notify("position added to xml");
          }
          else
            UI.Notify("you must be in a vehicle to add a position");
        }
        if (LSL.DEBUG)
          LSL.player.Character.IsInVehicle();
      }
      if (e.KeyCode != LSL.MenuKey || !(LSL.player != (Player) null) || !LSL.player.IsAlive)
        return;
      if (LSL.DEBUG && (Entity) LSL.player.Character != (Entity) null)
      {
        string toPaste = LSL.playerPos.X.ToString() + "f, " + LSL.playerPos.Y.ToString() + "f, " + LSL.playerPos.Z.ToString() + "f";
        Thread thread = new Thread((ThreadStart) (() => Clipboard.SetText(toPaste)));
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        thread.Abort();
      }
      this.showInventory = !this.showInventory;
      Game.PlaySound("Text_Arrive_Tone", "Phone_SoundSet_Default");
    }

    public static void callDealer(bool wepdealer)
    {
      if (wepdealer)
      {
        LSL.weaponDealer1 = World.CreatePed((Model) PedHash.ArmBoss01GMM, LSL.GenerateSpawnPos(World.GetNextPositionOnSidewalk(LSL.GenerateSpawnPos(LSL.playerPos.Around(60f), LSL.Nodetype.AnyRoad, true)), LSL.Nodetype.AnyRoad, true));
        while ((Entity) LSL.weaponDealer1 == (Entity) null)
          Script.Wait(0);
        LSL.pedTarget = LSL.playerPos;
        if ((Entity) LSL.weaponDealer1 != (Entity) null)
        {
          LSL.weaponDealer1.RelationshipGroup = LSL.playerRelationship;
          LsFunctions.AddBlip((Entity) LSL.weaponDealer1, BlipColor.Blue, "Zee's Dealer", true, true);
          LSL.weaponDealer1.CanSwitchWeapons = true;
          LSL.weaponDealer1.Weapons.Give(WeaponHash.Pistol, 9999, false, true);
          LSL.weaponDealer1.Armor = 100;
          Function.Call(Hash._0xC7622C0D36B2FDA8, (InputArgument) LSL.weaponDealer1, (InputArgument) 100);
          LSL.readyToDeal = false;
          Script.Wait(100);
          LSL.weaponDealer1.Task.GoTo(LSL.pedTarget);
          LSL.dealerTimer = Game.GameTime;
          LSL.LSLifeSave = XDocument.Load("scripts\\LSLife\\LSLifeSave.xml");
          foreach (XElement descendant in LSL.LSLifeSave.Descendants())
          {
            if (descendant.Name == (XName) "dealer1")
            {
              foreach (XElement element in descendant.Elements())
              {
                if (element.Name == (XName) "debt")
                  int.TryParse(element.Value, out LSL.dealer1Debt);
                if (element.Name == (XName) "weed")
                  int.TryParse(element.Value, out LSL.dealer1Weed);
                if (element.Name == (XName) "crack")
                  int.TryParse(element.Value, out LSL.dealer1Crack);
                if (element.Name == (XName) "cocain")
                  int.TryParse(element.Value, out LSL.dealer1Cocain);
                if (element.Name == (XName) "money")
                  int.TryParse(element.Value, out LSL.dealer1Money);
                if (element.Name == (XName) "reloaded")
                  bool.TryParse(element.Value, out LSL.dealer1Reloaded);
              }
            }
          }
          LSL.bushWeed.Maximum = LSL.dealer1Weed;
          LSL.crack.Maximum = LSL.dealer1Crack;
          LSL.cocaine.Maximum = LSL.dealer1Cocain;
        }
        else
          LsFunctions.SendTextMsg("~r~Spawning Weapon Dealer Failed");
      }
      else if (LSL.ZeesDealers == null && LSL.NewOrder != null)
      {
        LSL.ZeesDealers = new DrugDealer(LSL.NewOrder);
        if (LSL.ZeesDealers == null)
          return;
        LSL.PlaceOrder.Text = "Cancel Order";
      }
      else
      {
        LsFunctions.TextMsg("ZEE", "Order", "I'll let them know your not coming.");
        LSL.ZeesDealers.Dismiss();
        if (LSL.NewOrder != null)
        {
          LSL.NewOrder.PutBackDrugs();
          LSL.NewOrder.ResetOrder();
        }
        LSL.LsMenuPool.CloseAllMenus();
      }
    }

    public static TaskSequence PerformTaskSeq(
      int index,
      Vehicle vehicle,
      Vector3 target)
    {
      switch (index)
      {
        case 0:
          TaskSequence taskSequence1 = new TaskSequence();
          taskSequence1.AddTask.LeaveVehicle();
          taskSequence1.AddTask.WanderAround();
          return taskSequence1;
        case 1:
          TaskSequence taskSequence2 = new TaskSequence();
          taskSequence2.AddTask.GoTo(vehicle.Position + vehicle.RightVector * 2f * -1f);
          taskSequence2.AddTask.EnterVehicle(vehicle, VehicleSeat.Driver);
          taskSequence2.AddTask.CruiseWithVehicle(vehicle, 20f, 5);
          return taskSequence2;
        case 2:
          Vector3 forwardVector = LSL.player.Character.ForwardVector;
          Vector3 rightVector = LSL.player.Character.RightVector;
          TaskSequence taskSequence3 = new TaskSequence();
          taskSequence3.AddTask.ClearAllImmediately();
          taskSequence3.AddTask.DriveTo(vehicle, target, 10f, 20f, 5);
          if (LSL.player.Character.IsInVehicle())
            taskSequence3.AddTask.GoTo(LSL.playerPos + rightVector * 3f);
          else
            taskSequence3.AddTask.GoTo(LSL.playerPos + forwardVector * 2f);
          return taskSequence3;
        default:
          return (TaskSequence) null;
      }
    }

    public static bool CheckIfPedUsed(Ped _ped, bool _allowPolice, bool _allowVehicle)
    {
      if (!_ped.Exists() || _ped.IsPlayer || !_allowVehicle && _ped.IsInVehicle() || (_ped.Alpha == 0 || !_ped.IsHuman || (_ped.IsDead || !_ped.IsAlive)) || (LsFunctions.IsInInterior((Entity) _ped) || LsFunctions.IsUnderGround((Entity) _ped) || _ped.IsInAir || !_ped.IsInVehicle() && (double) _ped.Position.Z > (double) World.GetGroundHeight(_ped.Position + _ped.ForwardVector * 2f) + 2.0) || (LSL.player.Character.IsInVehicle() && _ped.IsInVehicle() && (Entity) _ped.CurrentVehicle == (Entity) LSL.player.Character.CurrentVehicle || (Game.Player.Character.CurrentPedGroup == _ped.CurrentPedGroup || (Entity) LSL.drugDealer1 != (Entity) null && (Entity) LSL.drugDealer1 == (Entity) _ped) || ((Entity) LSL.weaponDealer1 != (Entity) null && (Entity) LSL.weaponDealer1 == (Entity) _ped || LSL.wepDealer != null && (Entity) LSL.wepDealer.ped == (Entity) _ped || (LSL.customers.Count > 0 && LSL.customers.FindAll((Predicate<Customer>) (r => (Entity) r.Ped == (Entity) _ped)).Count != 0 || OldCustomerHandler.IsPedOldCustomer(_ped)))) || (LSL.angryPeds.Count > 0 && LSL.angryPeds.FindAll((Predicate<Ped>) (r => (Entity) r == (Entity) _ped)).Count != 0 || LSL.pigs.Count > 0 && LSL.pigs.FindAll((Predicate<Pig>) (r => (Entity) r.Ped == (Entity) _ped)).Count != 0 || LSL.jobs.Count > 0 && LSL.jobs.FindAll((Predicate<Customer>) (r => (Entity) r.Ped == (Entity) _ped)).Count != 0 || LSL.aDealer != null && ((Entity) LSL.aDealer.dPed != (Entity) null && (Entity) LSL.aDealer.dPed == (Entity) _ped || (Entity) LSL.aDealer.cPed != (Entity) null && (Entity) LSL.aDealer.cPed == (Entity) _ped || LSL.aDealer.LastCustomer == _ped.Handle)) || (LSL.ZeesDealers != null && ((Entity) LSL.ZeesDealers.lieut == (Entity) _ped || (Entity) LSL.ZeesDealers.goon == (Entity) _ped) || (Entity) LSL.attacker != (Entity) null && (Entity) _ped == (Entity) LSL.attacker))
        return true;
      if (LSL.DealerHandler.dealers.Count > 0)
      {
        foreach (PlayerDealer dealer in LSL.DealerHandler.dealers)
        {
          if (dealer != null && ((Entity) dealer.Ped != (Entity) null && (Entity) dealer.Ped == (Entity) _ped || (Entity) dealer.cPed != (Entity) null && (Entity) dealer.cPed == (Entity) _ped || (Entity) dealer.lPed == (Entity) _ped))
            return true;
        }
      }
      if (LSL.driveByHandler.GetDriveBies.Count > 0)
      {
        foreach (DriveBy getDriveBy in LSL.driveByHandler.GetDriveBies)
        {
          if (getDriveBy.Peds.Contains(_ped))
            return true;
        }
      }
      if (!Function.Call<bool>(Hash._0x47D6F43D77935C75, (InputArgument) _ped))
        return true;
      switch (Function.Call<int>(Hash._0xFF059E1E4C01E63C, (InputArgument) _ped.Handle))
      {
        case 24:
        case 26:
          return true;
        default:
          return !_allowPolice && LsFunctions.IsPolice(_ped);
      }
    }

    public void HandleAreaDealers()
    {
      if (LSL.aDealer == null || LSL.aDealer.NewState != Enums.DStates.TalkToPlayer || (LSL.aDealer.CurrentState != Enums.DStates.TalkToPlayer || !LSL.player.Character.IsStopped) || (double) LSL.aDealer.dPed.Position.DistanceTo(LSL.playerPos) >= 2.0)
        return;
      LSL.aDealer.PromtPlayerToTalk();
    }

    public static void MoveCustomer(Customer c, Vector3 target)
    {
      if (c.Target != target)
        c.Target = target;
      if (!(c.Target == target))
        return;
      if ((double) c.Ped.Position.DistanceTo(c.Target) <= 20.0)
      {
        if (!Function.Call<bool>(Hash._0x125BF4ABFC536B09, (InputArgument) c.Ped.Position.X, (InputArgument) c.Ped.Position.Y, (InputArgument) c.Ped.Position.Z))
        {
          c.Ped.Task.GoTo(c.Target);
          return;
        }
      }
      c.Ped.Task.RunTo(c.Target);
    }

    private static float MaxSpeed() => 0.0f;

    public static Vector3 GenerateSpawnPos(
      Vector3 desiredPos,
      LSL.Nodetype roadtype,
      bool sidewalk)
    {
      Vector3 zero = Vector3.Zero;
      bool flag = false;
      OutputArgument outputArgument = new OutputArgument();
      int num1 = 1;
      int num2 = 0;
      if (roadtype == LSL.Nodetype.AnyRoad)
        num2 = 1;
      if (roadtype == LSL.Nodetype.Road)
        num2 = 0;
      if (roadtype == LSL.Nodetype.Offroad)
      {
        num2 = 1;
        flag = true;
      }
      if (roadtype == LSL.Nodetype.Water)
        num2 = 3;
      int num3 = Function.Call<int>(Hash._0x22D7275A79FE8215, (InputArgument) desiredPos.X, (InputArgument) desiredPos.Y, (InputArgument) desiredPos.Z, (InputArgument) num1, (InputArgument) num2, (InputArgument) 200f, (InputArgument) 200f);
      if (flag)
      {
        while (true)
        {
          if (!Function.Call<bool>(Hash._0x4F5070AA58F69279, (InputArgument) num3))
          {
            num3 = Function.Call<int>(Hash._0x22D7275A79FE8215, (InputArgument) desiredPos.X, (InputArgument) desiredPos.Y, (InputArgument) desiredPos.Z, (InputArgument) num1, (InputArgument) num2, (InputArgument) 200f, (InputArgument) 200f);
            ++num1;
          }
          else
            break;
        }
      }
      Function.Call(Hash._0x703123E5E7D429C2, (InputArgument) num3, (InputArgument) outputArgument);
      Vector3 position = outputArgument.GetResult<Vector3>();
      if (sidewalk)
        position = World.GetNextPositionOnSidewalk(position);
      return position;
    }

    public Vector3 ClosetNodePos(Vector3 pos)
    {
      Vector3 zero = Vector3.Zero;
      int num = Function.Call<int>(Hash._0x22D7275A79FE8215, (InputArgument) pos.X, (InputArgument) pos.Y, (InputArgument) pos.Z, (InputArgument) 1, (InputArgument) 1, (InputArgument) 200f, (InputArgument) 200f);
      OutputArgument outputArgument = new OutputArgument();
      Function.Call(Hash._0x703123E5E7D429C2, (InputArgument) num, (InputArgument) outputArgument);
      return outputArgument.GetResult<Vector3>();
    }

    private int PedsInPlayersGroup() => LSL.player.Character.CurrentPedGroup.ToList(false).Count;

    public static List<LSL.PathnodeFlags> GetRoadFlags(Vector3 pos)
    {
      List<LSL.PathnodeFlags> pathnodeFlagsList = new List<LSL.PathnodeFlags>();
      OutputArgument outputArgument1 = new OutputArgument();
      OutputArgument outputArgument2 = new OutputArgument();
      if (Function.Call<bool>(Hash._0x0568566ACBB5DEDC, (InputArgument) pos.X, (InputArgument) pos.Y, (InputArgument) pos.Z, (InputArgument) outputArgument1, (InputArgument) outputArgument2))
      {
        outputArgument1.GetResult<int>();
        int result = outputArgument2.GetResult<int>();
        foreach (int num in Enum.GetValues(typeof (LSL.PathnodeFlags)).Cast<LSL.PathnodeFlags>())
        {
          if ((num & result) != 0)
            pathnodeFlagsList.Add((LSL.PathnodeFlags) num);
        }
      }
      return pathnodeFlagsList;
    }

    public static bool RoadHasFlag(Vector3 pos, LSL.PathnodeFlags flag)
    {
      OutputArgument outputArgument1 = new OutputArgument();
      OutputArgument outputArgument2 = new OutputArgument();
      if (Function.Call<bool>(Hash._0x0568566ACBB5DEDC, (InputArgument) pos.X, (InputArgument) pos.Y, (InputArgument) pos.Z, (InputArgument) outputArgument1, (InputArgument) outputArgument2))
      {
        outputArgument1.GetResult<int>();
        if (((LSL.PathnodeFlags) outputArgument2.GetResult<int>() & flag) != (LSL.PathnodeFlags) 0)
          return true;
      }
      return false;
    }

    public static bool IntHasFlag(int number, int flag) => (number & flag) != 0;

    private void OnAbort(object sender, EventArgs e)
    {
      if (false)
        return;
      StashHouseHandler houseHandler = LSL.HouseHandler;
      if ((houseHandler != null ? (houseHandler.Houses.Count > 0 ? 1 : 0) : 0) != 0)
      {
        foreach (StashHouse house in LSL.HouseHandler.Houses)
          house?.RemoveBlip();
      }
      if ((Entity) LSL.drugDealer1 != (Entity) null)
      {
        LSL.drugDealer1.CurrentBlip?.Remove();
        LSL.drugDealer1.Delete();
      }
      if ((Entity) LSL.weaponDealer1 != (Entity) null)
      {
        LSL.weaponDealer1.CurrentBlip?.Remove();
        LSL.weaponDealer1.Delete();
      }
      if (LSL.customers.Count >= 1)
      {
        foreach (Customer customer in LSL.customers.ToList<Customer>())
        {
          if (customer != null)
          {
            Ped ped = customer.Ped;
            ped.CurrentBlip?.Remove();
            ped.Delete();
          }
        }
      }
      if (LSL.pStashVehicle != null && (Entity) LSL.pStashVehicle.drugCar != (Entity) null)
      {
        if ((Entity) LSL.pStashVehicle.drugCar != (Entity) null && LSL.pStashVehicle.drugCar.Exists())
        {
          LSL.pStashVehicle.drugCar.CurrentBlip.Remove();
          LSL.pStashVehicle.drugCar.Delete();
        }
        LSL.pStashVehicle.drugCarBlip?.Remove();
      }
      if (LSL.jobs.Count > 0)
      {
        foreach (Customer job in LSL.jobs)
        {
          job.Ped.CurrentBlip.Remove();
          job.Ped.Delete();
        }
      }
      if (LSL.wepDealer != null)
      {
        if ((Entity) LSL.wepDealer.vehicle != (Entity) null)
        {
          LSL.wepDealer.vehicle.CurrentBlip.Remove();
          LSL.wepDealer.vehicle.Delete();
        }
        if ((Entity) LSL.wepDealer.ped != (Entity) null)
          LSL.wepDealer.ped.Delete();
      }
      if (LSL.aDealer != null)
      {
        if ((Entity) LSL.aDealer.dPed != (Entity) null)
        {
          LSL.aDealer.dPed.CurrentBlip?.Remove();
          LSL.aDealer.dPed.Delete();
        }
        if ((Entity) LSL.aDealer.cPed != (Entity) null)
        {
          LSL.aDealer.cPed.CurrentBlip?.Remove();
          LSL.aDealer.cPed.Delete();
        }
      }
      if (LSL.DealerHandler.dealers.Count > 0)
      {
        foreach (PlayerDealer dealer in LSL.DealerHandler.dealers)
        {
          if ((Entity) dealer.Ped != (Entity) null)
          {
            LsFunctions.NotImmuneToPlayer(dealer.Ped);
            dealer.cPed?.CurrentBlip?.Remove();
            dealer.cPed?.Delete();
            dealer.Ped?.CurrentBlip?.Remove();
            dealer.Ped.Delete();
          }
          else
            dealer.RemoveLocationBlip();
        }
      }
      World.RemoveRelationshipGroup(LSL.rival_nutral);
      World.RemoveRelationshipGroup(LSL.rival_enemy);
      World.RemoveRelationshipGroup(LSL.CustomerRel);
      LSL.driveByHandler.CleanDriveBys();
      if (LSL.pigs.Count > 0)
      {
        foreach (Pig pig in LSL.pigs)
        {
          pig.Ped?.CurrentBlip?.Remove();
          pig.Ped?.Delete();
          pig.Vehicle?.CurrentBlip?.Remove();
          pig.Vehicle.Delete();
        }
      }
      LSL.pickupHandler.RemoveBags();
      OldCustomerHandler.ClearOldCustomers();
    }

    public enum PedVariationData
    {
      PED_VARIATION_FACE,
      PED_VARIATION_HEAD,
      PED_VARIATION_HAIR,
      PED_VARIATION_TORSO,
      PED_VARIATION_LEGS,
      PED_VARIATION_HANDS,
      PED_VARIATION_FEET,
      PED_VARIATION_EYES,
      PED_VARIATION_ACCESSORIES,
      PED_VARIATION_TASKS,
      PED_VARIATION_TEXTURES,
      PED_VARIATION_TORSO2,
    }

    public enum AreaType
    {
      Poor,
      Normal,
      Rich,
    }

    public enum jurisdictionType
    {
      LSPD,
      LSSD,
      BCSO,
      SAPR,
    }

    public class Door
    {
      private readonly bool DEBUG = true;
      public Prop Prop;
      private bool Locked;

      public Door(Prop prop)
      {
        this.Prop = prop;
        this.Prop.IsInvincible = true;
        this.UseLock();
      }

      public void UseLock()
      {
        if (this.Locked)
        {
          this.Locked = false;
          Script.Wait(10);
          Function.Call(Hash._0x2F844A8B08D76685, (InputArgument) "BIG_SCORE_GOLD_CAGE_LOCK", (InputArgument) 0);
          Game.PlaySound("0x08053C9C", "BIG_SCORE_GOLD_CAGE_LOCK");
          Function.Call(Hash._0x9B12F9A24FABEDB0, (InputArgument) this.Prop.Model.Hash, (InputArgument) this.Prop.Position.X, (InputArgument) this.Prop.Position.Y, (InputArgument) this.Prop.Position.Z, (InputArgument) false, (InputArgument) 0.0f, (InputArgument) 50f, (InputArgument) 0.0f);
          if (!this.DEBUG)
            return;
          UI.Notify(this.Prop.Model.Hash.ToString() + " Door unlocked " + this.Prop.Position.ToString());
        }
        else
        {
          this.Locked = true;
          Script.Wait(10);
          Game.PlaySound("Bar_Lower_And_Lock", "DLC_IND_ROLLERCOASTER_SOUNDS");
          Function.Call(Hash._0x9B12F9A24FABEDB0, (InputArgument) this.Prop.Model.Hash, (InputArgument) this.Prop.Position.X, (InputArgument) this.Prop.Position.Y, (InputArgument) this.Prop.Position.Z, (InputArgument) true, (InputArgument) 0.0f, (InputArgument) 50f, (InputArgument) 0.0f);
          if (!this.DEBUG)
            return;
          UI.Notify(this.Prop.Model.Hash.ToString() + " Door locked " + this.Prop.Position.ToString());
        }
      }
    }

    public enum Nodetype
    {
      AnyRoad,
      Road,
      Offroad,
      Water,
    }

    public enum PathnodeFlags
    {
      Slow = 1,
      Two = 2,
      Intersection = 4,
      Eight = 8,
      SlowTraffic = 12, // 0x0000000C
      ThirtyTwo = 32, // 0x00000020
      Freeway = 64, // 0x00000040
      FourWayIntersection = 128, // 0x00000080
      BigIntersectionLeft = 512, // 0x00000200
    }
  }
}
