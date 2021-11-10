using System;
using System.ComponentModel.DataAnnotations.Schema;
using Utility;

namespace Entity.Model
{
    [Table("LogSys", Schema = "org")]
    public class LogSys : BaseModel
    {
        public long UserId { get; set; }

        public string ResourceId { get; set; }

        public string ResourceType { get; set; }

        public string TableName { get; set; }

        public string ScreenName { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public long EstimateBySecond { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Path { get; set; }

        public string Line { get; set; }

        public string Icon { get; set; }

        public LogType LogType { get; set; }

        public LogStatus LogStatus { get; set; }

        public LogAccessLevel LogAccessLevel { get; set; }

        public bool Sent { get; set; }
    }
}