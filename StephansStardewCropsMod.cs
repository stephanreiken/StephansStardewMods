using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Locations;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Entoarox.Framework;


namespace StephansStardewCrops
{
    public class Class1 : Mod
    {

        static public List<StardewValley.Object> springCropObjects = new List<StardewValley.Object>();
        static public List<StardewValley.Object> summerCropObjects = new List<StardewValley.Object>();
        static public List<StardewValley.Object> fallCropObjects = new List<StardewValley.Object>();
        static public List<int> springCropInts = new List<int>() {797,799,801, 821};
        static public List<int> summerCropInts = new List<int>() {803,805,807, 821};
        static public List<int> fallCropInts = new List<int>() {809,811,813};
        static Dictionary<Item, int[]> itemPriceAndStock;
        static List<Item> forSale;
        static System.Reflection.FieldInfo Stock = typeof(ShopMenu).GetField("itemPriceAndStock", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        static System.Reflection.FieldInfo Sale = typeof(ShopMenu).GetField("forSale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);


        public override void Entry(IModHelper helper)
        {
            //Log.Verbose("StephansStardewCrops => Initialized");
            MenuEvents.MenuChanged += Event_MenuChanged;
            foreach (int index in springCropInts)
                springCropObjects.Add(new StardewValley.Object(index, 1, false, -1, 0));
            foreach (int index in summerCropInts)
                summerCropObjects.Add(new StardewValley.Object(index, 1, false, -1, 0));
            foreach (int index in fallCropInts)
                fallCropObjects.Add(new StardewValley.Object(index, 1, false, -1, 0));

        }

        static void Event_MenuChanged(object sender, EventArgs e)
        {

            if (Game1.activeClickableMenu is ShopMenu)
            {
                if (Game1.currentLocation.Name == "SeedShop")
                {
                    ShopMenu menu = (ShopMenu)Game1.activeClickableMenu;
                    itemPriceAndStock = (Dictionary<Item, int[]>)Stock.GetValue(Game1.activeClickableMenu);
                    forSale = (List<Item>)Sale.GetValue(Game1.activeClickableMenu);
                    // Using Reflection here
                    if (Game1.IsSpring)
                    {
                        foreach (StardewValley.Object seedObject in springCropObjects)
                        {
                            forSale.Add(seedObject);
                            itemPriceAndStock.Add(seedObject, new int[2] { (seedObject.salePrice()), StardewValley.Menus.ShopMenu.infiniteStock });
                        }

                    }
                    else if (Game1.IsSummer)
                    {
                        foreach (StardewValley.Object seedObject in summerCropObjects)
                        {
                            forSale.Add(seedObject);
                            itemPriceAndStock.Add(seedObject, new int[2] { (seedObject.salePrice()), StardewValley.Menus.ShopMenu.infiniteStock });
                        }
                    }
                    else if (Game1.IsFall)
                    {
                        foreach (StardewValley.Object seedObject in fallCropObjects)
                        {
                            forSale.Add(seedObject);
                            itemPriceAndStock.Add(seedObject, new int[2] { (seedObject.salePrice()), StardewValley.Menus.ShopMenu.infiniteStock });
                        }
                    }
                    Stock.SetValue(Game1.activeClickableMenu, itemPriceAndStock);
                    Sale.SetValue(Game1.activeClickableMenu, forSale);
             //       Log.Verbose("StephansStardewCrops => Should Have Worked!");
                }
                else
                {
         //       Log.Verbose("StephansStardewCrops => Not Seed Shop");
                }
            }
            else
            {
      //          Log.Verbose("StephansStardewCrops => Not Shop Menu");
            }
        }
    }
}
