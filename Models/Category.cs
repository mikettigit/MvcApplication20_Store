using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public class Picture
    {
        private string RelativeUrl;
        private string Path;

        public string UrlForSized(Int32 width, Int32 height)
        { 
            string prefix = width.ToString() + "x" + height.ToString();
            string dirpath = System.IO.Path.GetDirectoryName(Path) + "\\" + System.IO.Path.GetFileName(Path);
            dirpath = dirpath.Replace(CatalogParams.BasePath, CatalogParams.BaseImagePath);
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            string NewPath = dirpath + "\\" + prefix + System.IO.Path.GetExtension(Path);
            if (!File.Exists(NewPath))
            {
                MvcApplication10.Helpers.ImageResizer.Proceed(Path, NewPath, width, height);
            }

            return RelativeUrl + "/" + System.IO.Path.GetFileName(dirpath) + "/" + prefix + System.IO.Path.GetExtension(Path);
        }

        public Picture(string _relativeUrl, string _path)
        {
            RelativeUrl = _relativeUrl;

            Path = _path;
            string Filename = System.IO.Path.GetFileNameWithoutExtension(Path);
            if (Filename.Length > 10)
            {
                int i = 1;
                string FileNameWithoutExt = System.IO.Path.GetDirectoryName(Path) + "\\" + Filename.Substring(0, 5) + "_";
                string FileExt = System.IO.Path.GetExtension(Path);
                var newfilename = FileNameWithoutExt + i.ToString() + FileExt;
                while (File.Exists(newfilename))
                {
                    i++;
                    newfilename = FileNameWithoutExt + i.ToString() + FileExt; 
                }
                File.Move(_path, newfilename);
                Path = newfilename;
            }
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
}
