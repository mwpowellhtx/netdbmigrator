using NUnit.Framework;

namespace Kingdom.Data
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        /// <summary>
        /// Sets up just prior to running each unit test.
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {
        }

        /// <summary>
        /// Tears down just after running each unit test.
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
        }

        /// <summary>
        /// Sets up the fixture prior to running all unit tests.
        /// </summary>
        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
        }

        /// <summary>
        /// Tears down the fixture after running all unit tests.
        /// </summary>
        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
        }
    }
}
