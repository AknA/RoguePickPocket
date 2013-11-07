using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals.WoWObjects;

namespace RoguePickPocket.Helpers {
    class LockboxHandler {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        private static WoWItem _lockBox;

        private static bool _isBusy;
        private static ulong _lastLockbox;

        private static readonly HashSet<uint> LockBoxes = new HashSet<uint>
        {
            4632, // Ornate Bronze Lockbox
            6354, // Small Locked Chest
            16882, // Battered Junkbox
            4633, // Heavy Bronze Lockbox
            4634, // Iron Lockbox
            6355, // Sturdy Locked Chest
            16883, // Worn Junkbox
            4636, // Strong Iron Lockbox
            4637, // Steel Lockbox
            16884, // Sturdy Junkbox
            4638, // Reinforced Steel Lockbox
            13875, // Ironbound Locked Chest
            5758, // Mithril Lockbox
            5759, // Thorium Lockbox
            13918, // Reinforced Locked Chest
            5760, // Eternium Lockbox
            12033, // Thaurissan Family Jewels
            29569, // Strong Junkbox
            31952, // Khorium Lockbox
            43575, // Reinforced Junkbox
            43622, // Froststeel Lockbox
            43624, // Titanium Lockbox
            45986, // Tiny Titanium Lockbox
            63349, // Flame-Scarred Junkbox
            68729, // Elementium Lockbox
            88567, // Ghost Iron Lockbox
            88165, // Vine-Cracked Junkbox
        };

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        public static bool IsCastingOrChannelling() {
            return Character.Me.IsCasting || Character.Me.IsChanneling;
        }

        public static WoWItem FindLockedBox() {

            // If your inventory is empty
            if(Character.Me.CarriedItems == null)
                return null;

            var lockbox = Character.Me.CarriedItems.FirstOrDefault(b => b != null
                && b.IsValid 
                && b.ItemInfo != null // If the tooltip shows up
                && b.ItemInfo.ItemClass == WoWItemClass.Miscellaneous // If the item is a misc item
                && b.ItemInfo.ContainerClass == WoWItemContainerClass.Container // if it is a container
                && b.ItemInfo.Level <= Character.Me.Level // Are we high enough level to open 
                && !b.IsOpenable // Box is locked
                && b.Usable // Can right click
                && b.Cooldown <= 0 // Is the item on global cooldown
                && LockBoxes.Contains(b.Entry)); // Do we have the lockbox

            if(lockbox == null) { return null; }

            // Otherwise, return carried items
            return lockbox;
        }

        public static WoWItem FindUnlockedBox() {
            // If your inventory is empty
            if(Character.Me.CarriedItems == null)
                return null;

            var lockbox = Character.Me.CarriedItems.FirstOrDefault(b => b != null 
                && b.IsValid 
                && b.ItemInfo != null
                && b.ItemInfo.ItemClass == WoWItemClass.Miscellaneous
                && b.ItemInfo.ContainerClass == WoWItemContainerClass.Container
                && b.IsOpenable
                && b.Usable
                && b.Cooldown <= 0
                && !Blacklist.Contains(b.Guid, BlacklistFlags.Node)
                && LockBoxes.Contains(b.Entry));

            if (lockbox == null) { return null; }

            // Otherwise, return carried items
            return lockbox;
        }

        public static void PickLock() {
            // Find a locked box
            _lockBox = FindLockedBox();
            RoguePickPocket.CustomLog.CustomNormalLog("PickLock: lockbox to pickpocket guid : " + _lockBox.Guid);

            // If we can cast pick lock
            if(SpellManager.CanCast(1804)) { 
                RoguePickPocket.CustomLog.CustomNormalLog("Picklocking {0} with GUID of {1}.", _lockBox.Name, _lockBox.Guid);
                SpellManager.Cast(1804); // picklock is casted
            }

            // If picklock is not currently casted, then we are done here
            if(Character.Me.CurrentPendingCursorSpell != null) { 
                // Use picklock on the item
                _lockBox.Use();
            }
        }

        public static void OpenLockbox() {

            // If we are in the middle of casting something, we are done here
            if(IsCastingOrChannelling()) { return; }
            RoguePickPocket.CustomLog.CustomNormalLog("OpenLockbox: not IsCastingOrChannelling.");

            // Find an unlocked box
            _lockBox = FindUnlockedBox();
            RoguePickPocket.CustomLog.CustomNormalLog("OpenLockbox: lockbox = " + _lockBox.Guid);

            // If the last lockbox is equal to the same entry as before, then we are done here
            if(_lastLockbox == _lockBox.Guid) { return; }
            RoguePickPocket.CustomLog.CustomNormalLog("OpenLockbox: current lockbox != last lockbox.");

            _lastLockbox = _lockBox.Guid;

            RoguePickPocket.CustomLog.CustomNormalLog("Opening {0}.", _lockBox.Name);
            _lockBox.UseContainerItem();

        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

    }
}
