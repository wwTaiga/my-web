FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /sln

COPY *.sln .
COPY src/MyWeb.Core/*.csproj ./src/MyWeb.Core/
COPY test/MyWeb.Core.Tests/*.csproj ./test/MyWeb.Core.Tests/
RUN dotnet restore

COPY src ./src
COPY test ./test
RUN dotnet build -c Release --no-restore

RUN dotnet test

RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MyWeb.Core.dll"]
