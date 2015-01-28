using System;
namespace TestPulse.Core
{
    public class Settings : IXmlPersistable
    {
        public Version CurrentVersion
        {
            get { throw new NotImplementedException(); }
        }

        public string RootName
        {
            get { throw new NotImplementedException(); }
        }

        public XmlBlob XmlBlob
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void LoadFrom(XmlStoreNode store)
        {
            throw new NotImplementedException();
        }

        public void SaveTo(XmlStoreNode store)
        {
            throw new NotImplementedException();
        }
    }
}
