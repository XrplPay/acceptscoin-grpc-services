#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Identity/AcceptsCoin.Services.IdentityServer/AcceptsCoin.Services.IdentityServer.csproj", "Services/Identity/AcceptsCoin.Services.IdentityServer/"]
COPY ["Common/AcceptsCoin.Common/AcceptsCoin.Common.csproj", "Common/AcceptsCoin.Common/"]
RUN dotnet restore "Services/Identity/AcceptsCoin.Services.IdentityServer/AcceptsCoin.Services.IdentityServer.csproj"
COPY . .
WORKDIR "/src/Services/Identity/AcceptsCoin.Services.IdentityServer"
RUN dotnet build "AcceptsCoin.Services.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcceptsCoin.Services.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcceptsCoin.Services.IdentityServer.dll"]
