using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output"); ;

            ImageProcess imageProcess = new ImageProcess();

            imageProcess.Clean(destinationPath);
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationTokenSource ctsTimeout = new CancellationTokenSource();
            ctsTimeout.CancelAfter(11000);
            CancellationTokenSource ctsCombination = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, ctsTimeout.Token);

            #region 等候使用者輸入 取消 c 按鍵
            QuieueClick(cts);
            #endregion

            Console.WriteLine($"MainThread: {Thread.CurrentThread.ManagedThreadId}");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var progress = new MyProgress<ResizeImageReport>((report) =>
            {
                //var processFileCount = report.ProcessFileList?.Count ?? 0;
                //Console.WriteLine($"狀態: {report.Status}, " +
                //    $"檔案: {report.CurrentFileName}, " +
                //    $"處理進度 {processFileCount}/{report.TotalCount}, " +
                //    $"FileProcessThread: {report.ThreadId}, " +
                //    $"ReportCWThread: {Thread.CurrentThread.ManagedThreadId}");
            });

            try
            {
                await imageProcess.ResizeImagesAsync(sourcePath, destinationPath, 5.0, cts.Token, progress);
            }
            catch (OperationCanceledException ex)
            {
                //Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            sw.Stop();

            Console.WriteLine($"花費時間: {sw.ElapsedMilliseconds} ms");

            Console.ReadLine();
        }

        private static void QuieueClick(CancellationTokenSource cts)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.C)
                {
                    cts.Cancel();
                }
                else
                {
                    QuieueClick(cts);
                }
            });
        }
    }
}
