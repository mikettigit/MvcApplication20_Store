using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;

namespace MvcApplication20.Models
{
    public class DataModel:IDisposable
    {
        public DataModel()
        {
        }

        public void Dispose()
        {
        }

        public string GetListXml(string CategoryName, string PageId)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["adminURL"] + "category/" + CategoryName + "/feed/" + (String.IsNullOrEmpty(PageId) ? "" : "?page=" + PageId));
            request.Method = "POST";
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.ContentLength = 0;
            //request.Timeout = 10;

            string xml = "";

            try
            { 
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                xml = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
            }
            catch (Exception e)
            {
            }

            return xml; 
        }

        public PostList ParseCategory(string xml)
        {
            PostList result = new PostList();

            if (!String.IsNullOrEmpty(xml))
            {
                XmlDocument xDocument = new XmlDocument();
                xDocument.InnerXml = xml;

                result.Translate = result.Name;
                XmlNode NodeCategoryTitle = xDocument.SelectSingleNode("//channel/title");
                if (NodeCategoryTitle != null)
                {
                    result.Translate = NodeCategoryTitle.InnerText;
                }
            }

            return result;
        }

        public List<Post> ParseList(string xml)
        {
            List<Post> result = new List<Post>();

            if (!String.IsNullOrEmpty(xml))
            {
                XmlDocument xDocument = new XmlDocument();
                xDocument.InnerXml = xml;

                XmlNodeList Nodes = xDocument.SelectNodes("//item");
                foreach (XmlNode Node in Nodes)
                {
                    Post post = new Post();

                    XmlNode NodeId = Node.SelectSingleNode("id");
                    if (NodeId != null)
                    {
                        post.Id = NodeId.InnerText;
                    }

                    XmlNode NodeTitle = Node.SelectSingleNode("title");
                    if (NodeTitle != null)
                    {
                        post.Title = NodeTitle.InnerText;
                    }

                    XmlNode NodeDate = Node.SelectSingleNode("pubDate");
                    if (NodeDate != null)
                    {
                        post.Date = Convert.ToDateTime(NodeDate.InnerText);
                    }

                    XmlNode NodeDescription = Node.SelectSingleNode("description");
                    if (NodeDescription != null)
                    {
                        post.Description = NodeDescription.InnerText;
                    }

                    result.Add(post);
                }
            }
            return result;
        }

        public Post GetItem(string Id)
        {
            Post result = new Post();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["adminURL"] + Id + "/feed/");
            request.Method = "POST";
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.ContentLength = 0;
            //request.Timeout = 10;

            string xml = "";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                xml = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
            }
            catch (Exception e)
            {
            }

            if (!String.IsNullOrEmpty(xml))
            {
                XmlDocument xDocument = new XmlDocument();
                xDocument.InnerXml = xml;

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xDocument.NameTable);
                nsmgr.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");

                XmlNode Node = xDocument.SelectSingleNode("//item");
                if (Node != null)
                {
                    XmlNode NodeId = Node.SelectSingleNode("id");
                    if (NodeId != null)
                    {
                        result.Id = NodeId.InnerText;
                    }

                    XmlNode NodeTitle = Node.SelectSingleNode("title");
                    if (NodeTitle != null)
                    {
                        result.Title = NodeTitle.InnerText;
                    }

                    XmlNode NodeDate = Node.SelectSingleNode("pubDate");
                    if (NodeDate != null)
                    {
                        try
                        {
                            result.Date = Convert.ToDateTime(NodeDate.InnerText);
                        }
                        catch
                        {
                            result.Date = DateTime.Now;
                        }
                    }

                    XmlNode NodeDescription = Node.SelectSingleNode("description");
                    if (NodeDescription != null)
                    {
                        result.Description = NodeDescription.InnerText;
                    }

                    XmlNode NodeContent = Node.SelectSingleNode("content:encoded", nsmgr);
                    if (NodeContent != null)
                    {
                        result.Content = NodeContent.InnerText;
                        if (String.IsNullOrEmpty(result.Content))
                        {
                            result.Content = result.Description;
                        }
                    }

                    XmlNode NodeMetaDescription = Node.SelectSingleNode("meta_decription");
                    if (NodeMetaDescription != null)
                    {
                        result.Meta_Description = NodeMetaDescription.InnerText;
                    }

                    XmlNode NodeMetaKeywords = Node.SelectSingleNode("meta_keywords");
                    if (NodeMetaKeywords != null)
                    {
                        result.Meta_Keywords = NodeMetaKeywords.InnerText;
                    }

                    XmlNode NodeMetaTitle = Node.SelectSingleNode("meta_title");
                    if (NodeMetaTitle != null)
                    {
                        result.Meta_Title = NodeMetaTitle.InnerText;
                    }
                }
            }
            return result;
        }
    }
}