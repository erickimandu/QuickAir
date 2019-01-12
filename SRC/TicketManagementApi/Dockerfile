FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["TicketManagementApi/TicketManagementApi.csproj", "TicketManagementApi/"]
RUN dotnet restore "TicketManagementApi/TicketManagementApi.csproj"
COPY . .
WORKDIR "/src/TicketManagementApi"
RUN dotnet build "TicketManagementApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TicketManagementApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TicketManagementApi.dll"]