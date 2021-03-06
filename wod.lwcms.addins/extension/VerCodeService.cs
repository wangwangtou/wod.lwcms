﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Drawing;

namespace wod.lwcms.addins.extension
{
    public class VerCodeService : IVerCodeService
    {
        private HttpContext context;
        public VerCodeService(HttpContext context)
        {
            this.context = context;
        }

        private static Random rdm = new Random();
        private static readonly string vercodeSessionName = "wod_vercode";

        public void GenerateVerCode()
        {
            var code = rdm.Next(10).ToString() + rdm.Next(10).ToString() + rdm.Next(10).ToString() + rdm.Next(10).ToString();
            context.Session[vercodeSessionName] = code;

            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            Bitmap map = new Bitmap(60, 20);
            Graphics graph = Graphics.FromImage(map);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, 60, 20);
            graph.DrawString(code.Substring(0, 1), new Font(SystemFonts.DialogFont.FontFamily,10, FontStyle.Bold), new SolidBrush(Color.Black), new PointF(10, 5));
            graph.DrawString(code.Substring(1, 1), new Font(SystemFonts.DialogFont.FontFamily,10, FontStyle.Italic), new SolidBrush(Color.Black), new PointF(20, 5));
            graph.DrawString(code.Substring(2, 1), new Font(SystemFonts.DialogFont.FontFamily,10, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(30, 5));
            graph.DrawString(code.Substring(3, 1), new Font(SystemFonts.DialogFont.FontFamily,10, FontStyle.Strikeout), new SolidBrush(Color.Black), new PointF(40, 5));
            for (int i = 0; i < 20; i++)
            {
                graph.FillRectangle(new SolidBrush(Color.Red), rdm.Next(60), rdm.Next(20), 2, 2);
            }
            map.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            context.Response.End();
        }

        public void ClearVerCode()
        {
            context.Session[vercodeSessionName] = null;
        }

        public bool CheckVerCode(string vercode)
        {
            return vercode != null && vercode.Length == 4 && context.Session[vercodeSessionName] != null && context.Session[vercodeSessionName].ToString() == vercode;
        }
    }
}
