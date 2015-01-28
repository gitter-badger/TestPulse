using System;
using System.Collections.ObjectModel;

namespace TestPulse.Core
{
    public class Manager : IDisposable
    {
        /// <summary>
        /// Gets the array of browser instances connected to this instance of the Manager.
        //     This should be considered a one-time snapshot and long-term object references
        //     to it should be avoided.
        /// </summary>
        public ReadOnlyCollection<Browser> Browsers
        {
            get { throw new NotImplementedException(); }
        }

        public Settings Settings
        {
            get { throw new NotImplementedException(); }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
