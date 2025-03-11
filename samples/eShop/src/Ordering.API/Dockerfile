FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["src/Ordering.API/Ordering.API.csproj", "src/Ordering.API/"]
# Add references to any other project dependencies here
RUN dotnet restore "src/Ordering.API/Ordering.API.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/src/Ordering.API"

# Build the application
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Copy the published output from the build
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
