using System;
using TechTalk.SpecFlow;

namespace TestPulse.SpecFlow
{
    public class Events
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        [BeforeScenario]
        public void BeforeScenario()
        {
            //TODO: implement logic that has to run before executing each scenario
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
           
        }
    }
}
