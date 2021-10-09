using System.Collections.Generic;
using System.Linq;

namespace ReleasesRetention
{
    public class ReleaseRetention
    {
        public string GetReleasesToKeep(List<Project> projects, List<Release> releases, List<Deployment> deployments, List<Environment> environments, int numberOfReleasesToKeep)
        {
            var result = CalculateReleasesToKeep(projects, releases, deployments, environments, numberOfReleasesToKeep);
            return FormatResult(result);
        }

        public List<ReleaseRetentionResult> CalculateReleasesToKeep(List<Project> projects, List<Release> releases, List<Deployment> deployments, List<Environment> environments, int numberOfReleasesToKeep)
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

                var matchingProject = orderByProject.Single(p => p.Key == project.Id).ToList();

                var validEnvironmentIdProjects = matchingProject.Where(p => environments.Select(e => e.Id).Contains(p.EnvironmentId));

                validEnvironmentIdProjects.GroupBy(p => p.EnvironmentId).ToList().ForEach(p =>
                {
                    var orderedProjects = p.ToList().OrderByDescending(p => p.DeployedAt).Take(numberOfReleasesToKeep);

                    var selectResult = orderedProjects.Select(r => new ReleaseRetentionResult { ReleaseId = r.ReleaseId, EnvironmentId = r.EnvironmentId });

                    result.AddRange(selectResult);
                });
            });

            return result;
        }

        public string FormatResult(List<ReleaseRetentionResult> result)
        {
            var formatter = new ReleaseRetentionResultFormatter();
            return formatter.Format(result);
        }
    }
}
