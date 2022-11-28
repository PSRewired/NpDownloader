using System.Xml.Serialization;

namespace NpDownloader.Models;

[XmlRoot(ElementName = "titlepatch")]
public class TitlePatch
{
    [XmlElement(ElementName = "tag")]
    public Tag? Tag { get; set; }

    [XmlAttribute(AttributeName = "status")]
    public string Status { get; set; } = null!;

    [XmlAttribute(AttributeName = "titleid")]
    public string TitleId { get; set; } = null!;
}