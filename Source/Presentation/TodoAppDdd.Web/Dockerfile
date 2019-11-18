FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Source/Presentation/TodoAppDdd.Web/TodoAppDdd.Web.csproj", "Source/Presentation/TodoAppDdd.Web/"]
COPY ["Source/Infrastructure/TodoAppDdd.Bootstrapper/TodoAppDdd.Bootstrapper.csproj", "Source/Infrastructure/TodoAppDdd.Bootstrapper/"]
COPY ["Source/Common/TodoAppDdd.Common/TodoAppDdd.Common.csproj", "Source/Common/TodoAppDdd.Common/"]
COPY ["Source/Core/TodoAppDdd.App.Contracts/TodoAppDdd.App.Contracts.csproj", "Source/Core/TodoAppDdd.App.Contracts/"]
RUN dotnet restore "Source/Presentation/TodoAppDdd.Web/TodoAppDdd.Web.csproj"
COPY . .
WORKDIR "/src/Source/Presentation/TodoAppDdd.Web"
RUN dotnet build "TodoAppDdd.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TodoAppDdd.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TodoAppDdd.Web.dll"]