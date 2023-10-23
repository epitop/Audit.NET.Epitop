using System.Collections.Generic;
using System.Threading.Tasks;
using Audit.Core;
using Audit.Core.Providers;
using NUnit.Framework;

namespace Audit.UnitTest
{
    public class InMemoryDataProviderTests
    {
        public class CustomAuditEvent : AuditEvent
        {
            public int AuditEventId { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            Audit.Core.Configuration.AuditDisabled = false;
            Audit.Core.Configuration.ResetCustomActions();
        }

        [TestCase(EventCreationPolicy.InsertOnEnd)]
        [TestCase(EventCreationPolicy.InsertOnStartReplaceOnEnd)]
        public void Test_InMemoryDataProvider(EventCreationPolicy creationPolicy)
        {
            Audit.Core.Configuration.Setup()
                .UseInMemoryProvider()
                .WithCreationPolicy(creationPolicy);

            var ev = new CustomAuditEvent() { AuditEventId = 123 };
            var target = new List<string>() { "initial" };
            using (var scope = AuditScope.Create(new AuditScopeOptions() { EventType = "test", TargetGetter = () => target, AuditEvent = ev }))
            {
                target.Add("final");
            }

            var dp = (InMemoryDataProvider)Configuration.DataProvider;
            var event0 = dp.GetEvent(0);
            var allEvents = dp.GetAllEvents();
            var allCustomEvents = dp.GetAllEventsOfType<CustomAuditEvent>();

            Assert.IsNotNull(event0);
            Assert.IsNotNull(allEvents);
            Assert.IsNotNull(allCustomEvents);
            Assert.AreEqual(1, allEvents.Count);
            Assert.AreEqual(1, allCustomEvents.Count);

            var eventObject = event0.Target.EventObject as List<string>;
            
            Assert.IsNotNull(eventObject);
            Assert.AreEqual(2, eventObject.Count);
            Assert.AreEqual("initial", eventObject[0]);
            // Assert.AreEqual("initial", @new[0]);
            // Assert.AreEqual("final", @new[1]);
            Assert.AreEqual("test", event0.EventType);
            Assert.AreEqual("test", allEvents[0].EventType);
            Assert.AreEqual("test", allCustomEvents[0].EventType);
            Assert.AreEqual(123, allCustomEvents[0].AuditEventId);

            dp.ClearEvents();

            var allEventsAfterClear = dp.GetAllEvents();
            Assert.AreEqual(0, allEventsAfterClear.Count);
        }

        [TestCase(EventCreationPolicy.InsertOnEnd)]
        [TestCase(EventCreationPolicy.InsertOnStartReplaceOnEnd)]
        public async Task Test_InMemoryDataProviderAsync(EventCreationPolicy creationPolicy)
        {
            Audit.Core.Configuration.Setup()
                .UseInMemoryProvider()
                .WithCreationPolicy(creationPolicy);

            var ev = new CustomAuditEvent() { AuditEventId = 123 };
            var target = new List<string>() { "initial" };
            using (var scope = await AuditScope.CreateAsync(new AuditScopeOptions() { EventType = "test", TargetGetter = () => target, AuditEvent = ev }))
            {
                target.Add("final");
            }

            var dp = (InMemoryDataProvider)Configuration.DataProvider;
            var event0 = await dp.GetEventAsync(0);
            var allEvents = dp.GetAllEvents();
            var allCustomEvents = dp.GetAllEventsOfType<CustomAuditEvent>();

            Assert.IsNotNull(event0);
            Assert.IsNotNull(allEvents);
            Assert.IsNotNull(allCustomEvents);
            Assert.AreEqual(1, allEvents.Count);
            Assert.AreEqual(1, allCustomEvents.Count);

            var eventObject = event0.Target.EventObject as List<string>;

            Assert.IsNotNull(eventObject);
            Assert.AreEqual(2, eventObject.Count);
            Assert.AreEqual("initial", eventObject[0]);
            // Assert.AreEqual("initial", @new[0]);
            // Assert.AreEqual("final", @new[1]);
            Assert.AreEqual("test", event0.EventType);
            Assert.AreEqual("test", allEvents[0].EventType);
            Assert.AreEqual("test", allCustomEvents[0].EventType);
            Assert.AreEqual(123, allCustomEvents[0].AuditEventId);

            dp.ClearEvents();

            var allEventsAfterClear = dp.GetAllEvents();
            Assert.AreEqual(0, allEventsAfterClear.Count);
        }
    }
}