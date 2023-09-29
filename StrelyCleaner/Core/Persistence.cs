using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrelyCleaner.Core
{
    public static class Persistence
    {
        
        public static bool RegisterlTaskService()
        {
            try
            {
                string TaskName = "StrelyCleaner";

                using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                {
                    Microsoft.Win32.TaskScheduler.Task tasks = ts.RootFolder.EnumerateTasks().ToList().Find(x => x.Name == TaskName);

                    if (tasks == null)
                    {

                        Microsoft.Win32.TaskScheduler.TaskDefinition td = ts.NewTask();
                        td.RegistrationInfo.Description = "StrelyCleaner Task Service";

                        Microsoft.Win32.TaskScheduler.TimeTrigger wt = new Microsoft.Win32.TaskScheduler.TimeTrigger();
                        wt.Repetition.Interval = TimeSpan.FromMinutes(1);
                        td.Triggers.Add(wt);

                        td.Actions.Add(new Microsoft.Win32.TaskScheduler.ExecAction(Application.ExecutablePath, "-silent"));

                        td.Principal.RunLevel = Microsoft.Win32.TaskScheduler.TaskRunLevel.Highest;

                        ts.RootFolder.RegisterTaskDefinition(TaskName, td);
                    }
                }
                return true;
            }
            catch 
            {
                // Core.ErrorLogger.LogError("Service.Install", ex.Message, ex.StackTrace)
                return false;
            }
        }

        public static void TaskService(bool Install = false)
        {
            Task Asynctask = new Task(new Action(() =>
            {
                try
                {
                    string TaskName = "StrelyCleaner";

                    using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                    {
                        Microsoft.Win32.TaskScheduler.Task tasks = ts.RootFolder.EnumerateTasks().ToList().Find(x => x.Name == TaskName);

                        if (tasks == null)
                        {
                            bool RegTask = RegisterlTaskService();
                            if (RegTask == true)
                            {
                                TaskService(Install);
                                return;
                            }
                        }
                        else
                            tasks.Enabled = Install;
                    }
                }
                catch 
                {
                }
            }), TaskCreationOptions.PreferFairness);
            Asynctask.Start();
        }

    }
}
