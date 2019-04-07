# HangfireSample

HangfireSample is a sample project accompanied with a blog at - to illustrate the usage of Hangfire

## Contents of this repository
* A .NET Core 2.2 web project
* docker-compose file for the above mentioned project and a PostgreSQL 9.6.3 instance

## Running the application

1. Run ``run-local-docker.sh`` to spin up a PostgreSQL 9.6.3 instance that has the needed SQL scripts (scripts in deploy/migration) executed to run the application. 
2. Run ``docker-compose up app`` or run the application non-dockerized through ``dotnet run``.


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)