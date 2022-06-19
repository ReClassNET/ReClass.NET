#include "SocketWrapper.h"

bool SocketWrapper::SendMsg(int32_t id)
{
	return Send(&id, sizeof(int32_t));
}

int32_t SocketWrapper::GetMsg()
{
	int32_t id;

	if(!Recive(&id, sizeof(int32_t)))
		id = -1;
	
	return id;
}
