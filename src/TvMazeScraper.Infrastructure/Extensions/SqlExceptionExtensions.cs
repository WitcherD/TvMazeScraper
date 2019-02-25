using System;
using System.Data.SqlClient;
using System.Linq;

namespace TvMazeScraper.Infrastructure.Extensions
{
    /// <summary>
    /// SqlException extension methods
    /// </summary>
    public static class SqlExceptionExtensions
    {
        /// <summary>
        /// Is it unique constraint error
        /// </summary>
        public static bool IsSqlUniqueConstraint(this Exception ex)
        {
            // case 2627 Unique constraint error
            // case 547  Constraint check violation
            // case 2601 Duplicated key row error
            return HasSqlExceptionCode(ex, 2627, 547, 2601);
        }

        public static bool HasSqlExceptionCode(this Exception ex, params int[] codes)
        {
            if (codes.Any())
            {
                var sqlException = ex.GetUnderlyingException<SqlException>();
                if (sqlException != null)
                {
                    return codes.Contains(sqlException.Number);
                }
            }

            return false;
        }

        public static T GetUnderlyingException<T>(this Exception exception) where T : Exception
        {
            var current = exception;
            while (current != null && !(current is T))
            {
                current = current.InnerException;
            }
            return (T)current;
        }
    }
}
