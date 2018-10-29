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



FROM microsoft/dotnet:2.1-sdk
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy and build everything else
COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/WebApiService.dll"]