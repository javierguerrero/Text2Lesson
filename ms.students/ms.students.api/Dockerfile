#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ms.students/ms.students.api/ms.students.api.csproj", "ms.students/ms.students.api/"]
COPY ["ms.students/ms.students.application/ms.students.application.csproj", "ms.students/ms.students.application/"]
COPY ["ms.students/ms.students.domain/ms.students.domain.csproj", "ms.students/ms.students.domain/"]
COPY ["ms.students/ms.students.infrastructure/ms.students.infrastructure.csproj", "ms.students/ms.students.infrastructure/"]
RUN dotnet restore "ms.students/ms.students.api/ms.students.api.csproj"
COPY . .
WORKDIR "/src/ms.students/ms.students.api"
RUN dotnet build "ms.students.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ms.students.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ms.students.api.dll"]