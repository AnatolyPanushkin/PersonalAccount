#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM nginx:alpine 
COPY ["PersonalAccount/nginx.conf", "PersonalAccount/"]
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PersonalAccount/PersonalAccount.csproj", "PersonalAccount/"]
RUN dotnet restore "PersonalAccount/PersonalAccount.csproj"
COPY . .
WORKDIR "/src/PersonalAccount"
RUN dotnet build "PersonalAccount.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PersonalAccount.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonalAccount.dll"]