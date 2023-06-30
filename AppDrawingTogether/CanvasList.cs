using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using AppDrawingTogether.IO;

namespace AppDrawingTogether
{
    internal class CanvasList : ListView
    {

        private string _directory;
        public CanvasList(string directory)
        {
            _directory = directory;
            View = View.Tile;
            TileSize = new Size(100, 100);
            UpdateList();
        }

        public void UpdateList()
        {
            LargeImageList = new ImageList();
            LargeImageList.ImageSize = new Size(100, 100);

            foreach (string fileName in FileHelper.GetFilePaths(_directory))
            {
                if (fileName.ToLower().Contains(".png"))
                {
                    string key = fileName.ToLower().Replace(".png", "");
                    Image img = FileHelper.ReadImage(_directory + $"/{fileName}");
                    LargeImageList.Images.Add(key, img);
                    Items.Add(fileName, key);
                }
            }
        }
        /*
         * 
        private ImageList GetImages()
        {
            ImageList images = new ImageList();
            images.ImageSize = new Size(100, 100);
            foreach(string fileName in FileHelper.GetFilePaths(_directory))
            {
                if (fileName.ToLower().Contains(".png"))
                {
                    string key = fileName.ToLower().Replace(".png", "");
                    Image img = FileHelper.ReadImage(_directory + $"/{fileName}");
                    images.Images.Add(key, img);
                }
            }
            return images;
        }
        */
    }
}
