FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["TodoAppDdd.Api/TodoAppDdd.Api.csproj", "TodoAppDdd.Api/"]
RUN dotnet restore "TodoAppDdd.Api/TodoAppDdd.Api.csproj"
COPY . .
WORKDIR "/src/TodoAppDdd.Api"
RUN dotnet build "TodoAppDdd.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TodoAppDdd.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TodoAppDdd.Api.dll"]