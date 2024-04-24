using System.Collections.Generic;
using System.Xml.Serialization;

namespace NpDownloader.Models;

[XmlRoot(ElementName = "tag")]
public class Tag
{
    [XmlElement(ElementName = "package")]
    public List<Package> Package { get; set; } = new();

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; } = null!;

    [XmlAttribute(AttributeName = "popup")]
    public bool Popup { get; set; }

    [XmlAttribute(AttributeName = "signoff")]
    public bool SignOff { get; set; }
}