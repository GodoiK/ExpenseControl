# Backend - ASP.NET Core Web API (.NET)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore ExpenseControl.Api.Server/ExpenseControl.Api.Server.csproj
RUN dotnet publish ExpenseControl.Api.Server/ExpenseControl.Api.Server.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ExpenseControl.Api.Server.dll"]
