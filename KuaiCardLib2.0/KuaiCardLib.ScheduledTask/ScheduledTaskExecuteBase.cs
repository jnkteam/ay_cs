using System;

namespace KuaiCardLib.ScheduledTask
{
	public abstract class ScheduledTaskExecuteBase : IScheduledTaskExecute
	{
		public void Execute()
		{
			DateTime now = DateTime.Now;
			this.ExecuteTask();
			ScheduledTaskLog.WriteExecuteLog(base.GetType(), now, DateTime.Now);
		}

		protected abstract void ExecuteTask();
	}
}
