# Declare base of final image
FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

# Phrase 1: build cpp library
FROM gcc as cpp_build
WORKDIR /src
COPY CppLibLinux/*.cpp ./
COPY CppLibLinux/*.h ./
RUN mkdir -p obj	&&\
	mkdir -p bin	&&\
	g++ -c -x c++ c_wrapper.cpp -g2 -gdwarf-2 -o "obj/c_wrapper.o" -Wall -Wswitch -W"no-deprecated-declarations" -W"empty-body" -Wconversion -W"return-type" -Wparentheses -W"no-format" -Wuninitialized -W"unreachable-code" -W"unused-function" -W"unused-value" -W"unused-variable" -O0 -fno-strict-aliasing -fno-omit-frame-pointer -fpic -fthreadsafe-statics -fexceptions -frtti -std=c++11	&&\
	g++ -o "bin/libCppLibLinux.so" -Wl,--no-undefined -Wl,-z,relro -Wl,-z,now -Wl,-z,noexecstack -shared obj/c_wrapper.o

# Phrase 2: build .Net Core app
FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY NetCoreAndCppDemo.sln ./
COPY KernelAPI/KernelAPI.csproj KernelAPI/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY KernelAPI/ KernelAPI/
WORKDIR /src/KernelAPI
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

# Final phrase: wire everything together
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=cpp_build /src/bin/* .
ENTRYPOINT ["dotnet", "KernelAPI.dll"]
