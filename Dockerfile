FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore HangfireSample.csproj

COPY . ./
RUN dotnet publish HangfireSample.csproj -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY — from=build /app/out .
ENTRYPOINT [“dotnet”, “HangfireSample.dll”]`