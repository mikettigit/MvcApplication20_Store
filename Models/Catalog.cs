using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication20.Models
{
    public class CatalogParams
    {
        public static readonly string BasePath;
        public static readonly string BaseImagePath;
        public static readonly string BaseCategoryUrl;
        public static readonly string BaseItemUrl;
        public static readonly string BaseImageUrl;

        public static string QueryByPath(string _path)
        {
            return _path.Replace(BasePath, "").Replace("\\","/").TrimStart(new [] {'/'});
        }

        static CatalogParams() 
        {
            BasePath = HttpContext.Current.Server.MapPath("/catalog");
            BaseImagePath = HttpContext.Current.Server.MapPath("/Images");
            BaseCategoryUrl = "/Catalog/Category";
            BaseItemUrl = "/Catalog/Item";
            BaseImageUrl = "/Images";
        }
    }

    public class CatalogItem
    {
        public string Id;
        public string Name;

        public string Path;
        public string RelativeUrl;

        public List<Picture> Pictures;
        public string Description;

        public CatalogItem()
        {
            Pictures = new List<Picture>();
        }
    }

    public class Category: CatalogItem
    {
        public bool isRoot;

        public List<Category> Parents
        {    
            get {
                List<Category> result = new List<Category>();
                if (!isRoot) {
                    Category CurrentParent = this;
                    do
                    {
                        DirectoryInfo ParentDirectoryInfo = System.IO.Directory.GetParent(CurrentParent.Path);
                        CurrentParent = new Category(CatalogParams.QueryByPath(ParentDirectoryInfo.FullName));  
                        result.Add(CurrentParent);
                    } while (!CurrentParent.isRoot);
                }
                return result;
            }
        }
        public bool HasChildren;
        public List<Category> Children
        {
            get
            {
                List<Category> result = new List<Category>();
                string[] ChildrenDirectories = System.IO.Directory.GetDirectories(Path);
                foreach (var ChildDirectory in ChildrenDirectories)
                {
                    if (!ChildDirectory.Contains(".item"))
                    {
                        result.Add(new Category(CatalogParams.QueryByPath(ChildDirectory)));
                    }
                }
                return result;
            }
        }
        public bool HasItems;
        public List<Item> Items
        {
            get
            {
                List<Item> result = new List<Item>();
                string[] ChildrenDirectories = System.IO.Directory.GetDirectories(Path);
                foreach (var ChildDirectory in ChildrenDirectories)
                {
                    if (ChildDirectory.Contains(".item"))
                    {
                        result.Add(new Item(CatalogParams.QueryByPath(ChildDirectory)));
                    }
                }
                return result;
            }
        }

        public Category(string _query) : base()
        {
            RelativeUrl = CatalogParams.BaseCategoryUrl + "/" + _query;
            string RelativeImageUrl = CatalogParams.BaseImageUrl + "/" + _query;
            string RelativePath = _query.Replace("/", "\\");
            Path = CatalogParams.BasePath + "\\" + RelativePath;

            Id = _query;
            isRoot = String.IsNullOrWhiteSpace(_query);
            HasChildren = Directory.EnumerateDirectories(Path).Any();

            string[] PathParts = RelativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (!isRoot) {
                
                Name = PathParts[PathParts.Length - 1];

                if (Directory.Exists(Path))
                {
                    //1
                    var descrSearchPattern = new System.Text.RegularExpressions.Regex(@"$(?<=\.(txt))", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    var descrfilenames = Directory.GetFiles(Path).Where(f => descrSearchPattern.IsMatch(f)).OrderBy(f => f);
                    if (descrfilenames.Count() > 0)
                    {
                        var descrfilename = descrfilenames.First();
                        Name = System.IO.Path.GetFileNameWithoutExtension(descrfilename);
                        Description = System.IO.File.ReadAllText(descrfilename).Replace("\n", "<br/>").Replace(" ", "&#32;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                    }

                    //2
                    var picturesSearchPattern = new System.Text.RegularExpressions.Regex(@"$(?<=\.(jpg|png|jpeg|gif))", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    var picturenames = Directory.GetFiles(Path).Where(f => picturesSearchPattern.IsMatch(f)).OrderBy(f => f).ToList();
                    if (picturenames.Count() > 0)
                    {
                        for (var i = 0; i < picturenames.Count(); i++)
                        {
                            Pictures.Add(new Picture(RelativeImageUrl, picturenames[i]));
                        }
                    }

                }

            }
        }
        
    }

    public class Item : CatalogItem
    {
        public string Price;
        public decimal PriceDouble
        {
            get
            {
                return String.IsNullOrWhiteSpace(Price) ? 0 : Convert.ToDecimal(Regex.Match(Price, @"[\d,\,]+").Value);
            }
        }

        public List<Category> Parents
        {
            get
            {
                List<Category> result = new List<Category>();
                CatalogItem CurrentParent = this;
                do
                {
                    DirectoryInfo ParentDirectoryInfo = System.IO.Directory.GetParent(CurrentParent.Path);
                    CurrentParent = new Category(CatalogParams.QueryByPath(ParentDirectoryInfo.FullName));
                    result.Add((Category)CurrentParent);
                } while (!((Category)CurrentParent).isRoot);
                return result;
            }
        }

        public Item(string _query)
            : base()
        {
            RelativeUrl = CatalogParams.BaseItemUrl + "/" + _query.Replace(".item", "");
            string RelativeImageUrl = CatalogParams.BaseImageUrl + "/" + _query;
            string RelativePath = _query.Replace("/", "\\");
            Path = CatalogParams.BasePath + "\\" + RelativePath;

            Id = _query;

            string[] PathParts = RelativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            Name = PathParts[PathParts.Length - 1];

            Price = "";

            if (Directory.Exists(Path))
            {
                //1
                var descrSearchPattern = new System.Text.RegularExpressions.Regex(@"$(?<=\.(txt))", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                var descrfilenames = Directory.GetFiles(Path).Where(f => descrSearchPattern.IsMatch(f)).OrderBy(f => f);
                if (descrfilenames.Count() > 0)
                {
                    var descrfilename = descrfilenames.First();
                    Name = System.IO.Path.GetFileNameWithoutExtension(descrfilename);
                    Description = System.IO.File.ReadAllText(descrfilename).Replace("\n", "<br/>").Replace(" ", "&#32;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                }

                //2
                var picturesSearchPattern = new System.Text.RegularExpressions.Regex(@"$(?<=\.(jpg|png|jpeg|gif))", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                var picturenames = Directory.GetFiles(Path).Where(f => picturesSearchPattern.IsMatch(f)).OrderBy(f => f).ToList();
                if (picturenames.Count() > 0)
                {
                    for (var i = 0; i < picturenames.Count(); i++)
                    {
                        Pictures.Add(new Picture(RelativeImageUrl, picturenames[i]));
                    }
                }

                //3
                var priceSearchPattern = new Regex(@"$(?<=\.(price))", RegexOptions.IgnoreCase);
                var pricenames = Directory.GetFiles(Path).Where(f => priceSearchPattern.IsMatch(f)).OrderBy(f => f);
                if (pricenames.Count() > 0)
                {
                    Price = System.IO.Path.GetFileNameWithoutExtension(pricenames.First());
                }

            }

        }

    }

}
