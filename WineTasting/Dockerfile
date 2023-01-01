FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
#RUN apt update
#RUN apt install -y --allow-unauthenticated \
#    libc6-dev \
#    libgdiplus \
#    libx11-dev \
# && rm -rf /var/lib/apt/lists/* \
    
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WineTastingWeb/WineTastingWeb.fsproj", "WineTastingWeb/"]
COPY ["WineTastingCore/WineTastingCore.fsproj", "WineTastingCore/"]
RUN dotnet restore "WineTastingWeb/WineTastingWeb.fsproj"
COPY . .
WORKDIR "/src/WineTastingWeb"
RUN dotnet dev-certs https --trust
RUN dotnet build "WineTastingWeb.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WineTastingWeb.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WineTastingWeb.dll"]
#
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /DockerSource
#COPY *.sln .
#COPY ./*/*.fsproj ./
#RUN find *.fsproj | sed -e 's/.fsproj//g' | xargs mkdir
#RUN find *.fsproj | sed -r -e 's/((.+).fsproj)/.\/\1 .\/\2/g' | xargs -I % sh -c 'mv %'
#RUN dotnet restore --runtime linux-musl-x64
#
#COPY . .
#WORKDIR "/DockerSource/WineTastingWeb"
#RUN dotnet publish \
#    -c release \
#    -o /DockerOutput/Website \
#    --no-restore \
#    --self-contained true \
#    --runtime linux-musl-x64 \
#    -p:PublishTrimmed=true \
#    -p:PublishSingleFile=true \
#    -p:DebugType=None
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0
#WORKDIR "/DockerOutput/Website"
#COPY --from=build /DockerOutput/Website ./
#ENTRYPOINT ["DockerOutput/Website"]
