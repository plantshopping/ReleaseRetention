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
        public void OneRelease_OneDeployment_KeepOneRelease_ReturnsCorrectRelease()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = "538fc083-130b-48cb-978b-5960e3b29093", Name = "Pets" } };

            var releases = new List<Release> { new Release { Id = "d943de9c-8a83-49f7-b301-a06e6ab176a2", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.1", Created = new DateTime(2000, 1, 1) } };

            var deployment = new List<Deployment> { new Deployment { Id = "0d02e3e6-3b00-4f54-9d03-d37f1e613a0f", ReleaseId = "d943de9c-8a83-49f7-b301-a06e6ab176a2", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 1) } };

            var environment = new List<Environment> { new Environment { Id = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", Name = "Staging" } };

            // Act
            var result = releaseRetention.CalculateRetention(projects, releases, deployment, environment, 1);

            // Assert
            var expectedResult = new ReleaseRetentionResult { ReleaseId = "d943de9c-8a83-49f7-b301-a06e6ab176a2", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03" };

            Assert.Single(result);
            Assert.Equal(expectedResult.ReleaseId, result[0].ReleaseId);
            Assert.Equal(expectedResult.EnvironmentId, result[0].EnvironmentId);
        }

        [Fact]
        public void MultipleReleases_OneDeployment_KeepOneRelease_ReturnsCorrectRelease()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = "538fc083-130b-48cb-978b-5960e3b29093", Name = "Pets" } };

            var releases = new List<Release> { new Release { Id = "d943de9c-8a83-49f7-b301-a06e6ab176a2", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.1", Created = new DateTime(2000, 1, 1) }, new Release { Id = "62976be4-253a-4dd1-8814-89aea49a4364", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.2", Created = new DateTime(2000, 1, 1) } };

            var deployment = new List<Deployment> { new Deployment { Id = "0d02e3e6-3b00-4f54-9d03-d37f1e613a0f", ReleaseId = "62976be4-253a-4dd1-8814-89aea49a4364", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 1) } };

            var environment = new List<Environment> { new Environment { Id = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", Name = "Staging" } };

            // Act
            var result = releaseRetention.CalculateRetention(projects, releases, deployment, environment, 1);

            // Assert
            var expectedResult = new ReleaseRetentionResult { ReleaseId = "62976be4-253a-4dd1-8814-89aea49a4364", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03" };

            Assert.Single(result);
            Assert.Equal(expectedResult.ReleaseId, result[0].ReleaseId);
            Assert.Equal(expectedResult.EnvironmentId, result[0].EnvironmentId);
        }

        [Fact]
        public void MultipleReleases_MultipleDeployments_SameEnvironment_KeepTwoRelease_ReturnsCorrectRelease()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = "538fc083-130b-48cb-978b-5960e3b29093", Name = "Pets" } };

            var releases = new List<Release> { new Release { Id = "d943de9c-8a83-49f7-b301-a06e6ab176a2", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.1", Created = new DateTime(2000, 1, 1) }, new Release { Id = "62976be4-253a-4dd1-8814-89aea49a4364", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.2", Created = new DateTime(2000, 1, 1) } };

            var deployment = new List<Deployment> { new Deployment { Id = "0d02e3e6-3b00-4f54-9d03-d37f1e613a0f", ReleaseId = "62976be4-253a-4dd1-8814-89aea49a4364", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 1) }, new Deployment { Id = "b1e672e9-7cb9-4878-b845-dee4a3b7e442", ReleaseId = "d943de9c-8a83-49f7-b301-a06e6ab176a2", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 2) } };

            var environment = new List<Environment> { new Environment { Id = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", Name = "Staging" } };

            // Act
            var result = releaseRetention.CalculateRetention(projects, releases, deployment, environment, 1);

            // Assert
            var expectedResult = new ReleaseRetentionResult { ReleaseId = "d943de9c-8a83-49f7-b301-a06e6ab176a2", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03" };

            Assert.Single(result);
            Assert.Equal(expectedResult.ReleaseId, result[0].ReleaseId);
            Assert.Equal(expectedResult.EnvironmentId, result[0].EnvironmentId);
        }

        [Fact]
        public void MultipleDeployments_DifferentEnvironment_KeepOneRelease_ReturnsCorrectRelease()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = "538fc083-130b-48cb-978b-5960e3b29093", Name = "Pets" } };

            var releases = new List<Release> { new Release { Id = "d943de9c-8a83-49f7-b301-a06e6ab176a2", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.1", Created = new DateTime(2000, 1, 1) }, new Release { Id = "62976be4-253a-4dd1-8814-89aea49a4364", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.2", Created = new DateTime(2000, 1, 1) } };

            var deployment = new List<Deployment> { new Deployment { Id = "b2de6832-1fe8-4a0b-a83c-6c6cbffb33e2", ReleaseId = "d943de9c-8a83-49f7-b301-a06e6ab176a2", EnvironmentId = "0cad7113-cb92-4ac3-a999-3fd71573a92a", DeployedAt = new DateTime(2000, 1, 1) }, new Deployment { Id = "b1e672e9-7cb9-4878-b845-dee4a3b7e442", ReleaseId = "d943de9c-8a83-49f7-b301-a06e6ab176a2", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 2) } };

            var environment = new List<Environment> { new Environment { Id = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", Name = "Staging" }, new Environment { Id = "0cad7113-cb92-4ac3-a999-3fd71573a92a", Name = "Production" } };

            // Act
            var result = releaseRetention.CalculateRetention(projects, releases, deployment, environment, 2);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("d943de9c-8a83-49f7-b301-a06e6ab176a2", result[0].ReleaseId);
            Assert.Equal("b29d67d1-dd1d-4c71-b144-6004c4cf2b03", result[0].EnvironmentId);

            Assert.Equal("d943de9c-8a83-49f7-b301-a06e6ab176a2", result[1].ReleaseId);
            Assert.Equal("0cad7113-cb92-4ac3-a999-3fd71573a92a", result[1].EnvironmentId);
        }

        [Fact]
        public void DifferentReleases_DifferentDeployments_DifferentEnvironment_KeepOneRelease_ReturnsCorrectRelease()
        {
            //Arrange
            var projects = new List<Project> { new Project { Id = "538fc083-130b-48cb-978b-5960e3b29093", Name = "Pets" }, new Project { Id = "739c22f8-e4f0-4dc8-b2fd-19506bd10069", Name = "Plants" } };

            var releases = new List<Release> {
                new Release { Id = "a0702078-f0de-4901-b984-9ee1c8e70c12", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.1", Created = new DateTime(2000, 1, 1) },
                 new Release { Id = "39971933-686d-425f-a63e-6c08be28649e", ProjectId = "538fc083-130b-48cb-978b-5960e3b29093", Version = "0.0.2", Created = new DateTime(2000, 1, 1) },
                new Release { Id = "8614d609-2199-4fd9-9fe0-b35675a6e279", ProjectId = "739c22f8-e4f0-4dc8-b2fd-19506bd10069", Version = "0.0.1", Created = new DateTime(2000, 1, 1) },
            };

            var deployment = new List<Deployment> { 
                new Deployment { Id = "26f55789-d647-4dd3-a7a8-0e468319be66", ReleaseId = "a0702078-f0de-4901-b984-9ee1c8e70c12", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 1) }, 
                new Deployment { Id = "d6e90f96-be97-4181-a3cc-09e8b18d7f14", ReleaseId = "39971933-686d-425f-a63e-6c08be28649e", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 2) },
              new Deployment { Id = "7fc5cef9-c393-4a4e-be7e-836f4aaab4a7", ReleaseId = "8614d609-2199-4fd9-9fe0-b35675a6e279", EnvironmentId = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", DeployedAt = new DateTime(2000, 1, 1) }
            };

            var environment = new List<Environment> { new Environment { Id = "b29d67d1-dd1d-4c71-b144-6004c4cf2b03", Name = "Staging" } };

            var result = releaseRetention.CalculateRetention(projects, releases, deployment, environment, 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("39971933-686d-425f-a63e-6c08be28649e", result[0].ReleaseId);
            Assert.Equal("b29d67d1-dd1d-4c71-b144-6004c4cf2b03", result[0].EnvironmentId);

            Assert.Equal("8614d609-2199-4fd9-9fe0-b35675a6e279", result[1].ReleaseId);
            Assert.Equal("b29d67d1-dd1d-4c71-b144-6004c4cf2b03", result[1].EnvironmentId);
        }

        // TODO Calculate release with no deployments, should return 0
    }
}
