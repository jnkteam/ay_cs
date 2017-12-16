namespace OriginalStudio.WebComponents.ScheduledTask
{
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.ScheduledTask;
    using System;
    using System.Threading;

    public class IntervalTask : OriginalStudio.Lib.ScheduledTask.ScheduledTask
    {
        protected override void ScheduleCallback()
        {
            while (true)
            {
                while (ScheduledTasks.TaskExecuting)
                {
                    Thread.Sleep(0x3e8);
                }
                try
                {
                    ScheduledTasks.TaskExecuting = true;
                    base.ExecuteTask();
                    ScheduledTaskLog.WriteLog(base.Config);
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                }
                finally
                {
                    ScheduledTasks.TaskExecuting = false;
                }
                Thread.Sleep((int) (base.Config.ThreadSleepSecond * 0x3e8));
            }
        }
    }
}

