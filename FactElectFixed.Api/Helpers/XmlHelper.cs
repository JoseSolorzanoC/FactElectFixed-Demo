using System.Xml;
using System.Xml.Linq;

namespace FactElectFixed.Api.Helpers;

public static class XmlHelper
{
    public static XmlDocument ToXmlDocument(this XDocument xDocument)
    {
        var xmlDocument = new XmlDocument();
        using XmlReader reader = xDocument.CreateReader();
        xmlDocument.Load(reader);

        return xmlDocument;
    }
}
