namespace KuaiCard.BLL
{
    using KuaiCard.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Xml;

    public class templateFactory
    {
        public static List<template> AllTemplate()
        {
            List<template> list = HttpRuntime.Cache.Get("TemplateConfiguration") as List<template>;
            if (list == null)
            {
                List<template> list2 = new List<template>();
                string[] strArray = Directory.GetFiles(HttpContext.Current.Server.MapPath("/Template/"), "*.xml", SearchOption.AllDirectories);
                foreach (string str in strArray)
                {
                    list2.Add(Get(str));
                }
                list = list2;
                HttpRuntime.Cache.Insert("TemplateConfiguration", list);
            }
            return list;
        }

        public static template Get(string dir)
        {
            template template = new template();
            XmlNodeList childNodes = GetConfig(dir).SelectSingleNode("about").ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                XmlElement element = (XmlElement) node;
                template.ID = element.GetAttribute("ID");
                template.Name = element.GetAttribute("name");
                template.Author = element.GetAttribute("author");
                template.Createdate = element.GetAttribute("createdate");
                template.IsAgent = element.GetAttribute("isAgent");
                template.Copyright = element.GetAttribute("copyright");
                template.Photo = template.ID + "/about.jpg";
                template.Bigphoto = template.ID + "/bigabout.jpg";
            }
            return template;
        }

        public static XmlDocument GetConfig(string dir)
        {
            string path = dir;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            XmlDocument document = new XmlDocument();
            document.Load(path);
            return document;
        }
    }
}

