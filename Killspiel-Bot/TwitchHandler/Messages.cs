using System;
using System.Collections.Generic;

namespace TwitchHandler
{
    static class Messages
    {
        public const string InvalidArgCount = "Ungültige Argumentzahl!";
        public const string GameActive = "Killspiel läuft noch!";
        public const string GameInactive = "Killspiel ist nicht aktiv!";
        public const string GameStarted = "Tipps werden jetzt angenommen!";
        public const string GameStopped = "Tipps werden nicht mehr angenmommen!";
        public const string InvalidCommand = "Ungültiger Befehl!";
        public const string InsufficientPermissions = "Keine Berechtigung!";

        public static string Winners(IEnumerable<string> winners)
        {
            return "Gewonnen haben: " + string.Join(", ", winners);
        }
            
    }
}
