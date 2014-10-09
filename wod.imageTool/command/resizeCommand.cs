using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;
using wod.imageTool.imageLib;

namespace wod.imageTool.command
{
    public class resizeCommand:parameterCommandBase<resizeParameter>
    {
        protected override void Execute(resizeParameter parameter)
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
                targetFolder = sourceFolder + "\\wod_resize";
            }
            else {
                targetFolder = parameter.TargetPath;
            }
            var targetDir = Directory.CreateDirectory(targetFolder);

            common.ResizeImages(parameter.RatioScale, targetFolder, sourceImage, parameter.MaxWidth, parameter.MaxHeight);
        }
    }

    public class resizeParameter
    {
        public resizeParameter()
        {
            RatioScale = true;
        }

        public bool RatioScale { get; set; }

        public double? MaxWidth { get; set; }
        public double? MaxHeight { get; set; }

        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
    }
}
