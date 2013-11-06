using System;
using System.Diagnostics;
using Styx;
using Styx.Helpers;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace HunterSpinBuddy {
    public class HunterSpinBuddy : HBPlugin {
        public override string Name { get { return "HunterSpinBuddy"; } }
        public override string Author { get { return "xxxxx"; } }
        public override Version Version { get { return new Version(1, 0, 5, 0); } }
        public override bool WantButton { get { return false; } }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private WoWPoint _Target = WoWPoint.Empty;
        private bool _RunOnce;
        private static readonly Stopwatch MyTimer = new Stopwatch();
        public override void Pulse() {
            switch (Lua.GetReturnVal<int>("return LeapNum", 0)) {
                case 0:
                    break;
                case 1:
                    _Target = GetPointInFrontOfMe(40.0f);
                    if (_Target != WoWPoint.Empty) {
                        // 6544 = Heroic Leap
                        if (SpellManager.CanCast(6544)) {
                            SpellManager.Cast(6544);
                            SpellManager.ClickRemoteLocation(_Target);
                        }
                    }
                    Lua.DoString("LeapNum = 0;");
                    break;
                case 2:
                    _Target = GetPointInFrontOfMe(30.0f);
                    // If you want to throw it at YOUR location then use this line :
                    // _Target = Me.Location;
                    if (_Target != WoWPoint.Empty) {
                        // 114203 = Demoralizing Banner
                        if (SpellManager.CanCast(114203)) {
                            SpellManager.Cast(114203);
                            SpellManager.ClickRemoteLocation(_Target);
                        }
                    }
                    Lua.DoString("LeapNum = 0;");
                    break;
                case 3:
                    if (!_RunOnce) {
                        Me.SetFacing(WoWMathHelper.NormalizeRadian(Me.RenderFacing + (float) Math.PI));
                        _RunOnce = true;
                    }
                    // 781 = Disengage
                    if (!MyTimer.IsRunning) { MyTimer.Restart(); }
                    if (MyTimer.ElapsedMilliseconds <= 25) { return; } 
                    if (SpellManager.CanCast(781)) { SpellManager.Cast(781); }
                    MyTimer.Stop();
                    _RunOnce = false;
                    Lua.DoString("LeapNum = 0;");
                    break;
                case 4:
                    Me.SetFacing(WoWMathHelper.NormalizeRadian(Me.RenderFacing + (float)Math.PI));
                    KeyboardManager.KeyUpDown((char) KeyboardManager.eVirtualKeyMessages.VK_UP);
                    // KeyboardManager.PressKey((char)KeyboardManager.eVirtualKeyMessages.VK_UP);
                    // KeyboardManager.ReleaseKey((char)KeyboardManager.eVirtualKeyMessages.VK_UP);
                    Lua.DoString("LeapNum = 0;");
                    break;
            }
        }
        private static WoWPoint GetPointInFrontOfMe(float range) {
            var a = Me.Location.RayCast(Me.RenderFacing, range);
            return !Navigator.CanNavigateFully(Me.Location, a) ? WoWPoint.Empty : a;
        }
    }
}