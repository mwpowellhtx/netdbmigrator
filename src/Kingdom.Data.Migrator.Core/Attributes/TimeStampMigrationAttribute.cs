using System;

namespace Kingdom.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TimeStampMigrationAttribute : AbstractMigrationAttribute
    {
        /// <summary>
        /// Gets the <see cref="AbstractMigrationAttribute.Value"/>
        /// as a <see cref="DateTime"/>.
        /// </summary>
        public DateTime TimeStamp
        {
            get { return (DateTime) Value; }
        }

        /// <summary>
        /// Gets an Id based on the <see cref="TimeStamp"/>.
        /// </summary>
        internal override long Id
        {
            get { return TimeStamp.ToLongId(); }
        }

        /// <summary>
        /// Gets the Kind: <see cref="AttributeKind.TimeStamp"/>.
        /// </summary>
        internal override AttributeKind Kind
        {
            get { return AttributeKind.TimeStamp; }
        }

        /// <summary>
        /// Gets the Text from the <see cref="TimeStamp"/>.
        /// </summary>
        internal override string Text
        {
            get { return TimeStamp.ToString(@"O"); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public TimeStampMigrationAttribute(int year, int month, int day,
            int hour, int minute, int second)
            : base(new DateTime(year, month, day, hour, minute, second))
        {
        }

        /// <summary>
        /// Verifies that the attribute is valid for use.
        /// </summary>
        internal override void Verify()
        {
        }
    }

    internal static class TimeStampExtensionMethods
    {
        /// <summary>
        /// Returns a long Id from the <see cref="timeStamp"/>.
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        internal static long ToLongId(this DateTime timeStamp)
        {
            var ts = timeStamp;
            return long.Parse(string.Format(@"{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}",
                ts.Year, ts.Month, ts.Day, ts.Hour, ts.Minute, ts.Second));
        }
    }
}
