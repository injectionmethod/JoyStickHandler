'###############################################################################################
'                                       JoystickHandler.vb                                     #
'                                                                                              #
'These functions can be used together to create a standalone joystick module for the WinMM API.#
'The module can be used to retrieve joystick information and data.                             #
'###############################################################################################

Module JoystickHandler
    ' Import the joyConfigChanged function from winmm.dll
    <System.Runtime.InteropServices.DllImport("winmm.dll")>
    Private Sub joyConfigChanged(ByVal dwFlags As Integer)
    End Sub

    ' Import the joyConfigChangedCallback function from winmm.dll
    <System.Runtime.InteropServices.DllImport("winmm.dll")>
    Private Function joyConfigChangedCallback(ByVal callback As IntPtr, ByVal dwFlags As Integer) As Integer
    End Function
    ' Import the joySetThreshold function from winmm.dll
    <System.Runtime.InteropServices.DllImport("winmm.dll")>
    Private Function joySetThreshold(ByVal uJoyID As Integer, ByVal threshold As Integer) As Integer
    End Function
    ' Import the joyGetThreshold function from winmm.dll
    <System.Runtime.InteropServices.DllImport("winmm.dll")>
    Private Function joyGetThreshold(ByVal uJoyID As Integer, ByRef puThreshold As Integer) As Integer
    End Function

    ' Import the joyGetPosEx function from winmm.dll
    <System.Runtime.InteropServices.DllImport("winmm.dll")>
    Private Function joyGetPosEx(ByVal uJoyID As Integer, ByRef pji As JOYINFOEX) As Integer
    End Function

    ' Import the joyGetNumDevs function from winmm.dll
    <System.Runtime.InteropServices.DllImport("winmm.dll")>
    Private Function joyGetNumDevs() As Integer
    End Function
    ' Import the joyGetDevCaps function from winmm.dll
    <System.Runtime.InteropServices.DllImport("winmm.dll")>
    Private Function joyGetDevCaps(ByVal uJoyID As Integer, ByRef pjc As JOYCAPS, ByVal cbjc As Integer) As Integer
    End Function
    ' Import the joySetCapture function from winmm.dll
    <System.Runtime.InteropServices.DllImport("winmm.dll")>
    Private Function joySetCapture(ByVal hwnd As IntPtr, ByVal uJoyID As Integer, ByVal uPeriod As Integer, ByVal bChanged As Boolean) As Integer
    End Function
    ' Import the joyReleaseCapture function from winmm.dll
    <System.Runtime.InteropServices.DllImport("winmm.dll")>
    Private Function joyReleaseCapture(ByVal uJoyID As Integer) As Integer
    End Function


    'Returns a list of joystick names
    Public Function GetJoystickNames() As List(Of String)
        Dim numJoysticks As Integer = joyGetNumDevs()
        Dim joystickNames As New List(Of String)

        For i As Integer = 0 To numJoysticks - 1
            Dim jc As JOYCAPS
            If joyGetDevCaps(i, jc, System.Runtime.InteropServices.Marshal.SizeOf(jc)) = 0 Then
                joystickNames.Add(jc.szPname)
            End If
        Next

        Return joystickNames
    End Function

    'Gets the ID of the chosen device
    Public Function GetDeviceId(ByVal joystickName As String) As Integer
        Dim deviceCount As Integer = joyGetNumDevs()
        Dim caps As JOYCAPS
        For i As Integer = 0 To deviceCount - 1
            If joyGetDevCaps(i, caps, System.Runtime.InteropServices.Marshal.SizeOf(caps)) = 0 Then
                If caps.szPname.ToLower().Contains(joystickName.ToLower()) Then
                    Return i
                End If
            End If
        Next
        Return -1 ' Device not found
    End Function

    'Sets the active device to be used
    Public Function SetActiveDevice(ByVal joystickName As String) As Boolean
        Dim deviceId As Integer = GetDeviceId(joystickName)
        If deviceId >= 0 Then
            Dim result As Integer = joySetCapture(Form1.Handle, deviceId, 0, False)
            Return (result = 0)
        Else
            Return False ' Device not found
        End If
    End Function

    'Releases the active device
    Public Function ReleaseJoystickCapture(ByVal joystickID As Integer) As Boolean
        Dim result As Integer = joyReleaseCapture(joystickID)
        If result = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    'Return information associated with selected joystick
    Public Function GetJoystickState(ByVal joystickID As Integer) As String
        Dim info As New JOYINFOEX()
        info.dwSize = System.Runtime.InteropServices.Marshal.SizeOf(info)
        info.dwFlags = &HFF
        Dim result As Integer = joyGetPosEx(joystickID, info)
        If result = 0 Then
            Return String.Format("Joystick State: Left Stick X-Axis={0}, Left Stick Y-Axis={1}, Right Stick X-Axis={3}, Right Stick Z-Axis={4}, Trigger-Axis={2}, V={5}, Buttons={6}", info.dwXpos, info.dwYpos, info.dwZpos, info.dwRpos, info.dwUpos, info.dwVpos, Convert.ToString(info.dwButtons, 2).PadLeft(16, "0"c))
        Else
            Return "Failed to get joystick state, Joystick {" + joystickID.ToString + "}"
        End If
    End Function

    ' Constants for joystick axis thresholds
    Public Const JOY_THRESHOLD_MIN As Integer = 0
    Public Const JOY_THRESHOLD_MAX As Integer = 65535
    ' Set the threshold for the given joystick ID
    Public Function SetJoystickThreshold(ByVal joystickID As Integer, ByVal threshold As Integer) As Boolean
        Dim result As Integer = joySetThreshold(joystickID, threshold)

        If result = 0 Then ' Success
            Return True
        Else ' Error
            Return False
        End If
    End Function
    ' Get the threshold for the given joystick ID
    Public Function GetJoystickThreshold(ByVal joystickID As Integer) As Integer
        Dim threshold As Integer = 0
        Dim result As Integer = joyGetThreshold(joystickID, threshold)

        If result = 0 Then ' Success
            Return threshold
        Else ' Error
            Return -1 ' Return a negative value to indicate an error occurred
        End If
    End Function

    ' Define a delegate type for the joyConfigChanged function
    Private Delegate Sub joyConfigChangedDelegate(ByVal dwFlags As Integer)


    ' Register a delegate for the joyConfigChanged function
    Public Sub RegisterJoyConfigChangedEvent(ByVal handler As Action(Of Integer))
        Dim joyConfigChangedDelegate As joyConfigChangedDelegate = AddressOf joyConfigChanged
        Dim joyConfigChangedHandler As IntPtr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(joyConfigChangedDelegate)
        Dim result As Integer = joyConfigChangedCallback(joyConfigChangedHandler, 0)

        If result <> 0 Then
            Throw New Exception("Failed to register joyConfigChanged event handler.")
        End If
    End Sub


    'STRUCTURES For Joystick Information
    Private Structure JOYCAPS
        Public wMid As Short
        Public wPid As Short
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=32)>
        Public szPname As String
        Public wXmin As Integer
        Public wXmax As Integer
        Public wYmin As Integer
        Public wYmax As Integer
        Public wZmin As Integer
        Public wZmax As Integer
        Public wNumButtons As Integer
        Public wPeriodMin As Integer
        Public wPeriodMax As Integer
        Public wRmin As Integer
        Public wRmax As Integer
        Public wUmin As Integer
        Public wUmax As Integer
        Public wVmin As Integer
        Public wVmax As Integer
        Public wCaps As Integer
        Public wMaxAxes As Integer
        Public wNumAxes As Integer
        Public wMaxButtons As Integer
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=32)>
        Public szRegKey As String
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=260)>
        Public szOEMVxD As String
    End Structure

    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)>
    Public Structure JOYINFOEX
        Public dwSize As Integer
        Public dwFlags As Integer
        Public dwXpos As Integer
        Public dwYpos As Integer
        Public dwZpos As Integer
        Public dwRpos As Integer
        Public dwUpos As Integer
        Public dwVpos As Integer
        Public dwButtons As Integer
        Public dwButtonNumber As Integer
        Public dwPOV As Integer
        Public dwReserved1 As Integer
        Public dwReserved2 As Integer
    End Structure
End Module