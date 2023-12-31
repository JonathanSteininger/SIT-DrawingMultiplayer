﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using DrawingTogether;

namespace AppDrawingTogether.IO
{
    internal static class FileHelper
    {

        public static void SaveImageAndData(string directoryPath, string ImageName, PictureBox canvas, List<LinePortion> LineData)
        {
            SaveImage(directoryPath + $"/{ImageName}.png", canvas);
            SaveObjectJson(directoryPath + $"/{ImageName}.data", LineData);
        }
        public static string[] GetFilePaths(string directoryPath)
        {
            if(!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            string[] temp = Directory.GetFiles(directoryPath);
            string[] names = new string[temp.Length];
            for(int i = 0; i< temp.Length; i++) names[i] = Path.GetFileName(temp[i]);
            return names;
        }
        static public void SaveObjectJson(string filePath, object obj)
        {
            VerifyPath(filePath);
            using(FileStream stream = new FileStream(filePath, FileMode.Create))
            using(BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(JsonConvert.SerializeObject(obj));
            }
        }
        static public object ReadObjectJSON(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                
                string json = reader.ReadString();
                return JsonConvert.DeserializeObject(json);
            }
        }
        static public void SaveImage(string filePath, PictureBox canvas)
        {
            VerifyPath(filePath);
            using (Bitmap map = new Bitmap(canvas.Width, canvas.Height))
            {
                canvas.DrawToBitmap(map, new Rectangle(0,0, canvas.Width, canvas.Height));
                map.Save(filePath, ImageFormat.Png);
            }
        }
        static public Image ReadImage(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            return Image.FromFile(filePath);
        }

        static private void VerifyPath(string filePath)
        {
            if (File.Exists(filePath)) return;

            string dir = ExtractDirectory(filePath);

            if (Directory.Exists(dir)) return;
            Directory.CreateDirectory(dir);
        }

        static private string ExtractDirectory(string filePath)
        {
            string[] parts = filePath.Split('/');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parts.Length - 1; i++)
            {
                sb.Append(parts[i]);
                sb.Append('/');
            }
            return sb.ToString();
        }
    }
}
