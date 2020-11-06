FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
RUN mkdir /app
WORKDIR /app
COPY ./.dist/MediaHuis.Notifications.WorkerService .
ENTRYPOINT ["dotnet", "MediaHuis.Notifications.WorkerService.dll"]
