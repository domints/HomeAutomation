FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine

WORKDIR /App

COPY ./out /App/

ENTRYPOINT ["dotnet", "HomeAutomation.dll"]
