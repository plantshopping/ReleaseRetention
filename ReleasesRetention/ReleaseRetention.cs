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

            // TODO: Look at DeployedAt time to determine the latest release

            var projectReleaseJoinResult = from project in projects
                        join release in releases on project.Id equals release.ProjectId
                        select new { ProjectId = project.Id, ProjectName = project.Name, ReleaseId = release.Id, ReleaseVersion = release.Version, ReleaseCreated = release.Created };

            var projectReleaseDeploymentJoin = from projectReleaseJoin in projectReleaseJoinResult
                                               join deployment in deployments on projectReleaseJoin.ReleaseId equals deployment.ReleaseId
                                               select new { ReleaseId = deployment.ReleaseId, EnvironmentId = deployment.EnvironmentId, DeployedAt = deployment.DeployedAt };


            // TODO: Hardcode first one for now
            var firstResult = new ReleaseRetentionResult
            {
                ReleaseId = projectReleaseDeploymentJoin.First().ReleaseId,
                EnvironmentId = projectReleaseDeploymentJoin.First().EnvironmentId
            };

            return new List<ReleaseRetentionResult> { firstResult };
        }
    }
}
