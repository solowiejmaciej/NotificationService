FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EventsConsumer/EventsConsumer.csproj", "EventsConsumer/"]
RUN dotnet restore "EventsConsumer/EventsConsumer.csproj"
COPY . .
WORKDIR "/src/EventsConsumer"
RUN dotnet build "EventsConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventsConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventsConsumer.dll"]
