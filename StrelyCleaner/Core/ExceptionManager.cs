using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraHelper.Adders.Base
{
    internal static class ExceptionManager
    {

        public static string LogFileName = "StrelyCleanner.log";
        public static void Initialize()
        {
            ExcepList = new System.Collections.Generic.List<string>();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionHandler;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
            try
            {
               System.Windows.Forms.Application.ThreadException += Application_Exception_Handler;
                System.Windows.Forms.Application.SetUnhandledExceptionMode(System.Windows.Forms.UnhandledExceptionMode.CatchException, false);
                if (System.IO.File.Exists(LogFileName) == true) { System.IO.File.Delete(LogFileName); }
            }
            catch
            {
            }
            
        }

        public static void FirstChanceExceptionHandler(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Exception ex = (Exception)e.Exception;
            WriteLogError(ex);
        }

        public static void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            WriteLogError(ex);
        }

        private static void Application_Exception_Handler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = (Exception)e.Exception;
            WriteLogError(ex);
        }

        public static void WriteLogError(Exception Excep)
        {
            try {

                String ErrorMessage = $"------------------------------------- {Excep.ToString()}" + Environment.NewLine +
             //$"FileName: {fileName}   -   MethodName: {methodName}  OnLine: {line.ToString()}  column: {col.ToString()}" + Environment.NewLine + Environment.NewLine +
             $"Message: {Excep.Message} " + Environment.NewLine + Environment.NewLine + $" {Excep.Source} " + Environment.NewLine + Environment.NewLine +
             "______________________________________________________________End Report.";
                ExcepList.Add(ErrorMessage);
                ConsoleColor CurrentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Excep.Message);
                Console.ForegroundColor = CurrentColor;

            } catch { }

        }

        public static void WriteConsoleError(Exception Excep, string ID = "")
        {

            try {

                String ErrorMessage = $"Error ID: {ID} ------------------------------------- {Excep.ToString()}" + Environment.NewLine +
             //$"FileName: {fileName}   -   MethodName: {methodName}  OnLine: {line.ToString()}  column: {col.ToString()}" + Environment.NewLine + Environment.NewLine +
             $"Message: {Excep.Message} " + Environment.NewLine + Environment.NewLine + $" {Excep.Source} " + Environment.NewLine + Environment.NewLine +
             "______________________________________________________________End Report.";

                ConsoleColor CurrentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ErrorMessage);
                Console.ForegroundColor = CurrentColor;

            } catch { }
         
        }

        static  System.Collections.Generic.List<string> ExcepList;

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(LogFileName, string.Join(Environment.NewLine, ExcepList));
        }
}
}
