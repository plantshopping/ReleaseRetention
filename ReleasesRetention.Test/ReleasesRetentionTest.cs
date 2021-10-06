using System;
using System.Collections.Generic;
using Xunit;

namespace ReleasesRetention.Test
{
    public class ReleasesRetentionTest
    {
        private readonly ReleaseRetention releaseRetention;

        public ReleasesRetentionTest()
        {
            releaseRetention = new ReleaseRetention();
        }

        [Fact]
        public void Calculate_OneReleaseToKeep_ReturnsLatestRelease()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = "538fc083-130b-48cb-978b-5960e3b29093", Name = "Pets" } };

            var releases = new List<Release> { new Release { Id = "d943de9c-8a83-49f7-b301-a06e6ab176a2", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.1", Created = new DateTime(2000, 1, 1) } };

            var deployment = new List<Deployment> { new Deployment { Id = "0d02e3e6-3b00-4f54-9d03-d37f1e613a0f", ReleaseId = "d943de9c-8a83-49f7-b301-a06e6ab176a2", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 1) } };

            var environment = new List<Environment> { new Environment { Id = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", Name = "Staging" } };

            // Act
            var result = releaseRetention.CalculateRetention(projects, releases, deployment, environment, 0);

            // Assert
            var expectedResult = new ReleaseRetentionResult { ReleaseId = "d943de9c-8a83-49f7-b301-a06e6ab176a2", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03" };

            Assert.Equal(expectedResult.ReleaseId, result[0].ReleaseId);
            Assert.Equal(expectedResult.EnvironmentId, result[0].ReleaseId);
        }

        [Fact]
        public void Calculate_TwoReleasesToKeep_ReturnsLatestRelease()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = "538fc083-130b-48cb-978b-5960e3b29093", Name = "Pets" } };

            var releases = new List<Release> { new Release { Id = "d943de9c-8a83-49f7-b301-a06e6ab176a2", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.1", Created = new DateTime(2000, 1, 1) }, new Release { Id = "62976be4-253a-4dd1-8814-89aea49a4364", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.2", Created = new DateTime(2000, 1, 1) } };

            var deployment = new List<Deployment> { new Deployment { Id = "0d02e3e6-3b00-4f54-9d03-d37f1e613a0f", ReleaseId = "62976be4-253a-4dd1-8814-89aea49a4364", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 1) } };

            var environment = new List<Environment> { new Environment { Id = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", Name = "Staging" } };

            // Act
            var result = releaseRetention.CalculateRetention(projects, releases, deployment, environment, 0);

            // Assert
            var expectedResult = new ReleaseRetentionResult { ReleaseId = "62976be4-253a-4dd1-8814-89aea49a4364", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03" };

            Assert.Equal(expectedResult.ReleaseId, result[0].ReleaseId);
            Assert.Equal(expectedResult.EnvironmentId, result[0].ReleaseId);
        }
    }
}
