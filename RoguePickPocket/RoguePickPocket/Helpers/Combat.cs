using System.Collections.Generic;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace RoguePickPocket.Helpers {
    public class Combat {

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

        public static void IncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits) {
            if(StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.Attackable) {
                outgoingUnits.Add(StyxWoW.Me.CurrentTarget);
            }
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

    }
}
