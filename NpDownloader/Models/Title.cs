using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NpDownloader.Models;

[XmlRoot("paramsfo")]
public class Title : IXmlSerializable
{
    public XmlSchema? GetSchema()
    {
        return default;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }
}