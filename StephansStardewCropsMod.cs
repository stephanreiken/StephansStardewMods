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


namespace StephansStardewCrops
{
    public class Class1 : Mod
    {
        static public List<StardewValley.Object> springCropObjects = new List<StardewValley.Object>();
        static public List<StardewValley.Object> summerCropObjects = new List<StardewValley.Object>();
        static public List<StardewValley.Object> fallCropObjects = new List<StardewValley.Object>();
        static public List<int> springCropInts = new List<int>() {797,799,801};
        static public List<int> summerCropInts = new List<int>() {803,805,807};
        static public List<int> fallCropInts = new List<int>() {809,811,813};
        static Dictionary<Item, int[]> itemPriceAndStock;
        static List<Item> forSale;
        static System.Reflection.FieldInfo Stock = typeof(ShopMenu).GetField("itemPriceAndStock", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        static System.Reflection.FieldInfo Sale = typeof(ShopMenu).GetField("forSale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);


        public override void Entry(params object[] objects)
        {
            Log.Verbose("StephansStardewCrops => Initialized");
            MenuEvents.MenuChanged += Event_MenuChanged;
            PlayerEvents.InventoryChanged += Event_InventoryChanged;
            GameEvents.LoadContent += Event_LoadContent;

        }

        static void Event_LoadContent(object sender, EventArgs e)
        {
            foreach (int index in springCropInts)
                springCropObjects.Add( new StardewValley.Object(index, 1, false, -1, 0));
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
                    Log.Verbose("StephansStardewCrops => Should Have Worked!");
                }
                else
                {
                Log.Verbose("StephansStardewCrops => Not Seed Shop");
                }
            }
            else
            {
                Log.Verbose("StephansStardewCrops => Not Shop Menu");
            }
        }

        static void Event_InventoryChanged(object send, EventArgs e)
        {
            for (int c = 0; c < Game1.player.Items.Count; c++)
            {
                if (Game1.player.Items[c].Name == "Vinegar Keg Alpha")
                {
                    Game1.player.Items[c] = new acidBarrel(165, 1, false, -1, 0);
                }
            }
        }
    }
    public class acidBarrel : StardewValley.Object
    {
        public acidBarrel()
            {
            }
        public acidBarrel(int parentSheetIndex, int initialStack, bool isRecipe = false, int price = -1, int quality = 0)
        {
            this.parentSheetIndex = parentSheetIndex;
            this.isRecipe = isRecipe;
            if (price != -1)
                this.price = price;
            this.quality = quality;
        }



        public override bool performToolAction(Tool t)
        {
            if (t == null || !t.isHeavyHitter() || t is MeleeWeapon)
                return base.performToolAction(t);
            if (this.heldObject != null)
                Game1.createItemDebris((Item)this.heldObject, this.tileLocation * (float)Game1.tileSize, -1, (GameLocation)null);
            Game1.soundBank.PlayCue("woodWhack");
            if (this.heldObject == null)
                return true;
            this.heldObject = (StardewValley.Object)null;
            this.minutesUntilReady = -1;
            return false;
        }

        public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
        {
            switch (dropIn.Category)
            {
                case -79:
                    this.heldObject = new StardewValley.Object(Vector2.Zero, 419, dropIn.Name + "Vinegar", false, true, false, false);
                    this.heldObject.Price = dropIn.Price * 2;
                    if (!probe)
                    {
                        this.heldObject.name = dropIn.Name + "Vinegar";
                        Game1.playSound("Ship");
                        Game1.playSound("bubbles");
                        this.minutesUntilReady = 600;
                        who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0.0f, (float)(-Game1.tileSize * 2)), false, false, (float)(((double)this.tileLocation.Y + 1.0) * (double)Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
                        {
                            alphaFade = 0.005f
                        });
                    }
                    return true;
             }
            return false;
        }

        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {
            return base.checkForAction(who, justCheckingForActivity);
        }
    }
}