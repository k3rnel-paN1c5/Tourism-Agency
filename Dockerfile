# Dockerfile (for development)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Domain/Domain.csproj ./Domain/
COPY Application/Application.csproj ./Application/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY TourismAgency/TourismAgency.csproj ./TourismAgency/
RUN dotnet restore ./Domain/Domain.csproj
RUN dotnet restore ./Application/Application.csproj
RUN dotnet restore ./Infrastructure/Infrastructure.csproj
RUN dotnet restore ./TourismAgency/TourismAgency.csproj

COPY Domain/ ./Domain/
COPY Application/ ./Application/
COPY Infrastructure/ ./Infrastructure/
COPY TourismAgency/ ./TourismAgency/

# Development specific setup
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80

WORKDIR /src/TourismAgency

ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:80"]