using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NpDownloader.Models;

[XmlRoot("paramsfo")]
public class ParamsSfo : IXmlSerializable
{
    public Dictionary<string, string> Params { get; set; } = new();
    public XmlSchema? GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        if (reader.IsEmptyElement || reader.Read() == false)
        {
            return;
        }

        while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
        {
            Params.Add(reader.Name, reader.ReadString());
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }
}