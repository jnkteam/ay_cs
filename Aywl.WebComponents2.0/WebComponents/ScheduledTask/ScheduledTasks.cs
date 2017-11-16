namespace OriginalStudio.WebComponents.ScheduledTask
{
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.ScheduledTask;
    using System;
    using System.Collections.Generic;

    public class ScheduledTasks
    {
        private OriginalStudio.Lib.ScheduledTask.ScheduledTask[] _scheduledTasks;
        public static bool TaskExecuting = false;

        public void Start()
        {
            List<ScheduledTaskConfiguration> configs = ScheduledTaskConfigurationSectionHandler.GetConfigs();
            if (configs != null)
            {
                this._scheduledTasks = new OriginalStudio.Lib.ScheduledTask.ScheduledTask[configs.Count];
                for (int i = 0; i < configs.Count; i++)
                {
                    try
                    {
                        ScheduledTask task = Activator.CreateInstance(Type.GetType(configs[i].ScheduledTaskType)) as OriginalStudio.ScheduledTask.ScheduledTask;
                        if (task != null)
                        {
                            task.Execute(configs[i]);
                            this._scheduledTasks[i] = task;
                        }
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception);
                    }
                }
            }
        }

        public void Stop()
        {
            if (this._scheduledTasks != null)
            {
                for (int i = 0; i < this._scheduledTasks.Length; i++)
                {
                    if (this._scheduledTasks[i] != null)
                    {
                        this._scheduledTasks[i].Stop();
                    }
                }
            }
        }
    }
}

