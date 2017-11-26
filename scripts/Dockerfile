FROM microsoft/dotnet:2.0.3-sdk
COPY /deploy /app
WORKDIR /app
EXPOSE 8083
ENTRYPOINT ["dotnet", "Server.dll"]