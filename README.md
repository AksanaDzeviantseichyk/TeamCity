#Task 21 - CI/CD (TeamCity)

Check that your minesweeper has 3 separated assemblies (1 solution with 3 projects):
- Core 
- Console
- Tests 

Install local TeamCity Server and agent and create new pipeline, trigered after new pushed commit:
- build solution
- run unit tests
- publish Core project as nuget package
