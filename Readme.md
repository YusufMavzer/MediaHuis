# Media Huis

``` sh

# initial
# install docker and docker compose
# install dotnet core 3.1
dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator

# this will the sql db and rabbitmq make sure ports 1433, 15672, 5672 aren't in use
# you can adjust when needed
docker-compose up -d

# When you wanted to start from scratch for this demo but don't forget to delete the migration folder
# NOTE: pwd should be $PATH/mediahuis/notifications/MediaHuis.Notifications/MediaHuis.Notifications
dotnet ef migrations add $MIGRATION_NAME --startup-project ../MediaHuis.Notifications.Api
dotnet ef database update --startup-project ../MediaHuis.Notifications.Api


# When done testing
# Note that the database will be removed since I didn't specify add any volume
# 'dotnet ef database ef' will need to be run again in that case
docker-compose down


# Future suggestion is to use MongoDB or any NoSql database instead of SQL.
# It boosts productivity and requires minimum setup

```