##Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
##For more information, please see https://aka.ms/containercompat
#
#FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-sac2016 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM microsoft/dotnet:2.1-sdk-nanoserver-sac2016 AS build
#WORKDIR /src
#COPY ["WebApiService/WebApiService.csproj", "WebApiService/"]
#RUN dotnet restore "WebApiService/WebApiService.csproj"
#COPY . .
#WORKDIR "/src/WebApiService"
#RUN dotnet build "WebApiService.csproj" -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish "WebApiService.csproj" -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "WebApiService.dll"]

FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WebApiService.dll"]