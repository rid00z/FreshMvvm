using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FreshMvvm.Tests.Fixtures
{
    [TestFixture]
    public class FreshNavigationCommandFixture
    {
        SharedLock _sharedLock;

        [SetUp]
        public void Setup()
        {
            _sharedLock = new SharedLock();
        }

        [Test]
        public async Task OnlyOneCommandWillExecuteTests()
        {
            //Flow: A executes, B executes, A starts, B ignored, A completes.
            int countBefore = 0;
            int countAfter = 0;
            Func<object, Task> execute = async (obj) =>
            {
                countBefore++;
                await Task.Delay(100);
                countAfter++;
            };

            //Execute is async void, so this will run in the background.
            var first = new FreshNavigationCommand(execute, _sharedLock).ExecuteAsync(null);
            var second = new FreshNavigationCommand(execute, _sharedLock).ExecuteAsync(null);

            await Task.WhenAll(first, second); //Need to let execute finish.

            Assert.AreEqual(1, countBefore);
            Assert.AreEqual(1, countAfter);
        }
    }
}
