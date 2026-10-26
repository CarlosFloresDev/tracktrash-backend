FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore src/TrackTrash.Api/TrackTrash.Api.csproj --locked-mode
RUN dotnet publish src/TrackTrash.Api/TrackTrash.Api.csproj --no-restore -c Release -o /out -p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /out .
COPY deployment/certs/ /app/certs/
USER $APP_UID
ENTRYPOINT ["dotnet", "TrackTrash.Api.dll"]
