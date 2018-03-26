using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace AppLog4Net
{
    public class AppLog
    {
        private ILog m_Log = null;
        private static bool bConfigLog = false;
        private const string strConfigFileName = "Log4NetConfig.xml";

        private void CheckAndConfigLogger()
        {
            string strConfigFileFullPath = AppDomain.CurrentDomain.BaseDirectory + strConfigFileName;
            if(!bConfigLog)
            {
                bConfigLog = true;
                FileInfo configFileInfo = new FileInfo(strConfigFileFullPath);
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }
        }

        public AppLog(string LoggerName)
        {
            CheckAndConfigLogger();
            m_Log = LogManager.GetLogger(LoggerName);
        }

        public AppLog(Type type)
        {
            CheckAndConfigLogger();
            m_Log = LogManager.GetLogger(type);
        }

        

        public void Debug(object message) { m_Log.Debug(message); }
        public void Debug(object message, Exception exception){ m_Log.Debug(message,exception); }
        public void DebugFormat(string format, object arg0){ m_Log.DebugFormat(format,arg0); }
        public void DebugFormat(string format, params object[] args){ m_Log.DebugFormat(format,args); }
        public void DebugFormat(IFormatProvider provider, string format, params object[] args){ m_Log.DebugFormat(provider,format,args);}
        public void DebugFormat(string format, object arg0, object arg1){ m_Log.DebugFormat(format,arg0,arg1);}
        public void DebugFormat(string format, object arg0, object arg1, object arg2){ m_Log.DebugFormat(format,arg0,arg1,arg2);}

        public void Error(object message) { m_Log.Error(message); }
        public void Error(object message, Exception exception){ m_Log.Error(message,exception); }
        public void ErrorFormat(string format, object arg0){ m_Log.ErrorFormat(format,arg0); }
        public void ErrorFormat(string format, params object[] args){ m_Log.ErrorFormat(format,args); }
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args){ m_Log.ErrorFormat(provider,format,args);}
        public void ErrorFormat(string format, object arg0, object arg1){ m_Log.ErrorFormat(format,arg0,arg1);}
        public void ErrorFormat(string format, object arg0, object arg1, object arg2){ m_Log.ErrorFormat(format,arg0,arg1,arg2);}

        public void Fatal(object message) { m_Log.Fatal(message); }
        public void Fatal(object message, Exception exception){ m_Log.Fatal(message,exception); }
        public void FatalFormat(string format, object arg0){ m_Log.FatalFormat(format,arg0); }
        public void FatalFormat(string format, params object[] args){ m_Log.FatalFormat(format,args); }
        public void FatalFormat(IFormatProvider provider, string format, params object[] args){ m_Log.FatalFormat(provider,format,args);}
        public void FatalFormat(string format, object arg0, object arg1){ m_Log.FatalFormat(format,arg0,arg1);}
        public void FatalFormat(string format, object arg0, object arg1, object arg2){ m_Log.FatalFormat(format,arg0,arg1,arg2);}

        public void Info(object message) { m_Log.Info(message); }
        public void Info(object message, Exception exception){ m_Log.Info(message,exception); }
        public void InfoFormat(string format, object arg0){ m_Log.InfoFormat(format,arg0); }
        public void InfoFormat(string format, params object[] args){ m_Log.InfoFormat(format,args); }
        public void InfoFormat(IFormatProvider provider, string format, params object[] args){ m_Log.InfoFormat(provider,format,args);}
        public void InfoFormat(string format, object arg0, object arg1){ m_Log.InfoFormat(format,arg0,arg1);}
        public void InfoFormat(string format, object arg0, object arg1, object arg2){ m_Log.InfoFormat(format,arg0,arg1,arg2);}

        public void Warn(object message) { m_Log.Warn(message); }
        public void Warn(object message, Exception exception) { m_Log.Warn(message, exception); }
        public void WarnFormat(string format, object arg0) { m_Log.WarnFormat(format, arg0); }
        public void WarnFormat(string format, params object[] args) { m_Log.WarnFormat(format, args); }
        public void WarnFormat(IFormatProvider provider, string format, params object[] args) { m_Log.WarnFormat(provider, format, args); }
        public void WarnFormat(string format, object arg0, object arg1) { m_Log.WarnFormat(format, arg0, arg1); }
        public void WarnFormat(string format, object arg0, object arg1, object arg2) { m_Log.WarnFormat(format, arg0, arg1, arg2); }
    }
}
