FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
RUN mkdir /app
WORKDIR /app
COPY ./.dist/MediaHuis.Notifications.Api .
ENTRYPOINT ["dotnet", "MediaHuis.Notifications.Api.dll"]
