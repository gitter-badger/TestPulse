using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Tools.Applications.Runtime;

namespace TestPulse.UIDriver.Helper
{
    public static class Utils
    {
        public static void ExecuteWithRetry(Action codeToRun, int retryMax = 10, int retryInterval = 2000)
        {
            int retryCount = 0;
            bool passed = false;

            while (!passed)
            {
                try
                {
                    codeToRun.Invoke();
                    passed = true;
                }
                catch (Exception e)
                {
                    if (e is ControlNotFoundException || e is ArgumentOutOfRangeException || e is AssertFailedException
                        || e is FailedToPerformActionOnBlockedControlException)
                    {
                        // Retry or throw
                        if (retryCount++ == retryMax)
                        {
                            throw;
                        }

                        Thread.Sleep(retryInterval);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
