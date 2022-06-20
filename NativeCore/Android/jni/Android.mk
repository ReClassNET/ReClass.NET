LOCAL_PATH := $(call my-dir)
include $(CLEAR_VARS)

LOCAL_MODULE := rcserver
LOCAL_C_INCLUDES += \
$(LOCAL_PATH)/../../Shared \
$(LOCAL_PATH)/../../Unix

LOCAL_SRC_FILES := \
main.cpp \
Remote.cpp \
Utils.cpp \
../../Unix/LinuxProcess.cpp \
../../Unix/LinuxSocketWrapper.cpp \
../../Shared/SocketWrapper.cpp

LOCAL_CFLAGS := -fexceptions

include $(BUILD_EXECUTABLE)