using System;
using Microsoft.VisualStudio.TestTools.UITesting;

namespace TestPulse.UIDriver
{
    public class UI : IUIDriver
    {
        public static void InitializePlayback()
        {
            if (!Playback.IsInitialized)
                Playback.Initialize();
        }

        public static void StopPlayback()
        {
            Playback.Cleanup();
        }
    }
}
