namespace Kingdom.Data
{
    public abstract class FluentlyAlterTestFixtureBase<TAlterTableFluently> : TestFixtureBase
        where TAlterTableFluently : class, IAlterTableFluently<TAlterTableFluently>, new()
    {
        protected FluentAlterRoot<TAlterTableFluently> Alter { get; private set; }

        public override void SetUp()
        {
            base.SetUp();

            Alter = new FluentAlterRoot<TAlterTableFluently>();
        }

        /// <summary>
        /// Helper method optionally setting the With condition.
        /// </summary>
        /// <param name="fluently"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        protected static TAlterTableFluently With(TAlterTableFluently fluently, CheckType? checkType)
        {
            // Setting, then "unsetting" the With Check|NoCheck here is deliberate to test the outcome.
            if (checkType == null)
                fluently = fluently.With(CheckType.Check);

            return checkType == null
                ? fluently.With()
                : fluently.With(checkType.Value);
        }
    }
}
