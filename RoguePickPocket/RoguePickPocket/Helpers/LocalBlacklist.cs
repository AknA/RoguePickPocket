using System;
using System.Collections.Generic;
using System.Linq;
using Styx.Common.Helpers;
using Styx.WoWInternals.WoWObjects;

namespace RoguePickPocket.Helpers {
    class LocalBlacklist {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        private static readonly Dictionary<ulong, DateTime> BlackList = new Dictionary<ulong, DateTime>();
        private static WaitTimer _sweepTimer;

        // ===========================================================
        // Constructors
        // ===========================================================

        public LocalBlacklist(TimeSpan maxSweepTime) {
            _sweepTimer = new WaitTimer(maxSweepTime) { WaitTime = maxSweepTime };
        }

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        public static void Add(ulong guid, TimeSpan timeSpan) {
            RemoveExpired();
            BlackList[guid] = DateTime.Now.Add(timeSpan);
        }

        public static void Add(WoWObject wowObject, TimeSpan timeSpan) {
            if(wowObject != null) {
                Add(wowObject.Guid, timeSpan);
            }
        }

        public static bool Contains(ulong guid) {
            DateTime expiry;
            if(BlackList.TryGetValue(guid, out expiry)) { return (expiry > DateTime.Now); }
            return false;
        }

        public static bool Contains(WoWObject wowObject)
        {
            return (wowObject != null) && Contains(wowObject.Guid);
        }

        public static void RemoveExpired() {
            if(_sweepTimer.IsFinished) {
                var now = DateTime.Now;
                var expiredEntries = (from key in BlackList.Keys
                                      where (BlackList[key] < now)
                                      select key).ToList();
                foreach(var entry in expiredEntries) { BlackList.Remove(entry); }
                _sweepTimer.Reset();
            }
        }
        public static void RemoveAll() {
            var everything = (from key in BlackList.Keys
                              select key).ToList();
            foreach(var entry in everything) { BlackList.Remove(entry); }
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

    }
}
