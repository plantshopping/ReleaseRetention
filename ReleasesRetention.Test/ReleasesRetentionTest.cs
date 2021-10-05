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


            // Act
            var result = releaseRetention.CalculateRetention(0);

            // Assert
            Assert.Empty(result);
        }
    }
}
