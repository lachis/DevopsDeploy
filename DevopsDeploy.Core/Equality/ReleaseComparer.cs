using System;
using System.Collections.Generic;
using DevopsDeploy.Domain.DTO;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Core.Equality
{
    public class ReleaseComparer : IEqualityComparer<ReleaseDTO>
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

        public bool Equals(ReleaseDTO x, ReleaseDTO y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return Equals(x.Release.Id, y.Release.Id);
        }

        public int GetHashCode(ReleaseDTO obj)
        {
            return (obj.Release != null ? obj.Release.GetHashCode() : 0);
        }
    }
}