using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Spectre.IO;

namespace Autograph
{
    public sealed class ProjectComparer : EqualityComparer<Project>
    {
        private readonly PathComparer _comparer;

        public ProjectComparer()
        {
            _comparer = new PathComparer(false);
        }

        public override bool Equals([AllowNull] Project x, [AllowNull] Project y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return _comparer.Equals(x.Path, y.Path);
        }

        public override int GetHashCode([DisallowNull] Project obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return _comparer.GetHashCode(obj.Path);
        }
    }
}
