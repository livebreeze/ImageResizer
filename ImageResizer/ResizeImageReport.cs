using System.Collections.Generic;

namespace ImageResizer
{
    public class ResizeImageReport
    {
        public List<string> ProcessFileList { get; set; }
        public List<string> SuccessFileList { get; set; }
        public int TotalCount { get; set; }
        public string CurrentFileName { get; set; }
        public int ThreadId { get; set; }
        public string Status { get; set; }
    }
}