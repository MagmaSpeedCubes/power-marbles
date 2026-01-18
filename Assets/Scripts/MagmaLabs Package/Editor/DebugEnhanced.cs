using UnityEngine;

namespace MagmaLabs.Utility.Editor{
public class DebugEnhanced : Debug
{
    public static void Log(string message)
    {
        if (Constants.DEBUG_MODE)
        {
            Debug.Log(message);
        }
    }

    public static void LogWarning(string message)
    {
        if (Constants.DEBUG_MODE)
        {
            Debug.LogWarning(message);
        }
    }

    public static void LogError(string message)
    {
        if (Constants.DEBUG_MODE)
        {
            Debug.LogError(message);
        }
    }

    /// <summary>
    /// Logs a debug message if the debug level is less than or equal to the level of detail of debug messages.
    /// </summary>
    /// <param name="message"></param> The message to log
    /// <param name="level"></param> The level of detail of the message
    /// 
    public static void LogDebugLevel(string message, int level)
    {
        if (Constants.DEBUG_MODE && level <= Constants.DEBUG_LEVEL)
        {
            Debug.Log(message);
        }

    }
}
}