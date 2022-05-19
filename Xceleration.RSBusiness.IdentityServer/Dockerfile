#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Xceleration.RSBusiness.IdentityServer/Xceleration.RSBusiness.IdentityServer.csproj", "Xceleration.RSBusiness.IdentityServer/"]
COPY ["Xceleration.RSBusiness.IdentityServer.Repository/Xceleration.RSBusiness.IdentityServer.Stores.csproj", "Xceleration.RSBusiness.IdentityServer.Repository/"]
COPY ["Xceleration.RSBusiness.IdentityServer.Contracts/Xceleration.RSBusiness.IdentityServer.Contracts.csproj", "Xceleration.RSBusiness.IdentityServer.Contracts/"]
COPY ["Xceleration.RSBusiness.IdentityServer.Domain/Xceleration.RSBusiness.IdentityServer.Domain.csproj", "Xceleration.RSBusiness.IdentityServer.Domain/"]
COPY ["Xceleration.RsBusiness.AzureKeyVault/Xceleration.RsBusiness.AzureKeyVault.csproj", "Xceleration.RsBusiness.AzureKeyVault/"]
RUN dotnet restore "Xceleration.RSBusiness.IdentityServer/Xceleration.RSBusiness.IdentityServer.csproj"
COPY . .
WORKDIR "/src/Xceleration.RSBusiness.IdentityServer"
RUN dotnet build "Xceleration.RSBusiness.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Xceleration.RSBusiness.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Xceleration.RSBusiness.IdentityServer.dll"]