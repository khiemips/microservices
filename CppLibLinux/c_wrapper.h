#pragma once

#include "kernel.h"

#include <string>
#include <iostream>

void helper_printdata(char* databuffer, int size) {
	std::cout << "Reading data of length: " << size << "..." << std::endl;
	std::string ss(databuffer, size);
	std::cout << ss << std::endl;
	std::cout << "Read done" << std::endl;
}

extern "C" {
#ifdef _WIN32
		__declspec(dllexport) Kernel* createKernel(Context* ctx);
		__declspec(dllexport) void startValidation(Kernel* kernel);
		__declspec(dllexport) void closeKernel(Kernel* kernel);
#endif

#if defined(unix) || defined(__unix) || defined(__unix__)
		Kernel* createKernel(Context* ctx);
		void startValidation(Kernel* kernel);
		void closeKernel(Kernel* kernel);
#endif
}
