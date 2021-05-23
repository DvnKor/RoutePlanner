# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY ./*.sln .
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /source/RoutePlannerDaemon
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /app ./
# ENTRYPOINT ["dotnet", "RoutePlannerDaemon.dll"]
ENV PORT=7654
EXPOSE 7654
CMD ASPNETCORE_URLS=http://*:$PORT dotnet RoutePlannerDaemon.dll