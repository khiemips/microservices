#pragma once

#ifdef _WIN32
typedef const char*(__stdcall * OpenWriteData_DECL)(const char*);
typedef void(__stdcall * WriteData_DECL)(const char*, const char*, int);
typedef void(__stdcall * CloseWriteData_DECL)(const char*);
typedef const char*(__stdcall * OpenReadData_DECL)(const char*);
typedef int(__stdcall * ReadData_DECL)(const char*, char*, int);
typedef void(__stdcall * CloseReadData_DECL)(const char*);
typedef void(__stdcall * LogDebug_DECL)(const char*);
#endif

#if defined(unix) || defined(__unix) || defined(__unix__)
typedef const char*(* OpenWriteData_DECL)(const char*);
typedef void(* WriteData_DECL)(const char*, const char*, int);
typedef void(* CloseWriteData_DECL)(const char*);
typedef const char*(* OpenReadData_DECL)(const char*);
typedef int(* ReadData_DECL)(const char*, char*, int);
typedef void(* CloseReadData_DECL)(const char*);
typedef void(* LogDebug_DECL)(const char*);
#endif

/* CAUTION: change the naming and order of the properties here may cause .NET Core side to fails
*/
class Context {
public:
	OpenWriteData_DECL OpenWriteData;
	WriteData_DECL WriteData;
	CloseWriteData_DECL CloseWriteData;

	OpenReadData_DECL OpenReadData;
	ReadData_DECL ReadData;
	CloseReadData_DECL CloseReadData;

	LogDebug_DECL LogDebug;
};
