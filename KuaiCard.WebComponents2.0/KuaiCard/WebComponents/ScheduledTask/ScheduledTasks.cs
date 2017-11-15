namespace KuaiCard.WebComponents.ScheduledTask
{
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.ScheduledTask;
    using System;
    using System.Collections.Generic;

    public class ScheduledTasks
    {
        private KuaiCardLib.ScheduledTask.ScheduledTask[] _scheduledTasks;
        public static bool TaskExecuting = false;

        public void Start()
        {
            List<ScheduledTaskConfiguration> configs = ScheduledTaskConfigurationSectionHandler.GetConfigs();
            if (configs != null)
            {
                this._scheduledTasks = new KuaiCardLib.ScheduledTask.ScheduledTask[configs.Count];
                for (int i = 0; i < configs.Count; i++)
                {
                    try
                    {
                        KuaiCardLib.ScheduledTask.ScheduledTask task = Activator.CreateInstance(Type.GetType(configs[i].ScheduledTaskType)) as KuaiCardLib.ScheduledTask.ScheduledTask;
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

