using NUnit.Framework;
using UnityEngine;

namespace BP.RefPool.Tests
{
    public class RefUtilsTests
    {
        [Test]
        public void ClampMinFloat_SmallerThanMin_ShouldBeMin()
        {
            float value = -10;
            float min = 1;
            float result = RefUtils.ClampMin(value, min);
            Assert.AreEqual(result, min);
        }

        [Test]
        public void ClampMinFloat_ValueLargerThanMin_ShouldBeSame()
        {
            float value = 100;
            float min = 1;
            float result = RefUtils.ClampMin(value, min);
            Assert.AreEqual(result, value);
        }

        [Test]
        public void ClampMinInt_SmallerThanMin_ShouldBeMin()
        {
            int value = -10;
            int min = 1;
            int result = RefUtils.ClampMin(value, min);
            Assert.AreEqual(result, min);
        }

        [Test]
        public void ClampMinInt_ValueLargerThanMin_ShouldBeSame()
        {
            int value = 100;
            int min = 1;
            int result = RefUtils.ClampMin(value, min);
            Assert.AreEqual(result, value);
        }

        [Test]
        public void CreatePooledItem_PoolerNull_ReturnsNull()
        {
            var pooledItem = RefUtils.CreatePooledItem(null);
            Assert.IsNull(pooledItem);
        }

        [Test]
        public void CreatePooledItem_MockPrefab_ReturnsNewPooledItem()
        {
            var mockPooler = CreateMockPooler();
            var prefab = new GameObject("MockPrefab");
            mockPooler.Prefab = prefab;

            var pooledItem = RefUtils.CreatePooledItem(mockPooler);
            Assert.NotNull(pooledItem);
            Assert.IsTrue(pooledItem.TryGetComponent<RefItem>(out var refItem));
            Assert.AreEqual(mockPooler, refItem.Releasable);
        }

        private RefPooler CreateMockPooler()
        {
            var mockObject = new GameObject("MockPooler");
            var pooler = mockObject.AddComponent<RefPooler>();
            pooler.Prefab = new GameObject("MockPrefab");
            return pooler;
        }
    }
}