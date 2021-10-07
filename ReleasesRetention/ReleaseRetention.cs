using System;
using System.Collections.Generic;
using System.Linq;
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

            var projectReleaseJoinResult = from project in projects
                                           join release in releases on project.Id equals release.ProjectId
                                           select new { ReleaseId = release.Id };

            var projectReleaseDeploymentJoin = from projectReleaseJoin in projectReleaseJoinResult
                                               join deployment in deployments on projectReleaseJoin.ReleaseId equals deployment.ReleaseId
                                               select new { ReleaseId = deployment.ReleaseId, EnvironmentId = deployment.EnvironmentId, DeployedAt = deployment.DeployedAt };
            
            var result = projectReleaseDeploymentJoin.OrderByDescending(r => r.DeployedAt).Take(numberOfReleasesToKeep).Select(r => new ReleaseRetentionResult { ReleaseId = r.ReleaseId, EnvironmentId = r.EnvironmentId});

            return result.ToList();
        }
    }
}
