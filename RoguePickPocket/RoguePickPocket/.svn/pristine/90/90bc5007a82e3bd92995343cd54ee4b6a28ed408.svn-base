using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace RoguePickPocket.Helpers {
    public static class Units {
        #region Local declarations
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static WoWUnit CurTar { get { return StyxWoW.Me.CurrentTarget; } }
        #endregion

        public static WoWUnit NearbyUnfriendlyMobs(double maxDistance) {
            return ObjectManager.GetObjectsOfTypeFast<WoWUnit>().FirstOrDefault(u => IsValidTarget(u) && u.Location.Distance(Me.Location) <= maxDistance);
        }

        public static bool IsValidTarget(this WoWUnit u) {
            return u.CanSelect && u.Attackable && u.IsHumanoid && !u.IsFriendly &&
                  !u.IsDead && !u.IsPet && !u.IsNonCombatPet && !u.IsCritter &&
                  !u.Name.Contains("Dummy");
        }
    }
}
