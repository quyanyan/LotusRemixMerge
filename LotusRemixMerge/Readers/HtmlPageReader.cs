using HtmlAgilityPack;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Net;
using System.Text;
//using System.Threading.Tasks;

namespace Readers.Html
{
    public class HtmlPageReader : InputReaderBase
    {
        protected override bool CanHandle(string location)
        {
            return location.EndsWith("html");
        }
        protected override IList<string> Handle(string location)
        {
            //HtmlDocument doc = new HtmlDocument();
            //StreamReader sr = File.OpenText("file path");
            //doc.Load(sr);

            var lines = new List<string>();
            string sException = null;
            string sRslt = null;
            WebResponse oWebRps = null;
            WebRequest oWebRqst = WebRequest.Create(location);
            oWebRqst.Timeout = 50000;
            try
            {
                oWebRps = oWebRqst.GetResponse();
            }
            catch (WebException e)
            {
                sException = e.Message.ToString();
            }
            catch (Exception e)
            {
                sException = e.ToString();
            }
            finally
            {
                if (oWebRps != null)
                {
                    StreamReader oStreamRd = new StreamReader(oWebRps.GetResponseStream(), Encoding.Default);
                    sRslt = oStreamRd.ReadToEnd();
                    oStreamRd.Close();
                    oWebRps.Close();
                }
            }
            CommonHelper.GetHvtImgUrls(sRslt, location);
            return GetString(sRslt);
        }
        List<string> lines = null;
        public List<string> GetString(string sRslt)
        {
            lines = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(sRslt);
            HtmlNode hrefList = doc.DocumentNode.SelectSingleNode(".//img[@src]");
            if (hrefList != null)
            {
                if (hrefList.Attributes["src"].Value.Contains("zhaopin.cn"))
                {
                    HtmlNodeCollection colSummary = doc.DocumentNode.SelectNodes("//div[@class='summary']");
                    if (colSummary != null)
                    {
                        foreach (var item in colSummary)
                        {
                            if (item.ChildNodes != null)
                            {
                                string strText = "";
                                foreach (var child in item.ChildNodes)
                                {
                                    if (child.Name.ToLower() == "#text")
                                    {
                                        strText += child.InnerText + "|";
                                        continue;
                                    }
                                    string[] result = GetChildNodesZL(child).Split('|');

                                    foreach (var r in result)
                                    {
                                        if (!string.IsNullOrEmpty(r))
                                        {
                                            lines.Add(r.Replace("\n", "").TrimEnd().TrimStart());
                                        }
                                    }
                                }
                                lines.Add(strText);
                            }
                            else
                            {
                                lines.Add(item.InnerText);
                            }
                        }
                    }

                    HtmlNodeCollection col = doc.DocumentNode.SelectNodes("//dl[@class='details']");
                    if (col != null)
                    {
                        foreach (var item in col)
                        {
                            if (item.ChildNodes != null)
                            {
                                foreach (var child in item.ChildNodes)
                                {
                                    if (child.Name.ToLower() == "#text")
                                    {
                                        lines.Add(child.InnerText);
                                        continue;
                                    }
                                    string[] result = GetChildNodesZL(child).Split('|');
                                    foreach (var r in result)
                                    {
                                        if (!string.IsNullOrEmpty(r.TrimEnd().TrimStart()))
                                        {
                                            lines.Add(r.Replace("\n", "").TrimEnd().TrimStart());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lines.Add(item.InnerText);
                            }
                            #region 定位节点
                            //    var aa = item.InnerText;
                            //    lines.Add(aa);
                            //    var a = item.ChildNodes;

                            //    //工作经验
                            //    var nodeswrok = item.SelectNodes("//div[@class='work-experience']");

                            //    foreach (var work in nodeswrok)
                            //    {
                            //        var aaa = work.InnerText;

                            //        var aaaa = work.ChildNodes;

                            //        foreach (HtmlNode w in work.ChildNodes)
                            //        {
                            //            lines.Add(w.InnerText);
                            //        }
                            //        //HtmlNode node = HtmlNode.CreateNode(work.OuterHtml.Replace("\n", "").Replace(" ",""));
                            //        //var time = node.SelectSingleNode("//p[1]").InnerText;
                            //        //lines.Add(time);
                            //        //var company = node.SelectSingleNode("//h6").InnerText;
                            //        //lines.Add(company);
                            //        //var sp = node.SelectSingleNode("//p[2]").InnerText;
                            //        //lines.Add(sp);
                            //        //var desc = node.SelectSingleNode("//p[3]").InnerText;
                            //        //lines.Add(desc);
                            //    }


                            //    //项目经验
                            //    var nodesPro = item.SelectNodes("//div[@class='project-experience']");

                            //    foreach (var pro in nodesPro)
                            //    {
                            //        foreach (var p in pro.ChildNodes)
                            //        {
                            //            lines.Add(p.InnerText);
                            //        }
                            //        //HtmlNode node = HtmlNode.CreateNode(work.OuterHtml.Replace("\n", "").Replace(" ", ""));
                            //        //var time = node.SelectSingleNode("//p[1]").InnerText;
                            //        //lines.Add(time);
                            //        //var company = node.SelectSingleNode("//h6").InnerText;
                            //        //lines.Add(company);
                            //        //var sp = node.SelectSingleNode("//p[2]").InnerText;
                            //        //lines.Add(sp);
                            //        //var desc = node.SelectSingleNode("//p[3]").InnerText;
                            //        //lines.Add(desc);
                            //    }
                            //    //教育经历
                            //    var nodesEdc = item.SelectNodes("//div[@class='education-background']");

                            //    foreach (var work in nodesEdc)
                            //    {
                            //        HtmlNode node = HtmlNode.CreateNode(work.OuterHtml.Replace("\n", "").Replace(" ", ""));
                            //        var time = node.SelectSingleNode("//p[1]").InnerText;
                            //        lines.Add(time);
                            //        var company = node.SelectSingleNode("//h6").InnerText;
                            //        lines.Add(company);

                            //    }
                            //    //专业技能
                            //    var nodesKills = item.SelectNodes("//div[@class='professional-skill']");

                            //    foreach (var work in nodesKills)
                            //    {
                            //        HtmlNode node = HtmlNode.CreateNode(work.OuterHtml.Replace("\n", "").Replace(" ", ""));
                            //        var time = work.InnerText;
                            //        lines.Add(time);

                            //    }
                            #endregion
                        }
                    }
                }
                else
                {
                    //无忧
                    HtmlNodeCollection colbox1 = doc.DocumentNode.SelectNodes("//table[@class='box1']");
                    foreach (var item in colbox1)
                    {
                        if (item.ChildNodes != null)
                        {
                            string strText = "";
                            foreach (var child in item.ChildNodes)
                            {
                                if (child.Name.ToLower() == "#text")
                                {
                                    if (!string.IsNullOrEmpty(child.InnerText.Replace("\n", "").Replace(" ", "")))
                                    {
                                        strText += child.InnerText.Replace("\n", "").Replace(" ", "") + "|";
                                    }
                                    continue;
                                }
                                GetChildNodesWY(child);
                            }
                            if (!string.IsNullOrEmpty(strText))
                            {
                                lines.Add(strText);
                            }
                        }
                        else
                        {
                            lines.Add(item.InnerText.Replace("\n", "").Replace(" ", ""));
                        }
                    }

                    HtmlNodeCollection colbox2 = doc.DocumentNode.SelectNodes("//table[@class='box2']");
                    foreach (var item in colbox2)
                    {
                        if (item.ChildNodes != null)
                        {
                            string strText = "";
                            foreach (var child in item.ChildNodes)
                            {
                                if (child.Name.ToLower() == "#text")
                                {
                                    if (!string.IsNullOrEmpty(child.InnerText.Replace("\n", "").Replace(" ", "")))
                                    {
                                        strText += child.InnerText.Replace("\n", "").Replace(" ", "") + "|";
                                    }
                                    continue;
                                }
                                GetChildNodesWY(child);
                            }
                            if (!string.IsNullOrEmpty(strText))
                            {
                                lines.Add(strText);
                            }
                        }
                        else
                        {
                            lines.Add(item.InnerText.Replace("\n", "").Replace(" ", ""));
                        }
                    }
                    HtmlNodeCollection colbox = doc.DocumentNode.SelectNodes("//table[@class='box']");
                    foreach (var item in colbox)
                    {
                        if (item.ChildNodes != null)
                        {
                            string strText = "";
                            foreach (var child in item.ChildNodes)
                            {
                                if (child.Name.ToLower() == "#text")
                                {
                                    if (!string.IsNullOrEmpty(child.InnerText.Replace("\n", "").Replace(" ", "")))
                                    {
                                        strText += child.InnerText.Replace("\n", "").Replace(" ", "") + "|";
                                    }
                                    continue;
                                }
                                GetChildNodesWY(child);
                            }
                            if (!string.IsNullOrEmpty(strText))
                            {
                                lines.Add(strText);
                            }
                        }
                        else
                        {
                            lines.Add(item.InnerText.Replace("\n", "").Replace(" ", ""));
                        }
                    }

                }

            }

            return lines;
        }

        private string GetChildNodesZL(HtmlNode node)
        {
            string resultStr = "";
            foreach (HtmlNode item in node.ChildNodes)
            {
                if (item.Name.ToLower() == "#text")
                {
                    resultStr += item.InnerText + "|";
                    continue;
                }


                if (item.ChildNodes.Count > 0)
                {
                    if (!string.IsNullOrEmpty(item.InnerText))
                    {
                        foreach (var em in item.ChildNodes)
                        {
                            resultStr += em.InnerText + "|";
                        }
                    }
                    GetChildNodesZL(item);
                }
                else
                {
                    resultStr += item.InnerText + "|";
                }
            }
            return resultStr;
        }

        private void GetChildNodesWY(HtmlNode node)
        {
            foreach (HtmlNode item in node.ChildNodes)
            {
                if (item.Name.ToLower() == "#text")
                {
                    if (!string.IsNullOrEmpty(item.InnerText.Replace("\n", "").Replace(" ", "")))
                    {
                        lines.Add(item.InnerText.Replace("\n", "").Replace(" ", ""));
                    }
                    continue;
                }
                if (item.ChildNodes.Count > 0)
                {
                    GetChildNodesWY(item);
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.InnerText.Replace("\n", "").Replace(" ", "")))
                    {
                        lines.Add(item.InnerText.Replace("\n", "").Replace(" ", ""));

                    }
                }
            }
        }

    }
}
