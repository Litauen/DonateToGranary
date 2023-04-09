using System;
using System.IO;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace DonateToGranary
{

    public static class Logger
    {
   
        public const string ModuleId = "DonateToGranary";
        private static readonly string LOG_PATH = @"..\\..\\Modules\\" + ModuleId + "\\logs\\";
        private static readonly string ERROR_FILE = LOG_PATH + "error.log";
        private static readonly string DEBUG_FILE = LOG_PATH + "debug.log";

        //  Bool To specify if messages should be logged to file by default
        public static bool logToFile = false;
        // Bool To specify if the system is in debug mode. Affects debug messages
        public static bool IsDebug = false;
        // String to be pre pended to displayed messages,  by default the value is null and is only prepended to messages if it is not null
        public static string? PrePrend = null;
        // String representation of the module version for use in logs and messages
        public static string ModVersion = "";

        static Logger()
        {
            if (!Directory.Exists(LOG_PATH)) Directory.CreateDirectory(LOG_PATH);
            if (!File.Exists(ERROR_FILE)) File.Create(ERROR_FILE);
            if (!File.Exists(DEBUG_FILE)) File.Create(DEBUG_FILE);
        }

        public static void LogDebug(string log)
        {
            //if (!Main.Settings.DebugMode) return;
            using (StreamWriter streamWriter = new(DEBUG_FILE, true))
                streamWriter.WriteLine(log);

            DisplayInfoMsg("DEBUG | " + log);
        }

        public static void LogError(string log)
        {
            using StreamWriter streamWriter = new(ERROR_FILE, true);
            streamWriter.WriteLine(log);
        }

        public static void LogError(Exception exception)
        {
            LogError("Message " + exception.Message);
            LogError("Error at " + exception.Source.ToString() + " in function " + exception.Message);
            LogError("With stacktrace :\n" + exception.StackTrace);
            LogError("----------------------------------------------------");

            //if (!Main.Settings.DebugMode) return;
            DisplayInfoMsg(exception.Message);
            DisplayInfoMsg(exception.Source);
            DisplayInfoMsg(exception.StackTrace);
        }

        public static void DisplayInfoMsg(string message)
        {
            InformationManager.DisplayMessage(new InformationMessage(message));
        }


        // ------------------------ Kaoses Common --------------------------------
        // https://github.com/lazeras/KaosesCommon/blob/e16484b8396d93ca478d201cc7cbda66fec4b1a5/KaosesCommon/Utils/IM.cs#L218

        public static void IM(string message, string color = "#FFFFFFFF")
        {
            TextObject to = new TextObject(message);
            Logger.DisplayColorInfoMessage(to.ToString(), Color.ConvertStringToColor(color), logToFile);
        }

        public static void IMGreen(string message)
        {
            IM(message, "#42FF00FF");
            //Logger.DisplayColorInfoMessage(message, Color.ConvertStringToColor("#42FF00FF"), logToFile);
        }

        public static void IMRed(string message)
        {
            IM(message, "#FF0042FF");
            //Logger.DisplayColorInfoMessage(message, Color.ConvertStringToColor("#FF0042FF"), logToFile);
        }

        public static void IMBlue(string message)
        {
            IM(message, "#4242FFFF");
            //Logger.DisplayColorInfoMessage(message, Color.ConvertStringToColor("#4242FFFF"), logToFile);
        }

        public static void IMGrey(string message)
        {
            IM(message, "#AAAAAAFF");
            //Logger.DisplayColorInfoMessage(message, Color.ConvertStringToColor("#AAAAAAFF"), logToFile);
        }
        private static void DisplayColorInfoMessage(string message, Color messageColor, bool logToFile = false)
        {
            string fullMessage = message;
            if (!string.IsNullOrWhiteSpace(Logger.PrePrend)) fullMessage = Logger.PrePrend + " : " + message;

            try
            {
                if (logToFile) LogDebug(fullMessage);
                InformationManager.DisplayMessage(new InformationMessage(fullMessage, messageColor));
            }
            catch (Exception ex)
            {
                LogError("messageStrLength: " + fullMessage.Length);
                LogError("messageStr: " + fullMessage);
                LogError(ex);
            }

        }

        // yellow notification at the top-center, like relation change
        internal static void AddQuickNotificationWithSound(TextObject content, BasicCharacterObject? announcer = null, string? sounEventPath = null)
        {
            MBInformationManager.AddQuickInformation(content, 0, announcer, sounEventPath);
        }

    }
}