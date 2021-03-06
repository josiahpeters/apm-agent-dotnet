﻿using System;
using System.Collections.Generic;
using Elastic.Apm.Model.Payload;
using Elastic.Apm.Report;

namespace Elastic.Apm.Tests.Mock
{
    public class MockPayloadSender : IPayloadSender
    {
        public List<Payload> Payloads = new List<Payload>();
        public List<Error> Errors = new List<Error>();

        public void QueueError(Error error) => Errors.Add(error);
        public void QueuePayload(Payload payload) => Payloads.Add(payload);
    }
}
