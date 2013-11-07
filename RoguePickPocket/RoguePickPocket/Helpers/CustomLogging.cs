using System.Windows.Media;
using Styx.Common;

namespace RoguePickPocket.Helpers {
    public class CustomLogging {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        private readonly string _nickname;

        // ===========================================================
        // Constructors
        // ===========================================================

        public CustomLogging()
        {
            
        }

        public CustomLogging(string pNickname)
        {
            _nickname = pNickname;
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

        public void CustomNormalLog(string message, params object[] args)
        {
            Logging.Write(Colors.DeepSkyBlue, "[" + _nickname + "]: " + message, args);
        }

        public void CustomDiagnosticLog(string message, params object[] args) {
            Logging.WriteDiagnostic(Colors.DeepSkyBlue, "[" + _nickname + "]: " + message, args);
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

    }
}
