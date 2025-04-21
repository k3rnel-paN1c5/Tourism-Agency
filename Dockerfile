# Dockerfile (for development)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY TourismAgency/TourismAgency.csproj ./TourismAgency/
COPY DataAccess/DataAccess.csproj ./DataAccess/
COPY BusinessLogic/BusinessLogic.csproj ./BusinessLogic/
COPY DTO/DTO.csproj ./DTO/
RUN dotnet restore ./TourismAgency/TourismAgency.csproj
RUN dotnet restore ./DataAccess/DataAccess.csproj
RUN dotnet restore ./BusinessLogic/BusinessLogic.csproj
RUN dotnet restore ./DTO/DTO.csproj

COPY TourismAgency/ ./TourismAgency/
COPY DataAccess/ ./DataAccess/
COPY BusinessLogic/ ./BusinessLogic/
COPY DTO/ ./DTO/

# Development specific setup
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80

WORKDIR /src/TourismAgency

ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:80"]