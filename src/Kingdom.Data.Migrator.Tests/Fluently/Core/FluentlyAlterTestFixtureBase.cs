namespace Kingdom.Data
{
    public abstract class FluentlyAlterTestFixtureBase<TAlterTableFluently> : TestFixtureBase
        where TAlterTableFluently : class, IAlterTableFluently, new()
    {
        protected FluentAlterRoot<TAlterTableFluently> Alter { get; private set; }

        public override void SetUp()
        {
            base.SetUp();

            Alter = new FluentAlterRoot<TAlterTableFluently>();
        }
    }
}
