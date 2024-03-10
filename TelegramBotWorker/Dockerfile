# Use the Microsoft official .NET 6.0 runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app

# Copy the published app to the container
COPY /bin/Release/net6.0/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "TelegramBotWorker.dll"]
