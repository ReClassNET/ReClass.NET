#include "NativeCore.hpp"
#include "../Shared/Keys.hpp"

void ReleaseInput();

RC_Pointer InitializeInput()
{
	return nullptr;
}

bool GetPressedKeys(RC_Pointer handle, Keys* state[], int* count)
{
	return false;
}

void ReleaseInput(RC_Pointer handle)
{

}
