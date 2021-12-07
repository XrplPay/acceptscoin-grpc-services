#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Gateways/AcceptsCoin.ApiGateway/AcceptsCoin.ApiGateway.csproj", "Gateways/AcceptsCoin.ApiGateway/"]
COPY ["Common/AcceptsCoin.Common/AcceptsCoin.Common.csproj", "Common/AcceptsCoin.Common/"]
COPY ["Services/Core/AcceptsCoin.Services.CoreServer/AcceptsCoin.Services.CoreServer.csproj", "Services/Core/AcceptsCoin.Services.CoreServer/"]
RUN dotnet restore "Gateways/AcceptsCoin.ApiGateway/AcceptsCoin.ApiGateway.csproj"
COPY . .
WORKDIR "/src/Gateways/AcceptsCoin.ApiGateway"
RUN dotnet build "AcceptsCoin.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcceptsCoin.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcceptsCoin.ApiGateway.dll"]
