using NyxVenture.datamodel;
using System.ComponentModel;
using System.Text;

namespace NyxVenture.Test
{
    [TestClass]
    public class ModelEventHandlingTests
    {
        [TestMethod]
        public void TestStringChangedEvent()
        {
            bool changedEvent = false;
            PropertyChangedEventArgs? eventArgs = null;

            Game game = new Game();
            game.PropertyChanged += (sender, args) =>
            {
                changedEvent = true;
                eventArgs = args;
            };

            Assert.IsFalse(game.IsObjectChanged);
            Assert.IsNull(game.Title);

            game.Title = "new game";
            Assert.IsTrue(game.IsObjectChanged);
            Assert.IsTrue(changedEvent);
            Assert.IsNotNull(game.Title);
            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(nameof(game.Title), eventArgs.PropertyName);
        }

        [TestMethod]
        public void TestIntChangedEvent()
        {
            bool changedEvent = false;
            PropertyChangedEventArgs? eventArgs = null;

            Feature feature = new Feature();
            feature.PropertyChanged += (sender, args) =>
            {
                changedEvent = true;
                eventArgs = args;
            };

            Assert.IsFalse(feature.IsObjectChanged);
            Assert.AreEqual(0, feature.MinValue);

            feature.MinValue = -1;
            Assert.IsTrue(feature.IsObjectChanged);
            Assert.IsTrue(changedEvent);
            Assert.AreEqual(-1, feature.MinValue);
            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(nameof(feature.MinValue), eventArgs.PropertyName);
        }

        [TestMethod]
        public void TestObjectChangedEvent()
        {
            bool changedEvent = false;
            PropertyChangedEventArgs? eventArgs = null;

            Game game = new Game();
            game.PropertyChanged += (sender, args) =>
            {
                changedEvent = true;
                eventArgs = args;
            };

            Chapter chapter = new Chapter();
            Assert.IsNull(game.StartChapter);
            Assert.IsFalse(game.IsObjectChanged);

            game.SetStartChapter(chapter);
            Assert.AreEqual(chapter, game.StartChapter);
            Assert.IsTrue(game.IsObjectChanged);
            Assert.IsTrue(changedEvent);
            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(nameof(game.StartChapter), eventArgs.PropertyName);
        }

        [TestMethod]
        public void TestListEvents()
        {
            bool changedEvent = false;
            PropertyChangedEventArgs? eventArgs = null;

            Game game = new Game();
            game.PropertyChanged += (sender, args) =>
            {
                changedEvent = true;
                eventArgs = args;
            };

            Feature[] features = game.Features;
            Assert.AreEqual(0, features.Length);
            Assert.IsFalse(game.IsObjectChanged);

            Feature feature1 = new Feature();
            Feature feature2 = new Feature();
            game.AddFeature(feature1);
            game.AddFeature(feature2);
            features = game.Features;
            Assert.AreEqual(2, features.Length);
            Assert.AreEqual(feature1, features[0]);
            Assert.AreEqual(feature2, features[1]);
            Assert.IsTrue(game.IsObjectChanged);
            Assert.IsTrue(changedEvent);
            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(nameof(game.Features), eventArgs.PropertyName);

            game.CleanChangedFlags();
            changedEvent = false;
            eventArgs = null;
            Assert.IsFalse(game.IsObjectChanged);

            game.RemoveFeature(feature1);
            features = game.Features;
            Assert.AreEqual(1, features.Length);
            Assert.IsTrue(game.IsObjectChanged);
            Assert.IsTrue(changedEvent);
            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(nameof(game.Features), eventArgs.PropertyName);
        }

        [TestMethod]
        public void TestObjectBubbleEvents()
        {
            bool subnodeChanged = false;
            BubbleChangeEventArgs? eventArgs = null;

            Game game = new Game();
            game.ModelChanged += (args) =>
            {
                subnodeChanged = true;
                eventArgs = args;
            };

            Chapter chapter = new Chapter();
            game.SetStartChapter(chapter);
            Assert.IsFalse(subnodeChanged);

            subnodeChanged = false;
            chapter.Name = "New chapter";
            Assert.IsTrue(subnodeChanged);

            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(nameof(chapter.Name), eventArgs.PropertyInformation.PropertyName);

            List<ModelBase> path = eventArgs.PathInformation;
            Assert.AreEqual(game, path[0]);
            Assert.AreEqual(chapter, path[1]);
        }

        [TestMethod]
        public void TestListBubbleEvents()
        {
            bool subnodeChanged = false;
            BubbleChangeEventArgs? eventArgs = null;

            Game game = new Game();
            game.ModelChanged += (args) =>
            {
                subnodeChanged = true;
                eventArgs = args;
            };

            Feature feature = game.CreateFeature();
            Assert.IsFalse(subnodeChanged);

            feature.Name = "new feature";
            Assert.IsTrue(subnodeChanged);

            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(nameof(feature.Name), eventArgs.PropertyInformation.PropertyName);

            List<ModelBase> path = eventArgs.PathInformation;
            Assert.AreEqual(game, path[0]);
            Assert.AreEqual(feature, path[1]);

            subnodeChanged = false;
            eventArgs = null; 

            // features added with the Add.. function must not cause a BubbleEvent
            Feature health = new Feature();
            game.AddFeature(health);
            Assert.IsFalse(subnodeChanged);

            health.Name = "Health";
            Assert.IsFalse(subnodeChanged);
            Assert.IsNull(eventArgs);
        }
    }
}