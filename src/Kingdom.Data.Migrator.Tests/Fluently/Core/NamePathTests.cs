using System;
using System.Collections.Generic;

namespace Kingdom.Data
{
    using NUnit.Framework;
    using StringValuesFixture = ValuesFixture<string>;

    public class NamePathTests : TestFixtureBase
    {
        private const string Empty = "";
        private const string Brackets = "[]";

        private static IEnumerable<TestCaseData> GetNamePathCorrectTestCases()
        {
            Func<string, string> stringify = s => string.Format(@"""{0}""", s);

            yield return new TestCaseData("foo.bar", Empty, true, new StringValuesFixture(stringify, "foo", "", "bar"));
            yield return new TestCaseData("foo..bar", Empty, false, new StringValuesFixture(stringify, "foo", "", "bar"));

            yield return new TestCaseData("[foo].[bar]", Brackets, true, new StringValuesFixture(stringify, "foo", "", "bar"));
            yield return new TestCaseData("[foo].[].[bar]", Brackets, false, new StringValuesFixture(stringify, "foo", "", "bar"));

            yield return new TestCaseData("[baz]", Brackets, true, new StringValuesFixture(stringify, "baz"));
            yield return new TestCaseData("[baz]", Brackets, false, new StringValuesFixture(stringify, "baz"));
        }

        private IEnumerable<TestCaseData> NamePathCorrectTestCases
        {
            get { return GetNamePathCorrectTestCases(); }
        }

        private static class TestCases
        {
            internal const string NamePathCorrectTestCases = @"NamePathCorrectTestCases";
        }

        [Test]
        [TestCaseSource(TestCases.NamePathCorrectTestCases)]
        public void VerifyNamePathCorrect(string expected, string decorator,
            bool ignoreEmptyNodes, StringValuesFixture fixture)
        {
            var path = new NamePath(fixture.Values)
            {
                IgnoreEmptyNodes = ignoreEmptyNodes,
                NodeDecorator = decorator
            };

            Assert.That(path.ToString(), Is.EqualTo(expected));
        }
    }
}
