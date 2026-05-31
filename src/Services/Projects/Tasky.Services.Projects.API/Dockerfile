FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine3.23 AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:10.0.202-alpine3.23-amd64 AS build
WORKDIR /src



# copy slnx file
COPY "Tasky.slnx" "Tasky.slnx"

# copy gatways files
COPY "Gateways/Tasky.Gateways.API/Tasky.Gateways.API.csproj" "Gateways/Tasky.Gateways.API/Tasky.Gateways.API.csproj"

# copy Identities files
COPY "Services/Identities/Tasky.Services.Identities.API/Tasky.Services.Identities.API.csproj" "Services/Identities/Tasky.Services.Identities.API/Tasky.Services.Identities.API.csproj"
COPY "Services/Identities/Tasky.Services.Identities.Infrastructure/Tasky.Services.Identities.Infrastructure.csproj" "Services/Identities/Tasky.Services.Identities.Infrastructure/Tasky.Services.Identities.Infrastructure.csproj"
COPY "Services/Identities/Tasky.Services.Identities.Application/Tasky.Services.Identities.Application.csproj" "Services/Identities/Tasky.Services.Identities.Application/Tasky.Services.Identities.Application.csproj"
COPY "Services/Identities/Tasky.Services.Identities.Domain/Tasky.Services.Identities.Domain.csproj" "Services/Identities/Tasky.Services.Identities.Domain/Tasky.Services.Identities.Domain.csproj"

# copy Projects files
COPY "Services/Projects/Tasky.Services.Projects.API/Tasky.Services.Projects.API.csproj" "Services/Projects/Tasky.Services.Projects.API/Tasky.Services.Projects.API.csproj"
COPY "Services/Projects/Tasky.Services.Projects.Infrastructure/Tasky.Services.Projects.Infrastructure.csproj" "Services/Projects/Tasky.Services.Projects.Infrastructure/Tasky.Services.Projects.Infrastructure.csproj"
COPY "Services/Projects/Tasky.Services.Projects.Application/Tasky.Services.Projects.Application.csproj" "Services/Projects/Tasky.Services.Projects.Application/Tasky.Services.Projects.Application.csproj"
COPY "Services/Projects/Tasky.Services.Projects.Domain/Tasky.Services.Projects.Domain.csproj" "Services/Projects/Tasky.Services.Projects.Domain/Tasky.Services.Projects.Domain.csproj"


COPY "NuGet.config" "NuGet.config"



RUN dotnet restore "Tasky.slnx"

COPY . .
WORKDIR /src/Services/Projects/Tasky.Services.Projects.API
RUN dotnet publish  -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app .
ENTRYPOINT [ "dotnet","Tasky.Services.Projects.API.dll" ]