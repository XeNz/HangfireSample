FROM microsoft/dotnet:2.2-sdk-stretch AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./HangfireSample/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2.2-aspnetcore-runtime-stretch-slim
WORKDIR /app
COPY --from=build-env /app/HangfireSample/out .
ENTRYPOINT ["dotnet", "HangfireSample.dll"]