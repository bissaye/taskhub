FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . /src
WORKDIR /src/TaskHub
RUN dotnet build "TaskHub.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish  "TaskHub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT ${ASPNETCORE_ENVIRONMENT}
ENTRYPOINT ["dotnet", "TaskHub.dll"]

