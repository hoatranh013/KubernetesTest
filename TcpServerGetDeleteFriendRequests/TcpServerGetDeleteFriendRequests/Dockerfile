FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/out .
COPY --from=build /app/health_check.sh .

RUN apt update && apt install -y procps

ENTRYPOINT ["dotnet", "TcpServerGetDeleteFriendRequests.dll"]