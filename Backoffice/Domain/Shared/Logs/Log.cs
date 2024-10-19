using System;
using System.Diagnostics.Eventing.Reader;
using Backoffice.Domain.Logs;
using Backoffice.Domain.Shared;
using Backoffice.Infraestructure.Logs;

namespace Backoffice.Domain.Logs
{
    public class Log : Entity<LogId>
    {

        public int Id {get; private set;}
        public DateTime DateTime {get; private set;}
        public string Description {get; private set;}
        public LogType LogType {get; private set;}
        public LogEntity LogEntity {get; private set;}

        public Log(string description, LogType logType, LogEntity logEntity)
        {
            this.DateTime = DateTime.Now;
            this.Description = description;
            this.LogType = logType;
            this.LogEntity = logEntity;
        }
    }
}