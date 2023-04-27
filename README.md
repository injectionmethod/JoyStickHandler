# JoyStickHandler
WinMM API Joystick Module - A collection of functions for interfacing with joysticks using the Windows Multimedia API (WinMM).

- *GetJoystickNames()*: Returns a list of the names of all connected joysticks on the system.
- *GetJoystickState(joystickID As Integer)*: Retrieves the current state of a specified joystick and returns the data as a structure.
- *GetJoystickThreshold(joystickID As Integer)*: Retrieves the current threshold value for a specified joystick.
- *SetJoystickThreshold(joystickID As Integer, threshold As Integer)*: Sets the threshold value for a specified joystick.
- *GetJoystickCalibration(joystickID As Integer)*: Retrieves the current calibration settings for a specified joystick.
- *SetJoystickCalibration(joystickID As Integer, calibration As JOYCALIBRATE)*: Sets the calibration settings for a specified joystick.
- *GetJoystickHWID(joystickID As Integer)*: Retrieves the hardware ID of a specified joystick.
- *GetDeviceID(devicePath As String)*: Retrieves the device ID of a specified device given its path.
