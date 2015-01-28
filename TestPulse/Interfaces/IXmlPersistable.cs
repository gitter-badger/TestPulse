using System;
namespace TestPulse
{
    // Summary:
    //     Interface for persisting XML formatted data
    public interface IXmlPersistable
    {
        // Summary:
        //     Gets the version string to use when serializing the object to XML. This string
        //     will be added as an attribute to the object's start tag.
        Version CurrentVersion { get; }
        //
        // Summary:
        //     Gets the tag name to use for the implementing object when it is serialized
        //     as the root of a document
        //
        // Remarks:
        //     This is only used for a document's root node. Interior nodes are stored with
        //     a tag name specified by the node's parent.
        string RootName { get; }
        //
        // Summary:
        //     Opaque storage for serialized XML information the deserializer doesn't understand.
        XmlBlob XmlBlob { get; set; }

        // Summary:
        //     Called when an object is deserialized. The object should read any data it
        //     needs to reconstitute itself from the store.
        //
        // Parameters:
        //   store:
        //
        // Remarks:
        //     The object's empty constructor is called before this method is called.
        void LoadFrom(XmlStoreNode store);
        //
        // Summary:
        //     Called when an object is serialized. The object should add any data that
        //     is needed to reconstitute the object to the store.
        //
        // Parameters:
        //   store:
        void SaveTo(XmlStoreNode store);
    }

    public class XmlBlob
    {
    }

    public class XmlStoreNode
    {
    }
}
