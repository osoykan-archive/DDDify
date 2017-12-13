using System;
using System.Transactions;

namespace DDDify.Uow
{
    /// <summary>
    ///     Unit of work options.
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        ///     Timeout of UOW As milliseconds.
        ///     Uses default value if not supplied.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        ///     If this UOW is transactional, this option indicated the isolation level of the transaction.
        ///     Uses default value if not supplied.
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        ///     Can be used to enable/disable the LazyLoad.
        /// </summary>
        public bool? IsLazyLoadEnabled { get; set; }
    }
}
