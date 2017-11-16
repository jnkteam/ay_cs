namespace OriginalStudio.WebComponents.ScheduledTask
{
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.ScheduledTask;
    using System;
    using System.Threading;

    public class DailyTask : OriginalStudio.Lib.ScheduledTask.ScheduledTask
    {
        private DateTime lastExecuteTime = DateTime.MinValue;

        protected override void ScheduleCallback()
        {
            while (true)
            {
                if (this.lastExecuteTime.AddDays(1.0) < DateTime.Now)
                {
                    while (ScheduledTasks.TaskExecuting)
                    {
                        Thread.Sleep(0x3e8);
                    }
                    try
                    {
                        ScheduledTasks.TaskExecuting = true;
                        base.ExecuteTask();
                        this.lastExecuteTime = DateTime.Today;
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
                }
                Thread.Sleep((int) (base.Config.ThreadSleepSecond * 0x3e8));
            }
        }
    }
}

