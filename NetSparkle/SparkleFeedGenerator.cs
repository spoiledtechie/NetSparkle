using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AppLimit.NetSparkle
{
    public class SparkleFeedGenerator
    {

        XmlWriter writer;
        public XmlDocument Document;

        #region Private Members
        private string _title;
        private string _link;
        private string _description;
        private string _language = "en";
        private DateTime _pubDate;
        private string _docs = "http://www.andymatuschak.org/xml-namespaces/sparkle";
        #endregion

        #region Public Members
        /// 
        /// Required - The name of the channel. It's how people refer to your service. If you have an HTML website that contains the same information as your RSS file, the title of your channel should be the same as the title of your website.
        /// 
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// 
        /// Required - The URL to the HTML website corresponding to the channel.
        /// 
        public string Link
        {
            get { return _link; }
            set { _link = value; }
        }

        /// 
        /// Required - Phrase or sentence describing the channel.
        /// 
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// 
        /// The language the channel is written in.
        /// 
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }


        /// 
        /// The publication date for the content in the channel. For example, the New York Times publishes on a daily basis, the publication date flips once every 24 hours. That's when the pubDate of the channel changes. 
        /// 
        public DateTime PubDate
        {
            get { return _pubDate; }
            set { _pubDate = value; }
        }

        /// 
        /// A URL that points to the documentation for the format used in the RSS file.
        /// 
        public string Docs
        {
            get { return _docs; }
            set { _docs = value; }
        }


        #endregion

        #region Constructors


        public SparkleFeedGenerator()
        {
            Document = new XmlDocument();
            writer = Document.CreateNavigator().AppendChild();
        }

        #endregion

        #region Methods
        /// 
        /// Writes the beginning of the RSS document
        /// 
        public void WriteStartDocument()
        {
            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "2.0");
            writer.WriteAttributeString("sparkle", _docs);
        }

        /// 
        /// Writes the end of the RSS document
        /// 
        public void WriteEndDocument()
        {
            writer.WriteEndElement(); //rss
            writer.WriteEndDocument();
        }

        /// 
        /// Closes this stream and the underlying stream
        /// 
        public void Close()
        {
            writer.Flush();
            writer.Close();

        }

        /// 
        /// Writes the beginning of a channel in the RSS document
        /// 
        public void WriteStartChannel()
        {
            try
            {
                writer.WriteStartElement("channel");

                writer.WriteElementString("title", _title);
                writer.WriteElementString("link", _link);
                writer.WriteElementString("description", _description);

                if (!String.IsNullOrEmpty(_language))
                    writer.WriteElementString("language", _language);

                if (_pubDate != null && _pubDate != DateTime.MinValue && _pubDate != DateTime.MaxValue)
                    writer.WriteElementString("pubDate", _pubDate.ToString("r"));
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// 
        /// Writes the end of a channel in the RSS document
        /// 
        public void WriteEndChannel()
        {
            writer.WriteEndElement(); //channel
        }

        /// 
        /// Writes an RSS Feed Item
        /// 
        /// The title of the item.
        /// The URL of the item
        /// The item synopsis.
        /// Email address of the author of the item.
        /// Includes the item in one or more categories
        /// URL of a page for comments relating to the item.
        /// A string that uniquely identifies the item.
        /// Indicates when the item was published.
        /// The URL of the RSS channel that the item came from.
        /// The URL of where the enclosure is located
        /// The length of the enclosure (how big it is in bytes).
        /// The standard MIME type of the enclosure.
        public void WriteItem(string title, string link, string description, DateTime pubDate, string encUrl, string encLength, string encType, string version, string dsaSignature, string releaseNotesUrl)
        {
            try
            {
                writer.WriteStartElement("item");
                writer.WriteElementString("title", title);
                writer.WriteElementString("link", link);
                writer.WriteRaw("");
                writer.WriteElementString("sparkle", "releaseNotesLink", _docs, releaseNotesUrl);

                if (pubDate != null && pubDate != DateTime.MinValue && pubDate != DateTime.MaxValue)
                    writer.WriteElementString("pubDate", pubDate.ToString("r"));

                if (!String.IsNullOrEmpty(encUrl) && !String.IsNullOrEmpty(encLength) && !String.IsNullOrEmpty(encType))
                {
                    writer.WriteStartElement("enclosure");
                    writer.WriteAttributeString("url", encUrl);
                    writer.WriteAttributeString("length", encLength);
                    writer.WriteAttributeString("type", encType);
                    writer.WriteAttributeString("sparkle", "version", _docs, version);
                    writer.WriteAttributeString("sparkle", "dsaSignature", _docs, dsaSignature);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
