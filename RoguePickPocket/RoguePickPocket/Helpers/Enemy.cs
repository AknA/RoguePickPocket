using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace RoguePickPocket.Helpers {
    class Enemy {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

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

        public static WoWUnit NearbyUnfriendlyMobs(double maxDistance) {
            return ObjectManager.GetObjectsOfTypeFast<WoWUnit>().FirstOrDefault(u => IsValidTarget(u) && u.Location.Distance(Character.Me.Location) <= maxDistance);
        }

        public static bool IsValidTarget(WoWUnit u) {
            return u.CanSelect && u.Attackable && u.IsHumanoid && !u.IsFriendly &&
                  !u.IsDead && !u.IsPet && !u.IsNonCombatPet && !u.IsCritter &&
                  !u.Name.Contains("Dummy");
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

    }
}
