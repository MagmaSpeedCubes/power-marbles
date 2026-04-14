using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace MagmaLabs.Editor {
public class DebugEnhanced : Debug
{

    /// <summary>
    /// Logs a debug message if the debug level is less than or equal to the level of detail of debug messages.
    /// </summary>
    /// <param name="message"></param> The message to log
    /// <param name="level"></param> The level of detail of the message
    /// <param name="detailLevel"></param> The maximum level of detail to log
    /// 
    public static void LogInfoLevel(string message, int messageLevel, int detailLevel)
    {
        if (messageLevel <= detailLevel)
        {
            Debug.Log(message);
        }

    }

    /// <summary>
    /// Logs a debug message if the debug mode is enabled.
    /// </summary>
    /// <param name="message"></param> The message to log
    /// <param name="debugMode"></param> Whether to log the message
    public static void LogDebugMode(string message, bool debugMode)
    {
        if (debugMode)
        {
            Debug.Log(message);
        }
    }



}
}