using System;
using System.Collections.Generic;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Core.Equality
{
    public class ReleaseComparer : IEqualityComparer<Release>
    {
        public bool Equals(Release x, Release y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.Id, y.Id, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Release obj)
        {
            return (obj.Id != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Id) : 0);
        }
    }
}