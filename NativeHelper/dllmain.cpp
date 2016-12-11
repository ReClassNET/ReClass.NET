#include "NativeHelper.hpp"

RequestFunctionPtrCallback requestFunction;

void __stdcall Initialize(RequestFunctionPtrCallback requestCallback)
{
	requestFunction = requestCallback;
}
