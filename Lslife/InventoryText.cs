using GTA;
using NativeUI;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace LSlife
{
  internal static class InventoryText
  {
    public static UIMenu Options;
    public static UIMenuSliderItem InventoryPosX;
    public static UIMenuSliderItem InventoryPosY;
    private static UIMenuItem Save;
    private static int posX = 1900;
    private static int posY = 100;
    private const int MaxX = 1920;
    private const int MaxY = 1080;

    public static void Innit()
    {
      InventoryText.LoadInvConfig();
      InventoryText.Options = LSL.LsMenuPool.AddSubMenu(LSL.zeeMenu, "Options");
      InventoryText.InventoryPosX = new UIMenuSliderItem("PosX: ~g~" + InventoryText.posX.ToString());
      InventoryText.InventoryPosX.Maximum = 1920;
      InventoryText.InventoryPosX.Value = InventoryText.posX;
      InventoryText.InventoryPosY = new UIMenuSliderItem("PosY: ~g~" + InventoryText.posY.ToString());
      InventoryText.InventoryPosY.Maximum = 1080;
      InventoryText.InventoryPosY.Value = InventoryText.posY;
      InventoryText.Save = new UIMenuItem("Save config");
      InventoryText.Options.AddItem((UIMenuItem) InventoryText.InventoryPosX);
      InventoryText.Options.AddItem((UIMenuItem) InventoryText.InventoryPosY);
      InventoryText.Options.AddItem(InventoryText.Save);
      InventoryText.InventoryPosX.OnSliderChanged += new ItemSliderEvent(InventoryText.InventoryPos_OnSliderChanged);
      InventoryText.InventoryPosY.OnSliderChanged += new ItemSliderEvent(InventoryText.InventoryPos_OnSliderChanged);
      InventoryText.Save.Activated += new ItemActivatedEvent(InventoryText.Save_Activated);
    }

    private static void LoadInvConfig()
    {
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "config")
        {
          using (IEnumerator<XElement> enumerator = descendant.Elements().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              XElement current = enumerator.Current;
              if (current.Name == (XName) "invPosX")
                int.TryParse(current.Value, out InventoryText.posX);
              else if (current.Name == (XName) "invPosY")
                int.TryParse(current.Value, out InventoryText.posY);
            }
            break;
          }
        }
      }
    }

    private static void Save_Activated(UIMenu sender, UIMenuItem selectedItem)
    {
      bool flag = false;
      foreach (XElement descendant in LSL.LSLifeSave.Descendants())
      {
        if (descendant.Name == (XName) "config")
        {
          flag = true;
          using (IEnumerator<XElement> enumerator = descendant.Elements().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              XElement current = enumerator.Current;
              if (current.Name == (XName) "invPosX")
                current.Value = InventoryText.posX.ToString();
              else if (current.Name == (XName) "invPosY")
                current.Value = InventoryText.posY.ToString();
            }
            break;
          }
        }
      }
      if (!flag)
      {
        XElement xelement1 = new XElement((XName) "config");
        XElement xelement2 = new XElement((XName) "invPosX");
        xelement2.Value = InventoryText.posX.ToString();
        XElement xelement3 = new XElement((XName) "invPosY");
        xelement3.Value = InventoryText.posY.ToString();
        xelement1.Add((object) xelement2);
        xelement1.Add((object) xelement3);
        LSL.LSLifeSave.Root.Add((object) xelement1);
      }
      LSL.LSLifeSave.Save("scripts\\LSLife\\LSLifeSave.xml");
      LSL.LsMenuPool.CloseAllMenus();
    }

    private static void InventoryPos_OnSliderChanged(UIMenuSliderItem sender, int newIndex)
    {
      if (sender == InventoryText.InventoryPosX)
      {
        InventoryText.posX = sender.Value;
        sender.Text = "PosX: ~g~" + InventoryText.posX.ToString();
      }
      if (sender != InventoryText.InventoryPosY)
        return;
      InventoryText.posY = sender.Value;
      sender.Text = "PosY: ~g~" + InventoryText.posY.ToString();
    }

    public static void InventoryGui()
    {
      Point position = new Point(InventoryText.posX, InventoryText.posY);
      UIResText uiResText1 = new UIResText("-Weed-", position, 0.4f, Color.WhiteSmoke, GTA.Font.Pricedown, UIResText.Alignment.Right);
      uiResText1.DropShadow = true;
      uiResText1.Shadow = true;
      uiResText1.Outline = true;
      UIResText uiResText2 = new UIResText("", new Point(position.X, position.Y + 20), 0.5f, Color.WhiteSmoke, GTA.Font.Pricedown, UIResText.Alignment.Right);
      uiResText2.Enabled = true;
      uiResText2.DropShadow = true;
      uiResText2.Shadow = true;
      uiResText2.Outline = true;
      UIResText uiResText3 = new UIResText("", new Point(position.X, position.Y + 45), 0.5f, Color.WhiteSmoke, GTA.Font.Pricedown, UIResText.Alignment.Right);
      uiResText3.Enabled = true;
      uiResText3.DropShadow = true;
      uiResText3.Shadow = true;
      uiResText3.Outline = true;
      UIResText uiResText4 = new UIResText("-Crack-", new Point(position.X, position.Y + 80), 0.4f, Color.WhiteSmoke, GTA.Font.Pricedown, UIResText.Alignment.Right);
      uiResText4.DropShadow = true;
      uiResText4.Shadow = true;
      uiResText4.Outline = true;
      UIResText uiResText5 = new UIResText("", new Point(position.X, position.Y + 80 + 20), 0.5f, Color.WhiteSmoke, GTA.Font.Pricedown, UIResText.Alignment.Right);
      uiResText5.Enabled = true;
      uiResText5.DropShadow = true;
      uiResText5.Shadow = true;
      uiResText5.Outline = true;
      UIResText uiResText6 = new UIResText("", new Point(position.X, position.Y + 100 + 25), 0.5f, Color.WhiteSmoke, GTA.Font.Pricedown, UIResText.Alignment.Right);
      uiResText6.Enabled = true;
      uiResText6.DropShadow = true;
      uiResText6.Shadow = true;
      uiResText6.Outline = true;
      UIResText uiResText7 = new UIResText("-Coke-", new Point(position.X, position.Y + 160), 0.4f, Color.WhiteSmoke, GTA.Font.Pricedown, UIResText.Alignment.Right);
      uiResText7.DropShadow = true;
      uiResText7.Shadow = true;
      uiResText7.Outline = true;
      UIResText uiResText8 = new UIResText("", new Point(position.X, position.Y + 160 + 20), 0.5f, Color.WhiteSmoke, GTA.Font.Pricedown, UIResText.Alignment.Right);
      uiResText8.Enabled = true;
      uiResText8.DropShadow = true;
      uiResText8.Shadow = true;
      uiResText8.Outline = true;
      UIResText uiResText9 = new UIResText("", new Point(position.X, position.Y + 160 + 20 + 25), 0.5f, Color.WhiteSmoke, GTA.Font.Pricedown, UIResText.Alignment.Right);
      uiResText9.Enabled = true;
      uiResText9.DropShadow = true;
      uiResText9.Shadow = true;
      uiResText9.Outline = true;
      uiResText2.Caption = "~r~" + LsFunctions.pWeedOunce.ToString() + "~s~z";
      uiResText3.Caption = LsFunctions.GramsToOz(LSL.PlayerInventory["Weed"]) ?? "";
      uiResText5.Caption = "~r~" + LsFunctions.pCrackOunce.ToString() + "~s~z";
      uiResText6.Caption = LsFunctions.GramsToOz(LSL.PlayerInventory["Crack"]) ?? "";
      uiResText8.Caption = "~r~" + LsFunctions.pCocaineOunce.ToString() + "~s~z";
      uiResText9.Caption = LsFunctions.GramsToOz(LSL.PlayerInventory["Cocaine"]) ?? "";
      uiResText1.Draw();
      uiResText2.Draw();
      uiResText3.Draw();
      uiResText4.Draw();
      uiResText5.Draw();
      uiResText6.Draw();
      uiResText7.Draw();
      uiResText8.Draw();
      uiResText9.Draw();
      UI.ShowHudComponentThisFrame(HudComponent.Cash);
    }
  }
}
