using System;
using NUnit.Framework;
using PostSharp.Extensibility;
using Rhino.Mocks;

namespace AOP.Hydra.PostSharp.UnitTests
{
    [TestFixture]
    public class InjectedAspectTests
    {
        [Test]
        public void InjectedAspect_Dependency_ShouldBeCommited()
        {
            // Arrange
            var mockRepository = new MockRepository();

            var transactionMock = mockRepository.DynamicMock<ITransaction>();

            using (mockRepository.Record())
            {
                transactionMock.Expect(m => m.Commit()).Repeat.Once();
            }

            var sut = new FakeService { Transaction = transactionMock };

            // Act
            sut.Process();

            // Assert
            transactionMock.VerifyAllExpectations();
        }

        private class FakeService : IService
        {
            public ITransaction Transaction { get; set; }

            [InjectedAspect]
            public void Process()
            {
                Console.WriteLine("Processing...");
            }
        }
    }
}