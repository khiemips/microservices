# Declare base of final image
FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80


# Phrase 2: build .Net Core app
FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src

COPY NetCoreAndCppDemo.sln ./
COPY ExplicitKernelContext/ExplicitKernelContext.csproj ExplicitKernelContext/
COPY ExplicitKernelContext.Blob/ExplicitKernelContext.Blob.csproj ExplicitKernelContext.Blob/
COPY ExplicitKernelContext.Redis/ExplicitKernelContext.Redis.csproj ExplicitKernelContext.Redis/
COPY KernelAPI/KernelAPI.csproj KernelAPI/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY ExplicitKernelContext/ ExplicitKernelContext/
COPY ExplicitKernelContext.Blob/ ExplicitKernelContext.Blob/
COPY ExplicitKernelContext.Redis/ ExplicitKernelContext.Redis/
COPY KernelAPI/ KernelAPI/
WORKDIR /src/KernelAPI
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

# Final phrase: wire everything together
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KernelAPI.dll"]
