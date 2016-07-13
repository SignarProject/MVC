using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using AsignarDataAccessLayer.SerializationSignatures;

namespace AsignarBusinessLayer.Converters
{
    public class XMLConverter
    {
        public string SerializeFilter(FilterSignature filter)
        {
            using (var mS = new MemoryStream())
            {
                using (TextWriter sW = new StreamWriter(mS))
                {
                    var xmlSerializer = new XmlSerializer(typeof(XElement));
                    xmlSerializer.Serialize(sW, filter);

                    var filterContent = XElement.Parse(Encoding.UTF8.GetString(mS.ToArray())).ToString();

                    return filterContent;
                }
            }
        }

        public FilterSignature DeserializeFilter(string filterContent)
        {
            XElement xFilter = XElement.Parse(filterContent);

            var serializer = new XmlSerializer(typeof(FilterSignature));
            var filter = serializer.Deserialize(xFilter.CreateReader()) as FilterSignature;

            return filter;
        }
    }
}
