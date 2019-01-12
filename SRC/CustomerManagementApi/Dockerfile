FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["CustomerManagementApi/CustomerManagementApi.csproj", "CustomerManagementApi/"]
RUN dotnet restore "CustomerManagementApi/CustomerManagementApi.csproj"
COPY . .
WORKDIR "/src/CustomerManagementApi"
RUN dotnet build "CustomerManagementApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CustomerManagementApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CustomerManagementApi.dll"]