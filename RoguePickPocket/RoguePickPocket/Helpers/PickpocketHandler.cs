using System.Linq;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace RoguePickPocket.Helpers {
    class PickpocketHandler {

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

        public static bool Ready() {
            if(Character.Me.IsOnTransport && Character.Me.IsGhost && Character.Me.IsDead && Character.Me.Combat &&
                Character.Me.IsActuallyInCombat) {
                return false;
            }

            if(ObjectManager.GetObjectsOfType<WoWUnit>().Any(unit => unit.Aggro || unit.PetAggro)) {
                return false;
            }

            return true;
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

    }
}
