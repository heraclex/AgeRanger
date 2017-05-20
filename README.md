NOTE: you can see file Summary.pdf for more detail, include Architechture Design

# Introduction
- Restore NuGet packages before building solution.
- Database: there were 3 *.db files:
	1. Databases\RunningDb\AgeRanger.db : for development 
	2. Databases\IntegrationTestDb\AgeRanger.db for running integration tests. No need to change ConnectionString
	3. Databases\AcceptanceTestDb\AgeRanger.db for running acceptance tests. Remember change connectionString in AgeRange\AgeRanger.WebApp\ConnectionString.config and point it to this database when perform Acceptance test.

# Development Environment
	- Visual Studio 2017 Pro.
	- Git.
	
# Back End:
	- ASP.Net MVC 5 & Web API 2.
 	- .Net Framework 4.5.2
 	- Autofac (DI)
 	- Log4Net
 	- EntityFramework 6
 	- SQLite 3
# Front End:
	- Angularjs 1.6 && angular-ui
	- Twitter Boostrap 3
	- toastrjs (john papa)(popup notifier)
# Testing:
	- Moq
	- Microsoft VS Test Framework
	- Selenium

**Business Requirements**

1. [Done] Displays a list of people in the DB with their First and Last names, age and their age group.
2. [Done] Search for a person by his/her first or last name and displays all relevant information for the person - first and last names, age, age group.
3. [Done] Add a new person - every person has the first name, last name, and age;
4. [Done] Edit existing person records and expose a WEB API
5. [Done] Delete an existing person.
6. [Done] All Unit Test are covered.
7. [ToDo] Migrating Data to SQL Server.

**System Design Requirements**
1. [Done] Single Page Application (Refer to ../AgeRange.WebApp/app/app.js)
2. [Done] Unit testing with all cases covered.
3. [Done] Integration testing. 
4. [Done] Acceptance testing. 

**Solution Structure**

* Projects

- AgeRanger.WebApp - Presentation layer with ASP.NET MVC Web Site with a SPA using AngularJS. Perform all UI logic.
- AgeRanger.Service - Domain Model and Service. Service is responsible for perform business logic and return Domain Model included all information that Presentation layer need. 
- AgeRanger.Repository - Generic of Data access layer (as a wrapper). Accessing to Db to retreive data.
- AgeRanger.DbContext - Use EF6 and SQLite to communicate with Db. All entities and Entity-Mappings are defined here.
- AgeRanger.DataBaseSchema - Project for DBA to migrate data to SQL Server.

* Test Projects

- AgeRange.Tests.UnitTest : Test all methods with no dependencies
- AgeRange.Tests.IntegrationTest: Start test point from service layers to database: Service->Repository->Db
- AgeRange.Tests.AcceptanceTest: Automation Test performed by selenium to make sure all feature work well before release time.

**Next Improvements**

1. Security - Security consideration:
	- JSON Web Token Authentication
	- Cross Site Anti Forgery
	- Role Based Security : only administrators have access right to Add/edit.

2. Consistent Error handling and message between contrpllerpi and client
3. Implement validation at client side
4. Confirmation popup when delete person
5. Adding Unit Test in AngularApp

# Adding Interceptor Log all system:

