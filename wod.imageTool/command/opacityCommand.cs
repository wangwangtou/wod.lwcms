using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;
using wod.imageTool.imageLib;

namespace wod.imageTool.command
{
    public class opacityCommand : parameterCommandBase<opacityParameter>
    {
        protected override void Execute(opacityParameter parameter)
        {
            var fi = new FileInfo(parameter.SourcePath);
            var sourceImage = new List<string>();
            var sourceFolder = "";
            if (fi.Exists && common.IsImage(fi))
            {
                sourceImage.Add(fi.FullName);
                sourceFolder = fi.Directory.FullName;
            }
            else
            {
                var di = new DirectoryInfo(parameter.SourcePath);
                sourceImage.AddRange(di.GetFiles().Where(f => common.IsImage(f)).Select(f=>f.FullName));
                sourceFolder = di.FullName;
            }
            var targetFolder = "";
            if (string.IsNullOrEmpty(parameter.TargetPath))
            {
                targetFolder = sourceFolder + "\\wod_opacity";
            }
            else {
                targetFolder = parameter.TargetPath;
            }
            var targetDir = Directory.CreateDirectory(targetFolder);

            System.Drawing.Color color = System.Drawing.Color.White;
            try 
	        {
                color = (System.Drawing.Color)(new System.Drawing.ColorConverter()).ConvertFromString(parameter.OpacityPoint);
	        }
	        catch (Exception)
	        {
	        }
            common.OpacityImages(targetFolder, sourceImage, color, parameter.OpacityRange);
        }
    }

    public class opacityParameter
    {
        public opacityParameter()
        {
            OpacityRange = 10;
            OpacityPoint = "#FFFFFF";
        }
        public string OpacityPoint { get; set; }
        public double? OpacityRange { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
    }
}
