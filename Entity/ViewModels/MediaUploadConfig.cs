using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class MediaUploadConfig
    {
        public string initialPreview { get; set; }
        public PreviewConfig[] initialPreviewConfig { get; set; }
        public bool append { get; set; }
    }
    public class PreviewConfig
    {
        public string caption { get; set; }
        public string url { get; set; }
        public string key { get; set; }
        public string type { get; set; }
        public string width { get; set; }
    }
}
