using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephansStardewCrops
{
    public class Class1 : Mod
    {
        public List<string> turnipData = new List<String>() { "797", "10", "YearOne", "spring" };
        static public StardewValley.Object turnipObject;
        static Dictionary<Item, int[]> itemPriceAndStock;
        static List<Item> forSale;
        static System.Reflection.FieldInfo Stock = typeof(ShopMenu).GetField("itemPriceAndStock", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        static System.Reflection.FieldInfo Sale = typeof(ShopMenu).GetField("forSale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);


        public override void Entry(params object[] objects)
        {
            Log.Verbose("StephansStardewCrops => Initialized");
            MenuEvents.MenuChanged += Event_MenuChanged;
            MenuEvents.MenuClosed += Event_MenuClosed;
            GameEvents.LoadContent += Event_LoadContent;

        }

        static void Event_LoadContent(object sender, EventArgs e)
        {
            turnipObject = new StardewValley.Object(797, 1, false, -1, 0); //Load the object to put in.

        }

        static void Event_MenuChanged(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu is ShopMenu)
            {
                ShopMenu menu = (ShopMenu)Game1.activeClickableMenu;
                    itemPriceAndStock = (Dictionary<Item, int[]>)Stock.GetValue(Game1.activeClickableMenu);
                    forSale = (List<Item>)Sale.GetValue(Game1.activeClickableMenu);
                    // Using Reflection here
                    forSale.Add(turnipObject.getOne());
                    itemPriceAndStock.Add(turnipObject, new int[2] { (turnipObject.salePrice()), 1 } );
                    Stock.SetValue(Game1.activeClickableMenu, itemPriceAndStock);
                    Sale.SetValue(Game1.activeClickableMenu, forSale);
            
            }
        }

        static void Event_MenuClosed(object send, EventArgsClickableMenuClosed e)
        { }

    }
}