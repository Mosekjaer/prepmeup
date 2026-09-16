# Build context is the repository root: docker build -f src/PrepMeUp.Api/Dockerfile .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY Directory.Build.props ./
COPY src/PrepMeUp.Domain/PrepMeUp.Domain.csproj src/PrepMeUp.Domain/
COPY src/PrepMeUp.Application/PrepMeUp.Application.csproj src/PrepMeUp.Application/
COPY src/PrepMeUp.Infrastructure/PrepMeUp.Infrastructure.csproj src/PrepMeUp.Infrastructure/
COPY src/PrepMeUp.Api/PrepMeUp.Api.csproj src/PrepMeUp.Api/
RUN dotnet restore src/PrepMeUp.Api/PrepMeUp.Api.csproj

COPY src/ src/
RUN dotnet publish src/PrepMeUp.Api/PrepMeUp.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
USER $APP_UID

ENTRYPOINT ["dotnet", "PrepMeUp.Api.dll"]
