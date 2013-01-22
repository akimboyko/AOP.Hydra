using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace AOP.Hydra.PostSharp.UnitTests
{
    [TestFixture]
    public class InjectionAspectWithCallbackAttributeTests
    {
        [TestCase(10)]
        public void InjectionAspectWithCallbackAttribute_InjectLogger_ExpectLogCalled(int nTimes)
        {
            var repository = new MockRepository();

            var loggerMock = repository.StrictMock<ILogger>();

            using (repository.Record())
            {
                loggerMock.Expect(m => m.Log(Arg<string>.Is.Anything)).Repeat.Times(nTimes);
            }

            var nInjections = 0;

            InjectionAspectWithCallbackAttribute.InjectLogger = () =>
                {
                    nInjections++;
                    return loggerMock;
                };

            foreach (var n in Enumerable.Range(0, nTimes))
                new FakeService().FakeCall();

            Assert.That(nInjections, Is.EqualTo(nTimes));
            loggerMock.VerifyAllExpectations();
        }

        private class FakeService
        {
            [InjectionAspectWithCallbackAttribute]
            internal void FakeCall()
            {
                
            }
        }

        [Test, Ignore("I gave a try to this approach in both with static and instance scope of PostSharp aspect. It works fine, but static implementation makes it hard to unittest in parallel. Thx anyway.")]
        public void InjectionAspectWithCallbackAttribute_NullInjeced_ExpectedException()
        {
            var exception = Assert.Throws<Exception>(() => new FakeService().FakeCall());
            Assert.That(exception, Has.Message.EqualTo("CreateSomeDependency is null"));
        }
    }
}
