﻿using System;
using System.Collections.Generic;

namespace Elastic.Apm.Model.Payload
{
    public class Span
    {
        public const String TYPE_DB = "db";
        public const String TYPE_EXTERNAL = "external";

        public const String SUBTYPE_HTTP = "http";
        public const String SUBTYPE_MSSQL = "mssql";
        public const String SUBTYPE_SQLITE = "sqlite";

        public const String ACTION_QUERY = "query";
        public const String ACTION_EXEC = "exec";

        public ContextC Context { get; set; }

        /// <summary>
        /// The duration of the span.
        /// If it's not set (HasValue returns false) then the value 
        /// is automatically calculated when <see cref="End"/> is called.
        /// </summary>
        /// <value>The duration.</value>
        public double? Duration { get; set; }

        public String Name { get; set; }

        public String Type { get; set; }

        public String Subtype { get; set; }

        public String Action { get; set; }

        public Decimal Start { get; set; }

        public int Id { get; set; }

        public List<Stacktrace> Stacktrace { get; set; }

        public Guid Transaction_id => transaction.Id;
        internal Transaction transaction;

        private readonly DateTimeOffset start;

        public Span(string name, string type, Transaction transaction)
        {
            this.transaction = transaction;
            start = DateTimeOffset.UtcNow;
            Start = (decimal)(start - transaction.start).TotalMilliseconds;
            this.Name = name;
            this.Type = type;

            Random rnd = new Random();
            Id = rnd.Next();
        }

        public void End()
        {
            if (!Duration.HasValue)
            {
                this.Duration = (DateTimeOffset.UtcNow - start).TotalMilliseconds;
            }

            transaction?.spans.Add(this);
        }

        public void CaptureException(Exception exception, string culprit = null)
            => transaction?.CaptureException(exception, culprit);

        public class ContextC
        {
            public Db Db { get; set; }
            public Http Http { get; set; }
        }
    }

    public class Db
    {
        public String Instance { get; set; }
        public String Statement { get; set; }
        public String Type { get; set; }
    }

    public class Http 
    {
        public String Url { get; set; }
        public int Status_code { get; set; }
        public String Method { get; set; }
    }
}
