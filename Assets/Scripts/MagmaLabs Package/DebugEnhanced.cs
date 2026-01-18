using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace MagmaLabs.Utilities.Editor{
public class DebugEnhanced : Debug
{

    /// <summary>
    /// Logs a debug message if the debug level is less than or equal to the level of detail of debug messages.
    /// </summary>
    /// <param name="message"></param> The message to log
    /// <param name="level"></param> The level of detail of the message
    /// <param name="detailLevel"></param> The maximum level of detail to log, default is Constants.DEBUG_INFO_LEVEL
    /// 
    public static void LogInfoLevel(string message, int messageLevel, int detailLevel = Constants.DEBUG_INFO_LEVEL)
    {
        if (messageLevel <= detailLevel)
        {
            Debug.Log(message);
        }

    }

}
}