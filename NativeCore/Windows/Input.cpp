#define DIRECTINPUT_VERSION 0x0800

#include <dinput.h>
#include <vector>

#include "NativeCore.hpp"
#include "../Shared/Keys.hpp"

Keys mapping[];

class DirectInput
{
public:
	DirectInput() = default;

	DirectInput(const DirectInput&) = delete;
	DirectInput(const DirectInput&&) = delete;
	DirectInput& operator=(DirectInput const&) = delete;
	DirectInput& operator=(DirectInput const&&) = delete;

	~DirectInput()
	{
		if (keyboardDevice)
		{
			keyboardDevice->Unacquire();
			keyboardDevice->Release();
			keyboardDevice = nullptr;
		}
		if (directInputInterface)
		{
			directInputInterface->Release();
			directInputInterface = nullptr;
		}
	}

	bool Initialize()
	{
		if (DirectInput8Create(GetModuleHandle(nullptr), DIRECTINPUT_VERSION, IID_IDirectInput8W, reinterpret_cast<void**>(&directInputInterface), nullptr) != DI_OK)
		{
			return false;
		}

		if (directInputInterface->CreateDevice(GUID_SysKeyboard, &keyboardDevice, nullptr) != DI_OK
			|| keyboardDevice->SetDataFormat(&c_dfDIKeyboard) != DI_OK)
		{
			return false;
		}

		return true;
	}

	bool ReadKeyboardState(Keys* keys[], int* count)
	{
		const int STATE_PRESSED = 0x80;

		currentState.clear();

		BYTE keyBuffer[256] = {};
		const auto result = keyboardDevice->GetDeviceState(sizeof(keyBuffer), &keyBuffer);
		if (result != DI_OK)
		{
			if (result == DIERR_NOTACQUIRED || result == DIERR_INPUTLOST)
			{
				keyboardDevice->Acquire();
			}
			return false;
		}

		auto modifier = Keys::None;
		if (keyBuffer[DIK_LSHIFT] & STATE_PRESSED || keyBuffer[DIK_RSHIFT] & STATE_PRESSED)
		{
			modifier |= Keys::Shift;
		}
		if (keyBuffer[DIK_LCONTROL] & STATE_PRESSED || keyBuffer[DIK_RCONTROL] & STATE_PRESSED)
		{
			modifier |= Keys::Control;
		}
		if (keyBuffer[DIK_LMENU] & STATE_PRESSED)
		{
			modifier |= Keys::Alt;
		}
		if (keyBuffer[DIK_RMENU] & STATE_PRESSED)
		{
			modifier |= Keys::Alt;
			modifier |= Keys::Control;
		}

		for (auto i = 0u; i < 0xEF; ++i)
		{
			if (keyBuffer[i] & STATE_PRESSED)
			{
				auto currentKey = mapping[i];
				if (currentKey != Keys::None)
				{
					switch (currentKey)
					{
					case Keys::LControlKey:
					case Keys::RControlKey:
						currentKey = Keys::ControlKey;
						break;
					case Keys::LShiftKey:
					case Keys::RShiftKey:
						currentKey = Keys::ControlKey;
						break;
					case Keys::LMenu:
					case Keys::RMenu:
						currentKey = Keys::Menu;
						break;
					}

					currentKey |= modifier;

					currentState.push_back(currentKey);
				}
			}
		}

		*keys = currentState.data();
		*count = static_cast<int>(currentState.size());

		return true;
	}

private:
	IDirectInput8W* directInputInterface = nullptr;
	IDirectInputDevice8W* keyboardDevice = nullptr;
	std::vector<Keys> currentState;
};

RC_Pointer RC_CallConv InitializeInput()
{
	auto input = new DirectInput();
	if (!input->Initialize())
	{
		delete input;

		return nullptr;
	}
	return static_cast<RC_Pointer>(input);
}

bool RC_CallConv GetPressedKeys(RC_Pointer handle, Keys* keys[], int* count)
{
	return static_cast<DirectInput*>(handle)->ReadKeyboardState(keys, count);
}

void RC_CallConv ReleaseInput(RC_Pointer handle)
{
	delete static_cast<DirectInput*>(handle);
}

