# Dockerfile (Development)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Set environment 
ENV ASPNETCORE_ENVIRONMENT=Development
# Expose port 80 
EXPOSE 80

WORKDIR /src/TourismAgency

ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:80"]