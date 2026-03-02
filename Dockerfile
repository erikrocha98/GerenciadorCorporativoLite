# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.sln ./
COPY **/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /src
RUN dotnet build -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Copy published files
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development

# Entry point
ENTRYPOINT ["dotnet", "DesafioFullStack.API.dll"]