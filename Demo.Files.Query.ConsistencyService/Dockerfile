FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY .. .
WORKDIR /source/Demo.Files.Query.ConsistencyService
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Demo.Files.Query.ConsistencyService.dll"]
