using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Quest.API.Helpers
{
    public class ExactQueryParamAttribute : Attribute, IActionConstraint
    {
        private readonly string[] _keys;

        public ExactQueryParamAttribute(params string[] keys)
        {
            _keys = keys;
        }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var query = context.RouteContext.HttpContext.Request.Query;
            return query.Count == _keys.Length && _keys.All(key => query.ContainsKey(key));
        }
    }
}
