﻿using System;
using System.Reflection;
using Elastic.Apm.Logging;
using Elastic.Apm.Model.Payload;

namespace Elastic.Apm.Api
{
    public static class ElasticApm
    {
        private static AbstractLogger publicApiLogger;
        public static AbstractLogger PublicApiLogger
        {
            get
            {
                if(publicApiLogger == null)
                {
                    publicApiLogger = Agent.CreateLogger("AgentAPI");
                }

                return publicApiLogger;
            }
        }

        private static Service service;
        /// <summary>
        /// Identifies the monitored service. If this remains unset the agent
        /// automatically populates it based on the entry assembly.
        /// </summary>
        /// <value>The service.</value>
        public static Service Service
        {
            get
            {
                if(service == null)
                {
                    service = new Service
                    {
                        Name = Assembly.GetEntryAssembly()?.GetName()?.Name,
                        Agent = new Model.Payload.Agent
                        {
                            Name = Consts.AgentName,
                            Version = Consts.AgentVersion
                        }
                    };
                }

                return service;
            }
            set
            {
                service = value;
            }
        }

        public static Transaction CurrentTransaction
            => TransactionContainer.Transactions.Value;

        public static Transaction StartTransaction(string name, string type)
        {
            var retVal = new Transaction(name, type)
            {
                Name = name,
                Type = type,
                service = Service
            };

            TransactionContainer.Transactions.Value = retVal;
            return retVal;
        }
    }
}
