FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/out .

RUN apt-get update
RUN apt-get install -y docker.io
RUN apt-get install -y curl 
RUN apt-get install -y dos2unix

ENTRYPOINT ["dotnet", "GrpcServiceInteractingBetweenUsers.dll"]