
using System.Collections.Generic;
using DuraApp.Core.Helpers.Enums;

namespace DuraApp.Core.Models.Result
{
    /// <summary>
    /// Unauthorized result.
    /// </summary>
    public class UnauthorizedResult<T> : Result<T>
    {
        private string _error;
        public UnauthorizedResult(string error)
        {
            _error = error;
        }
        public override ResultType ResultType => ResultType.Unauthorized;

        public override List<string> Errors => new List<string> { _error ?? "You are not authorized to access this." };

        public override T Data => default(T);
    }
}