INFO  2017-05-20 14:02:55,619 [44] ApiActionFilters       - ---> [API] Start Executing AgeRanger.WebApp.Controllers.Api.PersonController, action Filter
INFO  2017-05-20 14:02:55,620 [44] ServiceCallInterceptor - [Service] - Call method: IAgeRangerService.FindPeople ()
INFO  2017-05-20 14:02:55,620 [44] ositoryCallInterceptor - [Repository] - Call method: IAgeRangerRepository`1.GetAll()
INFO  2017-05-20 14:02:55,621 [63] ApiActionFilters       - ---> [API] Start Executing AgeRanger.WebApp.Controllers.Api.PersonController, action Filter
INFO  2017-05-20 14:02:55,622 [63] ServiceCallInterceptor - [Service] - Call method: IAgeRangerService.FindPeople ()
INFO  2017-05-20 14:02:55,622 [63] ositoryCallInterceptor - [Repository] - Call method: IAgeRangerRepository`1.GetAll()
INFO  2017-05-20 14:02:55,623 [44] ositoryCallInterceptor - [Repository] - End call IAgeRangerRepository`1.GetAll.
INFO  2017-05-20 14:02:55,625 [63] ositoryCallInterceptor - [Repository] - End call IAgeRangerRepository`1.GetAll.
INFO  2017-05-20 14:02:55,637 [44] ositoryCallInterceptor - [Repository] - Call method: IAgeRangerRepository`1.Query(g => ((Not(g.MaxAge.HasValue) AndAlso (g.MinAge.Value <= value(AgeRanger.Service.Implementation.AgeRangerService+<>c__DisplayClass10_0).maxAge)) OrElse ((g.MaxAge.HasValue AndAlso (g.MaxAge.Value >= value(AgeRanger.Service.Implementation.AgeRangerService+<>c__DisplayClass10_0).minAge)) AndAlso (g.MinAge.Value <= value(AgeRanger.Service.Implementation.AgeRangerService+<>c__DisplayClass10_0).maxAge))))
INFO  2017-05-20 14:02:55,637 [63] ositoryCallInterceptor - [Repository] - Call method: IAgeRangerRepository`1.Query(g => ((Not(g.MaxAge.HasValue) AndAlso (g.MinAge.Value <= value(AgeRanger.Service.Implementation.AgeRangerService+<>c__DisplayClass10_0).maxAge)) OrElse ((g.MaxAge.HasValue AndAlso (g.MaxAge.Value >= value(AgeRanger.Service.Implementation.AgeRangerService+<>c__DisplayClass10_0).minAge)) AndAlso (g.MinAge.Value <= value(AgeRanger.Service.Implementation.AgeRangerService+<>c__DisplayClass10_0).maxAge))))
INFO  2017-05-20 14:02:55,638 [44] ositoryCallInterceptor - [Repository] - End call IAgeRangerRepository`1.Query.
INFO  2017-05-20 14:02:55,638 [63] ositoryCallInterceptor - [Repository] - End call IAgeRangerRepository`1.Query.
INFO  2017-05-20 14:02:55,639 [44] ServiceCallInterceptor - [Service] - End call IAgeRangerService.FindPeople. It takes: 19 millisecond
INFO  2017-05-20 14:02:55,640 [44] ApiActionFilters       - ---> [API] End AgeRanger.WebApp.Controllers.Api.PersonController, action Filter. It takes: 20 millisecond ---> 

============================================

AgeRanger is a world leading application designed to identify person's age group!
The only problem with it is... It is not implemented - except a SQLite DB called AgeRanger.db.

To help AgeRanger to conquer the world please implement a web application that communicates with the DB mentioned above, and does the following:

 - Allows user to add a new person - every person has the first name, last name, and age;
 - Displays a list of people in the DB with their First and Last names, age and their age group. The age group should be determened based on the AgeGroup DB table - a person belongs to the age group where person's age >= 
 than group's MinAge and < than group's MaxAge. Please note that MinAge and MaxAge can be null;
 - Allows user to search for a person by his/her first or last name and displays all relevant information for the person - first and last names, age, age group.

In our fantasies AgeRanger is a single page application, and our DBA has already implied that he wants us to migrate it to SQL Server some time in the future.
And unit tests! We love unit tests!

Last, but not the least - our sales manager suggests you'll get bonus points if the application will also allow user to edit existing person records and expose a WEB API.

Please fork the project.

You are free to use any technology and frameworks you need. However if you decide to go with third party package manager or dev tool - don't forget to mention them in the README.md of your fork.

Good luck!