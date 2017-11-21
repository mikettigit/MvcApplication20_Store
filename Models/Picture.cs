using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcApplication20.Models
{
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
                MvcApplication20.Helpers.ImageResizer.Proceed(Path, NewPath, width, height);
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

}