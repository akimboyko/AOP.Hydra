using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using NUnit.Framework;

namespace AOP.Hydra.PostSharp.UnitTests
{
    [TestFixture]
    public class TracingAspectTests
    {
        [Test]
        public void TracingAspect_OnEntry_AddInfo()
        {
            var result = new FakeService().Process(3, 4);

            Assert.That(result, Is.EqualTo(12));

            FakeLoggerFactory.FakeLoggerInstance.Messages.Should().Contain(message => 
                                                                            message.Contains("OnEntry")
                                                                            && message.Contains(string.Format("{0}.Process(...)", typeof(FakeService).FullName)));

            FakeLoggerFactory.FakeLoggerInstance.Messages.Should().Contain(message => 
                                                                            message.Contains("OnExit")
                                                                            && message.Contains(string.Format("{0}.Process(...)", typeof(FakeService).FullName)));

            FakeLoggerFactory.FakeLoggerInstance.Messages.Should().Contain(message => 
                                                                            message.Contains("OnSuccess")
                                                                            && message.Contains(string.Format("{0}.Process(...)", typeof(FakeService).FullName)));
        }

        [Test]
        public void TracingAspect_OnException_AddInfo()
        {
            var fakeService = new FakeService();

            Assert.Throws<Exception>(() => fakeService.ProcessWithException(3, 4));

            FakeLoggerFactory.FakeLoggerInstance.Messages.Should().Contain(message =>
                                                                            message.Contains("OnEntry")
                                                                            && message.Contains(string.Format("{0}.ProcessWithException(...)", typeof(FakeService).FullName)));

            FakeLoggerFactory.FakeLoggerInstance.Messages.Should().Contain(message =>
                                                                            message.Contains("OnExit")
                                                                            && message.Contains(string.Format("{0}.ProcessWithException(...)", typeof(FakeService).FullName)));

            FakeLoggerFactory.FakeLoggerInstance.Messages.Should().Contain(message =>
                                                                            message.Contains("OnException")
                                                                            && message.Contains(string.Format("{0}.ProcessWithException(...)", typeof(FakeService).FullName)));
        }

        [TracingAspect(AbstractFactoryType = typeof(FakeLoggerFactory))]
        private class FakeService
        {
            public int Process(int n, int m)
            {
                return n * m;
            }

            public int ProcessWithException(int n, int m)
            {
                throw new Exception(string.Format("An exception occures while calculating {0} * {1}", n, m));
            }
        }

        private class FakeLogger : ILogger
        {
            private readonly IList<string> _messages = new List<string>();

            public IReadOnlyCollection<string> Messages
            {
                get
                {
                    return new ReadOnlyCollection<string>(_messages);
                }
            }

            public void Log(string message)
            {
                _messages.Add(message);
            }
        }

        private class FakeLoggerFactory : IAbstractFactory<FakeLogger>
        {
            public static readonly FakeLogger FakeLoggerInstance = new FakeLogger();

            public FakeLogger CreateInstance()
            {
                return FakeLoggerInstance;
            }
        }
    }
}
