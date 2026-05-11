# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AttendanceApi/AttendanceApi.csproj ./AttendanceApi/
RUN dotnet restore ./AttendanceApi/AttendanceApi.csproj

COPY AttendanceApi/ ./AttendanceApi/
WORKDIR /src/AttendanceApi

RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "AttendanceApi.dll"]