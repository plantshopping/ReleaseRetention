using System.Collections.Generic;
using System.Linq;

namespace ReleasesRetention
{
    // TODO: Make interface, DI etc.
    public class ReleaseRetention
    {
        // TODO: Read the input data then pass it into here
        public List<ReleaseRetentionResult> CalculateRetention(List<Project> projects, List<Release> releases, List<Deployment> deployments, List<Environment> environments, int numberOfReleasesToKeep)
        {
            if (numberOfReleasesToKeep == 0) return new List<ReleaseRetentionResult>();

            var projectReleaseJoinResult = from project in projects
                                           join release in releases on project.Id equals release.ProjectId
                                           select new { ReleaseId = release.Id, ProjectId = project.Id };

            var projectReleaseDeploymentJoin = from projectReleaseJoin in projectReleaseJoinResult
                                               join deployment in deployments on projectReleaseJoin.ReleaseId equals deployment.ReleaseId
                                               select new { ProjectId = projectReleaseJoin.ProjectId, ReleaseId = deployment.ReleaseId, EnvironmentId = deployment.EnvironmentId, DeployedAt = deployment.DeployedAt };

            var orderByProject = projectReleaseDeploymentJoin.GroupBy(p => p.ProjectId).Distinct();

            var result = new List<ReleaseRetentionResult>();
            
            projects.ForEach(project =>
            {
                if (orderByProject.Count() == 0) return;

                var orderedProjects = orderByProject.Single(p => p.Key == project.Id).ToList().OrderByDescending(p => p.DeployedAt).Take(numberOfReleasesToKeep);
                var selectResult = orderedProjects.Select(r => new ReleaseRetentionResult { ReleaseId = r.ReleaseId, EnvironmentId = r.EnvironmentId });
                result.AddRange(selectResult);
            });

            // TODO: Could validate that the environment exists
            return result;
        }
    }
}
