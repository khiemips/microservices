FROM microsoft/dotnet:2.0.4-runtime

COPY ./netcoreapp2.0 /app
COPY ./libCppLibLinux.so /app
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:80
EXPOSE 80

CMD [ "dotnet", "./KernelAPI.dll" ]