using SharpDX.XInput;


Console.WriteLine("Start XGamepadApp");
// Initialize XInput
var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

// Get 1st controller available
Controller controller = null;
foreach (var selectControler in controllers)
{
    if (selectControler.IsConnected)
    {
        controller = selectControler;
        break;
    }
}

if (controller == null)
{
    Console.WriteLine("No XInput controller installed");
}
else
{

    Console.WriteLine("Found a XInput controller available");
    Console.WriteLine("Press buttons on the controller to display events or escape key to exit... ");

    // Poll events from joystick
    var previousState = controller.GetState();
    while (controller.IsConnected)
    {
        if (IsKeyPressed(ConsoleKey.Escape))
        {
            break;
        }
        var state = controller.GetState();
        if (previousState.PacketNumber != state.PacketNumber)
            Console.WriteLine(state.Gamepad);
        Thread.Sleep(10);
        previousState = state;
    }
}
Console.WriteLine("End XGamepadApp");
/// <summary>
/// Determines whether the specified key is pressed.
/// </summary>
/// <param name="key">The key.</param>
/// <returns>
///   <c>true</c> if the specified key is pressed; otherwise, <c>false</c>.
/// </returns>
static bool IsKeyPressed(ConsoleKey key)
{
    return Console.KeyAvailable && Console.ReadKey(true).Key == key;
}
