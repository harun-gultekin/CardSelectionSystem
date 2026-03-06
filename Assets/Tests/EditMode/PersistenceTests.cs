using System.IO;
using NUnit.Framework;
using CardSelectionSystem.Core.Models;
using CardSelectionSystem.Core.Persistence;

namespace CardSelectionSystem.Tests
{
    [TestFixture]
    public class PersistenceTests
    {
        private string _tempFilePath;
        private JsonSaveService _saveService;

        [SetUp]
        public void SetUp()
        {
            _tempFilePath = Path.GetTempFileName();
            File.Delete(_tempFilePath);
            _saveService = new JsonSaveService(_tempFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFilePath))
                File.Delete(_tempFilePath);
        }

        [Test]
        public void SaveThenLoad_RestoresCorrectState()
        {
            var sequence = new[] { "Shield", "Evasion", "Pierce", "Headshot", "Sniper" };
            var original = new CycleState(sequence, 3);

            _saveService.Save(original);
            var loaded = _saveService.Load();

            Assert.IsNotNull(loaded);
            Assert.AreEqual(original.CurrentRound, loaded.CurrentRound);
            Assert.AreEqual(original.CycleSequence, loaded.CycleSequence);
        }

        [Test]
        public void Load_NoFile_ReturnsNull()
        {
            var result = _saveService.Load();

            Assert.IsNull(result);
        }

        [Test]
        public void Load_CorruptJson_ReturnsNull()
        {
            File.WriteAllText(_tempFilePath, "{{not valid json at all!!");

            var result = _saveService.Load();

            Assert.IsNull(result);
        }

        [Test]
        public void Delete_RemovesSaveFile()
        {
            var state = new CycleState(new[] { "Shield" }, 1);
            _saveService.Save(state);
            Assert.IsTrue(File.Exists(_tempFilePath));

            _saveService.Delete();

            Assert.IsFalse(File.Exists(_tempFilePath));
        }

        [Test]
        public void Save_OverwritesPreviousSave()
        {
            var first = new CycleState(new[] { "Shield", "Evasion" }, 1);
            _saveService.Save(first);

            var second = new CycleState(new[] { "Pierce", "Headshot", "Sniper" }, 2);
            _saveService.Save(second);

            var loaded = _saveService.Load();

            Assert.IsNotNull(loaded);
            Assert.AreEqual(2, loaded.CurrentRound);
            Assert.AreEqual(3, loaded.CycleSequence.Length);
            Assert.AreEqual("Pierce", loaded.CycleSequence[0]);
        }

        [Test]
        public void Load_EmptySequence_ReturnsNull()
        {
            File.WriteAllText(_tempFilePath, "{\"CycleSequence\":[],\"CurrentRound\":1}");

            var result = _saveService.Load();

            Assert.IsNull(result);
        }

        [Test]
        public void Delete_NoFile_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _saveService.Delete());
        }
    }
}
