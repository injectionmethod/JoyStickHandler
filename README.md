# JoyStickHandler
WinMM API Joystick Module - A collection of functions for interfacing with joysticks using the Windows Multimedia API (WinMM).

- *GetJoystickNames()*: Returns a list of the names of all connected joysticks on the system.
- *GetJoystickState(joystickID As Integer)*: Retrieves the current state of a specified joystick and returns the data as a structure.
- *GetJoystickThreshold(joystickID As Integer)*: Retrieves the current threshold value for a specified joystick.
- *SetJoystickThreshold(joystickID As Integer, threshold As Integer)*: Sets the threshold value for a specified joystick.
- *GetDeviceID(devicePath As String)*: Retrieves the device ID of a specified device given its path.
- *RegisterJoyConfigChangedEvent(ByVal handler As Action(Of Integer))*: Registers an event handler to be called when the configuration of any joystick is changed. 
- *SetActiveDevice(ByVal joystickName As String) As Boolean*: Sets the active device
- *ReleaseJoystickCapture(ByVal joystickID As Integer) As Boolean*: Releases the device
