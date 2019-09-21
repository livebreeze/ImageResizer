using System;

namespace ImageResizer
{
    class MyProgress<T> : Progress<T>
    {
        public MyProgress() : base()
        {

        }
        public MyProgress(Action<T> handler) : base(handler)
        {
        }

        protected override void OnReport(T value)
        {
            var report = value as ResizeImageReport;
            var processFileCount = report.ProcessFileList?.Count ?? 0;
            var successFileCount = report.SuccessFileList?.Count ?? 0;

            Console.WriteLine($"{report.Status}" +
                $"\t檔案: {report.CurrentFileName}" +
                $"\t處理進度(done,process,total) {successFileCount}/{processFileCount}/{report.TotalCount}" +
                $"\tFileProcessThread: {report.ThreadId}");

            base.OnReport(value);
        }
    }
}
