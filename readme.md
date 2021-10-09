## How to run
Run the unit tests located under ReleasesRetention.Test

## Assumptions
1. If a deployment doesn't have a valid environment id, we ignore that deployment and treat the release as if it was never deployed.
2. If a release doesn't have a deployment, don't bother retaining it.
3. If the same release was deployed to different environments, the release will appear twice in the result, one for each valid environment.
4. If the number of releases to keep is 0, we return empty list 😅