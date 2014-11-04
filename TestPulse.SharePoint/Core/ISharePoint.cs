using System;
namespace TestPulse.SharePoint.Core
{
    public interface ISharePoint : ISharePointList, ISharePointFile, ISharePointSite, ISharePointGroup, ISharePointRole
    {
    }
}