Keys mapping[] =
{
	Keys::None, /*0x00*/
	Keys::Escape, /* DIK_ESCAPE */ /*0x01*/
	Keys::D1, /* DIK_1 */ /*0x02*/
	Keys::D2, /* DIK_2 */ /*0x03*/
	Keys::D3, /* DIK_3 */ /*0x04*/
	Keys::D4, /* DIK_4 */ /*0x05*/
	Keys::D5, /* DIK_5 */ /*0x06*/
	Keys::D6, /* DIK_6 */ /*0x07*/
	Keys::D7, /* DIK_7 */ /*0x08*/
	Keys::D8, /* DIK_8 */ /*0x09*/
	Keys::D9, /* DIK_9 */ /*0x0A*/
	Keys::D0, /* DIK_0 */ /*0x0B*/
	Keys::OemMinus, /* DIK_MINUS */ /*0x0C*/
	Keys::OemPlus, /* DIK_EQUALS */ /*0x0D*/
	Keys::Back, /* DIK_BACK */ /*0x0E*/
	Keys::Tab, /* DIK_TAB */ /*0x0F*/
	Keys::Q, /* DIK_Q */ /*0x10*/
	Keys::W, /* DIK_W */ /*0x11*/
	Keys::E, /* DIK_E */ /*0x12*/
	Keys::R, /* DIK_R */ /*0x13*/
	Keys::T, /* DIK_T */ /*0x14*/
	Keys::Z, /* DIK_Y */ /*0x15*/
	Keys::U, /* DIK_U */ /*0x16*/
	Keys::I, /* DIK_I */ /*0x17*/
	Keys::O, /* DIK_O */ /*0x18*/
	Keys::P, /* DIK_P */ /*0x19*/
	Keys::Oem4, /* DIK_LBRACKET */ /*0x1A*/
	Keys::Oem6, /* DIK_RBRACKET */ /*0x1B*/
	Keys::Return, /* DIK_RETURN */ /*0x1C*/
	Keys::LControlKey, /* DIK_LControl */ /*0x1D*/
	Keys::A, /* DIK_A */ /*0x1E*/
	Keys::S, /* DIK_S */ /*0x1F*/
	Keys::D, /* DIK_D */ /*0x20*/
	Keys::F, /* DIK_F */ /*0x21*/
	Keys::G, /* DIK_G */ /*0x22*/
	Keys::H, /* DIK_H */ /*0x23*/
	Keys::J, /* DIK_J */ /*0x24*/
	Keys::K, /* DIK_K */ /*0x25*/
	Keys::L, /* DIK_L */ /*0x26*/
	Keys::Oem1, /* DIK_SEMICOLON */ /*0x27*/
	Keys::Oem7, /* DIK_APOSTROPHE */ /*0x28*/
	Keys::Oem3, /* DIK_GRAVE */ /*0x29*/
	Keys::LShiftKey, /* DIK_LSHIFT */ /*0x2A*/
	Keys::OemBackslash, /* DIK_BACKSLASH */ /*0x2B*/
	Keys::Y, /* DIK_Z */ /*0x2C*/
	Keys::X, /* DIK_X */ /*0x2D*/
	Keys::C, /* DIK_C */ /*0x2E*/
	Keys::V, /* DIK_V */ /*0x2F*/
	Keys::B, /* DIK_B */ /*0x30*/
	Keys::N, /* DIK_N */ /*0x31*/
	Keys::M, /* DIK_M */ /*0x32*/
	Keys::OemComma, /* DIK_COMMA */ /*0x33*/
	Keys::OemPeriod, /* DIK_PERIOD */ /*0x34*/
	Keys::Oem2, /* DIK_SLASH */ /*0x35*/
	Keys::RShiftKey, /* DIK_RSHIFT */ /*0x36*/
	Keys::Multiply, /* DIK_MULTIPLY */ /*0x37*/
	Keys::LMenu, /* DIK_LMENU */ /*0x38*/
	Keys::Space, /* DIK_SPACE */ /*0x39*/
	Keys::CapsLock, /* DIK_CAPITAL */ /*0x3A*/
	Keys::F1, /* DIK_F1 */ /*0x3B*/
	Keys::F2, /* DIK_F2 */ /*0x3C*/
	Keys::F3, /* DIK_F3 */ /*0x3D*/
	Keys::F4, /* DIK_F4 */ /*0x3E*/
	Keys::F5, /* DIK_F5 */ /*0x3F*/
	Keys::F6, /* DIK_F6 */ /*0x40*/
	Keys::F7, /* DIK_F7 */ /*0x41*/
	Keys::F8, /* DIK_F8 */ /*0x42*/
	Keys::F9, /* DIK_F9 */ /*0x43*/
	Keys::F10, /* DIK_F10 */ /*0x44*/
	Keys::NumLock, /* DIK_NUMLOCK */ /*0x45*/
	Keys::Scroll, /* DIK_SCROLL */ /*0x46*/
	Keys::NumPad7, /* DIK_NUMPAD7 */ /*0x47*/
	Keys::NumPad8, /* DIK_NUMPAD8 */ /*0x48*/
	Keys::NumPad9, /* DIK_NUMPAD9 */ /*0x49*/
	Keys::Subtract, /* DIK_SUBSTRACT */ /*0x4A*/
	Keys::NumPad4, /* DIK_NUMPAD4 */ /*0x4B*/
	Keys::NumPad5, /* DIK_NUMPAD5 */ /*0x4C*/
	Keys::NumPad6, /* DIK_NUMPAD6 */ /*0x4D*/
	Keys::Add, /* DIK_ADD */ /*0x4E*/
	Keys::NumPad1, /* DIK_NUMPAD1 */ /*0x4F*/
	Keys::NumPad2, /* DIK_NUMPAD2 */ /*0x50*/
	Keys::NumPad3, /* DIK_NUMPAD3 */ /*0x51*/
	Keys::NumPad0, /* DIK_NUMPAD0 */ /*0x52*/
	Keys::Decimal,  /* DIK_DECIMAL */ /*0x53*/
	Keys::None, /*0x54*/
	Keys::None, /*0x55*/
	Keys::Oem102, /* DIK_OEM_102 */ /*0x56*/
	Keys::F11, /* DIK_F11 */ /*0x57*/
	Keys::F12, /* DIK_F12 */ /*0x58*/
	Keys::None, /*0x59*/
	Keys::None, /*0x5A*/
	Keys::None, /*0x5B*/
	Keys::None, /*0x5C*/
	Keys::None, /*0x5D*/
	Keys::None, /*0x5E*/
	Keys::None, /*0x5F*/
	Keys::None, /*0x60*/
	Keys::None, /*0x61*/
	Keys::None, /*0x62*/
	Keys::None, /*0x63*/
	Keys::F13, /* DIK_F13 */ /*0x64*/
	Keys::F14, /* DIK_F14 */ /*0x65*/
	Keys::F15, /* DIK_F15 */ /*0x66*/
	Keys::None, /*0x67*/
	Keys::None, /*0x68*/
	Keys::None, /*0x69*/
	Keys::None, /*0x6A*/
	Keys::None, /*0x6B*/
	Keys::None, /*0x6C*/
	Keys::None, /*0x6D*/
	Keys::None, /*0x6E*/
	Keys::None, /*0x6F*/
	Keys::None, /* DIK_KANA */ /*0x70*/
	Keys::None, /*0x71*/
	Keys::None, /*0x72*/
	Keys::None, /* DIK_ABNT_C1 */ /*0x73*/
	Keys::None, /*0x74*/
	Keys::None, /*0x75*/
	Keys::None, /*0x76*/
	Keys::None, /*0x77*/
	Keys::None, /*0x78*/
	Keys::IMEConvert, /* DIK_CONVERT */ /*0x79*/
	Keys::None, /*0x7A*/
	Keys::IMENonconvert, /* DIK_NOCONVERT */ /*0x7B*/
	Keys::None, /*0x7C*/
	Keys::None, /* DIK_YEN */ /*0x7D*/
	Keys::None, /* DIK_ABNT_C2 */ /*0x7E*/
	Keys::None, /*0x7F*/
	Keys::None, /*0x80*/
	Keys::None, /*0x81*/
	Keys::None, /*0x82*/
	Keys::None, /*0x83*/
	Keys::None, /*0x84*/
	Keys::None, /*0x85*/
	Keys::None, /*0x86*/
	Keys::None, /*0x87*/
	Keys::None, /*0x88*/
	Keys::None, /*0x89*/
	Keys::None, /*0x8A*/
	Keys::None, /*0x8B*/
	Keys::None, /*0x8C*/
	Keys::None, /* DIK_NUMPADEQUALS */ /*0x8D*/
	Keys::None, /*0x8E*/
	Keys::None, /*0x8F*/
	Keys::MediaPreviousTrack, /* DIK_CIRCUMFLEX */ /*0x90*/
	Keys::None, /* DIK_AT */ /*0x91*/
	Keys::None, /* DIK_COLON */ /*0x92*/
	Keys::None, /* DIK_UNDERLINE */ /*0x93*/
	Keys::None, /* DIK_KANJI */ /*0x94*/
	Keys::None, /* DIK_STOP */ /*0x95*/
	Keys::None, /* DIK_AX */ /*0x96*/
	Keys::None, /* DIK_UNLABELED */ /*0x97*/
	Keys::None, /*0x98*/
	Keys::MediaNextTrack, /* DIK_NEXTTRACK */ /*0x99*/
	Keys::None, /*0x9A*/
	Keys::None, /*0x9B*/
	Keys::Return, /* DIK_NUMPADENTER */ /*0x9C*/
	Keys::RControlKey, /* DIK_RCONTROL */ /*0x9D*/
	Keys::None, /*0x9E*/
	Keys::None, /*0x9F*/
	Keys::VolumeMute, /* DIK_MUTE */ /*0xA0*/
	Keys::None, /* DIK_CALCULATOR */ /*0xA1*/
	Keys::MediaPlayPause, /* DIK_PLAYPAUSE */ /*0xA2*/
	Keys::None, /*0xA3*/
	Keys::MediaStop, /* DIK_MEDIASTOP */ /*0xA4*/
	Keys::None, /*0xA5*/
	Keys::None, /*0xA6*/
	Keys::None, /*0xA7*/
	Keys::None, /*0xA8*/
	Keys::None, /*0xA9*/
	Keys::None, /*0xAA*/
	Keys::None, /*0xAB*/
	Keys::None, /*0xAC*/
	Keys::None, /*0xAD*/
	Keys::VolumeDown, /* DIK_VOLUMEDOWN */ /*0xAE*/
	Keys::None, /*0xAF*/
	Keys::VolumeUp, /* DIK_VOLUMEUP */ /*0xB0*/
	Keys::None, /*0xB1*/
	Keys::BrowserHome, /* DIK_WEBHOME */ /*0xB2*/
	Keys::None, /* DIK_NUMPADCOMMA */ /*0xB3*/
	Keys::None, /*0xB4*/
	Keys::Divide, /* DIK_DIVIDE */ /*0xB5*/
	Keys::None, /*0xB6*/
	Keys::None, /* DIK_SYSRQ */ /*0xB7*/
	Keys::RMenu, /* DIK_RMENU */ /*0xB8*/
	Keys::None, /*0xB9*/
	Keys::None, /*0xBA*/
	Keys::None, /*0xBB*/
	Keys::None, /*0xBC*/
	Keys::None, /*0xBD*/
	Keys::None, /*0xBE*/
	Keys::None, /*0xBF*/
	Keys::None, /*0xC0*/
	Keys::None, /*0xC1*/
	Keys::None, /*0xC2*/
	Keys::None, /*0xC3*/
	Keys::None, /*0xC4*/
	Keys::Pause, /* DIK_PAUSE */ /*0xC5*/
	Keys::None, /*0xC6*/
	Keys::Home, /* DIK_HOME */ /*0xC7*/
	Keys::Up, /* DIK_UP */ /*0xC8*/
	Keys::Prior, /* DIK_PRIOR */ /*0xC9*/
	Keys::None, /*0xCA*/
	Keys::Left, /* DIK_LEFT */ /*0xCB*/
	Keys::None, /*0xCC*/
	Keys::Right, /* DIK_RIGHT */ /*0xCD*/
	Keys::None, /*0xCE*/
	Keys::End, /* DIK_END */ /*0xCF*/
	Keys::Down, /* DIK_DOWN */ /*0xD0*/
	Keys::Next, /* DIK_NEXT */ /*0xD1*/
	Keys::Insert, /* DIK_INSERT */ /*0xD2*/
	Keys::Delete, /* DIK_DELETE */ /*0xD3*/
	Keys::None, /*0xD4*/
	Keys::None, /*0xD5*/
	Keys::None, /*0xD6*/
	Keys::None, /*0xD7*/
	Keys::None, /*0xD8*/
	Keys::None, /*0xD9*/
	Keys::None, /*0xDA*/
	Keys::LWin, /* DIK_LWIN */ /*0xDB*/
	Keys::RWin, /* DIK_RWIN */ /*0xDC*/
	Keys::None, /* DIK_APPS */ /*0xDD*/
	Keys::None, /* DIK_POWER */ /*0xDE*/
	Keys::Sleep, /* DIK_SLEEP */ /*0xDF*/
	Keys::None, /*0xE0*/
	Keys::None, /*0xE1*/
	Keys::None, /*0xE2*/
	Keys::None, /* DIK_WAKE */ /*0xE3*/
	Keys::None, /*0xE4*/
	Keys::BrowserSearch, /* DIK_WEBSEARCH */ /*0xE5*/
	Keys::BrowserFavorites, /* DIK_WEBFAVORITES */ /*0xE6*/
	Keys::BrowserRefresh, /* DIK_WEBREFRESH */ /*0xE7*/
	Keys::BrowserStop, /* DIK_WEBSTOP */ /*0xE8*/
	Keys::BrowserForward, /* DIK_WEBFORWARD */ /*0xE9*/
	Keys::BrowserBack, /* DIK_WEBBACK */ /*0xEA*/
	Keys::None, /* DIK_MYCOMPUTER */ /*0xEB*/
	Keys::LaunchMail, /* DIK_MAIL */ /*0xEC*/
	Keys::SelectMedia, /* DIK_MEDIASELECT */ /*0xED*/
};