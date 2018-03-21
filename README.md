# Build on Windows
## Related projects:
* NetCoreWin
* CppLibWin
## How to build
1. Set the solution active configuration to 'Debug'. To do this, right-click on the solution, then
Properties > Configuration Properies > Configuration Manager ...
2. Build `CppLibWin`
3. Build and run `NetCoreWin`

# Build on Linux
## Related projects:
* NetCoreWin
* CppLibLinux: this has the exact same codes from `CppLibWin`
## How to build
1. Set solution active configuration to `Unix`.
2. Setup Docker container using the following Dockerfile content
```Dockerfile
FROM ubuntu

RUN apt-get update && apt-get install -y openssh-server g++ gdb gdbserver
RUN mkdir /var/run/sshd
RUN echo 'root:screencast' | chpasswd
RUN sed -i 's/PermitRootLogin prohibit-password/PermitRootLogin yes/' /etc/ssh/sshd_config

# SSH login fix. Otherwise user is kicked off after login
RUN sed 's@session\s*required\s*pam_loginuid.so@session optional pam_loginuid.so@g' -i /etc/pam.d/sshd

ENV NOTVISIBLE "in users profile"
RUN echo "export VISIBLE=now" >> /etc/profile

EXPOSE 22
CMD ["/usr/sbin/sshd", "-D"]
```
Docker commands
```CMD
docker build -f <path_to_Dockerfile> -t <image_name> .
docker run -d -P --name test_sshd <image_name>
docker port test_sshd 22
```
3. Connect the container to Visual Studio
Tools > Options > Cross Platform > Connection Manager. Username is `root`, pass is `screencast`.
4. Build CppLibLinux
5. Create some folder for the final build
* Copy the built .so from step 4
```CMD
docker cp test_sshd:/root/projects/CppLibLinux/bin/x64/Unix/libCppLibLinux.so <path_to_final_build_folder>
```
6. Build `NetCoreWin` and copy output folder (Unix) to the final build folder
Now inside the build folder you have content like this
```
build_folder
	netcoreapp2.0
		NetCoreWin.dll
		...
	libCppLibLinux.so
	linuxso.Dockerfile
```
Content of the dockerfiler is
```
FROM microsoft/dotnet:2.0.4-runtime

COPY ./netcoreapp2.0 /app
COPY ./libCppLibLinux.so /app
WORKDIR /app

CMD [ "dotnet", "./NetCoreWin.dll" ]
```
7. Final build to docker and verify
Inside the final build folder, run commands
```CMD
docker build -t pinvoke_linux -f linuxso.Dockerfile .
docker run pinvoke_linux
```
