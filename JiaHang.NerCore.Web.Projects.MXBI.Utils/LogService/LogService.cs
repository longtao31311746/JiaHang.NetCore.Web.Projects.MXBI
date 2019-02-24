using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
public static class LogService
{
    private static readonly object obj = new object();
    private static volatile ILog logInstance;
    private static ILog log { get => GetLogger(); }
    private static ILog GetLogger()
    {
        if (logInstance == null)
        {
            lock (obj)
            {
                if (logInstance == null)
                {
                    ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
                    XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
                    logInstance = LogManager.GetLogger(repository.Name, "NETCorelog4net");
                }
            }
        }
        return logInstance;
    }
    public static void WriteInfo(object info)=> log.Info(info);
    public static void WriteInfo(object info, Exception ex) => log.Info(info, ex);
    public static void WriteDebug(object obj)=> log.Debug(obj);
    public static void WriteDebug(object obj, Exception ex)=> log.Debug(obj, ex);
    public static void WriteError(object obj) => log.Error(obj);
    public static void WriteError(object obj, Exception ex) => log.Error(obj,ex);
}

