using System;
using Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("LogSys")]
    public class LogSys : BaseEntity    
    {
        public virtual long UserId { get; set; }

        public virtual string ResourceId { get; set; }

        public virtual string ResourceType { get; set; }

        public virtual string TableName { get; set; }

        public virtual string ScreenName { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual TimeSpan Time { get; set; }

        public virtual long EstimateBySecond { get; set; }

        public virtual string Title { get; set; }

        public virtual string Message { get; set; }

        public virtual string Path { get; set; }

        public virtual string Line { get; set; }

        public virtual string Icon { get; set; }

        public virtual LogType LogType { get; set; }

        public virtual LogStatus LogStatus { get; set; }

        public virtual LogAccessLevel LogAccessLevel { get; set; }

        public virtual bool Sent { get; set; }
    }
}