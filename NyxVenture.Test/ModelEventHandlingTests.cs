using NyxVenture.datamodel;

namespace NyxVenture.Test
{
    [TestClass]
    public class ModelEventHandlingTests
    {
        [TestMethod]
        public void TestStringChangedEvent()
        {
            bool changedEvent = false;

            Game game = new Game();
            game.PropertyChanged += (sender, args) =>
            {
                changedEvent = true;
            };

            Assert.IsFalse(game.IsObjectChanged);
            Assert.IsNull(game.Title);

            game.Title = "new game";
            Assert.IsTrue(game.IsObjectChanged);
            Assert.IsTrue(changedEvent);
            Assert.IsNotNull(game.Title);
        }

        [TestMethod]
        public void TestIntChangedEvent()
        {
            bool changedEvent = false;

            Feature feature = new Feature();
            feature.PropertyChanged += (sender, args) =>
            {
                changedEvent = true;
            };

            Assert.IsFalse(feature.IsObjectChanged);
            Assert.AreEqual(0, feature.MinValue);

            feature.MinValue = -1;
            Assert.IsTrue(feature.IsObjectChanged);
            Assert.IsTrue(changedEvent);
            Assert.AreEqual(-1, feature.MinValue);
        }

        [TestMethod]
        public void TestObjectChangedEvent()
        {
            bool changedEvent = false;

            Game game = new Game();
            game.PropertyChanged += (sender, args) =>
            {
                changedEvent = true;
            };

            Chapter chapter = new Chapter();
            Assert.IsNull(game.StartChapter);
            Assert.IsFalse(game.IsObjectChanged);

            game.StartChapter = chapter;
            Assert.AreEqual(chapter, game.StartChapter);
            Assert.IsTrue(game.IsObjectChanged);
            Assert.IsTrue(changedEvent);
        }

        [TestMethod]
        public void TestListEvents()
        {
            bool changedEvent = false;

            Game game = new Game();
            game.PropertyChanged += (sender, args) =>
            {
                changedEvent = true;
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

            game.CleanChangedFlags();
            changedEvent = false;
            Assert.IsFalse(game.IsObjectChanged);

            game.RemoveFeature(feature1);
            features = game.Features;
            Assert.AreEqual(1, features.Length);
            Assert.IsTrue(game.IsObjectChanged);
            Assert.IsTrue(changedEvent);
        }
    }
}