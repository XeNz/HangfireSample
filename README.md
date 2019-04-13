# HangfireSample

HangfireSample is a sample project to illustrate possible usages of Hangfire

## Contents of this repository
* A .NET Core 2.2 web project
* docker-compose file for the above mentioned project and a PostgreSQL 9.6.3 instance

## Running the application

1. Run ``run-local-docker.sh`` to spin up a PostgreSQL 9.6.3 instance that has the needed SQL scripts (scripts in deploy/migration) executed to run the application. 
2. Run ``docker-compose up app`` or run the application non-dockerized through ``dotnet run``.

## Minor functional cases / ideas interwoven in this project 

###Main functional flows

* A user can view a list of comments
* A user can view one comment in particular
* An external system can see how many times a comment has been viewed

###Secondary functional and technical requirements

#### Not letting the user wait for calculations that do not concern him
* When viewing a comment, we would like to update the view count of that comment. We would like it that the user does not have to wait on his response for this to happen.

#### When the external system(s) poll(s) the view count of a comment, we would like it to not hurt the performance of our application
* We make sure the database is not queried every time an external system needs to get the view count, and use a (very basic) caching system.



## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)