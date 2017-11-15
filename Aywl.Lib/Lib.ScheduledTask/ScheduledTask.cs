using OriginalStudio.Lib.ExceptionHandling;
using System;
using System.Threading;

namespace OriginalStudio.Lib.ScheduledTask
{
	public abstract class ScheduledTask
	{
		private ScheduledTaskConfiguration _config;

		protected Thread ScheduleThread;

		protected ScheduledTaskConfiguration Config
		{
			get
			{
				return this._config;
			}
			set
			{
				this._config = value;
			}
		}

		public void Execute(ScheduledTaskConfiguration config)
		{
			this.Config = config;
			this.ScheduleThread = new Thread(new ThreadStart(this.ScheduleCallback));
			this.ScheduleThread.Start();
		}

		protected void ExecuteTask()
		{
			if (this.Config != null)
			{
				for (int index = 0; index < this.Config.Executes.Count; index++)
				{
					try
					{
						((IScheduledTaskExecute)Activator.CreateInstance(Type.GetType(this.Config.Executes[index]), true)).Execute();
					}
					catch (Exception ex)
					{
						ExceptionHandler.HandleException(ex);
					}
				}
			}
		}

		protected abstract void ScheduleCallback();

		public void Stop()
		{
			if (this.ScheduleThread != null)
			{
				this.ScheduleThread.Abort();
			}
		}
	}
}
