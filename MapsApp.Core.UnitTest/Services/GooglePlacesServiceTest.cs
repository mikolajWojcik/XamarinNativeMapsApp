using MapsApp.Services;
using MapsApp.Services.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapsAppUnitTest.Services
{
    [TestFixture]
    public class GooglePlacesServiceTest
    {
        private IGooglePlacesService _service;

        [SetUp]
        protected void SetUp()
        {
            _service = new GooglePlacesService();
        }

        [Test]
        public void ShouldThrowExceptionForNullArgument()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await _service.GetPlacesAsync(null);
            });
        }

        [Test]
        public void ShouldThrowExceptionForEmptyArgument()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await _service.GetPlacesAsync(string.Empty);
            });
        }

        [Test]
        public void ShouldThrowExceptionForWhitespaceArgument()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await _service.GetPlacesAsync("  ");
            });
        }

        [Test]
        public async Task ShouldReturnOneResultForPreciseArgument()
        {
            var result = await _service.GetPlacesAsync("New York");

            Assert.Multiple(() =>
            {
                Assert.AreEqual(result.Count(), 1);
                Assert.IsTrue(result.Any(x => x.Name == "New York"));
            });         
        }

        [Test]
        public async Task ShouldFirePropertyChangedEventWhenIsNextPageAviableChanged()
        {
            var wasCalled = false;
            _service.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_service.IsNextPageAvailable))
                    wasCalled = true;
            };
            var result = await _service.GetPlacesAsync("square");

            Assert.IsTrue(wasCalled);
        }

        [Test]
        public async Task ShouldThrowExceptionWhenNextPageIsNotAviable()
        {           
            await _service.GetPlacesAsync("New York").ContinueWith((t) =>
            {
                Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await _service.LoadMorePlacesAsync();
                });
            });
        }

        [Test]
        public async Task ShouldLoadNextPageWhenAviable()
        {
            await _service.GetPlacesAsync("square").ContinueWith(async (t) =>
            {
                var result = await _service.LoadMorePlacesAsync();

                Assert.IsNotEmpty(result);
            });
        }
    }
}
