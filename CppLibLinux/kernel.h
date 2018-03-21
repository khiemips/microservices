#pragma once

#include "context.h"

class Kernel {
public:
	Kernel(Context ctx): ctx(ctx) {}
	~Kernel() { /*Do nothing, since ctx is created outside of cpp*/ }

	Context GetContext() { return ctx; }
private:
	Context ctx;
};
