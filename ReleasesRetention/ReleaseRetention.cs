using System;
using System.Collections.Generic;
using System.Text;

namespace ReleasesRetention
{
    // TODO: Make interface, DI etc.
    public class ReleaseRetention
    {
        // TODO: Read the input data then pass it into here
        public List<ReleaseRetentionResult> CalculateRetention(List<Project> projects, List<Release> releases, List<Deployment> deployments, List<Environment> environments, int numberOfReleasesToKeep)
        {
            // TODO: Assume if number of releases to keep is 0, we return all
            return new List<ReleaseRetentionResult>();
        }
    }
}
