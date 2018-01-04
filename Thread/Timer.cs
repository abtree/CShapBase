using System;
using System.Threading;

namespace Timer
{
    class Timer
    {
        #region /* System.Threading.Timer */
        private static void ThreadingTimer()
        {
            //从现在开始2秒后每隔3秒回调一次
            var t1 = new System.Threading.Timer(TimeAction, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3));
            Thread.Sleep(15000);  //15s
            t1.Dispose();  //结束timer
        }

        static void TimeAction(object o)
        {
            Console.WriteLine("System.Threading.Timer {0:T}", DateTime.Now);
        }
        #endregion

        #region /* System.Timers.Timer */
        private static void TimersTimer()
        {
            var t1 = new System.Timers.Timer(1000);  //1s 一次
            t1.AutoReset = true;
            t1.Elapsed += TimeAction1;
            t1.Start();
            Thread.Sleep(10000);  //10s
            t1.Stop();

            t1.Dispose();
        }

        static void TimeAction1(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("System.Timers.Timer {0:T}", e.SignalTime);
        }
        #endregion
        
        /* System.Windows.Forms.Timer */
        /* System.Web.UI.Timer */
        /* System.Windows.Threading.Timer */

        static void Main(string[] args)
        {
            //ThreadingTimer();
            TimersTimer();
        }
    }
}
