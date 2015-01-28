using System;
using TechTalk.SpecFlow;
using TestPulse.Core;
using TestPulse.UIDriver;

namespace TestPulse.AcceptanceTests.Hooks
{
    public class Events
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            UI.InitializePlayback();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            UI.StopPlayback();
        }
    }
}
